﻿using System;
using Katil.Business.Entities.Models.Base;
using Newtonsoft.Json;

namespace Katil.Business.Entities.Models.Files
{
    public class FileResponse : CommonResponse
    {
        [JsonProperty("file_id")]
        public int FileId { get; set; }

        [JsonProperty("file_guid")]
        public Guid FileGuid { get; set; }

        [JsonProperty("dispute_guid")]
        public Guid DisputeGuid { get; set; }

        [JsonProperty("file_type")]
        public byte FileType { get; set; }

        [JsonProperty("file_mime_type")]
        public string FileMimeType { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("file_date")]
        public string FileDate { get; set; }

        [JsonProperty("original_file_name")]
        public string OriginalFileName { get; set; }

        [JsonProperty("file_size")]
        public int FileSize { get; set; }

        [JsonProperty("file_title")]
        public string FileTitle { get; set; }

        [JsonProperty("file_status")]
        public byte? FileStatus { get; set; }

        [JsonProperty("added_by")]
        public int? AddedBy { get; set; }

        [JsonProperty("file_url")]
        public string FileUrl { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }

        [JsonProperty("file_package_id")]
        public int? FilePackageId { get; set; }

        [JsonProperty("file_considered")]
        public bool? FileConsidered { get; set; }

        [JsonProperty("file_referenced")]
        public bool? FileReferenced { get; set; }
    }
}