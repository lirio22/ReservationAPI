using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface IReservationService
{
    Task<List<ReservationResponse>> GetAllReservations(int? roomId, string? responsible, string? date, string? orderBy, bool? decrescentOrder, int page, int pageSize);
    Task<ReservationResponse?> GetReservationById(int id);
    Task<(ReservationStatusResponses,ReservationResponse?)> CreateNewReservation(ReservationCreateRequest request);
    Task<(ReservationStatusResponses, ReservationResponse?)> UpdateReservation(int id, ReservationUpdateRequest request);
    Task<bool> DeleteReservation(int id);
}