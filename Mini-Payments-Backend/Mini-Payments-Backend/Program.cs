using Microsoft.EntityFrameworkCore;
using Mini_Payments_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext Service with Npgsql in Container
builder.Services.AddDbContext<PaymentsContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.SetPostgresVersion(new Version(14, 0));
        }
    );
});


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
