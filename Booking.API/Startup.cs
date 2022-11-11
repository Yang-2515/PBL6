using Booking.API.Extensions;
using Booking.Domain.Interfaces.Repositories;
using Booking.Infrastructure.Data;
using Booking.Infrastructure.Data.Repositories;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Booking.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"])),

                };
            });
            services.AddHttpContextAccessor();

            services.AddDbContext(Configuration);
            services.AddGenericRepositories();
            services.AddServices();
            services.AddUnitOfWork();

            services.RegisterRabbitMQ(Configuration);
            services.RegisterEventBus();
            services.RegisterMediator();
        }

        public void ConfigureEventBus(WebApplication app)
        {
            var eventBus = app.Services.GetRequiredService<IEventBus>();

            //eventBus.Subscribe<UserCreatedIntergrationEvent, IIntegrationEventHandler<UserCreatedIntergrationEvent>>();
            //eventBus.Subscribe<UserUpdatedIntergrationEvent, IIntegrationEventHandler<UserUpdatedIntergrationEvent>>();
        }
    }
}
