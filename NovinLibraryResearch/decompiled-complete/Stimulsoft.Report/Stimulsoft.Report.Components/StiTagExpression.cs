using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiTagExpressionConverter))]
public class StiTagExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetTagEvent();
	}

	public StiTagExpression()
		: this(string.Empty)
	{
	}

	public StiTagExpression(string value)
		: base(value)
	{
	}

	public StiTagExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
