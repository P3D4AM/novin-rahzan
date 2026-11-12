using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public interface ISimpleTab
{
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates the text displayed on the tab.")]
	string Text { get; set; }

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the tab is visible.")]
	bool Visible { get; set; }

	[Browsable(false)]
	Rectangle DisplayRectangle { get; }

	[Browsable(true)]
	[Description("Indicates the inactive tab background color.")]
	[Category("Style")]
	Color BackColor { get; set; }

	[Browsable(true)]
	[Description("Indicates the inactive tab target gradient background color.")]
	[Category("Style")]
	Color BackColor2 { get; set; }

	[Browsable(true)]
	[Description("Indicates the gradient angle.")]
	[Category("Style")]
	[DefaultValue(90)]
	int BackColorGradientAngle { get; set; }

	[Browsable(true)]
	[Description("Indicates the inactive tab light border color.")]
	[Category("Style")]
	Color LightBorderColor { get; set; }

	[Browsable(true)]
	[Description("Indicates the inactive tab dark border color.")]
	[Category("Style")]
	Color DarkBorderColor { get; set; }

	[Browsable(true)]
	[Description("Indicates the inactive tab border color.")]
	[Category("Style")]
	Color BorderColor { get; set; }

	[Browsable(true)]
	[Description("Indicates the inactive tab text color.")]
	[Category("Style")]
	Color TextColor { get; set; }

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Design")]
	[Description("Indicates the name used to identify item.")]
	string Name { get; set; }

	[Browsable(true)]
	[DefaultValue(eTabItemColor.Default)]
	[Category("Style")]
	[Description("Applies predefined color to tab.")]
	eTabItemColor PredefinedColor { get; set; }

	bool IsSelected { get; }

	bool IsMouseOver { get; }

	eTabStripAlignment TabAlignment { get; }

	Font GetTabFont();
}
