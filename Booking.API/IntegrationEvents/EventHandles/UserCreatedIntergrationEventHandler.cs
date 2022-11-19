using Booking.API.IntegrationEvents.Events;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Users;
using EventBus.Abstractions;

namespace Booking.API.IntegrationEvents.EventHandles
{
    public class UserCreatedIntergrationEventHandler : IIntegrationEventHandler<UserCreatedIntergrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserCreatedIntergrationEventHandler> _logger;

        public UserCreatedIntergrationEventHandler(ILogger<UserCreatedIntergrationEventHandler> logger
            , IUnitOfWork unitOfWork
            , IUserRepository userRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task Handle(UserCreatedIntergrationEvent @event)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var user = new User(@event.Id.ToString(), @event.Name, @event.Email);
                _logger.LogInformation("Create new user");
                await _userRepository.InsertAsync(user);
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
