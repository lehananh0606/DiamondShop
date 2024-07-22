﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

using ShopRepository.Repositories.UnitOfWork;
using ShopRepository.Repositories.IRepository;
using ShopRepository.Repositories.Repository;
using Service.Commons;
using Service.Utils;
using DiamondShopSystem.Middleware;
using DiamondShopSystem.Contants;
using Service.IServices;
using Service.Services;
using ShopRepository.Models;


namespace DiamondShopSystem.Extensions
{
    public static class DependencyExtention
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbFactory, DbFactory>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped(typeof(IFirebaseService<>), typeof(FirebaseService<>));
            services.AddScoped<IFirebaseService<Auction>, FirebaseService<Auction>>();
            return services;
        }

        public static void AddJwtValidation(this WebApplicationBuilder builder)
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWTAuth:Key"]);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Message = JsonConvert.DeserializeObject<List<ErrorDetail>>(ErrorUtil.GetErrorString("Unauthorized", "You are not allowed to access this API."))
                        });
                    }
                };
            });
        
    }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Diamond Shop API",
                    Description = "The Diamond Shop API API is built for the Bidding Diamond."
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            return services;
        }

        public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services)
        {
            services.AddTransient<ExceptionMiddleware>();
            services.AddTransient<RequestDelegate>(serviceProvider =>
            {
                // Get the next middleware in the pipeline
                var next = serviceProvider.GetRequiredService<RequestDelegate>();
                return async context =>
                {
                    // Call the next middleware in the pipeline
                    await next(context);
                };
            });
            return services;
        }

        public static WebApplication AddApplicationConfig(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(CorsConstant.PolicyName);
            app.UseAuthentication();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthorization();
            //Add middleware extentions
            app.ConfigureExceptionMiddleware();
            app.MapControllers();
            app.UseRouting();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new HangfireAuthenticationFilter() }
            //});
            //BackgroundJob.Enqueue<IConfigurationService>(cf => cf.StartAllBackgroundJob());
            return app;
        }
    }
}
