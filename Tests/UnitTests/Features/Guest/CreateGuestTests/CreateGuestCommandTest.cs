using Moq;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventsAssociation.Core.Application.CommandDispatching.Commands.Guest;

namespace UnitTests.Features.Guest.CreateGuestTests;

public class CreateGuestCommandTest
{
   [Fact]
    public void CreateGuestCommand_Success()
    {
        // Arrange
        var emailCheckMock = new Mock<IEmailCheck>();
        emailCheckMock.Setup(m => m.DoesEmailExist(It.IsAny<string>())).Returns(false);

        string email = "john@via.dk";
        string firstName = "John";
        string lastName = "Doe";
        string pictureUrl = "http://example.com/profile.jpg";

        // Act
        var result = CreateGuestCommand.Create(email, firstName, lastName, pictureUrl, emailCheckMock.Object);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void CreateGuestCommand_Failure_InvalidEmail()
    {
        // Arrange
        var emailCheckMock = new Mock<IEmailCheck>();
        emailCheckMock.Setup(m => m.DoesEmailExist(It.IsAny<string>())).Returns(false);

        string invalidEmail = "invalid@notvia.com"; 
        string firstName = "John";
        string lastName = "Doe";
        string pictureUrl = "http://example.com/profile.jpg";

        // Act
        var result = CreateGuestCommand.Create(invalidEmail, firstName, lastName, pictureUrl, emailCheckMock.Object);

        // Assert
        Assert.True(result.IsErrorResult());
    }

    [Fact]
    public void CreateGuestCommand_Failure_InvalidFirstName()
    {
        // Arrange
        var emailCheckMock = new Mock<IEmailCheck>();
        emailCheckMock.Setup(m => m.DoesEmailExist(It.IsAny<string>())).Returns(false);

        string email = "john@via.dk";
        string invalidFirstName = "J"; // Invalid because it's too short
        string lastName = "Doe";
        string pictureUrl = "http://example.com/profile.jpg";

        // Act
        var result = CreateGuestCommand.Create(email, invalidFirstName, lastName, pictureUrl, emailCheckMock.Object);

        // Assert
        Assert.True(result.IsErrorResult());
    }

    [Fact]
    public void CreateGuestCommand_Failure_InvalidPictureUrl()
    {
        // Arrange
        var emailCheckMock = new Mock<IEmailCheck>();
        emailCheckMock.Setup(m => m.DoesEmailExist(It.IsAny<string>())).Returns(false);

        string email = "john@via.dk";
        string firstName = "John";
        string lastName = "Doe";
        string invalidPictureUrl = "http://example.com/profile"; 

        // Act
        var result = CreateGuestCommand.Create(email, firstName, lastName, invalidPictureUrl, emailCheckMock.Object);

        // Assert
        Assert.True(result.IsErrorResult());
    } 
}