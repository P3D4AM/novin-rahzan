using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiCollapsedExpressionConverter))]
public class StiCollapsedExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetCollapsedEvent();
	}

	public StiCollapsedExpression()
		: this(string.Empty)
	{
	}

	public StiCollapsedExpression(string value)
		: base(value)
	{
	}

	public StiCollapsedExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
