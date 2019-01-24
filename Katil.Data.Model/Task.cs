using System;
using System.ComponentModel.DataAnnotations;

namespace Katil.Data.Model
{
    public class Task : BaseEntity
    {
        public int TaskId { get; set; }

        [Required]
        public Guid DisputeGuid { get; set; }

        public Dispute Dispute { get; set; }

        public bool? IsDeleted { get; set; }

        [Required]
        public byte TaskLinkedTo { get; set; }

        public int? TaskLinkId { get; set; }

        public byte? TaskType { get; set; }

        [Required]
        [StringLength(1000)]
        public string TaskText { get; set; }

        public byte TaskPriority { get; set; }

        public byte? TaskStatus { get; set; }

        public DateTime? DateTaskCompleted { get; set; }

        public SystemUser SystemUser { get; set; }

        public int? TaskOwnerId { get; set; }

        public DateTime? TaskDueDate { get; set; }
    }
}
