using System.ComponentModel;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Design;
using Stimulsoft.Report.Gauge.Events;

namespace Stimulsoft.Report.Gauge;

[TypeConverter(typeof(StiSkipIndicesExpressionConverter))]
public class StiSkipIndicesExpression : StiExpression
{
	[Browsable(false)]
	public override bool ApplyFormat => false;

	public override bool FullConvert => true;

	public override StiEvent GetDefaultEvent()
	{
		return new StiGetSkipIndicesEvent();
	}

	public StiSkipIndicesExpression()
		: this(string.Empty)
	{
	}

	public StiSkipIndicesExpression(string value)
		: base(value)
	{
	}
}
