using Microsoft.EntityFrameworkCore;

public class RoomService : IRoomService
{
    public readonly AppDbContext _context;
    public RoomService(AppDbContext roomService)
    {
        _context = roomService;
    }

    public async Task<List<RoomResponse>> GetAllRooms()
    {
        return await _context.Rooms.AsNoTracking().Include(x => x.Reservations)
        .Select(x => new RoomResponse
        {
            Id = x.Id,
            Name = x.Name,
            Capacity = x.Capacity,
            IsActive = x.IsActive,
            Reservations = x.Reservations
        }).ToListAsync();
    }

    public async Task<RoomResponse?> GetRoomById(int id)
    {
        Room? room = await _context.Rooms.AsNoTracking().Include(x => x.Reservations)
        .Where(x => x.Id == id).FirstOrDefaultAsync();

        if(room is null)
            return null;

        return  ConvertToRoomResponse(room);
    }

    public async Task<RoomResponse?> CreateNewRoom(RoomCreateRequest request)
    {
        bool hasAny = await _context.Rooms.AnyAsync(x => x.Name == request.Name);
        if(hasAny)
            return null;
        
        Room room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            IsActive = request.IsActive
        };

        _context.Rooms.Add(room);

        await _context.SaveChangesAsync();

        return ConvertToRoomResponse(room);
    }

    public async Task<(string, RoomResponse?)> UpdateRoom(int id, RoomUpdateRequest request)
    {
        Room? room = await _context.Rooms.Include(x => x.Reservations)
        .Where(x => x.Id == id).FirstOrDefaultAsync();
        if(room is null)
            return ("Id not found", null);
        
        bool hasAny = await _context.Rooms.AnyAsync(x => x.Name == request.Name);
        if(hasAny)
            return ("Room name already exists!", null);

        room.Name = request.Name;
        room.Capacity = request.Capacity;
        room.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        return (string.Empty, ConvertToRoomResponse(room));
    }

    public async Task<bool?> DeleteRoom(int id)
    {
        Room? room = await _context.Rooms.Include(x => x.Reservations)
        .Where(x => x.Id == id).FirstOrDefaultAsync();
        if(room is null)
            return null;

        if(room.Reservations.Count() > 0)
            return false;

        _context.Rooms.Remove(room);

        await _context.SaveChangesAsync();

        return true;
    }

    private RoomResponse ConvertToRoomResponse(Room room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity,
            IsActive = room.IsActive,
            Reservations = room.Reservations
        };
    }
}