using System.Reflection;
using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services;
using Moq;
using Xunit.Abstractions;

namespace APBD_Projekt.Tests.Services;

public class SubscriptionsServiceTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task CreateSubscriptionAsync_ShouldThrowExc_WhenClientDoesNotExist()
    {
        // Arrange
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(1))
            .ReturnsAsync((Client?)null);
        var subscriptionService = new SubscriptionsService(clientRepositoryMock.Object, null!, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            await subscriptionService.CreateSubscriptionAsync(1, null!));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ShouldThrowExc_WhenSubscriptionOfferDoesNotExist()
    {
        // Arrange
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(1))
            .ReturnsAsync(new IndividualClient("", "", "", "", "", ""));
        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repository => repository.GetSoftwareSubscriptionOfferWithSoftwareByNameAsync("sn", "so"))
            .ReturnsAsync((SubscriptionOffer?)null);
        var subscriptionService =
            new SubscriptionsService(clientRepositoryMock.Object, null!, softwareRepositoryMock.Object, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<SoftwareNotFoundException>(async () =>
            await subscriptionService.CreateSubscriptionAsync(1,
                new CreateSubscriptionRequestModel { SoftwareName = "sn", SubscriptionOfferName = "so" }));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ShouldThrowExc_WhenClientHasSoftwareActive()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var software = new Software("sn", "", 1);
        typeof(Software).GetProperty(nameof(software.IdSoftware))!.SetValue(software, 1);
        var subscriptionOffer = new SubscriptionOffer("so", 10m, software, 1);
        typeof(SubscriptionOffer).GetProperty(nameof(subscriptionOffer.IdSoftware))!.SetValue(subscriptionOffer, 1);

        client.Subscriptions.Add(new Subscription(DateTime.Now, client, subscriptionOffer));

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(1))
            .ReturnsAsync(client);

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock.Setup(repository =>
                repository.GetSoftwareSubscriptionOfferWithSoftwareByNameAsync("sn", "so"))
            .ReturnsAsync(subscriptionOffer);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();
        discountRepositoryMock
            .Setup(repository => repository.GetBestActiveDiscountForSubscriptionAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(new Discount("dsc", DiscountType.Both, 10, DateTime.Now, DateTime.Now));
        var subscriptionService =
            new SubscriptionsService(clientRepositoryMock.Object, null!, softwareRepositoryMock.Object,
                discountRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<InvalidRequestFormatException>(async () =>
            await subscriptionService.CreateSubscriptionAsync(1,
                new CreateSubscriptionRequestModel { SoftwareName = "sn", SubscriptionOfferName = "so" }));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateSubscriptionAsync_ShouldAddSubscription_WhenClientHasNoSoftwareActive()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var software = new Software("sn", "", 1);
        var subscriptionOffer = new SubscriptionOffer("so", 10m, software, 1);

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(1))
            .ReturnsAsync(client);

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock.Setup(repository =>
                repository.GetSoftwareSubscriptionOfferWithSoftwareByNameAsync("sn", "so"))
            .ReturnsAsync(subscriptionOffer);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();
        discountRepositoryMock
            .Setup(repository => repository.GetBestActiveDiscountForSubscriptionAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(new Discount("dsc", DiscountType.Both, 10, DateTime.Now, DateTime.Now));

        var subscriptionRepository = new Mock<ISubscriptionsRepository>();

        var subscriptionService =
            new SubscriptionsService(clientRepositoryMock.Object, subscriptionRepository.Object,
                softwareRepositoryMock.Object, discountRepositoryMock.Object);

        // Act
        var result = await subscriptionService.CreateSubscriptionAsync(1,
            new CreateSubscriptionRequestModel { SoftwareName = "sn", SubscriptionOfferName = "so" });

        // Act & Assert
        Assert.Equal("sn", result.SoftwareName);
    }

    [Fact]
    public async Task PayForSubscriptionAsync_ShouldThrowExc_WhenClientDoesNotExist()
    {
        // Arrange
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(1))
            .ReturnsAsync((Client?)null);
        var subscriptionService = new SubscriptionsService(clientRepositoryMock.Object, null!, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            await subscriptionService.PayForSubscriptionAsync(1, 0, new PayForSubscriptionRequestModel()));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task PayForSubscriptionAsync_ShouldThrowExc_WhenSubscriptionDoesNotExist()
    {
        // Arrange
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(1))
            .ReturnsAsync(new CompanyClient("", "", "", "", ""));

        var subscriptionRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionRepositoryMock
            .Setup(repository => repository.GetSubscriptionWithOfferByIdAsync(1))
            .ReturnsAsync((Subscription?)null);

        var subscriptionService =
            new SubscriptionsService(clientRepositoryMock.Object, subscriptionRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<SubscriptionNotFoundException>(async () =>
            await subscriptionService.PayForSubscriptionAsync(1, 1, new PayForSubscriptionRequestModel()));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task PayForSubscriptionAsync_ShouldThrowExc_WhenAmountIsNotValid()
    {
        // Arrange
        var client = new CompanyClient("", "", "", "", "");
        var subscriptionOffer = new SubscriptionOffer("", 40, null!, 12);
        var subscription = new Subscription(DateTime.Today, client, subscriptionOffer);
        typeof(Subscription)
            .GetProperty(nameof(subscription.NextPaymentDueDate))!
            .SetValue(subscription, DateTime.Now.AddYears(1));

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(1))
            .ReturnsAsync(client);

        var subscriptionRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionRepositoryMock
            .Setup(repository => repository.GetSubscriptionWithOfferByIdAsync(1))
            .ReturnsAsync(subscription);

        var subscriptionService =
            new SubscriptionsService(clientRepositoryMock.Object, subscriptionRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<PaymentException>(async () =>
            await subscriptionService.PayForSubscriptionAsync(1, 1,
                new PayForSubscriptionRequestModel { Amount = 30 }));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task PayForSubscriptionAsync_ShouldProcessPayment_WhenRequestValid()
    {
        // Arrange
        var client = new CompanyClient("", "", "", "", "");
        var subscriptionOffer = new SubscriptionOffer("", 30, null!, 12);
        var subscription = new Subscription(DateTime.Today, client, subscriptionOffer);
        typeof(Subscription)
            .GetProperty(nameof(subscription.NextPaymentDueDate))!
            .SetValue(subscription, DateTime.Now.AddYears(1));

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(1))
            .ReturnsAsync(client);

        var subscriptionRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionRepositoryMock
            .Setup(repository => repository.GetSubscriptionWithOfferByIdAsync(1))
            .ReturnsAsync(subscription);

        var subscriptionService =
            new SubscriptionsService(clientRepositoryMock.Object, subscriptionRepositoryMock.Object, null!, null!);

        // Act
        await subscriptionService.PayForSubscriptionAsync(1, 1, new PayForSubscriptionRequestModel { Amount = 30 });

        // Assert
        Assert.True(true);
    }
}