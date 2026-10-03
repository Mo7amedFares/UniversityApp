using UniversityApp.Domain.Enums;
using UniversityApp.Domain.Common;

namespace UniversityApp.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int RequestId { get; set; }
        public int StudentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // رقم العملية الذي يعود من بوابة الدفع (Stripe/Paymob) للرجوع إليه عند الاسترداد
        public string? GatewayTransactionId { get; set; }

        public Request? Request { get; set; }
    }
}