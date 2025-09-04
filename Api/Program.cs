using HouseManagementApi.Data;
using HouseManagementApi.Middleware;
using HouseManagementApi.Services.PasswordHasher;
using HouseManagementApi.Services.User;
using Microsoft.EntityFrameworkCore;

// TODO: add logging

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

DotNetEnv.Env.Load();

builder.Services.AddDbContextPool<ApiDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
);

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// if (!app.Environment.IsDevelopment())
if (app.Environment.IsDevelopment()) // delete this line and uncomment above later
{
    app.UseExceptionHandler("/api/error");
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
