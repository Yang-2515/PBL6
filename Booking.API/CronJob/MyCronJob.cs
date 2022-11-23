using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Locations;
using Quartz;

namespace Booking.API.CronJob
{
    public class MyCronJob : IJob
    {
        private readonly ILogger<MyCronJob> _logger;
        private readonly IBookingRepository _bookingRepo;
        private readonly IUnitOfWork _unitOfWork;

        public MyCronJob(ILogger<MyCronJob> logger
            , IUnitOfWork unitOfWork
            , IBookingRepository bookingRepo)
        {
            _logger = logger;
            _bookingRepo = bookingRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{now} CronJob is working.", DateTime.Now.ToString("T"));
            var bookings = _bookingRepo.GetBookingOutOfDay().ToList();
            foreach (var booking in bookings)
            {
                booking.UpdateStatus(BookingStatus.Reject);
                await _unitOfWork.SaveChangeAsync();
                //push noti
            }
        }
    }
}
