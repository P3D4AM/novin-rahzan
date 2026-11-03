using System;

namespace Stimulsoft.Report.Dictionary;

public class StiLastFunctionService : StiAggregateFunctionService
{
	private object value;

	public override string ServiceName => "Last";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.Push(stateName, this, "value", value);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			value = base.States.Pop(stateName, this, "value");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			value = null;
		}
	}

	public override void CalcItem(object value)
	{
		this.value = value;
	}

	public override object GetValue()
	{
		return value;
	}

	public override void SetValue(object value)
	{
		this.value = value;
	}

	public override Type GetResultType()
	{
		return typeof(object);
	}

	public StiLastFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiLastFunctionService()
	{
	}
}
