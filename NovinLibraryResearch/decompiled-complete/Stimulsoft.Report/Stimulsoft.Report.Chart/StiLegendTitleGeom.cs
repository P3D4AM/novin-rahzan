using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiLegendTitleGeom : StiCellGeom
{
	private IStiLegend legend;

	public IStiLegend Legend => legend;

	public override void Draw(StiContext context)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		StiBrush brush = (StiBrush)new StiSolidBrush(legend.TitleColor);
		StiFontGeom font = StiFontGeom.ChangeFontSize(legend.TitleFont, legend.TitleFont.Size * context.Options.Zoom);
		StiStringFormatGeom defaultStringFormat = context.GetDefaultStringFormat();
		defaultStringFormat.Alignment = (StringAlignment)1;
		defaultStringFormat.LineAlignment = (StringAlignment)1;
		defaultStringFormat.FormatFlags = (StringFormatFlags)0;
		context.DrawString(legend.Title, font, brush, base.ClientRectangle, defaultStringFormat);
	}

	public StiLegendTitleGeom(IStiLegend legend, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.legend = legend;
	}
}
