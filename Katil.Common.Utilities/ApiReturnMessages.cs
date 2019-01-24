namespace Katil.Common.Utilities
{
    public static class ApiReturnMessages
    {
        // Auth
        public const string Authorized = "Authorized";
        public const string TokenIsNotSpecified = "Token is not specified";
        public const string WrongToken = "Wrong token specified";
        public const string SessionTimeRemaining = "session_time_remaining:{0}";

        // General messages
        public const string ConflictOccured = "Conflict occured";
        public const string Deleted = "Deleted";

        // Users
        public const string InactiveUser = "user with ID={0} does not match an active role 1 user in the dispute";
        public const string UserDoesNotExist = "user with ID={0} does not exist";
        public const string InvalidUser = "Invalid system user ID provided";
        public const string DuplicateRecord = "An active role record already exists";
        public const string PasswordResetComplete = "Password reset completed.";
        public const string OwnerNotAssociated = "The owner is not associated to this dispute";
        public const string InternlaUserProfileExists = "A profile already exists for this user,  Update the current profile";

        public const string DuplicateEmailForRole = "Email address already exists for this role, a new user with the same email cannot be created";
        public const string DuplicateUsernameForRole = "User name already exists for this role, a new user with the same user name cannot be created";
        public const string InactiveUsernameExists = "User name already exists for this role but is not active. Set the current user to active";
        public const string InactiveEmailExists = "User email already exists for this role but is not active. Set the current user to active";
        public const string SchedulerOnlyForStaffUsers = "Scheduler can only be set on staff user accounts";
    }
}
