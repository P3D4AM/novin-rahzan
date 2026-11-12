using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Schedule;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007SilverColorTableFactory
{
	public static Office2007RibbonBarStateColorTable GetRibbonBar(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(12435393), factory.GetColor(8750469));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(16185337), factory.GetColor(16448250));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(15659510), factory.GetColor(14804718));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(14015463), factory.GetColor(15660276));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(14672879), factory.GetColor(12830673));
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(3355443);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarMouseOver(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(12435393), factory.GetColor(8750469));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(16448508), factory.GetColor(16448250));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(16251130), factory.GetColor(15987959));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(15659253), factory.GetColor(16251385));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(14607086), factory.GetColor(11778503));
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(3355443);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarExpanded(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(8224125), factory.GetColor(9737364));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(Color.FromArgb(128, factory.GetColor(10000537)), Color.Transparent);
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(15066598), factory.GetColor(12567500));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(11778499), factory.GetColor(14607588));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(14607588), Color.Empty);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemDefault(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(15921908), factory.GetColor(14936047));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(15727097), factory.GetColor(11926015));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12500670), factory.GetColor(12500670));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(15923194), factory.GetColor(14804719));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16444610), factory.GetColor(16777149));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15252338), factory.GetColor(16699742));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(14406851), factory.GetColor(15322765));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(13883359), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(15264494), factory.GetColor(14343652));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12435137), factory.GetColor(12435137));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemMagenta(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(15452103), factory.GetColor(16179165));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16180966), factory.GetColor(15121339));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12827836), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16506522), factory.GetColor(16642516));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16438654), factory.GetColor(16309396));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14138493), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(14800345), factory.GetColor(15108485));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14735581), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14799831), factory.GetColor(15173765));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12435137), factory.GetColor(12500670));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemGreen(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(12706488), factory.GetColor(14807515));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12501693), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16506782), factory.GetColor(16642516));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16642773), factory.GetColor(16308362));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(13091250), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(14213089), factory.GetColor(10143890));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14278367), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14213089), factory.GetColor(10143890));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12435137), factory.GetColor(12500670));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemOrange(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(13092276), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16506522), factory.GetColor(16642516));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16372342), factory.GetColor(16308362));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14138493), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(3355443);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(14737884), factory.GetColor(11970867));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14737884), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14737884), factory.GetColor(15392621));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12435137), factory.GetColor(12435137));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(3355443);
		return office2007RibbonTabItemColorTable;
	}

	public static void SetExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
	{
		Color color = factory.GetColor(8355711);
		Color color2 = factory.GetColor(15001320);
		ct.Default.ExpandBackground = color;
		ct.Default.ExpandLight = color2;
		ct.Checked.ExpandBackground = color;
		ct.Checked.ExpandLight = color2;
		ct.Expanded.ExpandBackground = color;
		ct.Expanded.ExpandLight = color2;
		ct.MouseOver.ExpandBackground = color;
		ct.MouseOver.ExpandLight = color2;
		ct.Pressed.ExpandBackground = color;
		ct.Pressed.ExpandLight = color2;
	}

	public static Office2007ButtonItemColorTable GetButtonItemOrange(bool ribbonBar, ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14536603), factory.GetColor(12625782));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777207), factory.GetColor(16775062));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776151), factory.GetColor(16770957));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16766792), factory.GetColor(16770963));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(14402420), factory.GetColor(13155212));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16773311), factory.GetColor(16774879));
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(9139796), factory.GetColor(12892584));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(15312993), factory.GetColor(16624913));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(16299370), factory.GetColor(16556128));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16484924), factory.GetColor(16694626));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16769199), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(11566411), factory.GetColor(9467987));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16433300), factory.GetColor(16767404));
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(10450265), factory.GetColor(16764717));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(15454644), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(16505781), factory.GetColor(16697208));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(16692310), factory.GetColor(16640927));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16774559), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(9339237), factory.GetColor(13025458));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(15052373), factory.GetColor(16766538));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(16569255), factory.GetColor(16427099));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(16289065), factory.GetColor(16639129));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16774857), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(11566411), factory.GetColor(9139796));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16433300), factory.GetColor(16767147));
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(3355443);
		SetExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemOrangeWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemOrange = GetButtonItemOrange(ribbonBar: false, factory);
		buttonItemOrange.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(15462138), factory.GetColor(14080740));
		buttonItemOrange.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(12961745), factory.GetColor(13949154));
		buttonItemOrange.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		buttonItemOrange.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9278621), Color.Empty);
		buttonItemOrange.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemOrange.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(14936302), factory.GetColor(13357787));
		buttonItemOrange.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(13357787), factory.GetColor(13357787));
		buttonItemOrange.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(11776947), Color.Empty);
		buttonItemOrange.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemOrange.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		return buttonItemOrange;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlue(bool ribbonBar, ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(14741247));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(12639487), factory.GetColor(11128575));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(10931711), factory.GetColor(13296127));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(124, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(7640514), factory.GetColor(9482198));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14083059), factory.GetColor(8627664));
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(14741247));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(12967160), factory.GetColor(11127543));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(9484010), factory.GetColor(7640514));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(7640514), factory.GetColor(9482198));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14083059), factory.GetColor(8627664));
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(5668272), factory.GetColor(5668272));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(9484010), factory.GetColor(7640514));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(3355443);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(6391735), factory.GetColor(5014472));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(10663117), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(5471928), factory.GetColor(6915498));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(10141941), factory.GetColor(12179455));
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(3355443);
		SetExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlue = GetButtonItemBlue(ribbonBar: false, factory);
		buttonItemBlue.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(15462138), factory.GetColor(14080740));
		buttonItemBlue.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(12961745), factory.GetColor(13949154));
		buttonItemBlue.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		buttonItemBlue.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9278621), Color.Empty);
		buttonItemBlue.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlue.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(14936302), factory.GetColor(13357787));
		buttonItemBlue.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(13357787), factory.GetColor(13357787));
		buttonItemBlue.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(11776947), Color.Empty);
		buttonItemBlue.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlue.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		return buttonItemBlue;
	}

	public static Office2007ButtonItemColorTable GetButtonItemMagenta(bool ribbonBar, ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(7012448), factory.GetColor(5636429));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(15259391));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(15917035), factory.GetColor(15124187));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(15378644), factory.GetColor(14778817));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(10714065), factory.GetColor(8939694));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14271219), factory.GetColor(12297939));
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(7012448), factory.GetColor(5636429));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(15259391));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(13807814), factory.GetColor(13348289));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(13793973), factory.GetColor(13126815));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(10714065), factory.GetColor(8939694));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14271219), factory.GetColor(12297939));
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(7012448), factory.GetColor(5636429));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(15259391));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(15325154), factory.GetColor(14856401));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(13603002), factory.GetColor(13595568));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(7012448), factory.GetColor(5636429));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(15259391));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(15325154), factory.GetColor(14856401));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(13603002), factory.GetColor(13595568));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(10714065), factory.GetColor(8939694));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14271219), factory.GetColor(12297939));
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(0);
		Office2007ColorTableFactory.SetBlackExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemMagentaWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemMagenta = GetButtonItemMagenta(ribbonBar: false, factory);
		buttonItemMagenta.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(15462138), factory.GetColor(14080740));
		buttonItemMagenta.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(12961745), factory.GetColor(13949154));
		buttonItemMagenta.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		buttonItemMagenta.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9278621), Color.Empty);
		buttonItemMagenta.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemMagenta.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(14936302), factory.GetColor(13357787));
		buttonItemMagenta.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(13357787), factory.GetColor(13357787));
		buttonItemMagenta.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(11776947), Color.Empty);
		buttonItemMagenta.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemMagenta.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		return buttonItemMagenta;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupDefault(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(15985558));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(16704797)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(3355443);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupMagenta(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(14658993));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(15884889)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(3355443);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupGreen(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(11918000));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(7125332)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(3355443);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupOrange(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(15719840));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(15971611)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(3355443);
		return office2007RibbonTabGroupColorTable;
	}

	public static void InitializeScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(16579836), factory.GetColor(15790320), 0);
		@default.Border = new LinearGradientColorTable(factory.GetColor(15461871), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(15462128), 0f),
			new BackgroundColorBlend(factory.GetColor(15133168), 0.5f),
			new BackgroundColorBlend(factory.GetColor(13753060), 0.5f),
			new BackgroundColorBlend(factory.GetColor(12503771), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(15264233), factory.GetColor(15330546));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(6254227), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = t.ScrollBar.MouseOver;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(15857406), 0f),
			new BackgroundColorBlend(factory.GetColor(14674679), 0.5f),
			new BackgroundColorBlend(factory.GetColor(12243179), 0.5f),
			new BackgroundColorBlend(factory.GetColor(13491955), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(16448767), factory.GetColor(15660284));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(9213861), factory.GetColor(6713727));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(3357780), factory.GetColor(5596557));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(12571114), 0f),
			new BackgroundColorBlend(factory.GetColor(13295610), 0.5f),
			new BackgroundColorBlend(factory.GetColor(11193334), 0.5f),
			new BackgroundColorBlend(factory.GetColor(13886714), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(15329771), factory.GetColor(16580095));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(3960496), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = t.ScrollBar.MouseOverControl;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16054783), 0f),
			new BackgroundColorBlend(factory.GetColor(16054783), 0.5f),
			new BackgroundColorBlend(factory.GetColor(13623800), 0.5f),
			new BackgroundColorBlend(factory.GetColor(14675196), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(16514559), Color.Empty);
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(9213861), factory.GetColor(6713727));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(15462128), 0f),
			new BackgroundColorBlend(factory.GetColor(15133168), 0.5f),
			new BackgroundColorBlend(factory.GetColor(13753060), 0.5f),
			new BackgroundColorBlend(factory.GetColor(12503771), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(15264233), factory.GetColor(15330802));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(6254227), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = t.ScrollBar.Pressed;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(15133164), 0f),
			new BackgroundColorBlend(factory.GetColor(14869734), 0.6f),
			new BackgroundColorBlend(factory.GetColor(12831442), 0.6f),
			new BackgroundColorBlend(factory.GetColor(14146785), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(16119800), Color.Empty);
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(9213861), factory.GetColor(6647935));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(10403561), 0f),
			new BackgroundColorBlend(factory.GetColor(11259126), 0.5f),
			new BackgroundColorBlend(factory.GetColor(7251696), 0.5f),
			new BackgroundColorBlend(factory.GetColor(11915767), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(12768231), factory.GetColor(14543867));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(1526154), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = t.ScrollBar.Disabled;
		@default.Background = t.ScrollBar.Default.Background;
		@default.Border = t.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(12570615), factory.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void InitializeColorTable(Office2007ColorTable table, ColorFactory factory)
	{
		table.RibbonBar.Default = GetRibbonBar(factory);
		table.RibbonBar.MouseOver = GetRibbonBarMouseOver(factory);
		table.RibbonBar.Expanded = GetRibbonBarExpanded(factory);
		table.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable ribbonTabItemDefault = GetRibbonTabItemDefault(factory);
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		ribbonTabItemDefault = GetRibbonTabItemGreen(factory);
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		ribbonTabItemDefault = GetRibbonTabItemMagenta(factory);
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		ribbonTabItemDefault = GetRibbonTabItemOrange(factory);
		ribbonTabItemDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		table.RibbonTabItemColors.Add(ribbonTabItemDefault);
		table.RibbonControl = new Office2007RibbonColorTable();
		table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(13685981), factory.GetColor(13685981));
		table.RibbonControl.InnerBorder = new LinearGradientColorTable();
		table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(12500670), factory.GetColor(12500670));
		table.RibbonControl.TabDividerBorder = factory.GetColor(11317175);
		table.RibbonControl.TabDividerBorderLight = Color.Empty;
		table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(15659252), factory.GetColor(14804718));
		table.RibbonControl.PanelBottomBackground = new LinearGradientColorTable(factory.GetColor(14015463), factory.GetColor(15792891));
		table.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonNormalSilver.png");
		table.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonHotSilver.png");
		table.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonPressedSilver.png");
		table.ItemGroup.TopBackground = new LinearGradientColorTable(factory.GetColor(15856627), factory.GetColor(15790834));
		table.ItemGroup.BottomBackground = new LinearGradientColorTable(factory.GetColor(15198958), factory.GetColor(16185336));
		table.ItemGroup.InnerBorder = new LinearGradientColorTable(factory.GetColor(16382457), factory.GetColor(16777215));
		table.ItemGroup.OuterBorder = new LinearGradientColorTable(factory.GetColor(12961479), factory.GetColor(12895942));
		table.ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, factory.GetColor(13553358));
		table.ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, factory.GetColor(16777215));
		table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(15198443), factory.GetColor(11910087));
		table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(11120566), factory.GetColor(13093326));
		table.Bar.ToolbarBottomBorder = factory.GetColor(7501952);
		table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(16448250), Color.Empty);
		table.Bar.PopupToolbarBorder = factory.GetColor(3092271);
		table.Bar.StatusBarTopBorder = factory.GetColor(11317175);
		table.Bar.StatusBarTopBorderLight = factory.GetColor(Color.FromArgb(148, Color.White));
		table.Bar.StatusBarAltBackground.Clear();
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(15198443), 0f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(11975623), 0.4f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(10857391), 0.4f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(11449530), 1f));
		table.ButtonItemColors.Clear();
		table.RibbonButtonItemColors.Clear();
		table.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable buttonItemOrange = GetButtonItemOrange(ribbonBar: false, factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		table.ButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemOrangeWithBackground(factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		table.ButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemBlue(ribbonBar: false, factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		table.ButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemBlueWithBackground(factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		table.ButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemMagenta(ribbonBar: false, factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		table.ButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemMagentaWithBackground(factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.ButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemOrange(ribbonBar: true, factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		table.RibbonButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemOrangeWithBackground(factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		table.RibbonButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemBlue(ribbonBar: true, factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		table.RibbonButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemBlueWithBackground(factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		table.RibbonButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemMagenta(ribbonBar: true, factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		table.RibbonButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = GetButtonItemMagentaWithBackground(factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.RibbonButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = Office2007ColorTableFactory.GetButtonItemBlackOrange(ribbonBar: true, factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		buttonItemOrange.Default.Text = factory.GetColor(3355443);
		buttonItemOrange.MouseOver.Text = factory.GetColor(3355443);
		buttonItemOrange.Checked.Text = factory.GetColor(3355443);
		buttonItemOrange.Expanded.Text = factory.GetColor(3355443);
		buttonItemOrange.Pressed.Text = factory.GetColor(3355443);
		table.MenuButtonItemColors.Add(buttonItemOrange);
		buttonItemOrange = Office2007ColorTableFactory.GetButtonItemOffice2007WithBackground(factory);
		buttonItemOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		table.ButtonItemColors.Add(buttonItemOrange);
		table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
		table.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable ribbonTabGroupDefault = GetRibbonTabGroupDefault(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = GetRibbonTabGroupMagenta(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = GetRibbonTabGroupGreen(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		ribbonTabGroupDefault = GetRibbonTabGroupOrange(factory);
		ribbonTabGroupDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		table.RibbonTabGroupColors.Add(ribbonTabGroupDefault);
		table.Menu.Background = new LinearGradientColorTable(factory.GetColor(16448250), Color.Empty);
		table.Menu.Border = new LinearGradientColorTable(factory.GetColor(8816262), Color.Empty);
		table.Menu.Side = new LinearGradientColorTable(factory.GetColor(15724527), Color.Empty);
		table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(12961221), Color.Empty);
		table.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(16119285), Color.Empty);
		table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(15066597), Color.Empty);
		table.Menu.FileBackgroundBlend.Clear();
		table.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(14607077), 0f),
			new BackgroundColorBlend(factory.GetColor(13817820), 1f)
		});
		table.Menu.FileContainerBorder = factory.GetColor(11120308);
		table.Menu.FileContainerBorderLight = factory.GetColor(15987956);
		table.Menu.FileColumnOneBackground = factory.GetColor(16448250);
		table.Menu.FileColumnOneBorder = factory.GetColor(11120308);
		table.Menu.FileColumnTwoBackground = factory.GetColor(16448250);
		table.Menu.FileBottomContainerBackgroundBlend.Clear();
		table.Menu.FileBottomContainerBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(13291735), 0f),
			new BackgroundColorBlend(factory.GetColor(13423064), 0.4f),
			new BackgroundColorBlend(factory.GetColor(12765392), 0.4f),
			new BackgroundColorBlend(factory.GetColor(14279398), 1f)
		});
		table.ComboBox.Default.Background = factory.GetColor(15264492);
		table.ComboBox.Default.Border = factory.GetColor(11121080);
		table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandText = factory.GetColor(8158332);
		table.ComboBox.DefaultStandalone.Background = factory.GetColor(16777215);
		int color = 10790052;
		table.ComboBox.DefaultStandalone.Border = factory.GetColor(color);
		table.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(factory.GetColor(15528181), factory.GetColor(14673131), 90);
		table.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable();
		table.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(12040119), Color.Empty, 90);
		table.ComboBox.DefaultStandalone.ExpandText = factory.GetColor(0);
		table.ComboBox.MouseOver.Background = factory.GetColor(16777215);
		table.ComboBox.MouseOver.Border = factory.GetColor(color);
		table.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(factory.GetColor(16776418), factory.GetColor(16770981), 90);
		table.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(16776435), 90);
		table.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(14405273), Color.Empty, 90);
		table.ComboBox.MouseOver.ExpandText = factory.GetColor(0);
		table.ComboBox.DroppedDown.Background = factory.GetColor(16777215);
		table.ComboBox.DroppedDown.Border = factory.GetColor(9013641);
		table.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(factory.GetColor(15392959), factory.GetColor(16766038), 90);
		table.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(factory.GetColor(15854549), factory.GetColor(16770708), 90);
		table.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(factory.GetColor(10129251), Color.Empty, 90);
		table.ComboBox.DroppedDown.ExpandText = factory.GetColor(0);
		table.DialogLauncher.Default.DialogLauncher = factory.GetColor(6645872);
		table.DialogLauncher.Default.DialogLauncherShade = factory.GetColor(15461355);
		table.DialogLauncher.MouseOver.DialogLauncher = factory.GetColor(6645872);
		table.DialogLauncher.MouseOver.DialogLauncherShade = factory.GetColor(15461355);
		table.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776415), factory.GetColor(16773031));
		table.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767349), factory.GetColor(16769944));
		table.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(16776178));
		table.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14405273), Color.Empty);
		table.DialogLauncher.Pressed.DialogLauncher = factory.GetColor(6645872);
		table.DialogLauncher.Pressed.DialogLauncherShade = factory.GetColor(15461355);
		table.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(15261373), factory.GetColor(15386253));
		table.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16754488), factory.GetColor(16763982));
		table.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(15788756), factory.GetColor(16769937));
		table.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(10129251), factory.GetColor(11576434));
		InitializeBlackLegacyColors(table.LegacyColors, factory);
		table.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(7174537), factory.GetColor(8952241));
		table.SystemButton.Default.LightShade = factory.GetColor(16645631);
		table.SystemButton.Default.DarkShade = factory.GetColor(4539717);
		table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(7174537), factory.GetColor(8952241));
		table.SystemButton.MouseOver.LightShade = factory.GetColor(16645631);
		table.SystemButton.MouseOver.DarkShade = factory.GetColor(4539717);
		table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16251387), factory.GetColor(15857146));
		table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(14936820), factory.GetColor(14870770));
		table.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(factory.GetColor(16514045), Color.Transparent);
		table.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(factory.GetColor(16382716), Color.Transparent);
		table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(13159892), factory.GetColor(12174032));
		table.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16711423), factory.GetColor(16580094));
		table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(7174537), factory.GetColor(8952241));
		table.SystemButton.Pressed.LightShade = factory.GetColor(16645631);
		table.SystemButton.Pressed.DarkShade = factory.GetColor(4539717);
		table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(13291219), factory.GetColor(11186102));
		table.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(factory.GetColor(15001578), Color.Transparent);
		table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(9015447), factory.GetColor(12832212));
		table.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(factory.GetColor(14017257), Color.Transparent);
		table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(9936032), factory.GetColor(11383480));
		table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(15659509), factory.GetColor(16514559));
		table.Form.Active.BorderColors = new Color[5]
		{
			factory.GetColor(7501952),
			factory.GetColor(12303034),
			factory.GetColor(13685981),
			factory.GetColor(13685981),
			factory.GetColor(13685981)
		};
		table.Form.Inactive.BorderColors = new Color[5]
		{
			factory.GetColor(11844033),
			factory.GetColor(15790320),
			factory.GetColor(14737632),
			factory.GetColor(13685981),
			factory.GetColor(13685981)
		};
		table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(15198443), factory.GetColor(13290961));
		table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(12239306), factory.GetColor(15331063));
		table.Form.Active.CaptionBottomBorder = new Color[2]
		{
			factory.GetColor(15331063),
			factory.GetColor(11317175)
		};
		table.Form.Active.CaptionText = factory.GetColor(5067353);
		table.Form.Active.CaptionTextExtra = factory.GetColor(3501745);
		table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(15658992), factory.GetColor(15066856));
		table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(14541029), factory.GetColor(16054267));
		table.Form.Inactive.CaptionText = factory.GetColor(4473924);
		table.Form.Inactive.CaptionTextExtra = factory.GetColor(4473924);
		table.Form.BackColor = factory.GetColor(13290966);
		table.Form.TextColor = factory.GetColor(0);
		table.QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(factory.GetColor(14146011), factory.GetColor(14606306));
		table.QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(factory.GetColor(13817821), factory.GetColor(14343392));
		table.QuickAccessToolbar.Active.OutterBorderColor = Color.Empty;
		table.QuickAccessToolbar.Active.MiddleBorderColor = factory.GetColor(11447984);
		table.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(factory.GetColor(15198184), factory.GetColor(15658992));
		table.QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(factory.GetColor(15264493), factory.GetColor(14277340));
		table.QuickAccessToolbar.Inactive.OutterBorderColor = Color.Empty;
		table.QuickAccessToolbar.Inactive.MiddleBorderColor = factory.GetColor(11645362);
		table.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(14278374), factory.GetColor(14080995));
		table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(12765652);
		table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.InnerBorderColor = factory.GetColor(15593459);
		table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(15461355);
		table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(5002076);
		table.QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		table.QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		table.TabControl.Default = new Office2007TabItemStateColorTable();
		table.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14475493), factory.GetColor(14212578));
		table.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(12829635), factory.GetColor(14671839));
		table.TabControl.Default.InnerBorder = factory.GetColor(15594484);
		table.TabControl.Default.OuterBorder = factory.GetColor(7303284);
		table.TabControl.Default.Text = factory.GetColor(0);
		table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776683), factory.GetColor(16772264));
		table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767577), factory.GetColor(16770701));
		table.TabControl.MouseOver.InnerBorder = factory.GetColor(16777211);
		table.TabControl.MouseOver.OuterBorder = factory.GetColor(11967859);
		table.TabControl.MouseOver.Text = factory.GetColor(0);
		table.TabControl.Selected = new Office2007TabItemStateColorTable();
		table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.White), factory.GetColor(15264494));
		table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(15264494), factory.GetColor(15264494));
		table.TabControl.Selected.InnerBorder = factory.GetColor(Color.White);
		table.TabControl.Selected.OuterBorder = factory.GetColor(7303284);
		table.TabControl.Selected.Text = factory.GetColor(0);
		table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(13093587), factory.GetColor(13093587));
		table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(15264494), factory.GetColor(13948896));
		table.TabControl.TabPanelBorder = factory.GetColor(7303284);
		Office2007CheckBoxColorTable checkBoxItem = table.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = factory.GetColor(10198432);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(13291477)), Color.FromArgb(164, factory.GetColor(16185078)));
		checkBoxItem.Default.CheckInnerBorder = factory.GetColor(10661049);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(5204872), Color.Empty);
		checkBoxItem.Default.Text = factory.GetColor(3355443);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(16053492), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = factory.GetColor(10198432);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(16574383)), Color.FromArgb(128, factory.GetColor(16709863)));
		checkBoxItem.MouseOver.CheckInnerBorder = factory.GetColor(16438650);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(5204872), Color.Empty);
		checkBoxItem.MouseOver.Text = factory.GetColor(3355443);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(15068407), Color.Empty);
		checkBoxItem.Pressed.CheckBorder = factory.GetColor(10198432);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(16765031)), Color.FromArgb(164, factory.GetColor(16774357)));
		checkBoxItem.Pressed.CheckInnerBorder = factory.GetColor(15894822);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(5204872), Color.Empty);
		checkBoxItem.Pressed.Text = factory.GetColor(3355443);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = factory.GetColor(14014683);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(15659250)), Color.FromArgb(164, factory.GetColor(16514043)));
		checkBoxItem.Disabled.CheckInnerBorder = factory.GetColor(14738149);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(9276813), Color.Empty);
		checkBoxItem.Disabled.Text = factory.GetColor(9276813);
		InitializeScrollBarColorTable(table, factory);
		Office2007ColorTableFactory.InitializeAppSilverScrollBarColorTable(table, factory);
		Office2007ProgressBarColorTable progressBarItem = table.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = factory.GetColor(16317439);
		progressBarItem.InnerBorder = factory.GetColor(12632256);
		progressBarItem.Chunk = new GradientColorTable(6590497, 15072444, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(164, factory.GetColor(14214851)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(8958024)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(6987039)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = table.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = factory.GetColor(16317439);
		progressBarItem.InnerBorder = factory.GetColor(12632256);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		progressBarItem = table.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(13028309, 14738669);
		progressBarItem.OuterBorder = factory.GetColor(16317439);
		progressBarItem.InnerBorder = factory.GetColor(12632256);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, factory.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(11712968, 14015205, 0);
		Office2007GalleryColorTable gallery = table.Gallery;
		gallery.GroupLabelBackground = factory.GetColor(15461355);
		gallery.GroupLabelText = factory.GetColor(5002076);
		gallery.GroupLabelBorder = factory.GetColor(12961221);
		table.ListViewEx.Border = factory.GetColor(color);
		table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(13948891));
		table.ListViewEx.ColumnSeparator = factory.GetColor(7237007);
		table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(10997232), Color.Empty);
		table.ListViewEx.SelectionBorder = factory.GetColor(14938111);
		table.NavigationPane.ButtonBackground = new GradientColorTable();
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15462138), 0f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14080740), 0.4f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12961745), 0.4f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(13949154), 1f));
		table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(15000816));
		table.SuperTooltip.TextColor = factory.GetColor(5000268);
		Office2007SliderColorTable slider = table.Slider;
		slider.Default.LabelColor = factory.GetColor(2303530);
		slider.Default.SliderLabelColor = factory.GetColor(3355443);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15856114), 0.15f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12435651), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(7896453), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16316665), 0.85f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 1f));
		slider.Default.PartBorderColor = factory.GetColor(4014923);
		slider.Default.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(16777215));
		slider.Default.PartForeColor = factory.GetColor(6252401);
		slider.Default.PartForeLightColor = Color.FromArgb(168, factory.GetColor(15461356));
		slider.Default.TrackLineColor = factory.GetColor(2434341);
		slider.Default.TrackLineLightColor = factory.GetColor(13421772);
		slider.MouseOver.LabelColor = factory.GetColor(2303530);
		slider.MouseOver.SliderLabelColor = factory.GetColor(3355443);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16776693), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16768899), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14526221), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16774350), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16774350), 1f));
		slider.MouseOver.PartBorderColor = factory.GetColor(3092271);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(16777215));
		slider.MouseOver.PartForeColor = factory.GetColor(6775369);
		slider.MouseOver.PartForeLightColor = Color.FromArgb(168, factory.GetColor(16776955));
		slider.MouseOver.TrackLineColor = factory.GetColor(2434341);
		slider.MouseOver.TrackLineLightColor = factory.GetColor(13421772);
		slider.Pressed.LabelColor = factory.GetColor(2303530);
		slider.Pressed.SliderLabelColor = factory.GetColor(3355443);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15959586), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15901267), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15760912), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15976092), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16572615), 1f));
		slider.Pressed.PartBorderColor = factory.GetColor(3092271);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(16777215));
		slider.Pressed.PartForeColor = factory.GetColor(6771265);
		slider.Pressed.PartForeLightColor = Color.FromArgb(168, factory.GetColor(16770766));
		slider.Pressed.TrackLineColor = factory.GetColor(2434341);
		slider.Pressed.TrackLineLightColor = factory.GetColor(13421772);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = factory.GetColor(9276813);
		slider.Disabled.SliderLabelColor = factory.GetColor(9276813);
		slider.Disabled.PartBackground = new GradientColorTable();
		foreach (BackgroundColorBlend color2 in slider.Default.PartBackground.Colors)
		{
			slider.Disabled.PartBackground.Colors.Add(new BackgroundColorBlend(colorBlendFactory.GetColor(color2.Color), color2.Position));
		}
		slider.Disabled.PartBorderColor = colorBlendFactory.GetColor(slider.Default.PartBorderColor);
		slider.Disabled.PartBorderLightColor = colorBlendFactory.GetColor(slider.Default.PartBorderLightColor);
		slider.Disabled.PartForeColor = colorBlendFactory.GetColor(slider.Default.PartForeColor);
		slider.Disabled.PartForeLightColor = colorBlendFactory.GetColor(slider.Default.PartForeLightColor);
		slider.Disabled.TrackLineColor = colorBlendFactory.GetColor(slider.Default.TrackLineColor);
		slider.Disabled.TrackLineLightColor = colorBlendFactory.GetColor(slider.Default.TrackLineLightColor);
		table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(9474450);
		table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(15856627), factory.GetColor(13158858), 90);
		table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(16764057), factory.GetColor(16751464), 90);
		table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(13923901);
		table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(14145495), factory.GetColor(color), 90);
		table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(13923901);
		table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(13684944), factory.GetColor(10921638), 90);
		table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(10331049);
		table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(13684944), factory.GetColor(10921638), 90);
		table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(16777215);
		table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(15198183));
		table.DataGridView.RowNormalBorder = factory.GetColor(9474450);
		table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(16107413));
		table.DataGridView.RowSelectedBorder = factory.GetColor(13923901);
		table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15567694));
		table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(13923901);
		table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(12107716));
		table.DataGridView.RowMouseOverBorder = factory.GetColor(10331049);
		table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(12107716));
		table.DataGridView.RowPressedBorder = factory.GetColor(16777215);
		table.DataGridView.GridColor = factory.GetColor(13686757);
		table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(13027014));
		table.DataGridView.SelectorBorder = factory.GetColor(9474450);
		table.DataGridView.SelectorBorderDark = factory.GetColor(12829635);
		table.DataGridView.SelectorBorderLight = factory.GetColor(16382457);
		table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(16645629), factory.GetColor(15724527));
		table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(10592673));
		table.DataGridView.SelectorMouseOverBorder = factory.GetColor(9474450);
		table.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(12829635);
		table.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(16382457);
		table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(16645629), factory.GetColor(15724527));
		table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
		table.SideBar.Border = factory.GetColor(7303284);
		table.SideBar.SideBarPanelItemText = factory.GetColor(3355443);
		table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(15462138), 0f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14080740), 0.4f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(12961745), 0.4f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(13949154), 1f));
		table.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16505781), 0f));
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16697208), 0.4f));
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16692310), 0.4f));
		table.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(factory.GetColor(16640927), 1f));
		table.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16776409), 0f));
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770957), 0.4f));
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16766792), 0.4f));
		table.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(factory.GetColor(16770963), 1f));
		table.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16300137), 0f));
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16622433), 0.4f));
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16484924), 0.4f));
		table.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(factory.GetColor(16694112), 1f));
		table.WarningBox.BackColor = factory.GetColor(Color.FromArgb(255, 212, 216, 219));
		table.WarningBox.WarningBorderColor = factory.GetColor(Color.FromArgb(180, 180, 180));
		table.WarningBox.WarningBackColor1 = factory.GetColor(Color.FromArgb(255, 255, 255, 255));
		table.WarningBox.WarningBackColor2 = factory.GetColor(Color.FromArgb(255, 234, 234, 234));
		table.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(factory.GetColor(6384246)),
			new ColorDef(factory.GetColor(6384246)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14475234),
				factory.GetColor(13883098),
				factory.GetColor(11844289),
				factory.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(15264492)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(15264492)),
			new ColorDef(factory.GetColor(3560290)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(13619928),
				factory.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(11581118)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(16119285)),
			new ColorDef(factory.GetColor(15436032)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(factory.GetColor(15436032))
		};
		table.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(factory.GetColor(15790578)),
			new ColorDef(factory.GetColor(7303284)),
			new ColorDef(factory.GetColor(7303284)),
			new ColorDef(factory.GetColor(7303284)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(factory.GetColor(15436032))
		};
		table.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14475234),
				factory.GetColor(13883098),
				factory.GetColor(11844289),
				factory.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(factory.GetColor(6384246)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14475234),
				factory.GetColor(13817306),
				factory.GetColor(12041411),
				factory.GetColor(13291220)
			}, new float[4] { 0f, 0.6f, 0.6f, 1f }, 0f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14475234),
				factory.GetColor(13883098),
				factory.GetColor(11844289),
				factory.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(factory.GetColor(15264492)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(13619928),
				factory.GetColor(11581118)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(11581118)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(15436032)),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(16772473),
				factory.GetColor(16767083),
				factory.GetColor(16759552),
				factory.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(16777215))
		};
		table.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(factory.GetColor(4944290)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(16645887),
				factory.GetColor(12702698)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(11650540),
				factory.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(12773564),
				factory.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(16369568),
				factory.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(12957158),
				factory.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(15837868),
				factory.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(16776362),
				factory.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(-1)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(8388736))
		};
		table.SuperTab.Background = new SuperTabLinearGradientColorTable(factory.GetColor(13685981));
		table.SuperTab.InnerBorder = factory.GetColor(15727097);
		table.SuperTab.OuterBorder = factory.GetColor(12500670);
		table.SuperTab.ControlBoxDefault.Image = factory.GetColor(3355443);
		table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(16770978);
		table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(16760169);
		table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		table.SuperTab.ControlBoxPressed.Background = factory.GetColor(16485436);
		table.SuperTab.ControlBoxPressed.Border = factory.GetColor(16760169);
		table.SuperTab.ControlBoxPressed.Image = factory.GetColor(255, 0);
		table.SuperTab.InsertMarker = factory.GetColor(255, 128);
		table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		table.SuperTabItem.Default.Normal.Text = factory.GetColor(3355443);
		table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(3355443);
		table.SuperTabItem.Default.Disabled.Text = factory.GetColor(9276813);
		table.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		table.SuperTabItem.Default.Disabled.CloseMarker = factory.GetColor(9276813);
		table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15921908),
			factory.GetColor(14936047)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(15727097);
		table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(12500670);
		table.SuperTabItem.Default.Selected.Text = factory.GetColor(3355443);
		table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(3355443);
		table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15923194),
			factory.GetColor(14804719)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16444610);
		table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(15252338);
		table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(3355443);
		table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(3355443);
		table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(14406851),
			factory.GetColor(15322765)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(15264494);
		table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(12435137);
		table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(3355443);
		table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(3355443);
		table.SuperTabItem.Left = table.SuperTabItem.Default;
		table.SuperTabItem.Bottom = table.SuperTabItem.Default;
		table.SuperTabItem.Right = table.SuperTabItem.Default;
		table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15264494));
		table.SuperTabPanel.Default.InnerBorder = factory.GetColor(15727097);
		table.SuperTabPanel.Default.OuterBorder = factory.GetColor(12500670);
		table.SuperTabPanel.Left = table.SuperTabPanel.Default;
		table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
		table.SuperTabPanel.Right = table.SuperTabPanel.Default;
		SuperTabStyleColorFactory.GetOffice2010BackstageSilverColorTable(table.Backstage, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageSilverItemColorTable(table.BackstageItem, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageSilverPanelColorTable(table.BackstagePanel, factory);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = factory.GetColor(11121080);
		switchButtonColorTable.OffBackColor = factory.GetColor(15330803);
		switchButtonColorTable.OffTextColor = factory.GetColor(3355443);
		switchButtonColorTable.OnBackColor = factory.GetColor(9621584);
		switchButtonColorTable.OnTextColor = factory.GetColor(3355443);
		switchButtonColorTable.SwitchBackColor = factory.GetColor(13883362);
		switchButtonColorTable.SwitchBorderColor = factory.GetColor(9211021);
		switchButtonColorTable.TextColor = factory.GetColor(3355443);
		table.SwitchButton = new SwitchButtonColors();
		table.SwitchButton.Default = switchButtonColorTable;
		table.SwitchButton.Disabled.BorderColor = table.CheckBoxItem.Disabled.CheckBorder;
		table.SwitchButton.Disabled.SwitchBorderColor = table.SwitchButton.Disabled.BorderColor;
		table.SwitchButton.Disabled.OffTextColor = table.CheckBoxItem.Disabled.Text;
		table.SwitchButton.Disabled.OnTextColor = table.SwitchButton.Disabled.OffTextColor;
		table.SwitchButton.Disabled.TextColor = table.SwitchButton.Disabled.OffTextColor;
		table.SwitchButton.Disabled.SwitchBackColor = table.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		table.SwitchButton.Disabled.OffBackColor = table.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		table.SwitchButton.Disabled.OnBackColor = table.SwitchButton.Disabled.OffBackColor;
		table.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = factory.GetColor(11121080);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = factory.GetColor(15264492);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetFileMenuContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetTwoColumnMenuContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMenuColumnOneContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMenuColumnTwoContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMenuBottomContainer(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetTextBoxStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetRichTextBoxStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetItemPanelStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDateTimeInputBackgroundStyle(factory.GetColor(color), SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetRibbonClientPanelStyle(factory, eOffice2007ColorScheme.Silver);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetListViewBorderStyle(table.ListViewEx);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetStatusBarAltStyle(table.Bar);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetCrumbBarBackgroundStyle(factory.GetColor(Color.White), factory.GetColor("FFACAFB7"), factory.GetColor("FF5A5B5C"));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewDateTimeStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewNumericStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetDataGridViewIpAddressStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetSlideOutButtonStyle(table.CheckBoxItem.Default.CheckBorder);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMetroTilePanelStyle(factory.GetColor(Color.White));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMetroTileGroupStyle(factory.GetColor(Color.DarkGray));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = Office2007ColorTableFactory.GetMonthCalendarStyle(SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		table.StepIndicator.BackgroundColor = factory.GetColor(ColorFunctions.GetShade(table.Form.BackColor, 10));
		table.StepIndicator.IndicatorColor = factory.GetColor(10800168);
	}

	public static void InitializeBlackLegacyColors(ColorScheme c, ColorFactory factory)
	{
		c.BarBackground = factory.GetColor(15921913);
		c.BarBackground2 = factory.GetColor(10065845);
		c.BarStripeColor = factory.GetColor(7237007);
		c.BarCaptionBackground = factory.GetColor(8026521);
		c.BarCaptionBackground2 = factory.GetColor(6447481);
		c.BarCaptionInactiveBackground = factory.GetColor(16185336);
		c.BarCaptionInactiveBackground2 = factory.GetColor(14344166);
		c.BarCaptionInactiveText = factory.GetColor(3355443);
		c.BarCaptionText = factory.GetColor(16777215);
		c.BarFloatingBorder = factory.GetColor(8026521);
		c.BarPopupBackground = factory.GetColor(16644863);
		c.BarPopupBorder = factory.GetColor(8026521);
		c.ItemBackground = Color.Empty;
		c.ItemBackground2 = Color.Empty;
		c.ItemCheckedBackground = factory.GetColor(16764818);
		c.ItemCheckedBackground2 = factory.GetColor(16756553);
		c.ItemCheckedBorder = factory.GetColor(16755519);
		c.ItemCheckedText = factory.GetColor(0);
		c.ItemDisabledBackground = Color.Empty;
		c.ItemDisabledText = factory.GetColor(9276813);
		c.ItemExpandedShadow = Color.Empty;
		c.ItemExpandedBackground = factory.GetColor(15264241);
		c.ItemExpandedBackground2 = factory.GetColor(12237261);
		c.ItemExpandedText = factory.GetColor(0);
		c.ItemHotBackground = factory.GetColor(16774604);
		c.ItemHotBackground2 = factory.GetColor(16768900);
		c.ItemHotBorder = factory.GetColor(16760169);
		c.ItemHotText = factory.GetColor(0);
		c.ItemPressedBackground = factory.GetColor(16553789);
		c.ItemPressedBackground2 = factory.GetColor(16758878);
		c.ItemPressedBorder = factory.GetColor(16485436);
		c.ItemPressedText = factory.GetColor(0);
		c.ItemSeparator = Color.FromArgb(225, factory.GetColor(7237007));
		c.ItemSeparatorShade = Color.FromArgb(180, factory.GetColor(16777215));
		c.ItemText = factory.GetColor(0);
		c.MenuBackground = factory.GetColor(16644863);
		c.MenuBackground2 = Color.Empty;
		c.MenuBarBackground = factory.GetColor(15066606);
		c.MenuBorder = factory.GetColor(8158356);
		c.ItemExpandedBorder = c.MenuBorder;
		c.MenuSide = factory.GetColor(15724527);
		c.MenuSide2 = Color.Empty;
		c.MenuUnusedBackground = c.MenuBackground;
		c.MenuUnusedSide = factory.GetColor(15329769);
		c.MenuUnusedSide2 = Color.Empty;
		c.ItemDesignTimeBorder = Color.Black;
		c.BarDockedBorder = factory.GetColor(7303284);
		c.DockSiteBackColor = factory.GetColor(14145509);
		c.DockSiteBackColor2 = factory.GetColor(15987703);
		c.CustomizeBackground = factory.GetColor(11776712);
		c.CustomizeBackground2 = factory.GetColor(7763090);
		c.CustomizeText = factory.GetColor(0);
		c.PanelBackground = factory.GetColor(15790578);
		c.PanelBackground2 = factory.GetColor(12436169);
		c.PanelText = Color.Black;
		c.PanelBorder = factory.GetColor(7303284);
		c.ExplorerBarBackground = factory.GetColor(15987703);
		c.ExplorerBarBackground2 = factory.GetColor(14145509);
		c.SplitterBackground = c.PanelBackground;
		c.SplitterBackground2 = c.PanelBackground2;
		c.SplitterText = c.PanelText;
		c.SplitterBorder = c.PanelBorder;
	}
}
