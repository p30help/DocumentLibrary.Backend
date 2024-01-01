using TariffComparison.Domain.Common.Exceptions;
using TariffComparison.Domain.ValueObjects;

namespace TariffComparison.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Money_CreateWithNegativeAmount_ShouldThrowException()
    {
        // arrange
        decimal negativeAmount = -100;

        // act
        Action act = () => new Money(negativeAmount);

        // assert
        var exception = Assert.Throws<InvalidValueObjectStateException>(act);
        Assert.Equal("The amount cannot be negative", exception.Message);
    }

    [Fact]
    public void Money_AdditionOperator_ShouldReturnNewInstanceWithSumAmount()
    {
        // arrange
        Money money1 = new Money(100);
        Money money2 = new Money(50);

        // act
        Money result = money1 + money2;

        // assert
        Assert.NotEqual(money1, result);
        Assert.NotEqual(money2, result);
        Assert.Equal(150, result.Value);
    }

    [Fact]
    public void Money_SubtractionOperator_ShouldReturnNewInstanceWithSubtractedAmount()
    {
        // arrange
        Money money1 = new Money(70);
        Money money2 = new Money(50);

        // act
        Money result = money1 - money2;

        // assert
        Assert.NotEqual(money1, result);
        Assert.NotEqual(money2, result);
        Assert.Equal(20, result.Value);
    }

    [Fact]
    public void Money_LessThanOperator_ShouldReturnsExpectedResult()
    {
        // arrange
        Money money1 = new Money(10);
        Money money2 = new Money(20);

        // act
        bool result = money1 < money2;

        // assert
        Assert.True(result);
    }

    [Fact]
    public void Money_GreaterThanOperator_ShouldReturnsExpectedResult()
    {
        // arrange
        Money money1 = new Money(20);
        Money money2 = new Money(10);

        // act
        bool result = money1 > money2;

        // assert
        Assert.True(result);
    }

    [Fact]
    public void Money_LessThanOrEqualOperator_ShouldOperatorReturnsExpectedResult()
    {
        // arrange
        Money money1 = new Money(10);
        Money money2 = new Money(20);

        // act
        bool result = money1 <= money2;

        // assert
        Assert.True(result);
    }

    [Fact]
    public void Money_GreaterThanOrEqualOperator_ShouldReturnsExpectedResult()
    {
        // arrange
        Money money1 = new Money(20);
        Money money2 = new Money(20);

        // act
        bool result = money1 >= money2;

        // assert
        Assert.True(result);
    }
}

