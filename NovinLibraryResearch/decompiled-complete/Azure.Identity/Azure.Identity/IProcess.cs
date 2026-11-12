using System;
using System.Diagnostics;

namespace Azure.Identity;

internal interface IProcess : IDisposable
{
	bool HasExited { get; }

	int ExitCode { get; }

	ProcessStartInfo StartInfo { get; set; }

	event EventHandler Exited;

	event DataReceivedEventHandler OutputDataReceived;

	event DataReceivedEventHandler ErrorDataReceived;

	bool Start();

	void Kill();

	void BeginOutputReadLine();

	void BeginErrorReadLine();
}
