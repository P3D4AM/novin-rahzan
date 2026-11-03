using System;
using System.ComponentModel;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[RefreshProperties(RefreshProperties.All)]
[TypeConverter(typeof(StiConditionConverter))]
public class StiBaseCondition : StiFilter
{
	[StiNonSerialized]
	public object Tag { get; set; }

	public StiBaseCondition()
	{
	}

	public StiBaseCondition(string expression)
		: base(expression)
	{
	}

	public StiBaseCondition(string column, StiFilterCondition condition, DateTime date1)
		: base(column, condition, date1)
	{
	}

	public StiBaseCondition(string column, StiFilterCondition condition, DateTime date1, DateTime date2)
		: base(column, condition, date1, date2)
	{
	}

	public StiBaseCondition(string column, StiFilterCondition condition, string value, StiFilterDataType dataType)
		: base(column, condition, value, dataType)
	{
	}

	public StiBaseCondition(string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType)
		: base(column, condition, value1, value2, dataType)
	{
	}

	public StiBaseCondition(StiFilterItem item, string column, StiFilterCondition condition, string value1, string value2, StiFilterDataType dataType, string expression)
		: base(item, column, condition, value1, value2, dataType, expression)
	{
	}
}
