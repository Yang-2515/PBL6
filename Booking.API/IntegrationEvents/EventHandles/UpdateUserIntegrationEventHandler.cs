using Booking.API.IntegrationEvents.Events;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Users;
using EventBus.Abstractions;

namespace Booking.API.IntegrationEvents.EventHandles
{
    public class UpdateUserIntegrationEventHandler : IIntegrationEventHandler<UpdateUserIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserIntegrationEventHandler> _logger;

        public UpdateUserIntegrationEventHandler(ILogger<UpdateUserIntegrationEventHandler> logger
            , IUnitOfWork unitOfWork
            , IUserRepository userRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task Handle(UpdateUserIntegrationEvent @event)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var user = _userRepository.GetQuery(_ => _.Id == @event.UserId).FirstOrDefault();
                if(user == null)
                {
                    _logger.LogError("Update user: User Not found");
                    return;
                }
                user.Update(@event.Name, @event.Avatar);

                _logger.LogInformation("Update user");
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError(ex.Message);
            }
        }
    }
}
