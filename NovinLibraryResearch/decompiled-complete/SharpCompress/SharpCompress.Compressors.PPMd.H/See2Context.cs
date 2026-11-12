using System.Text;

namespace SharpCompress.Compressors.PPMd.H;

internal class See2Context
{
	public const int SIZE = 4;

	private int _summ;

	private int _shift;

	private int _count;

	public virtual int Mean
	{
		get
		{
			int num = Utility.URShift(_summ, _shift);
			_summ -= num;
			return num + ((num == 0) ? 1 : 0);
		}
	}

	public virtual int Count
	{
		get
		{
			return _count;
		}
		set
		{
			_count = value & 0xFF;
		}
	}

	public virtual int Shift
	{
		get
		{
			return _shift;
		}
		set
		{
			_shift = value & 0xFF;
		}
	}

	public virtual int Summ
	{
		get
		{
			return _summ;
		}
		set
		{
			_summ = value & 0xFFFF;
		}
	}

	public void Initialize(int initVal)
	{
		_shift = 3;
		_summ = (initVal << _shift) & 0xFFFF;
		_count = 4;
	}

	public virtual void Update()
	{
		if (_shift < 7 && --_count == 0)
		{
			_summ += _summ;
			_count = 3 << _shift++;
		}
		_summ &= 65535;
		_count &= 255;
		_shift &= 255;
	}

	public virtual void IncSumm(int dSumm)
	{
		Summ += dSumm;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SEE2Context[");
		stringBuilder.Append("\n  size=");
		stringBuilder.Append(4);
		stringBuilder.Append("\n  summ=");
		stringBuilder.Append(_summ);
		stringBuilder.Append("\n  shift=");
		stringBuilder.Append(_shift);
		stringBuilder.Append("\n  count=");
		stringBuilder.Append(_count);
		stringBuilder.Append("\n]");
		return stringBuilder.ToString();
	}
}
