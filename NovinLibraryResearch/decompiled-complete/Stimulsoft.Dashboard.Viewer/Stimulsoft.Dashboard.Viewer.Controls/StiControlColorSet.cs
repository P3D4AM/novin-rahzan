using System.Drawing;
using Stimulsoft.Base.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Controls;

public class StiControlColorSet
{
	public Color ForeColor { get; set; }

	public Color BackColor { get; set; }

	public Color GlyphColor { get; set; }

	public StiControlColorSet()
	{
		ForeColor = StiElementConsts.ForegroundColor;
		BackColor = StiElementConsts.BackgroundColor;
		GlyphColor = StiElementConsts.ForegroundColor;
	}

	public StiControlColorSet(Color foreColor, Color backColor, Color glyphColor)
	{
		ForeColor = foreColor;
		BackColor = backColor;
		GlyphColor = glyphColor;
	}
}
