using System;
using System.IO;

namespace SharpCompress.Crypto;

internal sealed class Crc32Stream : Stream
{
	public const uint DefaultPolynomial = 3988292384u;

	public const uint DefaultSeed = uint.MaxValue;

	private static uint[] defaultTable;

	private readonly uint[] table;

	private uint hash;

	private readonly Stream stream;

	public Stream WrappedStream => stream;

	public override bool CanRead => stream.CanRead;

	public override bool CanSeek => false;

	public override bool CanWrite => stream.CanWrite;

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

	public uint Crc => ~hash;

	public Crc32Stream(Stream stream)
		: this(stream, 3988292384u, uint.MaxValue)
	{
	}

	public Crc32Stream(Stream stream, uint polynomial, uint seed)
	{
		this.stream = stream;
		table = InitializeTable(polynomial);
		hash = seed;
	}

	public override void Flush()
	{
		stream.Flush();
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
		stream.Write(buffer, offset, count);
		hash = CalculateCrc(table, hash, MemoryExtensions.AsSpan(buffer, offset, count));
	}

	public override void WriteByte(byte value)
	{
		stream.WriteByte(value);
		hash = CalculateCrc(table, hash, value);
	}

	public static uint Compute(byte[] buffer)
	{
		return Compute(uint.MaxValue, buffer);
	}

	public static uint Compute(uint seed, byte[] buffer)
	{
		return Compute(3988292384u, seed, buffer);
	}

	public static uint Compute(uint polynomial, uint seed, ReadOnlySpan<byte> buffer)
	{
		return ~CalculateCrc(InitializeTable(polynomial), seed, buffer);
	}

	private static uint[] InitializeTable(uint polynomial)
	{
		if (polynomial == 3988292384u && defaultTable != null)
		{
			return defaultTable;
		}
		uint[] array = new uint[256];
		for (int i = 0; i < 256; i++)
		{
			uint num = (uint)i;
			for (int j = 0; j < 8; j++)
			{
				num = (((num & 1) != 1) ? (num >> 1) : ((num >> 1) ^ polynomial));
			}
			array[i] = num;
		}
		if (polynomial == 3988292384u)
		{
			defaultTable = array;
		}
		return array;
	}

	private static uint CalculateCrc(uint[] table, uint crc, ReadOnlySpan<byte> buffer)
	{
		for (int i = 0; i < buffer.Length; i++)
		{
			crc = CalculateCrc(table, crc, buffer[i]);
		}
		return crc;
	}

	private static uint CalculateCrc(uint[] table, uint crc, byte b)
	{
		return (crc >> 8) ^ table[(crc ^ b) & 0xFF];
	}
}
