using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiStripsYGeom : StiCellGeom
{
	private IStiStrips strip;

	public IStiStrips Strip => strip;

	public override void Draw(StiContext context)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		RectangleF rect = base.ClientRectangle;
		context.FillRectangle(strip.StripBrush, rect.X, rect.Y, rect.Width, rect.Height);
		if (strip.TitleVisible)
		{
			StiBrush brush = (StiBrush)new StiSolidBrush(strip.TitleColor);
			StiFontGeom font = StiFontGeom.ChangeFontSize(strip.Font, strip.Font.Size * context.Options.Zoom);
			StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
			string text = StiReportParser.Parse(strip.Text, (IStiReportComponent)(object)(strip.Chart as StiChart));
			context.DrawRotatedString(text, font, brush, rect, genericStringFormat, (StiRotationMode)4, 0f, strip.Antialiasing);
		}
	}

	public StiStripsYGeom(IStiStrips strip, RectangleF clientRectangle)
		: base(clientRectangle)
	{
		this.strip = strip;
	}
}
