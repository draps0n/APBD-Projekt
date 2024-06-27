using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;

namespace APBD_Projekt.Tests.Models;

public class ContractTests
{
    [Fact]
    public void ProcessPaymentAndSignIfPossible_ShouldThrowExc_WhenAmountTooHigh()
    {
        // Arrange
        var contract = new Contract(
            DateTime.Now,
            DateTime.Now.AddDays(3),
            2,
            100m,
            null!,
            null!
        );

        // Act & Assert
        Assert.Throws<BadRequestException>(() => contract.ProcessPaymentAndSignIfPossible(101));
    }

    [Fact]
    public void ProcessPaymentAndSignIfPossible_ShouldSignContract_WhenAmountRight()
    {
        // Arrange
        var contract = new Contract(
            DateTime.Now,
            DateTime.Now.AddDays(3),
            2,
            100m,
            null!,
            null!
        );

        // Act & Assert
        contract.ProcessPaymentAndSignIfPossible(100);
        var signedAt = contract.SignedAt;

        // Assert
        Assert.NotNull(signedAt);
    }

    [Fact]
    public void ProcessPaymentAndSignIfPossible_ShouldNotSignContract_WhenAmountTooLow()
    {
        // Arrange
        var contract = new Contract(
            DateTime.Now,
            DateTime.Now.AddDays(3),
            2,
            100m,
            null!,
            null!
        );

        // Act & Assert
        contract.ProcessPaymentAndSignIfPossible(10);
        var signedAt = contract.SignedAt;

        // Assert
        Assert.Null(signedAt);
    }

    [Fact]
    public void EnsureIsNotAlreadySigned_ShouldNotThrowExc_WhenNotSigned()
    {
        // Arrange
        var contract = new Contract(
            DateTime.Now,
            DateTime.Now.AddDays(3),
            2,
            100m,
            null!,
            null!
        );

        // Act
        contract.ProcessPaymentAndSignIfPossible(10);

        // Assert
        try
        {
            contract.EnsureIsNotAlreadySigned();
            Assert.True(true);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    [Fact]
    public void EnsureIsNotAlreadySigned_ShouldThrowExc_WhenSigned()
    {
        // Arrange
        var contract = new Contract(
            DateTime.Now,
            DateTime.Now.AddDays(3),
            2,
            100m,
            null!,
            null!
        );

        // Act
        contract.ProcessPaymentAndSignIfPossible(100);
        var signedAt = contract.SignedAt;

        // Act & Assert
        Assert.Throws<BadRequestException>(() => contract.EnsureIsNotAlreadySigned());
    }
    
    [Fact]
    public void IsActive_ShouldReturnTrue_WhenActive()
    {
        // Arrange
        var contract = new Contract(
            DateTime.Now,
            DateTime.Now.AddDays(3),
            2,
            100m,
            null!,
            null!
        );

        // Act
        var isActive = contract.IsActive();

        // Act & Assert
        Assert.True(isActive);
    }
    
    [Fact]
    public void IsActive_ShouldReturnFalse_WhenNotActive()
    {
        // Arrange
        var contract = new Contract(
            DateTime.Now.AddDays(-4),
            DateTime.Now.AddDays(-1),
            2,
            100m,
            null!,
            null!
        );

        // Act
        var isActive = contract.IsActive();

        // Act & Assert
        Assert.False(isActive);
    }
}