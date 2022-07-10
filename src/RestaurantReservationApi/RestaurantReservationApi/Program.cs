using RestaurantReservationApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Logging.AddConsole();
builder.Services.AddSingleton<IReservationRepository>(sp =>
{
    var cs = builder.Configuration.GetConnectionString("RestaurantDbConnection");
    var logger = sp.GetService<ILogger<SqlReservationRepository>>();
    return new SqlReservationRepository(cs, logger!);

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }

