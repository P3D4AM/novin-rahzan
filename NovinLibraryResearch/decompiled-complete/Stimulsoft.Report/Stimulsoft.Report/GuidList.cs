using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report;

public class GuidList : List<Guid>, IStiList, IEnumerable
{
	public string ListName => "GuidList";

	public Type ListType => typeof(Guid);

	public void AddElement(object value)
	{
		Add((Guid)StiConvert.ChangeType(value, typeof(Guid)));
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
		if (!(obj is GuidList guidList) || base.Count != guidList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], guidList[i]) != 0)
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

	public GuidList()
	{
	}

	public GuidList(params Guid[] values)
	{
		if (values != null)
		{
			foreach (Guid item in values)
			{
				Add(item);
			}
		}
	}
}
