using System;
using System.Collections;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSumDistinctDecimalFunctionService : StiAggregateFunctionService
{
	private decimal summary;

	private Hashtable values;

	public override string ServiceName => "SumDistinct";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDecimal(stateName, this, "summary", summary);
		if (values != null)
		{
			base.States.Push(stateName, this, "values", values.Clone());
		}
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			summary = base.States.PopDecimal(stateName, this, "summary");
			values = base.States.Pop(stateName, this, "values") as Hashtable;
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			summary = default(decimal);
			values = new Hashtable();
		}
	}

	public override void CalcItem(object value)
	{
		if (values == null)
		{
			values = new Hashtable();
		}
		if (value != null && value != DBNull.Value && values[value] == null)
		{
			values.Add(value, value);
			summary += StiObjectConverter.ConvertToDecimal(value);
		}
	}

	public void CalcItem(object value, object valueToSum)
	{
		if (values == null)
		{
			values = new Hashtable();
		}
		if (value != null && value != DBNull.Value && values[value] == null)
		{
			values.Add(value, value);
			summary += StiObjectConverter.ConvertToDecimal(valueToSum);
		}
	}

	public override object GetValue()
	{
		return summary;
	}

	public override void SetValue(object value)
	{
		summary = (decimal)value;
	}

	public override Type GetResultType()
	{
		return typeof(decimal);
	}

	public StiSumDistinctDecimalFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiSumDistinctDecimalFunctionService()
	{
	}
}
