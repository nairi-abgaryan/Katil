using System;
using System.ComponentModel.DataAnnotations;

namespace Katil.Data.Model
{
    public class File : BaseEntity
    {
        public int FileId { get; set; }

        [Required]
        public Guid FileGuid { get; set; }

        [Required]
        public Guid DisputeGuid { get; set; }

        [Required]
        public byte FileType { get; set; }

        [StringLength(255)]
        [Required]
        public string FileMimeType { get; set; }

        [StringLength(255)]
        [Required]
        public string FileName { get; set; }

        public DateTime? FileDate { get; set; }

        [StringLength(255)]
        [Required]
        public string OriginalFileName { get; set; }

        [Required]
        public int FileSize { get; set; }

        [StringLength(255)]
        [Required]
        public string FilePath { get; set; }

        [StringLength(100)]
        public string FileTitle { get; set; }

        public byte? FileStatus { get; set; }

        public int? AddedBy { get; set; }

        public bool? FileConsidered { get; set; }

        public bool? FileReferenced { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
