using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class StringList : List<string>, IStiList, IEnumerable
{
	public string ListName => "StringList";

	public Type ListType => typeof(string);

	public void AddElement(object value)
	{
		Add((string)StiConvert.ChangeType(value, typeof(string)));
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
		if (!(obj is StringList stringList) || base.Count != stringList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], stringList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(long value)
	{
		return Contains(StiValueHelper.TryToString((object)value));
	}

	public bool Contains(double value)
	{
		return Contains(StiValueHelper.TryToString((object)value));
	}

	public bool Contains(decimal value)
	{
		return Contains(StiValueHelper.TryToString((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public StringList()
	{
	}

	public StringList(params string[] values)
	{
		if (values != null)
		{
			foreach (string item in values)
			{
				Add(item);
			}
		}
	}
}
