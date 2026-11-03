using System.Drawing;

namespace Stimulsoft.Report.Components;

public interface IStiExportImageExtended : IStiExportImage
{
	Image GetImage(ref float zoom, StiExportFormat format);

	bool IsExportAsImage(StiExportFormat format);
}
