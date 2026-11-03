using System;

namespace Stimulsoft.Report.Dictionary;

public class StiMinDateFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private DateTime minimum;

	public override string ServiceName => "MinDate";

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
			minimum = (DateTime)base.States.Pop(stateName, this, "minimum");
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
			if (minimum > dateTime)
			{
				minimum = dateTime;
			}
		}
		else
		{
			minimum = dateTime;
			valueProcessed = true;
		}
	}

	public override object GetValue()
	{
		if (valueProcessed)
		{
			return minimum;
		}
		return null;
	}

	public override void SetValue(object value)
	{
		minimum = (DateTime)value;
	}

	public override Type GetResultType()
	{
		return typeof(DateTime);
	}

	public StiMinDateFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMinDateFunctionService()
	{
	}
}
