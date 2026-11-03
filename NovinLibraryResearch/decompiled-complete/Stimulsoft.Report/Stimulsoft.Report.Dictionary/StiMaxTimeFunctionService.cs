using System;

namespace Stimulsoft.Report.Dictionary;

public class StiMaxTimeFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private TimeSpan maximum;

	public override string ServiceName => "MaxTime";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.Push(stateName, this, "maximum", maximum);
		base.States.PushBool(stateName, this, "valueProcessed", valueProcessed);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			maximum = (TimeSpan)base.States.Pop(stateName, this, "maximum");
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
			if (maximum < timeSpan)
			{
				maximum = timeSpan;
			}
		}
		else
		{
			maximum = timeSpan;
			valueProcessed = true;
		}
	}

	public override object GetValue()
	{
		return maximum;
	}

	public override void SetValue(object value)
	{
		maximum = (TimeSpan)value;
	}

	public override Type GetResultType()
	{
		return typeof(TimeSpan);
	}

	public StiMaxTimeFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMaxTimeFunctionService()
	{
	}
}
