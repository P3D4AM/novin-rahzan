using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiAllowApplyBrushNegative
{
	bool AllowApplyBrushNegative { get; set; }

	StiBrush BrushNegative { get; set; }
}
