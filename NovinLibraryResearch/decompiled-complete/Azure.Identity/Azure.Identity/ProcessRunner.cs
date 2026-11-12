using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Identity;

internal sealed class ProcessRunner : IDisposable
{
	private readonly IProcess _process;

	private readonly TimeSpan _timeout;

	private readonly TaskCompletionSource<string> _tcs;

	private readonly TaskCompletionSource<ICollection<string>> _outputTcs;

	private readonly TaskCompletionSource<ICollection<string>> _errorTcs;

	private readonly ICollection<string> _outputData;

	private readonly ICollection<string> _errorData;

	private readonly CancellationToken _cancellationToken;

	private readonly CancellationTokenSource _timeoutCts;

	private CancellationTokenRegistration _ctRegistration;

	public ProcessRunner(IProcess process, TimeSpan timeout, CancellationToken cancellationToken)
	{
		_process = process;
		_timeout = timeout;
		_outputData = new List<string>();
		_errorData = new List<string>();
		_outputTcs = new TaskCompletionSource<ICollection<string>>(TaskCreationOptions.RunContinuationsAsynchronously);
		_errorTcs = new TaskCompletionSource<ICollection<string>>(TaskCreationOptions.RunContinuationsAsynchronously);
		_tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
		if (timeout.TotalMilliseconds >= 0.0)
		{
			_timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { cancellationToken });
			_cancellationToken = _timeoutCts.Token;
		}
		else
		{
			_cancellationToken = cancellationToken;
		}
	}

	public Task<string> RunAsync()
	{
		StartProcess();
		return _tcs.Task;
	}

	public string Run()
	{
		StartProcess();
		return _tcs.Task.GetAwaiter().GetResult();
	}

	private void StartProcess()
	{
		if (!TrySetCanceled() && !_tcs.Task.IsCompleted)
		{
			_process.StartInfo.UseShellExecute = false;
			_process.StartInfo.RedirectStandardOutput = true;
			_process.StartInfo.RedirectStandardError = true;
			_process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				OnDataReceived(args, _outputData, _outputTcs);
			};
			_process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs args)
			{
				OnDataReceived(args, _errorData, _errorTcs);
			};
			_process.Exited += delegate
			{
				HandleExitAsync();
			};
			_timeoutCts?.CancelAfter(_timeout);
			if (!_process.Start())
			{
				TrySetException(new InvalidOperationException("Failed to start process '" + _process.StartInfo.FileName + "'"));
			}
			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
			CancellationToken cancellationToken = _cancellationToken;
			_ctRegistration = cancellationToken.Register(HandleCancel, useSynchronizationContext: false);
		}
	}

	private async ValueTask HandleExitAsync()
	{
		if (_process.ExitCode == 0)
		{
			ICollection<string> values = await _outputTcs.Task.ConfigureAwait(continueOnCapturedContext: false);
			TrySetResult(string.Join(Environment.NewLine, values));
		}
		else
		{
			ICollection<string> values2 = await _errorTcs.Task.ConfigureAwait(continueOnCapturedContext: false);
			TrySetException(new InvalidOperationException(string.Join(Environment.NewLine, values2)));
		}
	}

	private void HandleCancel()
	{
		if (_tcs.Task.IsCompleted)
		{
			return;
		}
		if (!_process.HasExited)
		{
			try
			{
				_process.Kill();
			}
			catch (Exception exception)
			{
				TrySetException(exception);
				return;
			}
		}
		TrySetCanceled();
	}

	private static void OnDataReceived(DataReceivedEventArgs args, ICollection<string> data, TaskCompletionSource<ICollection<string>> tcs)
	{
		if (args.Data != null)
		{
			data.Add(args.Data);
		}
		else
		{
			tcs.SetResult(data);
		}
	}

	private void TrySetResult(string result)
	{
		_tcs.TrySetResult(result);
	}

	private bool TrySetCanceled()
	{
		CancellationToken cancellationToken = _cancellationToken;
		if (cancellationToken.IsCancellationRequested)
		{
			_tcs.TrySetCanceled(_cancellationToken);
		}
		cancellationToken = _cancellationToken;
		return cancellationToken.IsCancellationRequested;
	}

	private void TrySetException(Exception exception)
	{
		_tcs.TrySetException(exception);
	}

	public void Dispose()
	{
		_tcs.TrySetCanceled();
		_process.Dispose();
		_ctRegistration.Dispose();
		_timeoutCts?.Dispose();
	}
}
