using Booking.API.Services;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories;
using Booking.Infrastructure.Data;
using Booking.Infrastructure.Data.Repositories;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Reflection;

namespace Booking.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddImplementationsAsInterfaces(this IServiceCollection services
            , Type interfaceType
            , params Type[] implementationAssemblyTypes)
        {
            foreach (var assemblyType in implementationAssemblyTypes)
            {
                var implementationTypes = Assembly
                    .GetAssembly(assemblyType)
                    .GetTypes()
                    .Where(_ =>
                        _.IsClass
                        && !_.IsAbstract
                        && !_.IsInterface
                        && _.GetInterface(interfaceType.Name) != null
                        && !_.IsGenericType
                    );

                foreach (var implementationType in implementationTypes)
                {
                    var mainInterfaces = implementationType
                        .GetInterfaces()
                        .Where(_ => _.GenericTypeArguments.Count() == 0);

                    foreach (var mainInterface in mainInterfaces)
                    {
                        services.AddScoped(mainInterface, implementationType);
                    }
                }
            }
            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetSection("ConnectionString").Value);
            });
            return services;
        }

        public static IServiceCollection AddGenericRepositories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddImplementationsAsInterfaces(
                    typeof(IGenericRepository<>)
                    , typeof(GenericRepository<>)
                );
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<LocationService>();
            services.AddScoped<BookingService>();
            services.AddScoped<RoomService>();
            return services;
        }


        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection RegisterRabbitMQ(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory();
                factory.Uri = new Uri(Configuration.GetSection("RabbitMQConnectionString").Value);

                var retryCount = Int32.Parse(Configuration.GetSection("EventBusRetryCount").Value);

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            return services;
        }
        public static IServiceCollection RegisterMediator(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddMediatR(typeof(CreateUserDomainEvent).GetTypeInfo().Assembly);
            return services;
        }
        public static IServiceCollection RegisterEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQServices>(sp =>
            {
                var subscriptionClientName = "queue_test";
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQServices>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var retryCount = 5;

                return new EventBusRabbitMQServices(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, serviceScopeFactory, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            //services.AddTransient<IIntegrationEventHandler<UserCreatedIntergrationEvent>, UserCreatedIntergrationEventHandler>();
            return services;
        }
    }
}
