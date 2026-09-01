using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Domain.Common;
using UniversityApp.Domain.Enums;

namespace UniversityApp.Domain.Entities
{
    public class Payment:BaseEntity
    {
        public int RequestId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; } = PaymentStatus.pending;
        public Request Request { get; set; } = null!; // Navigation property to the Request entity
    }
}
