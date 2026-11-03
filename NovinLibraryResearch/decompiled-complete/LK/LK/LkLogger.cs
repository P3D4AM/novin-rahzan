using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Performance;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace LK;

public class LkLogger
{
	private static ENSource _source = ENSource.Accounting;

	private static string _assembleVersion = "0";

	private static string _path;

	private static long _lkSerial = 0L;

	public static void Init(string assembleVersion, ENSource source, long lkSerial, string path)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_source = source;
			_assembleVersion = assembleVersion;
			_lkSerial = lkSerial;
			_path = path;
			Logger logger = EnvironmentLoggerConfigurationExtensions.WithMachineName(EnvironmentLoggerConfigurationExtensions.WithEnvironmentUserName(new LoggerConfiguration().MinimumLevel.Override("Microsoft", (LogEventLevel)4).MinimumLevel.Override("System", (LogEventLevel)4).MinimumLevel.Verbose().Enrich.FromLogContext().Enrich.With<CallerEnricher>().Enrich).Enrich).Enrich.WithProperty("Source", (object)_source, false).Enrich.WithProperty("Version", (object)_assembleVersion, false).Enrich.WithProperty("Lk", (object)_lkSerial, false).Enrich.WithProperty("Path", (object)_path, false).CreateLogger();
			LogCache.Instance.Value.Set("LkLogger", (ILogger)(object)logger);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LkLogger.cs", 51);
		}
	}
}
