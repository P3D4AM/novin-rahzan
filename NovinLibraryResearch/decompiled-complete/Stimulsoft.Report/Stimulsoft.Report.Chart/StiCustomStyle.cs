using System.ComponentModel;

namespace Stimulsoft.Report.Chart;

public class StiCustomStyle : StiStyle01
{
	public override string ServiceName => "CustomStyle";

	[Browsable(false)]
	public StiCustomStyleCoreXF CustomCore => base.Core as StiCustomStyleCoreXF;

	public StiCustomStyle()
		: this(null)
	{
		base.Core = new StiCustomStyleCoreXF(this);
	}

	public StiCustomStyle(string reportStyleName)
	{
		Name = reportStyleName;
		base.Core = new StiCustomStyleCoreXF(this);
		((StiCustomStyleCoreXF)base.Core).ReportStyleName = reportStyleName;
	}
}
