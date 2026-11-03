using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Design;
using Stimulsoft.Report.Gauge.Events;

namespace Stimulsoft.Report.Gauge;

[TypeConverter(typeof(StiTextExpressionConverter))]
public class StiTextExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetTextEvent();
	}

	public StiTextExpression()
		: this(string.Empty)
	{
	}

	public StiTextExpression(string value)
		: base(value)
	{
	}
}
