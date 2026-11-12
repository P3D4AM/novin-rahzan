using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class Office2007RibbonColorTable
{
	public LinearGradientColorTable OuterBorder = new LinearGradientColorTable(ColorScheme.GetColor("8DB2E3"), ColorScheme.GetColor("88A1C2"));

	public LinearGradientColorTable InnerBorder = new LinearGradientColorTable(ColorScheme.GetColor("E7EFF8"), ColorScheme.GetColor("C0F9FF"));

	public LinearGradientColorTable TabsBackground = new LinearGradientColorTable(ColorScheme.GetColor("BFDBFF"), Color.Empty);

	public LinearGradientColorTable TabsGlassBackground = new LinearGradientColorTable(ColorScheme.GetColor("BFDBFF"), Color.Empty);

	public Color TabDividerBorder = ColorScheme.GetColor("AECAF0");

	public Color TabDividerBorderLight = ColorScheme.GetColor("D4E3F5");

	public int CornerSize = 3;

	public float PanelTopBackgroundHeight = 15f;

	public LinearGradientColorTable PanelTopBackground = new LinearGradientColorTable(ColorScheme.GetColor("DBE6F4"), ColorScheme.GetColor("CFDDEF"));

	public LinearGradientColorTable PanelBottomBackground = new LinearGradientColorTable(ColorScheme.GetColor("C9D9ED"), ColorScheme.GetColor("E7F2FF"));

	public Image StartButtonDefault;

	public Image StartButtonMouseOver;

	public Image StartButtonPressed;
}
