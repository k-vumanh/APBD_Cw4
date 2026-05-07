using System.ComponentModel.DataAnnotations;

namespace APBD_Cw4;

public class Reservation
{
    public int Id { get; set; }
    
    public int RoomId { get; set; }
    
    [Required(ErrorMessage = "Nazwa organizatora jest wymagana.")]
    public string OrganizerName { get; set; }
    
    [Required(ErrorMessage = "Temat jest wymagany.")]
    public string Topic  { get; set; }
    
    public DateTime Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    
    public string Status { get; set; }


}