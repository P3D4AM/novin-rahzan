using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Common.Rar;
using SharpCompress.Common.Rar.Headers;
using SharpCompress.Compressors.Rar;
using SharpCompress.Readers;

namespace SharpCompress.Archives.Rar;

public class RarArchiveEntry : RarEntry, IArchiveEntry, IEntry
{
	private readonly ICollection<RarFilePart> parts;

	private readonly RarArchive archive;

	private readonly ReaderOptions readerOptions;

	public override CompressionType CompressionType => CompressionType.Rar;

	public IArchive Archive => archive;

	internal override IEnumerable<FilePart> Parts => parts.Cast<FilePart>();

	internal override FileHeader FileHeader => parts.First().FileHeader;

	public override long Crc
	{
		get
		{
			CheckIncomplete();
			return parts.Select((RarFilePart fp) => fp.FileHeader).Single((FileHeader fh) => !fh.IsSplitAfter).FileCrc;
		}
	}

	public override long Size
	{
		get
		{
			CheckIncomplete();
			return parts.First().FileHeader.UncompressedSize;
		}
	}

	public override long CompressedSize
	{
		get
		{
			CheckIncomplete();
			return parts.Aggregate(0L, (long total, RarFilePart fp) => total + fp.FileHeader.CompressedSize);
		}
	}

	public bool IsComplete
	{
		get
		{
			IEnumerable<FileHeader> source = parts.Select((RarFilePart x) => x.FileHeader);
			if (!source.First().IsSplitBefore)
			{
				return !source.Last().IsSplitAfter;
			}
			return false;
		}
	}

	internal RarArchiveEntry(RarArchive archive, IEnumerable<RarFilePart> parts, ReaderOptions readerOptions)
	{
		this.parts = parts.ToList();
		this.archive = archive;
		this.readerOptions = readerOptions;
	}

	public Stream OpenEntryStream()
	{
		if (base.IsRarV3)
		{
			return new RarStream(archive.UnpackV1.Value, FileHeader, new MultiVolumeReadOnlyStream(Parts.Cast<RarFilePart>(), archive));
		}
		return new RarStream(archive.UnpackV2017.Value, FileHeader, new MultiVolumeReadOnlyStream(Parts.Cast<RarFilePart>(), archive));
	}

	private void CheckIncomplete()
	{
		if (!readerOptions.DisableCheckIncomplete && !IsComplete)
		{
			throw new IncompleteArchiveException("ArchiveEntry is incomplete and cannot perform this operation.");
		}
	}
}
