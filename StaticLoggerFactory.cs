using System.Collections.Concurrent;

namespace velocist.LogService;

/// <summary>
/// Static logger
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
	public static ILogger InitializeLoggerFactory<T>() {
		// Configura la factoría
		var factory = LoggerFactory.Create(builder => {
			_ = builder
				.AddConsole()
				.AddDebug()
				.SetMinimumLevel(LogLevel.Debug); // Puedes ajustar el nivel
		});

		InitializeLog(factory);

		return loggerByType.GetOrAdd(typeof(T), _loggerFactory.CreateLogger<T>());

	}
}
