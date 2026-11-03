using System;

namespace Stimulsoft.Report.Dictionary;

public class StiSumTimeFunctionService : StiAggregateFunctionService
{
	private decimal sumValue;

	public override string ServiceName => "SumTime";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDecimal(stateName, this, "sumValue", sumValue);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			sumValue = base.States.PopDecimal(stateName, this, "sumValue");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			sumValue = default(decimal);
		}
	}

	public override void CalcItem(object value)
	{
		if (value is TimeSpan timeSpan)
		{
			sumValue += (decimal)timeSpan.Ticks;
		}
	}

	public override object GetValue()
	{
		return new TimeSpan((long)sumValue);
	}

	public override void SetValue(object value)
	{
		sumValue = (decimal)value;
	}

	public override Type GetResultType()
	{
		return typeof(TimeSpan);
	}

	public StiSumTimeFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiSumTimeFunctionService()
	{
	}
}
