using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Models
{
    public class OrderInfo
    {
        public int BookingId { get; set; }
        public int Amount { get; set; }
        public string OrderDesc { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public int PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }

    }
}
