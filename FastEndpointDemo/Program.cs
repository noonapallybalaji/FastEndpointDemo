using Microsoft.EntityFrameworkCore;
using Orders.Data.DataBaseContext;
using Orders.ExternalServices.Interfaces;
using Orders.ExternalServices.Services;
using Orders.Web.Filters;
using Orders.Web.Interfaces;
using Orders.Web.Repositories;
using Orders.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TimingLoggingFilter>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IAuditService, AuditService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }