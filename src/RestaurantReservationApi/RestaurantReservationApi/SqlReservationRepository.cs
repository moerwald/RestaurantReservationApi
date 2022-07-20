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

    public async Task CreateAsync(Reservation reservation)
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

    public async Task<IEnumerable<Reservation>> ReadReservationsAsync(DateTime dateTime)
    {
        var result = new List<Reservation>();
        
        var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(_getReservationBasedOnDate, connection);
        
        cmd.Parameters.Add(new SqlParameter("@At", dateTime.Date ));
        
        try
        {
            await connection.OpenAsync();
            await using var rdr = await cmd.ExecuteReaderAsync();

            while (await rdr.ReadAsync())
            {
                result.Add(
                    new Reservation(
                        (string) rdr["Name"],
                        (DateTime) rdr["At"],
                        (string) rdr["Email"],
                        (int) rdr["Quantity"] ));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred");
        }

        return result;
    }

    private const string _createNewEntry = @"
        INSERT INTO
            [dbo].[Reservations] ([At], [Name], [Email], [Quantity])
            VALUES (@At, @Name, @Email, @Quantity) ";

    private const string _getReservationBasedOnDate = @"
        SELECT *
        FROM [RestaurantReservationDb].[dbo].[Reservations]
        WHERE CAST([At] AS DATE) = CAST(@At AS DATE)
        ";
}