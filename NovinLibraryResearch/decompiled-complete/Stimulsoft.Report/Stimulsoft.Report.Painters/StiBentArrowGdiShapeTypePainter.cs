using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiBentArrowGdiShapeTypePainter : StiGdiShapeTypePainter
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
		StiBentArrowShapeType stiBentArrowShapeType = (StiBentArrowShapeType)shapeType;
		GraphicsPath val = new GraphicsPath();
		try
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			if (rect.Height > rect.Width)
			{
				num2 = rect.Width / 4f;
				num = num2;
				num3 = num2 / 2f;
			}
			else
			{
				num = (int)(rect.Height / 4f);
				num2 = num;
				num3 = num2 / 2f;
			}
			switch (stiBentArrowShapeType.Direction)
			{
			case StiShapeDirection.Up:
				val.AddLines(new PointF[10]
				{
					new PointF(rect.X, rect.Bottom),
					new PointF(rect.X, rect.Bottom - num),
					new PointF(rect.Right - (num3 + num), rect.Bottom - num),
					new PointF(rect.Right - (num3 + num), rect.Y + num2),
					new PointF(rect.Right - num2 * 2f, rect.Y + num2),
					new PointF(rect.Right - num2, rect.Y),
					new PointF(rect.Right, rect.Y + num2),
					new PointF(rect.Right - num3, rect.Y + num2),
					new PointF(rect.Right - num3, rect.Bottom),
					new PointF(rect.X, rect.Bottom)
				});
				break;
			case StiShapeDirection.Left:
				val.AddLines(new PointF[10]
				{
					new PointF(rect.Right, rect.Bottom),
					new PointF(rect.Right, rect.Y + num3),
					new PointF(rect.X + num2, rect.Y + num3),
					new PointF(rect.X + num2, rect.Y),
					new PointF(rect.X, rect.Y + num2),
					new PointF(rect.X + num2, rect.Y + num2 * 2f),
					new PointF(rect.X + num2, rect.Y + num2 + num3),
					new PointF(rect.Right - num, rect.Y + num2 + num3),
					new PointF(rect.Right - num, rect.Bottom),
					new PointF(rect.Right, rect.Bottom)
				});
				break;
			case StiShapeDirection.Down:
				val.AddLines(new PointF[10]
				{
					new PointF(rect.Right, rect.Y),
					new PointF(rect.X + num3, rect.Y),
					new PointF(rect.X + num3, rect.Bottom - num2),
					new PointF(rect.X, rect.Bottom - num2),
					new PointF(rect.X + num2, rect.Bottom),
					new PointF(rect.X + num2 * 2f, rect.Bottom - num2),
					new PointF(rect.X + num2 + num3, rect.Bottom - num2),
					new PointF(rect.X + num2 + num3, rect.Y + num),
					new PointF(rect.Right, rect.Y + num),
					new PointF(rect.Right, rect.Y)
				});
				break;
			case StiShapeDirection.Right:
				val.AddLines(new PointF[10]
				{
					new PointF(rect.X, rect.Y),
					new PointF(rect.X, rect.Bottom - num3),
					new PointF(rect.Right - num2, rect.Bottom - num3),
					new PointF(rect.Right - num2, rect.Bottom),
					new PointF(rect.Right, rect.Bottom - num2),
					new PointF(rect.Right - num2, rect.Bottom - num2 * 2f),
					new PointF(rect.Right - num2, rect.Bottom - num2 - num3),
					new PointF(rect.X + num, rect.Bottom - num2 - num3),
					new PointF(rect.X + num, rect.Y),
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
