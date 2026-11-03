using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Surface;

public interface IStiAltProperties
{
	bool AltTitleVisible { get; set; }

	RectangleD AltClientRectangle { get; set; }
}
