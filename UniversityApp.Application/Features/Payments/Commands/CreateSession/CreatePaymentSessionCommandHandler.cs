using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UniversityApp.Application.Common.Interfaces;
using UniversityApp.Domain.Entities;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Application.Features.Payments.Commands.CreateSession
{
    public class CreatePaymentSessionCommandHandler : IRequestHandler<CreatePaymentSessionCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public CreatePaymentSessionCommandHandler(
            IApplicationDbContext context,
            IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<string> Handle(CreatePaymentSessionCommand request, CancellationToken cancellationToken)
        {

            // 1. جلب الطلب مع الخدمة لمعرفة السعر
            var studentRequest = await _context.Requests
                .Include(r => r.Service)
                .FirstOrDefaultAsync(r => r.Id == request.RequestId, cancellationToken);

            if (studentRequest == null)
                throw new KeyNotFoundException("الطلب غير موجود.");


            if (studentRequest.Status != RequestStatus.AwaitingPayment)
                throw new InvalidOperationException("هذا الطلب غير متاح للدفع في الوقت الحالي.");

            // 2. استدعاء بوابة الدفع (عبر الـ Interface)
            // سنفترض أننا جلبنا إيميل الطالب، وسعر الخدمة من studentRequest.Service.Price
            var sessionResult = await _paymentService.CreatePaymentSessionAsync(
                requestId: studentRequest.Id,
                amount: studentRequest.Service.Fees, 
                studentId: studentRequest.StudentId
            );

            // 3. حفظ عملية الدفع في الداتا بيز بوضع Pending
            var transaction = new Payment
            {
                RequestId = studentRequest.Id,
                StudentId = studentRequest.StudentId,
                Amount = studentRequest.Service.Fees,
                Status = PaymentStatus.Pending,
                GatewayTransactionId = sessionResult.GatewayTransactionId
            };

            _context.Payments.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            // 4. إرجاع رابط الدفع للـ Frontend
            return sessionResult.PaymentUrl;
        }
    }
}