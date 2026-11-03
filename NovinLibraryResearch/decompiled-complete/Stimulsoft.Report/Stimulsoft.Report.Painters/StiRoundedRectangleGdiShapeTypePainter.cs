using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiRoundedRectangleGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Invalid comparison between Unknown and I4
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiRoundedRectangleShapeType stiRoundedRectangleShapeType = (StiRoundedRectangleShapeType)shapeType;
		Graphics val = (Graphics)context;
		float num = Math.Min((rect.Height < rect.Width) ? rect.Height : rect.Width, 100f * zoom) * stiRoundedRectangleShapeType.Round;
		if (!(rect.Width >= num * 2f) || !(rect.Height >= num * 2f))
		{
			return;
		}
		try
		{
			num = (float)Math.Round(num, 2);
			if ((double)num < 0.01)
			{
				num = 0.01f;
			}
			GraphicsPath val2 = new GraphicsPath();
			try
			{
				val2.StartFigure();
				val2.AddLine(rect.X + num, rect.Y, rect.Right - num, rect.Y);
				val2.AddArc(rect.Right - 2f * num, rect.Y, 2f * num, 2f * num, -90f, 90f);
				val2.AddLine(rect.Right, rect.Y + num, rect.Right, rect.Bottom - num);
				val2.AddArc(rect.Right - 2f * num, rect.Bottom - 2f * num, 2f * num, 2f * num, 0f, 90f);
				val2.AddLine(rect.Right - num, rect.Bottom, rect.X + num, rect.Bottom);
				val2.AddArc(rect.X, rect.Bottom - 2f * num, 2f * num, 2f * num, 90f, 90f);
				val2.AddLine(rect.X, rect.Bottom - num, rect.X, rect.Y + num);
				val2.AddArc(rect.X, rect.Y, 2f * num, 2f * num, 180f, 90f);
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
		catch
		{
		}
	}
}
