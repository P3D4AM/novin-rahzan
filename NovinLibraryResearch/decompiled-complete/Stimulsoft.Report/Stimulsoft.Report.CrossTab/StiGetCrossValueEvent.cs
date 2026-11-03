using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.CrossTab.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CrossTab;

[TypeConverter(typeof(StiGetCrossValueEventConverter))]
public class StiGetCrossValueEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetCrossValueEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetCrossValueEventHandler);
	}

	public override string ToString()
	{
		return "GetCrossValue";
	}

	public StiGetCrossValueEvent()
		: this("")
	{
	}

	public StiGetCrossValueEvent(string script)
		: base(script)
	{
	}
}
