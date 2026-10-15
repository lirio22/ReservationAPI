using System.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public ReservationService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<List<ReservationResponse>> GetAllReservations(int? roomId, string? responsible, string? date, string? orderBy, bool? descendingOrder, int page = 1, int pageSize = 10)
    {
        IQueryable<Reservation> reservations = _context.Reservations;
         
        if(roomId.HasValue)
            reservations = reservations.Where(x => x.RoomId == roomId.Value);

        if(!string.IsNullOrEmpty(responsible))
            reservations = reservations.Where(x => x.ResponsibleName == responsible);

        if(!string.IsNullOrEmpty(date))
            reservations = reservations.Where(x => x.StartAt.Date.ToString().Contains(date));

        if(!string.IsNullOrEmpty(orderBy))
        {
            switch(orderBy)
            {
                case "startAt":
                if(descendingOrder.HasValue && descendingOrder.Value)
                    reservations = reservations.OrderByDescending(x => x.StartAt);
                else
                    reservations = reservations.OrderBy(x => x.StartAt);
                break;

                case "title":
                if(descendingOrder.HasValue && descendingOrder.Value)
                    reservations = reservations.OrderByDescending(x => x.Title);
                else
                    reservations = reservations.OrderBy(x => x.Title);
                break;

                case "id":
                if(descendingOrder.HasValue && descendingOrder.Value)
                    reservations = reservations.OrderByDescending(x => x.Id);
                else
                    reservations = reservations.OrderBy(x => x.Id);
                break;
            }
        }

        int maxPageSize = _configuration.GetValue<int>("ReservationSettings:MaxPageSize");
        if(pageSize > maxPageSize)
            pageSize = maxPageSize;
        if(pageSize <= 0)
            pageSize = 10;
        if(page < 0)
            page = 1;

        reservations = reservations.Skip(page * pageSize - 1).Take(pageSize);

        return await reservations
        .AsNoTracking()
        .Select(x => new ReservationResponse
        {
            Title = x.Title,
            StartAt = x.StartAt,
            EndAt = x.EndAt,
            ResponsibleName = x.ResponsibleName,
            RoomId = x.RoomId,            
            RoomName = x.Room.Name,
            RoomCapacity = x.Room.Capacity
        }).ToListAsync();
    }

    public async Task<ReservationResponse?> GetReservationById(int id)
    {        
        Reservation? reservation = await _context.Reservations.AsNoTracking().Where(x => x.Id == id).Include(x => x.Room).FirstOrDefaultAsync();        

        if(reservation is null)
            return null;

        return ConvertoToReservationResponse(reservation);
    }

    public async Task<(ReservationStatusResponses, ReservationResponse?)> CreateNewReservation(ReservationCreateRequest request)
    {        
        Room? room = await _context.Rooms.FindAsync(request.RoomId);

        if(room is null)
            return (ReservationStatusResponses.RoomNotFound, null);
        
        if(!room.IsActive)
            return (ReservationStatusResponses.RoomInactive, null);

        if(request.EndAt < request.StartAt)
            return (ReservationStatusResponses.InvalidTimeInterval, null);

        if(room.Reservations.Any(x =>
        x.StartAt < request.EndAt && x.StartAt > request.StartAt || 
        x.EndAt > request.StartAt && x.EndAt < request.EndAt))
            return (ReservationStatusResponses.TimeAlreadyReserved, null);        
 
        Reservation reservation = new Reservation
        {
            Title = request.Title,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            ResponsibleName = request.ResponsibleName,
            RoomId = request.RoomId,
            Room = room
        };

        _context.Reservations.Add(reservation);
        
        await _context.SaveChangesAsync();       
        
        return (ReservationStatusResponses.Ok, ConvertoToReservationResponse(reservation));
    }

    public async Task<(ReservationStatusResponses, ReservationResponse?)> UpdateReservation(int id, ReservationUpdateRequest request)
    {
         Room? room = await _context.Rooms.FindAsync(request.RoomId);         

        if(room is null)
            return (ReservationStatusResponses.RoomNotFound, null);
        
        if(!room.IsActive)
            return (ReservationStatusResponses.RoomInactive, null);

        if(request.EndAt < request.StartAt)
            return (ReservationStatusResponses.InvalidTimeInterval, null);

        if(room.Reservations.Any(x =>
        x.StartAt < request.EndAt && x.StartAt > request.StartAt || 
        x.EndAt > request.StartAt && x.EndAt < request.EndAt))
            return (ReservationStatusResponses.TimeAlreadyReserved, null);       

        Reservation reservation = new Reservation
        {
            Title = request.Title,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            ResponsibleName = request.ResponsibleName,
            RoomId = request.RoomId,
            Room = room
        };

        _context.Reservations.Add(reservation);
        
        await _context.SaveChangesAsync();

        return (ReservationStatusResponses.Ok, ConvertoToReservationResponse(reservation));
    }

    public async Task<bool> DeleteReservation(int id)
    {
        Reservation? reservation = await _context.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync();

        if(reservation is null)
            return false;

        _context.Reservations.Remove(reservation);

        await _context.SaveChangesAsync();

        return true;
    }

    private ReservationResponse ConvertoToReservationResponse(Reservation reservation)
    {
        return new ReservationResponse
        {
            Title = reservation.Title,
            StartAt = reservation.StartAt,
            EndAt = reservation.EndAt,
            ResponsibleName = reservation.ResponsibleName,
            RoomId = reservation.RoomId,            
            RoomName = reservation.Room.Name,
            RoomCapacity = reservation.Room.Capacity
        };
    }
}