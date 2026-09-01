namespace UniversityApp.Application.Features.Services.Queries.GetAllServices
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Fees { get; set; }
        public int WorkDays { get; set; }
    }
}