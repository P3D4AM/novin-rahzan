using System;
using System.Collections;

namespace Stimulsoft.Report.Dictionary;

public class StiRankFunctionService : StiAggregateFunctionService
{
	private Hashtable hash = new Hashtable();

	private StiRankOrder sortOrder;

	private bool dense = true;

	public override string ServiceName => "Rank";

	public override bool RecureParam => true;

	public override void SaveState(string stateName)
	{
		base.States.Push(stateName, this, "rank", hash);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			hash = (Hashtable)base.States.Pop(stateName, this, "rank");
		}
	}

	public override void Init()
	{
		if (base.IsFirstInit)
		{
			hash.Clear();
		}
	}

	public override void CalcItem(object value)
	{
		if (value != null && value != DBNull.Value)
		{
			if (hash.ContainsKey(value))
			{
				int num = (int)hash[value];
				hash[value] = num + 1;
			}
			else
			{
				hash[value] = 1;
			}
		}
	}

	public override object GetValue()
	{
		object[] array = new object[hash.Keys.Count];
		hash.Keys.CopyTo(array, 0);
		Array.Sort(array);
		if (sortOrder == StiRankOrder.Desc)
		{
			Array.Reverse((Array)array);
		}
		int num = 1;
		object[] array2 = array;
		foreach (object key in array2)
		{
			int num2 = (int)hash[key];
			hash[key] = num;
			num += (dense ? 1 : num2);
		}
		return hash;
	}

	public override void SetValue(object value)
	{
		hash = (Hashtable)value;
	}

	public override Type GetResultType()
	{
		return typeof(Hashtable);
	}

	public StiRankFunctionService(bool runningTotal, bool dense, StiRankOrder sortOrder)
		: base(runningTotal)
	{
		this.dense = dense;
		this.sortOrder = sortOrder;
	}

	public StiRankFunctionService(bool runningTotal)
		: base(runningTotal)
	{
	}

	public StiRankFunctionService()
	{
	}
}
