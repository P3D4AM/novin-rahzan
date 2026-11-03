using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiListOfToolTipsExpressionConverter))]
public class StiListOfToolTipsExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetListOfToolTipsEvent();
	}

	public StiListOfToolTipsExpression()
		: this(string.Empty)
	{
	}

	public StiListOfToolTipsExpression(string value)
		: base(value)
	{
	}
}
