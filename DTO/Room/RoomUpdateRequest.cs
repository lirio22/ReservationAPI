using System.ComponentModel.DataAnnotations;

public class RoomUpdateRequest
{
    [Required]
    [StringLength(maximumLength: 100, 
    ErrorMessage = "Name must be between 2 and 100 characters", 
    MinimumLength = 2)]
    public string Name {get; set;} = string.Empty;
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
    public int Capacity {get; set;}
    public bool IsActive {get; set;}
}