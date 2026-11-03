using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiCheckedExpressionConverter))]
public class StiCheckedExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetCheckedEvent();
	}

	public StiCheckedExpression()
		: this(string.Empty)
	{
	}

	public StiCheckedExpression(string value)
		: base(value)
	{
	}

	public StiCheckedExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
