using System;
using System.Collections;
using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report;

[TypeConverter(typeof(RangeConverter))]
[StiSerializable]
public abstract class Range
{
	public abstract string RangeName { get; }

	public abstract Type RangeType { get; }

	public abstract object FromObject { get; set; }

	public abstract object ToObject { get; set; }

	public string FromStrLoc => Loc.Get("PropertyMain", "RangeFrom");

	public string ToStrLoc => Loc.Get("PropertyMain", "RangeTo");

	public static bool IsRangeType(Type type)
	{
		return typeof(Range).IsAssignableFrom(type);
	}

	public abstract void Parse(string from, string to);

	public override bool Equals(object obj)
	{
		if (!(obj is Range range))
		{
			return false;
		}
		if (range != null && Comparer.Default.Compare(FromObject, range.FromObject) == 0)
		{
			return Comparer.Default.Compare(ToObject, range.ToObject) == 0;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
