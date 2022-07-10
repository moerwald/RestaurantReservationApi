using RestaurantReservationApi;
using RestaurantReservationApi.Controllers.Reservation;
using System.Data.SqlClient;

internal class SqlReservationRepository : IReservationRepository
{
    private string _connectionString;
    private readonly ILogger<SqlReservationRepository> _logger;

    public SqlReservationRepository(string connectionString, ILogger<SqlReservationRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task Create(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation, nameof(reservation));

        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(_createNewEntry, connection);

        cmd.Parameters.Add(new SqlParameter("@At", reservation.At));
        cmd.Parameters.Add(new SqlParameter("@Name", reservation.Name));
        cmd.Parameters.Add(new SqlParameter("@Email", reservation.Email));
        cmd.Parameters.Add(new SqlParameter("@Quantity", reservation.Quantity));


        try
        {
            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred");
        }
    }

    private const string _createNewEntry = @"
        INSERT INTO
            [dbo].[Reservations] ([At], [Name], [Email], [Quantity])
            VALUES (@At, @Name, @Email, @Quantity) ";
}