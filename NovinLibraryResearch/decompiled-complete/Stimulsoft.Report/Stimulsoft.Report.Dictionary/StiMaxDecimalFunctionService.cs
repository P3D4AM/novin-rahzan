using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMaxDecimalFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private decimal maximum;

	public override string ServiceName => "Max";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDecimal(stateName, this, "maximum", maximum);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			maximum = base.States.PopDecimal(stateName, this, "maximum");
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
				maximum = decimal.MinValue;
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
			return 0m;
		}
		return maximum;
	}

	public override void SetValue(object value)
	{
		maximum = (decimal)value;
	}

	public override Type GetResultType()
	{
		return typeof(decimal);
	}

	public StiMaxDecimalFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMaxDecimalFunctionService()
	{
	}
}
