using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace velocist.LogService.Extensions;

/// <summary>
/// Provides extension methods to register log services in the dependency injection container.
/// </summary>
public static class ServicesExtensions {

	/// <summary>
	/// Adds the log services.
	/// </summary>
	/// <param name="services">The services.</param>
	/// <param name="configuration">The configuration.</param>
	/// <param name="loggingsectionName">Name of the loggingsection.</param>
	/// <param name="log4netFileName">Name of the log4net file.</param>
	[Obsolete("Initialize GetStaticLogger instead.")]
	public static void AddLogServices(this IServiceCollection services, IConfiguration configuration, string loggingsectionName, string log4netFileName = null) {
		var loggingConfig = configuration.GetSection(loggingsectionName);
		services.AddLogging(loggingOptions => loggingOptions
		.ClearProviders()
		.AddConfiguration(loggingConfig)
#if DEBUG
		//builder.SetMinimumLevel(LogLevel.Trace)
		.AddLog4Net($"{log4netFileName}", true)
		.AddDebug() //For Debug.WriteLine or Trace.WriteLine
#else
		.AddLog4Net($"{log4netFileName}.config", true)
#endif
		.AddConsole()); //For Console.WriteLine
	}
}
