using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public interface IShapeDescriptor
{
	GraphicsPath GetShape(Rectangle bounds);

	GraphicsPath GetInnerShape(Rectangle bounds, int borderSize);

	bool CanDrawShape(Rectangle bounds);
}
