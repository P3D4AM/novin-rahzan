using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSumDoubleFunctionService : StiAggregateFunctionService
{
	private double summary;

	public override string ServiceName => "SumD";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDouble(stateName, this, "summary", summary);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			summary = base.States.PopDouble(stateName, this, "summary");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			summary = 0.0;
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			summary += StiObjectConverter.ConvertToDouble(value);
		}
	}

	public override object GetValue()
	{
		return summary;
	}

	public override void SetValue(object value)
	{
		summary = (double)value;
	}

	public override Type GetResultType()
	{
		return typeof(double);
	}

	public StiSumDoubleFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiSumDoubleFunctionService()
	{
	}
}
