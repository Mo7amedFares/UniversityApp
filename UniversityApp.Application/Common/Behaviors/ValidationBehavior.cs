using FluentValidation;
using MediatR;

namespace UniversityApp.Application.Common.Behaviors
{
    // هذا الكلاس يعترض أي طلب (TRequest) قادم لـ MediatR
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        // Dependency Injection: يجلب كل الـ Validators الموجودة في المشروع
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // تشغيل كل قواعد التحقق
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // تجميع الأخطاء إن وجدت
                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                {
                    // إذا وجدنا أخطاء، نوقف العملية ونرمي Exception خاص بالتحقق
                    throw new ValidationException(failures);
                }
            }

            // إذا كانت البيانات سليمة، نعبر للـ Handler ليكمل عمله
            return await next();
        }
    }
}