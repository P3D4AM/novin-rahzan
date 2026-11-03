using System.ComponentModel;

namespace Stimulsoft.Report.Gauge;

public class StiCustomGaugeStyle : StiGaugeStyleXF27
{
	public override string ServiceName => "CustomStyle";

	[Browsable(false)]
	public StiCustomGaugeStyleCoreXF CustomCore => base.Core as StiCustomGaugeStyleCoreXF;

	public StiCustomGaugeStyle()
		: this(null)
	{
		base.Core = new StiCustomGaugeStyleCoreXF(null);
	}

	public StiCustomGaugeStyle(StiGaugeStyle style)
	{
		base.Core = new StiCustomGaugeStyleCoreXF(style);
	}
}
