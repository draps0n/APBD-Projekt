using System.Reflection;
using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.RequestModels;
using Xunit.Abstractions;

namespace APBD_Projekt.Tests.Models;

public class ClientTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ClientTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void IsRegularClient_ShouldReturnFalse_WhenHasOnlyUnsignedContract()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        var contract = new Contract(
            DateTime.Today,
            DateTime.Today,
            0,
            0m,
            client,
            null!
        );
        client.Contracts.Add(contract);

        // Act
        var result = client.IsRegularClient();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRegularClient_ShouldReturnFalse_WhenHasNothing()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );

        // Act
        var result = client.IsRegularClient();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRegularClient_ShouldReturnTrue_WhenHasSignedContract()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        var contract = new Contract(
            DateTime.Today,
            DateTime.Today,
            0,
            0m,
            client,
            null!
        );
        typeof(Contract).GetProperty("SignedAt")!.SetValue(contract, DateTime.Now);
        client.Contracts.Add(contract);

        // Act
        var result = client.IsRegularClient();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EnsureIsOwnerOfContract_ShouldNotThrowExc_WhenIsOwner()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        typeof(IndividualClient).GetProperty("IdClient")!.SetValue(client, 1);
        var contract = new Contract(
            DateTime.Today,
            DateTime.Today,
            0,
            0m,
            client,
            null!
        );
        typeof(Contract).GetProperty("IdClient")!.SetValue(contract, 1);
        client.Contracts.Add(contract);

        // Act & Assert
        try
        {
            client.EnsureIsOwnerOfContract(contract);
            Assert.True(true);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [Fact]
    public void EnsureIsOwnerOfContract_ShouldThrowExc_WhenIsNotAnOwner()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        typeof(IndividualClient).GetProperty("IdClient")!.SetValue(client, 1);
        var contract = new Contract(
            DateTime.Today,
            DateTime.Today,
            0,
            0m,
            null!,
            null!
        );
        typeof(Contract).GetProperty("IdClient")!.SetValue(contract, 0);

        // Act & Assert
        var exc = Assert.Throws<BadRequestException>(() => client.EnsureIsOwnerOfContract(contract));
        _testOutputHelper.WriteLine(exc.Message);
    }

    [Fact]
    public void EnsureHasNoActiveSoftware_ShouldThrowExc_WhenHasActiveSoftwareContract()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        var software = new Software("", "", 100);
        typeof(Software).GetProperty("IdSoftware")!.SetValue(software, 1);
        var softwareVersion = new SoftwareVersion("", software);
        typeof(SoftwareVersion).GetProperty("IdSoftware")!.SetValue(softwareVersion, 1);
        var contract = new Contract(
            DateTime.Today,
            DateTime.Today,
            1,
            0m,
            null!,
            softwareVersion
        );
        client.Contracts.Add(contract);
        typeof(Contract).GetProperty("SignedAt")!.SetValue(contract, DateTime.Now);

        // Act & Assert
        var exc = Assert.Throws<BadRequestException>(() => client.EnsureHasNoActiveSoftware(software));
        _testOutputHelper.WriteLine(exc.Message);
    }

    [Fact]
    public void EnsureHasNoActiveSoftware_ShouldThrowExc_WhenHasActiveSoftwareContractOffer
        ()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        var software = new Software("", "", 100);
        typeof(Software).GetProperty("IdSoftware")!.SetValue(software, 1);
        var softwareVersion = new SoftwareVersion("", software);
        typeof(SoftwareVersion).GetProperty("IdSoftware")!.SetValue(softwareVersion, 1);
        var contract = new Contract(
            DateTime.Today,
            DateTime.Today.AddYears(1),
            1,
            0m,
            null!,
            softwareVersion
        );
        client.Contracts.Add(contract);

        // Act & Assert
        var exc = Assert.Throws<BadRequestException>(() => client.EnsureHasNoActiveSoftware(software));
        _testOutputHelper.WriteLine(exc.Message);
    }

    [Fact]
    public void EnsureHasNoActiveSoftware_ShouldThrowExc_WhenHasActiveSoftwareSubscription()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        var software = new Software("", "", 100);
        typeof(Software).GetProperty("IdSoftware")!.SetValue(software, 1);
        var subscriptionOffer = new SubscriptionOffer("", 1m, software, 1);
        typeof(SubscriptionOffer).GetProperty("IdSoftware")!.SetValue(subscriptionOffer, 1);
        var subscription = new Subscription(
            DateTime.Today,
            client,
            subscriptionOffer
        );
        client.Subscriptions.Add(subscription);

        // Act & Assert
        var exc = Assert.Throws<BadRequestException>(() => client.EnsureHasNoActiveSoftware(software));
        _testOutputHelper.WriteLine(exc.Message);
    }

    [Fact]
    public void EnsureHasNoActiveSoftware_ShouldNotThrowExc_WhenHasNoActiveSoftware()
    {
        // Arrange
        var client = new IndividualClient(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );
        var software = new Software("", "", 100);
        typeof(Software).GetProperty("IdSoftware")!.SetValue(software, 1);

        // Act & Assert
        try
        {
            client.EnsureHasNoActiveSoftware(software);
            Assert.True(true);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [Fact]
    public void Delete_ShouldSoftDeleteClientData_WhenIndividualClient()
    {
        // Arrange
        var individualClient = new IndividualClient(
            "",
            "",
            "",
            "",
            "",
            ""
        );
        
        // Act
        individualClient.Delete();
        var isDeleted = individualClient.IsDeleted;
        
        // Assert
        Assert.True(isDeleted);
    }

    [Fact]
    public void Delete_ShouldThrowExc_WhenCompanyClient()
    {
        // Arrange
        var companyClient = new CompanyClient(
            "",
            "",
            "",
            "",
            ""
        );

        // Act & Assert
        var exc = Assert.Throws<BadRequestException>(() => companyClient.Delete());
        _testOutputHelper.WriteLine(exc.Message);
    }
    
    
    [Fact]
    public void Update_ShouldUpdateCompanyClientFields_WhenCompanyClient()
    {
        // Arrange
        const string oldAddress = "";
        const string oldEmail = "";
        const string oldPhone = "";
        const string oldCompanyName = "";
        var companyClient = new CompanyClient(
            oldAddress,
            oldEmail,
            oldPhone,
            oldCompanyName,
            ""
        );
        var request = new UpdateClientRequestModel
        {
            Address = "a",
            CompanyName = "a",
            ClientType = "Company",
            Email = "a",
            LastName = "a",
            Name = "a",
            Phone = "a"
        };

        // Act
        companyClient.Update(request);
        
        // Assert
        Assert.NotEqual(oldAddress, companyClient.Address);
        Assert.NotEqual(oldEmail, companyClient.Email);
        Assert.NotEqual(oldPhone, companyClient.Phone);
        Assert.NotEqual(oldCompanyName, companyClient.CompanyName);
    }
    
    [Fact]
    public void Update_ShouldUpdateIndividualClientFields_WhenIndividualClient()
    {
        // Arrange
        const string oldAddress = "";
        const string oldEmail = "";
        const string oldPhone = "";
        const string oldName = "";
        const string oldLastName = "";
        var individualClient = new IndividualClient(
            oldAddress,
            oldEmail,
            oldPhone,
            oldName,
            oldLastName,
            ""
        );
        var request = new UpdateClientRequestModel
        {
            Address = "a",
            CompanyName = "a",
            ClientType = "Individual",
            Email = "a",
            LastName = "a",
            Name = "a",
            Phone = "a"
        };

        // Act
        individualClient.Update(request);
        
        // Assert
        Assert.NotEqual(oldAddress, individualClient.Address);
        Assert.NotEqual(oldEmail, individualClient.Email);
        Assert.NotEqual(oldPhone, individualClient.Phone);
        Assert.NotEqual(oldName, individualClient.Name);
        Assert.NotEqual(oldLastName, individualClient.LastName);
    }
    
    [Fact]
    public void Update_ShouldThrowExc_WhenIndividualClientIsDeleted()
    {
        // Arrange
        var individualClient = new IndividualClient(
            "",
            "",
            "",
            "",
            "",
            ""
        );
        var request = new UpdateClientRequestModel
        {
            Address = "a",
            CompanyName = "a",
            ClientType = "Individual",
            Email = "a",
            LastName = "a",
            Name = "a",
            Phone = "a"
        };

        // Act
        individualClient.Delete();
        
        
        // Act & Assert
        Assert.Throws<BadRequestException>(() => individualClient.Update(request));
    }

    [Fact]
    public void EnsureIsOfType_ShouldNotThrowExc_WhenMatching()
    {
        // Arrange
        var individualClient = new IndividualClient(
            "",
            "",
            "",
            "",
            "",
            ""
        );
        var companyClient = new CompanyClient(
            "",
            "",
            "",
            "",
            ""
        );
        
        // Act & Assert
        try
        {
            individualClient.EnsureIsOfType(ClientType.Individual);
            companyClient.EnsureIsOfType(ClientType.Company);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }
    
    [Fact]
    public void EnsureIsOfType_ShouldThrowExc_WhenNotMatching()
    {
        // Arrange
        var individualClient = new IndividualClient(
            "",
            "",
            "",
            "",
            "",
            ""
        );
        var companyClient = new CompanyClient(
            "",
            "",
            "",
            "",
            ""
        );
        
        // Act & Assert
        Assert.Throws<BadRequestException>(() => individualClient.EnsureIsOfType(ClientType.Company));
        Assert.Throws<BadRequestException>(() => companyClient.EnsureIsOfType(ClientType.Individual));
    }
}