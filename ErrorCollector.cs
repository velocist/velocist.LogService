global using static velocist.LogService.StaticLoggerFactory;
using System.Collections.Generic;
using System.Diagnostics;

namespace velocist.LogService;

/// <summary>
/// Centralized static error collector for type <typeparamref name="T"/> that does not depend on any logging framework.
/// </summary>
/// <typeparam name="T">The type for which errors are being collected (used for logging context).</typeparam>
public static class ErrorCollector<T> {

	/// <summary>
	/// Internal list to store collected errors.
	/// </summary>
	private static readonly List<ErrorInfo> _errors = [];
	/// <summary>
	/// Lock object for thread safety.
	/// </summary>
	private static readonly object _lock = new();
	/// <summary>
	/// Logger instance for the specified type.
	/// </summary>
	private static readonly ILogger Logger;

	/// <summary>
	/// Static constructor to initialize the logger.
	/// </summary>
	static ErrorCollector() {
		Logger = (ILogger<T>)GetStaticLogger<T>();
	}

	/// <summary>
	/// Event that is triggered when a new error is captured.
	/// </summary>
	public static event Action<ErrorInfo> ErrorCaptured;

	/// <summary>
	/// Collects an error and adds it to the internal list.
	/// </summary>
	/// <param name="methodName">Name of the method where the error occurred.</param>
	/// <param name="ex">The exception that occurred.</param>
	/// <param name="message">Optional custom message.</param>
	public static void AddError(string methodName, Exception ex, string message = null) {
		var error = new ErrorInfo {
			ClassName = typeof(T).Name,
			MethodName = methodName,
			Exception = ex,
			Message = message ?? ex.Message
		};
		lock (_lock) _errors.Add(error);
		Trace.WriteLine(error.ToString());
		Logger.LogError(ex, "{ClassName}.{MethodName}: {Message}", error.ClassName, error.MethodName, error.Message);
		ErrorCaptured?.Invoke(error);
	}

	/// <summary>
	/// Gets all collected errors as a read-only list.
	/// </summary>
	/// <returns>Read-only list of collected errors.</returns>
	public static IReadOnlyList<ErrorInfo> GetErrors() {
		lock (_lock) return _errors.AsReadOnly();
	}

	/// <summary>
	/// Clears all collected errors from the internal list.
	/// </summary>
	public static void Clear() {
		lock (_lock) _errors.Clear();
	}
}

/// <summary>
/// Centralized static error collector (non-generic) that does not depend on any logging framework.
/// </summary>
public static class ErrorCollector {

	/// <summary>
	/// Internal list to store collected errors.
	/// </summary>
	private static readonly List<ErrorInfo> _errors = [];
	/// <summary>
	/// Lock object for thread safety.
	/// </summary>
	private static readonly object _lock = new();
	private static readonly ILogger Logger;

	/// <summary>
	/// Event that is triggered when a new error is captured.
	/// </summary>
	public static event Action<ErrorInfo> ErrorCaptured;

	/// <summary>
	/// Static constructor to initialize the logger.
	/// </summary>
	static ErrorCollector() {
		Logger = (ILogger<ErrorInfo>)GetStaticLogger<ErrorInfo>();
	}

	/// <summary>
	/// Collects an error and adds it to the internal list.
	/// </summary>
	/// <param name="className">Name of the class where the error occurred.</param>
	/// <param name="methodName">Name of the method where the error occurred.</param>
	/// <param name="ex">The exception that occurred.</param>
	/// <param name="message">Optional custom message.</param>
	public static void AddError(string className, string methodName, Exception ex, string message = null) {
		var error = new ErrorInfo {
			ClassName = className,
			MethodName = methodName,
			Exception = ex,
			Message = message ?? ex.Message
		};
		lock (_lock) _errors.Add(error);
		Trace.WriteLine(error.ToString());
		Debug.WriteLine(error.ToString());
		Logger.LogError(ex, "{ClassName}.{MethodName}: {Message}", error.ClassName, error.MethodName, error.Message);
		ErrorCaptured?.Invoke(error);
	}

	/// <summary>
	/// Gets all collected errors as a read-only list.
	/// </summary>
	/// <returns>Read-only list of collected errors.</returns>
	public static IReadOnlyList<ErrorInfo> GetErrors() {
		lock (_lock) return _errors.AsReadOnly();
	}

	/// <summary>
	/// Clears all collected errors from the internal list.
	/// </summary>
	public static void Clear() {
		lock (_lock) _errors.Clear();
	}
}
