using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiYAxisViewGeom : StiYAxisGeom
{
	public override void DrawChildGeoms(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		if (base.IsCenterAxis)
		{
			rectangleF.X += ((StiAxisAreaCoreXF)base.Axis.Area.Core).GetDividerX() - (float)((StiAxisAreaCoreXF)base.Axis.Area.Core).ScrollDistanceX;
		}
		if (base.ChildGeoms == null)
		{
			return;
		}
		foreach (StiCellGeom childGeom in base.ChildGeoms)
		{
			if (AllowChildDrawing(childGeom))
			{
				if (!(childGeom is StiVertScrollBarGeom))
				{
					context.PushTranslateTransform(0f, 0f - (float)((StiAxisAreaCoreXF)base.Axis.Area.Core).ScrollDistanceY);
				}
				context.PushTranslateTransform(rectangleF.X, rectangleF.Y);
				childGeom.DrawGeom(context);
				context.PopTransform();
				if (!(childGeom is StiVertScrollBarGeom))
				{
					context.PopTransform();
				}
			}
		}
	}

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (base.IsCenterAxis)
		{
			rect.X += ((StiAxisAreaCoreXF)base.Axis.Area.Core).GetDividerX() - (float)((StiAxisAreaCoreXF)base.Axis.Area.Core).ScrollDistanceX;
		}
		if (!(rect.Width <= 0f) && !(rect.Height <= 0f) && base.Axis.ArrowStyle != 0 && !base.IsCenterAxis)
		{
			DrawArrow(context, rect);
		}
	}

	public StiYAxisViewGeom(IStiYAxis axis, RectangleF clientRectangle, bool isCenterAxis)
		: base(axis, clientRectangle, isCenterAxis)
	{
	}
}
