using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiComplexArrowGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics g = (Graphics)((context is Graphics) ? context : null);
		StiComplexArrowShapeType stiComplexArrowShapeType = (StiComplexArrowShapeType)shapeType;
		float num = ((rect.Width < rect.Height) ? (rect.Width / 2f) : (rect.Height / 2f));
		float num2 = rect.Height / 3.8f;
		float num3 = rect.Width / 3.8f;
		float num4 = ((rect.Height < rect.Width) ? (rect.Height / 2f) : (rect.Width / 2f));
		GraphicsPath val = new GraphicsPath();
		try
		{
			switch (stiComplexArrowShapeType.Direction)
			{
			case StiShapeDirection.Left:
			case StiShapeDirection.Right:
				val.AddLines(new PointF[11]
				{
					new PointF(rect.X, rect.Y + rect.Height / 2f),
					new PointF(rect.X + num, rect.Y),
					new PointF(rect.X + num, rect.Y + num2),
					new PointF(rect.Right - num, rect.Y + num2),
					new PointF(rect.Right - num, rect.Y),
					new PointF(rect.Right, rect.Y + rect.Height / 2f),
					new PointF(rect.Right - num, rect.Bottom),
					new PointF(rect.Right - num, rect.Bottom - num2),
					new PointF(rect.X + num, rect.Bottom - num2),
					new PointF(rect.X + num, rect.Bottom),
					new PointF(rect.X, rect.Y + rect.Height / 2f)
				});
				break;
			case StiShapeDirection.Up:
			case StiShapeDirection.Down:
				val.AddLines(new PointF[11]
				{
					new PointF(rect.X, rect.Y + num4),
					new PointF(rect.X + rect.Width / 2f, rect.Y),
					new PointF(rect.Right, rect.Y + num4),
					new PointF(rect.Right - num3, rect.Y + num4),
					new PointF(rect.Right - num3, rect.Bottom - num4),
					new PointF(rect.Right, rect.Bottom - num4),
					new PointF(rect.X + rect.Width / 2f, rect.Bottom),
					new PointF(rect.X, rect.Bottom - num4),
					new PointF(rect.X + num3, rect.Bottom - num4),
					new PointF(rect.X + num3, rect.Y + num4),
					new PointF(rect.X, rect.Y + num4)
				});
				break;
			}
			StiGdiShapeTypePainter.DrawShape(g, shape, val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
