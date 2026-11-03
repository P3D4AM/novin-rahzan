using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiTriangleGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Invalid comparison between Unknown and I4
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiTriangleShapeType stiTriangleShapeType = (StiTriangleShapeType)shapeType;
		Graphics val = (Graphics)context;
		GraphicsPath val2 = new GraphicsPath();
		try
		{
			val2.StartFigure();
			switch (stiTriangleShapeType.Direction)
			{
			case StiShapeDirection.Up:
				val2.AddLines(new PointF[3]
				{
					new PointF(rect.X, rect.Y + rect.Height),
					new PointF(rect.X + rect.Width, rect.Y + rect.Height),
					new PointF(rect.X + rect.Width / 2f, rect.Y)
				});
				break;
			case StiShapeDirection.Down:
				val2.AddLines(new PointF[3]
				{
					new PointF(rect.X, rect.Y),
					new PointF(rect.X + rect.Width, rect.Y),
					new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height)
				});
				break;
			case StiShapeDirection.Left:
				val2.AddLines(new PointF[3]
				{
					new PointF(rect.X + rect.Width, rect.Y),
					new PointF(rect.X + rect.Width, rect.Y + rect.Height),
					new PointF(rect.X, rect.Y + rect.Height / 2f)
				});
				break;
			case StiShapeDirection.Right:
				val2.AddLines(new PointF[3]
				{
					new PointF(rect.X, rect.Y),
					new PointF(rect.X, rect.Y + rect.Height),
					new PointF(rect.X + rect.Width, rect.Y + rect.Height / 2f)
				});
				break;
			}
			val2.CloseFigure();
			Brush brush = StiBrush.GetBrush(shape.Brush, rect);
			try
			{
				val.FillPath(brush, val2);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
			if ((int)shape.Style != 6)
			{
				Pen val3 = new Pen(shape.BorderColor, shape.Size * zoom);
				try
				{
					val3.DashStyle = StiPenUtils.GetPenStyle(shape.Style);
					val.DrawPath(val3, val2);
					return;
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
