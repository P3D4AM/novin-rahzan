using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class DecimalList : List<decimal>, IStiList, IEnumerable
{
	public string ListName => "DecimalList";

	public Type ListType => typeof(decimal);

	public void AddElement(object value)
	{
		Add((decimal)StiConvert.ChangeType(value, typeof(decimal)));
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
		if (!(obj is DecimalList decimalList) || base.Count != decimalList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], decimalList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(long value)
	{
		return Contains(StiValueHelper.TryToDecimal((object)value));
	}

	public bool Contains(string value)
	{
		return Contains(StiValueHelper.TryToDecimal((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public DecimalList()
	{
	}

	public DecimalList(params decimal[] values)
	{
		if (values != null)
		{
			foreach (decimal item in values)
			{
				Add(item);
			}
		}
	}
}
