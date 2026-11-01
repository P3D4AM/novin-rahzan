using System;
using System.Text;
using System.Windows.Forms;
using FormCustom;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Performance;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole.Themes;

namespace TimeAttendanceDAL.Logger;

public static class AttLogger
{
	private static Logger loggerUi;

	private static Logger loggerException;

	private static Logger loggerErrorRSFI;

	private static ENSource _source = (ENSource)5;

	private static string _assembleVersion = "0";

	public static Logger LoggerUi
	{
		get
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (loggerUi == null)
			{
				loggerUi = LoggerConfigurationAsyncExtensions.Async(ConsoleLoggerConfigurationExtensions.Console(EnvironmentLoggerConfigurationExtensions.WithMachineName(EnvironmentLoggerConfigurationExtensions.WithEnvironmentUserName(new LoggerConfiguration().MinimumLevel.Override("Microsoft", (LogEventLevel)4).MinimumLevel.Override("System", (LogEventLevel)4).MinimumLevel.Verbose().Enrich.FromLogContext().Enrich.With<CallerEnricher>().Enrich).Enrich).Enrich.WithProperty("Source", (object)_source, false).Enrich.WithProperty("Version", (object)_assembleVersion, false).Enrich.WithProperty("Lk", (object)RegistryManager.GetLkSerial(), false).Enrich.WithProperty("Path", (object)Application.StartupPath, false).WriteTo, (LogEventLevel)0, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null, (LoggingLevelSwitch)null, (LogEventLevel?)null, (ConsoleTheme)null, false, (object)null).WriteTo, (Action<LoggerSinkConfiguration>)delegate(LoggerSinkConfiguration a)
				{
					int? num = 30;
					TimeSpan? timeSpan = TimeSpan.FromSeconds(2.0);
					FileLoggerConfigurationExtensions.File(a, "logger/LoggerUi/logUi.log", (LogEventLevel)1, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null, (long?)1073741824L, (LoggingLevelSwitch)null, false, true, timeSpan, (RollingInterval)4, true, num, (Encoding)null, (FileLifecycleHooks)null, (TimeSpan?)null);
				}, 10000, false).CreateLogger();
				LogCache.Instance.Value.Set("LoggerUi", (ILogger)(object)LoggerUi);
			}
			return loggerUi;
		}
		private set
		{
			loggerUi = value;
		}
	}

	public static Logger LoggerException
	{
		get
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (loggerException == null)
			{
				loggerException = LoggerConfigurationAsyncExtensions.Async(ConsoleLoggerConfigurationExtensions.Console(EnvironmentLoggerConfigurationExtensions.WithMachineName(EnvironmentLoggerConfigurationExtensions.WithEnvironmentUserName(new LoggerConfiguration().MinimumLevel.Override("Microsoft", (LogEventLevel)4).MinimumLevel.Override("System", (LogEventLevel)4).MinimumLevel.Verbose().Enrich.FromLogContext().Enrich.With<CallerEnricher>().Enrich).Enrich).Enrich.WithProperty("Source", (object)_source, false).Enrich.WithProperty("Version", (object)_assembleVersion, false).Enrich.WithProperty("Lk", (object)RegistryManager.GetLkSerial(), false).Enrich.WithProperty("Path", (object)Application.StartupPath, false).WriteTo, (LogEventLevel)0, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null, (LoggingLevelSwitch)null, (LogEventLevel?)null, (ConsoleTheme)null, false, (object)null).WriteTo, (Action<LoggerSinkConfiguration>)delegate(LoggerSinkConfiguration a)
				{
					int? num = 30;
					TimeSpan? timeSpan = TimeSpan.FromSeconds(2.0);
					FileLoggerConfigurationExtensions.File(a, "logger/LoggerException/logException.log", (LogEventLevel)1, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null, (long?)1073741824L, (LoggingLevelSwitch)null, false, true, timeSpan, (RollingInterval)4, true, num, (Encoding)null, (FileLifecycleHooks)null, (TimeSpan?)null);
				}, 10000, false).CreateLogger();
				LogCache.Instance.Value.Set("LoggerException", (ILogger)(object)LoggerException);
			}
			return loggerException;
		}
		private set
		{
			loggerException = value;
		}
	}

	public static Logger LoggerErrorRSFI
	{
		get
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (loggerErrorRSFI == null)
			{
				loggerErrorRSFI = LoggerConfigurationAsyncExtensions.Async(ConsoleLoggerConfigurationExtensions.Console(EnvironmentLoggerConfigurationExtensions.WithMachineName(EnvironmentLoggerConfigurationExtensions.WithEnvironmentUserName(new LoggerConfiguration().MinimumLevel.Override("Microsoft", (LogEventLevel)4).MinimumLevel.Override("System", (LogEventLevel)4).MinimumLevel.Verbose().Enrich.FromLogContext().Enrich.With<CallerEnricher>().Enrich).Enrich).Enrich.WithProperty("Source", (object)_source, false).Enrich.WithProperty("Version", (object)_assembleVersion, false).Enrich.WithProperty("Lk", (object)RegistryManager.GetLkSerial(), false).Enrich.WithProperty("Path", (object)Application.StartupPath, false).WriteTo, (LogEventLevel)0, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null, (LoggingLevelSwitch)null, (LogEventLevel?)null, (ConsoleTheme)null, false, (object)null).WriteTo, (Action<LoggerSinkConfiguration>)delegate(LoggerSinkConfiguration a)
				{
					int? num = 30;
					TimeSpan? timeSpan = TimeSpan.FromSeconds(2.0);
					FileLoggerConfigurationExtensions.File(a, "logger/LoggerErrorRSFI/logErrorRSFI.log", (LogEventLevel)1, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null, (long?)1073741824L, (LoggingLevelSwitch)null, false, true, timeSpan, (RollingInterval)4, true, num, (Encoding)null, (FileLifecycleHooks)null, (TimeSpan?)null);
				}, 10000, false).CreateLogger();
				LogCache.Instance.Value.Set("LoggerException", (ILogger)(object)LoggerException);
			}
			return loggerErrorRSFI;
		}
		private set
		{
			loggerErrorRSFI = value;
		}
	}

	public static void init(string assembleVersion, ENSource source)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		_source = source;
		_assembleVersion = assembleVersion;
	}
}
