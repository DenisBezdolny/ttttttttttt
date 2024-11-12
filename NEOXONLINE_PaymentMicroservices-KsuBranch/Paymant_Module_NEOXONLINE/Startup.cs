using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Paymant_Module_NEOXONLINE.Contract.Exeptions;
using Payment.Application;
using Payment.Application.Payment_DAL.Contracts;
using Payment.Application.Payment_DAL.RealisationInterfaces;
using Serilog;
using System.Data.SqlClient;
using ILogger = Serilog.ILogger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Payment.BLL.Services.PayProduct;
using Payment.BLL.Contracts.PayProduct;
using Stripe;

namespace Paymant_Module_NEOXONLINE
{
    public class Startup
    {
        internal static void AddServices(WebApplicationBuilder builder)
        {
            AddSerilog(builder);
            RegisterDAL(builder.Services);

            var jwtIss = builder.Configuration.GetSection("Jwt:Iss").Get<string>();
            var jwtAud = builder.Configuration.GetSection("Jwt:Aud").Get<string>();
            var jwtKey = builder.Configuration.GetSection("Jwt:Secret").Get<string>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIss,
                        ValidAudience = jwtAud,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey))
                    };
                });
        }

        public static void ConfigureStripe(WebApplicationBuilder builder)
        {

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

        }

        public static void RegisterDAL(IServiceCollection services)
        {
            services.AddTransient<DbContextOptions<Payment_DbContext>>(provider =>
            {
                var builder = new DbContextOptionsBuilder<Payment_DbContext>();
                builder.UseNpgsql("host=localhost;port=5432;database=Paymant_Module_Db;Username=postgres;Password=KIDPay321");
                return builder.Options;
            });

            services.AddScoped<DbContext, Payment_DbContext>();

            services.AddScoped<IUnitOfWork>(prov =>
            {
                var context = prov.GetRequiredService<DbContext>();
                return new UnitOfWork(context);
            });
        }

        internal static void AddSerilog(WebApplicationBuilder builder)
        {
            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Month,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
            if (builder.Environment.IsDevelopment())
            {
                loggerConfig = loggerConfig.MinimumLevel.Debug();
            }
            else
            {
                loggerConfig = loggerConfig.MinimumLevel.Warning();
            }
            var logger = loggerConfig.CreateLogger();
            builder.Services.AddSingleton<ILogger>(logger);
        }

        private static bool TestConnection(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            var logger = provider.GetRequiredService<ILogger>();
            var context = provider.GetRequiredService<DbContext>();
            logger.Information("Testing the DB connection....");
            try
            {
                var createdAnew = context.Database.EnsureCreated();
                if (createdAnew)
                {
                    logger.Information("Successfully created the DB");
                }
                else
                {
                    logger.Information("The DB is already there");
                }
            }
            catch (Exception ex)
            {
                logger.Information("EnsureCreated failed");
                logger.Information(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
