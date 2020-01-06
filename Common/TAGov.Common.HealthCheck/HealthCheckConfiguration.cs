using System.Collections.Generic;

namespace TAGov.Common.HealthCheck
{
	public class HealthCheckConfiguration
	{
		public HealthCheckConfiguration()
		{
			SqlConnections = new List<HealthCheckSqlConnection>();
			Urls = new List<HealthCheckUrl>();
		}

		public List<HealthCheckSqlConnection> SqlConnections { get; }

		public List<HealthCheckUrl> Urls { get; }
	}
}
