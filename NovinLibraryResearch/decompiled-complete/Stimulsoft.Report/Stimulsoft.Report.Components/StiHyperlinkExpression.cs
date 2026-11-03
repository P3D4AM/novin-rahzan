using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiHyperlinkExpressionConverter))]
public class StiHyperlinkExpression : StiUnifiedExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetHyperlinkEvent();
	}

	public StiHyperlinkExpression()
		: this(string.Empty)
	{
	}

	public StiHyperlinkExpression(string value)
		: base(value)
	{
	}

	public StiHyperlinkExpression(StiComponent parent, string propertyName)
		: base(parent, propertyName)
	{
	}
}
