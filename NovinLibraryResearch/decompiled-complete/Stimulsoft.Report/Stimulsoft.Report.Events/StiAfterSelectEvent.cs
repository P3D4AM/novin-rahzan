using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[TypeConverter(typeof(StiAfterSelectEventConverter))]
public class StiAfterSelectEvent : StiEvent
{
	public override StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(TreeViewEventArgs), "e")
		};
	}

	public override Type GetEventType()
	{
		return typeof(TreeViewEventHandler);
	}

	public override string ToString()
	{
		return "AfterSelect";
	}

	public StiAfterSelectEvent()
		: this(string.Empty)
	{
	}

	public StiAfterSelectEvent(string script)
		: base(script)
	{
	}

	public StiAfterSelectEvent(StiComponent parent)
		: base(parent)
	{
	}
}
