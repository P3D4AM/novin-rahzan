using System;

namespace Stimulsoft.Report.Dictionary;

public class StiMinTimeFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private TimeSpan minimum;

	public override string ServiceName => "MinTime";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.Push(stateName, this, "minimum", minimum);
		base.States.PushBool(stateName, this, "valueProcessed", valueProcessed);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			minimum = (TimeSpan)base.States.Pop(stateName, this, "minimum");
			valueProcessed = base.States.PopBool(stateName, this, "valueProcessed");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			valueProcessed = false;
		}
	}

	public override void CalcItem(object value)
	{
		if (!(value is TimeSpan timeSpan))
		{
			return;
		}
		if (valueProcessed)
		{
			if (minimum > timeSpan)
			{
				minimum = timeSpan;
			}
		}
		else
		{
			minimum = timeSpan;
			valueProcessed = true;
		}
	}

	public override object GetValue()
	{
		return minimum;
	}

	public override void SetValue(object value)
	{
		minimum = (TimeSpan)value;
	}

	public override Type GetResultType()
	{
		return typeof(TimeSpan);
	}

	public StiMinTimeFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMinTimeFunctionService()
	{
	}
}
