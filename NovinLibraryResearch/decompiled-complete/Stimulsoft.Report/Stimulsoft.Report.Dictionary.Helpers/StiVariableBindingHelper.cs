using System;
using System.Linq;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Dictionary.Helpers;

public static class StiVariableBindingHelper
{
	public static object[] GetArray(object value)
	{
		if (value == null || value is object[])
		{
			return value as object[];
		}
		return new object[1] { value };
	}

	public static bool IsEqualBinding(object valueBinding, object[] selectedValues)
	{
		if (valueBinding == null)
		{
			return false;
		}
		Type valueBindingType = valueBinding.GetType();
		object v3 = ChangeTypeValueBinding(valueBinding);
		return selectedValues?.Any((object v2) => v3.Equals(ChangeTypeValueBinding(v2, valueBindingType))) ?? true;
	}

	public static object ChangeTypeValueBinding(object valueBinding, Type type = null)
	{
		object result = valueBinding;
		type = type ?? valueBinding.GetType();
		if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(sbyte?) || type == typeof(byte?) || type == typeof(short?) || type == typeof(ushort?) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) || type == typeof(ByteList) || type == typeof(ShortList) || type == typeof(IntList) || type == typeof(LongList))
		{
			result = StiValueHelper.TryToLong(valueBinding);
		}
		else if (type == typeof(double) || type == typeof(float) || type == typeof(double?) || type == typeof(float?) || type == typeof(DoubleList) || type == typeof(FloatList))
		{
			result = StiValueHelper.TryToDouble(valueBinding);
		}
		else if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(DecimalList))
		{
			result = StiValueHelper.TryToDecimal(valueBinding);
		}
		else if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(DateTimeList))
		{
			result = StiValueHelper.TryToDateTime(valueBinding);
		}
		else if (type == typeof(TimeSpan))
		{
			result = (TimeSpan)valueBinding;
		}
		else if (type == typeof(TimeSpan?))
		{
			result = (TimeSpan?)valueBinding;
		}
		else if (type == typeof(TimeSpanList))
		{
			result = valueBinding;
		}
		else if (type == typeof(bool) || type == typeof(bool?) || type == typeof(BoolList))
		{
			result = StiValueHelper.TryToBool(valueBinding);
		}
		else if (type == typeof(char) || type == typeof(char?) || type == typeof(CharList))
		{
			result = StiValueHelper.TryToChar(valueBinding);
		}
		else if (type == typeof(Guid))
		{
			result = (Guid)valueBinding;
		}
		else if (type == typeof(Guid?))
		{
			result = (Guid?)valueBinding;
		}
		else if (type == typeof(GuidList))
		{
			result = valueBinding;
		}
		return result;
	}
}
