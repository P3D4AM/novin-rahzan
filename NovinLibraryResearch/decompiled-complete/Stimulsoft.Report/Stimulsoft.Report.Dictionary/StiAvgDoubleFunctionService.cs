using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiAvgDoubleFunctionService : StiAggregateFunctionService
{
	private double summary;

	private long count;

	public override string ServiceName => "AvgD";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDouble(stateName, this, "summary", summary);
		base.States.PushInt64(stateName, this, "count", count);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			count = base.States.PopInt64(stateName, this, "count");
			summary = base.States.PopDouble(stateName, this, "summary");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			summary = 0.0;
			count = 0L;
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			summary += StiObjectConverter.ConvertToDouble(value);
			count++;
		}
	}

	public override object GetValue()
	{
		if (count == 0L)
		{
			return 0.0;
		}
		return summary / (double)count;
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to Avg function!");
	}

	public override Type GetResultType()
	{
		return typeof(double);
	}

	public StiAvgDoubleFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiAvgDoubleFunctionService()
	{
	}
}
