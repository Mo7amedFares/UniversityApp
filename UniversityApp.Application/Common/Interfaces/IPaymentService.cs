using System.Threading.Tasks;

namespace UniversityApp.Application.Common.Interfaces
{
    public class PaymentSessionResult
    {
        public string PaymentUrl { get; set; } = string.Empty;
        public string GatewayTransactionId { get; set; } = string.Empty;
    }

    public interface IPaymentService
    {
        Task<PaymentSessionResult> CreatePaymentSessionAsync(int requestId, decimal amount, int studentId);
        bool ValidateWebhookSignature(string payload, string signature);
    }
}