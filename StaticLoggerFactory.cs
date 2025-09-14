using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace velocist.LogService;

/// <summary>
/// Provides a static logger factory and logger retrieval for application-wide logging.
/// </summary>
public static class StaticLoggerFactory {

	/// <summary>
	/// The logger factory
	/// </summary>
	private static ILoggerFactory _loggerFactory;

	/// <summary>
	/// The logger by type
	/// </summary>
	private static readonly ConcurrentDictionary<Type, ILogger> loggerByType = new();

	/// <summary>
	/// Initializes the log.
	/// </summary>
	/// <param name="loggerFactory">The logger factory.</param>
	/// <exception cref="InvalidOperationException">Static logger already initialized</exception>
	/// <exception cref="ArgumentNullException">loggerFactory</exception>
	public static void InitializeLog(ILoggerFactory loggerFactory) =>
		//if (_loggerFactory != null)
		//	throw new InvalidOperationException("Static logger already initialized");

		_loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

	/// <summary>
	/// Gets the static logger.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException">Static logger is not initialized yet</exception>
	public static ILogger GetStaticLogger<T>() => _loggerFactory == null
			? InitializeLoggerFactory<T>()// throw new InvalidOperationException("Static logger is not initialized yet")
			: loggerByType.GetOrAdd(typeof(T), _loggerFactory.CreateLogger<T>());

	/// <summary>
	/// Initializes the internal logger factory if needed and returns a logger for the specified type.
	/// </summary>
	/// <typeparam name="T">Type for which the logger is created.</typeparam>
	/// <returns>Logger instance bound to type <typeparamref name="T"/>.</returns>
	public static ILogger InitializeLoggerFactory<T>(string loggingsectionName = "Logging", string logSettingsFileName = "logSettings.json", string log4netFileName = "log4net.config") {
		// Configura la factoría
		var factory = LoggerFactory.Create(builder => {
			Trace.WriteLine($"Creating LoggerFactory {AppContext.BaseDirectory}");
			_ = builder.AddConfiguration(new ConfigurationBuilder()
					//.SetBasePath(AppContext.BaseDirectory)
					.AddJsonFile(Path.Combine(AppContext.BaseDirectory, logSettingsFileName), optional: true, reloadOnChange: true)
					//.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
					//.AddEnvironmentVariables()
					.Build()
					.GetSection(loggingsectionName))
				.AddConsole()
				.AddDebug()
				.AddLog4Net(log4netFileName, true) // Asegúrate de tener el paquete Microsoft.Extensions.Logging.Log4Net.AspNetCore instalado
				.SetMinimumLevel(LogLevel.Debug); // Puedes ajustar el nivel
		});

		InitializeLog(factory);

		return loggerByType.GetOrAdd(typeof(T), _loggerFactory.CreateLogger<T>());

	}

}
