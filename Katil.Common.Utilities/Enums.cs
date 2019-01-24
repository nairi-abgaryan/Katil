using System.ComponentModel;

namespace Katil.Common.Utilities
{
    public enum Roles
    {
        [Description("ROLE_USER")]
        StaffUser = 1,
        [Description("ROLE_ADMIN")]
        ExternalUser = 2
    }

    public enum EmailStatus
    {
        UnSent = 0,
        Sent = 1,
        Pending = 2,
        Error = 3
    }

    public enum EmailMessageBodyType
    {
        Plain = 0,
        Html,
        Both
    }

    public enum EmailMessageType
    {
        ParticipatoryPaymentRequired = 1,
        ParticipatoryNeedsFeeWaiverProof = 2,
        ParticipatorySubmittedPaid = 3,
        PaymentReceipt = 4,
        ApplicantEvidenceReminder = 5,
        RespondentEvidenceReminder = 6,
        HearingReminder = 7,
        DisputeWithrdawn = 8,
        DisputeCancelled = 9,
        IntakeAbandonedNoPayment = 10,
        ParticipatoryUpdateSubmitted = 11,
        SubmitterReviewAppSubmitted = 12,
        AllPartiesReviewAppSubmitted = 13,
        NonParticipatorySubmittedPaid = 14,
        NonParticipatoryOfficePay = 15,
        NonParticipatoryUpdateSubmitted = 16
    }

    public enum SearchSortField
    {
        Submitted = 1,
        Created = 2,
        Modified = 3,
        Status = 4
    }

    public enum SortDir
    {
        ASC = 0,
        DESC
    }

    public enum PaymentMethod
    {
        Online = 1,
        Office = 2,
        FeeWaiver = 3
    }

    public enum PaymentStatus
    {
        Pending = 1,
        ApprovedOrPaid = 2,
        Rejected = 3,
        Cancelled = 4
    }

    public enum PaymentVerified
    {
        NotChecked = 0,
        Checked = 1,
        Error = 2
    }

    public enum PaymentProvider
    {
        Bambora = 1
    }

    public enum AttachmentType
    {
        Common = 1
    }

    public enum HearingStatus
    {
        NotActive = 0,
        Actve = 1,
        Completed = 2
    }

    public enum HearingType
    {
        ConferenceCall = 1,
        FaceToFace = 2,
        PreconferenceCall = 3,
        Other = 4
    }

    public enum CommonFileType
    {
        NotSet = 0,
        UserGuide = 1,
        Form = 2,
        ProfilePicture = 3,
        Signature = 4
    }

    public enum FileType
    {
        NotSet = 0,
        ExternalEvidence = 1,
        Notice = 2,
        InternalDocuments = 3,
        ExternalNonEvidence = 4,
        Other = 5
    }

    public enum FileMethod
    {
        UploadNow = 100,
        UploadLater = 101,
        Method103 = 103,
        DropOf = 104
    }

    public enum FilePackageType
    {
        IntakeSubmission = 1,
        FilePackageType3 = 2
    }

    public enum ApiCallType
    {
        Get = 1,
        Post = 2,
        Patch = 3,
        Delete = 4
    }

    public enum Months
    {
        January = 1,
        Fabruary,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
