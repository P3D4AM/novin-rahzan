using System;
using System.IO;
using System.Text;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Performance;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File;

namespace AccSqlServerPersistence.AssistanceClasses;

public class ConnectionLogger
{
	private static ENSource _source = ENSource.Accounting;

	private static string _assembleVersion = "0";

	private static string _path;

	private static long _lkSerial = 0L;

	public static void Init(string assembleVersion, ENSource source, long lkSerial, string path)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_source = source;
			_assembleVersion = assembleVersion;
			_lkSerial = lkSerial;
			_path = path;
			Logger logger = EnvironmentLoggerConfigurationExtensions.WithMachineName(EnvironmentLoggerConfigurationExtensions.WithEnvironmentUserName(new LoggerConfiguration().MinimumLevel.Override("Microsoft", (LogEventLevel)4).MinimumLevel.Override("System", (LogEventLevel)4).MinimumLevel.Verbose().Enrich.FromLogContext().Enrich.With<CallerEnricher>().Enrich).Enrich).Enrich.WithProperty("Source", (object)_source, false).Enrich.WithProperty("Version", (object)_assembleVersion, false).Enrich.WithProperty("Lk", (object)_lkSerial, false).Enrich.WithProperty("Path", (object)_path, false).CreateLogger();
			LogCache.Instance.Value.Set("ConnectionLogger", (ILogger)(object)logger);
			Logger logger2 = LoggerConfigurationAsyncExtensions.Async(EnvironmentLoggerConfigurationExtensions.WithMachineName(EnvironmentLoggerConfigurationExtensions.WithEnvironmentUserName(new LoggerConfiguration().MinimumLevel.Override("Microsoft", (LogEventLevel)4).MinimumLevel.Override("System", (LogEventLevel)4).MinimumLevel.Verbose().Enrich.FromLogContext().Enrich.With<CallerEnricher>().Enrich).Enrich).Enrich.WithProperty("Source", (object)_source, false).Enrich.WithProperty("Version", (object)_assembleVersion, false).Enrich.WithProperty("Lk", (object)_lkSerial, false).Enrich.WithProperty("Path", (object)_path, false).WriteTo, (Action<LoggerSinkConfiguration>)delegate(LoggerSinkConfiguration a)
			{
				string text = Path.Combine(path, "logger", "Hesabyar.log");
				int? num = 15;
				TimeSpan? timeSpan = TimeSpan.FromSeconds(10.0);
				FileLoggerConfigurationExtensions.File(a, text, (LogEventLevel)2, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", (IFormatProvider)null, (long?)1073741824L, (LoggingLevelSwitch)null, true, false, timeSpan, (RollingInterval)4, true, num, (Encoding)null, (FileLifecycleHooks)null, (TimeSpan?)null);
			}, 10000, false).CreateLogger();
			LogCache.Instance.Value.Set("HesabYarLogger", (ILogger)(object)logger2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\AssistanceClasses\\ConnectionLogger.cs", 86);
		}
	}
}
