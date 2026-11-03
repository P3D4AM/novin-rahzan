using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiTitleExpressionConverter))]
public class StiTitleExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetTitleEvent();
	}

	public StiTitleExpression()
		: this(string.Empty)
	{
	}

	public StiTitleExpression(string value)
		: base(value)
	{
	}
}
