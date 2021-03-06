﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using TAGov.Common.HealthCheck;


namespace TAGov.Common.Security.API
{
	/// <summary>
	/// Main Program
	/// </summary>
	public class Program
	{
    /// <summary>
    /// Main
    /// </summary>
    /// <param name="args">arguments. Currently not used</param>
	  public static void Main(string[] args)
	  {
	    BuildWebHost(args).Run();
	  }

	  /// <summary>
	  /// Boiler plate .NET Core 2.0 startup code.
	  /// </summary>
	  /// <param name="args">args from startup</param>
	  /// <returns></returns>
	  public static IWebHost BuildWebHost(string[] args) =>
	    WebHost.CreateDefaultBuilder(args)
	           .UseSharedHealthChecks()
	           .UseStartup<Startup>()
	           .Build();
	}
}
