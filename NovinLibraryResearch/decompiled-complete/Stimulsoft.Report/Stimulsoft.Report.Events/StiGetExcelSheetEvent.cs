using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetExcelSheetEventConverter))]
public class StiGetExcelSheetEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetExcelSheetEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetExcelSheetEventHandler);
	}

	public override string ToString()
	{
		return "GetExcelSheet";
	}

	public StiGetExcelSheetEvent()
		: this("")
	{
	}

	public StiGetExcelSheetEvent(string script)
		: base(script)
	{
	}

	public StiGetExcelSheetEvent(StiComponent parent)
		: base(parent)
	{
	}
}
