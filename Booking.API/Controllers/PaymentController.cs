using Booking.API.Services;
using Booking.API.ViewModel.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/booking/payment")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<string> Pay([FromBody] PaymentInfoRequest request)
        {
            return await _paymentService.Pay(request);
        }

        [HttpGet("return")]
        [AllowAnonymous]
        public async Task<string> ReturnPay(
            //Ma thanh toan
            [FromQuery] string vnp_TxnRef
            //Tien thanh toan
            , [FromQuery] int vnp_Amount
            //Ma ngan hang
            , [FromQuery] string vnp_BankCode
            //
            , [FromQuery] string vnp_BankTranNo
            //
            , [FromQuery] string vnp_CardType
            //
            ,[FromQuery] string vnp_OrderInfo
            //
            , [FromQuery] string vnp_PayDate
            //Response code 
            , [FromQuery] string vnp_ResponseCode
            //
            , [FromQuery] string vnp_TmnCode
            //
            , [FromQuery] string vnp_TransactionNo
            // Trang thai thanh toan
            , [FromQuery] string vnp_TransactionStatus
            // SecureHash
            , [FromQuery] string vnp_SecureHash
        )
        {
            var a = vnp_ResponseCode;
            var b = vnp_TransactionStatus;
            return await _paymentService.ReturnPay(
                 vnp_Amount
                , vnp_BankCode
                , vnp_BankTranNo
                , vnp_CardType
                , vnp_OrderInfo
                , vnp_PayDate
                , vnp_ResponseCode
                , vnp_TmnCode
                , vnp_TransactionNo
                , vnp_TransactionStatus
                , vnp_TxnRef
                , vnp_SecureHash
                );
        }
    }
}
