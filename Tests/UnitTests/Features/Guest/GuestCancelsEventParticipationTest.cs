using Moq;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Core.Domain.Services;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest;

public class GuestCancelsEventParticipationTest
{
    private readonly Mock<IGuestRepository> _guestRepoMock;
    private readonly Mock<IRequestRepository> _requestRepoMock;
    private readonly Mock<IVeaEventRepository> _eventRepoMock;
    private static readonly FromTo ValidFromTo = FromTo.Create(DateTime.Now.AddDays(6), DateTime.Now.AddDays(8)).Value;
    private readonly GuestCancelsEventParticipation service;
    public GuestCancelsEventParticipationTest()
    {
        _guestRepoMock = new Mock<IGuestRepository>();
        _requestRepoMock = new Mock<IRequestRepository>();
        _eventRepoMock = new Mock<IVeaEventRepository>();
        service = new GuestCancelsEventParticipation(
            _guestRepoMock.Object,
            _eventRepoMock.Object,
            _requestRepoMock.Object);
    }

    [Fact]
    public void S1_GuestCancelParticipation_SuccessfulCancellation_ShouldRemoveParticipation()
    
    {
        //Arrange
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            VeaEventStatus = VeaEventStatus.Active,
            FromTo = ValidFromTo,
        };
        var request = Request.Create("reason", eventId, guestId).Value;
        veaEvent.AddParticipant(guestId);
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);
        _requestRepoMock.Setup(repo => repo.Find(request.Id)).Returns(new Result<Request>(request));

        // Act
        var result = service.Handle(request.Id);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(request.RequestStatus, RequestStatus.Cancelled);
    }

    [Fact]
    public void S2_GuestCancelParticipation_GuestNotParticipating_ShouldChangeNothing()
    {
        //Arrange
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            VeaEventStatus = VeaEventStatus.Active,
            FromTo = ValidFromTo,
        };
        var request = Request.Create("reason", eventId, guestId).Value;

        RepoMockSetup(guestId, guest, eventId, veaEvent, request);


        // Act
        var result = service.Handle(request.Id);

        // Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    public void F1_GuestCancelParticipation_EventHasEnded_ShouldReturnError()
    {
        // Arrange 
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            VeaEventStatus = VeaEventStatus.Active,
            FromTo = FromTo.Create(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)).Value,
        };
        veaEvent.AddParticipant(guestId);

        var request = Request.Create("reason", eventId, guestId).Value;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = service.Handle(request.Id);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.EventHasEnded));
    }

    private void RepoMockSetup(GuestId guestId, VeaGuest guest, VeaEventId eventId, VeaEvent veaEvent, Request request)
    {
        _guestRepoMock.Setup(repo => repo.Find(guestId)).Returns(new Result<VeaGuest>(guest));
        _eventRepoMock.Setup(repo => repo.Find(eventId)).Returns(new Result<VeaEvent>(veaEvent));
        _requestRepoMock.Setup(repo => repo.Find(request.Id)).Returns(new Result<Request>(request));
    }
}