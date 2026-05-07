using Microsoft.AspNetCore.Mvc;

namespace APBD_Cw4;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase{
    [HttpGet]
    public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
       var query = StoredData.Rooms.AsQueryable();
       
       if (minCapacity.HasValue)
           query = query.Where(r => r.Capacity >= minCapacity.Value);
        
       if (hasProjector.HasValue)
           query = query.Where(r => r.HasProjector == hasProjector.Value);

       if (activeOnly.HasValue && activeOnly.Value)
           query = query.Where(r => r.IsActive);

       return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetRoom(int id)
    {
        var room = StoredData.Rooms.FirstOrDefault(r => r.Id == id);
        if(room == null) return NotFound($"Sala o ID: {id} nie zostala znaleziona.");
        
        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomByBuilding([FromRoute] string buildingCode)
    {
        var rooms = StoredData.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
        
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult CreateRoom([FromBody] Room newRoom)
    {
        newRoom.Id = StoredData.Rooms.Count > 0 ? StoredData.Rooms.Max(r => r.Id) + 1 : 1;
        StoredData.Rooms.Add(newRoom);
        
        return CreatedAtAction(nameof(GetRoom), new { id = newRoom.Id }, newRoom);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
    {
        var room = StoredData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound($"Sala o ID: {id} nie zostala znaleziona.");
        
        room.Name = updatedRoom.Name;
        room.BuildingCode = updatedRoom.BuildingCode;
        room.Floor =  updatedRoom.Floor;
        room.Capacity =  updatedRoom.Capacity;
        room.HasProjector = updatedRoom.HasProjector;
        room.IsActive = updatedRoom.IsActive;
        
        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = StoredData.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound($"Sala o ID: {id} nie zostala znaleziona.");
        
        bool hasReservations = StoredData.Reservations.Any(r => r.RoomId == id);
        if (hasReservations)
        {
            Conflict("Nie mozna usunac sali, do ktorej przypisane sa rezerwacjie.");
        }
        StoredData.Rooms.Remove(room);
        return NoContent();
    }
    
}