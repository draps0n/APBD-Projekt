using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using Xunit.Abstractions;

namespace APBD_Projekt.Tests.Models;

public class SubscriptionTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SubscriptionTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void CalculateFee_ShouldIncludeDiscount_WhenRegularClient()
    {
        // Arrange
        const decimal basePrice = 100m;
        var client = new IndividualClient("", "", "", "", "", "");
        var contract = new Contract(
            DateTime.Now,
            DateTime.Now,
            2,
            2m,
            client,
            null!
        );
        typeof(Contract).GetProperty("SignedAt")!.SetValue(contract, DateTime.Now);
        client.Contracts.Add(contract);
        var subscription = new Subscription(
            DateTime.Now,
            client,
            new SubscriptionOffer("", basePrice, null!, 1)
        );

        // Act
        var result = subscription.CalculateFee();

        // Assert
        Assert.Equal(basePrice * 0.95m, result);
    }

    [Fact]
    public void CalculateFee_ShouldNotIncludeDiscount_WhenNotRegularClient()
    {
        // Arrange
        const decimal basePrice = 100m;
        var client = new IndividualClient("", "", "", "", "", "");
        var subscription = new Subscription(
            DateTime.Now,
            client,
            new SubscriptionOffer("", basePrice, null!, 1)
        );

        // Act
        var result = subscription.CalculateFee();

        // Assert
        Assert.Equal(basePrice, result);
    }

    [Fact]
    public void ProcessPayment_ShouldThrowExcAndCancel_WhenLate()
    {
        // Arrange
        const decimal basePrice = 100m;
        var client = new IndividualClient("", "", "", "", "", "");
        var subscription = new Subscription(
            DateTime.Now.AddYears(-1),
            client,
            new SubscriptionOffer("", basePrice, null!, 1)
        );
        subscription.ProcessFirstPayment();

        // Act & Assert
        var e = Assert.Throws<BadRequestException>(() => subscription.ProcessPayment(10));
        _testOutputHelper.WriteLine(e.Message);
        Assert.NotNull(subscription.EndDate);
    }

    [Fact]
    public void ProcessPayment_ShouldThrowExc_WhenAlreadyPaid()
    {
        // Arrange
        const decimal basePrice = 100m;
        var client = new IndividualClient("", "", "", "", "", "");
        var subscription = new Subscription(
            DateTime.Now.AddDays(-1),
            client,
            new SubscriptionOffer("", basePrice, null!, 1)
        );
        subscription.ProcessFirstPayment();

        // Act & Assert
        var e = Assert.Throws<BadRequestException>(() => subscription.ProcessPayment(10));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public void ProcessPayment_ShouldThrowExc_WhenAmountIsNotRight()
    {
        // Arrange
        const decimal basePrice = 100m;
        var client = new IndividualClient("", "", "", "", "", "");
        var subscription = new Subscription(
            DateTime.Now.AddMonths(-1).AddDays(-5),
            client,
            new SubscriptionOffer("", basePrice, null!, 1)
        );
        subscription.ProcessFirstPayment();

        // Act & Assert
        var e = Assert.Throws<BadRequestException>(() => subscription.ProcessPayment(10));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public void ProcessPayment_ShouldAcceptPayment_WhenAmountIsRight()
    {
        // Arrange
        const decimal basePrice = 100m;
        var client = new IndividualClient("", "", "", "", "", "");
        var subscription = new Subscription(
            DateTime.Now.AddMonths(-1).AddDays(-5),
            client,
            new SubscriptionOffer("", basePrice, null!, 1)
        );
        subscription.ProcessFirstPayment();
        var paymentsAmountBefore = subscription.SubscriptionPayments.Count;

        // Act
        subscription.ProcessPayment(basePrice);
        var paymentsAmountAfter = subscription.SubscriptionPayments.Count;

        // Assert
        Assert.Equal(paymentsAmountBefore + 1, paymentsAmountAfter);
    }
}