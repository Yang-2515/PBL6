using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Locations;
using Booking.Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Booking.API.CronJob
{
    public class BookingOutOfDayJob : IJob
    {
        private readonly ILogger<BookingOutOfDayJob> _logger;
        private readonly IBookingRepository _bookingRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public BookingOutOfDayJob(ILogger<BookingOutOfDayJob> logger
            , IUnitOfWork unitOfWork
            , IUserRepository userRepo
            , IBookingRepository bookingRepo)
        {
            _logger = logger;
            _bookingRepo = bookingRepo;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{now} CronJob is working.", DateTime.Now.ToString("T"));
            var bookings = _bookingRepo.GetBookingOutOfDay().ToList();
            foreach (var booking in bookings)
            {
                booking.UpdateStatus(BookingStatus.Reject);
                var username = await _userRepo.GetQuery(_ => _.Id == booking.Room.Location.OwnerId).Select(x => x.Name).FirstOrDefaultAsync();
                booking.AddNoti(booking.Room.Location.OwnerId, username, "đã hủy bỏ yêu cầu thuê phòng", booking.UserId);
                await _unitOfWork.SaveChangeAsync();
                
            }
        }
    }
}
