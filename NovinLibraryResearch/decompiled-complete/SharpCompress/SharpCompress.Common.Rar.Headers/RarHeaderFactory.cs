using System.Collections.Generic;
using System.IO;
using SharpCompress.IO;
using SharpCompress.Readers;

namespace SharpCompress.Common.Rar.Headers;

internal class RarHeaderFactory
{
	private bool _isRar5;

	private ReaderOptions Options { get; }

	internal StreamingMode StreamingMode { get; }

	internal bool IsEncrypted { get; private set; }

	internal RarHeaderFactory(StreamingMode mode, ReaderOptions options)
	{
		StreamingMode = mode;
		Options = options;
	}

	internal IEnumerable<IRarHeader> ReadHeaders(Stream stream)
	{
		MarkHeader markHeader = MarkHeader.Read(stream, Options.LeaveStreamOpen, Options.LookForHeader);
		_isRar5 = markHeader.IsRar5;
		yield return markHeader;
		RarHeader header;
		do
		{
			RarHeader rarHeader;
			header = (rarHeader = TryReadNextHeader(stream));
			if (rarHeader != null)
			{
				yield return header;
				continue;
			}
			break;
		}
		while (header.HeaderType != HeaderType.EndArchive);
	}

	private RarHeader? TryReadNextHeader(Stream stream)
	{
		RarCrcBinaryReader rarCrcBinaryReader;
		if (!IsEncrypted)
		{
			rarCrcBinaryReader = new RarCrcBinaryReader(stream);
		}
		else
		{
			if (Options.Password == null)
			{
				throw new CryptographicException("Encrypted Rar archive has no password specified.");
			}
			rarCrcBinaryReader = new RarCryptoBinaryReader(stream, Options.Password);
		}
		RarHeader rarHeader = RarHeader.TryReadBase(rarCrcBinaryReader, _isRar5, Options.ArchiveEncoding);
		if (rarHeader == null)
		{
			return null;
		}
		switch (rarHeader.HeaderCode)
		{
		case 1:
		case 115:
		{
			ArchiveHeader archiveHeader = new ArchiveHeader(rarHeader, rarCrcBinaryReader);
			if (archiveHeader.IsEncrypted == true)
			{
				IsEncrypted = true;
			}
			return archiveHeader;
		}
		case 120:
		{
			ProtectHeader protectHeader = new ProtectHeader(rarHeader, rarCrcBinaryReader);
			switch (StreamingMode)
			{
			case StreamingMode.Seekable:
				rarCrcBinaryReader.BaseStream.Position += protectHeader.DataSize;
				break;
			case StreamingMode.Streaming:
				rarCrcBinaryReader.BaseStream.Skip(protectHeader.DataSize);
				break;
			default:
				throw new InvalidFormatException("Invalid StreamingMode");
			}
			return protectHeader;
		}
		case 3:
		{
			FileHeader fileHeader3 = new FileHeader(rarHeader, rarCrcBinaryReader, HeaderType.Service);
			SkipData(fileHeader3, rarCrcBinaryReader);
			return fileHeader3;
		}
		case 122:
		{
			FileHeader fileHeader2 = new FileHeader(rarHeader, rarCrcBinaryReader, HeaderType.NewSub);
			SkipData(fileHeader2, rarCrcBinaryReader);
			return fileHeader2;
		}
		case 2:
		case 116:
		{
			FileHeader fileHeader = new FileHeader(rarHeader, rarCrcBinaryReader, HeaderType.File);
			switch (StreamingMode)
			{
			case StreamingMode.Seekable:
				fileHeader.DataStartPosition = rarCrcBinaryReader.BaseStream.Position;
				rarCrcBinaryReader.BaseStream.Position += fileHeader.CompressedSize;
				break;
			case StreamingMode.Streaming:
			{
				ReadOnlySubStream readOnlySubStream = new ReadOnlySubStream(rarCrcBinaryReader.BaseStream, fileHeader.CompressedSize);
				if (fileHeader.R4Salt == null)
				{
					fileHeader.PackedStream = readOnlySubStream;
				}
				else
				{
					fileHeader.PackedStream = new RarCryptoWrapper(readOnlySubStream, Options.Password, fileHeader.R4Salt);
				}
				break;
			}
			default:
				throw new InvalidFormatException("Invalid StreamingMode");
			}
			return fileHeader;
		}
		case 5:
		case 123:
			return new EndArchiveHeader(rarHeader, rarCrcBinaryReader);
		case 4:
		{
			ArchiveCryptHeader result = new ArchiveCryptHeader(rarHeader, rarCrcBinaryReader);
			IsEncrypted = true;
			return result;
		}
		default:
			throw new InvalidFormatException("Unknown Rar Header: " + rarHeader.HeaderCode);
		}
	}

	private void SkipData(FileHeader fh, RarCrcBinaryReader reader)
	{
		switch (StreamingMode)
		{
		case StreamingMode.Seekable:
			fh.DataStartPosition = reader.BaseStream.Position;
			reader.BaseStream.Position += fh.CompressedSize;
			break;
		case StreamingMode.Streaming:
			reader.BaseStream.Skip(fh.CompressedSize);
			break;
		default:
			throw new InvalidFormatException("Invalid StreamingMode");
		}
	}
}
