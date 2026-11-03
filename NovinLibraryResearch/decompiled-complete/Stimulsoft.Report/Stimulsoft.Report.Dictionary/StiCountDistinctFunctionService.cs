using System;
using System.Collections;

namespace Stimulsoft.Report.Dictionary;

public class StiCountDistinctFunctionService : StiAggregateFunctionService
{
	private long counter;

	private Hashtable values;

	public override string ServiceName => "CountDistinct";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.PushInt64(stateName, this, "counter", counter);
		if (values != null)
		{
			base.States.Push(stateName, this, "values", values.Clone());
		}
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			counter = base.States.PopInt64(stateName, this, "counter");
			values = base.States.Pop(stateName, this, "values") as Hashtable;
		}
	}

	public override void Init()
	{
		if (!base.RunningTotal || base.IsFirstInit)
		{
			counter = 0L;
			values = new Hashtable();
		}
	}

	public override void CalcItem(object value)
	{
		if (values == null)
		{
			values = new Hashtable();
		}
		if (values[value] == null)
		{
			values.Add(value, value);
			counter++;
		}
	}

	public override object GetValue()
	{
		return counter;
	}

	public override void SetValue(object value)
	{
		counter = (long)value;
	}

	public override Type GetResultType()
	{
		return typeof(long);
	}

	public StiCountDistinctFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiCountDistinctFunctionService()
	{
	}
}
