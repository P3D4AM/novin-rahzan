using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetZipCodeEventConverter))]
public class StiGetZipCodeEvent : StiEvent
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
		return "GetZipCode";
	}

	public StiGetZipCodeEvent()
		: this("")
	{
	}

	public StiGetZipCodeEvent(string script)
		: base(script)
	{
	}

	public StiGetZipCodeEvent(StiComponent parent)
		: base(parent)
	{
	}
}
