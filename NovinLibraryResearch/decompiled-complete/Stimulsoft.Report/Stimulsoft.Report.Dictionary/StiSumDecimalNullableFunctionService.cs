using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiSumDecimalNullableFunctionService : StiAggregateFunctionService
{
	private decimal summary;

	private bool hasValues;

	public override string ServiceName => "SumNullable";

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
			hasValues = false;
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			summary += StiObjectConverter.ConvertToDecimal(value);
			hasValues = true;
		}
	}

	public override object GetValue()
	{
		if (hasValues)
		{
			return summary;
		}
		return null;
	}

	public override void SetValue(object value)
	{
		summary = (decimal)value;
		hasValues = true;
	}

	public override Type GetResultType()
	{
		return typeof(decimal?);
	}

	public StiSumDecimalNullableFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiSumDecimalNullableFunctionService()
	{
	}
}
