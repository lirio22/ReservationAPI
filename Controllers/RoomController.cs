using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Patterns;

[ApiController]
[Route("/api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomResponse>>> GetAllRooms()
    {
        var rooms = await _roomService.GetAllRooms();

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomResponse?>> GetRoomById(int id)
    {
        RoomResponse? room = await _roomService.GetRoomById(id);

        if(room is null)
            return NotFound();

        return Ok(room);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<RoomResponse?>> CreateNewRoom(RoomCreateRequest request)
    {
        RoomResponse? room = await _roomService.CreateNewRoom(request);

        if(room is null)
            return BadRequest("Room name already exists!");
        
        return Created($"/api/room/{room.Id}", room);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<RoomResponse?>> UpdateRoom(int id, RoomUpdateRequest request)
    {
        (string message, RoomResponse? room) = await _roomService.UpdateRoom(id, request);

        if(room is null)
        {
            switch(message)
            {
                case "Id not found":
                    return NotFound();

                case "Room name already exists!":
                    return BadRequest(message);
            }
        }

        return Ok(room);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<bool?>> DeleteRoom(int id)
    {
        bool? result = await _roomService.DeleteRoom(id);

        if(result is null)
            return NotFound();

        if(result is false)
            return BadRequest("Room still have reservations. Remove all reservations before deleting.");
        
        return NoContent();
    }
}