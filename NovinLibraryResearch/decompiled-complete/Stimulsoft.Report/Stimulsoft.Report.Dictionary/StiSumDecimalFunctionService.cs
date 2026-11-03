using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSumDecimalFunctionService : StiAggregateFunctionService
{
	private decimal summary;

	public override string ServiceName => "Sum";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushDecimal(stateName, this, "summary", summary);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			summary = base.States.PopDecimal(stateName, this, "summary");
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			summary = default(decimal);
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			summary += StiObjectConverter.ConvertToDecimal(value);
		}
	}

	public override object GetValue()
	{
		return summary;
	}

	public override void SetValue(object value)
	{
		summary = (decimal)value;
	}

	public override Type GetResultType()
	{
		return typeof(decimal);
	}

	public StiSumDecimalFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiSumDecimalFunctionService()
	{
	}
}
