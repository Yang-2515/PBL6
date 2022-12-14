using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Booking.API.CronJob
{
    public class RemindPaymentDailyJob : IJob
    {
        private readonly ILogger<RemindPaymentDailyJob> _logger;
        private readonly INotificationBookingRepository _notiBookingRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public RemindPaymentDailyJob(ILogger<RemindPaymentDailyJob> logger
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
            _logger.LogInformation("{now} RemindPaymentDailyJob is working.", DateTime.Now.ToString("T"));
            var bookings = await _bookingRepo.GetBookingDueForPaymentAsync();
            foreach (var booking in bookings)
            {
                var username = await _userRepo.GetQuery(_ => _.Id == booking.Room.Location.OwnerId).Select(x => x.Name).FirstOrDefaultAsync();
                booking.AddNoti(booking.Room.Location.OwnerId
                                , username
                                , "thông báo sắp đến hạn thanh toán tiền phòng"
                                , booking.UserId);
                await _unitOfWork.SaveChangeAsync();
                //push noti
            }
        }
    }
}
