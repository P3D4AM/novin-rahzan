using System;
using System.ComponentModel;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiGetListOfValuesOpenEventConverter))]
public class StiGetListOfValuesOpenEvent : StiEvent
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
		return "GetListOfValuesOpen";
	}

	public StiGetListOfValuesOpenEvent()
		: this("")
	{
	}

	public StiGetListOfValuesOpenEvent(string script)
		: base(script)
	{
	}

	public StiGetListOfValuesOpenEvent(StiComponent parent)
		: base(parent)
	{
	}
}
