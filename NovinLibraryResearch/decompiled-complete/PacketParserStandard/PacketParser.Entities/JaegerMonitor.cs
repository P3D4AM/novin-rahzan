using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Timers;
using PacketParser.Performance.Monitor;

namespace PacketParser.Entities;

public class JaegerMonitor : IDisposable
{
	private bool _isDisposed;

	private readonly string _memberName;

	private readonly string _sourceFilePath;

	private readonly Stopwatch _elapssedTimer;

	private Timer timer;

	public static bool IsEnable { get; set; }

	public static DateTime ExpireDate { get; set; }

	public int TimeoutPeriod { get; }

	public JaegerMonitor([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", int timeOutPriod = 60000)
	{
		if (GetIsEnable())
		{
			_memberName = memberName;
			_sourceFilePath = sourceFilePath;
			_elapssedTimer = Stopwatch.StartNew();
			TimeoutPeriod = timeOutPriod;
			timer = new Timer(timeOutPriod);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}
	}

	private bool GetIsEnable()
	{
		if (IsEnable && ExpireDate >= DateTime.Now)
		{
			return TraceStatistician.Instance != null;
		}
		return false;
	}

	private void Timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		try
		{
			if (!_isDisposed)
			{
				Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer_Elapsed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\JaegerMonitor.cs", 49);
		}
	}

	private void UpdateStatistics()
	{
		try
		{
			if (GetIsEnable() && _elapssedTimer != null)
			{
				TraceStatistician.Instance.Post(new WebPerfMonitor
				{
					ExcuteCount = 1,
					Duration = Convert.ToInt32(_elapssedTimer.ElapsedMilliseconds),
					ClassName = _sourceFilePath.GetClassName(),
					FuncName = _memberName
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateStatistics", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\JaegerMonitor.cs", 67);
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (timer != null)
				{
					timer.Elapsed -= Timer_Elapsed;
				}
				timer?.Stop();
				timer?.Dispose();
				timer = null;
				_elapssedTimer?.Stop();
				UpdateStatistics();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\JaegerMonitor.cs", 85);
		}
	}
}
