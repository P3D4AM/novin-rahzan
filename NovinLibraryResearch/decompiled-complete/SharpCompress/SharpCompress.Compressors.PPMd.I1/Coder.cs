using System.IO;

namespace SharpCompress.Compressors.PPMd.I1;

internal class Coder
{
	private const uint RANGE_TOP = 16777216u;

	private const uint RANGE_BOTTOM = 32768u;

	private uint _low;

	private uint _code;

	private uint _range;

	public uint _lowCount;

	public uint _highCount;

	public uint _scale;

	public void RangeEncoderInitialize()
	{
		_low = 0u;
		_range = uint.MaxValue;
	}

	public void RangeEncoderNormalize(Stream stream)
	{
		while (true)
		{
			if ((_low ^ (_low + _range)) >= 16777216)
			{
				if (_range >= 32768)
				{
					break;
				}
				if ((_range = (uint)(int)(0uL - (ulong)_low) & 0x7FFFu) != 0)
				{
				}
			}
			stream.WriteByte((byte)(_low >> 24));
			_range <<= 8;
			_low <<= 8;
		}
	}

	public void RangeEncodeSymbol()
	{
		_low += _lowCount * (_range /= _scale);
		_range *= _highCount - _lowCount;
	}

	public void RangeShiftEncodeSymbol(int rangeShift)
	{
		_low += _lowCount * (_range >>= rangeShift);
		_range *= _highCount - _lowCount;
	}

	public void RangeEncoderFlush(Stream stream)
	{
		for (uint num = 0u; num < 4; num++)
		{
			stream.WriteByte((byte)(_low >> 24));
			_low <<= 8;
		}
	}

	public void RangeDecoderInitialize(Stream stream)
	{
		_low = 0u;
		_code = 0u;
		_range = uint.MaxValue;
		for (uint num = 0u; num < 4; num++)
		{
			_code = (_code << 8) | (byte)stream.ReadByte();
		}
	}

	public void RangeDecoderNormalize(Stream stream)
	{
		while (true)
		{
			if ((_low ^ (_low + _range)) >= 16777216)
			{
				if (_range >= 32768)
				{
					break;
				}
				if ((_range = (uint)(int)(0uL - (ulong)_low) & 0x7FFFu) != 0)
				{
				}
			}
			_code = (_code << 8) | (byte)stream.ReadByte();
			_range <<= 8;
			_low <<= 8;
		}
	}

	public uint RangeGetCurrentCount()
	{
		return (_code - _low) / (_range /= _scale);
	}

	public uint RangeGetCurrentShiftCount(int rangeShift)
	{
		return (_code - _low) / (_range >>= rangeShift);
	}

	public void RangeRemoveSubrange()
	{
		_low += _range * _lowCount;
		_range *= _highCount - _lowCount;
	}
}
