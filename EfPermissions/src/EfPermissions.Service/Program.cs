using System;
using System.Linq;
using System.ServiceProcess;
using Microsoft.Data.Entity;
using Serilog;

namespace EfPermissions.Service
{
	public class Program : ServiceBase
	{
		public readonly ILogger Logger;

		public Program()
		{
			Logger = new LoggerConfiguration()
				.WriteTo.ColoredConsole()
				.WriteTo.File(@"C:\Temp\EfPermissions.log")
				.CreateLogger();
		}

		public void Main(string[] args)
		{
			if (args.Contains("debug"))
			{
				OnStart(args);
				Console.WriteLine("Press ESC to exit.");
				while (Console.ReadKey(intercept: true).Key != ConsoleKey.Escape) { /* Do nothing */ }
				OnStop();
			}
			else
			{
				Run(this);
			}
		}

		protected override void OnStart(string[] args)
		{
			Logger.Information("Starting...");

			using (var context = new EfPermissionsContext())
			{
				Logger.Information("Setting up database.");

				context.Database.EnsureDeleted();
				Logger.Information("   Deleted.");

				try
				{
					context.Database.Migrate();
				}
				catch (Exception e)
				{
					Logger.Fatal(e, "  Oh noes, can't migrate.");
					throw;
				}

				Logger.Information("   Migrated.");
			}

			Logger.Information("Started.");
		}

		protected override void OnStop()
		{
			Logger.Information("Stopped.");
		}
	}
}