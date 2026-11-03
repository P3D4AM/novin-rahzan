using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class IntList : List<int>, IStiList, IEnumerable
{
	public string ListName => "IntList";

	public Type ListType => typeof(int);

	public void AddElement(object value)
	{
		Add((int)StiConvert.ChangeType(value, typeof(int)));
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
		if (!(obj is IntList intList) || base.Count != intList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], intList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(decimal value)
	{
		return base.Contains(StiValueHelper.TryToInt((object)value));
	}

	public bool Contains(string value)
	{
		return base.Contains(StiValueHelper.TryToInt((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public IntList()
	{
	}

	public IntList(params int[] values)
	{
		if (values != null)
		{
			foreach (int item in values)
			{
				Add(item);
			}
		}
	}
}
