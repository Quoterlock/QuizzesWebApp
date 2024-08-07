using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Services;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Repositories;
using QuizApp_API.DataAccess.Interfaces;
using QuizApp_API.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("QuizAppAPI_ContextConnection") ?? throw new InvalidOperationException("Connection string 'QuizAppAPI_ContextConnection' not found.");
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb_ConnectionString") ?? throw new InvalidOperationException("Connection string 'MongoDb_ConnectionString' not found.");

// add db context for identities
builder.Services.AddDbContext<AppIdentityContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppIdentityContext>();
// add db context for other entities
builder.Services.AddSingleton(new MongoDbContext(mongoConnectionString));

builder.Services.AddScoped<IQuizzesRepository, QuizRepository>();
builder.Services.AddScoped<IQuizzesService, QuizzesService>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserProfilesService, UserProfilesService>();
builder.Services.AddScoped<IQuizResultsRepository, QuizResultsRepository>();
builder.Services.AddScoped<IQuizResultsService, QuizResultsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                          //.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddScoped<IAuthorizer, JwtAuthorizer>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
