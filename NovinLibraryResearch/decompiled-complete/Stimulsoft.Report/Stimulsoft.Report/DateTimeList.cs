using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class DateTimeList : List<DateTime>, IStiList, IEnumerable
{
	public string ListName => "DateTimeList";

	public Type ListType => typeof(DateTime);

	public bool Contains(DateTime? value)
	{
		return base.Contains(value.GetValueOrDefault());
	}

	public void AddElement(object value)
	{
		Add((DateTime)StiConvert.ChangeType(value, typeof(DateTime)));
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

	public string ToQueryString(string quotationMark, string dateTimeFormat)
	{
		return Func.EngineHelper.ToQueryString(this, quotationMark, dateTimeFormat);
	}

	public string ToQueryString(string quotationMark, bool needEscape)
	{
		return Func.EngineHelper.ToQueryString(this, quotationMark, null, needEscape);
	}

	public string ToQueryString(string quotationMark, string dateTimeFormat, bool needEscape)
	{
		return Func.EngineHelper.ToQueryString(this, quotationMark, dateTimeFormat, needEscape);
	}

	public object[] ToObjectArray()
	{
		return this.Cast<object>().ToArray();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is DateTimeList dateTimeList) || base.Count != dateTimeList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], dateTimeList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(string value)
	{
		return base.Contains(StiValueHelper.TryToDateTime((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public DateTimeList()
	{
	}

	public DateTimeList(params DateTime[] values)
	{
		if (values != null)
		{
			foreach (DateTime item in values)
			{
				Add(item);
			}
		}
	}
}
