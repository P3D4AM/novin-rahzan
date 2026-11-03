using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiTreemapAreaGeom : StiAreaGeom
{
	public override void Draw(StiContext context)
	{
		RectangleF rect = base.ClientRectangle;
		if (rect.Width > 0f && rect.Height > 0f)
		{
			if (base.Area.ShowShadow)
			{
				context.DrawCachedShadow(rect, (StiShadowSides)31, context.Options.IsPrinting);
			}
			context.FillRectangle(base.Area.Brush, rect.X, rect.Y, rect.Width, rect.Height);
			StiPenGeom pen = new StiPenGeom(base.Area.BorderColor);
			context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
	}

	public StiTreemapAreaGeom(IStiArea area, RectangleF clientRectangle)
		: base(area, clientRectangle)
	{
	}
}
