using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiGroupConditionExpressionConverter))]
public class StiGroupConditionExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetGroupConditionEvent();
	}

	public StiGroupConditionExpression()
		: this(string.Empty)
	{
	}

	public StiGroupConditionExpression(string value)
		: base(value)
	{
	}

	public StiGroupConditionExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
