using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagerApi;
using TaskManagerApi.DataModels;
using TaskManagerApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TaskHandler>();
builder.Services.AddSingleton<AuthHandler>();
builder.Services.AddSingleton<ITaskDatabase, InMemoryTaskDb>();
builder.Services.AddSingleton<IUserDatabase, InMemoryUserDb>();

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    o.SaveToken = true;
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


//Task management endpoints
app.MapGet("/task/{name}",
(
    [FromRoute] string name,
    [FromServices] TaskHandler handler,
    ClaimsPrincipal principal
) => handler.Get(name, principal.GetUserIdFromClaims()));

app.MapGet("/tasks",
(
    [FromServices] TaskHandler handler,
    ClaimsPrincipal principal
) => handler.GetAll(principal.GetUserIdFromClaims()));

app.MapPost("/task",
(
    [FromBody] TaskItemDto task,
    [FromServices] TaskHandler handler,
    ClaimsPrincipal principal
) => handler.Create(task, principal.GetUserIdFromClaims()));


//Auth endpoints
app.MapPost("/auth/register", [AllowAnonymous]
(
    [FromBody] LoginDetails loginDetails,
    [FromServices] AuthHandler handler
) => handler.Register(loginDetails));

app.MapPost("/auth/getToken", [AllowAnonymous]
(
    [FromBody] LoginDetails loginDetails,
    [FromServices] AuthHandler handler
) => handler.GetToken(loginDetails.name, loginDetails.password, builder.Configuration));

app.Run();

public partial class Program
{ }


static class ClaimsExtensions
{
    public static Guid GetUserIdFromClaims(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.Claims.Single(x => x.Type == "Id").Value);
    }
}

