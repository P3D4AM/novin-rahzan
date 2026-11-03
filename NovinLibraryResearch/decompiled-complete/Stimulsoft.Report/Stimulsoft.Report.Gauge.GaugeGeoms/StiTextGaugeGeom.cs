using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Gauge.GaugeGeoms;

public class StiTextGaugeGeom : StiGaugeGeom
{
	public string Text { get; }

	public Font Font { get; }

	public StiBrush Foreground { get; }

	public RectangleF Rect { get; }

	public StringFormat StringFormat { get; }

	public override StiGaugeGeomType Type => StiGaugeGeomType.Text;

	public StiTextGaugeGeom(string text, Font font, StiBrush foreground, RectangleF rect, StringFormat sf)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		Text = text;
		Font = font;
		Foreground = foreground;
		Rect = rect;
		if (sf == null)
		{
			sf = new StringFormat();
			sf.Alignment = (StringAlignment)1;
			sf.LineAlignment = (StringAlignment)1;
			sf.FormatFlags = (StringFormatFlags)4096;
		}
		StringFormat = sf;
	}
}
