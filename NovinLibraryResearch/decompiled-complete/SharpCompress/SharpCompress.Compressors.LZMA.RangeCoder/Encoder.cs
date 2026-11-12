using System.IO;

namespace SharpCompress.Compressors.LZMA.RangeCoder;

internal class Encoder
{
	public const uint K_TOP_VALUE = 16777216u;

	private Stream _stream;

	public ulong _low;

	public uint _range;

	private uint _cacheSize;

	private byte _cache;

	public void SetStream(Stream stream)
	{
		_stream = stream;
	}

	public void ReleaseStream()
	{
		_stream = null;
	}

	public void Init()
	{
		_low = 0uL;
		_range = uint.MaxValue;
		_cacheSize = 1u;
		_cache = 0;
	}

	public void FlushData()
	{
		for (int i = 0; i < 5; i++)
		{
			ShiftLow();
		}
	}

	public void FlushStream()
	{
		_stream.Flush();
	}

	public void CloseStream()
	{
		_stream.Dispose();
	}

	public void Encode(uint start, uint size, uint total)
	{
		_low += start * (_range /= total);
		_range *= size;
		while (_range < 16777216)
		{
			_range <<= 8;
			ShiftLow();
		}
	}

	public void ShiftLow()
	{
		if ((uint)_low < 4278190080u || (int)(_low >> 32) == 1)
		{
			byte b = _cache;
			do
			{
				_stream.WriteByte((byte)(b + (_low >> 32)));
				b = byte.MaxValue;
			}
			while (--_cacheSize != 0);
			_cache = (byte)((uint)_low >> 24);
		}
		_cacheSize++;
		_low = (uint)((int)_low << 8);
	}

	public void EncodeDirectBits(uint v, int numTotalBits)
	{
		for (int num = numTotalBits - 1; num >= 0; num--)
		{
			_range >>= 1;
			if (((v >> num) & 1) == 1)
			{
				_low += _range;
			}
			if (_range < 16777216)
			{
				_range <<= 8;
				ShiftLow();
			}
		}
	}

	public void EncodeBit(uint size0, int numTotalBits, uint symbol)
	{
		uint num = (_range >> numTotalBits) * size0;
		if (symbol == 0)
		{
			_range = num;
		}
		else
		{
			_low += num;
			_range -= num;
		}
		while (_range < 16777216)
		{
			_range <<= 8;
			ShiftLow();
		}
	}

	public long GetProcessedSizeAdd()
	{
		return -1L;
	}
}
