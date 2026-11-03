using System;
using System.Collections;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public class StiModeIntFunctionService : StiAggregateFunctionService
{
	private Hashtable values;

	public override string ServiceName => "ModeI";

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
			values = base.States.Pop(stateName, this, "values") as Hashtable;
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			values = new Hashtable();
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			long num = StiObjectConverter.ConvertToInt64(value);
			object obj = values[num] ?? ((object)0);
			obj = (int)obj + 1;
			values[num] = obj;
		}
	}

	public override object GetValue()
	{
		if (values == null || values.Count == 0)
		{
			return null;
		}
		long num = 0L;
		int num2 = 0;
		foreach (object key in values.Keys)
		{
			int num3 = (int)values[key];
			if (num3 > num2)
			{
				num2 = num3;
				num = (long)key;
			}
		}
		return num;
	}

	public override void SetValue(object value)
	{
		throw new ArgumentException("You can't set calculation result to ModeI function!");
	}

	public override Type GetResultType()
	{
		return typeof(long);
	}

	public StiModeIntFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiModeIntFunctionService()
	{
	}
}
