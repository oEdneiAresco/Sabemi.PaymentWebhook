using Sabemi.PaymentWebhook.Application;
using Sabemi.PaymentWebhook.Api.Middleware;
using Microsoft.EntityFrameworkCore;
using Sabemi.PaymentWebhook.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PaymentWebhookDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionHandlingMiddleware>(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
