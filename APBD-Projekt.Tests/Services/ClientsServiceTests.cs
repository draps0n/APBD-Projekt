using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services;
using Moq;
using Xunit.Abstractions;

namespace APBD_Projekt.Tests.Services;

public class ClientsServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClientsServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateNewClientAsync_ShouldThrowExc_WhenClientTypeDoesNotMatch()
    {
        // Arrange
        var request = new CreateClientRequestModel
        {
            ClientType = "ala"
        };
        var clientsService = new ClientsService(null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientTypeException>(async () =>
            await clientsService.CreateNewClientAsync(request));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateNewClientAsync_ShouldThrowExc_WhenFieldsInRequestDoNotMatchType()
    {
        // Arrange
        var request = new CreateClientRequestModel
        {
            ClientType = "Individual"
        };
        var clientsService = new ClientsService(null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<InvalidRequestFormatException>(async () =>
            await clientsService.CreateNewClientAsync(request));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateNewClientAsync_ShouldThrowExc_WhenPeselNotUnique()
    {
        // Arrange
        var request = new CreateClientRequestModel
        {
            ClientType = "Individual",
            Address = "",
            Phone = "",
            Email = "",
            Name = "adam",
            LastName = "kowal",
            PESEL = "12312312312"
        };
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByPeselAsync(It.IsAny<string>()))
            .ReturnsAsync(new IndividualClient("", "", "", "", "", ""));
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<NotUniqueIdentifierException>(async () =>
            await clientsService.CreateNewClientAsync(request));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateNewClientAsync_ShouldThrowExc_WhenKrsNotUnique()
    {
        // Arrange
        var request = new CreateClientRequestModel
        {
            ClientType = "Company",
            Address = "",
            Phone = "",
            Email = "",
            CompanyName = "cn",
            KRS = "1231231231"
        };
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByKrsAsync(It.IsAny<string>()))
            .ReturnsAsync(new CompanyClient("", "", "", "", ""));
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<NotUniqueIdentifierException>(async () =>
            await clientsService.CreateNewClientAsync(request));
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task CreateNewClientAsync_ShouldSuccessfullyCreateClient_WhenIndividualClientIsValid()
    {
        // Arrange
        var request = new CreateClientRequestModel
        {
            ClientType = "Individual",
            Address = "",
            Phone = "",
            Email = "",
            Name = "adam",
            LastName = "kowal",
            PESEL = "12312312312"
        };
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByPeselAsync(It.IsAny<string>()))
            .ReturnsAsync((IndividualClient?)null);
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act
        var result = await clientsService.CreateNewClientAsync(request);

        // Assert
        Assert.Equal("", result.Address);
        Assert.Equal("", result.Phone);
        Assert.Equal("", result.Email);
        Assert.Equal("adam", result.Name);
        Assert.Equal("kowal", result.LastName);
        Assert.Equal("12312312312", result.PESEL);
    }

    [Fact]
    public async Task CreateNewClientAsync_ShouldSuccessfullyCreateClient_WhenCompanyClientIsValid()
    {
        // Arrange
        var request = new CreateClientRequestModel
        {
            ClientType = "Company",
            Address = "",
            Phone = "",
            Email = "",
            CompanyName = "cn",
            KRS = "1231231231"
        };
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByKrsAsync(It.IsAny<string>()))
            .ReturnsAsync((CompanyClient?)null);
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act
        var result = await clientsService.CreateNewClientAsync(request);

        // Assert
        Assert.Equal("", result.Address);
        Assert.Equal("", result.Phone);
        Assert.Equal("", result.Email);
        Assert.Equal("cn", result.CompanyName);
        Assert.Equal("1231231231", result.KRS);
    }

    [Fact]
    public async Task DeleteClientByIdAsync_ShouldThrowExc_WhenClientDoesNotExist()
    {
        // Arrange
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Client?)null);
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            await clientsService.DeleteClientByIdAsync(1));
        _testOutputHelper.WriteLine(e.Message);
    }
    
    [Fact]
    public async Task DeleteClientByIdAsync_ShouldThrowExc_WhenClientIsCompanyClient()
    {
        // Arrange
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new CompanyClient("", "", "", "", ""));
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientTypeException>(async () =>
            await clientsService.DeleteClientByIdAsync(1));
        _testOutputHelper.WriteLine(e.Message);
    }
    
    [Fact]
    public async Task DeleteClientByIdAsync_ShouldSoftDelete_WhenClientIsIndividualClient()
    {
        // Arrange
        var client = new IndividualClient("a", "a", "a", "a", "a", "a");
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act
        await clientsService.DeleteClientByIdAsync(1);
        
        // Assert
        Assert.Equal("", client.Address);
        Assert.Equal("", client.Email);
        Assert.Equal("", client.Phone);
        Assert.Equal("", client.Name);
        Assert.Equal("", client.LastName);
        Assert.Equal("", client.PESEL);
        Assert.True(client.IsDeleted);
    }
    
    [Fact]
    public async Task UpdateClientByIdAsync_ShouldThrowExc_WhenClientTypeDoesNotMatch()
    {
        // Arrange
        var request = new UpdateClientRequestModel
        {
            ClientType = "ala"
        };
        var clientsService = new ClientsService(null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientTypeException>(async () =>
            await clientsService.UpdateClientByIdAsync(1, request));
        _testOutputHelper.WriteLine(e.Message);
    }
    
    [Fact]
    public async Task UpdateClientByIdAsync_ShouldThrowExc_WhenFieldsInRequestDoNotMatchType()
    {
        // Arrange
        var request = new UpdateClientRequestModel
        {
            ClientType = "Individual",
            LastName = null,
            Name = "a",
            Address = "",
            Phone = "",
            Email = ""
        };
        var clientsService = new ClientsService(null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<InvalidRequestFormatException>(async () =>
            await clientsService.UpdateClientByIdAsync(1, request));
        _testOutputHelper.WriteLine(e.Message);
    }
    
    [Fact]
    public async Task UpdateClientByIdAsync_ShouldThrowExc_WhenClientOfIdDoesNotMatchType()
    {
        // Arrange
        var request = new UpdateClientRequestModel
        {
            ClientType = "Individual",
            LastName = "a",
            Name = "a",
            Address = "a",
            Phone = "a",
            Email = "a"
        };
        var client = new CompanyClient("", "", "", "", "");
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientTypeException>(async () =>
            await clientsService.UpdateClientByIdAsync(1, request));
        _testOutputHelper.WriteLine(e.Message);
    }
    
    [Fact]
    public async Task UpdateClientByIdAsync_ShouldThrowExc_WhenClientIsDeleted()
    {
        // Arrange
        var request = new UpdateClientRequestModel
        {
            ClientType = "Individual",
            LastName = "a",
            Name = "a",
            Address = "a",
            Phone = "a",
            Email = "a"
        };
        var client = new IndividualClient("", "", "", "", "", "");
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act
        client.Delete();
        
        // Act & Assert
        var e = await Assert.ThrowsAsync<ClientNotFoundException>(async () =>
            await clientsService.UpdateClientByIdAsync(1, request));
        _testOutputHelper.WriteLine(e.Message);
    }
    
    [Fact]
    public async Task UpdateClientByIdAsync_ShouldUpdateClient_WhenClientDataValid()
    {
        // Arrange
        var request = new UpdateClientRequestModel
        {
            ClientType = "Individual",
            LastName = "a",
            Name = "a",
            Address = "a",
            Phone = "a",
            Email = "a"
        };
        var client = new IndividualClient("", "", "", "", "", "");
        var clientsRepositoryMock = new Mock<IClientsRepository>();
        clientsRepositoryMock
            .Setup(repository => repository.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(client);
        var clientsService = new ClientsService(clientsRepositoryMock.Object);

        // Act
        await clientsService.UpdateClientByIdAsync(1, request);
        
        // Assert
        Assert.Equal("a", client.Address);
        Assert.Equal("a", client.Phone);
        Assert.Equal("a", client.Email);
        Assert.Equal("a", client.Name);
        Assert.Equal("a", client.LastName);
    }
}