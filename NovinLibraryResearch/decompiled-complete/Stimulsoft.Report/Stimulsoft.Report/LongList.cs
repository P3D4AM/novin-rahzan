using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class LongList : List<long>, IStiList, IEnumerable
{
	public string ListName => "LongList";

	public Type ListType => typeof(long);

	public void AddElement(object value)
	{
		Add((long)StiConvert.ChangeType(value, typeof(long)));
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
		if (!(obj is LongList longList) || base.Count != longList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], longList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(decimal value)
	{
		return base.Contains(StiValueHelper.TryToLong((object)value));
	}

	public bool Contains(string value)
	{
		return base.Contains(StiValueHelper.TryToLong((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public LongList()
	{
	}

	public LongList(params long[] values)
	{
		if (values != null)
		{
			foreach (long item in values)
			{
				Add(item);
			}
		}
	}
}
