using System;
using System.Diagnostics;

namespace Azure.Identity;

internal class ProcessService : IProcessService
{
	private class ProcessWrapper : IProcess, IDisposable
	{
		private readonly Process _process;

		public bool HasExited => _process.HasExited;

		public int ExitCode => _process.ExitCode;

		public ProcessStartInfo StartInfo
		{
			get
			{
				return _process.StartInfo;
			}
			set
			{
				_process.StartInfo = value;
			}
		}

		public event EventHandler Exited
		{
			add
			{
				_process.Exited += value;
			}
			remove
			{
				_process.Exited -= value;
			}
		}

		public event DataReceivedEventHandler OutputDataReceived
		{
			add
			{
				_process.OutputDataReceived += value;
			}
			remove
			{
				_process.OutputDataReceived -= value;
			}
		}

		public event DataReceivedEventHandler ErrorDataReceived
		{
			add
			{
				_process.ErrorDataReceived += value;
			}
			remove
			{
				_process.ErrorDataReceived -= value;
			}
		}

		public ProcessWrapper(ProcessStartInfo processStartInfo)
		{
			_process = new Process
			{
				StartInfo = processStartInfo,
				EnableRaisingEvents = true
			};
		}

		public bool Start()
		{
			return _process.Start();
		}

		public void Kill()
		{
			_process.Kill();
		}

		public void BeginOutputReadLine()
		{
			_process.BeginOutputReadLine();
		}

		public void BeginErrorReadLine()
		{
			_process.BeginErrorReadLine();
		}

		public void Dispose()
		{
			_process.Dispose();
		}
	}

	public static IProcessService Default { get; } = new ProcessService();


	private ProcessService()
	{
	}

	public IProcess Create(ProcessStartInfo startInfo)
	{
		return new ProcessWrapper(startInfo);
	}
}
