using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prj3Database.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/api/TblWallet", ([FromServices] AppDbContext db) =>
{
    return Results.Ok(db.TblWallets.ToList());
});

app.MapGet("/api/TblWallet/{id}", ([FromServices] AppDbContext db, int id) =>
{
    var item = db.TblWallets.Where(x => x.WalletId == id).FirstOrDefault();
    if (item is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(item);
});

app.MapPost("/api/TblWaller/", ([FromServices] AppDbContext db, [FromBody] WalletRequestModel requestModel) =>
{
    var item = new TblWallet
    {
        Balance = 0,
        WalletUserName = requestModel.WalletUsername,
        FullName = requestModel.FullName,
        MobilNo = requestModel.MobileNo
    };
    db.Add(item);
    db.SaveChanges();
    return Results.Ok(item);
});

app.Run();

internal class WalletRequestModel
{
    public string WalletUsername { get; set; }
    public string FullName { get; set; }

    public string MobileNo { get; set; }

}
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


