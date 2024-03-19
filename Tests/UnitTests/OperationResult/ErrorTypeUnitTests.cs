using VIAEventsAssociation.Core.Tools.Enumeration;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.OperationResult;

public class ErrorTypeUnitTests
{
    [Fact]
    public void ErrorType_GetAll_ShouldReturnAllInstances()
    {
        var allInstances = Enumeration.GetAll<ErrorType>().ToList();
        Assert.Contains(ErrorType.Unknown, allInstances);
        Assert.Contains(ErrorType.ValidationFailed, allInstances);
        Assert.Contains(ErrorType.ResourceNotFound, allInstances);
        Assert.Contains(ErrorType.Unauthorized, allInstances);
    }
    
    [Fact]
    public void  ErrorType_Equals_EqualInstance_ShouldReturnTrue()
    {
        var instance1 = Enumeration.FromValue<ErrorType>(101);
        var instance2 = Enumeration.FromValue<ErrorType>(101);
        Assert.True(instance1.Equals(instance2));
    }
    
    [Fact]
    public void  ErrorType_Equals_DifferentInstance_ShouldReturnFalse()
    {
        var instance1 = Enumeration.FromValue<ErrorType>(101);
        var instance2 = Enumeration.FromValue<ErrorType>(102);
        Assert.False(instance1.Equals(instance2));
    }
    
    [Fact]
    public void ErrorType_AbsoluteDifference_ShouldReturnCorrectDifference()
    {
        var firstInstance = ErrorType.Unknown;
        var secondInstance = ErrorType.Unauthorized;

        var difference = Enumeration.AbsoluteDifference(firstInstance, secondInstance);

        Assert.Equal(102, difference);
    }
    
    [Fact]
    public void ErrorType_FromValue_ShouldReturnCorrectInstance()
    {
        var instance = Enumeration.FromValue<ErrorType>(101);

        Assert.Equal(ErrorType.ResourceNotFound, instance);
    }
    
    [Fact]
    public void ErrorType_FromDisplayName_ShouldReturnCorrectInstance()
    {
        var instance = Enumeration.FromDisplayName<ErrorType>("Unauthorized");

        Assert.Equal(ErrorType.Unauthorized, instance);
    }
    
    [Fact]
    public void ErrorType_CompareTo_ShouldReturnCorrectComparisonResult()
    {
        var firstInstance = ErrorType.Unknown;
        var secondInstance = ErrorType.ValidationFailed;

        var comparisonResult = firstInstance.CompareTo(secondInstance);

        Assert.True(comparisonResult < 0);
    }
}