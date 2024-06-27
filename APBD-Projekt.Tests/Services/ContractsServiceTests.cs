using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services;
using Moq;
using Xunit.Abstractions;

namespace APBD_Projekt.Tests.Services;

public class ContractsServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ContractsServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateContractAsync_ShouldThrowExc_WhenClientDoesNotExist()
    {
        // Arrange
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync((IndividualClient?)null);
        var contractsService = new ContractsService(clientRepositoryMock.Object, null!, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            await contractsService.CreateContractAsync(0, null!));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldThrowExc_WhenSoftwareVersionDoesNotExist()
    {
        // Arrange
        var request = new CreateContractRequestModel
        {
            SoftwareName = "sn",
            SoftwareVersion = "sv"
        };

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(new IndividualClient("", "", "", "", "", ""));

        var discountRepository = new Mock<IDiscountsRepository>();

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repository =>
                repository.GetSoftwareVersionWithSoftwareByNameAndVersionAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((SoftwareVersion?)null);

        var contractsService = new ContractsService(clientRepositoryMock.Object, null!, discountRepository.Object,
            softwareRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<SoftwareNotFoundException>(async () =>
            await contractsService.CreateContractAsync(1, request));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldThrowExc_WhenContractTimespanIsNotValid()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var softwareVersion = new SoftwareVersion("", null!);
        var request = new CreateContractRequestModel
        {
            SoftwareName = "sn",
            SoftwareVersion = "sv",
            EndDate = DateTime.Now
        };

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var discountRepository = new Mock<IDiscountsRepository>();

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repository =>
                repository.GetSoftwareVersionWithSoftwareByNameAndVersionAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(softwareVersion);

        var contractsService = new ContractsService(clientRepositoryMock.Object, null!, discountRepository.Object,
            softwareRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<InvalidRequestFormatException>(async () =>
            await contractsService.CreateContractAsync(1, request));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldThrowExc_WhenClientAlreadyHasThisSoftwareActive()
    {
        // Arrange
        const int softwareId = 1;
        var client = new IndividualClient("", "", "", "", "", "");
        var software = new Software("sn", "", 100);
        typeof(Software).GetProperty(nameof(software.IdSoftware))!.SetValue(software, softwareId);
        var subscriptionOffer = new SubscriptionOffer("", 100, software, 1);
        typeof(SubscriptionOffer).GetProperty(nameof(subscriptionOffer.IdSoftware))!.SetValue(subscriptionOffer,
            softwareId);
        var subscription = new Subscription(DateTime.Now, client, subscriptionOffer);
        var softwareVersion = new SoftwareVersion("sv", software);
        client.Subscriptions.Add(subscription);
        var request = new CreateContractRequestModel
        {
            SoftwareName = "sn",
            SoftwareVersion = "sv",
            EndDate = DateTime.Now.AddDays(15)
        };

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var discountRepository = new Mock<IDiscountsRepository>();

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repository =>
                repository.GetSoftwareVersionWithSoftwareByNameAndVersionAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(softwareVersion);

        var contractsService = new ContractsService(clientRepositoryMock.Object, null!, discountRepository.Object,
            softwareRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<InvalidRequestFormatException>(async () =>
            await contractsService.CreateContractAsync(1, request));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldReturnCorrectFinalPrice_WhenDataIsValid()
    {
        // Arrange
        const int softwareId = 1;
        var client = new IndividualClient("", "", "", "", "", "");
        var software = new Software("sn", "", 100);
        typeof(Software).GetProperty(nameof(software.IdSoftware))!.SetValue(software, softwareId);
        var softwareVersion = new SoftwareVersion("sv", software);
        var request = new CreateContractRequestModel
        {
            SoftwareName = "sn",
            SoftwareVersion = "sv",
            EndDate = DateTime.Now.AddDays(15)
        };

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();

        var contractsRepositoryMock = new Mock<IContractsRepository>();

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repository =>
                repository.GetSoftwareVersionWithSoftwareByNameAndVersionAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(softwareVersion);

        var contractsService = new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object,
            discountRepositoryMock.Object, softwareRepositoryMock.Object);

        // Act
        var result = await contractsService.CreateContractAsync(1, request);
        var finalPrice = result.FinalPrice;

        // Assert
        Assert.Equal(100, finalPrice);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldReturnCorrectFinalPrice_WhenDataIsValidAndDiscount()
    {
        // Arrange
        const int softwareId = 1;
        var client = new IndividualClient("", "", "", "", "", "");
        var software = new Software("sn", "", 100);
        typeof(Software).GetProperty(nameof(software.IdSoftware))!.SetValue(software, softwareId);
        var softwareVersion = new SoftwareVersion("sv", software);
        var discount = new Discount("", DiscountType.License, 10, DateTime.Now, DateTime.Now);
        var request = new CreateContractRequestModel
        {
            SoftwareName = "sn",
            SoftwareVersion = "sv",
            EndDate = DateTime.Now.AddDays(15)
        };

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();
        discountRepositoryMock
            .Setup(repository =>
                repository.GetBestActiveDiscountForContractAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(discount);

        var contractsRepositoryMock = new Mock<IContractsRepository>();

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repository =>
                repository.GetSoftwareVersionWithSoftwareByNameAndVersionAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(softwareVersion);

        var contractsService = new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object,
            discountRepositoryMock.Object, softwareRepositoryMock.Object);

        // Act
        var result = await contractsService.CreateContractAsync(1, request);
        var finalPrice = result.FinalPrice;

        // Assert
        Assert.Equal(90, finalPrice);
    }

    [Fact]
    public async Task CreateContractAsync_ShouldReturnCorrectFinalPrice_WhenDataIsValidAndDiscountAndIsRegularClient()
    {
        // Arrange
        const int softwareId = 1;
        var client = new IndividualClient("", "", "", "", "", "");
        var software = new Software("sn", "", 100);
        typeof(Software).GetProperty(nameof(software.IdSoftware))!.SetValue(software, softwareId);
        var subscriptionOffer = new SubscriptionOffer("", 100, software, 1);
        typeof(SubscriptionOffer).GetProperty(nameof(subscriptionOffer.IdSoftware))!.SetValue(subscriptionOffer,
            2);
        var subscription = new Subscription(DateTime.Now, client, subscriptionOffer);
        client.Subscriptions.Add(subscription);
        var softwareVersion = new SoftwareVersion("sv", software);
        var discount = new Discount("", DiscountType.License, 10, DateTime.Now, DateTime.Now);
        var request = new CreateContractRequestModel
        {
            SoftwareName = "sn",
            SoftwareVersion = "sv",
            EndDate = DateTime.Now.AddDays(15),
            YearsOfAdditionalSupport = 1
        };

        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();
        discountRepositoryMock
            .Setup(repository =>
                repository.GetBestActiveDiscountForContractAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(discount);

        var contractsRepositoryMock = new Mock<IContractsRepository>();

        var softwareRepositoryMock = new Mock<ISoftwareRepository>();
        softwareRepositoryMock
            .Setup(repository =>
                repository.GetSoftwareVersionWithSoftwareByNameAndVersionAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(softwareVersion);

        var contractsService = new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object,
            discountRepositoryMock.Object, softwareRepositoryMock.Object);

        // Act
        var result = await contractsService.CreateContractAsync(1, request);
        var finalPrice = result.FinalPrice;

        // Assert
        Assert.Equal(940.5m, finalPrice);
    }

    [Fact]
    public async Task DeleteContractByIdAsync_ShouldThrowExc_WhenContractDoesNotExist()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Contract?)null);
        var contractsService =
            new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ContractNotFoundException>(async () =>
            await contractsService.DeleteContractByIdAsync(0, 0));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task DeleteContractByIdAsync_ShouldThrowExc_WhenClientDoesNotExist()
    {
        // Arrange
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((IndividualClient?)null);
        var contractsService = new ContractsService(clientRepositoryMock.Object, null!, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            await contractsService.DeleteContractByIdAsync(0, 0));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task DeleteContractByIdAsync_ShouldThrowExc_WhenClientIsNotAnOwnerOfContract()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        var contract = new Contract(DateTime.Now, DateTime.Now, 0, 0m, null!, null!);
        typeof(Contract).GetProperty(nameof(contract.IdClient))!.SetValue(contract, 1);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);
        var contractsService =
            new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<InvalidRequestFormatException>(async () =>
            await contractsService.DeleteContractByIdAsync(0, 0));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task DeleteContractByIdAsync_ShouldDeleteContract_WhenIdsMatch()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        typeof(Client).GetProperty(nameof(client.IdClient))!.SetValue(client, 1);
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        var contract = new Contract(DateTime.Now, DateTime.Now, 0, 0m, client, null!);
        typeof(Contract).GetProperty(nameof(contract.IdClient))!.SetValue(contract, 1);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);
        var contractsService =
            new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object, null!, null!);

        // Act
        await contractsService.DeleteContractByIdAsync(1, 1);

        // Assert
        Assert.True(true);
    }

    [Fact]
    public async Task PayForContractAsync_ShouldThrowExc_WhenClientDoesNotExist()
    {
        // Arrange
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((IndividualClient?)null);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        var contractsService =
            new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            await contractsService.PayForContractAsync(1, 1, 100m));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task PayForContractAsync_ShouldThrowExc_WhenContractDoesNotExist()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Contract?)null);
        var contractsService =
            new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ContractNotFoundException>(async () =>
            await contractsService.PayForContractAsync(1, 1, 100m));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task PayForContractAsync_ShouldThrowExc_WhenPayingForSignedContract()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var contract = new Contract(DateTime.Now, DateTime.Now, 0, 0m, client, null!);
        typeof(Contract).GetProperty(nameof(contract.SignedAt))!.SetValue(contract, DateTime.Now);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);
        var contractsService =
            new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ContractException>(async () =>
            await contractsService.PayForContractAsync(1, 1, 100m));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task PayForContractAsync_ShouldThrowExc_WhenAmountLowerThanZero()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var contract = new Contract(DateTime.Now, DateTime.Now, 0, 0m, client, null!);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);
        var contractsService =
            new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object, null!, null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<PaymentException>(async () =>
            await contractsService.PayForContractAsync(1, 1, -1m));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task PayForContractAsync_ShouldReturnNewContract_WhenPreviousIsInactive()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);

        var software = new Software("", "", 1);
        typeof(Software).GetProperty(nameof(software.IdSoftware))!.SetValue(software, 1);
        var softwareVersion = new SoftwareVersion("", software);
        typeof(SoftwareVersion).GetProperty(nameof(softwareVersion.IdSoftware))!.SetValue(softwareVersion, 1);
        var contract = new Contract(DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-1), 0, 0m, client, softwareVersion);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();

        var contractsService = new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object,
            discountRepositoryMock.Object, null!);

        // Act
        var result = await contractsService.PayForContractAsync(1, 1, 1m);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(contract.StartDate, result.StartDate);
        Assert.NotEqual(contract.EndDate, result.EndDate);
    }
    
    [Fact]
    public async Task PayForContractAsync_ShouldReturnNothing_WhenPaymentIsPossibleAndIncomplete()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        
        var contract = new Contract(DateTime.Now, DateTime.Now.AddDays(4), 0, 2m, client, null!);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();

        var contractsService = new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object,
            discountRepositoryMock.Object, null!);

        // Act
        var result = await contractsService.PayForContractAsync(1, 1, 1m);

        // Assert
        Assert.Null(result);
        Assert.Null(contract.SignedAt);
    }
    
    [Fact]
    public async Task PayForContractAsync_ShouldReturnNothingAndSign_WhenPaymentIsPossibleAndComplete()
    {
        // Arrange
        var client = new IndividualClient("", "", "", "", "", "");
        var clientRepositoryMock = new Mock<IClientsRepository>();
        clientRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        clientRepositoryMock
            .Setup(repository => repository.GetClientWithBoughtProductsAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        
        var contract = new Contract(DateTime.Now, DateTime.Now.AddDays(4), 0, 2m, client, null!);

        var contractsRepositoryMock = new Mock<IContractsRepository>();
        contractsRepositoryMock
            .Setup(repository => repository.GetContractWithSoftwareClientAndPaymentsByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(contract);

        var discountRepositoryMock = new Mock<IDiscountsRepository>();

        var contractsService = new ContractsService(clientRepositoryMock.Object, contractsRepositoryMock.Object,
            discountRepositoryMock.Object, null!);

        // Act
        var result = await contractsService.PayForContractAsync(1, 1, 2m);

        // Assert
        Assert.Null(result);
        Assert.NotNull(contract.SignedAt);
    }
}