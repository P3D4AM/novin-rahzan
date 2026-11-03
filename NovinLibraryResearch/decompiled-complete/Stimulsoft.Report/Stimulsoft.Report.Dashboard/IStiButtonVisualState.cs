using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard;

public interface IStiButtonVisualState
{
	StiBrush Brush { get; set; }

	StiBrush TextBrush { get; set; }

	StiBrush IconBrush { get; set; }

	StiSimpleBorder Border { get; set; }

	Font Font { get; set; }

	IStiButtonElementIconSet GetIconSet();
}
