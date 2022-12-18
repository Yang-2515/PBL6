using Booking.API.IntegrationEvents.Events;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Users;
using EventBus.Abstractions;

namespace Booking.API.IntegrationEvents.EventHandles
{
    public class DeleteUserIntegrationEventHandler : IIntegrationEventHandler<DeleteUserIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserIntegrationEventHandler> _logger;

        public DeleteUserIntegrationEventHandler(ILogger<DeleteUserIntegrationEventHandler> logger
            , IUnitOfWork unitOfWork
            , IUserRepository userRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task Handle(DeleteUserIntegrationEvent @event)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var user = _userRepository.GetQuery(_ => _.Id == @event.UserId).FirstOrDefault();
                if (user == null)
                {
                    _logger.LogError("Delete user: User Not found");
                    return;
                }
                await _userRepository.RemoveAsync(user);

                _logger.LogInformation("Delete user");
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
