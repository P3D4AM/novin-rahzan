using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiFlowchartPreparationGdiShapeTypePainter : StiGdiShapeTypePainter
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
		StiFlowchartPreparationShapeType stiFlowchartPreparationShapeType = (StiFlowchartPreparationShapeType)shapeType;
		GraphicsPath val = new GraphicsPath();
		try
		{
			float num = rect.Width / 5f;
			float num2 = rect.Height / 5f;
			float num3 = rect.Width / 2f;
			float num4 = rect.Height / 2f;
			switch (stiFlowchartPreparationShapeType.Direction)
			{
			case StiShapeDirection.Left:
			case StiShapeDirection.Right:
				val.AddLines(new PointF[7]
				{
					new PointF(rect.X, rect.Y + num4),
					new PointF(rect.X + num, rect.Y),
					new PointF(rect.Right - num, rect.Y),
					new PointF(rect.Right, rect.Y + num4),
					new PointF(rect.Right - num, rect.Bottom),
					new PointF(rect.X + num, rect.Bottom),
					new PointF(rect.X, rect.Y + num4)
				});
				break;
			case StiShapeDirection.Up:
			case StiShapeDirection.Down:
				val.AddLines(new PointF[7]
				{
					new PointF(rect.X + num3, rect.Y),
					new PointF(rect.Right, rect.Y + num2),
					new PointF(rect.Right, rect.Bottom - num2),
					new PointF(rect.X + num3, rect.Bottom),
					new PointF(rect.X, rect.Bottom - num2),
					new PointF(rect.X, rect.Y + num2),
					new PointF(rect.X + num3, rect.Y)
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
