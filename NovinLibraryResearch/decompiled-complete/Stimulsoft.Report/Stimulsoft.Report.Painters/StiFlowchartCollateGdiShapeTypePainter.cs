using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiFlowchartCollateGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics g = (Graphics)context;
		StiFlowchartCollateShapeType stiFlowchartCollateShapeType = (StiFlowchartCollateShapeType)shapeType;
		GraphicsPath val = new GraphicsPath();
		try
		{
			switch (stiFlowchartCollateShapeType.Direction)
			{
			case StiShapeDirection.Up:
			case StiShapeDirection.Down:
				val.AddLines(new PointF[5]
				{
					new PointF(rect.X, rect.Y),
					new PointF(rect.Right, rect.Bottom),
					new PointF(rect.X, rect.Bottom),
					new PointF(rect.Right, rect.Y),
					new PointF(rect.X, rect.Y)
				});
				break;
			case StiShapeDirection.Left:
			case StiShapeDirection.Right:
				val.AddLines(new PointF[5]
				{
					new PointF(rect.X, rect.Y),
					new PointF(rect.Right, rect.Bottom),
					new PointF(rect.Right, rect.Y),
					new PointF(rect.X, rect.Bottom),
					new PointF(rect.X, rect.Y)
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
