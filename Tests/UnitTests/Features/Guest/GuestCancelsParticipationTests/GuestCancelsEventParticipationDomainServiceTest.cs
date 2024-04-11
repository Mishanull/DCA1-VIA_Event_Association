using Moq;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Core.Domain.Services;
using ViaEventAssociation.Core.Domain.Services.Guest;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestCancelsParticipationTests;

public class GuestCancelsEventParticipationDomainServiceTest
{
    private readonly Mock<IGuestRepository> _guestRepoMock;
    private readonly Mock<IVeaEventRepository> _eventRepoMock;
    private static readonly FromTo ValidFromTo = FromTo.Create(DateTime.Now.AddDays(6), DateTime.Now.AddDays(8)).Value!;
    private readonly GuestCancelsEventParticipation service;
    public GuestCancelsEventParticipationDomainServiceTest()
    {
        _guestRepoMock = new Mock<IGuestRepository>();
        _eventRepoMock = new Mock<IVeaEventRepository>();
        service = new GuestCancelsEventParticipation(
                _guestRepoMock.Object,
                _eventRepoMock.Object
            );
    }

    [Fact]
    public async Task S1_GuestCancelParticipation_SuccessfulCancellation_ShouldRemoveParticipation()
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
        var request = Request.Create("I am interested in this.", eventId, guestId).Value;
        veaEvent.AddParticipant(guestId);
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await service.Handle(request.Id);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(request.RequestStatus, RequestStatus.Cancelled);
    }

    [Fact]
    public async Task S2_GuestCancelParticipation_GuestNotParticipating_ShouldChangeNothing()
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
        var request = Request.Create("I am interested in this.", eventId, guestId).Value;

        RepoMockSetup(guestId, guest, eventId, veaEvent, request);


        // Act
        var result = await service.Handle(request.Id);

        // Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    public async Task F1_GuestCancelParticipation_EventHasEnded_ShouldReturnError()
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

        var request = Request.Create("I am interested in this.", eventId, guestId).Value!;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await service.Handle(request.Id);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.EventHasEnded));
    }

    private void RepoMockSetup(GuestId guestId, VeaGuest guest, VeaEventId eventId, VeaEvent veaEvent, Request request)
    {
        _guestRepoMock.Setup(repo => repo.FindAsync(guestId)).ReturnsAsync(new Result<VeaGuest>(guest));
        _guestRepoMock.Setup(repo => repo.FindRequestAsync(request.Id)).ReturnsAsync(new Result<Request>(request));
        _eventRepoMock.Setup(repo => repo.FindAsync(eventId)).ReturnsAsync(new Result<VeaEvent>(veaEvent));
    }
}