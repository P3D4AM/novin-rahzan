using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiFilterExpressionConverter))]
public class StiFilterExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool GenAddEvent => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetFilterEvent();
	}

	public StiFilterExpression()
		: this(string.Empty)
	{
	}

	public StiFilterExpression(string value)
		: base(value)
	{
	}

	public StiFilterExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
