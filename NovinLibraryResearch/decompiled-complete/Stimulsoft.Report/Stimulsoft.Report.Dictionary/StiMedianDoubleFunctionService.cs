using System;
using System.Collections;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiMedianDoubleFunctionService : StiAggregateFunctionService
{
	private ArrayList values;

	public override string ServiceName => "MedianD";

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
			double num = StiObjectConverter.ConvertToDouble(value);
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
			return (double)values[0];
		}
		values.Sort();
		int num = values.Count / 2;
		if (values.Count % 2 == 1)
		{
			return (double)values[num];
		}
		double num2 = (double)values[num - 1];
		double num3 = (double)values[num];
		return (num2 + num3) / 2.0;
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to MedianD function!");
	}

	public override Type GetResultType()
	{
		return typeof(double);
	}

	public StiMedianDoubleFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMedianDoubleFunctionService()
	{
	}
}
