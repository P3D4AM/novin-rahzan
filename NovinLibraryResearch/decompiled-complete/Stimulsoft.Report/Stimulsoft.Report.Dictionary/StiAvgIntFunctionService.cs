using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiAvgIntFunctionService : StiAggregateFunctionService
{
	private long summary;

	private long count;

	public override string ServiceName => "AvgI";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushInt64(stateName, this, "summary", summary);
		base.States.PushInt64(stateName, this, "count", count);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			count = base.States.PopInt64(stateName, this, "count");
			summary = base.States.PopInt64(stateName, this, "summary");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			summary = 0L;
			count = 0L;
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			summary += StiObjectConverter.ConvertToInt64(value);
			count++;
		}
	}

	public override object GetValue()
	{
		if (count == 0L)
		{
			return 0;
		}
		return summary / count;
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to AvgI function!");
	}

	public override Type GetResultType()
	{
		return typeof(long);
	}

	public StiAvgIntFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiAvgIntFunctionService()
	{
	}
}
