using System;
using System.Collections;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMedianDecimalFunctionService : StiAggregateFunctionService
{
	private ArrayList values;

	public override string ServiceName => "Median";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		if (values != null)
		{
			base.States.Push(stateName, this, "values", values.Clone());
		}
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			values = base.States.Pop(stateName, this, "values") as ArrayList;
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			values = new ArrayList();
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			decimal num = StiObjectConverter.ConvertToDecimal(value);
			values.Add(num);
		}
	}

	public override object GetValue()
	{
		if (values == null || values.Count < 1)
		{
			return null;
		}
		if (values.Count == 1)
		{
			return (decimal)values[0];
		}
		values.Sort();
		int num = values.Count / 2;
		if (values.Count % 2 == 1)
		{
			return (decimal)values[num];
		}
		decimal num2 = (decimal)values[num - 1];
		decimal num3 = (decimal)values[num];
		return (num2 + num3) / 2m;
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to Median function!");
	}

	public override Type GetResultType()
	{
		return typeof(decimal);
	}

	public StiMedianDecimalFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMedianDecimalFunctionService()
	{
	}
}
