using System;
using System.IO;
using System.Linq;
using SharpCompress.IO;

namespace SharpCompress.Common.SevenZip;

internal class SevenZipFilePart : FilePart
{
	private CompressionType? _type;

	private readonly Stream _stream;

	private readonly ArchiveDatabase _database;

	private const uint K_COPY = 0u;

	private const uint K_DELTA = 3u;

	private const uint K_LZMA2 = 33u;

	private const uint K_LZMA = 196865u;

	private const uint K_PPMD = 197633u;

	private const uint K_BCJ = 50528515u;

	private const uint K_BCJ2 = 50528539u;

	private const uint K_DEFLATE = 262408u;

	private const uint K_B_ZIP2 = 262658u;

	internal CFileItem Header { get; }

	internal CFolder? Folder { get; }

	internal int Index { get; }

	internal override string FilePartName => Header.Name;

	public CompressionType CompressionType
	{
		get
		{
			CompressionType? type = _type;
			if (!type.HasValue)
			{
				_type = GetCompression();
			}
			return _type.Value;
		}
	}

	internal bool IsEncrypted => Folder._coders.FindIndex((CCoderInfo c) => c._methodId._id == 116459265) != -1;

	internal SevenZipFilePart(Stream stream, ArchiveDatabase database, int index, CFileItem fileEntry, ArchiveEncoding archiveEncoding)
		: base(archiveEncoding)
	{
		_stream = stream;
		_database = database;
		Index = index;
		Header = fileEntry;
		if (Header.HasStream)
		{
			Folder = database._folders[database._fileIndexToFolderIndexMap[index]];
		}
	}

	internal override Stream? GetRawStream()
	{
		return null;
	}

	internal override Stream GetCompressedStream()
	{
		if (!Header.HasStream)
		{
			return null;
		}
		Stream folderStream = _database.GetFolderStream(_stream, Folder, _database.PasswordProvider);
		int num = _database._folderStartFileIndex[_database._folders.IndexOf(Folder)];
		int num2 = Index - num;
		long num3 = 0L;
		for (int i = 0; i < num2; i++)
		{
			num3 += _database._files[num + i].Size;
		}
		if (num3 > 0)
		{
			folderStream.Skip(num3);
		}
		return new ReadOnlySubStream(folderStream, Header.Size);
	}

	internal CompressionType GetCompression()
	{
		switch (Folder._coders.First()._methodId._id)
		{
		case 33uL:
		case 196865uL:
			return CompressionType.LZMA;
		case 197633uL:
			return CompressionType.PPMd;
		case 262658uL:
			return CompressionType.BZip2;
		default:
			throw new NotImplementedException();
		}
	}
}
