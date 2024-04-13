using Moq;
using UnitTests.FakeServices.Repositories;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.Contracts.UnitOfWork;
using ViaEventAssociation.Core.Domain.GuestAgg;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Commands.Guest;
using ViaEventsAssociation.Core.Application.CommandHandler.Common.Base;
using ViaEventsAssociation.Core.Application.CommandHandler.Handlers.GuestHandlers;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.CreateGuestTests;

public class CreateGuestHandlerTest
{
    private IGuestRepository _guestRepository;
    private ICommandHandler<CreateGuestCommand> handler;
    private CreateGuestCommand _validCommand;
    public CreateGuestHandlerTest()
    {
        CreateValidCommand();
    }

    [Fact]
    public async Task CreateGuest_AllValuesValid_Success()
    {
        //Arrange
        SetupSuccess();

        //Act
        var result = await handler.HandleAsync(_validCommand);

        //Assert
        Assert.False(result.IsErrorResult());
    }

    [Fact]
    public async Task CreateGuest_RepoError_Failure()
    {
        //Arrange
        SetupFailure();

        //Act
        var result = await handler.HandleAsync(_validCommand);

        //Assert
        Assert.True(result.IsErrorResult());
    }

    private void SetupFailure()
    {

        var repoMock = new Mock<IGuestRepository>();
        var repoResult = new Result();
        repoResult.CollectError(ErrorHelper.CreateVeaError("Error", ErrorType.Unknown));
        repoMock.Setup(r => r.AddAsync(It.IsAny<VeaGuest>())).ReturnsAsync(repoResult);
        handler = new CreateGuestHandler(repoMock.Object);
    }
    private void SetupSuccess()
    {
        _guestRepository = new FakeGuestRepository();
        handler = new CreateGuestHandler(_guestRepository);
    }

    private void CreateValidCommand()
    {
        string email = "john@via.dk";
        string firstName = "John";
        string lastName = "Doe";
        string pictureUrl = "http://example.com/profile.jpg";
        var emailCheckMock = new Mock<IEmailCheck>();
        emailCheckMock.Setup(m => m.DoesEmailExist(It.IsAny<string>())).Returns(false);
        _validCommand = CreateGuestCommand.Create(email, firstName, lastName, pictureUrl, emailCheckMock.Object).Value!;
    }
}