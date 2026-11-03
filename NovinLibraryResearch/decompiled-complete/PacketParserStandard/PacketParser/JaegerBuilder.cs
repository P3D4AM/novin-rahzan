using System;
using PacketParser.Entities;
using PacketParser.Performance.Monitor;
using Serilog;

namespace PacketParser;

public class JaegerBuilder
{
	private string depUser_Username = "";

	private string serviceName = ENSource.None.ToString();

	private ILogger logger;

	private bool consoleExporter;

	private ITraceSummaryMetrics _metricsEngine;

	public JaegerBuilder()
	{
		logger = Log.Logger;
	}

	public JaegerBuilder(ILogger logger)
	{
		this.logger = logger;
	}

	public JaegerBuilder SetIme(long ime)
	{
		TraceStatistician.AndroidIme = ime;
		return this;
	}

	public JaegerBuilder SetVersion(string version)
	{
		TraceStatistician.VersionStr = version;
		return this;
	}

	public JaegerBuilder SetVersion(int version)
	{
		TraceStatistician.Version = version;
		return this;
	}

	public JaegerBuilder SetSource(ENSource source)
	{
		TraceStatistician.ENSource = source;
		return this;
	}

	public JaegerBuilder SetLock(long lk)
	{
		TraceStatistician.LKSerial = lk;
		return this;
	}

	public JaegerBuilder SetEnabled(bool isActive)
	{
		JaegerMonitor.IsEnable = isActive;
		return this;
	}

	public JaegerBuilder SetExpireDate(DateTime date)
	{
		JaegerMonitor.ExpireDate = date;
		return this;
	}

	public JaegerBuilder SetUsername(string username)
	{
		depUser_Username = username;
		TraceStatistician.Username = username;
		return this;
	}

	public JaegerBuilder SetServiceName(string serviceName)
	{
		this.serviceName = serviceName;
		return this;
	}

	public JaegerBuilder SetPassword(string password)
	{
		TraceStatistician.Password = password;
		return this;
	}

	public JaegerBuilder SetMetrics(ITraceSummaryMetrics metricsEngine = null)
	{
		new TraceStatistician(metricsEngine);
		if (metricsEngine != null)
		{
			_metricsEngine = metricsEngine;
		}
		return this;
	}

	public JaegerBuilder AddConsoleExporter()
	{
		consoleExporter = true;
		return this;
	}

	public void Build()
	{
		if (JaegerMonitor.IsEnable)
		{
			if (TraceStatistician.ENSource == ENSource.None)
			{
				throw new InvalidOperationException("Source has not been set");
			}
			new TraceStatistician(_metricsEngine);
		}
	}
}
