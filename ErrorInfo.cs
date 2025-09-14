namespace velocist.LogService;
/// <summary>
/// Represents the structure for error information, including timestamp, class, method, exception, and message.
/// </summary>
public class ErrorInfo {
	/// <summary>
	/// The date and time when the error was captured.
	/// </summary>
	public DateTime Timestamp { get; set; } = DateTime.Now;
	/// <summary>
	/// The name of the class where the error occurred.
	/// </summary>
	public string ClassName { get; set; }
	/// <summary>
	/// The name of the method where the error occurred.
	/// </summary>
	public string MethodName { get; set; }
	/// <summary>
	/// The exception object associated with the error.
	/// </summary>
	public Exception Exception { get; set; }
	/// <summary>
	/// The error message or additional information.
	/// </summary>
	public string Message { get; set; }
	/// <summary>
	/// Returns a string representation of the error information.
	/// </summary>
	/// <returns>Formatted string with timestamp, class, method, and message.</returns>
	public override string ToString() => $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {ClassName}.{MethodName}: {Message}";
}