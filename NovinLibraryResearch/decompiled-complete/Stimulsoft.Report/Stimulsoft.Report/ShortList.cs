using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class ShortList : List<short>, IStiList, IEnumerable
{
	public string ListName => "ShortList";

	public Type ListType => typeof(short);

	public void AddElement(object value)
	{
		Add((short)StiConvert.ChangeType(value, typeof(short)));
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
		if (!(obj is ShortList shortList) || base.Count != shortList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], shortList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(decimal value)
	{
		return base.Contains((short)StiValueHelper.TryToLong((object)value));
	}

	public bool Contains(string value)
	{
		return base.Contains((short)StiValueHelper.TryToLong((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public ShortList()
	{
	}

	public ShortList(params short[] values)
	{
		if (values != null)
		{
			foreach (short item in values)
			{
				Add(item);
			}
		}
	}
}
