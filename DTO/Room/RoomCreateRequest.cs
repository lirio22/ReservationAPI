using System.ComponentModel.DataAnnotations;

public class RoomCreateRequest
{
    [Required]
    [StringLength(maximumLength: 100, 
    ErrorMessage = "Name must be between 2 and 100 characters", 
    MinimumLength = 2)]
    public string Name {get; set;} = string.Empty;
    [Required]
    public int Capacity {get; set;}
    public bool IsActive {get; set;}
}