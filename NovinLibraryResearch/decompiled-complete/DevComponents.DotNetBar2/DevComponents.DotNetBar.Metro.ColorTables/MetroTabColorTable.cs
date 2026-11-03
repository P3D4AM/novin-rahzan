using System.Drawing;

namespace DevComponents.DotNetBar.Metro.ColorTables;

public class MetroTabColorTable
{
	public MetroTabStripColorTable TabStrip = new MetroTabStripColorTable();

	public ElementStyle TabPanelBackgroundStyle = new ElementStyle();

	public MetroTabItemColorTable MetroTabItem = new MetroTabItemColorTable();

	public Color ActiveCaptionText;

	public Color InactiveCaptionText;

	public eTextFormat CaptionTextFormat = eTextFormat.EndEllipsis | eTextFormat.NoPrefix | eTextFormat.VerticalCenter;
}
