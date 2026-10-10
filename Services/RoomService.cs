public class RoomService : IRoomService
{
    public async Task<List<RoomResponse>> GetAllRooms()
    {
        throw new NotImplementedException();
    }

    public async Task<RoomResponse?> GetRoomById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<RoomResponse?> CreateNewRoom(RoomCreateRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<RoomResponse?> UpdateRoom(int id, RoomUpdateRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<bool?> DeleteRoom(int id)
    {
        throw new NotImplementedException();
    }
}