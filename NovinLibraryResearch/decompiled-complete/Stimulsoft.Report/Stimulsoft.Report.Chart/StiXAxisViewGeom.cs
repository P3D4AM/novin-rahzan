using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiXAxisViewGeom : StiXAxisGeom
{
	public override void DrawChildGeoms(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		if (base.IsCenterAxis)
		{
			rectangleF.Y += (float)((double)((StiAxisAreaCoreXF)base.Axis.Area.Core).GetDividerY() - ((StiAxisAreaCoreXF)base.Axis.Area.Core).ScrollDistanceY);
		}
		if (base.ChildGeoms != null)
		{
			foreach (StiCellGeom childGeom in base.ChildGeoms)
			{
				if (AllowChildDrawing(childGeom))
				{
					if (!(childGeom is StiHorzScrollBarGeom))
					{
						context.PushTranslateTransform(0f - (float)((StiAxisAreaCoreXF)base.Axis.Area.Core).ScrollDistanceX, 0f);
					}
					context.PushTranslateTransform(rectangleF.X, rectangleF.Y);
					childGeom.DrawGeom(context);
					context.PopTransform();
					if (!(childGeom is StiHorzScrollBarGeom))
					{
						context.PopTransform();
					}
				}
			}
		}
		Draw(context);
	}

	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (base.IsCenterAxis)
		{
			rect.Y += (float)((double)((StiAxisAreaCoreXF)base.Axis.Area.Core).GetDividerY() - ((StiAxisAreaCoreXF)base.Axis.Area.Core).ScrollDistanceY);
		}
		if (!(rect.Width <= 0f) && !(rect.Height <= 0f) && base.Axis.ArrowStyle != 0 && !base.IsCenterAxis)
		{
			DrawArrow(context, rect);
		}
	}

	public StiXAxisViewGeom(IStiXAxis axis, RectangleF clientRectangle, bool isCenterAxis)
		: base(axis, clientRectangle, isCenterAxis)
	{
	}
}
