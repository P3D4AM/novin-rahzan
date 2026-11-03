using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge;

public interface IStiGaugeMarker
{
	bool ShowValue { get; set; }

	StiBrush TextBrush { get; set; }

	Font Font { get; set; }

	string Format { get; set; }
}
