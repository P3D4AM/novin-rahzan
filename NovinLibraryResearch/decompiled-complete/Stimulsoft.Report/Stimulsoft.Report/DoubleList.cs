using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class DoubleList : List<double>, IStiList, IEnumerable
{
	public string ListName => "DoubleList";

	public Type ListType => typeof(double);

	public void AddElement(object value)
	{
		Add((double)StiConvert.ChangeType(value, typeof(double)));
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
		if (!(obj is DoubleList doubleList) || base.Count != doubleList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], doubleList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(long value)
	{
		return Contains(StiValueHelper.TryToDouble((object)value));
	}

	public bool Contains(string value)
	{
		return Contains(StiValueHelper.TryToDouble((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public DoubleList()
	{
	}

	public DoubleList(params double[] values)
	{
		if (values != null)
		{
			foreach (double item in values)
			{
				Add(item);
			}
		}
	}
}
