using System.Drawing;
using Microsoft.AspNetCore.Mvc;

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
        throw new NotImplementedException();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomResponse?>> GetRoomById(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<RoomResponse?>> CreateNewRoom(RoomCreateRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RoomResponse?>> UpdateRoom(int id, RoomUpdateRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool?>> DeleteRoom(int id)
    {
        throw new NotImplementedException();
    }
}