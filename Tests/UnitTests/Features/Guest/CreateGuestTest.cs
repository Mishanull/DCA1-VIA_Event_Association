using Moq;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Domain.GuestAgg.Guest;

namespace UnitTests.Features.Guest;
public class CreateGuestTest
{

    [Fact]
    public void S1_CreateVeaGuest_WithValidData_ShouldCreateViaGuest()
    {
        // Arrange
        var emailValue = "test@via.dk";
        var firstNameValue = "John";
        var lastNameValue = "Doe";
        var pictureUrlValue = "http://example.com/image.jpg";
        var mockEmailCheck = new Mock<IEmailCheck>();
        mockEmailCheck.Setup(service => service.DoesEmailExist(emailValue)).Returns(false);
        var email = Email.Create(emailValue, mockEmailCheck.Object).Value;
        var firstName = FirstName.Create(firstNameValue).Value; 
        var lastName = LastName.Create(lastNameValue).Value; 
        var pictureUrl = PictureUrl.Create(pictureUrlValue).Value; 

        // Act
        var result = VeaGuest.Create(email, firstName, lastName, pictureUrl);

        // Assert
        Assert.False(result.IsErrorResult());
        Assert.Equal(emailValue.ToLower(), result.Value.Email?.Value); 
        Assert.Equal(firstNameValue, result.Value.FirstName?.Value); 
        Assert.Equal(lastNameValue, result.Value.LastName?.Value); 
        Assert.Equal(pictureUrlValue, result.Value.PictureUrl?.Value); 
    }
    [Fact]
    public void F1_CreateVeaGuest_EmailDoesNotEndWithViaDk_ShouldReturnError()
    {
        // Arrange
        var email = "test@example.com"; 
        var mockEmailCheck = new Mock<IEmailCheck>();
        mockEmailCheck.Setup(service => service.DoesEmailExist(email)).Returns(false);
        
        // Act
        var result = Email.Create(email, mockEmailCheck.Object);
            
        
        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Email must end with '@via.dk'"));
    }

    [Fact]
    public void F2_CreateVeaGuest_EmailNotInCorrectFormat_ShouldReturnError()
    {
        // Arrange
        var email = "testatviadk"; 
        var mockEmailCheck = new Mock<IEmailCheck>();
        mockEmailCheck.Setup(service => service.DoesEmailExist(email)).Returns(false);
        
        // Act
        var result = Email.Create(email, mockEmailCheck.Object);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Email is not in a valid format"));
    }

    [Fact]
    public void F3_CreateVeaGuest_FirstNameInvalidShort_ShouldReturnError()
    {
        // Arrange
        var firstName = "J"; 
        
        // Act
        var result = FirstName.Create(firstName);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("First name value should be between 2 and 25 characters."));
    }
    
    [Fact]
    public void  F4_CreateVeaGuest_FirstNameInvalidLong_ShouldReturnError()
    {
        // Arrange
        var firstName = "karakraskrakksadsakdsadksadaksdkasdaskdsadsamcxvcxvcx";
        
        // Act
        var result = FirstName.Create(firstName);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("First name value should be between 2 and 25 characters."));
    }

    [Fact]
    public void  F5_CreateVeaGuest_LastNameInvalidShort_ShouldReturnError()
    {
        // Arrange
        var lastName = "J"; 
        
        // Act
        var result = LastName.Create(lastName);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Last name value should be between 2 and 25 characters."));
    }
    
    [Fact]
    public void F6_CreateVeaGuest_LastNameInvalidLong_ShouldReturnError()
    {
        // Arrange
        var lastName = "karakraskrakksadsakdsadksadaksdkasdaskdsadsamcxvcxvcx";
        
        // Act
        var result = LastName.Create(lastName);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Last name value should be between 2 and 25 characters."));
    }
    
    [Fact]
    public void F7_CreateVeaGuest_EmailAlreadyRegistered_ShouldReturnError()
    {
        // Arrange
        var emailValue = "existing@via.dk";
        var mockEmailCheck = new Mock<IEmailCheck>();
        mockEmailCheck.Setup(service => service.DoesEmailExist(emailValue)).Returns(true);

        // Act
        var result = Email.Create(emailValue, mockEmailCheck.Object);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Contains(result.Errors, e => e.Message.Message.Contains("Email already registered"));
    }

    [Fact]
    public void F8_CreateVeaGuest_NameContainsNumbers_ShouldReturnError()
    {
        // Arrange
        var firstName = "John3";
        var lastName = "Mark2";

        // Act
        var resultFirstName = FirstName.Create(firstName);
        var resultLastName = LastName.Create(lastName);
        
        // Assert
        Assert.True(resultFirstName.IsErrorResult());
        Assert.True(resultLastName.IsErrorResult());
        Assert.Contains(resultFirstName.Errors, e => e.Message.Message.Contains("The first name must only contain letters."));
        Assert.Contains(resultLastName.Errors, e => e.Message.Message.Contains("The last name must only contain letters."));
    }

    [Fact]
    public void F9_CreateVeaGuest_NameContainsSymbols_ShouldReturnError()
    {
        // Arrange
        var lastName = "Do@#" ;
        var firstName = "ds@#";
        
        // Act
        var resultLastName = LastName.Create(lastName);
        var resultFirstName = FirstName.Create(firstName);
        
        // Assert
        Assert.True(resultLastName.IsErrorResult());
        Assert.True(resultFirstName.IsErrorResult());
        Assert.Contains(resultLastName.Errors, e => e.Message.Message.Contains("The last name must only contain letters"));
        Assert.Contains(resultFirstName.Errors, e => e.Message.Message.Contains("The first name must only contain letters"));
    }
}