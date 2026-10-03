using System.Threading.Tasks;
using UniversityApp.Application.Common.Interfaces;

namespace UniversityApp.Infrastructure.Services
{
    public class MockPaymentService : IPaymentService
    {
       

        public Task<PaymentSessionResult> CreatePaymentSessionAsync(int requestId, decimal amount, int studentId)
        {
            return Task.FromResult(new PaymentSessionResult
            {
                PaymentUrl = $"https://dummy-payment-gateway.com/pay/{requestId}",
                GatewayTransactionId = $"txn_dummy_{System.Guid.NewGuid().ToString().Substring(0, 8)}"
            });
        }

        public bool ValidateWebhookSignature(string payload, string signature)
        {
            return true; // محاكاة لنجاح التحقق من البنك
        }
    }
}