using System.IO;

namespace SharpCompress.Compressors.LZMA.RangeCoder;

internal class Decoder
{
	public const uint K_TOP_VALUE = 16777216u;

	public uint _range;

	public uint _code;

	public Stream _stream;

	public long _total;

	public bool IsFinished => _code == 0;

	public void Init(Stream stream)
	{
		_stream = stream;
		_code = 0u;
		_range = uint.MaxValue;
		for (int i = 0; i < 5; i++)
		{
			_code = (_code << 8) | (byte)_stream.ReadByte();
		}
		_total = 5L;
	}

	public void ReleaseStream()
	{
		_stream = null;
	}

	public void CloseStream()
	{
		_stream.Dispose();
	}

	public void Normalize()
	{
		while (_range < 16777216)
		{
			_code = (_code << 8) | (byte)_stream.ReadByte();
			_range <<= 8;
			_total++;
		}
	}

	public void Normalize2()
	{
		if (_range < 16777216)
		{
			_code = (_code << 8) | (byte)_stream.ReadByte();
			_range <<= 8;
			_total++;
		}
	}

	public uint GetThreshold(uint total)
	{
		return _code / (_range /= total);
	}

	public void Decode(uint start, uint size)
	{
		_code -= start * _range;
		_range *= size;
		Normalize();
	}

	public uint DecodeDirectBits(int numTotalBits)
	{
		uint num = _range;
		uint num2 = _code;
		uint num3 = 0u;
		for (int num4 = numTotalBits; num4 > 0; num4--)
		{
			num >>= 1;
			uint num5 = num2 - num >> 31;
			num2 -= num & (num5 - 1);
			num3 = (num3 << 1) | (1 - num5);
			if (num < 16777216)
			{
				num2 = (num2 << 8) | (byte)_stream.ReadByte();
				num <<= 8;
				_total++;
			}
		}
		_range = num;
		_code = num2;
		return num3;
	}

	public uint DecodeBit(uint size0, int numTotalBits)
	{
		uint num = (_range >> numTotalBits) * size0;
		uint result;
		if (_code < num)
		{
			result = 0u;
			_range = num;
		}
		else
		{
			result = 1u;
			_code -= num;
			_range -= num;
		}
		Normalize();
		return result;
	}
}
