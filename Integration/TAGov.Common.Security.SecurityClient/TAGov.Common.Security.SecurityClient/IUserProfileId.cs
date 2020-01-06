namespace TAGov.Common.Security.SecurityClient
{
	public interface IUserProfileId
	{
		bool IsAuthenticated(out int profileLoginId);
	}
}
