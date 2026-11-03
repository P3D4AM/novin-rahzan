using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiPictorialAreaGeom : StiAreaGeom
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

	private void DrawBorder(StiContext context)
	{
		RectangleF rectangleF = base.ClientRectangle;
		if (!rectangleF.IsEmpty)
		{
			StiPenGeom pen = new StiPenGeom(base.Area.BorderColor);
			context.DrawRectangle(pen, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
		}
	}

	public StiPictorialAreaGeom(IStiArea area, RectangleF clientRectangle)
		: base(area, clientRectangle)
	{
	}
}
