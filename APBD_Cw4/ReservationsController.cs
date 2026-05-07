using Microsoft.AspNetCore.Mvc;

namespace APBD_Cw4;

[ApiController]
[Route("[api/controller]")]
public class ReservationsController : ControllerBase
{

    [HttpGet]
    public IActionResult GetReservations([FromQuery] DateTime? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var query = StoredData.Reservations.AsQueryable();

        if (date.HasValue)
        
            query = query.Where(r => r.Date.Date == date.Value.Date);
        
        
        if(!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status != null && r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        
        if(roomId.HasValue)
            query = query.Where(r => r.RoomId == roomId.Value);
        
        return Ok(query.ToList());
    }
    
    [HttpGet("{id}")]
    public IActionResult GetReservation(int id)
    {
        var reservation = StoredData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();

        return Ok(reservation);
    }
    
    [HttpPost]
    public IActionResult CreateReservation([FromBody] Reservation newReservation)
    {
        if (newReservation.EndTime <= newReservation.StartTime)
        {
            ModelState.AddModelError(nameof(newReservation.EndTime), "Czas zakończenia musi byc pozniejszy niż czas rozpoczęcia.");
            return BadRequest(ModelState);
        }

        var room = StoredData.Rooms.FirstOrDefault(r => r.Id == newReservation.RoomId);
        if (room == null) return NotFound("Sala nie istnieje.");
        if (!room.IsActive) return BadRequest("Nie można zarezerwować nieaktywnej sali.");
        
        bool isConflict = StoredData.Reservations.Any(r => 
            r.RoomId == newReservation.RoomId && 
            r.Date.Date == newReservation.Date.Date &&
            ((newReservation.StartTime >= r.StartTime && newReservation.StartTime < r.EndTime) || 
             (newReservation.EndTime > r.StartTime && newReservation.EndTime <= r.EndTime) || 
             (newReservation.StartTime <= r.StartTime && newReservation.EndTime >= r.EndTime))
        );

        if (isConflict) return Conflict("Sala jest już zarezerwowana w tym przedziale czasowym.");

        newReservation.Id = StoredData.Reservations.Count > 0 ? StoredData.Reservations.Max(r => r.Id) + 1 : 1;
        StoredData.Reservations.Add(newReservation);

        return CreatedAtAction(nameof(GetReservation), new { id = newReservation.Id }, newReservation);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
    {
        if (updatedReservation.EndTime <= updatedReservation.StartTime)
        {
            ModelState.AddModelError(nameof(updatedReservation.EndTime), "Czas zakończenia musi być późniejszy niż czas rozpoczęcia.");
            return BadRequest(ModelState);
        }

        var reservation = StoredData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();

        var room = StoredData.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        if (room == null) return NotFound("Wskazana sala nie istnieje.");
        if (!room.IsActive) return BadRequest("Nie można przypisać do nieaktywnej sali.");

        bool isConflict = StoredData.Reservations.Any(r => 
            r.RoomId == updatedReservation.RoomId && 
            r.Date.Date == updatedReservation.Date.Date &&
            r.Id != id &&
            ((updatedReservation.StartTime >= r.StartTime && updatedReservation.StartTime < r.EndTime) || 
             (updatedReservation.EndTime > r.StartTime && updatedReservation.EndTime <= r.EndTime) || 
             (updatedReservation.StartTime <= r.StartTime && updatedReservation.EndTime >= r.EndTime))
        );

        if (isConflict) return Conflict("Aktualizacja spowoduje konflikt czasowy z inna rezerwacja.");

        reservation.RoomId = updatedReservation.RoomId;
        reservation.OrganizerName = updatedReservation.OrganizerName;
        reservation.Topic = updatedReservation.Topic;
        reservation.Date = updatedReservation.Date;
        reservation.StartTime = updatedReservation.StartTime;
        reservation.EndTime = updatedReservation.EndTime;
        reservation.Status = updatedReservation.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = StoredData.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();
        
        StoredData.Reservations.Remove(reservation);
        return NoContent();
    }
}