using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSumIntFunctionService : StiAggregateFunctionService
{
	private long summary;

	public override string ServiceName => "SumI";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushInt64(stateName, this, "summary", summary);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			summary = base.States.PopInt64(stateName, this, "summary");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			summary = 0L;
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			summary += StiObjectConverter.ConvertToInt64(value);
		}
	}

	public override object GetValue()
	{
		return summary;
	}

	public override void SetValue(object value)
	{
		summary = (long)value;
	}

	public override Type GetResultType()
	{
		return typeof(long);
	}

	public StiSumIntFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiSumIntFunctionService()
	{
	}
}
