using Microsoft.EntityFrameworkCore;
using Mini_Payments_Backend.Models;
using Mini_Payments_Backend.Profiles;
using Mini_Payments_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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

builder.Services.AddAutoMapper(typeof(PaymentsMappingProfile));
builder.Services.AddScoped<IAccountService, AccountsService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
