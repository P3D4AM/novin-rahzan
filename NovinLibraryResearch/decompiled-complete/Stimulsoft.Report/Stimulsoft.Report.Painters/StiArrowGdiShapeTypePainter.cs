using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiArrowGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Invalid comparison between Unknown and I4
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)context;
		StiArrowShapeType stiArrowShapeType = (StiArrowShapeType)shapeType;
		GraphicsPath val2 = new GraphicsPath();
		try
		{
			val2.StartFigure();
			if (stiArrowShapeType.Direction == StiShapeDirection.Up || stiArrowShapeType.Direction == StiShapeDirection.Down)
			{
				float num = rect.Width * stiArrowShapeType.ArrowWidth;
				float num2 = ((stiArrowShapeType.ArrowHeight != 0f) ? (rect.Height * stiArrowShapeType.ArrowHeight) : Math.Min(rect.Width / 2f, rect.Height / 2f));
				if (stiArrowShapeType.Direction == StiShapeDirection.Up)
				{
					val2.AddLines(new PointF[7]
					{
						new PointF(rect.X + num, rect.Y + rect.Height),
						new PointF(rect.X + rect.Width - num, rect.Y + rect.Height),
						new PointF(rect.X + rect.Width - num, rect.Y + num2),
						new PointF(rect.X + rect.Width, rect.Y + num2),
						new PointF(rect.X + rect.Width / 2f, rect.Y),
						new PointF(rect.X, rect.Y + num2),
						new PointF(rect.X + num, rect.Y + num2)
					});
				}
				else
				{
					val2.AddLines(new PointF[7]
					{
						new PointF(rect.X + num, rect.Y),
						new PointF(rect.X + rect.Width - num, rect.Y),
						new PointF(rect.X + rect.Width - num, rect.Y + rect.Height - num2),
						new PointF(rect.X + rect.Width, rect.Y + rect.Height - num2),
						new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height),
						new PointF(rect.X, rect.Y + rect.Height - num2),
						new PointF(rect.X + num, rect.Y + rect.Height - num2)
					});
				}
			}
			else
			{
				float num = rect.Height * stiArrowShapeType.ArrowWidth;
				float num2 = ((stiArrowShapeType.ArrowHeight != 0f) ? (rect.Width * stiArrowShapeType.ArrowHeight) : Math.Min(rect.Width / 2f, rect.Height / 2f));
				if (stiArrowShapeType.Direction == StiShapeDirection.Left)
				{
					val2.AddLines(new PointF[7]
					{
						new PointF(rect.X + rect.Width, rect.Y + num),
						new PointF(rect.X + rect.Width, rect.Y + rect.Height - num),
						new PointF(rect.X + num2, rect.Y + rect.Height - num),
						new PointF(rect.X + num2, rect.Y + rect.Height),
						new PointF(rect.X, rect.Y + rect.Height / 2f),
						new PointF(rect.X + num2, rect.Y),
						new PointF(rect.X + num2, rect.Y + num)
					});
				}
				else
				{
					val2.AddLines(new PointF[7]
					{
						new PointF(rect.X, rect.Y + num),
						new PointF(rect.X, rect.Y + rect.Height - num),
						new PointF(rect.X + rect.Width - num2, rect.Y + rect.Height - num),
						new PointF(rect.X + rect.Width - num2, rect.Y + rect.Height),
						new PointF(rect.X + rect.Width, rect.Y + rect.Height / 2f),
						new PointF(rect.X + rect.Width - num2, rect.Y),
						new PointF(rect.X + rect.Width - num2, rect.Y + num)
					});
				}
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
