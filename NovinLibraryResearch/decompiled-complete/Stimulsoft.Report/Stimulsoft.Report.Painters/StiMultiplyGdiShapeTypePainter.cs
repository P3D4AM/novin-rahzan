using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiMultiplyGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics g = (Graphics)((context is Graphics) ? context : null);
		GraphicsPath val = new GraphicsPath();
		try
		{
			float num = rect.Width / 4f;
			float num2 = rect.Height / 4f;
			val.AddLines(new PointF[13]
			{
				new PointF(rect.X, rect.Y + num2),
				new PointF(rect.X + num, rect.Y),
				new PointF(rect.X + rect.Width / 2f, rect.Y + num2),
				new PointF(rect.Right - num, rect.Y),
				new PointF(rect.Right, rect.Y + num2),
				new PointF(rect.Right - num, rect.Y + rect.Height / 2f),
				new PointF(rect.Right, rect.Bottom - num2),
				new PointF(rect.Right - num, rect.Bottom),
				new PointF(rect.X + rect.Width / 2f, rect.Bottom - num2),
				new PointF(rect.X + num, rect.Bottom),
				new PointF(rect.X, rect.Bottom - num2),
				new PointF(rect.X + num, rect.Y + rect.Height / 2f),
				new PointF(rect.X, rect.Y + num2)
			});
			StiGdiShapeTypePainter.DrawShape(g, shape, val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
