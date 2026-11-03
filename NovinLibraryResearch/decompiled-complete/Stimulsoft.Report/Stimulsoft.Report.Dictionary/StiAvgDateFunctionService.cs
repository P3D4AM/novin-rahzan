using System;

namespace Stimulsoft.Report.Dictionary;

public class StiAvgDateFunctionService : StiAggregateFunctionService
{
	private decimal avgValue;

	private long count;

	public override string ServiceName => "AvgDate";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDecimal(stateName, this, "avgValue", avgValue);
		base.States.PushInt64(stateName, this, "count", count);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			avgValue = base.States.PopDecimal(stateName, this, "avgValue");
			count = base.States.PopInt64(stateName, this, "count");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			avgValue = default(decimal);
			count = 0L;
		}
	}

	public override void CalcItem(object value)
	{
		if (value is DateTime dateTime)
		{
			avgValue += (decimal)dateTime.Ticks;
			count++;
		}
	}

	public override object GetValue()
	{
		if (count == 0L)
		{
			return null;
		}
		return new DateTime((long)(avgValue / (decimal)count));
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to AvgDate function!");
	}

	public override Type GetResultType()
	{
		return typeof(DateTime);
	}

	public StiAvgDateFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiAvgDateFunctionService()
	{
	}
}
