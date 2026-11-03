using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiOvalGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics val = (Graphics)context;
		Brush brush = StiBrush.GetBrush(shape.Brush, rect);
		try
		{
			val.FillEllipse(brush, rect);
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
		if ((int)shape.Style != 6)
		{
			Pen val2 = new Pen(shape.BorderColor, shape.Size * zoom);
			try
			{
				val2.DashStyle = StiPenUtils.GetPenStyle(shape.Style);
				val.DrawEllipse(val2, rect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}
}
