using System;

namespace Stimulsoft.Report.Dictionary;

public class StiMaxDateFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private DateTime maximum;

	public override string ServiceName => "MaxDate";

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
			maximum = (DateTime)base.States.Pop(stateName, this, "maximum");
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
		if (!(value is DateTime dateTime))
		{
			return;
		}
		if (valueProcessed)
		{
			if (maximum < dateTime)
			{
				maximum = dateTime;
			}
		}
		else
		{
			maximum = dateTime;
			valueProcessed = true;
		}
	}

	public override object GetValue()
	{
		if (valueProcessed)
		{
			return maximum;
		}
		return null;
	}

	public override void SetValue(object value)
	{
		maximum = (DateTime)value;
	}

	public override Type GetResultType()
	{
		return typeof(DateTime);
	}

	public StiMaxDateFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMaxDateFunctionService()
	{
	}
}
