using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class ByteList : List<byte>, IStiList, IEnumerable
{
	public string ListName => "ByteList";

	public Type ListType => typeof(byte);

	public void AddElement(object value)
	{
		Add((byte)StiConvert.ChangeType(value, typeof(byte)));
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
		if (!(obj is ByteList byteList) || base.Count != byteList.Count)
		{
			return false;
		}
		for (int i = 0; i < base.Count; i++)
		{
			if (Comparer.Default.Compare(base[i], byteList[i]) != 0)
			{
				return false;
			}
		}
		return true;
	}

	public bool Contains(decimal value)
	{
		return base.Contains((byte)StiValueHelper.TryToInt((object)value));
	}

	public bool Contains(string value)
	{
		return base.Contains((byte)StiValueHelper.TryToInt((object)value));
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public ByteList()
	{
	}

	public ByteList(params byte[] values)
	{
		if (values != null)
		{
			foreach (byte item in values)
			{
				Add(item);
			}
		}
	}
}
