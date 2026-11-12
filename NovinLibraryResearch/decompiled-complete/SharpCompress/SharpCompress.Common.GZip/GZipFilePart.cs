using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Common.Tar.Headers;
using SharpCompress.Compressors;
using SharpCompress.Compressors.Deflate;

namespace SharpCompress.Common.GZip;

internal sealed class GZipFilePart : FilePart
{
	private string? _name;

	private readonly Stream _stream;

	internal long EntryStartPosition { get; }

	internal DateTime? DateModified { get; private set; }

	internal int? Crc { get; private set; }

	internal int? UncompressedSize { get; private set; }

	internal override string FilePartName => _name;

	internal GZipFilePart(Stream stream, ArchiveEncoding archiveEncoding)
		: base(archiveEncoding)
	{
		_stream = stream;
		ReadAndValidateGzipHeader();
		if (stream.CanSeek)
		{
			long position = stream.Position;
			stream.Position = stream.Length - 8;
			ReadTrailer();
			stream.Position = position;
		}
		EntryStartPosition = stream.Position;
	}

	internal override Stream GetCompressedStream()
	{
		return new DeflateStream(_stream, CompressionMode.Decompress);
	}

	internal override Stream GetRawStream()
	{
		return _stream;
	}

	private void ReadTrailer()
	{
		Span<byte> span = stackalloc byte[8];
		_stream.Read(span);
		Crc = BinaryPrimitives.ReadInt32LittleEndian(span);
		UncompressedSize = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(4));
	}

	private void ReadAndValidateGzipHeader()
	{
		Span<byte> buffer = stackalloc byte[10];
		switch (_stream.Read(buffer))
		{
		case 0:
			break;
		default:
			throw new ZlibException("Not a valid GZIP stream.");
		case 10:
		{
			if (buffer[0] != 31 || buffer[1] != 139 || buffer[2] != 8)
			{
				throw new ZlibException("Bad GZIP header.");
			}
			int num = BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(4));
			DateTime ePOCH = TarHeader.EPOCH;
			DateModified = ePOCH.AddSeconds(num);
			if ((buffer[3] & 4) == 4)
			{
				int num2 = _stream.Read(buffer.Slice(0, 2));
				short num3 = (short)(buffer[0] + buffer[1] * 256);
				if (!Utility.ReadFully(buffer: new byte[num3], stream: _stream))
				{
					throw new ZlibException("Unexpected end-of-file reading GZIP header.");
				}
				num2 = num3;
			}
			if ((buffer[3] & 8) == 8)
			{
				_name = ReadZeroTerminatedString(_stream);
			}
			if ((buffer[3] & 0x10) == 16)
			{
				ReadZeroTerminatedString(_stream);
			}
			if ((buffer[3] & 2) == 2)
			{
				_stream.ReadByte();
			}
			break;
		}
		}
	}

	private string ReadZeroTerminatedString(Stream stream)
	{
		Span<byte> buffer = stackalloc byte[1];
		List<byte> list = new List<byte>();
		bool flag = false;
		do
		{
			if (stream.Read(buffer) != 1)
			{
				throw new ZlibException("Unexpected EOF reading GZIP header.");
			}
			if (buffer[0] == 0)
			{
				flag = true;
			}
			else
			{
				list.Add(buffer[0]);
			}
		}
		while (!flag);
		byte[] bytes = list.ToArray();
		return base.ArchiveEncoding.Decode(bytes);
	}
}
