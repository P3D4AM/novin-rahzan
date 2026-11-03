using System;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

[Serializable]
[TypeConverter(typeof(RangeValueConverter))]
public struct RangeValue
{
	[DefaultValue(0)]
	public int Min;

	[DefaultValue(0)]
	public int Max;

	public RangeValue(int min, int max)
	{
		Min = min;
		Max = max;
	}

	public override string ToString()
	{
		return $"Min={Min}, Max={Max}";
	}
}
