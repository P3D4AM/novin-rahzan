using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMinDoubleFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private double minimum;

	public override string ServiceName => "MinD";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDouble(stateName, this, "minimum", minimum);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			minimum = base.States.PopDouble(stateName, this, "minimum");
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
				minimum = double.MaxValue;
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
				minimum = num;
				valueProcessed = true;
			}
			else if (minimum > num)
			{
				minimum = num;
			}
		}
	}

	public override object GetValue()
	{
		if (StiOptions.Engine.AllowZeroResultForMinMaxAggregateFunctions && !valueProcessed)
		{
			return 0.0;
		}
		return minimum;
	}

	public override void SetValue(object value)
	{
		minimum = (double)value;
	}

	public override Type GetResultType()
	{
		return typeof(double);
	}

	public StiMinDoubleFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMinDoubleFunctionService()
	{
	}
}
