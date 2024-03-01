using VIAEventsAssociation.Core.Tools.OperationResult.Error;
using VIAEventsAssociation.Core.Tools.OperationResult.Helpers;
using VIAEventsAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.OperationResult;

public class ResultHelperUnitTests
{ 
    [Fact]
    public void CreateSuccess_WithGenericValue_ShouldCreateSuccessResult()
    {
        var value = 42;
        var result = ResultHelper.CreateSuccess(value);
        Assert.Equal(value, result.Value);
    }
    
    [Fact]
    public void CreateSuccess_WithoutGenericValue_ShouldCreateSuccessResult()
    {
        var result = ResultHelper.CreateSuccess();
        Assert.NotNull(result);
        Assert.IsType<Result>(result);
    }
    
    [Fact]
    public void CreateEmptyErrorResult_ShouldCreateErrorResult()
    {
        var errorResult = ResultHelper.CreateEmptyErrorResult();
        Assert.NotNull(errorResult);
        Assert.IsType<ErrorResult>(errorResult);
    }
    
    [Fact]
    public void CreateErrorResult_WithErrors_ShouldCreateErrorResultWithErrors()
    {
        var veaError1 = new VeaError(ErrorType.UnknownType, new ErrorMessage("Error 1"));
        var veaError2 = new VeaError(ErrorType.ValidationFailedType, new ErrorMessage("Error 2"));

        var errors = new HashSet<VeaError>{veaError1, veaError2};
        var errorResult = ResultHelper.CreateErrorResult(errors);
        Assert.NotNull(errorResult);
        Assert.True(errorResult.Value.TryGetValue(veaError1, out var result1));
        Assert.Equal(result1.Type, veaError1.Type);
        Assert.Equal(result1.Message, veaError1.Message);
        Assert.True(errorResult.Value.TryGetValue(veaError2, out var result2));
        Assert.Equal(result2.Type, veaError2.Type);
        Assert.Equal(result2.Message, veaError2.Message);
    }
    
    [Fact]
    public void CreateErrorResultWithSingleError_ShouldCreateErrorResultWithSingleError()
    {
        var veaError = new VeaError(ErrorType.UnknownType, new ErrorMessage("Single Error"));
        var errorResult = ResultHelper.CreateErrorResultWithSingleError(veaError.Type, veaError.Message);
        Assert.NotNull(errorResult);
        Assert.True(errorResult.Value.TryGetValue(veaError, out var result));
        Assert.Equal(result.Type, veaError.Type);
        Assert.Equal(result.Message, veaError.Message);
    }
}