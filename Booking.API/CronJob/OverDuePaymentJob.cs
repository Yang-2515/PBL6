using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Booking.API.CronJob
{
    public class OverDuePaymentJob : IJob
    {
        private readonly ILogger<OverDuePaymentJob> _logger;
        private readonly IBookingRepository _bookingRepo;
        private readonly INotificationBookingRepository _notificationBooking;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OverDuePaymentJob(ILogger<OverDuePaymentJob> logger
            , IUnitOfWork unitOfWork
            , INotificationBookingRepository notificationBooking
            , IBookingRepository bookingRepo
            , IUserRepository userRepo)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
            _notificationBooking = notificationBooking;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{now} OverDuePaymentJob is working.", DateTime.Now.ToString("T"));
            var bookings = await _bookingRepo.GetBookingOverDuePaymentAsync();
            foreach (var booking in bookings)
            {
                var username = await _userRepo.GetQuery(_ => _.Id == booking.Room.Location.OwnerId).Select(x => x.Name).FirstOrDefaultAsync();
                booking.AddNoti(booking.Room.Location.OwnerId
                                , username
                                , "thông báo đã quá hạn thanh toán tiền thuê phòng"
                                , booking.UserId);
                booking.AddNoti(booking.UserId
                                , booking.UserName
                                , "đã quá hạn thanh toán tiền phòng"
                                , booking.Room.Location.OwnerId);
                await _unitOfWork.SaveChangeAsync();
                //push noti
            }
        }
    }
}
