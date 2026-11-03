using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMinDecimalFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private decimal minimum;

	public override string ServiceName => "Min";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDecimal(stateName, this, "minimum", minimum);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			minimum = base.States.PopDecimal(stateName, this, "minimum");
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
				minimum = decimal.MaxValue;
			}
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			decimal num = StiObjectConverter.ConvertToDecimal(value);
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
			return 0m;
		}
		return minimum;
	}

	public override void SetValue(object value)
	{
		minimum = (decimal)value;
	}

	public override Type GetResultType()
	{
		return typeof(decimal);
	}

	public StiMinDecimalFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMinDecimalFunctionService()
	{
	}
}
