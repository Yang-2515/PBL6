using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Booking.API.CronJob
{
    public class ExtendDueBookingJob : IJob
    {
        private readonly ILogger<ExtendDueBookingJob> _logger;
        private readonly INotificationBookingRepository _notiBookingRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ExtendDueBookingJob(ILogger<ExtendDueBookingJob> logger
            , IUnitOfWork unitOfWork
            , INotificationBookingRepository notiBookingRepo
            , IBookingRepository bookingRepo
            , IUserRepository userRepo)
        {
            _logger = logger;
            _notiBookingRepo = notiBookingRepo;
            _unitOfWork = unitOfWork;
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{now} ExtendDueBooking is working.", DateTime.Now.ToString("T"));
            var bookings = await _bookingRepo.GetBookingMustExtendDueAsync();
            foreach (var booking in bookings)
            {
                booking.UpdateStatus(Domain.BookingStatus.ExtendDue);
                var username = await _userRepo.GetQuery(_ => _.Id == booking.Room.Location.OwnerId).Select(x => x.Name).FirstOrDefaultAsync();
                booking.AddNoti(booking.Room.Location.OwnerId
                                , username
                                , "thông báo cần gia hạn thuê phòng"
                                , booking.UserId);
                booking.AddNoti(booking.UserId
                                , booking.UserName
                                , "cần gia hạn thuê phòng"
                                , booking.Room.Location.OwnerId);
                await _unitOfWork.SaveChangeAsync();
                //push noti
            }
        }
    }
}
