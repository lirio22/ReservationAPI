public interface IRoomService
{
    Task<List<RoomResponse>> GetAllRooms();
    Task<RoomResponse?> GetRoomById (int id);
    Task<RoomResponse?> CreateNewRoom (RoomCreateRequest request);
    Task<(string, RoomResponse?)> UpdateRoom (int id, RoomUpdateRequest request);
    Task<bool?> DeleteRoom (int id);
}