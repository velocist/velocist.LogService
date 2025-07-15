namespace velocist.LogService {

	/// <summary>
	/// The logger factory for application
	/// </summary>
	/// <seealso cref="ILoggerFactory" />
	internal class LoggerFactoryApp : ILoggerFactory {

		private readonly IConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggerFactoryApp"/> class.
		/// </summary>
		/// <param name="_configuration">The configuration.</param>
		public LoggerFactoryApp(IConfiguration _configuration) {
			configuration = _configuration;
		}

		/// <summary>
		/// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
		/// </summary>
		/// <param name="categoryName">The category name for messages produced by the logger.</param>
		/// <returns>
		/// The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.
		/// </returns>
		ILogger ILoggerFactory.CreateLogger(string categoryName) => LoggerFactory.Create(builder => {
			//builder.ClearProviders();
			var loggingConfig = configuration.GetSection(LogServiceSettings.SectionLogging);
			builder.AddConfiguration(loggingConfig);
#if DEBUG
			//builder.SetMinimumLevel(LogLevel.Trace);
			builder.AddLog4Net($"{LogServiceSettings.Log4NetFile}.config", true);
			//builder.AddDebug();
			//builder.AddConsole();
			//builder.AddSimpleConsole();
#else
            builder.AddLog4Net($"{LogServiceSettings.Log4NetFile}.config", true);
#endif
			builder.AddConsole();
		}).CreateLogger(categoryName);

		void ILoggerFactory.AddProvider(ILoggerProvider provider) => throw new NotImplementedException();

		private bool disposedValue;

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing) {
			if (!disposedValue) {
				if (disposing) {
					// TODO: eliminar el estado administrado (objetos administrados)
				}

				// TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
				// TODO: establecer los campos grandes como NULL
				disposedValue = true;
			}
		}

		// // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
		// ~Logger()
		// {
		//     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
		//     Dispose(disposing: false);
		// }

		void IDisposable.Dispose() {
			// No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}