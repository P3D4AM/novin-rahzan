using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMaxIntFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private long maximum;

	public override string ServiceName => "MaxI";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushInt64(stateName, this, "maximum", maximum);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			maximum = base.States.PopInt64(stateName, this, "maximum");
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
				maximum = long.MinValue;
			}
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			long num = StiObjectConverter.ConvertToInt64(value);
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
			return 0;
		}
		return maximum;
	}

	public override void SetValue(object value)
	{
		maximum = (long)value;
	}

	public override Type GetResultType()
	{
		return typeof(long);
	}

	public StiMaxIntFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMaxIntFunctionService()
	{
	}
}
