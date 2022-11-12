using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Booking.Domain.Entities;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public DbSet<User> Users { get; set;}
        public DbSet<Room> Rooms { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Wards> Wardses { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<BookingUtility> BookingUtilities { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<BookingUtility>()
                .HasOne(e => e.Booking)
                .WithMany(_ => _.BookingUtilities)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .Entity<BookingUtility>()
                .HasOne(e => e.Utility)
                .WithMany(_ => _.BookingUtilities)
                .OnDelete(DeleteBehavior.SetNull);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.SaveChangesAsync(cancellationToken);
            await _mediator.DispatchDomainEventsAsync(this);
            return true;
        }

        public DbConnection GetConnection()
        {
            DbConnection _connection = Database.GetDbConnection();
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            return _connection;
        }
    }

    public class ApplicationDbContextDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlServer("server=118.69.61.226;database=Intern_Microservice;user id=sa;password=vStation123;");

            return new ApplicationDbContext(optionsBuilder.Options, new NoMediator());
        }

        private class NoMediator : IMediator
        {
            public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                return default(IAsyncEnumerable<TResponse>) ?? throw new NotImplementedException();
            }

            public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
            {
                return default(IAsyncEnumerable<object?>) ?? throw new NotImplementedException();
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(TResponse) ?? throw new ArgumentNullException());
            }

            public Task<object?> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(object));
            }
        }
    }
}
