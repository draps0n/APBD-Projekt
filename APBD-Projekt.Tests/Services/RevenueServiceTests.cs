using System.Reflection;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.Services;
using APBD_Projekt.Services.Abstractions;
using APBD_Projekt.Tests.TestObjects;
using Moq;
using Xunit.Abstractions;

namespace APBD_Projekt.Tests.Services;

public class RevenueServiceTests
{
    private readonly ICurrencyService _currencyService;
    private readonly ITestOutputHelper _testOutputHelper;

    public RevenueServiceTests(ITestOutputHelper testOutputHelper)
    {
        var currencyServiceMock = new Mock<ICurrencyService>();
        currencyServiceMock
            .Setup(cs => cs.ConvertFromPlnToCurrencyAsync(It.IsAny<decimal>(), It.IsAny<string>()))
            .ReturnsAsync((decimal money, string _) => money);
        _currencyService = currencyServiceMock.Object;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetCurrentTotalRevenueAsync_ShouldReturnCorrectValue()
    {
        // Arrange
        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repo => repo.GetCurrentContractsRevenueAsync())
            .ReturnsAsync(100m);
        var subscriptionsRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetCurrentSubscriptionsRevenueAsync())
            .ReturnsAsync(500m);

        var revenueService = new RevenueService(contractsRepositoryMock.Object, subscriptionsRepositoryMock.Object,
            new SoftwareRepository(null!), _currencyService);

        // Act
        var result = await revenueService.GetCurrentTotalRevenueAsync("");

        // Assert
        Assert.Equal(600m, result.CurrentRevenue);
    }

    [Fact]
    public async Task GetCurrentRevenueForSoftwareAsync_ShouldReturnCorrectValue_WhenSoftwareExists()
    {
        // Arrange
        const int softwareId = 1;
        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repo => repo.GetCurrentContractsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(100m);
        var subscriptionsRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetCurrentSubscriptionsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(500m);

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repo => repo.GetSoftwareByIdAsync(softwareId))
            .ReturnsAsync(new Software("Word", "word desc", 100));

        var revenueService = new RevenueService(contractsRepositoryMock.Object, subscriptionsRepositoryMock.Object,
            softwareRepositoryMock.Object, _currencyService);

        // Act
        var result = await revenueService.GetCurrentRevenueForSoftwareAsync(softwareId, "");

        // Assert
        Assert.Equal(600m, result.CurrentRevenue);
    }

    [Fact]
    public async Task GetCurrentRevenueForSoftwareAsync_ShouldThrowExc_WhenSoftwareDoesNotExists()
    {
        // Arrange
        const int softwareId = 1;
        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repo => repo.GetCurrentContractsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(100m);
        var subscriptionsRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetCurrentSubscriptionsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(500m);

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repo => repo.GetSoftwareByIdAsync(softwareId))
            .ReturnsAsync((Software?)null);

        var revenueService = new RevenueService(contractsRepositoryMock.Object, subscriptionsRepositoryMock.Object,
            softwareRepositoryMock.Object, _currencyService);

        // Act & Assert
        var e = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await revenueService.GetCurrentRevenueForSoftwareAsync(softwareId, ""));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task GetForecastedRevenueForSoftwareAsync_ShouldThrowExc_WhenSoftwareDoesNotExists()
    {
        // Arrange
        const int softwareId = 1;
        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repo => repo.GetForecastedContractsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(100m);
        var subscriptionsRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetCurrentSubscriptionsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(500m);
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetNotYetPaidSubscriptionsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(200m);

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repo => repo.GetSoftwareByIdAsync(softwareId))
            .ReturnsAsync((Software?)null);

        var revenueService = new RevenueService(contractsRepositoryMock.Object, subscriptionsRepositoryMock.Object,
            softwareRepositoryMock.Object, _currencyService);

        // Act & Assert
        var e = await Assert.ThrowsAsync<NotFoundException>(async () =>
            await revenueService.GetForecastedRevenueForSoftwareAsync(softwareId, ""));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task GetForecastedRevenueForSoftwareAsync_ShouldReturnCorrectValue_WhenSoftwareExists()
    {
        // Arrange
        const int softwareId = 1;
        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repo => repo.GetForecastedContractsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(100m);
        var subscriptionsRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetCurrentSubscriptionsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(500m);
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetNotYetPaidSubscriptionsRevenueForSoftwareAsync(softwareId))
            .ReturnsAsync(200m);

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repo => repo.GetSoftwareByIdAsync(softwareId))
            .ReturnsAsync(new Software("", "", 10));

        var revenueService = new RevenueService(contractsRepositoryMock.Object, subscriptionsRepositoryMock.Object,
            softwareRepositoryMock.Object, _currencyService);

        // Act
        var result = await revenueService.GetForecastedRevenueForSoftwareAsync(softwareId, "");

        // Assert
        Assert.Equal(800m, result.ForecastedRevenue);
    }
    
    [Fact]
    public async Task GetForecastedTotalRevenueAsync_ShouldReturnCorrectValue()
    {
        // Arrange
        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repo => repo.GetForecastedContractsRevenueAsync())
            .ReturnsAsync(100m);
        var subscriptionsRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetCurrentSubscriptionsRevenueAsync())
            .ReturnsAsync(500m);
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetNotYetPaidSubscriptionsRevenueAsync())
            .ReturnsAsync(200m);

        var revenueService = new RevenueService(contractsRepositoryMock.Object, subscriptionsRepositoryMock.Object,
            new SoftwareRepository(null!), _currencyService);

        // Act
        var result = await revenueService.GetForecastedTotalRevenueAsync("");

        // Assert
        Assert.Equal(800m, result.ForecastedRevenue);
    }

    [Fact]
    public async Task ConvertToDesiredCurrencyAsync_ShouldReturnPln_WhenCurrencyNotGiven()
    {
        // Arrange
        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repo => repo.GetCurrentContractsRevenueAsync())
            .ReturnsAsync(100m);
        var subscriptionsRepositoryMock = new Mock<ISubscriptionsRepository>();
        subscriptionsRepositoryMock
            .Setup(repo => repo.GetCurrentSubscriptionsRevenueAsync())
            .ReturnsAsync(500m);

        var revenueService = new RevenueService(contractsRepositoryMock.Object, subscriptionsRepositoryMock.Object,
            new SoftwareRepository(null!), _currencyService);

        // Act
        var result = await revenueService.GetCurrentTotalRevenueAsync(null);

        // Assert
        Assert.Equal("PLN", result.Currency);
    }
}