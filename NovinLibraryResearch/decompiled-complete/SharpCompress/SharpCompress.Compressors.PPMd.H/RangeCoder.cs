using System.IO;
using System.Text;
using SharpCompress.Compressors.Rar;

namespace SharpCompress.Compressors.PPMd.H;

internal class RangeCoder
{
	internal const int TOP = 16777216;

	internal const int BOT = 32768;

	internal const long UINT_MASK = 4294967295L;

	private long _low;

	private long _code;

	private long _range;

	private readonly IRarUnpack _unpackRead;

	private readonly Stream _stream;

	internal int CurrentCount
	{
		get
		{
			_range = (_range / SubRange.Scale) & 0xFFFFFFFFu;
			return (int)((_code - _low) / _range);
		}
	}

	private long Char
	{
		get
		{
			if (_unpackRead != null)
			{
				return _unpackRead.Char;
			}
			if (_stream != null)
			{
				return _stream.ReadByte();
			}
			return -1L;
		}
	}

	internal SubRange SubRange { get; private set; }

	internal RangeCoder(IRarUnpack unpackRead)
	{
		_unpackRead = unpackRead;
		Init();
	}

	internal RangeCoder(Stream stream)
	{
		_stream = stream;
		Init();
	}

	private void Init()
	{
		SubRange = new SubRange();
		_low = (_code = 0L);
		_range = 4294967295L;
		for (int i = 0; i < 4; i++)
		{
			_code = ((_code << 8) | Char) & 0xFFFFFFFFu;
		}
	}

	internal long GetCurrentShiftCount(int shift)
	{
		_range = Utility.URShift(_range, shift);
		return ((_code - _low) / _range) & 0xFFFFFFFFu;
	}

	internal void Decode()
	{
		_low = (_low + _range * SubRange.LowCount) & 0xFFFFFFFFu;
		_range = (_range * (SubRange.HighCount - SubRange.LowCount)) & 0xFFFFFFFFu;
	}

	internal void AriDecNormalize()
	{
		bool flag = false;
		while ((_low ^ (_low + _range)) < 16777216 || (flag = _range < 32768))
		{
			if (flag)
			{
				_range = -_low & 0x7FFF & 0xFFFFFFFFu;
				flag = false;
			}
			_code = ((_code << 8) | Char) & 0xFFFFFFFFu;
			_range = (_range << 8) & 0xFFFFFFFFu;
			_low = (_low << 8) & 0xFFFFFFFFu;
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("RangeCoder[");
		stringBuilder.Append("\n  low=");
		stringBuilder.Append(_low);
		stringBuilder.Append("\n  code=");
		stringBuilder.Append(_code);
		stringBuilder.Append("\n  range=");
		stringBuilder.Append(_range);
		stringBuilder.Append("\n  subrange=");
		stringBuilder.Append(SubRange);
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}
}
