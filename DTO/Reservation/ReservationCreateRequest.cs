using System.ComponentModel.DataAnnotations;

public class ReservationCreateRequest
{
    [Required]
    [StringLength(150, ErrorMessage = "Title must be between 2 and 150 characters", MinimumLength = 2)]
    public string Title {get; set;} = string.Empty;
    [Required]
    public DateTime StartAt {get; set;}
    [Required]
    public DateTime EndAt {get; set;}
    [Required]
    [StringLength(100, ErrorMessage = "Responsible Name must be between 2 and 100 characters", MinimumLength = 2)]
    public string ResponsibleName {get; set;} = string.Empty;
    [Required]
    public int RoomId {get; set;}
}