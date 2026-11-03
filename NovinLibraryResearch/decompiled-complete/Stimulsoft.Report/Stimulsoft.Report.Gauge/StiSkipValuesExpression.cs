using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Design;
using Stimulsoft.Report.Gauge.Events;

namespace Stimulsoft.Report.Gauge;

[TypeConverter(typeof(StiSkipValuesExpressionConverter))]
public class StiSkipValuesExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetSkipValuesEvent();
	}

	public StiSkipValuesExpression()
		: this(string.Empty)
	{
	}

	public StiSkipValuesExpression(string value)
		: base(value)
	{
	}
}
