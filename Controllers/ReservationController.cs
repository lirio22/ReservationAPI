using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }        

    [HttpGet]
    public async Task<ActionResult<List<ReservationResponse>>> GetAllReservations(int? roomId, string? responsible, string? date, string? orderBy, bool? descendingOrder, int page, int pageSize)
    {
        var reservations = await _reservationService.GetAllReservations(roomId, responsible, date, orderBy, descendingOrder, page, pageSize);

        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReservationResponse?>> GetReservationById(int id)
    {
        ReservationResponse? reservation = await _reservationService.GetReservationById(id);

        if(reservation is null)
            return NotFound();

        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<(ReservationStatusResponses, ReservationResponse?)>> CreateNewReservation(ReservationCreateRequest request)
    {
        (ReservationStatusResponses statusResponse, ReservationResponse? reservationResponse) = await _reservationService.CreateNewReservation(request);

        if(reservationResponse is null)
        {
            switch(statusResponse)
            {
                case ReservationStatusResponses.RoomNotFound:
                return NotFound("Room not found");

                case ReservationStatusResponses.RoomInactive:
                return BadRequest("Room Inactive");

                case ReservationStatusResponses.InvalidTimeInterval:
                return BadRequest("Invalide time interval");

                case ReservationStatusResponses.TimeAlreadyReserved:
                return BadRequest("Hours already reserved");            

                default:
                return BadRequest();
            }
        }

            return Created($"/api/reservations/{reservationResponse.Id}", reservationResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<(ReservationStatusResponses, ReservationResponse?)>> UpdateReservation(int id, ReservationUpdateRequest request)
    {
        (ReservationStatusResponses statusResponse, ReservationResponse? reservationResponse) = await _reservationService.UpdateReservation(id, request);

        if(reservationResponse is null)
            return NotFound("Reservation not found");

        switch(statusResponse)
        {
            case ReservationStatusResponses.RoomNotFound:
            return NotFound("Room not found");

            case ReservationStatusResponses.RoomInactive:
            return BadRequest("Room Inactive");

            case ReservationStatusResponses.InvalidTimeInterval:
            return BadRequest("Invalide time interval");

            case ReservationStatusResponses.TimeAlreadyReserved:
            return BadRequest("Hours already reserved");

            case ReservationStatusResponses.Ok:
            return Ok(reservationResponse);

            default:
            return BadRequest();
        }
    } 

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeleteReservation(int id)
    {
        bool deleted = await _reservationService.DeleteReservation(id);

        if(!deleted)
            return NotFound();
        
        return NoContent();
    }
}