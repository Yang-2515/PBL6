using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Base
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
    }

    public interface IInfoEntity
    {
        public bool IsDelete { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
    }

    public abstract class InfoEntity : BaseEntity, IInfoEntity
    {
        public virtual bool IsDelete { get; set; }
        public virtual DateTime? CreateOn { get; set; }
        public virtual DateTime? UpdateOn { get; set; }
    }

    public abstract class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
    }

    public abstract class Entity : InfoEntity
    {
        [NotMapped]
        private List<BaseDomainEvent> _events;

        public Entity()
        {
            _events = new();
        }

        [NotMapped]
        public IReadOnlyList<BaseDomainEvent> Events => _events.AsReadOnly();

        protected void AddEvent(BaseDomainEvent @event)
        {
            _events = _events ?? new List<BaseDomainEvent>();
            _events.Add(@event);
        }

        protected void RemoveEvent(BaseDomainEvent @event)
        {
            _events.Remove(@event);
        }

        public void ClearEvents()
        {
            _events?.Clear();
        }
    }
}
