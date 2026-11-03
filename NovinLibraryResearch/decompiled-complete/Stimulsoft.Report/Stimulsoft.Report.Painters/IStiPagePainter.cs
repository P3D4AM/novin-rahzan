using System.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public interface IStiPagePainter
{
	Bitmap GetWatermarkImage(StiPage page, double zoom, bool useMargins);
}
