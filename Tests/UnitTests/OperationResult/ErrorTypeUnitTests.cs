using VIAEventsAssociation.Core.Tools.Enumeration;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.OperationResult;

public class ErrorTypeUnitTests
{
    [Fact]
    public void ErrorType_GetAll_ShouldReturnAllInstances()
    {
        var allInstances = Enumeration.GetAll<ErrorType>().ToList();
        Assert.Equal(4, allInstances.Count);
        Assert.Contains(ErrorType.UnknownType, allInstances);
        Assert.Contains(ErrorType.ValidationFailedType, allInstances);
        Assert.Contains(ErrorType.ResourceNotFoundType, allInstances);
        Assert.Contains(ErrorType.UnauthorizedType, allInstances);
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
        var firstInstance = ErrorType.UnknownType;
        var secondInstance = ErrorType.UnauthorizedType;

        var difference = Enumeration.AbsoluteDifference(firstInstance, secondInstance);

        Assert.Equal(102, difference);
    }
    
    [Fact]
    public void ErrorType_FromValue_ShouldReturnCorrectInstance()
    {
        var instance = Enumeration.FromValue<ErrorType>(101);

        Assert.Equal(ErrorType.ResourceNotFoundType, instance);
    }
    
    [Fact]
    public void ErrorType_FromDisplayName_ShouldReturnCorrectInstance()
    {
        var instance = Enumeration.FromDisplayName<ErrorType>("Unauthorized");

        Assert.Equal(ErrorType.UnauthorizedType, instance);
    }
    
    [Fact]
    public void ErrorType_CompareTo_ShouldReturnCorrectComparisonResult()
    {
        var firstInstance = ErrorType.UnknownType;
        var secondInstance = ErrorType.ValidationFailedType;

        var comparisonResult = firstInstance.CompareTo(secondInstance);

        Assert.True(comparisonResult < 0);
    }
}