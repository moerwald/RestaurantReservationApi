

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Logging.AddConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }

