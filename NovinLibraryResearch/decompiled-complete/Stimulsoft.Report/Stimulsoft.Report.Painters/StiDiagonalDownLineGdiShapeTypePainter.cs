using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiDiagonalDownLineGdiShapeTypePainter : StiGdiShapeTypePainter
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
			val.FillRectangle(brush, rect);
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
				float num2 = 1.55f * num;
				val.DrawLine(val2, rect.X + num2, rect.Y, rect.Right, rect.Bottom - num2);
				val.DrawLine(val2, rect.X, rect.Y + num2, rect.Right - num2, rect.Bottom);
			}
			else
			{
				val.DrawLine(val2, rect.X, rect.Y, rect.Right, rect.Bottom);
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}
}
