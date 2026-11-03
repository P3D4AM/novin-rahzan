using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Design;
using Stimulsoft.Report.Gauge.Events;

namespace Stimulsoft.Report.Gauge;

[TypeConverter(typeof(StiValueExpressionConverter))]
public class StiValueExpression : StiExpression
{
	public override bool FullConvert => false;

	public override StiEvent GetDefaultEvent()
	{
		return new Stimulsoft.Report.Gauge.Events.StiGetValueEvent();
	}

	public StiValueExpression()
		: this(string.Empty)
	{
	}

	public StiValueExpression(string value)
		: base(value)
	{
	}
}
