using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.CrossTab.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.CrossTab;

[TypeConverter(typeof(StiGetDisplayCrossValueEventConverter))]
public class StiGetDisplayCrossValueEvent : StiEvent
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
		return "GetDisplayCrossValue";
	}

	public StiGetDisplayCrossValueEvent()
		: this("")
	{
	}

	public StiGetDisplayCrossValueEvent(string script)
		: base(script)
	{
	}
}
