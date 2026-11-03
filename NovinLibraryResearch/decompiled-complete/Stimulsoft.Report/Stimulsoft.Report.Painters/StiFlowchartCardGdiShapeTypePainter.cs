using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiFlowchartCardGdiShapeTypePainter : StiGdiShapeTypePainter
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
			val.AddLines(new PointF[6]
			{
				new PointF(rect.Right, rect.Y),
				new PointF(rect.Right, rect.Bottom),
				new PointF(rect.X, rect.Bottom),
				new PointF(rect.X, rect.Y + rect.Height / 5f),
				new PointF(rect.X + rect.Width / 5f, rect.Y),
				new PointF(rect.Right, rect.Y)
			});
			StiGdiShapeTypePainter.DrawShape(g, shape, val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
