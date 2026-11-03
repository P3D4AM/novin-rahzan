using System.ComponentModel;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiArgumentExpressionConverter))]
public class StiArgumentExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetArgumentEvent();
	}

	public StiArgumentExpression()
		: this(string.Empty)
	{
	}

	public StiArgumentExpression(string value)
		: base(value)
	{
	}
}
