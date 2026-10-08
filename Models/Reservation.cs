public class Reservation
{
    public int Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public DateTime StartAt {get; set;}
    public DateTime EndAt {get; set;}
    public string ResponsibleName {get; set;} = string.Empty;
    public int RoomId {get; set;}
    public Room Room {get; set;} = new Room();
}