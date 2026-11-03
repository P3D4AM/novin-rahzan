using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiGroupSummaryExpressionConverter))]
public class StiGroupSummaryExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetSummaryExpressionEvent();
	}

	public StiGroupSummaryExpression()
		: this(string.Empty)
	{
	}

	public StiGroupSummaryExpression(string value)
		: base(value)
	{
	}

	public StiGroupSummaryExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
