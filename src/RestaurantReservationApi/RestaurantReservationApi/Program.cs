using RestaurantReservationApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Logging.AddConsole();
builder.Services.AddSingleton<IReservationRepository>(
    new SqlReservationRepository(
        builder.Configuration.GetConnectionString("RestaurantDbConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }

