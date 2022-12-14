using Booking.API.Extensions;
using Booking.API.ViewModel.Payments;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces;
using Booking.Domain.Interfaces.Repositories.Bookings;
using Booking.Domain.Interfaces.Repositories.Payments;
using Booking.Domain.Models;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Booking.API.Services
{
    public class PaymentService
    {
        private static readonly ILog log =
          LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;
        private SortedList<String, String> _responseData = new SortedList<String, String>(new VnPayCompare());
        public PaymentService(IConfiguration configuration
            , IHttpContextAccessor httpContextAccessor
            , IUnitOfWork unitOfWork
            , IPaymentRepository paymentRepository
            , IBookingRepository bookingRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }
        public async Task<string> Pay(PaymentInfoRequest request)
        {
            //Get Config Info
            var paymentConfigInfo = _configuration.GetSection("Payment");
            var vnp_Returnurl = paymentConfigInfo.GetSection("vnp_Returnurl").Value; //URL nhan ket qua tra ve 
            var vnp_Url = paymentConfigInfo.GetSection("vnp_Url").Value; //URL thanh toan cua VNPAY 
            var vnp_TmnCode = paymentConfigInfo.GetSection("vnp_TmnCode").Value; //Ma website
            var vnp_HashSecret = paymentConfigInfo.GetSection("vnp_HashSecret").Value; //Chuoi bi mat
            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                throw new ArgumentNullException("Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config");
            }
            var dateNowTick = DateTime.Now.Ticks.ToString();
            var paymentCode = "P" + dateNowTick + request.BookingId.ToString();
            var isPaymentValid = await _paymentRepository.AnyAsync(_ => _.PaymentCode == paymentCode);
            if (isPaymentValid)
            {
                throw new BadHttpRequestException("Da ton tai ma giao dich");
            }
            //Get payment input
            var booking = await _bookingRepository.GetAsync(request.BookingId);
            if (booking == null)
            {
                throw new BadHttpRequestException("Khong ton tai bookingId");
            }
            Payment payment = new Payment();
            //Save order to db
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(_ => _.Type == "id")?.Value;
            payment.UserId = userId;
            payment.BookingId = request.BookingId; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            payment.Amount = request.Amount; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            payment.Status = null; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            payment.OrderDesc = request.OrderDesc;
            payment.CreateOn = DateTime.Now;
            payment.PaymentCode = paymentCode;
            

            var locale = request.Language;
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (payment.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (request.Bank != BankType.None)
            {
                var a = request.Bank.GetType();
                var c = nameof(request.OrderCategory);
                var b = request.Bank.ToString();
                vnpay.AddRequestData("vnp_BankCode", request.Bank.ToString());
            }
            vnpay.AddRequestData("vnp_CreateDate", payment.CreateOn.Value.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(_httpContextAccessor.HttpContext));
            vnpay.AddRequestData("vnp_Locale", locale.GetDescription());
            vnpay.AddRequestData("vnp_OrderInfo", payment.PaymentCode);
            vnpay.AddRequestData("vnp_OrderType", request.OrderCategory.ToString()); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", "http://localhost:5001/api/booking/payment/return");
            vnpay.AddRequestData("vnp_TxnRef", "P" + dateNowTick); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            // 20221210152938
            var dateExpire = DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss");
            vnpay.AddRequestData("vnp_ExpireDate", dateExpire);
            //Billing
            vnpay.AddRequestData("vnp_Bill_Mobile", request.BillingMobile.Trim());
            vnpay.AddRequestData("vnp_Bill_Email", request.BillingEmail.Trim());
            vnpay.AddRequestData("vnp_Bill_FirstName", request.BillingFirstName.Trim());
            vnpay.AddRequestData("vnp_Bill_LastName", request.BillingLastName.Trim());

            vnpay.AddRequestData("vnp_Bill_Address", request.BillingAddress.Trim());
            vnpay.AddRequestData("vnp_Bill_City", request.BillingCity.Trim());
            vnpay.AddRequestData("vnp_Bill_Country", request.BillingCountry.Trim());
            vnpay.AddRequestData("vnp_Bill_State", "");

            // Invoice

            vnpay.AddRequestData("vnp_Inv_Phone", request.ShippingMobile.Trim());
            vnpay.AddRequestData("vnp_Inv_Email", request.ShippingEmail.Trim());
            vnpay.AddRequestData("vnp_Inv_Customer", request.ShippingCustomer.Trim());
            vnpay.AddRequestData("vnp_Inv_Address", request.ShippingAddress.Trim());
            vnpay.AddRequestData("vnp_Inv_Company", request.ShippingCompany);
            vnpay.AddRequestData("vnp_Inv_Taxcode", request.ShippingTaxCode);
            vnpay.AddRequestData("vnp_Inv_Type", request.ShippingBillType.ToString());

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);

            await _paymentRepository.InsertAsync(payment);
            await _unitOfWork.SaveChangeAsync();

            return paymentUrl;
        }

        public async Task<string> ReturnPay(
            int vnp_Amount
            , string vnp_BankCode
            , string vnp_BankTranNo
            , string vnp_CardType
            , string vnp_OrderInfo
            , string vnp_PayDate
            , string vnp_ResponseCode
            , string vnp_TmnCode
            , string vnp_TransactionNo
            , string vnp_TransactionStatus
            , string vnp_TxnRef
            , string vnp_SecureHash
             )
        {
            // Add response data
            VnPayLibrary vnpay = new VnPayLibrary();
            if (!string.IsNullOrEmpty(vnp_Amount.ToString()))
            {
                vnpay.AddResponseData("vnp_Amount", vnp_Amount.ToString());
            }
            if (!string.IsNullOrEmpty(vnp_BankCode))
            {
                vnpay.AddResponseData("vnp_BankCode", vnp_BankCode);
            }
            if (!string.IsNullOrEmpty(vnp_BankTranNo))
            {
                vnpay.AddResponseData("vnp_BankTranNo", vnp_BankTranNo);
            }
            if (!string.IsNullOrEmpty(vnp_CardType))
            {
                vnpay.AddResponseData("vnp_CardType", vnp_CardType);
            }
            if (!string.IsNullOrEmpty(vnp_OrderInfo))
            {
                vnpay.AddResponseData("vnp_OrderInfo", vnp_OrderInfo);
            }
            if (!string.IsNullOrEmpty(vnp_PayDate))
            {
                vnpay.AddResponseData("vnp_PayDate", vnp_PayDate);
            }
            if (!string.IsNullOrEmpty(vnp_ResponseCode))
            {
                vnpay.AddResponseData("vnp_ResponseCode", vnp_ResponseCode);
            }
            if (!string.IsNullOrEmpty(vnp_TmnCode))
            {
                vnpay.AddResponseData("vnp_TmnCode", vnp_TmnCode);
            }
            if (!string.IsNullOrEmpty(vnp_TransactionNo))
            {
                vnpay.AddResponseData("vnp_TransactionNo", vnp_TransactionNo);
            }
            if (!string.IsNullOrEmpty(vnp_TransactionStatus))
            {
                vnpay.AddResponseData("vnp_TransactionStatus", vnp_TransactionStatus);
            }
            if (!string.IsNullOrEmpty(vnp_TxnRef))
            {
                vnpay.AddResponseData("vnp_TxnRef", vnp_TxnRef);
            }
            if (!string.IsNullOrEmpty(vnp_SecureHash))
            {
                vnpay.AddResponseData("vnp_SecureHash", vnp_SecureHash);
            }

            var paymentConfigInfo = _configuration.GetSection("Payment");
            var vnp_HashSecret = paymentConfigInfo.GetSection("vnp_HashSecret").Value;
            //int orderId = Convert.ToInt32(vnp_TxnRef);
            var trtrt = vnp_OrderInfo.Replace(vnp_TxnRef, "");
            var bookingId = Convert.ToInt32(vnp_OrderInfo.Replace(vnp_TxnRef, ""));
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    //Thanh toan thanh cong
                    var payment = await _paymentRepository.GetQuery(_ => _.PaymentCode == vnp_OrderInfo).FirstOrDefaultAsync();
                    if (payment == null)
                    {
                        throw new BadHttpRequestException("Khong tim thay payment");
                    }

                    payment.Status = true;
                    payment.TranCode = vnp_TransactionNo;
                    
                    var isExistPaymentForBooking = await _paymentRepository.AnyAsync
                                                    ( _ => 
                                                        _.BookingId == bookingId
                                                        && _.Status == true
                                                    );
                    //Neu da thanh toan cho 1 booking id roi
                    if (isExistPaymentForBooking)
                    {
                       
                    }
                    //Neu chua thanh toan cho booking nao ca
                    else
                    {

                    }

                    await _unitOfWork.SaveChangeAsync();
                    

                    Console.WriteLine("Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ");
                    Console.WriteLine("Thanh toán thành công, BookingId={0}", bookingId);
                    Console.WriteLine("Mã giao dịch thanh toán:" + vnp_OrderInfo);
                    Console.WriteLine("Số tiền thanh toán (VND):" + vnp_Amount.ToString());
                    Console.WriteLine("Ngân hàng thanh toán:" + vnp_BankCode);
                }
                else
                {
                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode

                    // payment.Status = 0

                    Console.WriteLine("Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode);
                    Console.WriteLine("Thanh toan loi, BookingId={0},ResponseCode={1}", bookingId, vnp_ResponseCode);
                }
            }
            else
            {
                Console.WriteLine("Có lỗi xảy ra trong quá trình xử lý");
            }
            return "";
        }

    }
}
