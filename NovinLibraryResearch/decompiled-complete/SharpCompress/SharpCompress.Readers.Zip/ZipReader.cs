using System.Collections.Generic;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Common.Zip;
using SharpCompress.Common.Zip.Headers;

namespace SharpCompress.Readers.Zip;

public class ZipReader : AbstractReader<ZipEntry, ZipVolume>
{
	private readonly StreamingZipHeaderFactory _headerFactory;

	public override ZipVolume Volume { get; }

	private ZipReader(Stream stream, ReaderOptions options)
		: base(options, ArchiveType.Zip)
	{
		Volume = new ZipVolume(stream, options);
		_headerFactory = new StreamingZipHeaderFactory(options.Password, options.ArchiveEncoding);
	}

	public static ZipReader Open(Stream stream, ReaderOptions? options = null)
	{
		stream.CheckNotNull("stream");
		return new ZipReader(stream, options ?? new ReaderOptions());
	}

	protected override IEnumerable<ZipEntry> GetEntries(Stream stream)
	{
		foreach (ZipHeader item in _headerFactory.ReadStreamHeader(stream))
		{
			if (item != null)
			{
				switch (item.ZipHeaderType)
				{
				case ZipHeaderType.LocalEntry:
					yield return new ZipEntry(new StreamingZipFilePart((LocalEntryHeader)item, stream));
					break;
				case ZipHeaderType.DirectoryEnd:
					yield break;
				}
			}
		}
	}
}
