using Booking.API.Extensions;
using Booking.Domain.Interfaces.Repositories;
using Booking.Infrastructure.Data;
using Booking.Infrastructure.Data.Repositories;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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
