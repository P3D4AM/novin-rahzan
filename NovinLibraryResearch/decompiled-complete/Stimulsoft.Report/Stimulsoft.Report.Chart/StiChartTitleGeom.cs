using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiChartTitleGeom : StiCellGeom
{
	private IStiChartTitle title;

	public IStiChartTitle Title => title;

	public override void Draw(StiContext context)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		if (Title.Visible)
		{
			float angle = (float)Title.Dock;
			if (Title.Dock == StiChartTitleDock.Bottom)
			{
				angle = 0f;
			}
			StiFontGeom font = StiFontGeom.ChangeFontSize(Title.Font, Title.Font.Size * context.Options.Zoom);
			StiStringFormatGeom defaultStringFormat = context.GetDefaultStringFormat();
			defaultStringFormat.Alignment = Title.Alignment;
			defaultStringFormat.Trimming = (StringTrimming)0;
			defaultStringFormat.FormatFlags = (StringFormatFlags)22528;
			context.DrawRotatedString(Title.Text, font, Title.Brush, base.ClientRectangle, defaultStringFormat, (StiRotationMode)4, angle, Title.Antialiasing);
		}
	}

	public StiChartTitleGeom(IStiChartTitle title, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.title = title;
	}
}
