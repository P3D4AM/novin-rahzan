using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCompress.Common.GZip;

public class GZipEntry : Entry
{
	private readonly GZipFilePart _filePart;

	public override CompressionType CompressionType => CompressionType.GZip;

	public override long Crc => _filePart.Crc.GetValueOrDefault();

	public override string Key => _filePart.FilePartName;

	public override string? LinkTarget => null;

	public override long CompressedSize => 0L;

	public override long Size => _filePart.UncompressedSize.GetValueOrDefault();

	public override DateTime? LastModifiedTime => _filePart.DateModified;

	public override DateTime? CreatedTime => null;

	public override DateTime? LastAccessedTime => null;

	public override DateTime? ArchivedTime => null;

	public override bool IsEncrypted => false;

	public override bool IsDirectory => false;

	public override bool IsSplitAfter => false;

	internal override IEnumerable<FilePart> Parts => ((FilePart)_filePart).AsEnumerable();

	internal GZipEntry(GZipFilePart filePart)
	{
		_filePart = filePart;
	}

	internal static IEnumerable<GZipEntry> GetEntries(Stream stream, OptionsBase options)
	{
		yield return new GZipEntry(new GZipFilePart(stream, options.ArchiveEncoding));
	}
}
