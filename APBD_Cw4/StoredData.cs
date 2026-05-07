
namespace APBD_Cw4;

public static class StoredData
{
    public static List<Room> Rooms { get; set; } = new List<Room>()
    {
        new  Room{Id = 1, Name = "Sala A", BuildingCode = "A", Floor = 1, Capacity = 16, HasProjector = true, IsActive = true},
        new  Room{Id = 2, Name = "Sala B", BuildingCode = "A", Floor = 2, Capacity = 10, HasProjector = true, IsActive = true},
        new  Room{Id = 3, Name = "Sala C1", BuildingCode = "B", Floor = 3, Capacity = 20, HasProjector = false, IsActive = true},
        new  Room{Id = 4, Name = "Sala C2", BuildingCode = "B", Floor = 4, Capacity = 15, HasProjector = false, IsActive = false},
    };

    public static List<Reservation> Reservations { get; set; } = new List<Reservation>()
    {
        new Reservation
        {
            Id = 1, RoomId = 2, OrganizerName = "Jan Kowalski", Topic = "Spotkanie", Date = new DateTime(2026, 5, 4),
            StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 15), Status = "Confirmed"
        },
        new Reservation
        {
            Id = 2, RoomId = 1, OrganizerName = "Anna Nowak", Topic = "Warsztaty", Date = new DateTime(2026, 6, 20),
            StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 0), Status = "Planned"
        }
    };
}