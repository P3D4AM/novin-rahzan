using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetTagEventConverter))]
public class StiGetTagEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiValueEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiValueEventHandler);
	}

	public override string ToString()
	{
		return "GetTag";
	}

	public StiGetTagEvent()
		: this("")
	{
	}

	public StiGetTagEvent(string script)
		: base(script)
	{
	}

	public StiGetTagEvent(StiComponent parent)
		: base(parent)
	{
	}
}
