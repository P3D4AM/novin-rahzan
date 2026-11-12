using System.Text;

namespace SharpCompress.Compressors.PPMd.H;

internal class SubRange
{
	private long _lowCount;

	private long _highCount;

	private long _scale;

	internal long HighCount
	{
		get
		{
			return _highCount;
		}
		set
		{
			_highCount = value & 0xFFFFFFFFu;
		}
	}

	internal long LowCount
	{
		get
		{
			return _lowCount & 0xFFFFFFFFu;
		}
		set
		{
			_lowCount = value & 0xFFFFFFFFu;
		}
	}

	internal long Scale
	{
		get
		{
			return _scale;
		}
		set
		{
			_scale = value & 0xFFFFFFFFu;
		}
	}

	internal void IncScale(int dScale)
	{
		Scale += dScale;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SubRange[");
		stringBuilder.Append("\n  lowCount=");
		stringBuilder.Append(_lowCount);
		stringBuilder.Append("\n  highCount=");
		stringBuilder.Append(_highCount);
		stringBuilder.Append("\n  scale=");
		stringBuilder.Append(_scale);
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}
}
