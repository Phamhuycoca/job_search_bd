using job_search_be.Api.Infrastructure.Extensions;
using job_search_be.Application.Helpers;
using job_search_be.Application.Module;
using job_search_be.Domain.Entity;
using job_search_be.Infrastructure.Context;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationModules();

//Jwt
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Cấu hình bảo mật Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [Space] then your token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
            new string[]{}
        }
        });
});
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWTSettings"));
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddAuthorization();










//ConnectStrings
builder.Services.AddDbContext<job_search_DbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("Base_Context")));
var app = builder.Build();



// Seed data
/*using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<job_search_DbContext>();

        await dbContext.Database.MigrateAsync();

        if (!dbContext.Roles.Any())
        {
            List<Role> roles = new List<Role> 
            {
                new Role {RoleId=Guid.NewGuid(),NameRole="SuperAdmin"},
                new Role {RoleId=Guid.NewGuid(),NameRole="Nhân viên"},
                new Role {RoleId=Guid.NewGuid(),NameRole="Nhà tuyển dụng"},
                new Role {RoleId=Guid.NewGuid(),NameRole="Người dùng"},
            };
            await dbContext.Roles.AddRangeAsync(roles);
            await dbContext.SaveChangesAsync();
        }



        List<Permission> permissions = new List<Permission>
        {
            new Permission { PermissionId = Guid.NewGuid(), Name = "Read" },
            new Permission { PermissionId = Guid.NewGuid(), Name = "List" },
            new Permission { PermissionId = Guid.NewGuid(), Name = "Write" },
            new Permission { PermissionId = Guid.NewGuid(), Name = "Modify" },
            new Permission { PermissionId = Guid.NewGuid(), Name = "Delete" }
        };
        if (!dbContext.Permissions.Any())
        {
            await dbContext.Permissions.AddRangeAsync(permissions);
            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.Users.Any())
        {
            DateTime now = DateTime.Now;
            string password = "12345678a";

            await dbContext.Users.AddAsync(
                new User
                {
                    createdAt = DateTime.Now,
                    FullName = "Phạm Khắc Huy",
                    Email = "Phamkhachuy240702@gmail.com",
                    Gender = "Nam",
                    PassWord = PasswordHelper.CreateHashedPassword(password),
                    Address = "Hải phòng",
                    Avatar = "https://res.cloudinary.com/drhdgw1xx/image/upload/v1709880283/m76gmmyuzzv3phiyuy2u.jpg",
                    PhoneNumber = "0325472224",
                    Is_Active = false,
                }
            );
            await dbContext.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        throw new ApiException(400, $"An error occurred: {ex.Message}");
    }
}

*/


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionMiddleware();
app.UseAuthentication();
app.UseCors(builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});
app.UseAuthorization();
app.MapControllers();

app.Run();
