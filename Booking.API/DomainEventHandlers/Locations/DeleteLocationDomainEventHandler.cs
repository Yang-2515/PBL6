using Booking.Domain.DomainEvents.Locations;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Rooms;
using MediatR;

namespace Booking.API.DomainEventHandlers.Locations
{
    public class DeleteLocationDomainEventHandler : INotificationHandler<DeleteLocationDomainEvent>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteLocationDomainEventHandler(IRoomRepository roomRepository
            , IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteLocationDomainEvent notification, CancellationToken cancellationToken)
        {
            foreach (var room in _roomRepository.GetByFilter(notification.LocationId, null, null, null, null, null).ToList())
            {
                room.Remove();
            }

            await _unitOfWork.SaveChangeAsync();
        }
    }
}
