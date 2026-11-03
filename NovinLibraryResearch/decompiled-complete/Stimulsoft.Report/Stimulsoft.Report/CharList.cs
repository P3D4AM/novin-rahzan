using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report;

public class CharList : List<char>, IStiList, IEnumerable
{
	public string ListName => "CharList";

	public Type ListType => typeof(char);

	public void AddElement(object value)
	{
		Add((char)StiConvert.ChangeType(value, typeof(char)));
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
		if (!(obj is CharList charList) || base.Count != charList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], charList[i]) != 0)
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

	public CharList()
	{
	}

	public CharList(params char[] values)
	{
		if (values != null)
		{
			foreach (char item in values)
			{
				Add(item);
			}
		}
	}
}
