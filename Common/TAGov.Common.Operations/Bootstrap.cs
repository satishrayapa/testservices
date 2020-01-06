using System;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;

namespace TAGov.Common.Operations
{
	public static class Bootstrap
	{
		public static void Execute(string[] args, IOperations operations)
		{
			var commandLineApplication = new CommandLineApplication(false);

			var applyMigrate = commandLineApplication.Option(
				"--ef-migrate",
				"Apply entity framework migrations and exit",
				CommandOptionType.NoValue);

			var verifyMigrate = commandLineApplication.Option(
				"--ef-migrate-check",
				"Check the status of entity framework migrations",
				CommandOptionType.NoValue);

			var appSettingsDirectory = commandLineApplication.Option(
				"--appsettings-directory",
				"Directory to where the appsettings exist. The environment variable of aspnetcore will be checked to determine appsettings file.",
				CommandOptionType.SingleValue);

			operations.AddOption(commandLineApplication);

			commandLineApplication.HelpOption("-? | -h | --help");

			commandLineApplication.OnExecute(() =>
			{
				ExecuteApp(applyMigrate, verifyMigrate, appSettingsDirectory, operations);
				return 0;
			});

			commandLineApplication.Execute(args);
		}

		private static IConfiguration GetConfiguration(CommandOption appSettingsDirectory)
		{
			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var builder = new ConfigurationBuilder();

      if (appSettingsDirectory.HasValue())
      {
        builder.SetBasePath(appSettingsDirectory.Value())
          .AddJsonFile($"appsettings.json", true, true)
          .AddJsonFile($"appsettings.{environmentName}.json", true, true);
      }

		  builder.AddEnvironmentVariables();

			return builder.Build();
		}

		private static void ExecuteApp(CommandOption applyMigrate, CommandOption verifyMigrate, CommandOption appSettingsDirectory, IOperations operations)
		{
			if (verifyMigrate.HasValue() && applyMigrate.HasValue())
			{
				Console.WriteLine("ef-migrate and ef-migrate-check are mutually exclusive, select one, and try again");
				Environment.Exit(2);
			}

			if (verifyMigrate.HasValue())
			{
				Console.WriteLine("Validating status of Entity Framework migrations");

				var migrations = operations.GetEfMigrations(GetConfiguration(appSettingsDirectory)).ToList();

				if (!migrations.Any())
				{
					Console.WriteLine("No pending migrations");
					Environment.Exit(0);
				}

				Console.WriteLine("Pending migrations {0}", migrations.Count);
				foreach (var migration in migrations)
				{
					Console.WriteLine($"\t{migration}");
				}

				Environment.Exit(3);
			}

			if (applyMigrate.HasValue())
			{
				Console.WriteLine("Applying Entity Framework migrations...");

				operations.ApplyEfMigrations(GetConfiguration(appSettingsDirectory));

				Environment.Exit(0);
			}

			Environment.Exit(operations.Apply(GetConfiguration(appSettingsDirectory)));
		}
	}
}
