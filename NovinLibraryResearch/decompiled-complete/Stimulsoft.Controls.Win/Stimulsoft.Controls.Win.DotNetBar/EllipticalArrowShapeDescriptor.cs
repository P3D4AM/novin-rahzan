using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(EllipticalShapeDescriptorConverter))]
public class EllipticalArrowShapeDescriptor : ShapeDescriptor
{
	public override GraphicsPath GetShape(Rectangle bounds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddEllipse(bounds);
		int num = bounds.Width / 4;
		int num2 = bounds.Width / 2;
		int num3 = bounds.Height / 2;
		int num4 = bounds.Height / 3;
		Point point = new Point(bounds.X + num2, bounds.Y + num4 + 1);
		Point point2 = new Point(bounds.X + num + 1, bounds.Y + num3);
		Point point3 = new Point(bounds.X + num2, bounds.Y + 2 * num4);
		Point point4 = new Point(bounds.X + 3 * num, bounds.Y + num3);
		val.AddLine(point, point2);
		val.CloseFigure();
		val.AddLine(point4, point2);
		val.CloseFigure();
		val.AddLine(point2, point3);
		val.CloseFigure();
		return val;
	}

	public override GraphicsPath GetInnerShape(Rectangle bounds, int borderSize)
	{
		return GetShape(bounds);
	}

	public override bool CanDrawShape(Rectangle bounds)
	{
		if (bounds.Width > 2)
		{
			return bounds.Height > 2;
		}
		return false;
	}
}
