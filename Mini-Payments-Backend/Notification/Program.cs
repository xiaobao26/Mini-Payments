using MassTransit;
using Notification.Consumers;
using Notification.EmailService;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

// Bind SMTP settings
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("EmailSettings"));

// Register your EmailService
builder.Services.AddSingleton<IEmailService, EmailService>();

// MassTransit + RabbitMQ + Consumers
builder.Services.AddMassTransit(x =>
{
    // register consumers
    x.AddConsumer<SendAccountCreatedEmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        // broker connection
        cfg.Host(builder.Configuration["RabbitMq:Host"]!,
            h =>
            {
                h.Username(builder.Configuration["RabbitMq:Username"]!);
                h.Password(builder.Configuration["RabbitMq:Password"]!);
            });

        // define queue and wire consumers
        cfg.ReceiveEndpoint("mini-payment-app-email-notifications", e =>
        {
            e.ConfigureConsumer<SendAccountCreatedEmailConsumer>(context);

            // retry policy
            e.UseMessageRetry(r => r.Exponential(
                retryLimit:    5,
                minInterval:   TimeSpan.FromSeconds(1),
                maxInterval:   TimeSpan.FromSeconds(30),
                intervalDelta: TimeSpan.FromSeconds(2)
            ));
        });
    });
});

var host = builder.Build();
host.Run();
