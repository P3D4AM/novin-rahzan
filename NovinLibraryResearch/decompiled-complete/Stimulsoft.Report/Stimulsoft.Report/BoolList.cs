using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report;

public class BoolList : List<bool>, IStiList, IEnumerable
{
	public string ListName => "BoolList";

	public Type ListType => typeof(bool);

	public void AddElement(object value)
	{
		Add((bool)StiConvert.ChangeType(value, typeof(bool)));
	}

	public override string ToString()
	{
		return ToQueryString();
	}

	public string ToQueryString()
	{
		return Func.EngineHelper.ToQueryString(this, null, null);
	}

	public string ToQueryString(string quotationMark)
	{
		return Func.EngineHelper.ToQueryString(this, quotationMark, null);
	}

	public string ToQueryString(string quotationMark, bool needEscape)
	{
		return Func.EngineHelper.ToQueryString(this, quotationMark, null, needEscape);
	}

	public object[] ToObjectArray()
	{
		return this.Cast<object>().ToArray();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is BoolList boolList) || base.Count != boolList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], boolList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public BoolList()
	{
	}

	public BoolList(params bool[] values)
	{
		if (values != null)
		{
			foreach (bool item in values)
			{
				Add(item);
			}
		}
	}
}
