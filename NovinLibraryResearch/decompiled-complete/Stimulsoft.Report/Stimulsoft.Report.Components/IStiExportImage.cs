using System.Drawing;

namespace Stimulsoft.Report.Components;

public interface IStiExportImage
{
	Image GetImage(ref float zoom);
}
