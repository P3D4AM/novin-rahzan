using System;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Common.Zip.Headers;
using SharpCompress.IO;

namespace SharpCompress.Common.Zip;

internal sealed class SeekableZipHeaderFactory : ZipHeaderFactory
{
	private const int MINIMUM_EOCD_LENGTH = 22;

	private const int ZIP64_EOCD_LENGTH = 20;

	private const int MAX_SEARCH_LENGTH_FOR_EOCD = 65557;

	private bool _zip64;

	internal SeekableZipHeaderFactory(string? password, ArchiveEncoding archiveEncoding)
		: base(StreamingMode.Seekable, password, archiveEncoding)
	{
	}

	internal IEnumerable<ZipHeader> ReadSeekableHeader(Stream stream)
	{
		BinaryReader reader = new BinaryReader(stream);
		SeekBackToHeader(stream, reader);
		long position2 = stream.Position;
		DirectoryEndHeader directoryEndHeader = new DirectoryEndHeader();
		directoryEndHeader.Read(reader);
		if (directoryEndHeader.IsZip64)
		{
			_zip64 = true;
			stream.Seek(position2 - 20 - 4, SeekOrigin.Begin);
			if (reader.ReadUInt32() != 117853008)
			{
				throw new ArchiveException("Failed to locate the Zip64 Directory Locator");
			}
			Zip64DirectoryEndLocatorHeader zip64DirectoryEndLocatorHeader = new Zip64DirectoryEndLocatorHeader();
			zip64DirectoryEndLocatorHeader.Read(reader);
			stream.Seek(zip64DirectoryEndLocatorHeader.RelativeOffsetOfTheEndOfDirectoryRecord, SeekOrigin.Begin);
			if (reader.ReadUInt32() != 101075792)
			{
				throw new ArchiveException("Failed to locate the Zip64 Header");
			}
			Zip64DirectoryEndHeader zip64DirectoryEndHeader = new Zip64DirectoryEndHeader();
			zip64DirectoryEndHeader.Read(reader);
			stream.Seek(zip64DirectoryEndHeader.DirectoryStartOffsetRelativeToDisk, SeekOrigin.Begin);
		}
		else
		{
			stream.Seek(directoryEndHeader.DirectoryStartOffsetRelativeToDisk, SeekOrigin.Begin);
		}
		long position = stream.Position;
		while (true)
		{
			stream.Position = position;
			uint headerBytes = reader.ReadUInt32();
			ZipHeader zipHeader = ReadHeader(headerBytes, reader, _zip64);
			position = stream.Position;
			if (zipHeader == null)
			{
				break;
			}
			if (zipHeader is DirectoryEntryHeader directoryEntryHeader)
			{
				directoryEntryHeader.HasData = directoryEntryHeader.CompressedSize != 0;
				yield return directoryEntryHeader;
			}
			else if (zipHeader is DirectoryEndHeader directoryEndHeader2)
			{
				yield return directoryEndHeader2;
			}
		}
	}

	private static bool IsMatch(byte[] haystack, int position, byte[] needle)
	{
		for (int i = 0; i < needle.Length; i++)
		{
			if (haystack[position + i] != needle[i])
			{
				return false;
			}
		}
		return true;
	}

	private static void SeekBackToHeader(Stream stream, BinaryReader reader)
	{
		if (stream.Length < 22)
		{
			throw new ArchiveException("Could not find Zip file Directory at the end of the file. File may be corrupted.");
		}
		int num = (int)((stream.Length < 65557) ? stream.Length : 65557);
		byte[] needle = new byte[4] { 6, 5, 75, 80 };
		stream.Seek(-num, SeekOrigin.End);
		byte[] array = reader.ReadBytes(num);
		Array.Reverse((Array)array);
		int num2 = num;
		for (int i = 0; i < num2; i++)
		{
			if (IsMatch(array, i, needle))
			{
				stream.Seek(-i, SeekOrigin.End);
				return;
			}
		}
		throw new ArchiveException("Failed to locate the Zip Header");
	}

	internal LocalEntryHeader GetLocalHeader(Stream stream, DirectoryEntryHeader directoryEntryHeader)
	{
		stream.Seek(directoryEntryHeader.RelativeOffsetOfEntryHeader, SeekOrigin.Begin);
		BinaryReader binaryReader = new BinaryReader(stream);
		uint headerBytes = binaryReader.ReadUInt32();
		return (ReadHeader(headerBytes, binaryReader, _zip64) as LocalEntryHeader) ?? throw new InvalidOperationException();
	}
}
