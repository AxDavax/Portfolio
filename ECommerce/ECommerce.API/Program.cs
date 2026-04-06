using ECommerce.Application;
using ECommerce.Infrastructure;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWebApp", policy =>
    {
        policy.WithOrigins("https://localhost:7083")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:ApiKey"]!;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorWebApp");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();