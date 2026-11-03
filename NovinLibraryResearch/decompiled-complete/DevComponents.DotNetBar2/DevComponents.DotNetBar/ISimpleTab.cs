using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

public interface ISimpleTab
{
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates the text displayed on the tab.")]
	[DevCoBrowsable(true)]
	string Text { get; set; }

	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether the tab is visible.")]
	[Browsable(true)]
	[DefaultValue(true)]
	bool Visible { get; set; }

	[Browsable(false)]
	Rectangle DisplayRectangle { get; }

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates the inactive tab background color.")]
	Color BackColor { get; set; }

	[Browsable(true)]
	[Description("Indicates the inactive tab target gradient background color.")]
	[Category("Style")]
	Color BackColor2 { get; set; }

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates the gradient angle.")]
	[DefaultValue(90)]
	int BackColorGradientAngle { get; set; }

	[Browsable(true)]
	[Description("Indicates the inactive tab light border color.")]
	[Category("Style")]
	Color LightBorderColor { get; set; }

	[Browsable(true)]
	[Category("Style")]
	[Description("Indicates the inactive tab dark border color.")]
	Color DarkBorderColor { get; set; }

	[Category("Style")]
	[Browsable(true)]
	[Description("Indicates the inactive tab border color.")]
	Color BorderColor { get; set; }

	[Description("Indicates the inactive tab text color.")]
	[Browsable(true)]
	[Category("Style")]
	Color TextColor { get; set; }

	[Description("Indicates the name used to identify item.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Design")]
	string Name { get; set; }

	[DefaultValue(eTabItemColor.Default)]
	[Category("Style")]
	[Description("Applies predefined color to tab.")]
	[Browsable(true)]
	eTabItemColor PredefinedColor { get; set; }

	bool IsSelected { get; }

	bool IsMouseOver { get; }

	eTabStripAlignment TabAlignment { get; }

	Font GetTabFont();
}
