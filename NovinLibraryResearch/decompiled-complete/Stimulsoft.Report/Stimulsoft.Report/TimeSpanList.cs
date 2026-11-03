using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class TimeSpanList : List<TimeSpan>, IStiList, IEnumerable
{
	public string ListName => "TimeSpanList";

	public Type ListType => typeof(TimeSpan);

	public bool Contains(TimeSpan? value)
	{
		return base.Contains(value.GetValueOrDefault());
	}

	public void AddElement(object value)
	{
		Add((TimeSpan)StiConvert.ChangeType(value, typeof(TimeSpan)));
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
		if (!(obj is TimeSpanList timeSpanList) || base.Count != timeSpanList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], timeSpanList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(string value)
	{
		return base.Contains(StiValueHelper.TryToTimeSpan((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public TimeSpanList()
	{
	}

	public TimeSpanList(params TimeSpan[] values)
	{
		if (values != null)
		{
			foreach (TimeSpan item in values)
			{
				Add(item);
			}
		}
	}
}
