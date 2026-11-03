using System;

namespace Stimulsoft.Report.Dictionary;

public class StiCountFunctionService : StiAggregateFunctionService
{
	private long counter;

	public override string ServiceName => "Count";

	public override bool RecureParam => false;

	public override void SaveState(string stateName)
	{
		base.States.PushInt64(stateName, this, "counter", counter);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			counter = base.States.PopInt64(stateName, this, "counter");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			counter = 0L;
		}
	}

	public override void CalcItem(object value)
	{
		counter++;
	}

	public override object GetValue()
	{
		return counter;
	}

	public override void SetValue(object value)
	{
		counter = (long)value;
	}

	public override Type GetResultType()
	{
		return typeof(long);
	}

	public StiCountFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiCountFunctionService()
	{
	}
}
