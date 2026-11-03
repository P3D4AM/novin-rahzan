using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiPointerExpressionConverter))]
public class StiPointerExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetPointerEvent();
	}

	public StiPointerExpression()
		: this(string.Empty)
	{
	}

	public StiPointerExpression(string val)
		: base(val)
	{
	}

	public StiPointerExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
