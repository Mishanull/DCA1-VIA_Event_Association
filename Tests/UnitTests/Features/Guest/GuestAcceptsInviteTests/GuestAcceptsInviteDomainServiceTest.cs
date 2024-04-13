using Moq;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.Services.Guest;
using VIAEventsAssociation.Core.Tools.Enumeration;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestAcceptsInviteTests;

public class GuestAcceptsInviteDomainServiceTest
{
    private readonly Mock<IGuestRepository> _guestRepoMock;
    private readonly Mock<IVeaEventRepository> _eventRepoMock;
    private readonly Mock<ICreatorRepository> _creatorRepoMock;
    private readonly Mock<IEmailCheck> _mockEmailCheck;
    private readonly Email _defaultEmail;
    private readonly GuestAcceptsInvite uut;
    public GuestAcceptsInviteDomainServiceTest()
    {
        _guestRepoMock = new Mock<IGuestRepository>();
        _eventRepoMock = new Mock<IVeaEventRepository>();
        _creatorRepoMock = new Mock<ICreatorRepository>();
        _mockEmailCheck = new Mock<IEmailCheck>();
        _defaultEmail = Email.Create("creator@example.com", _mockEmailCheck.Object).Value!;
        uut = new GuestAcceptsInvite(
                _guestRepoMock.Object,
                _creatorRepoMock.Object,
                _eventRepoMock.Object);
    }
    private void RepoMockSetup(VeaGuest guest, VeaEvent veaEvent, Creator creator, Invite invite)
    {
        _guestRepoMock.Setup(repo => repo.FindAsync(guest.Id)).ReturnsAsync(new Result<VeaGuest>(guest));
        _eventRepoMock.Setup(repo => repo.FindAsync(veaEvent.Id)).ReturnsAsync(new Result<VeaEvent>(veaEvent));
        _creatorRepoMock.Setup(repo => repo.FindAsync(creator.Id)).ReturnsAsync(new Result<Creator>(creator));
        _creatorRepoMock.Setup(repo => repo.FindInviteAsync(It.IsAny<InviteId>())).ReturnsAsync(new Result<Invite>(invite));
        _mockEmailCheck.Setup(service => service.DoesEmailExist(_defaultEmail.Value)).Returns(true);
    }

    [Fact]
    public async Task S1_AcceptsPendingInvite_ShouldAddParticipant()
    {
        // Arrange
        var guest = new VeaGuest(new GuestId());
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Active };
        var creator = Creator.Create(_defaultEmail).Value!;
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value!;

        RepoMockSetup(guest, veaEvent, creator, invite);

        // Act
        var result = await uut.Handle(invite.Id);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(InviteStatus.Accepted, invite.InviteStatus);
    }

    [Fact]
    public async Task F1_NoInvitationForGuest_ShouldReturnError()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Active };
        var creator = Creator.Create(_defaultEmail).Value!;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value!;
        var inviteId = invite.Id;
        var findInviteResult = new Result<Invite>(invite);
        findInviteResult.CollectError(ErrorHelper.CreateVeaError("Invite not found.", ErrorType.ResourceNotFound));
        RepoMockSetup(guest, veaEvent, creator, invite);
        _creatorRepoMock.Setup(r => r.FindInviteAsync(It.IsAny<InviteId>())).ReturnsAsync(findInviteResult);
        
        // Act
        var result = await uut.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Invite not found."));
    }

    [Fact]
    public async Task F2_InvitationAcceptedForFullEvent_ShouldReturnError()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Active };
        var creator = Creator.Create(_defaultEmail).Value!;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value!;
        var inviteId = invite.Id;
        RepoMockSetup(guest, veaEvent, creator, invite);
        veaEvent.MaxGuests = MaxGuests.Create(0).Value!; // Assuming this sets the event as full

        // Act
        var result = await uut.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Event is full, cannot invite more people."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    public async Task F3_InvitationAcceptedForCancelledEvent_ShouldReturnError(int statusCode)
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = Enumeration.FromValue<VeaEventStatus>(statusCode)! };
        var creator = Creator.Create(_defaultEmail).Value!;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value!;
        var inviteId = invite.Id;
        RepoMockSetup(guest, veaEvent, creator, invite);

        // Act
        var result = await uut.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("An invite can only be accepted for an active event."));
    }

    [Fact]
    public async Task F4_InviteToReadyEvent_ShouldReturnError()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Ready };
        var creator = Creator.Create(_defaultEmail).Value!;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value!;
        var inviteId = invite.Id;
        RepoMockSetup(guest, veaEvent, creator, invite);

        // Act
        var result = await uut.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("This event cannot be joined yet."));
    }
}