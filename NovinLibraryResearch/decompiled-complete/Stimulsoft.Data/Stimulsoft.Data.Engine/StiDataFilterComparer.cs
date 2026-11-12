using System;
using System.Collections.Generic;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Data.Engine;

internal class StiDataFilterComparer : IComparer<object>
{
	public bool ConvertStrings { get; set; }

	public int Compare(object x, object y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return 1;
		}
		if (y == null)
		{
			return -1;
		}
		if (x.GetType().IsNumericType() && y.GetType().IsNumericType())
		{
			return StiValueHelper.TryToDecimal(x).CompareTo(StiValueHelper.TryToDecimal(y));
		}
		if (x is string && y is string)
		{
			return string.Compare((string)x, (string)y, StringComparison.Ordinal);
		}
		if (x is DateTime && y is DateTime)
		{
			return -((DateTime)x).CompareTo((DateTime)y);
		}
		if (x is string && y.GetType().IsNumericType())
		{
			if (!ConvertStrings)
			{
				return -1;
			}
			return StiValueHelper.TryToDecimal(x).CompareTo(StiValueHelper.TryToDecimal(y));
		}
		if (y is string && x.GetType().IsNumericType())
		{
			if (!ConvertStrings)
			{
				return 1;
			}
			return StiValueHelper.TryToDecimal(x).CompareTo(StiValueHelper.TryToDecimal(y));
		}
		return 0;
	}

	public bool Similar(object x, object y)
	{
		if (x == null && y == null)
		{
			return true;
		}
		if (x == null || y == null)
		{
			return false;
		}
		if (x is string && y is string)
		{
			return (string)x == (string)y;
		}
		if (x.GetType().IsNumericType() && y.GetType().IsNumericType())
		{
			return StiValueHelper.TryToDecimal(x) == StiValueHelper.TryToDecimal(y);
		}
		if (x is DateTime && y is DateTime)
		{
			return ((DateTime)x).Ticks == ((DateTime)y).Ticks;
		}
		if (ConvertStrings && ((x is string && y.GetType().IsNumericType()) || (y is string && x.GetType().IsNumericType())))
		{
			return StiValueHelper.TryToDecimal(x) == StiValueHelper.TryToDecimal(y);
		}
		return false;
	}

	public StiDataFilterComparer()
	{
	}

	public StiDataFilterComparer(bool convertStrings)
		: this()
	{
		ConvertStrings = convertStrings;
	}
}
