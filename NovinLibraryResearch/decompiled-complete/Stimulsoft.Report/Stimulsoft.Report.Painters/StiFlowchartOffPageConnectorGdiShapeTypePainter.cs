using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiFlowchartOffPageConnectorGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics g = (Graphics)((context is Graphics) ? context : null);
		StiFlowchartOffPageConnectorShapeType stiFlowchartOffPageConnectorShapeType = (StiFlowchartOffPageConnectorShapeType)shapeType;
		GraphicsPath val = new GraphicsPath();
		try
		{
			float num = rect.Height / 5f;
			float num2 = rect.Width / 5f;
			switch (stiFlowchartOffPageConnectorShapeType.Direction)
			{
			case StiShapeDirection.Down:
				val.AddLines(new PointF[6]
				{
					new PointF(rect.X, rect.Y),
					new PointF(rect.Right, rect.Y),
					new PointF(rect.Right, rect.Bottom - num),
					new PointF(rect.X + rect.Width / 2f, rect.Bottom),
					new PointF(rect.X, rect.Bottom - num),
					new PointF(rect.X, rect.Y)
				});
				break;
			case StiShapeDirection.Up:
				val.AddLines(new PointF[6]
				{
					new PointF(rect.X, rect.Bottom),
					new PointF(rect.X, rect.Y + num),
					new PointF(rect.X + rect.Width / 2f, rect.Y),
					new PointF(rect.Right, rect.Y + num),
					new PointF(rect.Right, rect.Bottom),
					new PointF(rect.X, rect.Bottom)
				});
				break;
			case StiShapeDirection.Left:
				val.AddLines(new PointF[6]
				{
					new PointF(rect.X + num2, rect.Y),
					new PointF(rect.Right, rect.Y),
					new PointF(rect.Right, rect.Bottom),
					new PointF(rect.X + num2, rect.Bottom),
					new PointF(rect.X, rect.Y + rect.Height / 2f),
					new PointF(rect.X + num2, rect.Y)
				});
				break;
			case StiShapeDirection.Right:
				val.AddLines(new PointF[6]
				{
					new PointF(rect.X, rect.Y),
					new PointF(rect.Right - num2, rect.Y),
					new PointF(rect.Right, rect.Y + rect.Height / 2f),
					new PointF(rect.Right - num2, rect.Bottom),
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
