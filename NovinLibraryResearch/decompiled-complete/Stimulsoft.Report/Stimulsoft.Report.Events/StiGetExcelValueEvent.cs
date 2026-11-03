using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetExcelValueEventConverter))]
public class StiGetExcelValueEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetExcelValueEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetExcelValueEventHandler);
	}

	public override string ToString()
	{
		return "GetExcelValue";
	}

	public StiGetExcelValueEvent()
		: this("")
	{
	}

	public StiGetExcelValueEvent(string script)
		: base(script)
	{
	}

	public StiGetExcelValueEvent(StiComponent parent)
		: base(parent)
	{
	}
}
