using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiToolTipExpressionConverter))]
public class StiToolTipExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetToolTipEvent();
	}

	public StiToolTipExpression()
		: this(string.Empty)
	{
	}

	public StiToolTipExpression(string value)
		: base(value)
	{
	}

	public StiToolTipExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
