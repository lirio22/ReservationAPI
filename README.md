# ReservationAPI

A RESTful API built with ASP.NET Core for managing rooms and room reservations.

The project focuses on backend development concepts such as relational data modeling, asynchronous database queries, filtering, sorting, pagination, authentication, authorization, and business-rule validation.

One of the main business rules implemented by the API is preventing overlapping reservations for the same room.

## Features

- Room management
- Reservation management
- Create, read, update, and delete operations
- Room availability validation
- Reservation time-conflict detection
- Filter reservations by room
- Filter reservations by responsible person
- Filter reservations by date
- Sort reservation results
- Pagination
- DTOs for API requests and responses
- Data validation
- JWT authentication
- Authorization policies
- Custom middleware
- Entity Framework Core migrations

## Tech Stack

- C#
- .NET / ASP.NET Core Web API
- Entity Framework Core
- LINQ
- SQLite
- JWT Authentication
- Dependency Injection
- REST

## Data Model

The application contains two main entities:

### Room

Represents a room that can be reserved.

A room can have multiple reservations.

### Reservation

Represents a reservation for a specific room and time period.

The relationship can be represented as:

```text
Room
 │
 └── 1 : N
        │
     Reservation
```

## Reservation Conflict Validation

The API prevents two reservations from occupying overlapping time periods in the same room.

Before creating or updating a reservation, the application checks the database for an existing reservation whose time interval conflicts with the requested interval.

This validation is performed asynchronously using Entity Framework Core queries.

Conceptually, two reservations conflict when:

```text
newStart < existingEnd
AND
newEnd > existingStart
```

If an overlapping reservation already exists for the room, the new reservation is rejected.

## Filtering, Sorting and Pagination

Reservation queries support multiple optional parameters, including:

- Room
- Responsible person
- Date
- Sorting
- Ascending or descending order
- Page
- Page size

The query is built dynamically using LINQ before being executed by Entity Framework Core.

## Authentication and Authorization

The application uses JWT authentication.

Authorization policies are used to protect restricted operations and control access to administrative functionality.

## Database

The project uses SQLite with Entity Framework Core.

Database schema changes are managed through EF Core migrations.

To create or update the database:

```bash
dotnet ef database update
```

## Running the Project

Clone the repository:

```bash
git clone https://github.com/lirio22/ReservationAPI.git
```

Enter the project directory:

```bash
cd ReservationAPI
```

Restore the dependencies:

```bash
dotnet restore
```

Apply the database migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

## What I Practiced

This project was created to practice and consolidate backend development concepts including:

- REST API design
- ASP.NET Core controllers
- Services and dependency injection
- DTO design
- Input validation
- Entity Framework Core
- LINQ
- Relational database modeling
- Asynchronous database operations
- Filtering and dynamic queries
- Sorting
- Pagination
- Business-rule validation
- Reservation time-conflict detection
- Authentication and authorization
- Middleware
- Database migrations

## Project Status

Completed as a backend development portfolio and study project.

## API Endpoints

### Reservations

| Method | Endpoint | Description | Authorization |
|---|---|---|---|
| `GET` | `/api/reservations` | Returns reservations with optional filtering, sorting, and pagination | Public |
| `GET` | `/api/reservations/{id}` | Returns a reservation by ID | Public |
| `POST` | `/api/reservations` | Creates a new reservation | Authenticated user |
| `PUT` | `/api/reservations/{id}` | Updates an existing reservation | Authenticated user |
| `DELETE` | `/api/reservations/{id}` | Deletes a reservation | Admin only |

#### Reservation Query Parameters

`GET /api/reservations` supports the following optional query parameters:

| Parameter | Type | Description |
|---|---|---|
| `roomId` | `int` | Filters reservations by room |
| `responsible` | `string` | Filters reservations by responsible person's name |
| `date` | `string` | Filters reservations by date |
| `orderBy` | `string` | Selects the field used for sorting |
| `descendingOrder` | `bool` | Uses descending sort order when `true` |
| `page` | `int` | Page number. Default: `1` |
| `pageSize` | `int` | Number of results per page. Default: `10` |

Example:

```http
GET /api/reservations?roomId=2&responsible=Ana&orderBy=startAt&descendingOrder=false&page=1&pageSize=10
```

When creating or updating a reservation, the API validates the requested time interval and prevents overlapping reservations for the same room.

Possible business-rule errors include:

- Room not found
- Room inactive
- Invalid time interval
- Requested time already reserved

### Rooms

| Method | Endpoint | Description | Authorization |
|---|---|---|---|
| `GET` | `/api/rooms` | Returns all rooms | Public |
| `GET` | `/api/rooms/{id}` | Returns a room by ID | Public |
| `POST` | `/api/rooms` | Creates a new room | Admin only |
| `PUT` | `/api/rooms/{id}` | Updates an existing room | Admin only |
| `DELETE` | `/api/rooms/{id}` | Deletes a room | Admin only |

Room names must be unique.

A room cannot be deleted while reservations associated with it still exist.

### Common Responses

- `200 OK` — Request completed successfully
- `201 Created` — Resource created successfully
- `204 No Content` — Resource deleted successfully
- `400 Bad Request` — Invalid request or business-rule violation
- `401 Unauthorized` — Authentication is required
- `403 Forbidden` — The authenticated user does not have permission
- `404 Not Found` — Requested reservation or room was not found
