using Moq;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.Contracts.Repositories;
using ViaEventAssociation.Core.Domain.CreatorAgg;
using ViaEventAssociation.Core.Domain.CreatorAgg.InviteEntity;
using ViaEventAssociation.Core.Domain.EventAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventAssociation.Core.Domain.Services;
using VIAEventsAssociation.Core.Tools.Enumeration;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.GuestIsInvitedTests;

public class GuestIsInvitedToEventDomainServiceTest
{
    private readonly Mock<IGuestRepository> _guestRepoMock;
    private readonly Mock<IVeaEventRepository> _eventRepoMock;
    private readonly Mock<ICreatorRepository> _creatorRepoMock;
    private readonly Mock<IInviteRepository> _inviteRepoMock;
    private readonly Mock<IEmailCheck> _mockEmailCheck;
    private readonly Email _defaultEmail;
    private readonly GuestIsInvitedToEvent _service;

    public GuestIsInvitedToEventDomainServiceTest()
    {
        _guestRepoMock = new Mock<IGuestRepository>();
        _eventRepoMock = new Mock<IVeaEventRepository>();
        _inviteRepoMock = new Mock<IInviteRepository>();
        _creatorRepoMock = new Mock<ICreatorRepository>();
        _mockEmailCheck = new Mock<IEmailCheck>();
        _defaultEmail = Email.Create("creator@example.com", _mockEmailCheck.Object).Value;
        _service = new GuestIsInvitedToEvent(
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
    public async Task S1_InviteGuestToReadyOrActiveEvent_Success()
    {
        // Arrange
        var guest = new VeaGuest(new GuestId());
        var veaEvent = new VeaEvent(new VeaEventId())
        {
            VeaEventStatus = VeaEventStatus.Ready
        };
        var creator = Creator.Create(_defaultEmail).Value;
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;

        RepoMockSetup(guest, veaEvent, invite, creator);

        // Act
        var result = await _service.Handle(invite);

        // Assert
        Assert.False(result.IsErrorResult());
    }

    [Theory]
    [InlineData(1)] // Draft
    [InlineData(4)] // Cancelled
    public async Task F1_InviteGuestToInactiveEvent_Failure(int status)
    {
        // Arrange
        var guest = new VeaGuest(new GuestId());
        var veaEvent = new VeaEvent(new VeaEventId())
        {
            VeaEventStatus = Enumeration.FromValue<VeaEventStatus>(status)
        };
        var creator = Creator.Create(_defaultEmail).Value;
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;
        RepoMockSetup(guest, veaEvent, invite, creator);

        // Act
        var result = await _service.Handle(invite);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Type == ErrorType.EventNotActive);
    }

    [Fact]
    public async Task F2_InviteGuestToFullEvent_Failure()
    {
        // Arrange
        var guest = new VeaGuest(new GuestId());
        var veaEvent = new VeaEvent(new VeaEventId())
        {
            VeaEventStatus = VeaEventStatus.Active
        };
        var creator = Creator.Create(_defaultEmail).Value;
        var invite = Invite.Create(guest.Id, creator.Id, veaEvent.Id).Value;

        veaEvent.MaxGuests = MaxGuests.Create(0).Value;
        RepoMockSetup(guest, veaEvent, invite, creator);

        // Act
        var result = await _service.Handle(invite);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => Equals(e.Type, ErrorType.EventIsFull));
    }
}