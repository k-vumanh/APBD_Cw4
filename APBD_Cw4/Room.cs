using System.ComponentModel.DataAnnotations;

namespace APBD_Cw4;

public class Room
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Nazwa sali jest wymagana.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Kod budynku jest wymagany.")]
    public string BuildingCode { get; set; }
    
    public int Floor { get; set; }
    
    [Range(1,int.MaxValue, ErrorMessage = "Pojemność musi być większa od zera")]
    public int Capacity { get; set; }
    
    public bool HasProjector{ get; set; }
    
    public bool IsActive { get; set; }
}