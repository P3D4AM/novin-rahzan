using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiAvgDecimalFunctionService : StiAggregateFunctionService
{
	private decimal summary;

	private long count;

	public override string ServiceName => "Avg";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDecimal(stateName, this, "summary", summary);
		base.States.PushInt64(stateName, this, "count", count);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			count = base.States.PopInt64(stateName, this, "count");
			summary = base.States.PopDecimal(stateName, this, "summary");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			summary = default(decimal);
			count = 0L;
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			summary += StiObjectConverter.ConvertToDecimal(value);
			count++;
		}
	}

	public override object GetValue()
	{
		if (count == 0L)
		{
			return 0m;
		}
		return summary / (decimal)count;
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to Avg function!");
	}

	public override Type GetResultType()
	{
		return typeof(decimal);
	}

	public StiAvgDecimalFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiAvgDecimalFunctionService()
	{
	}
}
