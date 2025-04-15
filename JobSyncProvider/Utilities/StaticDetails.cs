namespace JobSyncProvider.Utilities
{
	public class StaticDetails
	{
		//User Roles
		public const string ROLE_ADMIN = "Admin";
		public const string ROLE_EMPLOYER = "Employer";
		public const string ROLE_JOBSEEKER = "JobSeeker";

		//Record Status
		public const string STATUS_VALID = "Valid";
		public const string STATUS_INVALID = "Invalid";
        public const bool STATUS_ACTIVE = true;
        public const bool STATUS_INACTIVE = false;

		//JobApplication Status
		public const string APPLICATION_PENDING = "Pending";
		public const string APPLICATION_INPROGRESS= "In Progress";
		public const string APPLICATION_COMPLETED = "Completed";
		public const string APPLICATION_CANCELLED = "Cancelled";
		public const string APPLICATION_FAILED = "Failed";
		public const string APPLICATION_PAUSED = "Paused";

	}
}
