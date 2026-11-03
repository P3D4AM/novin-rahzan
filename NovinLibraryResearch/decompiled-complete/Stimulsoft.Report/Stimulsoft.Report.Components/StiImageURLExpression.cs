using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiImageURLExpressionConverter))]
public class StiImageURLExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetImageURLEvent();
	}

	public StiImageURLExpression()
		: this(string.Empty)
	{
	}

	public StiImageURLExpression(string value)
		: base(value)
	{
	}

	public StiImageURLExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
