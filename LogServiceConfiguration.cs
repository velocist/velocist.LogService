namespace velocist.LogService {

	/// <summary>
	/// Class to configuaration log
	/// </summary>
	internal class LogServiceConfiguration {

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <value>
		/// The configuration.
		/// </value>
		public static IConfiguration Configuration { get; private set; }

		/// <summary>
		/// Initializes the <see cref="LogServiceConfiguration"/> class.
		/// </summary>
		static LogServiceConfiguration() {
			//_configuration = velocist.Configuration.ConfigurationService.Configuration;
			Configuration = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile($"{LogServiceSettings.LogSettingsFile}.json", optional: false)
			   .Build();
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns>Return configuration.</returns>
		public static IConfiguration GetConfiguration() => Configuration;

	}
}
