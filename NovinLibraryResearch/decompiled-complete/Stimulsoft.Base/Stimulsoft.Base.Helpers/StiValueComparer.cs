using System;

namespace Stimulsoft.Base.Helpers;

public static class StiValueComparer
{
	public static bool EqualValues(object value1, object value2)
	{
		if (value1 == null || value2 == null)
		{
			return false;
		}
		if (value1 is Array && value2 is Array)
		{
			return CompareArrays(value1 as Array, value2 as Array);
		}
		if (value1.GetType().IsNumericType() && value2.GetType().IsNumericType())
		{
			return StiValueHelper.TryToDecimal(value1) == StiValueHelper.TryToDecimal(value2);
		}
		if (value1 is string)
		{
			return (value1 as string).Equals(value2.ToString());
		}
		return value1.Equals(value2);
	}

	private static bool CompareArrays(Array a, Array b)
	{
		if (a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (!EqualValues(a.GetValue(i), b.GetValue(i)))
			{
				return false;
			}
		}
		return true;
	}
}
