using System.Drawing;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Rendering;

public class TabFormColorTable
{
	public TabFormStateColorTable Active = new TabFormStateColorTable();

	public TabFormStateColorTable Inactive = new TabFormStateColorTable();

	public Thickness BorderThickness = new Thickness(1.0, 1.0, 1.0, 1.0);

	public Color BackColor = Color.Empty;

	public Color TextColor = Color.Empty;

	public eTextFormat CaptionTextFormat = eTextFormat.EndEllipsis | eTextFormat.NoPrefix | eTextFormat.VerticalCenter;
}
