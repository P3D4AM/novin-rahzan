using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiAxisAreaViewGeom : StiAreaGeom
{
	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (!rect.IsEmpty)
		{
			if (rect.Width > 0f && rect.Height > 0f && base.Area.ShowShadow)
			{
				context.DrawCachedShadow(rect, (StiShadowSides)31, context.Options.IsPrinting);
			}
			base.Draw(context);
		}
	}

	public override void DrawGeom(StiContext context)
	{
		base.DrawGeom(context);
		DrawBorder(context);
	}

	public override void DrawChildGeoms(StiContext context)
	{
		if (base.ChildGeoms == null)
		{
			return;
		}
		RectangleF clipRect = base.ClientRectangle;
		clipRect.X = 0f;
		clipRect.Y = 0f;
		context.PushTranslateTransform(base.ClientRectangle.X, base.ClientRectangle.Y);
		foreach (StiCellGeom childGeom in base.ChildGeoms)
		{
			bool flag = childGeom is StiAxisAreaGeom || (childGeom is StiXAxisGeom && ((StiXAxisGeom)childGeom).IsCenterAxis) || (childGeom is StiYAxisGeom && ((StiYAxisGeom)childGeom).IsCenterAxis);
			if (flag)
			{
				context.PushClip(clipRect);
			}
			if (AllowChildDrawing(childGeom))
			{
				childGeom.DrawGeom(context);
			}
			if (flag)
			{
				context.PopClip();
			}
		}
		context.PopTransform();
	}

	private void DrawBorder(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		if (!rectangleF.IsEmpty)
		{
			StiPenGeom pen = new StiPenGeom(base.Area.BorderColor);
			context.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
	}

	public StiAxisAreaViewGeom(IStiArea area, RectangleF clientRectangle)
		: base(area, clientRectangle)
	{
	}
}
