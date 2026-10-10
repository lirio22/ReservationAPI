public interface IRoomService
{
    Task<List<RoomResponse>> GetAllRooms();
    Task<RoomResponse?> GetRoomById (int id);
    Task<RoomResponse?> CreateNewRoom (RoomCreateRequest request);
    Task<RoomResponse?> UpdateRoom (int id, RoomUpdateRequest request);
    Task<bool?> DeleteRoom (int id);
}