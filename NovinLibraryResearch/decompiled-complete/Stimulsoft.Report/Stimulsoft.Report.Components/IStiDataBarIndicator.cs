using System.Drawing;

namespace Stimulsoft.Report.Components;

public interface IStiDataBarIndicator
{
	StiBrushType BrushType { get; set; }

	Color PositiveColor { get; set; }

	Color NegativeColor { get; set; }

	Color PositiveBorderColor { get; set; }

	Color NegativeBorderColor { get; set; }

	bool ShowBorder { get; set; }

	StiDataBarDirection Direction { get; set; }
}
