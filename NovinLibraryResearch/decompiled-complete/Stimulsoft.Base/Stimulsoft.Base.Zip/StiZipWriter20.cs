using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using Stimulsoft.Base.Crypto;

namespace Stimulsoft.Base.Zip;

public class StiZipWriter20
{
	private static class Constants
	{
		public const int Version = 20;

		public const int LocalHeaderBaseSize = 30;

		public const int CentralHeaderBaseSize = 46;

		public const int DataDescriptorSize = 16;

		public const int CentralHeaderSignature = 33639248;

		public const int LocalHeaderSignature = 67324752;

		public const int EndOfCentralDirectorySignature = 101010256;

		public const int DataDescriptorSignature = 134695760;

		public static Encoding DefaultEncoding;

		static Constants()
		{
			try
			{
				DefaultEncoding = Encoding.GetEncoding(CultureInfo.InstalledUICulture.TextInfo.OEMCodePage);
			}
			catch
			{
				try
				{
					DefaultEncoding = Encoding.GetEncoding(CultureInfo.InstalledUICulture.TextInfo.ANSICodePage);
				}
				catch
				{
					DefaultEncoding = Encoding.GetEncoding(Encoding.ASCII.CodePage);
				}
			}
		}
	}

	private enum CompressionMethod
	{
		Stored = 0,
		Deflated = 8
	}

	[Flags]
	private enum GeneralBitFlags
	{
		None = 0,
		Descriptor = 8,
		UnicodeText = 0x800
	}

	public class ZipException : ApplicationException
	{
		public ZipException(string message)
			: base(message)
		{
		}
	}

	private class ZipEntry
	{
		private string name;

		private ulong size;

		private ulong compressedSize;

		private uint crc;

		private DateTime dateTime;

		private CompressionMethod method = CompressionMethod.Deflated;

		private int flags;

		private long offset;

		private bool isUnknownSize = true;

		public bool IsUnknownSize => isUnknownSize;

		public int Flags
		{
			get
			{
				return flags;
			}
			set
			{
				flags = value;
			}
		}

		public bool UseUnicodeText
		{
			get
			{
				return (flags & 0x800) != 0;
			}
			set
			{
				if (value)
				{
					flags |= 2048;
				}
				else
				{
					flags &= -2049;
				}
			}
		}

		public long Offset
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
			}
		}

		public DateTime DateTime
		{
			get
			{
				return dateTime;
			}
			set
			{
				dateTime = value;
			}
		}

		public string Name => name;

		public long Size
		{
			get
			{
				return (long)size;
			}
			set
			{
				size = (ulong)value;
				isUnknownSize = false;
			}
		}

		public long CompressedSize
		{
			get
			{
				return (long)compressedSize;
			}
			set
			{
				compressedSize = (ulong)value;
			}
		}

		public long Crc
		{
			get
			{
				return crc;
			}
			set
			{
				crc = (uint)value;
			}
		}

		public CompressionMethod CompressionMethod
		{
			get
			{
				return method;
			}
			set
			{
				method = value;
			}
		}

		public bool IsDirectory
		{
			get
			{
				int length = name.Length;
				if (length > 0)
				{
					if (name[length - 1] != '/')
					{
						return name[length - 1] == '\\';
					}
					return true;
				}
				return false;
			}
		}

		public ZipEntry(string name)
			: this(name, CompressionMethod.Deflated)
		{
		}

		public ZipEntry(string name, CompressionMethod method)
		{
			if (name == null)
			{
				throw new ZipException("ZipEntry name is null");
			}
			if (name.Length > 65535)
			{
				throw new ZipException("ZipEntry name is too long");
			}
			this.name = name;
			DateTime = DateTime.Now;
			this.method = method;
			flags = 0;
			isUnknownSize = true;
		}
	}

	private class DeflaterOutputStream : Stream
	{
		protected int TotalOut;

		protected Stream baseOutputStream;

		private DeflateStream def;

		private bool isClosed;

		private bool isFinished;

		private bool isStreamOwner;

		public bool IsStreamOwner
		{
			get
			{
				return isStreamOwner;
			}
			set
			{
				isStreamOwner = value;
			}
		}

		public bool CanPatchEntries => baseOutputStream.CanSeek;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => baseOutputStream.CanWrite;

		public override long Length => baseOutputStream.Length;

		public override long Position
		{
			get
			{
				return baseOutputStream.Position;
			}
			set
			{
				throw new NotSupportedException("Position property not supported");
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("DeflaterOutputStream Seek not supported");
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
		}

		public override int ReadByte()
		{
			throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("DeflaterOutputStream Read not supported");
		}

		public override void WriteByte(byte value)
		{
			Write(new byte[1] { value }, 0, 1);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (isFinished)
			{
				throw new ZipException("DeflateStream is finished");
			}
			long position = baseOutputStream.Position;
			def.Write(buffer, offset, count);
			TotalOut += (int)(baseOutputStream.Position - position);
		}

		public void Reset()
		{
			isFinished = false;
			def = new DeflateStream(baseOutputStream, CompressionMode.Compress, leaveOpen: true);
			TotalOut = 0;
		}

		public virtual void Finish()
		{
			if (!isFinished)
			{
				isFinished = true;
				long position = baseOutputStream.Position;
				def.Close();
				def = null;
				TotalOut += (int)(baseOutputStream.Position - position);
				baseOutputStream.Flush();
			}
		}

		public override void Flush()
		{
			def.Flush();
			baseOutputStream.Flush();
		}

		public override void Close()
		{
			if (!isClosed)
			{
				isClosed = true;
				Finish();
				if (isStreamOwner)
				{
					baseOutputStream.Close();
				}
				baseOutputStream = null;
			}
		}

		public DeflaterOutputStream(Stream baseOutputStream)
		{
			if (baseOutputStream == null)
			{
				throw new ZipException("OutputStream cannot be null");
			}
			if (!baseOutputStream.CanWrite)
			{
				throw new ZipException("OutputStream must support writing");
			}
			this.baseOutputStream = baseOutputStream;
		}
	}

	private class ZipOutputStream : DeflaterOutputStream
	{
		private ArrayList entriesList = new ArrayList();

		private Crc32 crc = new Crc32();

		private ZipEntry currentEntry;

		private long size;

		private long offset;

		private bool patchEntryHeader;

		private long crcPatchPos;

		private const int extraLength = 0;

		private const int commentLength = 0;

		private void WriteDataShort(int value)
		{
			baseOutputStream.WriteByte((byte)((uint)value & 0xFFu));
			baseOutputStream.WriteByte((byte)((uint)(value >> 8) & 0xFFu));
		}

		private void WriteDataInt(int value)
		{
			WriteDataShort(value);
			WriteDataShort(value >> 16);
		}

		private void WriteDataLong(long value)
		{
			WriteDataInt((int)value);
			WriteDataInt((int)(value >> 32));
		}

		public void PutNextEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ZipException("ZipEntry is null");
			}
			if (entriesList == null)
			{
				throw new ZipException("ZipOutputStream was finished");
			}
			if (entriesList.Count == int.MaxValue)
			{
				throw new ZipException("Too many entries for Zip file");
			}
			if (currentEntry != null)
			{
				CloseEntry();
			}
			if (entry.CompressionMethod == CompressionMethod.Stored && !base.CanPatchEntries)
			{
				entry.CompressionMethod = CompressionMethod.Deflated;
			}
			patchEntryHeader = true;
			if (!base.CanPatchEntries)
			{
				entry.Flags |= 8;
				patchEntryHeader = false;
			}
			entry.Offset = offset;
			WriteDataInt(67324752);
			WriteDataShort(20);
			WriteDataShort(entry.Flags);
			WriteDataShort((byte)entry.CompressionMethod);
			WriteDataInt((int)GetDosTime(entry.DateTime));
			if (patchEntryHeader)
			{
				crcPatchPos = baseOutputStream.Position;
			}
			WriteDataInt(0);
			WriteDataInt(0);
			WriteDataInt(0);
			byte[] array = ConvertToArray(entry.UseUnicodeText, entry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name too long");
			}
			WriteDataShort(array.Length);
			WriteDataShort(0);
			if (array.Length != 0)
			{
				baseOutputStream.Write(array, 0, array.Length);
			}
			offset += 30 + array.Length;
			currentEntry = entry;
			size = 0L;
			crc.Reset();
			if (entry.CompressionMethod == CompressionMethod.Deflated)
			{
				Reset();
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (currentEntry == null)
			{
				throw new ZipException("No open entry");
			}
			if (buffer == null)
			{
				throw new ZipException("Buffer is null");
			}
			if (offset < 0)
			{
				throw new ZipException("Offset cannot be negative");
			}
			if (count < 0)
			{
				throw new ZipException("Count cannot be negative");
			}
			if (buffer.Length - offset < count)
			{
				throw new ZipException("Invalid offset/count combination");
			}
			crc.Update(buffer, offset, count);
			size += count;
			switch (currentEntry.CompressionMethod)
			{
			case CompressionMethod.Deflated:
				base.Write(buffer, offset, count);
				break;
			case CompressionMethod.Stored:
				baseOutputStream.Write(buffer, offset, count);
				break;
			}
		}

		public void CloseEntry()
		{
			if (currentEntry == null)
			{
				throw new ZipException("No open entry");
			}
			if (currentEntry.CompressionMethod == CompressionMethod.Deflated)
			{
				base.Finish();
			}
			long num = ((currentEntry.CompressionMethod == CompressionMethod.Deflated) ? TotalOut : size);
			currentEntry.Size = size;
			currentEntry.CompressedSize = num;
			currentEntry.Crc = crc.Value;
			offset += num;
			if (patchEntryHeader)
			{
				patchEntryHeader = false;
				long position = baseOutputStream.Position;
				baseOutputStream.Seek(crcPatchPos, SeekOrigin.Begin);
				WriteDataInt((int)currentEntry.Crc);
				WriteDataInt((int)currentEntry.CompressedSize);
				WriteDataInt((int)currentEntry.Size);
				baseOutputStream.Seek(position, SeekOrigin.Begin);
			}
			if (((uint)currentEntry.Flags & 8u) != 0)
			{
				WriteDataInt(134695760);
				WriteDataInt((int)currentEntry.Crc);
				WriteDataInt((int)currentEntry.CompressedSize);
				WriteDataInt((int)currentEntry.Size);
				offset += 16L;
			}
			entriesList.Add(currentEntry);
			currentEntry = null;
		}

		public override void Finish()
		{
			if (entriesList == null)
			{
				return;
			}
			if (currentEntry != null)
			{
				CloseEntry();
			}
			long num = entriesList.Count;
			long num2 = 0L;
			foreach (ZipEntry entries in entriesList)
			{
				WriteDataInt(33639248);
				WriteDataShort(20);
				WriteDataShort(20);
				WriteDataShort(entries.Flags);
				WriteDataShort((short)entries.CompressionMethod);
				WriteDataInt((int)GetDosTime(entries.DateTime));
				WriteDataInt((int)entries.Crc);
				WriteDataInt((int)entries.CompressedSize);
				WriteDataInt((int)entries.Size);
				byte[] array = ConvertToArray(entries.UseUnicodeText, entries.Name);
				if (array.Length > 65535)
				{
					throw new ZipException("Entry name too long.");
				}
				WriteDataShort(array.Length);
				WriteDataShort(0);
				WriteDataShort(0);
				WriteDataShort(0);
				WriteDataShort(0);
				if (entries.IsDirectory)
				{
					WriteDataInt(16);
				}
				else
				{
					WriteDataInt(0);
				}
				WriteDataInt((int)entries.Offset);
				if (array.Length != 0)
				{
					baseOutputStream.Write(array, 0, array.Length);
				}
				num2 += 46 + array.Length;
			}
			long num3 = offset;
			WriteDataInt(101010256);
			WriteDataShort(0);
			WriteDataShort(0);
			WriteDataShort((short)num);
			WriteDataShort((short)num);
			WriteDataInt((int)num2);
			WriteDataInt((int)num3);
			WriteDataShort(0);
			entriesList = null;
		}

		public override void Close()
		{
			entriesList = null;
			crc = null;
			currentEntry = null;
			base.Close();
		}

		public ZipOutputStream(Stream baseOutputStream)
			: base(baseOutputStream)
		{
		}
	}

	private Stream mainStream;

	private ZipOutputStream zipStream;

	private static byte[] ConvertToArray(bool useUnicode, string str)
	{
		if (str == null)
		{
			return new byte[0];
		}
		if (useUnicode)
		{
			return Encoding.UTF8.GetBytes(str);
		}
		return Constants.DefaultEncoding.GetBytes(str);
	}

	private static uint GetDosTime(DateTime dt)
	{
		return (uint)((((dt.Year - 1980) & 0x7F) << 25) | (dt.Month << 21) | (dt.Day << 16) | (dt.Hour << 11) | (dt.Minute << 5)) | ((uint)dt.Second >> 1);
	}

	public void Begin(Stream stream, bool leaveOpen)
	{
		mainStream = stream;
		if (mainStream == null)
		{
			throw new ZipException("Output stream is null");
		}
		zipStream = new ZipOutputStream(mainStream);
		zipStream.IsStreamOwner = !leaveOpen;
	}

	public void AddFile(string fileName, MemoryStream dataStream)
	{
		AddFile(fileName, dataStream, closeDataStream: false);
	}

	public void AddFile(string fileName, string sourceFileName)
	{
		AddFile(fileName, sourceFileName, useUnicodeFileName: false);
	}

	public void AddFile(string fileName, string sourceFileName, bool useUnicodeFileName)
	{
		if (File.Exists(sourceFileName))
		{
			byte[] buffer = File.ReadAllBytes(sourceFileName);
			MemoryStream dataStream = new MemoryStream(buffer);
			AddFile(fileName, dataStream, closeDataStream: false, useUnicodeFileName);
		}
	}

	public void AddFile(string fileName, MemoryStream dataStream, bool closeDataStream)
	{
		AddFile(fileName, dataStream, closeDataStream, useUnicodeFileName: false);
	}

	public void AddFile(string fileName, MemoryStream dataStream, bool closeDataStream, bool useUnicodeFileName)
	{
		ZipEntry zipEntry = new ZipEntry(fileName);
		zipEntry.UseUnicodeText = useUnicodeFileName;
		zipStream.PutNextEntry(zipEntry);
		dataStream.WriteTo(zipStream);
		zipStream.CloseEntry();
		if (closeDataStream)
		{
			dataStream.Close();
		}
	}

	public void End()
	{
		zipStream.Finish();
		zipStream.Close();
		mainStream = null;
		zipStream = null;
	}

	public StiZipWriter20()
	{
		mainStream = null;
		zipStream = null;
	}
}
