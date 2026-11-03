using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.Performance.Monitor;

public class TraceStatistician : IDisposable
{
	private CancellationTokenSource _disposedToken = new CancellationTokenSource();

	private static int sendToNpApiInterval_seconds = 300000;

	private readonly ITraceSummaryMetrics metrics;

	private static DateTime _nextSendToNpApi = DateTime.Now.AddSeconds(60.0);

	private bool hasError;

	public static TraceStatistician Instance { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	private BufferBlock<WebPerfMonitor> Ingester { get; set; } = new BufferBlock<WebPerfMonitor>(new DataflowBlockOptions
	{
		BoundedCapacity = 1000
	});


	public static long LKSerial { get; set; }

	public static ENSource ENSource { get; set; } = ENSource.None;


	public static int Version { get; set; }

	public static string VersionStr { get; set; }

	public static long AndroidIme { get; set; }

	public static string Username { get; set; }

	public static string Password { get; set; }

	public bool IsDisposed => _disposedToken?.Token.IsCancellationRequested ?? true;

	public static int SendToNpApiInterval_seconds
	{
		get
		{
			return sendToNpApiInterval_seconds;
		}
		set
		{
			sendToNpApiInterval_seconds = value;
			_nextSendToNpApi = DateTime.Now.AddSeconds(value);
		}
	}

	public bool Post(WebPerfMonitor item)
	{
		try
		{
			if (hasError)
			{
				return false;
			}
			return Ingester.Post(item);
		}
		catch (Exception ex)
		{
			hasError = true;
			WebErrorLog.Instance.StartLog(ex, "", "Post", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Monitor\\TraceStatistician.cs", 37);
			return false;
		}
	}

	public TraceStatistician(ITraceSummaryMetrics metrics = null)
	{
		TraceStatistician traceStatistician = this;
		Instance?.Dispose();
		Instance = this;
		if (metrics != null)
		{
			this.metrics = metrics;
		}
		_disposedToken?.Cancel();
		_disposedToken = new CancellationTokenSource();
		CancellationToken token = _disposedToken.Token;
		Task.Run(() => traceStatistician.IngestionLoop(token));
	}

	private async Task IngestionLoop(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				_ = 1;
				try
				{
					if (token.IsCancellationRequested)
					{
						goto end_IL_003c;
					}
					WebPerfMonitor item = await Ingester.ReceiveAsync(token);
					Ingester.TryReceiveAll(out IList<WebPerfMonitor> items);
					if (items == null)
					{
						items = new List<WebPerfMonitor>();
					}
					items.Add(item);
					List<WebPerfMonitor> list = (from p in items
						group p by new { p.ClassName, p.FuncName } into p
						select new WebPerfMonitor
						{
							ClassName = p.Key.ClassName,
							FuncName = p.Key.FuncName,
							Duration = p.Sum((WebPerfMonitor q) => q.Duration),
							ExcuteCount = p.Sum((WebPerfMonitor q) => q.ExcuteCount)
						}).ToList();
					if (items.Count > 1000)
					{
						string text = string.Join("\r\n", from p in list.OrderByDescending((WebPerfMonitor p) => p.ExcuteCount).Take(10)
							select $"{p.ClassName}.{p.FuncName} - count({p.ExcuteCount})") ?? "";
						WebErrorLog.Instance.StartLog("Monitors overflow. " + $"to prevent overflow exception removed all monitored data! count:{items.Count}\r\n" + "all of them has been cleared!\r\n" + $"dic.count:{list.Count}\r\n" + "\r\n" + text, "", "IngestionLoop", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Monitor\\TraceStatistician.cs", 122);
					}
					token.ThrowIfCancellationRequested();
					PerfMonitorStandard monitorLog = new PerfMonitorStandard
					{
						Monitors = list,
						AndroidIme = AndroidIme,
						Source = ENSource,
						LkSerial = LKSerial,
						Version = Version,
						MonitorVersion = 3,
						Username = Username,
						Password = Password,
						VersionName = VersionStr
					};
					await SendLog(monitorLog);
					goto end_IL_002d;
					end_IL_003c:;
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "Error while updating trace statistics", "IngestionLoop", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Monitor\\TraceStatistician.cs", 145);
					goto end_IL_002d;
				}
				break;
				end_IL_002d:;
			}
			finally
			{
				await Task.Delay(60000);
			}
		}
	}

	private async Task<bool> SendLog(PerfMonitorStandard monitorLog, int retrycount = 10)
	{
		try
		{
			string url = "http://novinrank.ir/api/monitorlogservice";
			await HttpHelpers.PostStreamAsync(monitorLog, url, default(CancellationToken));
			monitorLog.Dispose();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Monitor\\TraceStatistician.cs", 161);
			return false;
		}
	}

	public void Dispose()
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			_disposedToken?.Cancel();
			_disposedToken?.Dispose();
			_disposedToken = null;
			if (Ingester != null)
			{
				WebPerfMonitor item;
				while (Ingester.TryReceive(out item))
				{
				}
			}
			Ingester?.Complete();
			Ingester = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Monitor\\TraceStatistician.cs", 182);
		}
	}
}
