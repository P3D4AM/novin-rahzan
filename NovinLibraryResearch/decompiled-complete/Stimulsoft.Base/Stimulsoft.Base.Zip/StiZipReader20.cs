using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;

namespace Stimulsoft.Base.Zip;

public class StiZipReader20 : IDisposable
{
	internal enum CompressionMethod
	{
		Stored = 0,
		Deflated = 8,
		Deflate64 = 9,
		BZip2 = 11,
		WinZipAES = 99
	}

	[Flags]
	private enum GeneralBitFlags
	{
		Patched = 0x20,
		StrongEncryption = 0x40,
		UnicodeText = 0x800,
		EnhancedCompress = 0x1000,
		HeaderMasked = 0x2000
	}

	private enum HostSystemID
	{
		Msdos = 0,
		Amiga = 1,
		OpenVms = 2,
		Unix = 3,
		VMCms = 4,
		AtariST = 5,
		OS2 = 6,
		Macintosh = 7,
		ZSystem = 8,
		Cpm = 9,
		WindowsNT = 10,
		MVS = 11,
		Vse = 12,
		AcornRisc = 13,
		Vfat = 14,
		AlternateMvs = 15,
		BeOS = 16,
		Tandem = 17,
		OS400 = 18,
		OSX = 19,
		WinZipAES = 99
	}

	public class StiZipEntry
	{
		[Flags]
		private enum Known : byte
		{
			None = 0,
			Size = 1,
			CompressedSize = 2,
			Crc = 4,
			Time = 8,
			ExternalAttributes = 0x10
		}

		private Known known;

		private int externalFileAttributes = -1;

		private ushort versionMadeBy;

		private ushort versionToExtract;

		private string name;

		private ulong size;

		private ulong compressedSize;

		private uint crc;

		private uint dosTime;

		private CompressionMethod method = CompressionMethod.Deflated;

		private byte[] extra;

		private string comment;

		private int flags;

		private long zipFileIndex = -1L;

		private long offset;

		internal StiZipReader20 zipFile;

		public bool IsCrypted => (flags & 1) != 0;

		internal int Flags
		{
			set
			{
				flags = value;
			}
		}

		internal long ZipFileIndex
		{
			get
			{
				return zipFileIndex;
			}
			set
			{
				zipFileIndex = value;
			}
		}

		internal long Offset
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

		internal int ExternalFileAttributes
		{
			get
			{
				if ((known & Known.ExternalAttributes) == 0)
				{
					return -1;
				}
				return externalFileAttributes;
			}
			set
			{
				externalFileAttributes = value;
				known |= Known.ExternalAttributes;
			}
		}

		private int HostSystem => (versionMadeBy >> 8) & 0xFF;

		internal long DosTime
		{
			set
			{
				dosTime = (uint)value;
				known |= Known.Time;
			}
		}

		internal DateTime DateTime
		{
			set
			{
				uint num = (uint)value.Year;
				uint num2 = (uint)value.Month;
				uint num3 = (uint)value.Day;
				uint num4 = (uint)value.Hour;
				uint num5 = (uint)value.Minute;
				uint num6 = (uint)value.Second;
				if (num < 1980)
				{
					num = 1980u;
					num2 = 1u;
					num3 = 1u;
					num4 = 0u;
					num5 = 0u;
					num6 = 0u;
				}
				else if (num > 2107)
				{
					num = 2107u;
					num2 = 12u;
					num3 = 31u;
					num4 = 23u;
					num5 = 59u;
					num6 = 59u;
				}
				DosTime = (((num - 1980) & 0x7F) << 25) | (num2 << 21) | (num3 << 16) | (num4 << 11) | (num5 << 5) | (num6 >> 1);
			}
		}

		public string FullName => name;

		public string Name
		{
			get
			{
				if (IsFile)
				{
					return Path.GetFileName(name);
				}
				return name;
			}
		}

		public long Size
		{
			get
			{
				if ((known & Known.Size) == 0)
				{
					return -1L;
				}
				return (long)size;
			}
			set
			{
				size = (ulong)value;
				known |= Known.Size;
			}
		}

		internal long CompressedSize
		{
			get
			{
				if ((known & Known.CompressedSize) == 0)
				{
					return -1L;
				}
				return (long)compressedSize;
			}
			set
			{
				compressedSize = (ulong)value;
				known |= Known.CompressedSize;
			}
		}

		internal long Crc
		{
			set
			{
				if ((crc & -4294967296L) != 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				crc = (uint)value;
				known |= Known.Crc;
			}
		}

		internal CompressionMethod CompressionMethod => method;

		internal byte[] ExtraData
		{
			set
			{
				extra = value;
			}
		}

		public string Comment
		{
			set
			{
				comment = value;
			}
		}

		public bool IsDirectory
		{
			get
			{
				int length = name.Length;
				return (length > 0 && (name[length - 1] == '/' || name[length - 1] == '\\')) || HasDosAttributes(16);
			}
		}

		public bool IsFile
		{
			get
			{
				if (!IsDirectory)
				{
					return !HasDosAttributes(8);
				}
				return false;
			}
		}

		internal StiZipEntry(string name, int versionRequiredToExtract, int madeByInfo, CompressionMethod method)
		{
			DateTime = DateTime.Now;
			this.name = name;
			versionMadeBy = (ushort)madeByInfo;
			versionToExtract = (ushort)versionRequiredToExtract;
			this.method = method;
		}

		public void ExtractToFile(string path, bool overwrite)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (directoryName != null && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (!IsFile)
			{
				return;
			}
			Stream entryStream = zipFile.GetEntryStream(this);
			FileStream fileStream = new FileStream(path, FileMode.Create);
			byte[] buffer = new byte[8192];
			int num = (int)Size;
			while (num > 0)
			{
				int num2 = entryStream.Read(buffer, 0, (num > 8192) ? 8192 : num);
				if (num2 > 0)
				{
					fileStream.Write(buffer, 0, num2);
					num -= num2;
				}
				else
				{
					num = 0;
				}
			}
			fileStream.Flush();
			fileStream.Close();
		}

		private bool HasDosAttributes(int attributes)
		{
			bool result = false;
			if ((known & Known.ExternalAttributes) != 0 && (HostSystem == 0 || HostSystem == 10) && (ExternalFileAttributes & attributes) == attributes)
			{
				result = true;
			}
			return result;
		}

		internal void ProcessExtraData(bool localHeader)
		{
			StiZipExtraData stiZipExtraData = new StiZipExtraData(extra);
			if (stiZipExtraData.Find(1))
			{
				if (stiZipExtraData.ValueLength < 4)
				{
					throw new StiZipException("Extra data extended Zip64 information length is invalid");
				}
				if (localHeader || size == uint.MaxValue)
				{
					size = (ulong)stiZipExtraData.ReadLong();
				}
				if (localHeader || compressedSize == uint.MaxValue)
				{
					compressedSize = (ulong)stiZipExtraData.ReadLong();
				}
				if (!localHeader && offset == uint.MaxValue)
				{
					offset = stiZipExtraData.ReadLong();
				}
			}
			if (method == CompressionMethod.WinZipAES)
			{
				throw new StiZipException("Encryption is not supported");
			}
		}

		public bool IsCompressionMethodSupported()
		{
			if (CompressionMethod != CompressionMethod.Deflated)
			{
				return CompressionMethod == CompressionMethod.Stored;
			}
			return true;
		}
	}

	private class StiPartialInputStream : Stream
	{
		private Stream _baseStream;

		private long _start;

		private long _length;

		private long _readPos;

		private long _end;

		public override long Position
		{
			get
			{
				return _readPos - _start;
			}
			set
			{
				long num = _start + value;
				if (num < _start)
				{
					throw new ArgumentException("Negative position is invalid");
				}
				if (num >= _end)
				{
					throw new InvalidOperationException("Cannot seek past end");
				}
				_readPos = num;
			}
		}

		public override long Length => _length;

		public override bool CanWrite => false;

		public override bool CanSeek => true;

		public override bool CanRead => true;

		public StiPartialInputStream(StiZipReader20 zipFile, long start, long length)
		{
			_start = start;
			_length = length;
			_baseStream = zipFile._baseStream;
			_readPos = start;
			_end = start + length;
		}

		public override int ReadByte()
		{
			if (_readPos >= _end)
			{
				return -1;
			}
			lock (_baseStream)
			{
				_baseStream.Seek(_readPos++, SeekOrigin.Begin);
				return _baseStream.ReadByte();
			}
		}

		public override void Close()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			lock (_baseStream)
			{
				if (count > _end - _readPos)
				{
					count = (int)(_end - _readPos);
					if (count == 0)
					{
						return 0;
					}
				}
				_baseStream.Seek(_readPos, SeekOrigin.Begin);
				int num = _baseStream.Read(buffer, offset, count);
				if (num > 0)
				{
					_readPos += num;
				}
				return num;
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = _readPos;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = _start + offset;
				break;
			case SeekOrigin.Current:
				num = _readPos + offset;
				break;
			case SeekOrigin.End:
				num = _end + offset;
				break;
			}
			if (num < _start)
			{
				throw new ArgumentException("Negative position is invalid");
			}
			if (num >= _end)
			{
				throw new IOException("Cannot seek past end");
			}
			_readPos = num;
			return _readPos;
		}

		public override void Flush()
		{
		}
	}

	private sealed class StiZipConstants
	{
		public const int VersionMadeBy = 51;

		public const int VersionZip64 = 45;

		public const int LocalHeaderBaseSize = 30;

		public const int EndOfCentralRecordBaseSize = 22;

		public const int LocalHeaderSignature = 67324752;

		public const int CentralHeaderSignature = 33639248;

		public const int Zip64CentralFileHeaderSignature = 101075792;

		public const int Zip64CentralDirLocatorSignature = 117853008;

		public const int EndOfCentralDirectorySignature = 101010256;

		private static int defaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage;

		public static string ConvertToStringExt(int flags, byte[] data, int count)
		{
			if (data == null)
			{
				return string.Empty;
			}
			if (((uint)flags & 0x800u) != 0)
			{
				return Encoding.UTF8.GetString(data, 0, count);
			}
			Encoding encoding = null;
			try
			{
				encoding = Encoding.GetEncoding(defaultCodePage);
			}
			catch
			{
				defaultCodePage = Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage;
				try
				{
					encoding = Encoding.GetEncoding(defaultCodePage);
				}
				catch
				{
					defaultCodePage = Encoding.ASCII.CodePage;
					encoding = Encoding.GetEncoding(defaultCodePage);
				}
			}
			return encoding.GetString(data, 0, count);
		}
	}

	public class StiZipException : ApplicationException
	{
		public StiZipException()
			: base(string.Empty)
		{
		}

		public StiZipException(string message)
			: base(message)
		{
		}
	}

	internal class StiZipHelperStream : Stream
	{
		private bool isOwner_;

		private Stream stream_;

		public override bool CanRead => stream_.CanRead;

		public override bool CanSeek => stream_.CanSeek;

		public override long Length => stream_.Length;

		public override long Position
		{
			get
			{
				return stream_.Position;
			}
			set
			{
				stream_.Position = value;
			}
		}

		public override bool CanWrite => stream_.CanWrite;

		public StiZipHelperStream(string name)
		{
			stream_ = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);
			isOwner_ = true;
		}

		public StiZipHelperStream(Stream stream)
		{
			stream_ = stream;
		}

		public override void Flush()
		{
			stream_.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return stream_.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			stream_.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return stream_.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			stream_.Write(buffer, offset, count);
		}

		public override void Close()
		{
			Stream stream = stream_;
			stream_ = null;
			if (isOwner_ && stream != null)
			{
				isOwner_ = false;
				stream.Close();
			}
		}

		public long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			long num = endLocation - minimumBlockSize;
			if (num < 0)
			{
				return -1L;
			}
			long num2 = Math.Max(num - maximumVariableData, 0L);
			do
			{
				if (num < num2)
				{
					return -1L;
				}
				Seek(num--, SeekOrigin.Begin);
			}
			while (ReadLEInt() != signature);
			return Position;
		}

		public int ReadLEShort()
		{
			int num = stream_.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			int num2 = stream_.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException();
			}
			return num | (num2 << 8);
		}

		public int ReadLEInt()
		{
			return ReadLEShort() | (ReadLEShort() << 16);
		}

		public long ReadLELong()
		{
			return (uint)ReadLEInt() | ((long)ReadLEInt() << 32);
		}
	}

	private sealed class StiZipExtraData
	{
		private int _index;

		private int _readValueStart;

		private int _readValueLength;

		private byte[] _data;

		public int ValueLength => _readValueLength;

		public StiZipExtraData()
		{
			Clear();
		}

		public StiZipExtraData(byte[] data)
		{
			if (data == null)
			{
				_data = new byte[0];
			}
			else
			{
				_data = data;
			}
		}

		public void Clear()
		{
			if (_data == null || _data.Length != 0)
			{
				_data = new byte[0];
			}
		}

		public bool Find(int headerID)
		{
			_readValueStart = _data.Length;
			_readValueLength = 0;
			_index = 0;
			int num = _readValueStart;
			int num2 = headerID - 1;
			while (num2 != headerID && _index < _data.Length - 3)
			{
				num2 = ReadShortInternal();
				num = ReadShortInternal();
				if (num2 != headerID)
				{
					_index += num;
				}
			}
			bool flag = num2 == headerID && _index + num <= _data.Length;
			if (flag)
			{
				_readValueStart = _index;
				_readValueLength = num;
			}
			return flag;
		}

		public long ReadLong()
		{
			ReadCheck(8);
			return (ReadInt() & 0xFFFFFFFFu) | ((long)ReadInt() << 32);
		}

		public int ReadInt()
		{
			ReadCheck(4);
			int result = _data[_index] + (_data[_index + 1] << 8) + (_data[_index + 2] << 16) + (_data[_index + 3] << 24);
			_index += 4;
			return result;
		}

		private void ReadCheck(int length)
		{
			if (_readValueStart > _data.Length || _readValueStart < 4)
			{
				throw new StiZipException("Find must be called before calling a Read method");
			}
			if (_index > _readValueStart + _readValueLength - length)
			{
				throw new StiZipException("End of extra data");
			}
			if (_index + length < 4)
			{
				throw new StiZipException("Cannot read before start of tag");
			}
		}

		private int ReadShortInternal()
		{
			if (_index > _data.Length - 2)
			{
				throw new StiZipException("End of extra data");
			}
			int result = _data[_index] + (_data[_index + 1] << 8);
			_index += 2;
			return result;
		}
	}

	private class Utils
	{
		public static void ReadFully(Stream stream, byte[] buffer)
		{
			ReadFully(stream, buffer, 0, buffer.Length);
		}

		public static void ReadFully(Stream stream, byte[] buffer, int offset, int count)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			while (count > 0)
			{
				int num = stream.Read(buffer, offset, count);
				if (num <= 0)
				{
					throw new EndOfStreamException();
				}
				offset += num;
				count -= num;
			}
		}
	}

	private bool _isDisposed;

	private Stream _baseStream;

	private bool isStreamOwner;

	private long offsetOfFirstEntry;

	private StiZipEntry[] _entries;

	public StiZipEntry[] Entries => _entries;

	public Stream GetEntryStream(StiZipEntry entry)
	{
		if (_isDisposed)
		{
			throw new ObjectDisposedException("ZipFile");
		}
		if (entry.IsCrypted)
		{
			throw new StiZipException("Encryption is not supported");
		}
		long num = LocateEntry(entry);
		CompressionMethod compressionMethod = entry.CompressionMethod;
		Stream result = new StiPartialInputStream(this, num, entry.CompressedSize);
		switch (compressionMethod)
		{
		case CompressionMethod.Deflated:
		{
			byte[] array = new byte[entry.CompressedSize];
			_baseStream.Seek(num, SeekOrigin.Begin);
			_baseStream.Read(array, 0, array.Length);
			MemoryStream stream = new MemoryStream(array);
			result = new DeflateStream(stream, CompressionMode.Decompress);
			break;
		}
		default:
			throw new StiZipException("Unsupported compression method " + compressionMethod);
		case CompressionMethod.Stored:
			break;
		}
		return result;
	}

	private ushort ReadLEUshort()
	{
		int num = _baseStream.ReadByte();
		if (num < 0)
		{
			throw new EndOfStreamException("End of stream");
		}
		int num2 = _baseStream.ReadByte();
		if (num2 < 0)
		{
			throw new EndOfStreamException("End of stream");
		}
		return (ushort)((ushort)num | (ushort)(num2 << 8));
	}

	private uint ReadLEUint()
	{
		return (uint)(ReadLEUshort() | (ReadLEUshort() << 16));
	}

	private ulong ReadLEUlong()
	{
		return ReadLEUint() | ((ulong)ReadLEUint() << 32);
	}

	private long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
	{
		using StiZipHelperStream stiZipHelperStream = new StiZipHelperStream(_baseStream);
		return stiZipHelperStream.LocateBlockWithSignature(signature, endLocation, minimumBlockSize, maximumVariableData);
	}

	private void ReadEntries()
	{
		long num = LocateBlockWithSignature(101010256, _baseStream.Length, 22, 65535);
		if (num < 0)
		{
			throw new StiZipException("Cannot find central directory");
		}
		ushort num2 = ReadLEUshort();
		ushort num3 = ReadLEUshort();
		ulong num4 = ReadLEUshort();
		ulong num5 = ReadLEUshort();
		ulong num6 = ReadLEUint();
		long num7 = ReadLEUint();
		bool flag = false;
		if (num2 == ushort.MaxValue || num3 == ushort.MaxValue || num4 == 65535 || num5 == 65535 || num6 == uint.MaxValue || num7 == uint.MaxValue)
		{
			flag = true;
			long num8 = LocateBlockWithSignature(117853008, num, 0, 4096);
			if (num8 < 0)
			{
				throw new StiZipException("Cannot find Zip64 locator");
			}
			ReadLEUint();
			ulong num9 = ReadLEUlong();
			uint num10 = ReadLEUint();
			_baseStream.Position = (long)num9;
			long num11 = ReadLEUint();
			if (num11 != 101075792)
			{
				throw new StiZipException($"Invalid Zip64 Central directory signature at {num9:X}");
			}
			ulong num12 = ReadLEUlong();
			int num13 = ReadLEUshort();
			int num14 = ReadLEUshort();
			uint num15 = ReadLEUint();
			uint num16 = ReadLEUint();
			num4 = ReadLEUlong();
			num5 = ReadLEUlong();
			num6 = ReadLEUlong();
			num7 = (long)ReadLEUlong();
		}
		_entries = new StiZipEntry[num4];
		if (!flag && num7 < num - (long)(4 + num6))
		{
			offsetOfFirstEntry = num - ((long)(4 + num6) + num7);
			if (offsetOfFirstEntry <= 0)
			{
				throw new StiZipException("Invalid embedded zip archive");
			}
		}
		_baseStream.Seek(offsetOfFirstEntry + num7, SeekOrigin.Begin);
		for (ulong num17 = 0uL; num17 < num4; num17++)
		{
			if (ReadLEUint() != 33639248)
			{
				throw new StiZipException("Wrong Central Directory signature");
			}
			int madeByInfo = ReadLEUshort();
			int versionRequiredToExtract = ReadLEUshort();
			int flags = ReadLEUshort();
			int method = ReadLEUshort();
			uint num18 = ReadLEUint();
			uint num19 = ReadLEUint();
			long num20 = ReadLEUint();
			long num21 = ReadLEUint();
			int num22 = ReadLEUshort();
			int num23 = ReadLEUshort();
			int num24 = ReadLEUshort();
			int num25 = ReadLEUshort();
			int num26 = ReadLEUshort();
			uint externalFileAttributes = ReadLEUint();
			long offset = ReadLEUint();
			byte[] array = new byte[Math.Max(num22, num24)];
			Utils.ReadFully(_baseStream, array, 0, num22);
			string name = StiZipConstants.ConvertToStringExt(flags, array, num22);
			StiZipEntry stiZipEntry = new StiZipEntry(name, versionRequiredToExtract, madeByInfo, (CompressionMethod)method);
			stiZipEntry.zipFile = this;
			stiZipEntry.Crc = (long)num19 & 0xFFFFFFFFL;
			stiZipEntry.Size = num21 & 0xFFFFFFFFu;
			stiZipEntry.CompressedSize = num20 & 0xFFFFFFFFu;
			stiZipEntry.Flags = flags;
			stiZipEntry.DosTime = num18;
			stiZipEntry.ZipFileIndex = (long)num17;
			stiZipEntry.Offset = offset;
			stiZipEntry.ExternalFileAttributes = (int)externalFileAttributes;
			if (num23 > 0)
			{
				byte[] array2 = new byte[num23];
				Utils.ReadFully(_baseStream, array2);
				stiZipEntry.ExtraData = array2;
			}
			stiZipEntry.ProcessExtraData(localHeader: false);
			if (num24 > 0)
			{
				Utils.ReadFully(_baseStream, array, 0, num24);
				stiZipEntry.Comment = StiZipConstants.ConvertToStringExt(flags, array, num24);
			}
			_entries[num17] = stiZipEntry;
		}
	}

	private long LocateEntry(StiZipEntry entry)
	{
		_baseStream.Seek(offsetOfFirstEntry + entry.Offset, SeekOrigin.Begin);
		if (ReadLEUint() != 67324752)
		{
			throw new StiZipException($"Wrong local header signature @{offsetOfFirstEntry + entry.Offset:X}");
		}
		short num = (short)ReadLEUshort();
		short num2 = (short)ReadLEUshort();
		short num3 = (short)ReadLEUshort();
		short num4 = (short)ReadLEUshort();
		short num5 = (short)ReadLEUshort();
		uint num6 = ReadLEUint();
		long num7 = ReadLEUint();
		long num8 = ReadLEUint();
		int num9 = ReadLEUshort();
		int num10 = ReadLEUshort();
		byte[] buffer = new byte[num9];
		Utils.ReadFully(_baseStream, buffer);
		byte[] array = new byte[num10];
		Utils.ReadFully(_baseStream, array);
		StiZipExtraData stiZipExtraData = new StiZipExtraData(array);
		if (entry.IsFile)
		{
			if (!entry.IsCompressionMethodSupported())
			{
				throw new StiZipException("Compression method not supported");
			}
			if (num > 51 || (num > 20 && num < 45) || ((uint)num2 & 0x3060u) != 0)
			{
				throw new StiZipException("The library does not support the zip version required to extract this entry");
			}
		}
		int num11 = num9 + num10;
		return offsetOfFirstEntry + entry.Offset + 30 + num11;
	}

	void IDisposable.Dispose()
	{
		DisposeInternal(disposing: true);
		GC.SuppressFinalize(this);
	}

	~StiZipReader20()
	{
		DisposeInternal(disposing: false);
	}

	public void Dispose(bool disposing)
	{
		DisposeInternal(disposing);
	}

	private void DisposeInternal(bool disposing)
	{
		if (!_isDisposed)
		{
			_isDisposed = true;
			_entries = new StiZipEntry[0];
			if (isStreamOwner && _baseStream != null)
			{
				_baseStream.Close();
			}
		}
	}

	private void OpenZipFile(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (stream.Length == 0L)
		{
			throw new ArgumentException("Stream is empty", "stream");
		}
		if (!stream.CanSeek)
		{
			throw new ArgumentException("Stream is not seekable", "stream");
		}
		_baseStream = stream;
		isStreamOwner = true;
		try
		{
			ReadEntries();
		}
		catch
		{
			Dispose(disposing: true);
			throw;
		}
	}

	public StiZipReader20(string path)
	{
		FileStream stream = new FileStream(path, FileMode.Open);
		OpenZipFile(stream);
	}

	public StiZipReader20(Stream stream)
	{
		OpenZipFile(stream);
	}
}
