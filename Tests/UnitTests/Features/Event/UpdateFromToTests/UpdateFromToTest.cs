using UnitTests.Utils;
using ViaEventAssociation.Core.Domain.Common.ValueObjects;
using ViaEventAssociation.Core.Domain.EventAgg;
using VIAEventsAssociation.Core.Tools.OperationResult.Error;

namespace UnitTests.Features.Event.UpdateFromToTests;

public class UpdateFromToTest
{
    private static readonly DateTime FakeDateTime = new(2023, 08, 24, 12, 00, 00);

    [Theory]
    [InlineData("2023-08-25T19:00:00", "2023-08-25T23:59:00")]
    [InlineData("2023-08-25T12:00:00", "2023-08-25T16:30:00")]
    [InlineData("2023-08-25T08:00:00", "2023-08-25T12:15:00")]
    [InlineData("2023-08-25T10:00:00", "2023-08-25T20:00:00")]
    [InlineData("2023-08-25T13:00:00", "2023-08-25T23:00:00")]
    public void S1_UpdateFromToOfDraftEvent_WithValidFromTo_ShouldUpdateFromTo(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .WithStatus(VeaEventStatus.Draft)
            .Build();
        var newFromTo = FromTo.Create(from, to).Value;

        // Act
        veaEvent.UpdateFromTo(newFromTo);

        // Assert
        Assert.Equal(veaEvent.FromTo, newFromTo);
    }

    [Theory]
    [InlineData("2023-08-25T19:00:00", "2023-08-26T01:00:00")] //end time edge case
    [InlineData("2023-08-25T08:00:00", "2023-08-25T16:30:00")] //start time edge case
    [InlineData("2023-08-25T08:00:00", "2023-08-25T18:00:00")] //duration edge case
    public void S2_UpdateFromToOfDraftEvent_WithValidFromTo_ShouldUpdateFromTo(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .WithStatus(VeaEventStatus.Draft)
            .Build();
        var newFromTo = FromTo.Create(from, to).Value;

        // Act
        veaEvent.UpdateFromTo(newFromTo);

        // Assert
        Assert.Equal(veaEvent.FromTo, newFromTo);
    }

    [Fact]
    public void S3_UpdateFromToOfReadyEvent_WithValidFromTo_ShouldUpdateFromToAndChangeEventStatusToDraft()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .WithFromTo(FakeDateTime.AddDays(1), FakeDateTime.AddDays(1).AddHours(4))
            .WithStatus(VeaEventStatus.Ready)
            .Build();
        var newFromTo = FromTo.Create(FakeDateTime.AddDays(2), FakeDateTime.AddDays(2).AddHours(4)); //one day later than planned

        // Act
        veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.Equal(veaEvent.FromTo, newFromTo.Value);
        Assert.Equal(veaEvent.VeaEventStatus, VeaEventStatus.Draft);
    }

    [Fact]
    public void S4_UpdateFromTo_WithValidFromToAndStartTimeInTheFuture_ShouldUpdateFromTo()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(DateTime.Now, DateTime.Now.AddHours(2)).Value!; //start time is in the future

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo);

        // Assert
        Assert.True(result.IsErrorResult());
    }

    [Fact]
    public void S5_UpdateFromTo_WithValidFromToOf10HoursDurationOrLess_ShouldUpdateFromTo()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(FakeDateTime.AddDays(1), FakeDateTime.AddDays(1).AddHours(10)); //more than 10 hours fails

        // Act
        veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.Equal(veaEvent.FromTo, newFromTo.Value);
    }

    [Theory]
    [InlineData("2023-08-26T19:00:00", "2023-08-25T01:00:00")]
    [InlineData("2023-08-26T19:00:00", "2023-08-25T23:59:00")]
    [InlineData("2023-08-27T12:00:00", "2023-08-25T16:30:00")]
    [InlineData("2023-08-01T08:00:00", "2023-07-31T12:15:00")]
    public void F1_UpdateFromTo_WithInvalidFromTo_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Theory]
    [InlineData("2023-08-26T19:00:00", "2023-08-26T14:00:00")]
    [InlineData("2023-08-26T16:00:00", "2023-08-26T00:00:00")]
    [InlineData("2023-08-26T19:00:00", "2023-08-26T18:59:00")]
    [InlineData("2023-08-26T12:00:00", "2023-08-26T10:10:00")]
    [InlineData("2023-08-26T08:00:00", "2023-08-26T00:30:00")]
    public void F2_UpdateFromTo_WithStartTimeAfterEndTime_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Theory]
    [InlineData("2023-08-26T14:00:00", "2023-08-26T14:50:00")]
    [InlineData("2023-08-26T18:00:00", "2023-08-26T18:59:00")]
    [InlineData("2023-08-26T12:00:00", "2023-08-26T12:30:00")]
    [InlineData("2023-08-26T08:00:00", "2023-08-26T08:00:00")]
    public void F3_UpdateFromTo_WithStartDateSameAsEndDateButDurationShorterThan1Hour_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Theory]
    [InlineData("2023-08-25T23:30:00", "2023-08-26T00:15:00")]
    [InlineData("2023-08-30T23:01:00", "2023-08-31T00:00:00")]
    [InlineData("2023-08-30T23:59:00", "2023-08-31T00:01:00")]
    public void F4_UpdateFromTo_WithStartDateBeforeEndDateButDurationShorterThan1Hour_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Theory]
    [InlineData("2023-08-25T07:59:00", "2023-08-25T14:50:00")]
    [InlineData("2023-08-25T07:59:00", "2023-08-25T15:00:00")]
    [InlineData("2023-08-25T01:01:00", "2023-08-25T08:30:00")]
    [InlineData("2023-08-25T05:59:00", "2023-08-25T07:59:00")]
    [InlineData("2023-08-25T00:59:00", "2023-08-25T07:59:00")]
    public void F5_UpdateFromTo_WithStartTimeBeforeEightAm_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Theory]
    [InlineData("2023-08-24T23:50:00", "2023-08-25T01:01:00")]
    [InlineData("2023-08-24T22:00:00", "2023-08-25T07:59:00")]
    [InlineData("2023-08-30T23:00:00", "2023-08-31T02:30:00")]
    public void F6_UpdateFromTo_WithStartTimeBefore1amAndEndTimeAfter1am_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Fact]
    public void F7_UpdateFromToOfActiveEvent_WithValidFromTo_ShouldThrowActionNotAllowedError()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Active)
            .Build();
        var newFromTo = FromTo.Create(FakeDateTime.AddDays(1), FakeDateTime.AddDays(1).AddHours(4));

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.ActionNotAllowed);
    }

    [Fact]
    public void F8_UpdateFromToOfCancelledEvent_WithValidFromTo_ShouldThrowActionNotAllowedError()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithStatus(VeaEventStatus.Cancelled)
            .Build();
        var newFromTo = FromTo.Create(FakeDateTime.AddDays(1), FakeDateTime.AddDays(1).AddHours(4));

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.ActionNotAllowed);
    }

    [Theory]
    [InlineData("2023-08-30T08:00:00", "2023-08-30T18:01:00")]
    [InlineData("2023-08-30T14:59:00", "2023-08-31T01:00:00")]
    [InlineData("2023-08-30T14:00:00", "2023-08-31T00:01:00")]
    [InlineData("2023-08-30T14:00:00", "2023-08-31T18:30:00")]
    public void F9_UpdateFromTo_WithDurationLongerThanTenHours_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Fact]
    public void F10_UpdateFromTo_WithStartTimeInThePast_ShouldThrowInvalidFromToError()
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(FakeDateTime.AddDays(-1), FakeDateTime.AddDays(-1).AddHours(4));

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

    [Theory]
    [InlineData("2023-08-31T00:30:00", "2023-08-31T08:30:00")]
    [InlineData("2023-08-30T23:59:00", "2023-08-31T08:01:00")]
    [InlineData("2023-08-31T01:00:00", "2023-08-31T08:00:00")]
    public void F11_UpdateFromTo_WithFromToSpanningBetween1amAnd8am_ShouldThrowInvalidFromToError(DateTime from, DateTime to)
    {
        // Arrange
        var veaEvent = new VeaEventBuilder().Init()
            .WithTime(FakeDateTime)
            .Build();
        var newFromTo = FromTo.Create(from, to);

        // Act
        var result = veaEvent.UpdateFromTo(newFromTo.Value);

        // Assert
        Assert.True(result.IsErrorResult());
        Assert.Equal(result.Errors.First().Type, ErrorType.InvalidFromTo);
    }

}