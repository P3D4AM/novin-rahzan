using System.IO;
using System.Text;

namespace Serilog.Sinks.File;

public abstract class FileLifecycleHooks
{
	public virtual Stream OnFileOpened(string path, Stream underlyingStream, Encoding encoding)
	{
		return OnFileOpened(underlyingStream, encoding);
	}

	public virtual Stream OnFileOpened(Stream underlyingStream, Encoding encoding)
	{
		return underlyingStream;
	}

	public virtual void OnFileDeleting(string path)
	{
	}

	public FileLifecycleHooks Then(FileLifecycleHooks next)
	{
		return new FileLifeCycleHookChain(this, next);
	}
}
