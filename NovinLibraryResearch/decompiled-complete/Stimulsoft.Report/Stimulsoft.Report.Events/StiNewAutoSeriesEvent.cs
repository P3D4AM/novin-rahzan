using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiNewAutoSeriesEventConverter))]
public class StiNewAutoSeriesEvent : StiEvent
{
	public override string ToString()
	{
		return "NewAutoSeries";
	}

	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiNewAutoSeriesEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiNewAutoSeriesEventHandler);
	}

	public StiNewAutoSeriesEvent()
		: this("")
	{
	}

	public StiNewAutoSeriesEvent(string script)
		: base(script)
	{
	}

	public StiNewAutoSeriesEvent(StiComponent parent)
		: base(parent)
	{
	}
}
