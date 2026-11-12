using System;
using System.Drawing;
using Stimulsoft.Controls.Win.DotNetBar.Schedule;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007ColorTableFactory
{
	public static void SetBlueExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
	{
		Color color = factory.GetColor(5668273);
		Color color2 = factory.GetColor(15397625);
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

	public static void SetBlackExpandColors(Office2007ButtonItemColorTable ct, ColorFactory factory)
	{
		Color color = factory.GetColor(4605510);
		Color color2 = factory.GetColor(14408668);
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

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrange(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(1393291);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14536603), factory.GetColor(12625782));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777207), factory.GetColor(16773822));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776409), factory.GetColor(16770957));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16766792), factory.GetColor(16770963));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(132, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(14402420), factory.GetColor(14402419));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16773311), factory.GetColor(16774361));
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(1393291);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(6258614), factory.GetColor(12892584));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(11636829), factory.GetColor(16624913));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(16300137), factory.GetColor(16622433));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16484924), factory.GetColor(16694112));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16768166), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(11566411), factory.GetColor(9467987));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16433300), factory.GetColor(16767404));
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(5486);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(13350041), factory.GetColor(16770175));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(15454644), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(16505781), factory.GetColor(16697208));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(16692310), factory.GetColor(16640927));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16640927), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(5486);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(9339237), factory.GetColor(13025458));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(15052373), factory.GetColor(16764717));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(16569255), factory.GetColor(16427099));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(16289322), factory.GetColor(16507286));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16642480), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(11566411), factory.GetColor(9467987));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16433300), factory.GetColor(16767404));
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(5486);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueOrangeWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueOrange = GetButtonItemBlueOrange(factory);
		buttonItemBlueOrange.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14938111), factory.GetColor(12901887));
		buttonItemBlueOrange.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(11391487), factory.GetColor(12639231));
		buttonItemBlueOrange.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(7836601), Color.Empty);
		buttonItemBlueOrange.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueOrange.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(15266300), factory.GetColor(15331836));
		buttonItemBlueOrange.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(13820404), factory.GetColor(15463421));
		buttonItemBlueOrange.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(7836601), Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueOrange.Disabled.Text = factory.GetColor(9276813);
		return buttonItemBlueOrange;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueBlue(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(1393291);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(14741247));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(12639487), factory.GetColor(11128575));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(10931711), factory.GetColor(13296127));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(124, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(7640514), factory.GetColor(9482198));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14083059), factory.GetColor(8627664));
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(1393291);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(14741247));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(12967160), factory.GetColor(11127543));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(9484010), factory.GetColor(7640514));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(7640514), factory.GetColor(9482198));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14083059), factory.GetColor(8627664));
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(5486);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(5668272), factory.GetColor(5668272));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(9484010), factory.GetColor(7640514));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(5486);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(6391735), factory.GetColor(5014472));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(10663117), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(5471928), factory.GetColor(6915498));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(10141941), factory.GetColor(12179455));
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(5486);
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueBlueWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueBlue = GetButtonItemBlueBlue(factory);
		buttonItemBlueBlue.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14938111), factory.GetColor(12901887));
		buttonItemBlueBlue.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(11391487), factory.GetColor(12639231));
		buttonItemBlueBlue.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(7836601), Color.Empty);
		buttonItemBlueBlue.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueBlue.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(15266300), factory.GetColor(15331836));
		buttonItemBlueBlue.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(13820404), factory.GetColor(15463421));
		buttonItemBlueBlue.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(7836601), Color.Empty);
		buttonItemBlueBlue.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueBlue.Disabled.Text = factory.GetColor(9276813);
		return buttonItemBlueBlue;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueMagenta(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(1393291);
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
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(5486);
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
		SetBlueExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlueMagentaWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlueMagenta = GetButtonItemBlueMagenta(factory);
		buttonItemBlueMagenta.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14938111), factory.GetColor(12901887));
		buttonItemBlueMagenta.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(11391487), factory.GetColor(12639231));
		buttonItemBlueMagenta.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(7836601), Color.Empty);
		buttonItemBlueMagenta.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlueMagenta.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(15266300), factory.GetColor(15331836));
		buttonItemBlueMagenta.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(13820404), factory.GetColor(15463421));
		buttonItemBlueMagenta.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(7836601), Color.Empty);
		buttonItemBlueMagenta.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlueMagenta.Disabled.Text = factory.GetColor(9276813);
		return buttonItemBlueMagenta;
	}

	public static Office2007ButtonItemColorTable GetButtonItemOffice2007WithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(13365499), factory.GetColor(7322597));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(6731231), factory.GetColor(10677750));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(11664639), Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.White, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(7970230), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.WhiteSmoke), factory.GetColor(Color.LightGray));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(Color.Silver), factory.GetColor(15463421));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.WhiteSmoke, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(7970230), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = factory.GetColor(9276813);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16579042), factory.GetColor(16510399));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16439370), factory.GetColor(16573845));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(Color.White, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12560249), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(12560249), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(16300664), factory.GetColor(16560718));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16555023), factory.GetColor(16562743));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(15837002), Color.Empty);
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(16579042), factory.GetColor(16510399));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(16439370), factory.GetColor(16573845));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.White, Color.Empty);
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(15837002), Color.Empty);
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(16562743), factory.GetColor(16555023));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(16560718), factory.GetColor(16300664));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(15837002), Color.Empty);
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarBlue(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(12964575), factory.GetColor(10403803));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(15199991), factory.GetColor(15857661));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(14608629), factory.GetColor(13754352));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(13097197), factory.GetColor(14215413));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(12769521), factory.GetColor(12638447));
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(4090538);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarBlueMouseOver(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(11388894), factory.GetColor(8302035));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(15003645), factory.GetColor(15266044));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(14478075), factory.GetColor(14477560));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(13164799), factory.GetColor(14085631));
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(4090553);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarBlueExpanded(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(8821678), factory.GetColor(11651033));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(11387356), factory.GetColor(8955856));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(7772616), factory.GetColor(11851261));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(12835314), Color.Empty);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueDefault(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(15791870), factory.GetColor(14411509));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16317439), factory.GetColor(12581631));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(9943526), factory.GetColor(9286371));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(15463422), factory.GetColor(14805750));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16444870), factory.GetColor(16777149));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14138506), factory.GetColor(16699742));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(13490912), factory.GetColor(14995607));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(12902143), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14675455), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10009577), factory.GetColor(10009577));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueMagenta(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13945058), factory.GetColor(15525874));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(13484767));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12499649), factory.GetColor(12632001));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13945058), factory.GetColor(15525874));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(15525875), factory.GetColor(13484767));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12499649), factory.GetColor(12632001));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(12899832), factory.GetColor(12762344));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14741247), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueGreen(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(14019283), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12173751), factory.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14019283), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12173751), factory.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(11788003), factory.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14872575), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlueOrange(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289388), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289388), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(13623521), factory.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(14872575), factory.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12699857), factory.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueDefault(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(13483991));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(13658313)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(1393291);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueMagenta(ColorFactory factory)
	{
		return GetRibbonTabGroupBlueDefault(factory);
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueGreen(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(10936974));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(7059796)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(1393291);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlueOrange(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(15527849));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(16770585)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(1393291);
		return office2007RibbonTabGroupColorTable;
	}

	public static SuperTabColorTable GetSuperTabBlueDefault(ColorFactory factory)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(factory.GetColor(12573695));
		superTabColorTable.InnerBorder = factory.GetColor(12581631);
		superTabColorTable.OuterBorder = factory.GetColor(9286371);
		superTabColorTable.ControlBoxDefault.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = factory.GetColor(16770978);
		superTabColorTable.ControlBoxMouseOver.Border = factory.GetColor(16760169);
		superTabColorTable.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = factory.GetColor(16485436);
		superTabColorTable.ControlBoxPressed.Border = factory.GetColor(16760169);
		superTabColorTable.ControlBoxPressed.Image = factory.GetColor(255, 0);
		superTabColorTable.InsertMarker = factory.GetColor(255, 128);
		return superTabColorTable;
	}

	public static SuperTabItemColorTable GetSuperTabItemBlueDefault(ColorFactory factory)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		superTabItemColorTable.Default.Normal.Text = factory.GetColor(1393291);
		superTabItemColorTable.Default.Normal.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15791870),
			factory.GetColor(14411509)
		}, new float[4] { 0f, 0.15f, 0.15f, 1f });
		superTabItemColorTable.Default.Selected.InnerBorder = factory.GetColor(12581631);
		superTabItemColorTable.Default.Selected.OuterBorder = factory.GetColor(9286371);
		superTabItemColorTable.Default.Selected.Text = factory.GetColor(1393291);
		superTabItemColorTable.Default.Selected.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15463422),
			factory.GetColor(14805750)
		}, new float[4] { 0f, 0.07f, 0.07f, 1f });
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16777149);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = factory.GetColor(14137215);
		superTabItemColorTable.Default.SelectedMouseOver.Text = factory.GetColor(1393291);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(factory.GetColor(12573695), factory.GetColor(14930328));
		superTabItemColorTable.Default.MouseOver.InnerBorder = factory.GetColor(15726335);
		superTabItemColorTable.Default.MouseOver.OuterBorder = factory.GetColor(10075112);
		superTabItemColorTable.Default.MouseOver.Text = factory.GetColor(1393291);
		superTabItemColorTable.Default.MouseOver.CloseMarker = factory.GetColor(4222879);
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	public static SuperTabPanelColorTable GetSuperTabPanelBlueDefault(ColorFactory factory)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(15791870), factory.GetColor(14411509));
		superTabPanelColorTable.Default.InnerBorder = factory.GetColor(12581631);
		superTabPanelColorTable.Default.OuterBorder = factory.GetColor(9286371);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	public static SuperTabColorTable GetBackstageBlueDefault(ColorFactory factory)
	{
		return SuperTabStyleColorFactory.GetOffice2010BackstageBlueColorTable(null, factory);
	}

	public static SuperTabItemColorTable GetBackstageItemBlueDefault(ColorFactory factory)
	{
		return SuperTabStyleColorFactory.GetOffice2010BackstageBlueItemColorTable(null, factory);
	}

	public static SuperTabPanelColorTable GetBackstagePanelBlueDefault(ColorFactory factory)
	{
		return SuperTabStyleColorFactory.GetOffice2010BackstageBluePanelColorTable(null, factory);
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarBlack(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(11448500), factory.GetColor(8487297));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(15330286), factory.GetColor(15461355));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(13554650), factory.GetColor(12699343));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(11844549), factory.GetColor(15068396));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(11974840), factory.GetColor(10264222));
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(16777215);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarBlackMouseOver(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(11448500), factory.GetColor(8487297));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(factory.GetColor(16119543), factory.GetColor(15857142));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(15527664), factory.GetColor(15198700));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(14935529), factory.GetColor(16054006));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(11185067), factory.GetColor(7171694));
		office2007RibbonBarStateColorTable.TitleText = factory.GetColor(16777215);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable GetRibbonBarBlackExpanded(ColorFactory factory)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(factory.GetColor(8158332), factory.GetColor(12237498));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(factory.GetColor(12632256), factory.GetColor(11383224));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(factory.GetColor(10396588), factory.GetColor(14014424));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(factory.GetColor(12763842), Color.Empty);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackDefault(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(15658735), factory.GetColor(13554393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(15988213), factory.GetColor(12121852));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12500670), factory.GetColor(12500670));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(15593199), factory.GetColor(13291478));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16444354), factory.GetColor(16777149));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(15252338), factory.GetColor(16699742));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(9737106), factory.GetColor(9339471));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(15581735), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(9934485), factory.GetColor(9667917));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(9868949), factory.GetColor(11577734));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackMagenta(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(13945058), factory.GetColor(15459826));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(14801387), factory.GetColor(13484767));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(12368321), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(13945058), factory.GetColor(15459826));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(13022937), factory.GetColor(13484767));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(12368321), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(9145745), factory.GetColor(8352917));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(9008797), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(9079695), factory.GetColor(8352917));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10066074), factory.GetColor(10526370));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackGreen(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(12706488), factory.GetColor(14938589));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(15004640), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(11583660), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(12706488), factory.GetColor(14938589));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(10802326), factory.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(11583660), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(9015431), factory.GetColor(6656600));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(7118172), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(8883334), factory.GetColor(6656599));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(9935255), factory.GetColor(10331035));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable GetRibbonTabItemBlackOrange(ColorFactory factory)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = factory.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(factory.GetColor(16776150), factory.GetColor(16577429));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289387), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(factory.GetColor(16774559), factory.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16773233), factory.GetColor(16577429));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(13289387), factory.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = factory.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(factory.GetColor(9605251), factory.GetColor(11970867));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(factory.GetColor(12760369), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(9407875), factory.GetColor(11641907));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(10263446), factory.GetColor(12500670));
		office2007RibbonTabItemColorTable.MouseOver.Text = factory.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlackOrange(bool ribbonBar, ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(14536603), factory.GetColor(12626039));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777207), factory.GetColor(16775062));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776409), factory.GetColor(16770957));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16766792), factory.GetColor(16770963));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(14402420), factory.GetColor(13155212));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16773311), factory.GetColor(16774879));
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(9139796), factory.GetColor(12892584));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(15312993), factory.GetColor(16624913));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(16299370), factory.GetColor(16556128));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(16484924), factory.GetColor(16694626));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16769199), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(11566411), factory.GetColor(9467987));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16433300), factory.GetColor(16767404));
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(10450265), factory.GetColor(16764717));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(15454644), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(16505781), factory.GetColor(16697208));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(16692310), factory.GetColor(16640927));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16774559), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(9339237), factory.GetColor(13025458));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(15052373), factory.GetColor(16766538));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(16569255), factory.GetColor(16427099));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(16289065), factory.GetColor(16639129));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(factory.GetColor(16774857), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(11566411), factory.GetColor(9139796));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(16433300), factory.GetColor(16767147));
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(0);
		SetBlackExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlackOrangeWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlackOrange = GetButtonItemBlackOrange(ribbonBar: false, factory);
		buttonItemBlackOrange.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(16316665), factory.GetColor(14672612));
		buttonItemBlackOrange.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(13093841), factory.GetColor(14409442));
		buttonItemBlackOrange.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		buttonItemBlackOrange.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9013125), Color.Empty);
		buttonItemBlackOrange.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlackOrange.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(13554650), factory.GetColor(12699343));
		buttonItemBlackOrange.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(11844549), factory.GetColor(15068396));
		buttonItemBlackOrange.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(12964318), Color.Empty);
		buttonItemBlackOrange.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackOrange.Disabled.Text = factory.GetColor(9276813);
		return buttonItemBlackOrange;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlackBlue(bool ribbonBar, ColorFactory factory)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(14741247));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(12639487), factory.GetColor(11128575));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(10931711), factory.GetColor(13296127));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(124, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(factory.GetColor(7640514), factory.GetColor(9482198));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14083059), factory.GetColor(8627664));
		office2007ButtonItemColorTable.MouseOver.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(16777211), factory.GetColor(14741247));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(12967160), factory.GetColor(11127543));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(9484010), factory.GetColor(7640514));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(factory.GetColor(7640514), factory.GetColor(9482198));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(14083059), factory.GetColor(8627664));
		office2007ButtonItemColorTable.Pressed.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(factory.GetColor(5668272), factory.GetColor(5668272));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(factory.GetColor(9484010), factory.GetColor(7640514));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = factory.GetColor(0);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(factory.GetColor(6391735), factory.GetColor(5014472));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(factory.GetColor(10663117), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(factory.GetColor(14149369), factory.GetColor(13098232));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(factory.GetColor(11784437), factory.GetColor(14149111));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(factory.GetColor(5471928), factory.GetColor(6915498));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(factory.GetColor(10141941), factory.GetColor(12179455));
		office2007ButtonItemColorTable.Expanded.Text = factory.GetColor(0);
		SetBlackExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlackBlueWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlackBlue = GetButtonItemBlackBlue(ribbonBar: false, factory);
		buttonItemBlackBlue.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(16316665), factory.GetColor(14672612));
		buttonItemBlackBlue.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(13093841), factory.GetColor(14409442));
		buttonItemBlackBlue.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		buttonItemBlackBlue.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9013125), Color.Empty);
		buttonItemBlackBlue.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlackBlue.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(13554650), factory.GetColor(12699343));
		buttonItemBlackBlue.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(11844549), factory.GetColor(15068396));
		buttonItemBlackBlue.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(12964318), Color.Empty);
		buttonItemBlackBlue.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackBlue.Disabled.Text = factory.GetColor(9276813);
		return buttonItemBlackBlue;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlackMagenta(bool ribbonBar, ColorFactory factory)
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
		SetBlackExpandColors(office2007ButtonItemColorTable, factory);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable GetButtonItemBlackMagentaWithBackground(ColorFactory factory)
	{
		Office2007ButtonItemColorTable buttonItemBlackMagenta = GetButtonItemBlackMagenta(ribbonBar: false, factory);
		buttonItemBlackMagenta.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(16316665), factory.GetColor(14672612));
		buttonItemBlackMagenta.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(13093841), factory.GetColor(14409442));
		buttonItemBlackMagenta.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		buttonItemBlackMagenta.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Default.OuterBorder = new LinearGradientColorTable(factory.GetColor(9013125), Color.Empty);
		buttonItemBlackMagenta.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Disabled = new Office2007ButtonItemStateColorTable();
		buttonItemBlackMagenta.Disabled.TopBackground = new LinearGradientColorTable(factory.GetColor(13554650), factory.GetColor(12699343));
		buttonItemBlackMagenta.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Disabled.BottomBackground = new LinearGradientColorTable(factory.GetColor(11844549), factory.GetColor(15068396));
		buttonItemBlackMagenta.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Disabled.OuterBorder = new LinearGradientColorTable(factory.GetColor(4934475), Color.Empty);
		buttonItemBlackMagenta.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		buttonItemBlackMagenta.Disabled.Text = factory.GetColor(9276813);
		return buttonItemBlackMagenta;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackDefault(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(10526880));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(13487565)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(16777215);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackMagenta(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(9465999));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(11500962)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(16777215);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackGreen(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(7249757));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, factory.GetColor(7059796)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(16777215);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable GetRibbonTabGroupBlackOrange(ColorFactory factory)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, factory.GetColor(10918721));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(255, factory.GetColor(16769792)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = factory.GetColor(16777215);
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
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor(12571114), 0f),
			new BackgroundColorBlend(factory.GetColor(13295610), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11193334), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11193334), 0.7f),
			new BackgroundColorBlend(factory.GetColor(13886714), 1f)
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

	public static void InitializeAppBlueScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(9810130), factory.GetColor(8166337), 0);
		@default.Border = new LinearGradientColorTable(factory.GetColor(11718902));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[6]
		{
			new BackgroundColorBlend(factory.GetColor(14935527), 0f),
			new BackgroundColorBlend(factory.GetColor(14936560), 0.4f),
			new BackgroundColorBlend(factory.GetColor(13161697), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11977944), 0.8f),
			new BackgroundColorBlend(factory.GetColor(11977944), 0.8f),
			new BackgroundColorBlend(factory.GetColor(14804975), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(14935527), factory.GetColor(14804975));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(5859729), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(9146000)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.MouseOver;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor(12571114), 0f),
			new BackgroundColorBlend(factory.GetColor(13295610), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11193334), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11193334), 0.7f),
			new BackgroundColorBlend(factory.GetColor(13886714), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(15329771), factory.GetColor(16580095));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(3960496), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(8226450)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.MouseOverControl;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(5596557), factory.GetColor(3357780));
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = t.AppScrollBar.Default.TrackSignBackground;
		@default = t.AppScrollBar.Pressed;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor(10337767), 0f),
			new BackgroundColorBlend(factory.GetColor(11259126), 0.4f),
			new BackgroundColorBlend(factory.GetColor(7251696), 0.4f),
			new BackgroundColorBlend(factory.GetColor(7251696), 0.7f),
			new BackgroundColorBlend(factory.GetColor(11915767), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(12768231), factory.GetColor(14543867));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(1526154), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(7241382), factory.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(12768231), factory.GetColor(14543867));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(1526154), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(8487297)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.Disabled;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(12570615), factory.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void InitializeAppSilverScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(11843519));
		@default.Border = new LinearGradientColorTable(factory.GetColor(10330022));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8225934), factory.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16119286), 0f),
			new BackgroundColorBlend(factory.GetColor(15395564), 0.4f),
			new BackgroundColorBlend(factory.GetColor(14343133), 0.4f),
			new BackgroundColorBlend(factory.GetColor(12698053), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(16382457), factory.GetColor(13619154));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(7237491), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.MouseOver;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor(14479611), 0f),
			new BackgroundColorBlend(factory.GetColor(13232634), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11131894), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11131894), 0.7f),
			new BackgroundColorBlend(factory.GetColor(9748696), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(16645887), factory.GetColor(14409183));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(3964849), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8225934), factory.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.MouseOverControl;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8225934), factory.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = t.AppScrollBar.Default.TrackSignBackground;
		@default = t.AppScrollBar.Pressed;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor(12904953), 0f),
			new BackgroundColorBlend(factory.GetColor(10870518), 0.4f),
			new BackgroundColorBlend(factory.GetColor(7326448), 0.4f),
			new BackgroundColorBlend(factory.GetColor(7326448), 0.7f),
			new BackgroundColorBlend(factory.GetColor(6468051), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(14939644), factory.GetColor(10405849));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(1595786), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8225934), factory.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(14939644), factory.GetColor(10405849));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(1595786), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(8487297)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.Disabled;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(12570615), factory.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void InitializeAppBlackScrollBarColorTable(Office2007ColorTable t, ColorFactory factory)
	{
		Office2007ScrollBarStateColorTable @default = t.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(factory.GetColor(4210752));
		@default.Border = new LinearGradientColorTable(factory.GetColor(6447714));
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(13290186), factory.GetColor(7960953), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(16382457), 0f),
			new BackgroundColorBlend(factory.GetColor(15395564), 0.4f),
			new BackgroundColorBlend(factory.GetColor(14343133), 0.4f),
			new BackgroundColorBlend(factory.GetColor(12698053), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(16382457), factory.GetColor(13619154));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(3092271), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.MouseOver;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor(14479611), 0f),
			new BackgroundColorBlend(factory.GetColor(13232634), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11131894), 0.4f),
			new BackgroundColorBlend(factory.GetColor(11131894), 0.7f),
			new BackgroundColorBlend(factory.GetColor(9748696), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(16645887), factory.GetColor(14409183));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(3964849), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8618883), factory.GetColor(5131854), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.MouseOverControl;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8618883), factory.GetColor(5131854), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(t.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = t.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = t.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = t.AppScrollBar.Default.TrackSignBackground;
		@default = t.AppScrollBar.Pressed;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor(12904953), 0f),
			new BackgroundColorBlend(factory.GetColor(10870518), 0.4f),
			new BackgroundColorBlend(factory.GetColor(7326448), 0.4f),
			new BackgroundColorBlend(factory.GetColor(7326448), 0.7f),
			new BackgroundColorBlend(factory.GetColor(6468051), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(factory.GetColor(14939644), factory.GetColor(10405849));
		@default.ThumbOuterBorder = new LinearGradientColorTable(factory.GetColor(1595786), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(8225934), factory.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = new LinearGradientColorTable(factory.GetColor(14939644), factory.GetColor(10405849));
		@default.TrackOuterBorder = new LinearGradientColorTable(factory.GetColor(1595786), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, factory.GetColor(8487297)), Color.FromArgb(64, Color.White));
		@default = t.AppScrollBar.Disabled;
		@default.Background = t.AppScrollBar.Default.Background;
		@default.Border = t.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(factory.GetColor(12570615), factory.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void InitializeVistaBlackColorTable(Office2007ColorTable table, ColorFactory factory)
	{
		table.RibbonBar.Default = GetRibbonBarBlack(factory);
		table.RibbonBar.MouseOver = GetRibbonBarBlackMouseOver(factory);
		table.RibbonBar.Expanded = GetRibbonBarBlackExpanded(factory);
		table.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable ribbonTabItemBlackDefault = GetRibbonTabItemBlackDefault(factory);
		ribbonTabItemBlackDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		table.RibbonTabItemColors.Add(ribbonTabItemBlackDefault);
		ribbonTabItemBlackDefault = GetRibbonTabItemBlackGreen(factory);
		ribbonTabItemBlackDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		table.RibbonTabItemColors.Add(ribbonTabItemBlackDefault);
		ribbonTabItemBlackDefault = GetRibbonTabItemBlackMagenta(factory);
		ribbonTabItemBlackDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		table.RibbonTabItemColors.Add(ribbonTabItemBlackDefault);
		ribbonTabItemBlackDefault = GetRibbonTabItemBlackOrange(factory);
		ribbonTabItemBlackDefault.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		table.RibbonTabItemColors.Add(ribbonTabItemBlackDefault);
		table.RibbonControl = new Office2007RibbonColorTable();
		table.RibbonControl.TabsBackground = new LinearGradientColorTable(factory.GetColor(5460819), factory.GetColor(5460819));
		table.RibbonControl.InnerBorder = new LinearGradientColorTable(factory.GetColor(13554393), factory.GetColor(12500670));
		table.RibbonControl.OuterBorder = new LinearGradientColorTable(factory.GetColor(5460819), factory.GetColor(5197647));
		table.RibbonControl.TabDividerBorder = factory.GetColor(4276545);
		table.RibbonControl.TabDividerBorderLight = factory.GetColor(6710886);
		table.RibbonControl.PanelTopBackground = new LinearGradientColorTable(factory.GetColor(14146528), factory.GetColor(12699343));
		table.RibbonControl.PanelBottomBackground = new LinearGradientColorTable(factory.GetColor(11844549), factory.GetColor(15461355));
		table.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonNormalBlack.png");
		table.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonHotBlack.png");
		table.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.LoadBitmap("SystemImages.BlankStartButtonPressedBlack.png");
		table.ItemGroup.TopBackground = new LinearGradientColorTable(factory.GetColor(14081759), factory.GetColor(14410468));
		table.ItemGroup.BottomBackground = new LinearGradientColorTable(factory.GetColor(13883610), factory.GetColor(14738919));
		table.ItemGroup.InnerBorder = new LinearGradientColorTable(factory.GetColor(14673638), factory.GetColor(15593713));
		table.ItemGroup.OuterBorder = new LinearGradientColorTable(factory.GetColor(11384763), factory.GetColor(9213338));
		table.ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, factory.GetColor(13553358));
		table.ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, factory.GetColor(16777215));
		table.Bar.ToolbarTopBackground = new LinearGradientColorTable(factory.GetColor(3355443), factory.GetColor(3882566));
		table.Bar.ToolbarBottomBackground = new LinearGradientColorTable(factory.GetColor(3092528), factory.GetColor(4539717));
		table.Bar.ToolbarBottomBorder = factory.GetColor(5000268);
		table.Bar.PopupToolbarBackground = new LinearGradientColorTable(factory.GetColor(16448250), Color.Empty);
		table.Bar.PopupToolbarBorder = factory.GetColor(3092271);
		table.Bar.StatusBarTopBorder = factory.GetColor(3355443);
		table.Bar.StatusBarTopBorderLight = Color.Empty;
		table.Bar.StatusBarAltBackground.Clear();
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(13026758), 0f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(11842740), 0.4f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(9935000), 0.4f));
		table.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(factory.GetColor(4738130), 1f));
		table.ButtonItemColors.Clear();
		table.RibbonButtonItemColors.Clear();
		table.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable buttonItemBlackOrange = GetButtonItemBlackOrange(ribbonBar: false, factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		table.ButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackOrangeWithBackground(factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		table.ButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackBlue(ribbonBar: false, factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		table.ButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackBlueWithBackground(factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		table.ButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackMagenta(ribbonBar: false, factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		table.ButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackMagentaWithBackground(factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.ButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackOrange(ribbonBar: true, factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		table.RibbonButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackOrangeWithBackground(factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		table.RibbonButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackBlue(ribbonBar: true, factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		table.RibbonButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackBlueWithBackground(factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		table.RibbonButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackMagenta(ribbonBar: true, factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		table.RibbonButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackMagentaWithBackground(factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		table.RibbonButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemBlackOrange(ribbonBar: true, factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		buttonItemBlackOrange.Default.Text = factory.GetColor(16777215);
		buttonItemBlackOrange.MouseOver.Text = factory.GetColor(0);
		buttonItemBlackOrange.Checked.Text = factory.GetColor(0);
		buttonItemBlackOrange.Expanded.Text = factory.GetColor(0);
		buttonItemBlackOrange.Pressed.Text = factory.GetColor(0);
		table.MenuButtonItemColors.Add(buttonItemBlackOrange);
		buttonItemBlackOrange = GetButtonItemOffice2007WithBackground(factory);
		buttonItemBlackOrange.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		table.ButtonItemColors.Add(buttonItemBlackOrange);
		table.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(factory));
		table.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable ribbonTabGroupBlackDefault = GetRibbonTabGroupBlackDefault(factory);
		ribbonTabGroupBlackDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		table.RibbonTabGroupColors.Add(ribbonTabGroupBlackDefault);
		ribbonTabGroupBlackDefault = GetRibbonTabGroupBlackMagenta(factory);
		ribbonTabGroupBlackDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		table.RibbonTabGroupColors.Add(ribbonTabGroupBlackDefault);
		ribbonTabGroupBlackDefault = GetRibbonTabGroupBlackGreen(factory);
		ribbonTabGroupBlackDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		table.RibbonTabGroupColors.Add(ribbonTabGroupBlackDefault);
		ribbonTabGroupBlackDefault = GetRibbonTabGroupBlackOrange(factory);
		ribbonTabGroupBlackDefault.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		table.RibbonTabGroupColors.Add(ribbonTabGroupBlackDefault);
		table.Menu.Background = new LinearGradientColorTable(factory.GetColor(16448250), Color.Empty);
		table.Menu.Border = new LinearGradientColorTable(factory.GetColor(8816262), Color.Empty);
		table.Menu.Side = new LinearGradientColorTable(factory.GetColor(15331054), Color.Empty);
		table.Menu.SideBorder = new LinearGradientColorTable(factory.GetColor(12961221), Color.Empty);
		table.Menu.SideBorderLight = new LinearGradientColorTable(factory.GetColor(16119285), Color.Empty);
		table.Menu.SideUnused = new LinearGradientColorTable(factory.GetColor(15066597), Color.Empty);
		table.Menu.FileBackgroundBlend.Clear();
		table.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(factory.GetColor(5328976), 0f),
			new BackgroundColorBlend(factory.GetColor(5131855), 1f)
		});
		table.Menu.FileContainerBorder = factory.GetColor(7040113);
		table.Menu.FileContainerBorderLight = factory.GetColor(7040113);
		table.Menu.FileColumnOneBackground = factory.GetColor(16448250);
		table.Menu.FileColumnOneBorder = factory.GetColor(12961221);
		table.Menu.FileColumnTwoBackground = factory.GetColor(15330030);
		table.Menu.FileBottomContainerBackgroundBlend.Clear();
		table.Menu.FileBottomContainerBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor(4408914), 0f),
			new BackgroundColorBlend(factory.GetColor(3948618), 0.4f),
			new BackgroundColorBlend(factory.GetColor(3092271), 0.4f),
			new BackgroundColorBlend(factory.GetColor(4210752), 1f)
		});
		table.ComboBox.Default.Background = factory.GetColor(15263976);
		int color = 9013641;
		table.ComboBox.Default.Border = factory.GetColor(color);
		table.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		table.ComboBox.Default.ExpandText = factory.GetColor(8158332);
		table.ComboBox.DefaultStandalone.Background = factory.GetColor(16777215);
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
		table.ComboBox.DroppedDown.Border = factory.GetColor(color);
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
		table.SystemButton.Default.Foreground = new LinearGradientColorTable(factory.GetColor(8686741), factory.GetColor(10463671));
		table.SystemButton.Default.LightShade = factory.GetColor(10463671);
		table.SystemButton.Default.DarkShade = factory.GetColor(8487040);
		table.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		table.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(factory.GetColor(3552565), factory.GetColor(5199715));
		table.SystemButton.MouseOver.LightShade = factory.GetColor(10199464);
		table.SystemButton.MouseOver.DarkShade = factory.GetColor(4541526);
		table.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(11647169), factory.GetColor(8950945));
		table.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(6845829), factory.GetColor(10334657));
		table.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(factory.GetColor(13489114), Color.Transparent);
		table.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(factory.GetColor(11918836), Color.Transparent);
		table.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(factory.GetColor(5726317), factory.GetColor(6054505));
		table.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(factory.GetColor(13489114), factory.GetColor(14344163));
		table.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		table.SystemButton.Pressed.Foreground = new LinearGradientColorTable(factory.GetColor(7171180), factory.GetColor(9148068));
		table.SystemButton.Pressed.LightShade = factory.GetColor(8358036);
		table.SystemButton.Pressed.DarkShade = factory.GetColor(7171180);
		table.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(factory.GetColor(3618615), factory.GetColor(2829099));
		table.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(factory.GetColor(6842472), Color.Transparent);
		table.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(factory.GetColor(0), factory.GetColor(461067));
		table.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(factory.GetColor(5335426), Color.Transparent);
		table.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(factory.GetColor(2237481), factory.GetColor(1644825));
		table.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(factory.GetColor(4605510), factory.GetColor(3355443));
		table.Form.Active.BorderColors = new Color[5]
		{
			factory.GetColor(3092271),
			factory.GetColor(5066061),
			factory.GetColor(5460819),
			factory.GetColor(5460819),
			factory.GetColor(5460819)
		};
		table.Form.Inactive.BorderColors = new Color[5]
		{
			factory.GetColor(9605778),
			factory.GetColor(10461087),
			factory.GetColor(11250603),
			factory.GetColor(5460819),
			factory.GetColor(5460819)
		};
		table.Form.Active.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(4409170), factory.GetColor(3816773));
		table.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(3092528), factory.GetColor(4079166));
		table.Form.Active.CaptionBottomBorder = new Color[2]
		{
			factory.GetColor(4737096),
			factory.GetColor(4868682)
		};
		table.Form.Active.CaptionText = factory.GetColor(11457023);
		table.Form.Active.CaptionTextExtra = factory.GetColor(16777215);
		table.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(factory.GetColor(10132641), factory.GetColor(9935004));
		table.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(factory.GetColor(9605778), factory.GetColor(10132122));
		table.Form.Inactive.CaptionText = factory.GetColor(14803425);
		table.Form.Inactive.CaptionTextExtra = factory.GetColor(14803425);
		table.Form.BackColor = factory.GetColor(8224125);
		table.Form.TextColor = factory.GetColor(16777215);
		table.QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(factory.GetColor(8093316), factory.GetColor(7961472));
		table.QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(factory.GetColor(7171694), factory.GetColor(4473924));
		table.QuickAccessToolbar.Active.OutterBorderColor = factory.GetColor(4671303);
		table.QuickAccessToolbar.Active.MiddleBorderColor = factory.GetColor(3487029);
		table.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(factory.GetColor(10987434), factory.GetColor(11776950));
		table.QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(factory.GetColor(11382189), factory.GetColor(9803157));
		table.QuickAccessToolbar.Inactive.OutterBorderColor = factory.GetColor(11119017);
		table.QuickAccessToolbar.Inactive.MiddleBorderColor = factory.GetColor(6118749);
		table.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		table.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(factory.GetColor(9277587), factory.GetColor(8750985));
		table.QuickAccessToolbar.Standalone.OutterBorderColor = factory.GetColor(6119524);
		table.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		table.QuickAccessToolbar.Standalone.InnerBorderColor = factory.GetColor(13356238);
		table.QuickAccessToolbar.QatCustomizeMenuLabelBackground = factory.GetColor(15461355);
		table.QuickAccessToolbar.QatCustomizeMenuLabelText = factory.GetColor(4605510);
		table.QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		table.QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(factory.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		table.TabControl.Default = new Office2007TabItemStateColorTable();
		table.TabControl.Default.TopBackground = new LinearGradientColorTable(factory.GetColor(14212322), factory.GetColor(11845063));
		table.TabControl.Default.BottomBackground = new LinearGradientColorTable(factory.GetColor(13949407), factory.GetColor(15461355));
		table.TabControl.Default.InnerBorder = factory.GetColor(15461355);
		table.TabControl.Default.OuterBorder = factory.GetColor(4605510);
		table.TabControl.Default.Text = factory.GetColor(3158064);
		table.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		table.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(factory.GetColor(16776683), factory.GetColor(16772264));
		table.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(factory.GetColor(16767577), factory.GetColor(16770701));
		table.TabControl.MouseOver.InnerBorder = factory.GetColor(16777211);
		table.TabControl.MouseOver.OuterBorder = factory.GetColor(11967859);
		table.TabControl.MouseOver.Text = factory.GetColor(3552822);
		table.TabControl.Selected = new Office2007TabItemStateColorTable();
		table.TabControl.Selected.TopBackground = new LinearGradientColorTable(factory.GetColor(Color.White), factory.GetColor(16250871));
		table.TabControl.Selected.BottomBackground = new LinearGradientColorTable(factory.GetColor(16250871), factory.GetColor(16250871));
		table.TabControl.Selected.InnerBorder = factory.GetColor(Color.White);
		table.TabControl.Selected.OuterBorder = factory.GetColor(4605510);
		table.TabControl.Selected.Text = factory.GetColor(2565927);
		table.TabControl.TabBackground = new LinearGradientColorTable(factory.GetColor(16645629), factory.GetColor(12829635));
		table.TabControl.TabPanelBackground = new LinearGradientColorTable(factory.GetColor(16250871), factory.GetColor(12829635));
		table.TabControl.TabPanelBorder = factory.GetColor(4605510);
		Office2007CheckBoxColorTable checkBoxItem = table.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(factory.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = factory.GetColor(8684676);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(10661049)), Color.FromArgb(164, factory.GetColor(16185078)));
		checkBoxItem.Default.CheckInnerBorder = factory.GetColor(10661049);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(factory.GetColor(5204093), Color.Empty);
		checkBoxItem.Default.Text = factory.GetColor(0);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(factory.GetColor(16053492), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = factory.GetColor(8684676);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(16438650)), Color.FromArgb(128, factory.GetColor(16709863)));
		checkBoxItem.MouseOver.CheckInnerBorder = factory.GetColor(16438650);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(factory.GetColor(5204093), Color.Empty);
		checkBoxItem.MouseOver.Text = factory.GetColor(0);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(factory.GetColor(15068407), Color.Empty);
		checkBoxItem.Pressed.CheckBorder = factory.GetColor(8684676);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(15894822)), Color.FromArgb(164, factory.GetColor(16774357)));
		checkBoxItem.Pressed.CheckInnerBorder = factory.GetColor(15894822);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(factory.GetColor(5204093), Color.Empty);
		checkBoxItem.Pressed.Text = factory.GetColor(0);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(factory.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = factory.GetColor(11448757);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, factory.GetColor(14738149)), Color.FromArgb(164, factory.GetColor(16514043)));
		checkBoxItem.Disabled.CheckInnerBorder = factory.GetColor(14738149);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(factory.GetColor(9276813), Color.Empty);
		checkBoxItem.Disabled.Text = factory.GetColor(9276813);
		InitializeScrollBarColorTable(table, factory);
		InitializeAppBlackScrollBarColorTable(table, factory);
		Office2007ProgressBarColorTable progressBarItem = table.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = factory.GetColor(13421772);
		progressBarItem.InnerBorder = factory.GetColor(2434341);
		progressBarItem.Chunk = new GradientColorTable(6788896, 12779350, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(164, factory.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, factory.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, factory.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(8750469, 9474448, 0);
		progressBarItem = table.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = factory.GetColor(13421772);
		progressBarItem.InnerBorder = factory.GetColor(2434341);
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
		progressBarItem.ChunkShadow = new GradientColorTable(8750469, 9474448, 0);
		progressBarItem = table.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = factory.GetColor(13421772);
		progressBarItem.InnerBorder = factory.GetColor(2434341);
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
		progressBarItem.ChunkShadow = new GradientColorTable(8750469, 9474448, 0);
		Office2007GalleryColorTable gallery = table.Gallery;
		gallery.GroupLabelBackground = factory.GetColor(15461355);
		gallery.GroupLabelText = factory.GetColor(0);
		gallery.GroupLabelBorder = factory.GetColor(12961221);
		table.ListViewEx.Border = factory.GetColor(color);
		table.ListViewEx.ColumnBackground = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(13948891));
		table.ListViewEx.ColumnSeparator = factory.GetColor(9542052);
		table.ListViewEx.SelectionBackground = new LinearGradientColorTable(factory.GetColor(10997232), Color.Empty);
		table.ListViewEx.SelectionBorder = factory.GetColor(14938111);
		table.NavigationPane.ButtonBackground = new GradientColorTable();
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16316665), 0f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14672612), 0.4f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(13093841), 0.4f));
		table.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14409442), 1f));
		table.SuperTooltip.BackgroundColors = new LinearGradientColorTable(factory.GetColor(16777215), factory.GetColor(15000816));
		table.SuperTooltip.TextColor = factory.GetColor(5000268);
		Office2007SliderColorTable slider = table.Slider;
		slider.Default.LabelColor = factory.GetColor(16777215);
		slider.Default.SliderLabelColor = factory.GetColor(0);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15724527), 0.15f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(12501699), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(7106936), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(14474462), 0.85f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 1f));
		slider.Default.PartBorderColor = factory.GetColor(3751750);
		slider.Default.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(16777215));
		slider.Default.PartForeColor = factory.GetColor(5989228);
		slider.Default.PartForeLightColor = Color.FromArgb(168, factory.GetColor(15395562));
		slider.Default.TrackLineColor = factory.GetColor(2434341);
		slider.Default.TrackLineLightColor = factory.GetColor(13421772);
		slider.MouseOver.LabelColor = factory.GetColor(16777215);
		slider.MouseOver.SliderLabelColor = factory.GetColor(0);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16777214), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16046995), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15645223), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16576191), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16445650), 1f));
		slider.MouseOver.PartBorderColor = factory.GetColor(2960685);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(16777215));
		slider.MouseOver.PartForeColor = factory.GetColor(6775369);
		slider.MouseOver.PartForeLightColor = Color.FromArgb(168, factory.GetColor(16774356));
		slider.MouseOver.TrackLineColor = factory.GetColor(2434341);
		slider.MouseOver.TrackLineLightColor = factory.GetColor(13421772);
		slider.Pressed.LabelColor = factory.GetColor(16777215);
		slider.Pressed.SliderLabelColor = factory.GetColor(0);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15109178), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15901267), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15958807), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(16301446), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(factory.GetColor(15979188), 1f));
		slider.Pressed.PartBorderColor = factory.GetColor(2960685);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(28, factory.GetColor(16777215));
		slider.Pressed.PartForeColor = factory.GetColor(6771523);
		slider.Pressed.PartForeLightColor = Color.FromArgb(168, factory.GetColor(16768187));
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
		table.DataGridView.ColumnHeaderNormalBorder = factory.GetColor(11974326);
		table.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(factory.GetColor(16316664), factory.GetColor(14606046), 90);
		table.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(factory.GetColor(16374175), factory.GetColor(15843679), 90);
		table.DataGridView.ColumnHeaderSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(16766349), factory.GetColor(15897146), 90);
		table.DataGridView.ColumnHeaderSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(factory.GetColor(14737632), factory.GetColor(12829635), 90);
		table.DataGridView.ColumnHeaderMouseOverBorder = factory.GetColor(11974326);
		table.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(factory.GetColor(14737632), factory.GetColor(12829635), 90);
		table.DataGridView.ColumnHeaderPressedBorder = factory.GetColor(16777215);
		table.DataGridView.RowNormalBackground = new LinearGradientColorTable(factory.GetColor(15592941));
		table.DataGridView.RowNormalBorder = factory.GetColor(11974326);
		table.DataGridView.RowSelectedBackground = new LinearGradientColorTable(factory.GetColor(16766349));
		table.DataGridView.RowSelectedBorder = factory.GetColor(15897910);
		table.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowSelectedMouseOverBorder = factory.GetColor(15897910);
		table.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowMouseOverBorder = factory.GetColor(11974326);
		table.DataGridView.RowPressedBackground = new LinearGradientColorTable(factory.GetColor(15843420));
		table.DataGridView.RowPressedBorder = factory.GetColor(16777215);
		table.DataGridView.GridColor = factory.GetColor(13686757);
		table.DataGridView.SelectorBackground = new LinearGradientColorTable(factory.GetColor(13224393));
		table.DataGridView.SelectorBorder = factory.GetColor(11974326);
		table.DataGridView.SelectorBorderDark = factory.GetColor(13421772);
		table.DataGridView.SelectorBorderLight = factory.GetColor(15461355);
		table.DataGridView.SelectorSign = new LinearGradientColorTable(factory.GetColor(8224125), factory.GetColor(6776679));
		table.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(factory.GetColor(11382189), factory.GetColor(10461087));
		table.DataGridView.SelectorMouseOverBorder = factory.GetColor(11974326);
		table.DataGridView.SelectorMouseOverBorderDark = factory.GetColor(13421772);
		table.DataGridView.SelectorMouseOverBorderLight = factory.GetColor(15461355);
		table.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(factory.GetColor(8224125), factory.GetColor(6776679));
		table.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(0);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(0);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor("FFFFFCD9"), 0f),
			new BackgroundColorBlend(factory.GetColor("FFFFE78F"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFFFD74A"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFFFE794"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor("FFDDCF9B");
		crumbBarItemViewStateColorTable.BorderLight = factory.GetColor("80FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(0);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(factory.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(factory.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(factory.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = factory.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		table.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = factory.GetColor(0);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(factory.GetColor("FFC89861"), 0f),
			new BackgroundColorBlend(factory.GetColor("FFEDAE68"), 0.1f),
			new BackgroundColorBlend(factory.GetColor("FFFCA160"), 0.6f),
			new BackgroundColorBlend(factory.GetColor("FFFC8F3D"), 0.6f),
			new BackgroundColorBlend(factory.GetColor("FFFEBD61"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = factory.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = factory.GetColor("408B7654");
		table.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = factory.GetColor(11316396);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = factory.GetColor(14344930);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetFileMenuContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetTwoColumnMenuContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMenuColumnOneContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMenuColumnTwoContainerStyle(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMenuBottomContainer(table);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetTextBoxStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetRichTextBoxStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetItemPanelStyle(factory.GetColor(color));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetDateTimeInputBackgroundStyle(factory.GetColor(color), SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetRibbonClientPanelStyle(factory, eOffice2007ColorScheme.Black);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetListViewBorderStyle(table.ListViewEx);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetStatusBarAltStyle(table.Bar);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetCrumbBarBackgroundStyle(factory.GetColor(Color.White), factory.GetColor("FF333333"), factory.GetColor("FF252525"));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetDataGridViewStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetDataGridViewDateTimeStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetDataGridViewNumericStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetDataGridViewIpAddressStyle();
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetSlideOutButtonStyle(table.CheckBoxItem.Default.CheckBorder);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMetroTilePanelStyle(factory.GetColor(Color.White));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMetroTileGroupStyle(factory.GetColor(Color.DarkGray));
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = GetMonthCalendarStyle(SystemColors.Window);
		table.StyleClasses.Add(elementStyle.Class, elementStyle);
		table.SideBar.Background = new LinearGradientColorTable(factory.GetColor(Color.White));
		table.SideBar.Border = factory.GetColor(10988982);
		table.SideBar.SideBarPanelItemText = factory.GetColor(Color.Black);
		table.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(16316665), 0f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14672612), 0.4f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(13093841), 0.4f));
		table.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(factory.GetColor(14409442), 1f));
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
		table.WarningBox.BackColor = factory.GetColor(Color.FromArgb(64, 64, 64));
		table.WarningBox.WarningBorderColor = factory.GetColor(Color.FromArgb(86, 86, 86));
		table.WarningBox.WarningBackColor1 = factory.GetColor(Color.FromArgb(255, 255, 255));
		table.WarningBox.WarningBackColor2 = factory.GetColor(Color.FromArgb(234, 234, 234));
		table.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(factory.GetColor(0)),
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
			new ColorDef(factory.GetColor(5002076)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(14475234),
				factory.GetColor(11844289)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(11844289)),
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
			new ColorDef(factory.GetColor(3355443)),
			new ColorDef(factory.GetColor(3355443)),
			new ColorDef(factory.GetColor(3355443)),
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
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				factory.GetColor(14475234),
				factory.GetColor(13883098),
				factory.GetColor(11844289),
				factory.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 0f),
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
			new ColorDef(factory.GetColor(11581118)),
			new ColorDef(factory.GetColor(15264492)),
			new ColorDef(factory.GetColor(16777215)),
			new ColorDef(factory.GetColor(13093841)),
			new ColorDef(factory.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				factory.GetColor(14475234),
				factory.GetColor(11844289)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(factory.GetColor(0)),
			new ColorDef(factory.GetColor(11844289)),
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
		table.SuperTab.Background = new SuperTabLinearGradientColorTable(factory.GetColor(13554393));
		table.SuperTab.InnerBorder = factory.GetColor(15988213);
		table.SuperTab.OuterBorder = factory.GetColor(12500670);
		table.SuperTab.ControlBoxDefault.Image = factory.GetColor(255, 0);
		table.SuperTab.ControlBoxMouseOver.Background = factory.GetColor(16770978);
		table.SuperTab.ControlBoxMouseOver.Border = factory.GetColor(16760169);
		table.SuperTab.ControlBoxMouseOver.Image = factory.GetColor(255, 0);
		table.SuperTab.ControlBoxPressed.Background = factory.GetColor(16485436);
		table.SuperTab.ControlBoxPressed.Border = factory.GetColor(16760169);
		table.SuperTab.ControlBoxPressed.Image = factory.GetColor(255, 0);
		table.SuperTab.InsertMarker = factory.GetColor(255, 128);
		table.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		table.SuperTabItem.Default.Normal.Text = factory.GetColor(0);
		table.SuperTabItem.Default.Normal.CloseMarker = factory.GetColor(0);
		table.SuperTabItem.Default.Disabled.Text = factory.GetColor(9276813);
		table.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		table.SuperTabItem.Default.Disabled.CloseMarker = factory.GetColor(9276813);
		table.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15658735),
			factory.GetColor(13554393)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.Selected.InnerBorder = factory.GetColor(15988213);
		table.SuperTabItem.Default.Selected.OuterBorder = factory.GetColor(12500670);
		table.SuperTabItem.Default.Selected.Text = factory.GetColor(0);
		table.SuperTabItem.Default.Selected.CloseMarker = factory.GetColor(0);
		table.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(15593199),
			factory.GetColor(13291478)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.SelectedMouseOver.InnerBorder = factory.GetColor(16444354);
		table.SuperTabItem.Default.SelectedMouseOver.OuterBorder = factory.GetColor(15252338);
		table.SuperTabItem.Default.SelectedMouseOver.Text = factory.GetColor(0);
		table.SuperTabItem.Default.SelectedMouseOver.CloseMarker = factory.GetColor(0);
		table.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			factory.GetColor(14406851),
			factory.GetColor(15322765)
		}, new float[2] { 0f, 1f });
		table.SuperTabItem.Default.MouseOver.InnerBorder = factory.GetColor(15264494);
		table.SuperTabItem.Default.MouseOver.OuterBorder = factory.GetColor(12435137);
		table.SuperTabItem.Default.MouseOver.Text = factory.GetColor(0);
		table.SuperTabItem.Default.MouseOver.CloseMarker = factory.GetColor(0);
		table.SuperTabItem.Left = table.SuperTabItem.Default;
		table.SuperTabItem.Bottom = table.SuperTabItem.Default;
		table.SuperTabItem.Right = table.SuperTabItem.Default;
		table.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(factory.GetColor(13554393));
		table.SuperTabPanel.Default.InnerBorder = factory.GetColor(15988213);
		table.SuperTabPanel.Default.OuterBorder = factory.GetColor(12500670);
		table.SuperTabPanel.Left = table.SuperTabPanel.Default;
		table.SuperTabPanel.Bottom = table.SuperTabPanel.Default;
		table.SuperTabPanel.Right = table.SuperTabPanel.Default;
		SuperTabStyleColorFactory.GetOffice2010BackstageBlackColorTable(table.Backstage, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageBlackItemColorTable(table.BackstageItem, factory);
		SuperTabStyleColorFactory.GetOffice2010BackstageBlackPanelColorTable(table.BackstagePanel, factory);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = factory.GetColor(color);
		switchButtonColorTable.OffBackColor = factory.GetColor(13225686);
		switchButtonColorTable.OffTextColor = factory.GetColor(0);
		switchButtonColorTable.OnBackColor = factory.GetColor(9621584);
		switchButtonColorTable.OnTextColor = factory.GetColor(0);
		switchButtonColorTable.SwitchBackColor = factory.GetColor(11843254);
		switchButtonColorTable.SwitchBorderColor = factory.GetColor(9013899);
		switchButtonColorTable.TextColor = factory.GetColor(0);
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
		table.StepIndicator.BackgroundColor = factory.GetColor(ColorFunctions.GetShade(table.Form.BackColor, 10));
		table.StepIndicator.IndicatorColor = factory.GetColor(10800168);
	}

	public static void InitializeBlackLegacyColors(ColorScheme c, ColorFactory factory)
	{
		c.BarBackground = factory.GetColor(13488341);
		c.BarBackground2 = factory.GetColor(9147035);
		c.BarStripeColor = factory.GetColor(3619907);
		c.BarCaptionBackground = factory.GetColor(7766158);
		c.BarCaptionBackground2 = factory.GetColor(5002076);
		c.BarCaptionInactiveBackground = factory.GetColor(15790578);
		c.BarCaptionInactiveBackground2 = factory.GetColor(12436169);
		c.BarCaptionInactiveText = factory.GetColor(3619907);
		c.BarCaptionText = factory.GetColor(16777215);
		c.BarFloatingBorder = factory.GetColor(3619907);
		c.BarPopupBackground = factory.GetColor(16185078);
		c.BarPopupBorder = factory.GetColor(9542052);
		c.ItemBackground = Color.Empty;
		c.ItemBackground2 = Color.Empty;
		c.ItemCheckedBackground = factory.GetColor(16765802);
		c.ItemCheckedBackground2 = factory.GetColor(16500815);
		c.ItemCheckedBorder = factory.GetColor(12276995);
		c.ItemCheckedText = factory.GetColor(0);
		c.ItemDisabledBackground = Color.Empty;
		c.ItemDisabledText = factory.GetColor(9276813);
		c.ItemExpandedShadow = Color.Empty;
		c.ItemExpandedBackground = factory.GetColor(9542052);
		c.ItemExpandedBackground2 = factory.GetColor(6647418);
		c.ItemExpandedText = factory.GetColor(0);
		c.ItemHotBackground = factory.GetColor(16774604);
		c.ItemHotBackground2 = factory.GetColor(16767861);
		c.ItemHotBorder = factory.GetColor(16760169);
		c.ItemHotText = factory.GetColor(0);
		c.ItemPressedBackground = factory.GetColor(16553789);
		c.ItemPressedBackground2 = factory.GetColor(16758878);
		c.ItemPressedBorder = factory.GetColor(16485436);
		c.ItemPressedText = factory.GetColor(0);
		c.ItemSeparator = Color.FromArgb(225, factory.GetColor(9542052));
		c.ItemSeparatorShade = Color.FromArgb(180, factory.GetColor(16777215));
		c.ItemText = factory.GetColor(0);
		c.MenuBackground = factory.GetColor(16185078);
		c.MenuBackground2 = Color.Empty;
		c.MenuBarBackground = factory.GetColor(5460819);
		c.MenuBorder = factory.GetColor(9542052);
		c.ItemExpandedBorder = c.MenuBorder;
		c.MenuSide = factory.GetColor(15724527);
		c.MenuSide2 = Color.Empty;
		c.MenuUnusedBackground = c.MenuBackground;
		c.MenuUnusedSide = factory.GetColor(15329769);
		c.MenuUnusedSide2 = Color.Empty;
		c.ItemDesignTimeBorder = Color.Black;
		c.BarDockedBorder = factory.GetColor(5002076);
		c.DockSiteBackColor = factory.GetColor(5460819);
		c.DockSiteBackColor2 = Color.Empty;
		c.CustomizeBackground = factory.GetColor(11712447);
		c.CustomizeBackground2 = factory.GetColor(5331042);
		c.CustomizeText = factory.GetColor(0);
		c.PanelBackground = factory.GetColor(15790578);
		c.PanelBackground2 = factory.GetColor(12436169);
		c.PanelText = Color.Black;
		c.PanelBorder = factory.GetColor(10988982);
		c.ExplorerBarBackground = factory.GetColor(15790578);
		c.ExplorerBarBackground2 = factory.GetColor(12436169);
		c.MdiSystemItemForeground = Color.LightGray;
		c.SplitterBackground = c.PanelBackground;
		c.SplitterBackground2 = c.PanelBackground2;
		c.SplitterText = c.PanelText;
		c.SplitterBorder = c.PanelBorder;
	}

	public static ElementStyle GetFileMenuContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 3;
		elementStyle.PaddingRight = 3;
		elementStyle.PaddingTop = 12;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend.CopyTo(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle GetTwoColumnMenuContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.BorderBottom = eStyleBorderType.Double;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.BorderColor = menu.FileContainerBorder;
		elementStyle.BorderColorLight = menu.FileContainerBorderLight;
		elementStyle.BorderLeft = eStyleBorderType.Double;
		elementStyle.BorderLeftWidth = 1;
		elementStyle.BorderRight = eStyleBorderType.Double;
		elementStyle.BorderRightWidth = 1;
		elementStyle.BorderTop = eStyleBorderType.Double;
		elementStyle.BorderTopWidth = 1;
		elementStyle.PaddingBottom = 1;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		elementStyle.PaddingTop = 1;
		return elementStyle;
	}

	public static ElementStyle GetMenuColumnOneContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle GetMenuColumnTwoContainerStyle(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle GetMenuBottomContainer(Office2007ColorTable table)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		Office2007MenuColorTable menu = table.Menu;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBottomContainerBackgroundBlend.Count];
		menu.FileBottomContainerBackgroundBlend.CopyTo(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle GetTextBoxStyle(Color borderColor)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.TextBoxBorderKey;
		elementStyle.BorderColor = borderColor;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.PaddingBottom = 2;
		elementStyle.PaddingTop = 2;
		elementStyle.PaddingLeft = 2;
		elementStyle.PaddingRight = 2;
		return elementStyle;
	}

	public static ElementStyle GetRichTextBoxStyle(Color borderColor)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RichTextBoxBorderKey;
		elementStyle.BorderColor = borderColor;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		return elementStyle;
	}

	public static ElementStyle GetItemPanelStyle(Color borderColor, Color backColor)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.ItemPanelKey;
		elementStyle.BorderColor = borderColor;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.PaddingBottom = 1;
		elementStyle.BackColor = backColor;
		return elementStyle;
	}

	public static ElementStyle GetMetroTilePanelStyle(Color backColor)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.MetroTilePanelKey;
		elementStyle.PaddingLeft = 16;
		elementStyle.PaddingRight = 16;
		elementStyle.PaddingTop = 16;
		elementStyle.PaddingBottom = 16;
		elementStyle.BackColor = backColor;
		return elementStyle;
	}

	public static ElementStyle GetMetroTileGroupStyle(Color textColor)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.MetroTileGroupTitleKey;
		elementStyle.TextColor = textColor;
		elementStyle.Font = new Font("Segoe UI Semibold", 27.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		return elementStyle;
	}

	public static ElementStyle GetItemPanelStyle(Color borderColor)
	{
		return GetItemPanelStyle(borderColor, Color.White);
	}

	public static ElementStyle GetDateTimeInputBackgroundStyle(Color borderColor, Color backColor)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.DateTimeInputBackgroundKey;
		elementStyle.BorderColor = borderColor;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BackColor = backColor;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.PaddingBottom = 1;
		return elementStyle;
	}

	public static ElementStyle GetRibbonClientPanelStyle(ColorFactory f, eOffice2007ColorScheme cs)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonClientPanelKey;
		elementStyle.BackColorGradientAngle = 90;
		switch (cs)
		{
		case eOffice2007ColorScheme.Blue:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(12506352), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(5668273), 0.8f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(6656461), 1f));
			break;
		case eOffice2007ColorScheme.Silver:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(13422552), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(11251382), 0.8f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(10198950), 1f));
			break;
		case eOffice2007ColorScheme.Black:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(8289918), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(3552822), 0.8f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(f.GetColor(657930), 1f));
			break;
		}
		return elementStyle;
	}

	public static ElementStyle GetListViewBorderStyle(Office2007ListViewColorTable lvct)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.ListViewBorderKey;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BorderColor = lvct.Border;
		return elementStyle;
	}

	public static ElementStyle GetStatusBarAltStyle(Office2007BarColorTable t)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.Office2007StatusBarBackground2Key;
		elementStyle.PaddingLeft = 4;
		elementStyle.BorderLeft = eStyleBorderType.Etched;
		elementStyle.BorderLeftWidth = 1;
		elementStyle.BorderLeftColor = Color.FromArgb(196, t.StatusBarTopBorder);
		elementStyle.BorderColorLight = Color.FromArgb(128, Color.White);
		if (t.StatusBarAltBackground.Count > 0)
		{
			elementStyle.BackColorBlend.CopyFrom(t.StatusBarAltBackground);
			elementStyle.BackColorGradientAngle = 90;
		}
		return elementStyle;
	}

	public static ElementStyle GetCrumbBarBackgroundStyle(Color backColor, Color borderColor1, Color borderColor2)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.CrumbBarBackgroundKey;
		elementStyle.BackColor = backColor;
		elementStyle.BorderColor = borderColor1;
		elementStyle.BorderColor2 = borderColor2;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		return elementStyle;
	}

	public static ElementStyle GetDataGridViewStyle()
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.DataGridViewBorderKey;
		elementStyle.BorderWidth = 0;
		elementStyle.Border = eStyleBorderType.None;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingTop = 3;
		elementStyle.PaddingLeft = 3;
		elementStyle.PaddingRight = 3;
		return elementStyle;
	}

	public static ElementStyle GetDataGridViewNumericStyle()
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.DataGridViewNumericBorderKey;
		elementStyle.BorderWidth = 0;
		elementStyle.Border = eStyleBorderType.None;
		elementStyle.PaddingBottom = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.PaddingLeft = 2;
		elementStyle.PaddingRight = 2;
		return elementStyle;
	}

	public static ElementStyle GetDataGridViewDateTimeStyle()
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.DataGridViewDateTimeBorderKey;
		elementStyle.BorderWidth = 0;
		elementStyle.Border = eStyleBorderType.None;
		elementStyle.PaddingBottom = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle GetDataGridViewIpAddressStyle()
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.DataGridViewIpAddressBorderKey;
		elementStyle.BorderWidth = 0;
		elementStyle.Border = eStyleBorderType.None;
		elementStyle.PaddingBottom = 2;
		elementStyle.PaddingTop = 2;
		elementStyle.PaddingLeft = 2;
		elementStyle.PaddingRight = 2;
		return elementStyle;
	}

	public static ElementStyle GetSlideOutButtonStyle(Color backColor)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
		elementStyle.BackColor = backColor;
		return elementStyle;
	}

	public static ElementStyle GetMonthCalendarStyle(Color backColor)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.MonthCalendarAdvKey);
		elementStyle.BackColor = backColor;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		elementStyle.BorderWidth = 1;
		return elementStyle;
	}
}
