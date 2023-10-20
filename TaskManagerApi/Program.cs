using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using TaskManagerApi;
using TaskManagerApi.DataModels;
using TaskManagerApi.Extensions;
using TaskManagerApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TaskHandler>();
builder.Services.AddSingleton<AuthHandler>();
builder.Services.AddSingleton(x => new SqlConnection(builder.Configuration["dbConnectionString"]));
builder.Services.AddSingleton<PasswordHasher<User>>();
builder.Services.AddSingleton<ITaskDatabase, TaskDatabase>();
builder.Services.AddSingleton<IUserDatabase, UserDatabase>();

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

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder => 
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


//Task management endpoints
app.MapGet("/task/{name}", [Authorize]
(
    [FromRoute] string name,
    [FromServices] TaskHandler handler,
    ClaimsPrincipal principal
) => handler.Get(name, principal.GetUserIdFromClaims()));

app.MapGet("/tasks", [Authorize]
(
    [FromServices] TaskHandler handler,
    [FromQuery] bool? isComplete,
    ClaimsPrincipal principal
) => handler.GetAll(principal.GetUserIdFromClaims(), isComplete ?? false));

app.MapPost("/task", [Authorize]
(
    [FromBody] TaskItemDto task,
    [FromServices] TaskHandler handler,
    ClaimsPrincipal principal
) => handler.Create(task, principal.GetUserIdFromClaims()));

app.MapPut("/task/{name}", [Authorize]
(
    [FromRoute] string name,
    [FromBody] TaskItemDto task,
    [FromServices] TaskHandler handler,
    ClaimsPrincipal principal
) => handler.Update(name, task, principal.GetUserIdFromClaims()));

app.MapDelete("/task/{name}", [Authorize]
(
    [FromRoute] string name,
    [FromServices] TaskHandler handler,
    ClaimsPrincipal principal
) => handler.Delete(name, principal.GetUserIdFromClaims()));


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
) => handler.GetToken(loginDetails, builder.Configuration));

app.Run();

public partial class Program
{ }


