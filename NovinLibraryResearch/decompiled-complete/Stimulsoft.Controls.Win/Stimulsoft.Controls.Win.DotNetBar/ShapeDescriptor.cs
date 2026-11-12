using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

public abstract class ShapeDescriptor : IShapeDescriptor
{
	public abstract GraphicsPath GetShape(Rectangle bounds);

	public abstract GraphicsPath GetInnerShape(Rectangle bounds, int borderSize);

	public abstract bool CanDrawShape(Rectangle bounds);
}
