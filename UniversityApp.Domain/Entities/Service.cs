using UniversityApp.Domain.Common;

namespace UniversityApp.Domain.Entities
{
    public class Service : BaseEntity // ورثنا الـ Id و CreatedAt
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorkDays { get; set; }
        public decimal Fees { get; set; }
        public bool HasRequiredFiles { get; set; }

        // Navigation Property: لربط الخدمة بالطلبات المقدمة عليها (علاقة 1 إلى متعدد)
        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}