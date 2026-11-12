using System;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Common.Tar.Headers;
using SharpCompress.Compressors;
using SharpCompress.Compressors.BZip2;
using SharpCompress.Compressors.Deflate;
using SharpCompress.Compressors.LZMA;
using SharpCompress.IO;

namespace SharpCompress.Writers.Tar;

public class TarWriter : AbstractWriter
{
	private readonly bool finalizeArchiveOnClose;

	public TarWriter(Stream destination, TarWriterOptions options)
		: base(ArchiveType.Tar, options)
	{
		finalizeArchiveOnClose = options.FinalizeArchiveOnClose;
		if (!destination.CanWrite)
		{
			throw new ArgumentException("Tars require writable streams.");
		}
		if (base.WriterOptions.LeaveStreamOpen)
		{
			destination = new NonDisposingStream(destination);
		}
		switch (options.CompressionType)
		{
		case CompressionType.BZip2:
			destination = new BZip2Stream(destination, CompressionMode.Compress, decompressConcatenated: false);
			break;
		case CompressionType.GZip:
			destination = new GZipStream(destination, CompressionMode.Compress);
			break;
		case CompressionType.LZip:
			destination = new LZipStream(destination, CompressionMode.Compress);
			break;
		default:
			throw new InvalidFormatException("Tar does not support compression: " + options.CompressionType);
		case CompressionType.None:
			break;
		}
		InitalizeStream(destination);
	}

	public override void Write(string filename, Stream source, DateTime? modificationTime)
	{
		Write(filename, source, modificationTime, null);
	}

	private string NormalizeFilename(string filename)
	{
		filename = filename.Replace('\\', '/');
		int num = filename.IndexOf(':');
		if (num >= 0)
		{
			filename = filename.Remove(0, num + 1);
		}
		return filename.Trim(new char[1] { '/' });
	}

	public void Write(string filename, Stream source, DateTime? modificationTime, long? size)
	{
		if (!source.CanSeek && !size.HasValue)
		{
			throw new ArgumentException("Seekable stream is required if no size is given.");
		}
		long size2 = size ?? source.Length;
		TarHeader tarHeader = new TarHeader(base.WriterOptions.ArchiveEncoding);
		tarHeader.LastModifiedTime = modificationTime ?? TarHeader.EPOCH;
		tarHeader.Name = NormalizeFilename(filename);
		tarHeader.Size = size2;
		tarHeader.Write(base.OutputStream);
		size = source.TransferTo(base.OutputStream);
		PadTo512(size.Value);
	}

	private void PadTo512(long size)
	{
		int num = (int)(((size + 511) & -512) - size);
		Stream outputStream = base.OutputStream;
		Span<byte> span = stackalloc byte[num];
		outputStream.Write(span);
	}

	protected override void Dispose(bool isDisposing)
	{
		if (isDisposing)
		{
			Stream outputStream;
			if (finalizeArchiveOnClose)
			{
				outputStream = base.OutputStream;
				Span<byte> span = stackalloc byte[1024];
				outputStream.Write(span);
			}
			outputStream = base.OutputStream;
			if (!(outputStream is BZip2Stream bZip2Stream))
			{
				if (outputStream is LZipStream lZipStream)
				{
					lZipStream.Finish();
				}
			}
			else
			{
				bZip2Stream.Finish();
			}
		}
		base.Dispose(isDisposing);
	}
}
