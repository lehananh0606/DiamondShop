using DiamondShopSystem.Constants;
using DiamondShopSystem.Contants;
using DiamondShopSystem.Extensions;
using DiamondShopSystem.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl.Matchers;
using Service.Commons;
using Service.IServices;
using Service.Quartz;
using Service.Services;
using Service.Validate;
using Service.ViewModels.AcountToken;
using Service.ViewModels.Request;
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
            builder.AddJwtValidation();

            // Dependency Injection
            builder.Services.Configure<JWTAuth>(builder.Configuration.GetSection("JWTAuth"));
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
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
            });

            // Fluent Validation
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AccountRequestValidator>());
            builder.Services.AddValidatorsFromAssemblyContaining<AccountTokenValidator>();


            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.AddJobAndTrigger<HelloJob>(builder.Configuration);
                q.AddJobAndTrigger<EveryDay1AmJob>(builder.Configuration);
                q.AddJobAndTrigger<MonthlyAt1AMOn1st>(builder.Configuration);
                q.AddJobAndTrigger<EveryMinute>(builder.Configuration);


            });
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.AddApplicationConfig();
            app.Run();
        }
    }
}
