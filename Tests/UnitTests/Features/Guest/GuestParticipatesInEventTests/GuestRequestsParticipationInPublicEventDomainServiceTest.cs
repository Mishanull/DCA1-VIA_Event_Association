using Moq;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.GuestAgg.Request;
using ViaEventAssociation.Core.Domain.GuestAgg.RequestEntity;
using ViaEventAssociation.Core.Domain.Services.Guest;
using VIAEventsAssociation.Core.Tools.Enumeration;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestParticipatesInEventTests;

public class GuestRequestsParticipationInPublicEventDomainServiceTest
{
    private readonly Mock<IGuestRepository> _guestRepoMock;
    private readonly Mock<IVeaEventRepository> _eventRepoMock;
    private static readonly string Reason = "I am interested in this.";
    private static readonly FromTo ValidFromTo = FromTo.Create(DateTime.Now.AddDays(6), DateTime.Now.AddDays(8)).Value!;
    private readonly GuestRequestParticipationPublicEvent _service;


    public GuestRequestsParticipationInPublicEventDomainServiceTest()
    {
        _guestRepoMock = new Mock<IGuestRepository>();
        _eventRepoMock = new Mock<IVeaEventRepository>();
        _service = new GuestRequestParticipationPublicEvent(
            _guestRepoMock.Object,
            _eventRepoMock.Object);
    }
    
    [Fact]
    public async Task S1_RequestParticipation_ActivePublicEvent_Success()
    {
        // Arrange
        var status = VeaEventStatus.Active;
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            VeaEventStatus = status,
            FromTo = ValidFromTo,
            VeaEventType = VeaEventType.Public
        };
        var request = Request.Create(Reason, eventId, guestId).Value!;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await _service.Handle(request);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.Empty(result.Errors);
        Assert.Equal(request.RequestStatus, RequestStatus.Approved);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    public async Task F1_GuestRequestParticipation_EventNotActive_ShouldReturnError(int statusCode)
    {
        // Arrange
        var status = Enumeration.FromValue<VeaEventStatus>(statusCode);
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            VeaEventStatus = status,
            FromTo = ValidFromTo
        };
        var request = Request.Create(Reason, eventId, guestId).Value;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await _service.Handle(request);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Type.Equals(ErrorType.EventNotActive));
    }

    private void RepoMockSetup(GuestId guestId, VeaGuest guest, VeaEventId eventId, VeaEvent veaEvent, Request request)
    {
        _guestRepoMock.Setup(repo => repo.FindAsync(guestId)).ReturnsAsync(new Result<VeaGuest>(guest));
        _eventRepoMock.Setup(repo => repo.FindAsync(eventId)).ReturnsAsync(new Result<VeaEvent>(veaEvent));
    }


    [Fact]
    public async Task F2_RequestParticipation_FullEvent_ShouldReturnError()
    {
        // Arrange
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            VeaEventStatus = VeaEventStatus.Active,
            MaxGuests = MaxGuests.Create(0).Value,
            FromTo = ValidFromTo
        };
        var request = Request.Create(Reason, eventId, guestId).Value;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await _service.Handle(request);

        // Assert
        Assert.True(result.IsErrorResult());

        Assert.Contains(result.Errors, e => e.Type.Equals(ErrorType.EventIsFull));
    }

    [Fact]
    public async Task F3_RequestParticipation_PastEvent_ShouldReturnError()
    {
        // Arrange
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            FromTo = FromTo.Create(new DateTime(2004, 12, 23), new DateTime(2004, 12, 25)).Value!,
            VeaEventStatus = VeaEventStatus.Active
        };
        var request = Request.Create(Reason, eventId, guestId).Value!;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await _service.Handle(request);

        // Assert
        Assert.True(result.IsErrorResult());

        Assert.Contains(result.Errors, e => e.Type.Equals(ErrorType.EventHasEnded));
    }

    [Fact]
    public async Task F4_RequestParticipation_PrivateEvent_ShouldReturnError()
    {
        // Arrange
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var request = Request.Create(Reason, eventId, guestId).Value!;
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId);
        veaEvent.VeaEventStatus = VeaEventStatus.Active;
        veaEvent.VeaEventType = VeaEventType.Private;
        veaEvent.FromTo = ValidFromTo;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await _service.Handle(request);

        // Assert
        Assert.True(result.IsErrorResult());

        Assert.Contains(result.Errors, e => e.Type.Equals(ErrorType.EventIsPrivate));
    }

    [Fact]
    public async Task F5_RequestParticipation_AlreadyParticipant_ShouldReturnError()
    {
        // Arrange
        var guestId = new GuestId();
        var eventId = new VeaEventId();
        var guest = new VeaGuest(guestId);
        var veaEvent = new VeaEvent(eventId)
        {
            VeaEventStatus = VeaEventStatus.Active
        };
        veaEvent.AddParticipant(guestId);
        veaEvent.FromTo = ValidFromTo;
        var request = Request.Create(Reason, eventId, guestId).Value;
        RepoMockSetup(guestId, guest, eventId, veaEvent, request);

        // Act
        var result = await _service.Handle(request);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Type.Equals(ErrorType.AlreadyAParticipantInEvent));
    }
}