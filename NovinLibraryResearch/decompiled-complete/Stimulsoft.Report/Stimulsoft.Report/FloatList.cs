using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class FloatList : List<float>, IStiList, IEnumerable
{
	public string ListName => "FloatList";

	public Type ListType => typeof(float);

	public void AddElement(object value)
	{
		Add((float)StiConvert.ChangeType(value, typeof(float)));
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
		if (!(obj is FloatList floatList) || base.Count != floatList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], floatList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(long value)
	{
		return Contains(StiValueHelper.TryToFloat((object)value));
	}

	public bool Contains(string value)
	{
		return Contains(StiValueHelper.TryToFloat((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public FloatList()
	{
	}

	public FloatList(params float[] values)
	{
		if (values != null)
		{
			foreach (float item in values)
			{
				Add(item);
			}
		}
	}
}
