using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UniversityApp.Api.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // بناء الشكل الموحد للاستجابة
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            // 1. معالجة أخطاء التحقق (Validation)
            if (exception is ValidationException validationException)
            {
                problemDetails.Title = "يوجد خطأ في البيانات المدخلة.";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Extensions["errors"] = validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
            }
            // 2. معالجة البيانات غير الموجودة (Not Found)
            else if (exception is KeyNotFoundException keyNotFoundException)
            {
                problemDetails.Title = "المورد غير موجود.";
                problemDetails.Detail = keyNotFoundException.Message;
                problemDetails.Status = StatusCodes.Status404NotFound;
            }
            // 3. معالجة قواعد البيزنس المكسورة (Bad Request)
            else if (exception is InvalidOperationException invalidOpException)
            {
                problemDetails.Title = "عملية غير مسموح بها.";
                problemDetails.Detail = invalidOpException.Message;
                problemDetails.Status = StatusCodes.Status400BadRequest;
            }
            // 4. معالجة الأخطاء غير المتوقعة (Internal Server Error)
            else
            {
                problemDetails.Title = "حدث خطأ داخلي في الخادم.";
                // ملاحظة: في بيئة الإنتاج (Production) لا ترسل exception.Message للمستخدم لأسباب أمنية
                problemDetails.Detail = exception.Message;
                problemDetails.Status = StatusCodes.Status500InternalServerError;
            }

            // إرسال الرد بالرمز الصحيح
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}