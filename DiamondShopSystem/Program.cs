using DiamondShopSystem.Constants;
using DiamondShopSystem.Contants;
using DiamondShopSystem.Extensions;
using DiamondShopSystem.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.Commons;
using Service.IServices;
using Service.Services;
using Service.Validate;
using Service.ViewModels.AcountToken;
using ShopRepository.Models;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Repositories.Repository;
using ShopRepository.Repositories.UnitOfWork;
using System.Reflection;
using System.Text;

namespace DiamondShopSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(opts
                => opts.SuppressModelStateInvalidFilter = true);
            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddConfigSwagger();

            // JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JWTAuth");
            builder.Services.Configure<JWTAuth>(jwtSettings);

            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Dependency Injection
            builder.Services.AddUnitOfWork();
            builder.Services.AddServices();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddAutoMapper(typeof(AutoMapperService));
            builder.Services.AddExceptionMiddleware();

            // CORS Policy
            builder.Services.AddCors(cors => cors.AddPolicy(
                name: CorsConstant.PolicyName,
                policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }));

            // Add DbContext with SQL Server configuration
            builder.Services.AddDbContext<DiamondShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));

            // Fluent Validation
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AccountRequestValidator>());
            builder.Services.AddValidatorsFromAssemblyContaining<AccountTokenValidator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(CorsConstant.PolicyName);
            app.UseRouting(); // Must be placed before Authentication & Authorization
            app.UseAuthentication(); // Must be placed before Authorization
            app.UseAuthorization(); // Must be placed after Authentication
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
