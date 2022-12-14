using System.ComponentModel;

namespace Booking.API.ViewModel.Payments
{
    public enum LanguageType
    {
        [Description("vn")]
        VietNamese = 1,
        [Description("en")]
        English = 2
    }

    public enum BankType
    {
        [Description("Không chọn")]
        None = 0,
        [Description("VNPAYQR")]
        VNPAYQR = 1,
        [Description("LOCAL BANK")]
        VNBANK = 2,
        [Description("INTERNATIONAL CARD")]
        INTCARD = 3,
        [Description("VISA")]
        VISA = 4,
        [Description("MASTERCARD")]
        MASTERCARD = 5,
        [Description("JCB")]
        JCB = 6,
        [Description("UPI")]
        UPI = 7,
        [Description("Ngân hàng NCB")]
        NCB = 8,
        [Description("Ngân hàng SACOMBANK")]
        SACOMBANK = 9
    }

    public enum OrderCategoryType
    {
        [Description("Nạp tiền điện thoại")]
        topup = 1,
        [Description("Thanh toán hóa đơn")]
        billpayment = 2,
        [Description("Thời trang")]
        fashion = 3,
        [Description("Thanh toán trực tuyến")]
        other = 4

    }

    public enum BillType
    {
        [Description("Cá Nhân")]
        I = 1,
        [Description("Công ty/Tổ chức")]
        O = 2,
    }
}
