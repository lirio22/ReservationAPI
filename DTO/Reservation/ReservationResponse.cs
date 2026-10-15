public class ReservationResponse
{
    public int Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public DateTime StartAt {get; set;}
    public DateTime EndAt {get; set;}
    public string ResponsibleName {get; set;} = string.Empty;
    public int RoomId {get; set;}
    public string RoomName {get; set;} = string.Empty;
    public int RoomCapacity{get; set;}
}