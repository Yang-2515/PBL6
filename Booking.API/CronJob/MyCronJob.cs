using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Locations;
using Quartz;

namespace Booking.API.CronJob
{
    public class MyCronJob : IJob
    {
        private readonly ILogger<MyCronJob> _logger;
        private readonly ILocationRepository _locationRepo;
        private readonly IUnitOfWork _unitOfWork;

        public MyCronJob(ILogger<MyCronJob> logger
            , IUnitOfWork unitOfWork
            , ILocationRepository locationRepo)
        {
            _logger = logger;
            _locationRepo = locationRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("{now} CronJob 1 is working.", DateTime.Now.ToString("T"));
            await _locationRepo.InsertAsync(new Location("Nha tro VM"
                                                , "aaaaaaaaaaaaaa"
                                                , "20 Nguyen Binh"
                                                , "dfghyjtefrehjjyghyh"
                                                , 48
                                                , 492
                                                , 20236
                                                , true
                                                , null));
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
