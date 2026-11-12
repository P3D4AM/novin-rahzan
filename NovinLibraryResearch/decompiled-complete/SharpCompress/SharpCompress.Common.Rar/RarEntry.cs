using System;
using SharpCompress.Common.Rar.Headers;

namespace SharpCompress.Common.Rar;

public abstract class RarEntry : Entry
{
	internal abstract FileHeader FileHeader { get; }

	internal bool IsRarV3
	{
		get
		{
			if (FileHeader.CompressionAlgorithm != 29)
			{
				return FileHeader.CompressionAlgorithm == 36;
			}
			return true;
		}
	}

	public override long Crc => FileHeader.FileCrc;

	public override string Key => FileHeader.FileName;

	public override string? LinkTarget => null;

	public override DateTime? LastModifiedTime => FileHeader.FileLastModifiedTime;

	public override DateTime? CreatedTime => FileHeader.FileCreatedTime;

	public override DateTime? LastAccessedTime => FileHeader.FileLastAccessedTime;

	public override DateTime? ArchivedTime => FileHeader.FileArchivedTime;

	public override bool IsEncrypted => FileHeader.IsEncrypted;

	public override bool IsDirectory => FileHeader.IsDirectory;

	public override bool IsSplitAfter => FileHeader.IsSplitAfter;

	public override string ToString()
	{
		return $"Entry Path: {Key} Compressed Size: {CompressedSize} Uncompressed Size: {Size} CRC: {Crc}";
	}
}
