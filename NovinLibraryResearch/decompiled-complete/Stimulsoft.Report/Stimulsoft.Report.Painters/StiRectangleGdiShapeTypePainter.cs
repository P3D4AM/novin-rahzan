using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiRectangleGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)context;
		Brush brush = StiBrush.GetBrush(shape.Brush, rect);
		try
		{
			StiDrawing.FillRectangle(val, brush, rect);
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
		if ((int)shape.Style == 6)
		{
			return;
		}
		float num = ((zoom > 1f) ? zoom : 1f);
		Pen val2 = new Pen(shape.BorderColor, ((int)shape.Style == 5) ? num : (shape.Size * zoom));
		try
		{
			val2.DashStyle = StiPenUtils.GetPenStyle(shape.Style);
			if ((int)shape.Style == 5)
			{
				RectangleF rectangleF = rect;
				RectangleF rectangleF2 = rect;
				rectangleF.Inflate(0f - num, 0f - num);
				rectangleF2.Inflate(num, num);
				val.DrawRectangle(val2, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
				val.DrawRectangle(val2, rectangleF2.X, rectangleF2.Y, rectangleF2.Width, rectangleF2.Height);
			}
			else
			{
				val.DrawRectangle(val2, rect.X, rect.Y, rect.Width, rect.Height);
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
