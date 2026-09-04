using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityApp.Domain.Enums
{
    public enum RequestStatus
    {
        AwaitingPayment = 1,  // 1. الطالب أنشأ الطلب ولم يدفع بعد (مخفي عن الإدمن)
        Pending = 2,          // 2. الطالب دفع بنجاح (الآن يظهر للإدمن للمراجعة)
        Processing = 3,       // 3. الإدمن بدأ العمل على الطلب
        Completed = 4,        // 4. الطلب انتهى وتمت الموافقة
        Rejected = 5,         // 5. الطلب مرفوض
        Cancelled = 6         // 6. الطالب ألغى الطلب (قبل أن يبدأ الإدمن فيه)
    }
}
