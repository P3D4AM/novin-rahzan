using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMaxDoubleFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private double maximum;

	public override string ServiceName => "MaxD";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDouble(stateName, this, "maximum", maximum);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			maximum = base.States.PopDouble(stateName, this, "maximum");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			if (StiOptions.Engine.AllowZeroResultForMinMaxAggregateFunctions)
			{
				valueProcessed = false;
			}
			else
			{
				maximum = double.MinValue;
			}
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			double num = StiObjectConverter.ConvertToDouble(value);
			if (StiOptions.Engine.AllowZeroResultForMinMaxAggregateFunctions && !valueProcessed)
			{
				maximum = num;
				valueProcessed = true;
			}
			else if (maximum < num)
			{
				maximum = num;
			}
		}
	}

	public override object GetValue()
	{
		if (StiOptions.Engine.AllowZeroResultForMinMaxAggregateFunctions && !valueProcessed)
		{
			return 0.0;
		}
		return maximum;
	}

	public override void SetValue(object value)
	{
		maximum = (double)value;
	}

	public override Type GetResultType()
	{
		return typeof(double);
	}

	public StiMaxDoubleFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMaxDoubleFunctionService()
	{
	}
}
