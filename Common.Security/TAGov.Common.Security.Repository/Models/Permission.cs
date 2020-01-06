namespace TAGov.Common.Security.Repository.Models
{
	public class Permission
	{
		public string ApplicationName { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public bool CanView { get; set; }
		public bool CanCreate { get; set; }
		public bool CanModify { get; set; }
		public bool CanDelete { get; set; }
    public int AppFunctionId { get; set; }
    public int AppFunctionParentId { get; set; }
	}
}
