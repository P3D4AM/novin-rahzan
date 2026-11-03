using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters;

public abstract class StiGdiShapeTypePainter : StiShapeTypePainter
{
	public static void DrawShape(Graphics g, StiShape shape, Point[] points, RectangleF rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		GraphicsPath val = new GraphicsPath();
		try
		{
			val.AddLines(points);
			Brush brush = StiBrush.GetBrush(shape.Brush, rect);
			try
			{
				g.FillPath(brush, val);
			}
			finally
			{
				((IDisposable)brush)?.Dispose();
			}
			if ((int)shape.Style != 6)
			{
				Pen val2 = new Pen(shape.BorderColor, shape.Size);
				try
				{
					val2.DashStyle = StiPenUtils.GetPenStyle(shape.Style);
					g.DrawLines(val2, points);
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawShape(Graphics g, StiShape shape, GraphicsPath path, RectangleF rect)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		path.CloseFigure();
		Brush brush = StiBrush.GetBrush(shape.Brush, rect);
		try
		{
			g.FillPath(brush, path);
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
		if ((int)shape.Style != 6)
		{
			Pen val = new Pen(shape.BorderColor, shape.Size * (float)StiScale.Factor);
			try
			{
				val.DashStyle = StiPenUtils.GetPenStyle(shape.Style);
				g.DrawPath(val, path);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
