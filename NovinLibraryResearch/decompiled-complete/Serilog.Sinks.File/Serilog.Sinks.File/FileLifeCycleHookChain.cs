using System;
using System.IO;
using System.Text;

namespace Serilog.Sinks.File;

internal class FileLifeCycleHookChain : FileLifecycleHooks
{
	private readonly FileLifecycleHooks _first;

	private readonly FileLifecycleHooks _second;

	public FileLifeCycleHookChain(FileLifecycleHooks first, FileLifecycleHooks second)
	{
		_first = first ?? throw new ArgumentNullException("first");
		_second = second ?? throw new ArgumentNullException("second");
	}

	public override Stream OnFileOpened(string path, Stream underlyingStream, Encoding encoding)
	{
		Stream underlyingStream2 = _first.OnFileOpened(path, underlyingStream, encoding);
		return _second.OnFileOpened(path, underlyingStream2, encoding);
	}

	public override void OnFileDeleting(string path)
	{
		_first.OnFileDeleting(path);
		_second.OnFileDeleting(path);
	}
}
