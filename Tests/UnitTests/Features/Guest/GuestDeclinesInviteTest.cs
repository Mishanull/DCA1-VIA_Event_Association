using ViaEventAssociation.Core.Domain.Contracts.Repositories;

namespace UnitTests.Features.Guest;

using Moq;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.Services;
using VIAEventsAssociation.Core.Tools.Enumeration;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

public class GuestDeclinesInviteTest
{
    private readonly Mock<IGuestRepository> _guestRepoMock;
    private readonly Mock<IVeaEventRepository> _eventRepoMock;
    private readonly Mock<ICreatorRepository> _creatorRepoMock;
    private readonly Mock<IInviteRepository> _inviteRepoMock;
    private readonly Mock<IEmailCheck> _mockEmailCheck;
    private readonly Email _defaultEmail;
    private readonly GuestDeclinesInvite service;

    public GuestDeclinesInviteTest()
    {
        _guestRepoMock = new Mock<IGuestRepository>();
        _eventRepoMock = new Mock<IVeaEventRepository>();
        _inviteRepoMock = new Mock<IInviteRepository>();
        _creatorRepoMock = new Mock<ICreatorRepository>();
        _mockEmailCheck = new Mock<IEmailCheck>();
        _defaultEmail = Email.Create("creator@example.com", _mockEmailCheck.Object).Value;
        service = new GuestDeclinesInvite(
            _guestRepoMock.Object,
            _creatorRepoMock.Object,
            _eventRepoMock.Object,
            _inviteRepoMock.Object);
    }
    private void RepoMockSetup(VeaGuest guest, VeaEvent veaEvent, Invite invite, Creator creator)
    {
        _guestRepoMock.Setup(repo => repo.Find(guest.Id)).Returns(new Result<VeaGuest>(guest));
        _eventRepoMock.Setup(repo => repo.Find(veaEvent.Id)).Returns(new Result<VeaEvent>(veaEvent));
        _inviteRepoMock.Setup(repo => repo.Find(invite.Id)).Returns(new Result<Invite>(invite));
        _creatorRepoMock.Setup(repo => repo.Find(creator.Id)).Returns(new Result<Creator>(creator));
        _mockEmailCheck.Setup(emailService => emailService.DoesEmailExist(_defaultEmail.Value)).Returns(true);

    }

    [Fact]
    public void S1_DeclinesPendingInvite_ShouldAddParticipant()
    {
        // Arrange
        var guest = new VeaGuest(new GuestId());
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Active };
       var creator = Creator.Create( _defaultEmail).Value;
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;
        RepoMockSetup(guest, veaEvent, invite, creator);
        
        // Act
        var result = service.Handle(invite.Id);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(InviteStatus.Declined, invite.InviteStatus);
    }

    [Fact]
    public void F1_NoInvitationForGuest_ShouldReturnError()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Active };
        var creator = Creator.Create( _defaultEmail).Value;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;
        var inviteId = invite.Id;
        var findInviteResult = new Result<Invite>(invite);
        findInviteResult.CollectError(ErrorHelper.CreateVeaError("Invite not found.", ErrorType.ResourceNotFound));
        RepoMockSetup(guest, veaEvent, invite, creator);
        _inviteRepoMock.Setup(r => r.Find(inviteId)).Returns(findInviteResult);


        // Act
        var result = service.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Invite not found."));
    }

    [Fact]
    public void F2_InvitationDeclinedForFullEvent_ShouldReturnError()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Active };
        var creator = Creator.Create( _defaultEmail).Value;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;
        var inviteId = invite.Id;
        RepoMockSetup(guest, veaEvent, invite, creator);
        veaEvent.MaxGuests = MaxGuests.Create(0).Value; 

        // Act
        var result = service.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Event is full, cannot invite more people."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    public void F3_InvitationDeclinedForCancelledOrDraftEvent_ShouldReturnError(int statusCode)
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = Enumeration.FromValue<VeaEventStatus>(statusCode) };
        var creator = Creator.Create( _defaultEmail).Value;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;
        var inviteId = invite.Id;
        RepoMockSetup(guest, veaEvent, invite, creator);

        // Act
        var result = service.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("An invite cannot be be declined for a cancelled or draft event."));
    }

    [Fact]
    public void F4_InviteToReadyEvent_ShouldReturnError()
    {
        // Arrange
        var veaEvent = new VeaEvent(new VeaEventId()) { VeaEventStatus = VeaEventStatus.Ready };
        var creator = Creator.Create( _defaultEmail).Value;
        var guest = new VeaGuest(new GuestId());
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;
        var inviteId = invite.Id;
        RepoMockSetup(guest, veaEvent, invite, creator);

        // Act
        var result = service.Handle(inviteId);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("This event cannot be declined yet."));
    }
}