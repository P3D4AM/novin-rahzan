using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetListOfArgumentsEventConverter))]
public class StiGetListOfArgumentsEvent : StiEvent
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
		return "GetListOfArguments";
	}

	public StiGetListOfArgumentsEvent()
		: this("")
	{
	}

	public StiGetListOfArgumentsEvent(string script)
		: base(script)
	{
	}

	public StiGetListOfArgumentsEvent(StiComponent parent)
		: base(parent)
	{
	}
}
