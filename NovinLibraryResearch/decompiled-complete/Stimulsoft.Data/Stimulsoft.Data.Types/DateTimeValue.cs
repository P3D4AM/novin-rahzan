using System;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Data.Types;

public class DateTimeValue
{
	public DateTime? Value { get; }

	public DateTimeValue(object value)
	{
		Value = StiValueHelper.TryToNullableDateTime(value);
	}
}
