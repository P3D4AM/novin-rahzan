using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMinIntFunctionService : StiAggregateFunctionService
{
	private bool valueProcessed;

	private long minimum;

	public override string ServiceName => "MinI";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushInt64(stateName, this, "minimum", minimum);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			minimum = base.States.PopInt64(stateName, this, "minimum");
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
				minimum = long.MaxValue;
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
			return 0;
		}
		return minimum;
	}

	public override void SetValue(object value)
	{
		minimum = (long)value;
	}

	public override Type GetResultType()
	{
		return typeof(long);
	}

	public StiMinIntFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMinIntFunctionService()
	{
	}
}
