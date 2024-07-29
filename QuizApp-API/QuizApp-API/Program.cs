using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Services;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp_API.DataAccess.Data;
using QuizApp_API.DataAccess.Repositories;
using QuizApp_API.DataAccess.Interfaces;
using QuizApp_API.BusinessLogic;

namespace QuizApp_API
{
    class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            ConfigureHttpRequest(app);
            app.Run();
        }

        private static void ConfigureHttpRequest(WebApplication app)
        {
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
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var identitiesDbConnectionString = builder.Configuration.GetConnectionString("QuizAppIdentitiesDb") 
                ?? throw new InvalidOperationException("Connection string 'QuizAppAPI_ContextConnection' not found.");
            var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb_ConnectionString") 
                ?? throw new InvalidOperationException("Connection string 'MongoDb_ConnectionString' not found.");
            var quizAppDbConnectionString = builder.Configuration.GetConnectionString("QuizAppDb") 
                ?? throw new InvalidOperationException("Connection string 'MongoDb_ConnectionString' not found.");

            // add db context for identities
            builder.Services.AddDbContext<AppIdentityContext>(options => options.UseNpgsql(identitiesDbConnectionString));
            builder.Services.AddDbContext<QuizAppDbContext>(options => options.UseNpgsql(quizAppDbConnectionString));
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppIdentityContext>();
            // add db context for other entities
            builder.Services.AddSingleton(new MongoDbContext(mongoConnectionString));

            builder.Services.AddScoped<IQuizzesRepository, QuizRepository>();
            builder.Services.AddScoped<IQuizzesService, QuizzesService>();
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            builder.Services.AddScoped<IUserProfilesService, UserProfilesService>();
            builder.Services.AddScoped<IQuizResultsRepository, QuizResultsRepository>();
            builder.Services.AddScoped<IQuizResultsService, QuizResultsService>();
            builder.Services.AddScoped<IRatesRepository, RatesRepository>();
            builder.Services.AddScoped<IRatesService, RatesService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:5173")
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]??string.Empty))
                };
            });

            builder.Services.AddScoped<IAuthorizer, JwtAuthorizer>();
            builder.Services.AddScoped<IUserService, UserService>();
        }
    }
}