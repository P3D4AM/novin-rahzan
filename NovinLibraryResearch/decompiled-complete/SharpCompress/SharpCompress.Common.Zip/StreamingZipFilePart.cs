using System.IO;
using SharpCompress.Common.Zip.Headers;
using SharpCompress.Compressors.Deflate;
using SharpCompress.IO;

namespace SharpCompress.Common.Zip;

internal sealed class StreamingZipFilePart : ZipFilePart
{
	private Stream? _decompressionStream;

	internal StreamingZipFilePart(ZipFileEntry header, Stream stream)
		: base(header, stream)
	{
	}

	protected override Stream CreateBaseStream()
	{
		return base.Header.PackedStream;
	}

	internal override Stream GetCompressedStream()
	{
		if (!base.Header.HasData)
		{
			return Stream.Null;
		}
		_decompressionStream = CreateDecompressionStream(GetCryptoStream(CreateBaseStream()), base.Header.CompressionMethod);
		if (base.LeaveStreamOpen)
		{
			return new NonDisposingStream(_decompressionStream);
		}
		return _decompressionStream;
	}

	internal BinaryReader FixStreamedFileLocation(ref RewindableStream rewindableStream)
	{
		if (base.Header.IsDirectory)
		{
			return new BinaryReader(rewindableStream);
		}
		if (base.Header.HasData && !base.Skipped)
		{
			if (_decompressionStream == null)
			{
				_decompressionStream = GetCompressedStream();
			}
			_decompressionStream.Skip();
			if (_decompressionStream is DeflateStream deflateStream)
			{
				rewindableStream.Rewind(deflateStream.InputBuffer);
			}
			base.Skipped = true;
		}
		BinaryReader result = new BinaryReader(rewindableStream);
		_decompressionStream = null;
		return result;
	}
}
