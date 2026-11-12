using System.Collections.Generic;
using System.IO;
using SharpCompress.Common.Zip.Headers;
using SharpCompress.IO;

namespace SharpCompress.Common.Zip;

internal class StreamingZipHeaderFactory : ZipHeaderFactory
{
	internal StreamingZipHeaderFactory(string? password, ArchiveEncoding archiveEncoding)
		: base(StreamingMode.Streaming, password, archiveEncoding)
	{
	}

	internal IEnumerable<ZipHeader> ReadStreamHeader(Stream stream)
	{
		RewindableStream rewindableStream = ((!(stream is RewindableStream rewindableStream2)) ? new RewindableStream(stream) : rewindableStream2);
		while (true)
		{
			BinaryReader binaryReader = new BinaryReader(rewindableStream);
			if (_lastEntryHeader != null && (FlagUtility.HasFlag(_lastEntryHeader.Flags, HeaderFlags.UsePostDataDescriptor) || _lastEntryHeader.IsZip64))
			{
				binaryReader = ((StreamingZipFilePart)_lastEntryHeader.Part).FixStreamedFileLocation(ref rewindableStream);
				long? num = (rewindableStream.CanSeek ? new long?(rewindableStream.Position) : null);
				uint num2 = binaryReader.ReadUInt32();
				if (num2 == 134695760)
				{
					num2 = binaryReader.ReadUInt32();
				}
				_lastEntryHeader.Crc = num2;
				_lastEntryHeader.CompressedSize = binaryReader.ReadUInt32();
				_lastEntryHeader.UncompressedSize = binaryReader.ReadUInt32();
				if (num.HasValue)
				{
					_lastEntryHeader.DataStartPosition = num - _lastEntryHeader.CompressedSize;
				}
			}
			_lastEntryHeader = null;
			uint headerBytes = binaryReader.ReadUInt32();
			ZipHeader zipHeader = ReadHeader(headerBytes, binaryReader);
			if (zipHeader == null)
			{
				break;
			}
			if (zipHeader.ZipHeaderType == ZipHeaderType.LocalEntry)
			{
				LocalEntryHeader localEntryHeader = (LocalEntryHeader)zipHeader;
				if (localEntryHeader.CompressedSize > 0)
				{
					zipHeader.HasData = true;
				}
				else if (localEntryHeader.Flags.HasFlag(HeaderFlags.UsePostDataDescriptor))
				{
					bool isRecording = rewindableStream.IsRecording;
					if (!isRecording)
					{
						rewindableStream.StartRecording();
					}
					uint headerBytes2 = binaryReader.ReadUInt32();
					zipHeader.HasData = !ZipHeaderFactory.IsHeader(headerBytes2);
					rewindableStream.Rewind(!isRecording);
				}
				else
				{
					zipHeader.HasData = false;
				}
			}
			yield return zipHeader;
		}
	}
}
