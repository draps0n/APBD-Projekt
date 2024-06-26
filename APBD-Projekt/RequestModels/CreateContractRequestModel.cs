using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.RequestModels;

public class CreateContractRequestModel
{
    [Required]
    public DateTime EndDate { get; set; }
    
    [Range(0, 3)]
    public int? YearsOfAdditionalSupport { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string SoftwareName { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string SoftwareVersion { get; set; }
}