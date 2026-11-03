using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using PacketParser.Entities;

namespace PacketParser;

public class SemaphoreUtil : IDisposable
{
	private bool _isDisposed;

	private SemaphoreSlim _sem;

	private CancellationTokenSource source;

	private readonly bool _awaitOnSemaphore;

	private readonly int timeout;

	private readonly string callerName;

	private System.Timers.Timer timer;

	private short _disposingCount;

	public bool SemaphoreHasBeenTaken { get; private set; }

	public CancellationToken Token => source?.Token ?? new CancellationToken(canceled: true);

	public bool IsDisposed => _isDisposed;

	public SemaphoreUtil(SemaphoreSlim sem, bool awaitOnSemaphore, int timeout, string callerName, params CancellationToken[] tokens)
	{
		if (string.IsNullOrEmpty(callerName))
		{
			throw new NullReferenceException("Please fill callerName with ClassName.FunctionName");
		}
		try
		{
			_awaitOnSemaphore = awaitOnSemaphore;
			_sem = sem ?? throw new ArgumentNullException("sem");
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			List<CancellationToken> list = new List<CancellationToken> { cancellationTokenSource.Token };
			if (tokens != null && tokens.Any())
			{
				foreach (CancellationToken item in tokens)
				{
					list.Add(item);
				}
			}
			source = CancellationTokenSource.CreateLinkedTokenSource(list.ToArray());
		}
		catch (OperationCanceledException)
		{
			Dispose(disposing: true);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\SemaphoreUtil.cs", 62);
			Dispose(disposing: true);
		}
		this.timeout = timeout;
		this.callerName = callerName;
	}

	internal async Task Start()
	{
		if (_awaitOnSemaphore)
		{
			timer = new System.Timers.Timer(timeout);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
			await _sem.WaitAsync(source.Token);
			SemaphoreHasBeenTaken = true;
		}
	}

	private void Timer_Elapsed(object sender, ElapsedEventArgs e)
	{
		try
		{
			if (!IsDisposed)
			{
				if (SemaphoreHasBeenTaken && DateTime.Now.Year > 2025)
				{
					throw new Exception("Caller:" + callerName + "\r\n" + $"timeOut:{timeout}");
				}
				Dispose(disposing: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer_Elapsed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\SemaphoreUtil.cs", 95);
			Dispose(disposing: true);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		checked
		{
			lock (this)
			{
				if (_isDisposed)
				{
					return;
				}
				try
				{
					_disposingCount++;
					_isDisposed = true;
					if (timer != null)
					{
						timer.Elapsed -= Timer_Elapsed;
					}
					timer?.Close();
					timer?.Dispose();
					timer = null;
					if (SemaphoreHasBeenTaken)
					{
						_sem?.Release();
						_sem = null;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"DisposingCount:{_disposingCount}", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\SemaphoreUtil.cs", 126);
				}
				finally
				{
					if (_disposingCount > 1)
					{
						throw new Exception($"disposed Count is :{_disposingCount}");
					}
					source?.Cancel();
					source?.Dispose();
					source = null;
				}
			}
		}
	}

	~SemaphoreUtil()
	{
		Dispose(disposing: false);
	}
}
