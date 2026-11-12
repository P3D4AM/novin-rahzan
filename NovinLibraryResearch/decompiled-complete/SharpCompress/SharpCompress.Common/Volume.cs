using System;
using System.IO;
using SharpCompress.IO;
using SharpCompress.Readers;

namespace SharpCompress.Common;

public abstract class Volume : IVolume, IDisposable
{
	private readonly Stream _actualStream;

	internal Stream Stream => _actualStream;

	protected ReaderOptions ReaderOptions { get; }

	public virtual bool IsFirstVolume => true;

	public virtual bool IsMultiVolume => true;

	internal Volume(Stream stream, ReaderOptions readerOptions)
	{
		ReaderOptions = readerOptions;
		if (readerOptions.LeaveStreamOpen)
		{
			stream = new NonDisposingStream(stream);
		}
		_actualStream = stream;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_actualStream.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
