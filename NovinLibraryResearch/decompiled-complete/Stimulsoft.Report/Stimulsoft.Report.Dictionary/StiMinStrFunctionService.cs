using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.Dictionary;

public class StiMinStrFunctionService : StiAggregateFunctionService
{
	private List<string> values;

	public static Comparison<string> AscComparison = (string str1, string str2) => str1.CompareTo(str2);

	public override string ServiceName => "MinStr";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		List<string> list = new List<string>();
		foreach (string value in values)
		{
			list.Add(value);
		}
		base.States.Push(stateName, this, "values", list);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			values = base.States.Pop(stateName, this, "values") as List<string>;
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			values = new List<string>();
		}
	}

	public override void CalcItem(object value)
	{
		string item = ((value != null) ? value.ToString() : string.Empty);
		values.Add(item);
	}

	public override object GetValue()
	{
		if (values == null || values.Count == 0)
		{
			return string.Empty;
		}
		values.Sort(AscComparison);
		return values[0];
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to MinStr function!");
	}

	public override Type GetResultType()
	{
		return typeof(string);
	}

	public StiMinStrFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiMinStrFunctionService()
	{
	}
}
