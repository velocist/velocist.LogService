using Autofac;
namespace velocist.LogService {

	/// <summary>
	/// Container for inject services 
	/// </summary>
	[Obsolete("Check use.")]
	public class LogServiceContainer {

		/// <summary>
		/// Gets the container.
		/// </summary>
		/// <value>
		/// The container.
		/// </value>
		public static IContainer Container { get; private set; }

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <value>
		/// The configuration.
		/// </value>
		public static IConfiguration Configuration { get; private set; }

		/// <summary>
		/// Initializes the <see cref="LogServiceContainer"/> class.
		/// </summary>
		static LogServiceContainer() {

			var builder = new ContainerBuilder();

			Configuration = LogServiceConfiguration.GetConfiguration();

			//Register logging service
			_ = builder.RegisterInstance(new LoggerFactoryApp(Configuration)).As<ILoggerFactory>();
			_ = builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
			Container = builder.Build();
		}

		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <typeparam name="T">The type of the log</typeparam>
		/// <returns>Returns the log of type T</returns>
		public static ILogger<T> GetLog<T>() => Container.Resolve<ILogger<T>>();

		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <returns></returns>
		public static ILogger GetLog() => Container.Resolve<ILogger>();

		/// <summary>
		/// Resolves this instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Resolve<T>() => Container.Resolve<T>();

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		/// <returns>Return configuration.</returns>
		public static IConfiguration GetConfiguration() => Configuration;

	}
}
