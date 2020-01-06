namespace TAGov.Common.HealthCheck
{
	public class HealthCheckSqlConnection
	{
		public string ConnectionString { get; set; }
		public int? CacheInSeconds { get; set; }
	}
}
