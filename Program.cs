using EmailScheduler.Model.Helper;
using EmailScheduler.Service;
using EmailScheduler.Service.Interface;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddHostedService<EmailScheduler.EmailScheduler>();
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("EmailSettings"));

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
