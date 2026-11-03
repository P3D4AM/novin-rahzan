using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetListOfHyperlinksEventConverter))]
public class StiGetListOfHyperlinksEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(StiGetValueEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(StiGetValueEventHandler);
	}

	public override string ToString()
	{
		return "GetListOfHyperlinks";
	}

	public StiGetListOfHyperlinksEvent()
		: this("")
	{
	}

	public StiGetListOfHyperlinksEvent(string script)
		: base(script)
	{
	}

	public StiGetListOfHyperlinksEvent(StiComponent parent)
		: base(parent)
	{
	}
}
