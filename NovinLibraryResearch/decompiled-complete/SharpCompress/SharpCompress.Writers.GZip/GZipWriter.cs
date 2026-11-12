using System;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Compressors;
using SharpCompress.Compressors.Deflate;
using SharpCompress.IO;

namespace SharpCompress.Writers.GZip;

public sealed class GZipWriter : AbstractWriter
{
	private bool _wroteToStream;

	public GZipWriter(Stream destination, GZipWriterOptions? options = null)
		: base(ArchiveType.GZip, options ?? new GZipWriterOptions())
	{
		if (base.WriterOptions.LeaveStreamOpen)
		{
			destination = new NonDisposingStream(destination);
		}
		InitalizeStream(new GZipStream(destination, CompressionMode.Compress, options?.CompressionLevel ?? CompressionLevel.Default, base.WriterOptions.ArchiveEncoding.GetEncoding()));
	}

	protected override void Dispose(bool isDisposing)
	{
		if (isDisposing)
		{
			base.OutputStream.Dispose();
		}
		base.Dispose(isDisposing);
	}

	public override void Write(string filename, Stream source, DateTime? modificationTime)
	{
		if (_wroteToStream)
		{
			throw new ArgumentException("Can only write a single stream to a GZip file.");
		}
		GZipStream gZipStream = (GZipStream)base.OutputStream;
		gZipStream.FileName = filename;
		gZipStream.LastModified = modificationTime;
		source.TransferTo(gZipStream);
		_wroteToStream = true;
	}
}
