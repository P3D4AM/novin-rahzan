using System;
using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Writers;

public abstract class AbstractWriter : IWriter, IDisposable
{
	private bool _isDisposed;

	protected Stream OutputStream { get; private set; }

	public ArchiveType WriterType { get; }

	protected WriterOptions WriterOptions { get; }

	protected AbstractWriter(ArchiveType type, WriterOptions writerOptions)
	{
		WriterType = type;
		WriterOptions = writerOptions;
	}

	protected void InitalizeStream(Stream stream)
	{
		OutputStream = stream;
	}

	public abstract void Write(string filename, Stream source, DateTime? modificationTime);

	protected virtual void Dispose(bool isDisposing)
	{
		if (isDisposing)
		{
			OutputStream.Dispose();
		}
	}

	public void Dispose()
	{
		if (!_isDisposed)
		{
			GC.SuppressFinalize(this);
			Dispose(isDisposing: true);
			_isDisposed = true;
		}
	}

	~AbstractWriter()
	{
		if (!_isDisposed)
		{
			Dispose(isDisposing: false);
			_isDisposed = true;
		}
	}
}
