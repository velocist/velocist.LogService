using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace velocist.LogService {

    [Obsolete]
    /// <summary>
    /// Configure services log
    /// </summary>
    internal static class LogServiceExtensions {

        /// <summary>
        /// Adds the service log.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddServiceLog(this IServiceCollection services) => services.AddLogging(optonsLog => {
            LogServiceConfiguration.GetConfiguration();
            //optonsLog.ClearProviders();
            //var loggingConfig = LogServiceConfiguration.GetConfiguration().GetSection(LogServiceSettings.SectionLogging);
            //optonsLog.AddConfiguration(loggingConfig);
            ////builder.SetMinimumLevel(LogLevel.Debug);
            ////builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Debug);
            //optonsLog.AddConsole();
            //optonsLog.AddDebug();
        });
    }
}
