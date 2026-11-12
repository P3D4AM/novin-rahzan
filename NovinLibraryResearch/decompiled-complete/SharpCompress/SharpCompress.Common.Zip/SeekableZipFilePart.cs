using System.IO;
using SharpCompress.Common.Zip.Headers;
using SharpCompress.IO;

namespace SharpCompress.Common.Zip;

internal class SeekableZipFilePart : ZipFilePart
{
	private bool _isLocalHeaderLoaded;

	private readonly SeekableZipHeaderFactory _headerFactory;

	private readonly DirectoryEntryHeader _directoryEntryHeader;

	internal string? Comment => ((DirectoryEntryHeader)base.Header).Comment;

	internal SeekableZipFilePart(SeekableZipHeaderFactory headerFactory, DirectoryEntryHeader header, Stream stream)
		: base(header, stream)
	{
		_headerFactory = headerFactory;
		_directoryEntryHeader = header;
	}

	internal override Stream GetCompressedStream()
	{
		if (!_isLocalHeaderLoaded)
		{
			LoadLocalHeader();
			_isLocalHeaderLoaded = true;
		}
		return base.GetCompressedStream();
	}

	private void LoadLocalHeader()
	{
		bool hasData = base.Header.HasData;
		base.Header = _headerFactory.GetLocalHeader(base.BaseStream, (DirectoryEntryHeader)base.Header);
		base.Header.HasData = hasData;
	}

	protected override Stream CreateBaseStream()
	{
		base.BaseStream.Position = base.Header.DataStartPosition.Value;
		if (base.Header.CompressedSize == 0L && FlagUtility.HasFlag(base.Header.Flags, HeaderFlags.UsePostDataDescriptor))
		{
			DirectoryEntryHeader directoryEntryHeader = _directoryEntryHeader;
			if (directoryEntryHeader != null && directoryEntryHeader.HasData)
			{
				DirectoryEntryHeader directoryEntryHeader2 = _directoryEntryHeader;
				if (directoryEntryHeader2 == null || directoryEntryHeader2.CompressedSize != 0)
				{
					return new ReadOnlySubStream(base.BaseStream, _directoryEntryHeader.CompressedSize);
				}
			}
		}
		return base.BaseStream;
	}
}
