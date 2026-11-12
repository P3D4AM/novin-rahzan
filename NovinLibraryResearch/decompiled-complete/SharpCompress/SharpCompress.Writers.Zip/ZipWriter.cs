using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using SharpCompress.Common;
using SharpCompress.Common.Zip;
using SharpCompress.Common.Zip.Headers;
using SharpCompress.Compressors;
using SharpCompress.Compressors.BZip2;
using SharpCompress.Compressors.Deflate;
using SharpCompress.Compressors.LZMA;
using SharpCompress.Compressors.PPMd;
using SharpCompress.IO;

namespace SharpCompress.Writers.Zip;

public class ZipWriter : AbstractWriter
{
	internal class ZipWritingStream : Stream
	{
		private readonly CRC32 crc = new CRC32();

		private readonly ZipCentralDirectoryEntry entry;

		private readonly Stream originalStream;

		private readonly Stream writeStream;

		private readonly ZipWriter writer;

		private readonly ZipCompressionMethod zipCompressionMethod;

		private readonly CompressionLevel compressionLevel;

		private CountingWritableSubStream? counting;

		private ulong decompressed;

		private bool limitsExceeded;

		private bool isDisposed;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		internal ZipWritingStream(ZipWriter writer, Stream originalStream, ZipCentralDirectoryEntry entry, ZipCompressionMethod zipCompressionMethod, CompressionLevel compressionLevel)
		{
			this.writer = writer;
			this.originalStream = originalStream;
			this.writer = writer;
			this.entry = entry;
			this.zipCompressionMethod = zipCompressionMethod;
			this.compressionLevel = compressionLevel;
			writeStream = GetWriteStream(originalStream);
		}

		private Stream GetWriteStream(Stream writeStream)
		{
			counting = new CountingWritableSubStream(writeStream);
			Stream result = counting;
			switch (zipCompressionMethod)
			{
			case ZipCompressionMethod.None:
				return result;
			case ZipCompressionMethod.Deflate:
				return new DeflateStream(counting, CompressionMode.Compress, compressionLevel);
			case ZipCompressionMethod.BZip2:
				return new BZip2Stream(counting, CompressionMode.Compress, decompressConcatenated: false);
			case ZipCompressionMethod.LZMA:
			{
				counting.WriteByte(9);
				counting.WriteByte(20);
				counting.WriteByte(5);
				counting.WriteByte(0);
				LzmaStream lzmaStream = new LzmaStream(new LzmaEncoderProperties(!originalStream.CanSeek), isLzma2: false, counting);
				counting.Write(lzmaStream.Properties, 0, lzmaStream.Properties.Length);
				return lzmaStream;
			}
			case ZipCompressionMethod.PPMd:
				counting.Write(writer.PpmdProperties.Properties, 0, 2);
				return new PpmdStream(writer.PpmdProperties, counting, compress: true);
			default:
				throw new NotSupportedException("CompressionMethod: " + zipCompressionMethod);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (isDisposed)
			{
				return;
			}
			isDisposed = true;
			base.Dispose(disposing);
			if (!disposing)
			{
				return;
			}
			writeStream.Dispose();
			if (limitsExceeded)
			{
				originalStream.Dispose();
				return;
			}
			entry.Crc = (uint)crc.Crc32Result;
			entry.Compressed = counting.Count;
			entry.Decompressed = decompressed;
			bool flag = entry.Compressed >= uint.MaxValue || entry.Decompressed >= uint.MaxValue;
			uint compressed = (uint)(flag ? uint.MaxValue : counting.Count);
			uint uncompressed = (uint)(flag ? uint.MaxValue : entry.Decompressed);
			if (originalStream.CanSeek)
			{
				originalStream.Position = (long)(entry.HeaderOffset + 6);
				originalStream.WriteByte(0);
				if (counting.Count == 0L && entry.Decompressed == 0L)
				{
					originalStream.Position = (long)(entry.HeaderOffset + 8);
					originalStream.WriteByte(0);
					originalStream.WriteByte(0);
				}
				originalStream.Position = (long)(entry.HeaderOffset + 14);
				writer.WriteFooter(entry.Crc, compressed, uncompressed);
				if (flag && entry.Zip64HeaderOffset == 0)
				{
					throw new NotSupportedException("Attempted to write a stream that is larger than 4GiB without setting the zip64 option");
				}
				if (entry.Zip64HeaderOffset != 0)
				{
					originalStream.Position = (long)(entry.HeaderOffset + entry.Zip64HeaderOffset);
					Span<byte> span = stackalloc byte[8];
					BinaryPrimitives.WriteUInt16LittleEndian(span, 1);
					originalStream.Write(span.Slice(0, 2));
					BinaryPrimitives.WriteUInt16LittleEndian(span, 16);
					originalStream.Write(span.Slice(0, 2));
					BinaryPrimitives.WriteUInt64LittleEndian(span, entry.Decompressed);
					originalStream.Write(span);
					BinaryPrimitives.WriteUInt64LittleEndian(span, entry.Compressed);
					originalStream.Write(span);
				}
				originalStream.Position = writer.streamPosition + (long)entry.Compressed;
				writer.streamPosition += (long)entry.Compressed;
			}
			else
			{
				if (flag)
				{
					throw new NotSupportedException("Streams larger than 4GiB are not supported for non-seekable streams");
				}
				Span<byte> span2 = stackalloc byte[4];
				BinaryPrimitives.WriteUInt32LittleEndian(span2, 134695760u);
				originalStream.Write(span2);
				writer.WriteFooter(entry.Crc, compressed, uncompressed);
				writer.streamPosition += (long)(entry.Compressed + 16);
			}
			writer.entries.Add(entry);
		}

		public override void Flush()
		{
			writeStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (entry.Zip64HeaderOffset == 0 && (limitsExceeded || decompressed + (uint)count > uint.MaxValue || counting.Count + (uint)count > uint.MaxValue))
			{
				throw new NotSupportedException("Attempted to write a stream that is larger than 4GiB without setting the zip64 option");
			}
			decompressed += (uint)count;
			crc.SlurpBlock(buffer, offset, count);
			writeStream.Write(buffer, offset, count);
			if (entry.Zip64HeaderOffset == 0 && (decompressed > uint.MaxValue || counting.Count > uint.MaxValue))
			{
				limitsExceeded = true;
				throw new NotSupportedException("Attempted to write a stream that is larger than 4GiB without setting the zip64 option");
			}
		}
	}

	private readonly CompressionType compressionType;

	private readonly CompressionLevel compressionLevel;

	private readonly List<ZipCentralDirectoryEntry> entries = new List<ZipCentralDirectoryEntry>();

	private readonly string zipComment;

	private long streamPosition;

	private PpmdProperties? ppmdProps;

	private readonly bool isZip64;

	private PpmdProperties PpmdProperties => ppmdProps ?? (ppmdProps = new PpmdProperties());

	public ZipWriter(Stream destination, ZipWriterOptions zipWriterOptions)
		: base(ArchiveType.Zip, zipWriterOptions)
	{
		zipComment = zipWriterOptions.ArchiveComment ?? string.Empty;
		isZip64 = zipWriterOptions.UseZip64;
		if (destination.CanSeek)
		{
			streamPosition = destination.Position;
		}
		compressionType = zipWriterOptions.CompressionType;
		compressionLevel = zipWriterOptions.DeflateCompressionLevel;
		if (base.WriterOptions.LeaveStreamOpen)
		{
			destination = new NonDisposingStream(destination);
		}
		InitalizeStream(destination);
	}

	protected override void Dispose(bool isDisposing)
	{
		if (isDisposing)
		{
			ulong num = 0uL;
			foreach (ZipCentralDirectoryEntry entry in entries)
			{
				num += entry.Write(base.OutputStream);
			}
			WriteEndRecord(num);
		}
		base.Dispose(isDisposing);
	}

	private static ZipCompressionMethod ToZipCompressionMethod(CompressionType compressionType)
	{
		return compressionType switch
		{
			CompressionType.None => ZipCompressionMethod.None, 
			CompressionType.Deflate => ZipCompressionMethod.Deflate, 
			CompressionType.BZip2 => ZipCompressionMethod.BZip2, 
			CompressionType.LZMA => ZipCompressionMethod.LZMA, 
			CompressionType.PPMd => ZipCompressionMethod.PPMd, 
			_ => throw new InvalidFormatException("Invalid compression method: " + compressionType), 
		};
	}

	public override void Write(string entryPath, Stream source, DateTime? modificationTime)
	{
		Write(entryPath, source, new ZipWriterEntryOptions
		{
			ModificationDateTime = modificationTime
		});
	}

	public void Write(string entryPath, Stream source, ZipWriterEntryOptions zipWriterEntryOptions)
	{
		using Stream destination = WriteToStream(entryPath, zipWriterEntryOptions);
		source.TransferTo(destination);
	}

	public Stream WriteToStream(string entryPath, ZipWriterEntryOptions options)
	{
		ZipCompressionMethod zipCompressionMethod = ToZipCompressionMethod(options.CompressionType ?? compressionType);
		entryPath = NormalizeFilename(entryPath);
		ZipWriterEntryOptions zipWriterEntryOptions = options;
		DateTime? modificationDateTime = zipWriterEntryOptions.ModificationDateTime;
		DateTime valueOrDefault = modificationDateTime.GetValueOrDefault();
		if (!modificationDateTime.HasValue)
		{
			valueOrDefault = DateTime.Now;
			ZipWriterEntryOptions zipWriterEntryOptions2 = zipWriterEntryOptions;
			DateTime? modificationDateTime2 = valueOrDefault;
			zipWriterEntryOptions2.ModificationDateTime = modificationDateTime2;
		}
		zipWriterEntryOptions = options;
		if (zipWriterEntryOptions.EntryComment == null)
		{
			string text = (zipWriterEntryOptions.EntryComment = string.Empty);
		}
		ZipCentralDirectoryEntry entry = new ZipCentralDirectoryEntry(zipCompressionMethod, entryPath, (ulong)streamPosition, base.WriterOptions.ArchiveEncoding)
		{
			Comment = options.EntryComment,
			ModificationTime = options.ModificationDateTime
		};
		bool value = isZip64;
		if (options.EnableZip64.HasValue)
		{
			value = options.EnableZip64.Value;
		}
		uint num = (uint)WriteHeader(entryPath, options, entry, value);
		streamPosition += num;
		return new ZipWritingStream(this, base.OutputStream, entry, zipCompressionMethod, options.DeflateCompressionLevel ?? compressionLevel);
	}

	private string NormalizeFilename(string filename)
	{
		filename = filename.Replace('\\', '/');
		int num = filename.IndexOf(':');
		if (num >= 0)
		{
			filename = filename.Remove(0, num + 1);
		}
		return filename.Trim(new char[1] { '/' });
	}

	private unsafe int WriteHeader(string filename, ZipWriterEntryOptions zipWriterEntryOptions, ZipCentralDirectoryEntry entry, bool useZip64)
	{
		if (!base.OutputStream.CanSeek && useZip64)
		{
			throw new NotSupportedException("Zip64 extensions are not supported on non-seekable streams");
		}
		ZipCompressionMethod zipCompressionMethod = ToZipCompressionMethod(zipWriterEntryOptions.CompressionType ?? compressionType);
		byte[] array = base.WriterOptions.ArchiveEncoding.Encode(filename);
		Span<byte> span = stackalloc byte[4];
		BinaryPrimitives.WriteUInt32LittleEndian(span, 67324752u);
		base.OutputStream.Write(span);
		Stream outputStream;
		Span<byte> span2;
		if (zipCompressionMethod == ZipCompressionMethod.Deflate)
		{
			if (base.OutputStream.CanSeek && useZip64)
			{
				outputStream = base.OutputStream;
				span2 = stackalloc byte[2] { 45, 0 };
				outputStream.Write(span2);
			}
			else
			{
				outputStream = base.OutputStream;
				span2 = stackalloc byte[2] { 20, 0 };
				outputStream.Write(span2);
			}
		}
		else
		{
			outputStream = base.OutputStream;
			span2 = stackalloc byte[2] { 63, 0 };
			outputStream.Write(span2);
		}
		HeaderFlags headerFlags = (object.Equals(base.WriterOptions.ArchiveEncoding.GetEncoding(), Encoding.UTF8) ? HeaderFlags.Efs : HeaderFlags.None);
		if (!base.OutputStream.CanSeek)
		{
			headerFlags |= HeaderFlags.UsePostDataDescriptor;
			if (zipCompressionMethod == ZipCompressionMethod.LZMA)
			{
				headerFlags |= HeaderFlags.Bit1;
			}
		}
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)headerFlags);
		base.OutputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)zipCompressionMethod);
		base.OutputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt32LittleEndian(span, zipWriterEntryOptions.ModificationDateTime.DateTimeToDosTime());
		base.OutputStream.Write(span);
		outputStream = base.OutputStream;
		byte* intPtr = stackalloc byte[12];
		// IL initblk instruction
		Unsafe.InitBlock(intPtr, 0, 12);
		span2 = new Span<byte>(intPtr, 12);
		outputStream.Write(span2);
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)array.Length);
		base.OutputStream.Write(span.Slice(0, 2));
		int num = 0;
		if (base.OutputStream.CanSeek && useZip64)
		{
			num = 20;
		}
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)num);
		base.OutputStream.Write(span.Slice(0, 2));
		base.OutputStream.Write(array, 0, array.Length);
		if (num != 0)
		{
			base.OutputStream.Write(new byte[num], 0, num);
			entry.Zip64HeaderOffset = (ushort)(30 + array.Length);
		}
		return 30 + array.Length + num;
	}

	private void WriteFooter(uint crc, uint compressed, uint uncompressed)
	{
		Span<byte> span = stackalloc byte[4];
		BinaryPrimitives.WriteUInt32LittleEndian(span, crc);
		base.OutputStream.Write(span);
		BinaryPrimitives.WriteUInt32LittleEndian(span, compressed);
		base.OutputStream.Write(span);
		BinaryPrimitives.WriteUInt32LittleEndian(span, uncompressed);
		base.OutputStream.Write(span);
	}

	private void WriteEndRecord(ulong size)
	{
		bool flag = entries.Count > 65535 || streamPosition >= uint.MaxValue || size >= uint.MaxValue;
		uint value = (uint)((size >= uint.MaxValue) ? uint.MaxValue : size);
		uint value2 = (uint)((streamPosition >= uint.MaxValue) ? uint.MaxValue : streamPosition);
		Span<byte> span = stackalloc byte[8];
		Stream outputStream;
		Span<byte> span2;
		if (flag)
		{
			int num = 44;
			outputStream = base.OutputStream;
			span2 = stackalloc byte[4] { 80, 75, 6, 6 };
			outputStream.Write(span2);
			BinaryPrimitives.WriteUInt64LittleEndian(span, (ulong)num);
			base.OutputStream.Write(span);
			BinaryPrimitives.WriteUInt16LittleEndian(span, 45);
			base.OutputStream.Write(span.Slice(0, 2));
			BinaryPrimitives.WriteUInt16LittleEndian(span, 45);
			base.OutputStream.Write(span.Slice(0, 2));
			BinaryPrimitives.WriteUInt32LittleEndian(span, 0u);
			base.OutputStream.Write(span.Slice(0, 4));
			base.OutputStream.Write(span.Slice(0, 4));
			BinaryPrimitives.WriteUInt64LittleEndian(span, (ulong)entries.Count);
			base.OutputStream.Write(span);
			base.OutputStream.Write(span);
			BinaryPrimitives.WriteUInt64LittleEndian(span, size);
			base.OutputStream.Write(span);
			BinaryPrimitives.WriteUInt64LittleEndian(span, (ulong)streamPosition);
			base.OutputStream.Write(span);
			outputStream = base.OutputStream;
			span2 = stackalloc byte[4] { 80, 75, 6, 7 };
			outputStream.Write(span2);
			BinaryPrimitives.WriteUInt32LittleEndian(span, 0u);
			base.OutputStream.Write(span.Slice(0, 4));
			BinaryPrimitives.WriteUInt64LittleEndian(span, (ulong)streamPosition + size);
			base.OutputStream.Write(span);
			BinaryPrimitives.WriteUInt32LittleEndian(span, 1u);
			base.OutputStream.Write(span.Slice(0, 4));
			streamPosition += 12 + num + 20;
		}
		outputStream = base.OutputStream;
		span2 = stackalloc byte[8] { 80, 75, 5, 6, 0, 0, 0, 0 };
		outputStream.Write(span2);
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)entries.Count);
		base.OutputStream.Write(span.Slice(0, 2));
		base.OutputStream.Write(span.Slice(0, 2));
		BinaryPrimitives.WriteUInt32LittleEndian(span, value);
		base.OutputStream.Write(span.Slice(0, 4));
		BinaryPrimitives.WriteUInt32LittleEndian(span, value2);
		base.OutputStream.Write(span.Slice(0, 4));
		byte[] array = base.WriterOptions.ArchiveEncoding.Encode(zipComment);
		BinaryPrimitives.WriteUInt16LittleEndian(span, (ushort)array.Length);
		base.OutputStream.Write(span.Slice(0, 2));
		base.OutputStream.Write(array, 0, array.Length);
	}
}
