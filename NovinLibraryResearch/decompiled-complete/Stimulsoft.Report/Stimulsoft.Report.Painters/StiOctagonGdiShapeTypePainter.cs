using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiOctagonGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Invalid comparison between Unknown and I4
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiOctagonShapeType stiOctagonShapeType = (StiOctagonShapeType)shapeType;
		Graphics val = (Graphics)context;
		float num = ((shape.Report != null) ? ((float)shape.Report.Unit.ConvertToHInches((double)stiOctagonShapeType.Bevel)) : stiOctagonShapeType.Bevel) * zoom;
		float num2 = num;
		if (stiOctagonShapeType.AutoSize)
		{
			num = rect.Width / 3.4133961f;
			num2 = rect.Height / 3.4133961f;
		}
		if (num > rect.Width / 2f)
		{
			num = rect.Width / 2f;
		}
		if (num2 > rect.Height / 2f)
		{
			num2 = rect.Height / 2f;
		}
		try
		{
			float num3 = Math.Max(shape.Size, 1f);
			num = (float)Math.Round(num, 2);
			num2 = (float)Math.Round(num2, 2);
			GraphicsPath val2 = new GraphicsPath();
			try
			{
				val2.StartFigure();
				val2.AddLine(rect.X + num, rect.Y, rect.Right - num, rect.Y);
				val2.AddLine(rect.Right - num, rect.Y, rect.Right, rect.Y + num2);
				val2.AddLine(rect.Right, rect.Y + num2, rect.Right, rect.Bottom - num2);
				val2.AddLine(rect.Right, rect.Bottom - num2, rect.Right - num, rect.Bottom);
				val2.AddLine(rect.Right - num, rect.Bottom, rect.X + num, rect.Bottom);
				val2.AddLine(rect.X + num, rect.Bottom, rect.X, rect.Bottom - num2);
				val2.AddLine(rect.X, rect.Bottom - num2, rect.X, rect.Y + num2);
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
					Pen val3 = new Pen(shape.BorderColor, num3 * zoom);
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
