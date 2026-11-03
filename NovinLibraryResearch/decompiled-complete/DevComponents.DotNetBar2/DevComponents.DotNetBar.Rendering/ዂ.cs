using System;
using System.Drawing;
using DevComponents.AdvTree.Display;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Schedule;

namespace DevComponents.DotNetBar.Rendering;

internal class ዂ
{
	private const int ዃ = 5;

	public static void ዄ(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(5668273);
		Color color2 = ቖ.GetColor(15397625);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = color2;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = color2;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = color2;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = color2;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = color2;
	}

	public static void _12C6(Office2007ButtonItemColorTable ዅ, ColorFactory ቖ)
	{
		Color color = ቖ.GetColor(4605510);
		Color color2 = ቖ.GetColor(14408668);
		ዅ.Default.ExpandBackground = color;
		ዅ.Default.ExpandLight = color2;
		ዅ.Checked.ExpandBackground = color;
		ዅ.Checked.ExpandLight = color2;
		ዅ.Expanded.ExpandBackground = color;
		ዅ.Expanded.ExpandLight = color2;
		ዅ.MouseOver.ExpandBackground = color;
		ዅ.MouseOver.ExpandLight = color2;
		ዅ.Pressed.ExpandBackground = color;
		ዅ.Pressed.ExpandLight = color2;
	}

	public static Office2007ButtonItemColorTable _12C7(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(1393291);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14536603), ቖ.GetColor(12625782));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777207), ቖ.GetColor(16773822));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776409), ቖ.GetColor(16770957));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16766792), ቖ.GetColor(16770963));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(132, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(14402420), ቖ.GetColor(14402419));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16773311), ቖ.GetColor(16774361));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(1393291);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6258614), ቖ.GetColor(12892584));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(11636829), ቖ.GetColor(16624913));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16300137), ቖ.GetColor(16622433));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16484924), ቖ.GetColor(16694112));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16768166), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(11566411), ቖ.GetColor(9467987));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16433300), ቖ.GetColor(16767404));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(5486);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13350041), ቖ.GetColor(16770175));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15454644), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16505781), ቖ.GetColor(16697208));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16692310), ቖ.GetColor(16640927));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16640927), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(5486);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9339237), ቖ.GetColor(13025458));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15052373), ቖ.GetColor(16764717));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16569255), ቖ.GetColor(16427099));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16289322), ቖ.GetColor(16507286));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16642480), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(11566411), ቖ.GetColor(9467987));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16433300), ቖ.GetColor(16767404));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(5486);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ወ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = _12C7(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14938111), ቖ.GetColor(12901887));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11391487), ቖ.GetColor(12639231));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7836601), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15266300), ቖ.GetColor(15331836));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13820404), ቖ.GetColor(15463421));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7836601), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዉ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(1393291);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(14741247));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12639487), ቖ.GetColor(11128575));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(10931711), ቖ.GetColor(13296127));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(124, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(7640514), ቖ.GetColor(9482198));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14083059), ቖ.GetColor(8627664));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(1393291);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(14741247));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12967160), ቖ.GetColor(11127543));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(9484010), ቖ.GetColor(7640514));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(7640514), ቖ.GetColor(9482198));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14083059), ቖ.GetColor(8627664));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(5486);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5668272), ቖ.GetColor(5668272));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9484010), ቖ.GetColor(7640514));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(5486);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6391735), ቖ.GetColor(5014472));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(10663117), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(5471928), ቖ.GetColor(6915498));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(10141941), ቖ.GetColor(12179455));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(5486);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዊ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዉ(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14938111), ቖ.GetColor(12901887));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11391487), ቖ.GetColor(12639231));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7836601), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15266300), ቖ.GetColor(15331836));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13820404), ቖ.GetColor(15463421));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7836601), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዋ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(1393291);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15917035), ቖ.GetColor(15124187));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15378644), ቖ.GetColor(14778817));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13807814), ቖ.GetColor(13348289));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13793973), ቖ.GetColor(13126815));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15325154), ቖ.GetColor(14856401));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13603002), ቖ.GetColor(13595568));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(5486);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15325154), ቖ.GetColor(14856401));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13603002), ቖ.GetColor(13595568));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(0);
		ዄ(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዌ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዋ(ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14938111), ቖ.GetColor(12901887));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11391487), ቖ.GetColor(12639231));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7836601), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15266300), ቖ.GetColor(15331836));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13820404), ቖ.GetColor(15463421));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7836601), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ው(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13365499), ቖ.GetColor(7322597));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(6731231), ቖ.GetColor(10677750));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(11664639), Color.Empty);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.White, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7970230), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(Color.WhiteSmoke), ቖ.GetColor(Color.LightGray));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(Color.Silver), ቖ.GetColor(15463421));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.WhiteSmoke, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7970230), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16579042), ቖ.GetColor(16510399));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16439370), ቖ.GetColor(16573845));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(Color.White, Color.Empty);
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12560249), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(12560249), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16300664), ቖ.GetColor(16560718));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16555023), ቖ.GetColor(16562743));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15837002), Color.Empty);
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16579042), ቖ.GetColor(16510399));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16439370), ቖ.GetColor(16573845));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.White, Color.Empty);
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15837002), Color.Empty);
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16562743), ቖ.GetColor(16555023));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16560718), ቖ.GetColor(16300664));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15837002), Color.Empty);
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(9665882), Color.Empty);
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Empty);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonBarStateColorTable ዎ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12964575), ቖ.GetColor(10403803));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15199991), ቖ.GetColor(15857661));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14608629), ቖ.GetColor(13754352));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13097197), ቖ.GetColor(14215413));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(12769521), ቖ.GetColor(12638447));
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(4090538);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable ዏ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11388894), ቖ.GetColor(8302035));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(16777215));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15003645), ቖ.GetColor(15266044));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14478075), ቖ.GetColor(14477560));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(13164799), ቖ.GetColor(14085631));
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(4090553);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable ዐ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8821678), ቖ.GetColor(11651033));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(11387356), ቖ.GetColor(8955856));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(7772616), ቖ.GetColor(11851261));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(12835314), Color.Empty);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዑ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15791870), ቖ.GetColor(14411509));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16317439), ቖ.GetColor(12581631));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9943526), ቖ.GetColor(9286371));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15463422), ቖ.GetColor(14805750));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16444870), ቖ.GetColor(16777149));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14138506), ቖ.GetColor(16699742));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13490912), ቖ.GetColor(14995607));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(12902143), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14675455), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10009577), ቖ.GetColor(10009577));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዒ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(13945058), ቖ.GetColor(15525874));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(13484767));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12499649), ቖ.GetColor(12632001));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13945058), ቖ.GetColor(15525874));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(13484767));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12499649), ቖ.GetColor(12632001));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(12899832), ቖ.GetColor(12762344));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14741247), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዓ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(15004383));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14019283), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12173751), ቖ.GetColor(12566974));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(11788003), ቖ.GetColor(9100959));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(14609663), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዔ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16642700));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289388), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(1393291);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13623521), ቖ.GetColor(15329177));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(13361919), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14872575), ቖ.GetColor(13099007));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12699857), ቖ.GetColor(12634064));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(1393291);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabGroupColorTable ዕ(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(13483991));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(13658313)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(1393291);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable ዖ(ColorFactory ቖ)
	{
		return ዕ(ቖ);
	}

	public static Office2007RibbonTabGroupColorTable _12D7(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(10936974));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(7059796)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(1393291);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable ዘ(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(15527849));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(16770585)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(1393291);
		return office2007RibbonTabGroupColorTable;
	}

	public static SuperTabColorTable ዙ(ColorFactory ቖ)
	{
		SuperTabColorTable superTabColorTable = new SuperTabColorTable();
		superTabColorTable.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(12573695));
		superTabColorTable.InnerBorder = ቖ.GetColor(12581631);
		superTabColorTable.OuterBorder = ቖ.GetColor(9286371);
		superTabColorTable.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxMouseOver.Background = ቖ.GetColor(16770978);
		superTabColorTable.ControlBoxMouseOver.Border = ቖ.GetColor(16760169);
		superTabColorTable.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		superTabColorTable.ControlBoxPressed.Background = ቖ.GetColor(16485436);
		superTabColorTable.ControlBoxPressed.Border = ቖ.GetColor(16760169);
		superTabColorTable.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		superTabColorTable.InsertMarker = ቖ.GetColor(255, 128);
		return superTabColorTable;
	}

	public static SuperTabItemColorTable ዚ(ColorFactory ቖ)
	{
		SuperTabItemColorTable superTabItemColorTable = new SuperTabItemColorTable();
		superTabItemColorTable.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		superTabItemColorTable.Default.Normal.Text = ቖ.GetColor(1393291);
		superTabItemColorTable.Default.Normal.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(15791870),
			ቖ.GetColor(14411509)
		}, new float[4] { 0f, 0.15f, 0.15f, 1f });
		superTabItemColorTable.Default.Selected.InnerBorder = ቖ.GetColor(12581631);
		superTabItemColorTable.Default.Selected.OuterBorder = ቖ.GetColor(9286371);
		superTabItemColorTable.Default.Selected.Text = ቖ.GetColor(1393291);
		superTabItemColorTable.Default.Selected.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(15463422),
			ቖ.GetColor(14805750)
		}, new float[4] { 0f, 0.07f, 0.07f, 1f });
		superTabItemColorTable.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(16777149);
		superTabItemColorTable.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(14137215);
		superTabItemColorTable.Default.SelectedMouseOver.Text = ቖ.GetColor(1393291);
		superTabItemColorTable.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(12573695), ቖ.GetColor(14930328));
		superTabItemColorTable.Default.MouseOver.InnerBorder = ቖ.GetColor(15726335);
		superTabItemColorTable.Default.MouseOver.OuterBorder = ቖ.GetColor(10075112);
		superTabItemColorTable.Default.MouseOver.Text = ቖ.GetColor(1393291);
		superTabItemColorTable.Default.MouseOver.CloseMarker = ቖ.GetColor(4222879);
		superTabItemColorTable.Left = superTabItemColorTable.Default;
		superTabItemColorTable.Bottom = superTabItemColorTable.Default;
		superTabItemColorTable.Right = superTabItemColorTable.Default;
		return superTabItemColorTable;
	}

	public static SuperTabPanelColorTable ዛ(ColorFactory ቖ)
	{
		SuperTabPanelColorTable superTabPanelColorTable = new SuperTabPanelColorTable();
		superTabPanelColorTable.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(15791870), ቖ.GetColor(14411509));
		superTabPanelColorTable.Default.InnerBorder = ቖ.GetColor(12581631);
		superTabPanelColorTable.Default.OuterBorder = ቖ.GetColor(9286371);
		superTabPanelColorTable.Left = superTabPanelColorTable.Default;
		superTabPanelColorTable.Bottom = superTabPanelColorTable.Default;
		superTabPanelColorTable.Right = superTabPanelColorTable.Default;
		return superTabPanelColorTable;
	}

	public static SuperTabColorTable ዜ(ColorFactory ቖ)
	{
		return ጦ.ጮ(null, ቖ);
	}

	public static SuperTabItemColorTable ዝ(ColorFactory ቖ)
	{
		return ጦ.ጶ(null, ቖ);
	}

	public static SuperTabPanelColorTable ዞ(ColorFactory ቖ)
	{
		return ጦ.ጼ(null, ቖ);
	}

	public static Office2007RibbonBarStateColorTable ዟ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11448500), ቖ.GetColor(8487297));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15330286), ቖ.GetColor(15461355));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13554650), ቖ.GetColor(12699343));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11844549), ቖ.GetColor(15068396));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(11974840), ቖ.GetColor(10264222));
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(16777215);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable ዠ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11448500), ቖ.GetColor(8487297));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16119543), ቖ.GetColor(15857142));
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15527664), ቖ.GetColor(15198700));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(14935529), ቖ.GetColor(16054006));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(11185067), ቖ.GetColor(7171694));
		office2007RibbonBarStateColorTable.TitleText = ቖ.GetColor(16777215);
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonBarStateColorTable ዡ(ColorFactory ቖ)
	{
		Office2007RibbonBarStateColorTable office2007RibbonBarStateColorTable = new Office2007RibbonBarStateColorTable();
		office2007RibbonBarStateColorTable.TopBackgroundHeight = 15f;
		office2007RibbonBarStateColorTable.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(8158332), ቖ.GetColor(12237498));
		office2007RibbonBarStateColorTable.InnerBorder = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonBarStateColorTable.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12632256), ቖ.GetColor(11383224));
		office2007RibbonBarStateColorTable.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(10396588), ቖ.GetColor(14014424));
		office2007RibbonBarStateColorTable.TitleBackground = new LinearGradientColorTable(ቖ.GetColor(12763842), Color.Empty);
		office2007RibbonBarStateColorTable.TitleText = Color.Empty;
		return office2007RibbonBarStateColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዢ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(15658735), ቖ.GetColor(13554393));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15988213), ቖ.GetColor(12121852));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12500670), ቖ.GetColor(12500670));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(15593199), ቖ.GetColor(13291478));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16444354), ቖ.GetColor(16777149));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(15252338), ቖ.GetColor(16699742));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(9737106), ቖ.GetColor(9339471));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(15581735), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9934485), ቖ.GetColor(9667917));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9868949), ቖ.GetColor(11577734));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዣ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(13945058), ቖ.GetColor(15459826));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14801387), ቖ.GetColor(13484767));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12368321), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(13945058), ቖ.GetColor(15459826));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13022937), ቖ.GetColor(13484767));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12368321), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(9145745), ቖ.GetColor(8352917));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(9008797), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9079695), ቖ.GetColor(8352917));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10066074), ቖ.GetColor(10526370));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዤ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(12706488), ቖ.GetColor(14938589));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15004640), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11583660), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(12706488), ቖ.GetColor(14938589));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(10802326), ቖ.GetColor(12115633));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11583660), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(9015431), ቖ.GetColor(6656600));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(7118172), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(8883334), ቖ.GetColor(6656599));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9935255), ቖ.GetColor(10331035));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007RibbonTabItemColorTable ዥ(ColorFactory ቖ)
	{
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = new Office2007RibbonTabItemColorTable();
		office2007RibbonTabItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007RibbonTabItemColorTable.Selected = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.Selected.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.Selected.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.Selected.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16776150), ቖ.GetColor(16577429));
		office2007RibbonTabItemColorTable.Selected.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289387), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.Selected.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.SelectedMouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.SelectedMouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(16774559), ቖ.GetColor(16775893));
		office2007RibbonTabItemColorTable.SelectedMouseOver.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007RibbonTabItemColorTable.SelectedMouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16773233), ቖ.GetColor(16577429));
		office2007RibbonTabItemColorTable.SelectedMouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(13289387), ቖ.GetColor(12763842));
		office2007RibbonTabItemColorTable.SelectedMouseOver.Text = ቖ.GetColor(0);
		office2007RibbonTabItemColorTable.MouseOver = new Office2007RibbonTabItemStateColorTable();
		office2007RibbonTabItemColorTable.MouseOver.Background = new LinearGradientColorTable(ቖ.GetColor(9605251), ቖ.GetColor(11970867));
		office2007RibbonTabItemColorTable.MouseOver.BackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(12760369), Color.Transparent);
		office2007RibbonTabItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9407875), ቖ.GetColor(11641907));
		office2007RibbonTabItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10263446), ቖ.GetColor(12500670));
		office2007RibbonTabItemColorTable.MouseOver.Text = ቖ.GetColor(16777215);
		return office2007RibbonTabItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዦ(bool ዧ, ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14536603), ቖ.GetColor(12626039));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777207), ቖ.GetColor(16775062));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776409), ቖ.GetColor(16770957));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16766792), ቖ.GetColor(16770963));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(14402420), ቖ.GetColor(13155212));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16773311), ቖ.GetColor(16774879));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9139796), ቖ.GetColor(12892584));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15312993), ቖ.GetColor(16624913));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16299370), ቖ.GetColor(16556128));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16484924), ቖ.GetColor(16694626));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16769199), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(11566411), ቖ.GetColor(9467987));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16433300), ቖ.GetColor(16767404));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10450265), ቖ.GetColor(16764717));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15454644), Color.Transparent);
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16505781), ቖ.GetColor(16697208));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(32, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16692310), ቖ.GetColor(16640927));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774559), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9339237), ቖ.GetColor(13025458));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15052373), ቖ.GetColor(16766538));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16569255), ቖ.GetColor(16427099));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16289065), ቖ.GetColor(16639129));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(ቖ.GetColor(16774857), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(11566411), ቖ.GetColor(9139796));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(16433300), ቖ.GetColor(16767147));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(0);
		_12C6(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable የ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዦ(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16316665), ቖ.GetColor(14672612));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13093841), ቖ.GetColor(14409442));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9013125), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13554650), ቖ.GetColor(12699343));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11844549), ቖ.GetColor(15068396));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12964318), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዩ(bool ዧ, ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(14741247));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12639487), ቖ.GetColor(11128575));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(10931711), ቖ.GetColor(13296127));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(124, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(7640514), ቖ.GetColor(9482198));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14083059), ቖ.GetColor(8627664));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(3889794), Color.Empty);
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(14741247));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(12967160), ቖ.GetColor(11127543));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(9484010), ቖ.GetColor(7640514));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(7640514), ቖ.GetColor(9482198));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14083059), ቖ.GetColor(8627664));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5668272), ቖ.GetColor(5668272));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(9484010), ቖ.GetColor(7640514));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(6391735), ቖ.GetColor(5014472));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(10663117), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14149369), ቖ.GetColor(13098232));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11784437), ቖ.GetColor(14149111));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(5471928), ቖ.GetColor(6915498));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(10141941), ቖ.GetColor(12179455));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(0);
		_12C6(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዪ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዩ(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16316665), ቖ.GetColor(14672612));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13093841), ቖ.GetColor(14409442));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9013125), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13554650), ቖ.GetColor(12699343));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11844549), ቖ.GetColor(15068396));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(12964318), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ያ(bool ዧ, ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = new Office2007ButtonItemColorTable();
		office2007ButtonItemColorTable.Default = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.MouseOver = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15917035), ቖ.GetColor(15124187));
		office2007ButtonItemColorTable.MouseOver.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(96, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(15378644), ቖ.GetColor(14778817));
		office2007ButtonItemColorTable.MouseOver.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.MouseOver.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.MouseOver.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Pressed = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13807814), ቖ.GetColor(13348289));
		office2007ButtonItemColorTable.Pressed.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13793973), ቖ.GetColor(13126815));
		office2007ButtonItemColorTable.Pressed.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(184, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Pressed.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.Pressed.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Checked.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Checked.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Checked.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15325154), ቖ.GetColor(14856401));
		office2007ButtonItemColorTable.Checked.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(164, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13603002), ቖ.GetColor(13595568));
		office2007ButtonItemColorTable.Checked.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(92, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Checked.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Expanded = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Expanded.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(7012448), ቖ.GetColor(5636429));
		office2007ButtonItemColorTable.Expanded.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15525875), ቖ.GetColor(15259391));
		office2007ButtonItemColorTable.Expanded.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15325154), ቖ.GetColor(14856401));
		office2007ButtonItemColorTable.Expanded.TopBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13603002), ቖ.GetColor(13595568));
		office2007ButtonItemColorTable.Expanded.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(192, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Expanded.SplitBorder = new LinearGradientColorTable(ቖ.GetColor(10714065), ቖ.GetColor(8939694));
		office2007ButtonItemColorTable.Expanded.SplitBorderLight = new LinearGradientColorTable(ቖ.GetColor(14271219), ቖ.GetColor(12297939));
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(0);
		_12C6(office2007ButtonItemColorTable, ቖ);
		return office2007ButtonItemColorTable;
	}

	public static Office2007ButtonItemColorTable ዬ(ColorFactory ቖ)
	{
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ያ(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16316665), ቖ.GetColor(14672612));
		office2007ButtonItemColorTable.Default.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13093841), ቖ.GetColor(14409442));
		office2007ButtonItemColorTable.Default.BottomBackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(128, Color.White), Color.Transparent);
		office2007ButtonItemColorTable.Default.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(9013125), Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Default.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled = new Office2007ButtonItemStateColorTable();
		office2007ButtonItemColorTable.Disabled.TopBackground = new LinearGradientColorTable(ቖ.GetColor(13554650), ቖ.GetColor(12699343));
		office2007ButtonItemColorTable.Disabled.TopBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11844549), ቖ.GetColor(15068396));
		office2007ButtonItemColorTable.Disabled.BottomBackgroundHighlight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.InnerBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(4934475), Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorder = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.SplitBorderLight = new LinearGradientColorTable(Color.Empty, Color.Empty);
		office2007ButtonItemColorTable.Disabled.Text = ቖ.GetColor(9276813);
		return office2007ButtonItemColorTable;
	}

	public static Office2007RibbonTabGroupColorTable ይ(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(10526880));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(13487565)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(16777215);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable ዮ(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(9465999));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(11500962)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(16777215);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable ዯ(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(7249757));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(64, ቖ.GetColor(7059796)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(16777215);
		return office2007RibbonTabGroupColorTable;
	}

	public static Office2007RibbonTabGroupColorTable ደ(ColorFactory ቖ)
	{
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = new Office2007RibbonTabGroupColorTable();
		office2007RibbonTabGroupColorTable.Background = new LinearGradientColorTable(Color.Transparent, ቖ.GetColor(10918721));
		office2007RibbonTabGroupColorTable.BackgroundHighlight = new LinearGradientColorTable(Color.FromArgb(255, ቖ.GetColor(16769792)), Color.Transparent);
		office2007RibbonTabGroupColorTable.Border = new LinearGradientColorTable(Color.FromArgb(16, Color.DarkGray), Color.FromArgb(192, Color.DarkGray));
		office2007RibbonTabGroupColorTable.Text = ቖ.GetColor(16777215);
		return office2007RibbonTabGroupColorTable;
	}

	public static void ዱ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.ScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(16579836), ቖ.GetColor(15790320), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(15461871), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15462128), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15133168), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13753060), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(12503771), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(15264233), ቖ.GetColor(15330546));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6254227), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.MouseOver;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor(12571114), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13295610), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11193334), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11193334), 0.7f),
			new BackgroundColorBlend(ቖ.GetColor(13886714), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16448767), ቖ.GetColor(15660284));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9213861), ቖ.GetColor(6713727));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(3357780), ቖ.GetColor(5596557));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(12571114), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13295610), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(11193334), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13886714), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(15329771), ቖ.GetColor(16580095));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(3960496), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.MouseOverControl;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16054783), 0f),
			new BackgroundColorBlend(ቖ.GetColor(16054783), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13623800), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(14675196), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16514559), Color.Empty);
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9213861), ቖ.GetColor(6713727));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15462128), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15133168), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(13753060), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(12503771), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(15264233), ቖ.GetColor(15330802));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(6254227), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.Pressed;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(15133164), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14869734), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor(12831442), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor(14146785), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16119800), Color.Empty);
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(9213861), ቖ.GetColor(6647935));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675));
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(10403561), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11259126), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(7251696), 0.5f),
			new BackgroundColorBlend(ቖ.GetColor(11915767), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(12768231), ቖ.GetColor(14543867));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1526154), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(6316128)), Color.FromArgb(64, Color.White));
		@default = ዲ.ScrollBar.Disabled;
		@default.Background = ዲ.ScrollBar.Default.Background;
		@default.Border = ዲ.ScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዳ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(9810130), ቖ.GetColor(8166337), 0);
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(11718902));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[6]
		{
			new BackgroundColorBlend(ቖ.GetColor(14935527), 0f),
			new BackgroundColorBlend(ቖ.GetColor(14936560), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(13161697), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11977944), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(11977944), 0.8f),
			new BackgroundColorBlend(ቖ.GetColor(14804975), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(14935527), ቖ.GetColor(14804975));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(5859729), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(9146000)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOver;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor(12571114), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13295610), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11193334), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11193334), 0.7f),
			new BackgroundColorBlend(ቖ.GetColor(13886714), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(15329771), ቖ.GetColor(16580095));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(3960496), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(8226450)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOverControl;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(5596557), ቖ.GetColor(3357780));
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Pressed;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor(10337767), 0f),
			new BackgroundColorBlend(ቖ.GetColor(11259126), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(7251696), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(7251696), 0.7f),
			new BackgroundColorBlend(ቖ.GetColor(11915767), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(12768231), ቖ.GetColor(14543867));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1526154), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(7241382), ቖ.GetColor(4344675), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(12768231), ቖ.GetColor(14543867));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1526154), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(8487297)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.Disabled;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዴ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(11843519));
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(10330022));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8225934), ቖ.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16119286), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15395564), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(14343133), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(12698053), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16382457), ቖ.GetColor(13619154));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(7237491), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOver;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor(14479611), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13232634), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11131894), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11131894), 0.7f),
			new BackgroundColorBlend(ቖ.GetColor(9748696), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16645887), ቖ.GetColor(14409183));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(3964849), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8225934), ቖ.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOverControl;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8225934), ቖ.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Pressed;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor(12904953), 0f),
			new BackgroundColorBlend(ቖ.GetColor(10870518), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(7326448), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(7326448), 0.7f),
			new BackgroundColorBlend(ቖ.GetColor(6468051), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(14939644), ቖ.GetColor(10405849));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1595786), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8225934), ቖ.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(14939644), ቖ.GetColor(10405849));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1595786), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(8487297)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.Disabled;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ድ(Office2007ColorTable ዲ, ColorFactory ቖ)
	{
		Office2007ScrollBarStateColorTable @default = ዲ.AppScrollBar.Default;
		@default.Background = new LinearGradientColorTable(ቖ.GetColor(4210752));
		@default.Border = new LinearGradientColorTable(ቖ.GetColor(6447714));
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(13290186), ቖ.GetColor(7960953), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(16382457), 0f),
			new BackgroundColorBlend(ቖ.GetColor(15395564), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(14343133), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(12698053), 1f)
		});
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16382457), ቖ.GetColor(13619154));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(3092271), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOver;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor(14479611), 0f),
			new BackgroundColorBlend(ቖ.GetColor(13232634), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11131894), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(11131894), 0.7f),
			new BackgroundColorBlend(ቖ.GetColor(9748696), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(16645887), ቖ.GetColor(14409183));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(3964849), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8618883), ቖ.GetColor(5131854), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = @default.ThumbInnerBorder;
		@default.TrackOuterBorder = @default.ThumbOuterBorder;
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(7303023)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.MouseOverControl;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.ThumbInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.ThumbOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8618883), ቖ.GetColor(5131854), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(ዲ.AppScrollBar.Default.TrackBackground);
		@default.TrackInnerBorder = ዲ.AppScrollBar.Default.TrackInnerBorder;
		@default.TrackOuterBorder = ዲ.AppScrollBar.Default.TrackOuterBorder;
		@default.TrackSignBackground = ዲ.AppScrollBar.Default.TrackSignBackground;
		@default = ዲ.AppScrollBar.Pressed;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbBackground.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor(12904953), 0f),
			new BackgroundColorBlend(ቖ.GetColor(10870518), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(7326448), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(7326448), 0.7f),
			new BackgroundColorBlend(ቖ.GetColor(6468051), 1f)
		});
		@default.ThumbInnerBorder = new LinearGradientColorTable(ቖ.GetColor(14939644), ቖ.GetColor(10405849));
		@default.ThumbOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1595786), Color.Empty);
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(8225934), ቖ.GetColor(4935509), 90);
		@default.TrackBackground.Clear();
		@default.TrackBackground.CopyFrom(@default.ThumbBackground);
		@default.TrackInnerBorder = new LinearGradientColorTable(ቖ.GetColor(14939644), ቖ.GetColor(10405849));
		@default.TrackOuterBorder = new LinearGradientColorTable(ቖ.GetColor(1595786), Color.Empty);
		@default.TrackSignBackground = new LinearGradientColorTable(Color.FromArgb(180, ቖ.GetColor(8487297)), Color.FromArgb(64, Color.White));
		@default = ዲ.AppScrollBar.Disabled;
		@default.Background = ዲ.AppScrollBar.Default.Background;
		@default.Border = ዲ.AppScrollBar.Default.Border;
		@default.ThumbBackground.Clear();
		@default.ThumbInnerBorder = new LinearGradientColorTable();
		@default.ThumbOuterBorder = new LinearGradientColorTable();
		@default.ThumbSignBackground = new LinearGradientColorTable(ቖ.GetColor(12570615), ቖ.GetColor(7502996));
		@default.TrackBackground.Clear();
		@default.TrackInnerBorder = new LinearGradientColorTable();
		@default.TrackOuterBorder = new LinearGradientColorTable();
		@default.TrackSignBackground = new LinearGradientColorTable();
	}

	public static void ዶ(Office2007ColorTable ቯ, ColorFactory ቖ)
	{
		ቯ.RibbonBar.Default = ዟ(ቖ);
		ቯ.RibbonBar.MouseOver = ዠ(ቖ);
		ቯ.RibbonBar.Expanded = ዡ(ቖ);
		ቯ.RibbonTabItemColors.Clear();
		Office2007RibbonTabItemColorTable office2007RibbonTabItemColorTable = ዢ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Default);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዤ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Green);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዣ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Magenta);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		office2007RibbonTabItemColorTable = ዥ(ቖ);
		office2007RibbonTabItemColorTable.Name = Enum.GetName(typeof(eRibbonTabColor), eRibbonTabColor.Orange);
		ቯ.RibbonTabItemColors.Add(office2007RibbonTabItemColorTable);
		ቯ.RibbonControl = new Office2007RibbonColorTable();
		ቯ.RibbonControl.TabsBackground = new LinearGradientColorTable(ቖ.GetColor(5460819), ቖ.GetColor(5460819));
		ቯ.RibbonControl.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13554393), ቖ.GetColor(12500670));
		ቯ.RibbonControl.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5460819), ቖ.GetColor(5197647));
		ቯ.RibbonControl.TabDividerBorder = ቖ.GetColor(4276545);
		ቯ.RibbonControl.TabDividerBorderLight = ቖ.GetColor(6710886);
		ቯ.RibbonControl.PanelTopBackground = new LinearGradientColorTable(ቖ.GetColor(14146528), ቖ.GetColor(12699343));
		ቯ.RibbonControl.PanelBottomBackground = new LinearGradientColorTable(ቖ.GetColor(11844549), ቖ.GetColor(15461355));
		ቯ.RibbonControl.StartButtonDefault = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonNormalBlack.png");
		ቯ.RibbonControl.StartButtonMouseOver = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonHotBlack.png");
		ቯ.RibbonControl.StartButtonPressed = (Image)(object)BarFunctions.ই("SystemImages.BlankStartButtonPressedBlack.png");
		ቯ.ItemGroup.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14081759), ቖ.GetColor(14410468));
		ቯ.ItemGroup.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13883610), ቖ.GetColor(14738919));
		ቯ.ItemGroup.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(14673638), ቖ.GetColor(15593713));
		ቯ.ItemGroup.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(11384763), ቖ.GetColor(9213338));
		ቯ.ItemGroup.ItemGroupDividerDark = Color.FromArgb(196, ቖ.GetColor(13553358));
		ቯ.ItemGroup.ItemGroupDividerLight = Color.FromArgb(128, ቖ.GetColor(16777215));
		ቯ.Bar.ToolbarTopBackground = new LinearGradientColorTable(ቖ.GetColor(3355443), ቖ.GetColor(3882566));
		ቯ.Bar.ToolbarBottomBackground = new LinearGradientColorTable(ቖ.GetColor(3092528), ቖ.GetColor(4539717));
		ቯ.Bar.ToolbarBottomBorder = ቖ.GetColor(5000268);
		ቯ.Bar.PopupToolbarBackground = new LinearGradientColorTable(ቖ.GetColor(16448250), Color.Empty);
		ቯ.Bar.PopupToolbarBorder = ቖ.GetColor(3092271);
		ቯ.Bar.StatusBarTopBorder = ቖ.GetColor(3355443);
		ቯ.Bar.StatusBarTopBorderLight = Color.Empty;
		ቯ.Bar.StatusBarAltBackground.Clear();
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(13026758), 0f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(11842740), 0.4f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(9935000), 0.4f));
		ቯ.Bar.StatusBarAltBackground.Add(new BackgroundColorBlend(ቖ.GetColor(4738130), 1f));
		ቯ.ButtonItemColors.Clear();
		ቯ.RibbonButtonItemColors.Clear();
		ቯ.MenuButtonItemColors.Clear();
		Office2007ButtonItemColorTable office2007ButtonItemColorTable = ዦ(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = የ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዩ(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዪ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ያ(ዧ: false, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዬ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዦ(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = የ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.OrangeWithBackground);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዩ(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Blue);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዪ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.BlueWithBackground);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ያ(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Magenta);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዬ(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.MagentaWithBackground);
		ቯ.RibbonButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ዦ(ዧ: true, ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Orange);
		office2007ButtonItemColorTable.Default.Text = ቖ.GetColor(16777215);
		office2007ButtonItemColorTable.MouseOver.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Checked.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Expanded.Text = ቖ.GetColor(0);
		office2007ButtonItemColorTable.Pressed.Text = ቖ.GetColor(0);
		ቯ.MenuButtonItemColors.Add(office2007ButtonItemColorTable);
		office2007ButtonItemColorTable = ው(ቖ);
		office2007ButtonItemColorTable.Name = Enum.GetName(typeof(eButtonColor), eButtonColor.Office2007WithBackground);
		ቯ.ButtonItemColors.Add(office2007ButtonItemColorTable);
		ቯ.ButtonItemColors.Add(ButtonItemStaticColorTables.CreateBlueOrbColorTable(ቖ));
		ቯ.RibbonTabGroupColors.Clear();
		Office2007RibbonTabGroupColorTable office2007RibbonTabGroupColorTable = ይ(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Default);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዮ(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Magenta);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ዯ(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Green);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		office2007RibbonTabGroupColorTable = ደ(ቖ);
		office2007RibbonTabGroupColorTable.Name = Enum.GetName(typeof(eRibbonTabGroupColor), eRibbonTabGroupColor.Orange);
		ቯ.RibbonTabGroupColors.Add(office2007RibbonTabGroupColorTable);
		ቯ.Menu.Background = new LinearGradientColorTable(ቖ.GetColor(16448250), Color.Empty);
		ቯ.Menu.Border = new LinearGradientColorTable(ቖ.GetColor(8816262), Color.Empty);
		ቯ.Menu.Side = new LinearGradientColorTable(ቖ.GetColor(15331054), Color.Empty);
		ቯ.Menu.SideBorder = new LinearGradientColorTable(ቖ.GetColor(12961221), Color.Empty);
		ቯ.Menu.SideBorderLight = new LinearGradientColorTable(ቖ.GetColor(16119285), Color.Empty);
		ቯ.Menu.SideUnused = new LinearGradientColorTable(ቖ.GetColor(15066597), Color.Empty);
		ቯ.Menu.FileBackgroundBlend.Clear();
		ቯ.Menu.FileBackgroundBlend.AddRange(new BackgroundColorBlend[2]
		{
			new BackgroundColorBlend(ቖ.GetColor(5328976), 0f),
			new BackgroundColorBlend(ቖ.GetColor(5131855), 1f)
		});
		ቯ.Menu.FileContainerBorder = ቖ.GetColor(7040113);
		ቯ.Menu.FileContainerBorderLight = ቖ.GetColor(7040113);
		ቯ.Menu.FileColumnOneBackground = ቖ.GetColor(16448250);
		ቯ.Menu.FileColumnOneBorder = ቖ.GetColor(12961221);
		ቯ.Menu.FileColumnTwoBackground = ቖ.GetColor(15330030);
		ቯ.Menu.FileBottomContainerBackgroundBlend.Clear();
		ቯ.Menu.FileBottomContainerBackgroundBlend.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor(4408914), 0f),
			new BackgroundColorBlend(ቖ.GetColor(3948618), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(3092271), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor(4210752), 1f)
		});
		ቯ.ComboBox.Default.Background = ቖ.GetColor(15263976);
		int color = 9013641;
		ቯ.ComboBox.Default.Border = ቖ.GetColor(color);
		ቯ.ComboBox.Default.ExpandBackground = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandBorderOuter = new LinearGradientColorTable();
		ቯ.ComboBox.Default.ExpandText = ቖ.GetColor(8158332);
		ቯ.ComboBox.DefaultStandalone.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DefaultStandalone.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DefaultStandalone.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(15528181), ቖ.GetColor(14673131), 90);
		ቯ.ComboBox.DefaultStandalone.ExpandBorderInner = new LinearGradientColorTable();
		ቯ.ComboBox.DefaultStandalone.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(12040119), Color.Empty, 90);
		ቯ.ComboBox.DefaultStandalone.ExpandText = ቖ.GetColor(0);
		ቯ.ComboBox.MouseOver.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.MouseOver.Border = ቖ.GetColor(color);
		ቯ.ComboBox.MouseOver.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(16776418), ቖ.GetColor(16770981), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(16776435), 90);
		ቯ.ComboBox.MouseOver.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(14405273), Color.Empty, 90);
		ቯ.ComboBox.MouseOver.ExpandText = ቖ.GetColor(0);
		ቯ.ComboBox.DroppedDown.Background = ቖ.GetColor(16777215);
		ቯ.ComboBox.DroppedDown.Border = ቖ.GetColor(color);
		ቯ.ComboBox.DroppedDown.ExpandBackground = new LinearGradientColorTable(ቖ.GetColor(15392959), ቖ.GetColor(16766038), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderInner = new LinearGradientColorTable(ቖ.GetColor(15854549), ቖ.GetColor(16770708), 90);
		ቯ.ComboBox.DroppedDown.ExpandBorderOuter = new LinearGradientColorTable(ቖ.GetColor(10129251), Color.Empty, 90);
		ቯ.ComboBox.DroppedDown.ExpandText = ቖ.GetColor(0);
		ቯ.DialogLauncher.Default.DialogLauncher = ቖ.GetColor(6645872);
		ቯ.DialogLauncher.Default.DialogLauncherShade = ቖ.GetColor(15461355);
		ቯ.DialogLauncher.MouseOver.DialogLauncher = ቖ.GetColor(6645872);
		ቯ.DialogLauncher.MouseOver.DialogLauncherShade = ቖ.GetColor(15461355);
		ቯ.DialogLauncher.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776415), ቖ.GetColor(16773031));
		ቯ.DialogLauncher.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767349), ቖ.GetColor(16769944));
		ቯ.DialogLauncher.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(16777211), ቖ.GetColor(16776178));
		ቯ.DialogLauncher.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(14405273), Color.Empty);
		ቯ.DialogLauncher.Pressed.DialogLauncher = ቖ.GetColor(6645872);
		ቯ.DialogLauncher.Pressed.DialogLauncherShade = ቖ.GetColor(15461355);
		ቯ.DialogLauncher.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(15261373), ቖ.GetColor(15386253));
		ቯ.DialogLauncher.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16754488), ቖ.GetColor(16763982));
		ቯ.DialogLauncher.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(15788756), ቖ.GetColor(16769937));
		ቯ.DialogLauncher.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(10129251), ቖ.GetColor(11576434));
		ዷ(ቯ.LegacyColors, ቖ);
		ቯ.SystemButton.Default = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Default.Foreground = new LinearGradientColorTable(ቖ.GetColor(8686741), ቖ.GetColor(10463671));
		ቯ.SystemButton.Default.LightShade = ቖ.GetColor(10463671);
		ቯ.SystemButton.Default.DarkShade = ቖ.GetColor(8487040);
		ቯ.SystemButton.MouseOver = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.MouseOver.Foreground = new LinearGradientColorTable(ቖ.GetColor(3552565), ቖ.GetColor(5199715));
		ቯ.SystemButton.MouseOver.LightShade = ቖ.GetColor(10199464);
		ቯ.SystemButton.MouseOver.DarkShade = ቖ.GetColor(4541526);
		ቯ.SystemButton.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(11647169), ቖ.GetColor(8950945));
		ቯ.SystemButton.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(6845829), ቖ.GetColor(10334657));
		ቯ.SystemButton.MouseOver.TopHighlight = new LinearGradientColorTable(ቖ.GetColor(13489114), Color.Transparent);
		ቯ.SystemButton.MouseOver.BottomHighlight = new LinearGradientColorTable(ቖ.GetColor(11918836), Color.Transparent);
		ቯ.SystemButton.MouseOver.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(5726317), ቖ.GetColor(6054505));
		ቯ.SystemButton.MouseOver.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(13489114), ቖ.GetColor(14344163));
		ቯ.SystemButton.Pressed = new Office2007SystemButtonStateColorTable();
		ቯ.SystemButton.Pressed.Foreground = new LinearGradientColorTable(ቖ.GetColor(7171180), ቖ.GetColor(9148068));
		ቯ.SystemButton.Pressed.LightShade = ቖ.GetColor(8358036);
		ቯ.SystemButton.Pressed.DarkShade = ቖ.GetColor(7171180);
		ቯ.SystemButton.Pressed.TopBackground = new LinearGradientColorTable(ቖ.GetColor(3618615), ቖ.GetColor(2829099));
		ቯ.SystemButton.Pressed.TopHighlight = new LinearGradientColorTable(ቖ.GetColor(6842472), Color.Transparent);
		ቯ.SystemButton.Pressed.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(0), ቖ.GetColor(461067));
		ቯ.SystemButton.Pressed.BottomHighlight = new LinearGradientColorTable(ቖ.GetColor(5335426), Color.Transparent);
		ቯ.SystemButton.Pressed.OuterBorder = new LinearGradientColorTable(ቖ.GetColor(2237481), ቖ.GetColor(1644825));
		ቯ.SystemButton.Pressed.InnerBorder = new LinearGradientColorTable(ቖ.GetColor(4605510), ቖ.GetColor(3355443));
		ቯ.Form.Active.BorderColors = new Color[5]
		{
			ቖ.GetColor(3092271),
			ቖ.GetColor(5066061),
			ቖ.GetColor(5460819),
			ቖ.GetColor(5460819),
			ቖ.GetColor(5460819)
		};
		ቯ.Form.Inactive.BorderColors = new Color[5]
		{
			ቖ.GetColor(9605778),
			ቖ.GetColor(10461087),
			ቖ.GetColor(11250603),
			ቖ.GetColor(5460819),
			ቖ.GetColor(5460819)
		};
		ቯ.Form.Active.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(4409170), ቖ.GetColor(3816773));
		ቯ.Form.Active.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(3092528), ቖ.GetColor(4079166));
		ቯ.Form.Active.CaptionBottomBorder = new Color[2]
		{
			ቖ.GetColor(4737096),
			ቖ.GetColor(4868682)
		};
		ቯ.Form.Active.CaptionText = ቖ.GetColor(11457023);
		ቯ.Form.Active.CaptionTextExtra = ቖ.GetColor(16777215);
		ቯ.Form.Inactive.CaptionTopBackground = new LinearGradientColorTable(ቖ.GetColor(10132641), ቖ.GetColor(9935004));
		ቯ.Form.Inactive.CaptionBottomBackground = new LinearGradientColorTable(ቖ.GetColor(9605778), ቖ.GetColor(10132122));
		ቯ.Form.Inactive.CaptionText = ቖ.GetColor(14803425);
		ቯ.Form.Inactive.CaptionTextExtra = ቖ.GetColor(14803425);
		ቯ.Form.BackColor = ቖ.GetColor(8224125);
		ቯ.Form.TextColor = ቖ.GetColor(16777215);
		ቯ.QuickAccessToolbar.Active.TopBackground = new LinearGradientColorTable(ቖ.GetColor(8093316), ቖ.GetColor(7961472));
		ቯ.QuickAccessToolbar.Active.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(7171694), ቖ.GetColor(4473924));
		ቯ.QuickAccessToolbar.Active.OutterBorderColor = ቖ.GetColor(4671303);
		ቯ.QuickAccessToolbar.Active.MiddleBorderColor = ቖ.GetColor(3487029);
		ቯ.QuickAccessToolbar.Active.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Inactive.TopBackground = new LinearGradientColorTable(ቖ.GetColor(10987434), ቖ.GetColor(11776950));
		ቯ.QuickAccessToolbar.Inactive.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(11382189), ቖ.GetColor(9803157));
		ቯ.QuickAccessToolbar.Inactive.OutterBorderColor = ቖ.GetColor(11119017);
		ቯ.QuickAccessToolbar.Inactive.MiddleBorderColor = ቖ.GetColor(6118749);
		ቯ.QuickAccessToolbar.Inactive.InnerBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.TopBackground = new LinearGradientColorTable();
		ቯ.QuickAccessToolbar.Standalone.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(9277587), ቖ.GetColor(8750985));
		ቯ.QuickAccessToolbar.Standalone.OutterBorderColor = ቖ.GetColor(6119524);
		ቯ.QuickAccessToolbar.Standalone.MiddleBorderColor = Color.Empty;
		ቯ.QuickAccessToolbar.Standalone.InnerBorderColor = ቖ.GetColor(13356238);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelBackground = ቖ.GetColor(15461355);
		ቯ.QuickAccessToolbar.QatCustomizeMenuLabelText = ቖ.GetColor(4605510);
		ቯ.QuickAccessToolbar.Active.GlassBorder = new LinearGradientColorTable(ቖ.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		ቯ.QuickAccessToolbar.Inactive.GlassBorder = new LinearGradientColorTable(ቖ.GetColor(Color.FromArgb(132, Color.Black)), Color.FromArgb(80, Color.Black));
		ቯ.TabControl.Default = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Default.TopBackground = new LinearGradientColorTable(ቖ.GetColor(14212322), ቖ.GetColor(11845063));
		ቯ.TabControl.Default.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(13949407), ቖ.GetColor(15461355));
		ቯ.TabControl.Default.InnerBorder = ቖ.GetColor(15461355);
		ቯ.TabControl.Default.OuterBorder = ቖ.GetColor(4605510);
		ቯ.TabControl.Default.Text = ቖ.GetColor(3158064);
		ቯ.TabControl.MouseOver = new Office2007TabItemStateColorTable();
		ቯ.TabControl.MouseOver.TopBackground = new LinearGradientColorTable(ቖ.GetColor(16776683), ቖ.GetColor(16772264));
		ቯ.TabControl.MouseOver.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16767577), ቖ.GetColor(16770701));
		ቯ.TabControl.MouseOver.InnerBorder = ቖ.GetColor(16777211);
		ቯ.TabControl.MouseOver.OuterBorder = ቖ.GetColor(11967859);
		ቯ.TabControl.MouseOver.Text = ቖ.GetColor(3552822);
		ቯ.TabControl.Selected = new Office2007TabItemStateColorTable();
		ቯ.TabControl.Selected.TopBackground = new LinearGradientColorTable(ቖ.GetColor(Color.White), ቖ.GetColor(16250871));
		ቯ.TabControl.Selected.BottomBackground = new LinearGradientColorTable(ቖ.GetColor(16250871), ቖ.GetColor(16250871));
		ቯ.TabControl.Selected.InnerBorder = ቖ.GetColor(Color.White);
		ቯ.TabControl.Selected.OuterBorder = ቖ.GetColor(4605510);
		ቯ.TabControl.Selected.Text = ቖ.GetColor(2565927);
		ቯ.TabControl.TabBackground = new LinearGradientColorTable(ቖ.GetColor(16645629), ቖ.GetColor(12829635));
		ቯ.TabControl.TabPanelBackground = new LinearGradientColorTable(ቖ.GetColor(16250871), ቖ.GetColor(12829635));
		ቯ.TabControl.TabPanelBorder = ቖ.GetColor(4605510);
		Office2007CheckBoxColorTable checkBoxItem = ቯ.CheckBoxItem;
		checkBoxItem.Default.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16053492), Color.Empty);
		checkBoxItem.Default.CheckBorder = ቖ.GetColor(8684676);
		checkBoxItem.Default.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(10661049)), Color.FromArgb(164, ቖ.GetColor(16185078)));
		checkBoxItem.Default.CheckInnerBorder = ቖ.GetColor(10661049);
		checkBoxItem.Default.CheckSign = new LinearGradientColorTable(ቖ.GetColor(5204093), Color.Empty);
		checkBoxItem.Default.Text = ቖ.GetColor(0);
		checkBoxItem.MouseOver.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16053492), Color.Empty);
		checkBoxItem.MouseOver.CheckBorder = ቖ.GetColor(8684676);
		checkBoxItem.MouseOver.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(16438650)), Color.FromArgb(128, ቖ.GetColor(16709863)));
		checkBoxItem.MouseOver.CheckInnerBorder = ቖ.GetColor(16438650);
		checkBoxItem.MouseOver.CheckSign = new LinearGradientColorTable(ቖ.GetColor(5204093), Color.Empty);
		checkBoxItem.MouseOver.Text = ቖ.GetColor(0);
		checkBoxItem.Pressed.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(15068407), Color.Empty);
		checkBoxItem.Pressed.CheckBorder = ቖ.GetColor(8684676);
		checkBoxItem.Pressed.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(15894822)), Color.FromArgb(164, ቖ.GetColor(16774357)));
		checkBoxItem.Pressed.CheckInnerBorder = ቖ.GetColor(15894822);
		checkBoxItem.Pressed.CheckSign = new LinearGradientColorTable(ቖ.GetColor(5204093), Color.Empty);
		checkBoxItem.Pressed.Text = ቖ.GetColor(0);
		checkBoxItem.Disabled.CheckBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), Color.Empty);
		checkBoxItem.Disabled.CheckBorder = ቖ.GetColor(11448757);
		checkBoxItem.Disabled.CheckInnerBackground = new LinearGradientColorTable(Color.FromArgb(192, ቖ.GetColor(14738149)), Color.FromArgb(164, ቖ.GetColor(16514043)));
		checkBoxItem.Disabled.CheckInnerBorder = ቖ.GetColor(14738149);
		checkBoxItem.Disabled.CheckSign = new LinearGradientColorTable(ቖ.GetColor(9276813), Color.Empty);
		checkBoxItem.Disabled.Text = ቖ.GetColor(9276813);
		ዱ(ቯ, ቖ);
		ድ(ቯ, ቖ);
		Office2007ProgressBarColorTable progressBarItem = ቯ.ProgressBarItem;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = ቖ.GetColor(13421772);
		progressBarItem.InnerBorder = ቖ.GetColor(2434341);
		progressBarItem.Chunk = new GradientColorTable(6788896, 12779350, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(164, ቖ.GetColor(15663063)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(9286228)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(6918699)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(8750469, 9474448, 0);
		progressBarItem = ቯ.ProgressBarItemPaused;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = ቖ.GetColor(13421772);
		progressBarItem.InnerBorder = ቖ.GetColor(2434341);
		progressBarItem.Chunk = new GradientColorTable(11446016, 16776653, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16776099)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13814272)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16708608)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(8750469, 9474448, 0);
		progressBarItem = ቯ.ProgressBarItemError;
		progressBarItem.BackgroundColors = new GradientColorTable(8882571, 9934999);
		progressBarItem.OuterBorder = ቖ.GetColor(13421772);
		progressBarItem.InnerBorder = ቖ.GetColor(2434341);
		progressBarItem.Chunk = new GradientColorTable(13762560, 16764365, 0);
		progressBarItem.ChunkOverlay = new GradientColorTable();
		progressBarItem.ChunkOverlay.LinearGradientAngle = 90;
		progressBarItem.ChunkOverlay.Colors.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(Color.FromArgb(192, ቖ.GetColor(16748431)), 0f),
			new BackgroundColorBlend(Color.FromArgb(128, ቖ.GetColor(13762560)), 0.5f),
			new BackgroundColorBlend(Color.FromArgb(64, ቖ.GetColor(16646144)), 0.5f),
			new BackgroundColorBlend(Color.Transparent, 1f)
		});
		progressBarItem.ChunkShadow = new GradientColorTable(8750469, 9474448, 0);
		Office2007GalleryColorTable gallery = ቯ.Gallery;
		gallery.GroupLabelBackground = ቖ.GetColor(15461355);
		gallery.GroupLabelText = ቖ.GetColor(0);
		gallery.GroupLabelBorder = ቖ.GetColor(12961221);
		ቯ.ListViewEx.Border = ቖ.GetColor(color);
		ቯ.ListViewEx.ColumnBackground = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(13948891));
		ቯ.ListViewEx.ColumnSeparator = ቖ.GetColor(9542052);
		ቯ.ListViewEx.ColumnForeColor = ቖ.GetColor(0);
		ቯ.ListViewEx.SelectionBackground = new LinearGradientColorTable(ቖ.GetColor(10997232), Color.Empty);
		ቯ.ListViewEx.SelectionBorder = ቖ.GetColor(14938111);
		ቯ.NavigationPane.ButtonBackground = new GradientColorTable();
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16316665), 0f));
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14672612), 0.4f));
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(13093841), 0.4f));
		ቯ.NavigationPane.ButtonBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14409442), 1f));
		ቯ.SuperTooltip.BackgroundColors = new LinearGradientColorTable(ቖ.GetColor(16777215), ቖ.GetColor(15000816));
		ቯ.SuperTooltip.TextColor = ቖ.GetColor(5000268);
		Office2007SliderColorTable slider = ቯ.Slider;
		slider.Default.LabelColor = ቖ.GetColor(16777215);
		slider.Default.SliderLabelColor = ቖ.GetColor(0);
		slider.Default.PartBackground = new GradientColorTable();
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15724527), 0.15f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(12501699), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(7106936), 0.5f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14474462), 0.85f));
		slider.Default.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 1f));
		slider.Default.PartBorderColor = ቖ.GetColor(3751750);
		slider.Default.PartBorderLightColor = Color.FromArgb(28, ቖ.GetColor(16777215));
		slider.Default.PartForeColor = ቖ.GetColor(5989228);
		slider.Default.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(15395562));
		slider.Default.TrackLineColor = ቖ.GetColor(2434341);
		slider.Default.TrackLineLightColor = ቖ.GetColor(13421772);
		slider.MouseOver.LabelColor = ቖ.GetColor(16777215);
		slider.MouseOver.SliderLabelColor = ቖ.GetColor(0);
		slider.MouseOver.PartBackground = new GradientColorTable();
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777215), 0f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16777214), 0.2f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16046995), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15645223), 0.5f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16576191), 0.85f));
		slider.MouseOver.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16445650), 1f));
		slider.MouseOver.PartBorderColor = ቖ.GetColor(2960685);
		slider.MouseOver.PartBorderLightColor = Color.FromArgb(28, ቖ.GetColor(16777215));
		slider.MouseOver.PartForeColor = ቖ.GetColor(6775369);
		slider.MouseOver.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(16774356));
		slider.MouseOver.TrackLineColor = ቖ.GetColor(2434341);
		slider.MouseOver.TrackLineLightColor = ቖ.GetColor(13421772);
		slider.Pressed.LabelColor = ቖ.GetColor(16777215);
		slider.Pressed.SliderLabelColor = ቖ.GetColor(0);
		slider.Pressed.PartBackground = new GradientColorTable();
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15109178), 0f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15901267), 0.2f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16368011), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15958807), 0.5f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16301446), 0.85f));
		slider.Pressed.PartBackground.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(15979188), 1f));
		slider.Pressed.PartBorderColor = ቖ.GetColor(2960685);
		slider.Pressed.PartBorderLightColor = Color.FromArgb(28, ቖ.GetColor(16777215));
		slider.Pressed.PartForeColor = ቖ.GetColor(6771523);
		slider.Pressed.PartForeLightColor = Color.FromArgb(168, ቖ.GetColor(16768187));
		slider.Pressed.TrackLineColor = ቖ.GetColor(2434341);
		slider.Pressed.TrackLineLightColor = ቖ.GetColor(13421772);
		ColorBlendFactory colorBlendFactory = new ColorBlendFactory(ColorScheme.GetColor(13619151));
		slider.Disabled.LabelColor = ቖ.GetColor(9276813);
		slider.Disabled.SliderLabelColor = ቖ.GetColor(9276813);
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
		Office2007ColorTable.ቮ(ቯ, ቖ);
		ቯ.DataGridView.ColumnHeaderNormalBorder = ቖ.GetColor(11974326);
		ቯ.DataGridView.ColumnHeaderNormalBackground = new LinearGradientColorTable(ቖ.GetColor(16316664), ቖ.GetColor(14606046), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16374175), ቖ.GetColor(15843679), 90);
		ቯ.DataGridView.ColumnHeaderSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(16766349), ቖ.GetColor(15897146), 90);
		ቯ.DataGridView.ColumnHeaderSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.ColumnHeaderMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(14737632), ቖ.GetColor(12829635), 90);
		ቯ.DataGridView.ColumnHeaderMouseOverBorder = ቖ.GetColor(11974326);
		ቯ.DataGridView.ColumnHeaderPressedBackground = new LinearGradientColorTable(ቖ.GetColor(14737632), ቖ.GetColor(12829635), 90);
		ቯ.DataGridView.ColumnHeaderPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.RowNormalBackground = new LinearGradientColorTable(ቖ.GetColor(15592941));
		ቯ.DataGridView.RowNormalBorder = ቖ.GetColor(11974326);
		ቯ.DataGridView.RowSelectedBackground = new LinearGradientColorTable(ቖ.GetColor(16766349));
		ቯ.DataGridView.RowSelectedBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowSelectedMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowSelectedMouseOverBorder = ቖ.GetColor(15897910);
		ቯ.DataGridView.RowMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowMouseOverBorder = ቖ.GetColor(11974326);
		ቯ.DataGridView.RowPressedBackground = new LinearGradientColorTable(ቖ.GetColor(15843420));
		ቯ.DataGridView.RowPressedBorder = ቖ.GetColor(16777215);
		ቯ.DataGridView.GridColor = ቖ.GetColor(13686757);
		ቯ.DataGridView.SelectorBackground = new LinearGradientColorTable(ቖ.GetColor(13224393));
		ቯ.DataGridView.SelectorBorder = ቖ.GetColor(11974326);
		ቯ.DataGridView.SelectorBorderDark = ቖ.GetColor(13421772);
		ቯ.DataGridView.SelectorBorderLight = ቖ.GetColor(15461355);
		ቯ.DataGridView.SelectorSign = new LinearGradientColorTable(ቖ.GetColor(8224125), ቖ.GetColor(6776679));
		ቯ.DataGridView.SelectorMouseOverBackground = new LinearGradientColorTable(ቖ.GetColor(11382189), ቖ.GetColor(10461087));
		ቯ.DataGridView.SelectorMouseOverBorder = ቖ.GetColor(11974326);
		ቯ.DataGridView.SelectorMouseOverBorderDark = ቖ.GetColor(13421772);
		ቯ.DataGridView.SelectorMouseOverBorderLight = ቖ.GetColor(15461355);
		ቯ.DataGridView.SelectorMouseOverSign = new LinearGradientColorTable(ቖ.GetColor(8224125), ቖ.GetColor(6776679));
		ቯ.AdvTree = new TreeColorTable();
		_135B._135E(ቯ.AdvTree, ቖ);
		ቯ.CrumbBarItemView = new CrumbBarItemViewColorTable();
		CrumbBarItemViewStateColorTable crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Default = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(0);
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOver = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(0);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFCD9"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFE78F"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFD74A"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFE794"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FFDDCF9B");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("80FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.MouseOverInactive = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(0);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[4]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFFFFDEC"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF4CA"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFEBA6"), 0.4f),
			new BackgroundColorBlend(ቖ.GetColor("FFFFF2C5"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8E8F8F");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("90FFFFFF");
		crumbBarItemViewStateColorTable = new CrumbBarItemViewStateColorTable();
		ቯ.CrumbBarItemView.Pressed = crumbBarItemViewStateColorTable;
		crumbBarItemViewStateColorTable.Foreground = ቖ.GetColor(0);
		crumbBarItemViewStateColorTable.Background = new BackgroundColorBlendCollection();
		crumbBarItemViewStateColorTable.Background.AddRange(new BackgroundColorBlend[5]
		{
			new BackgroundColorBlend(ቖ.GetColor("FFC89861"), 0f),
			new BackgroundColorBlend(ቖ.GetColor("FFEDAE68"), 0.1f),
			new BackgroundColorBlend(ቖ.GetColor("FFFCA160"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFC8F3D"), 0.6f),
			new BackgroundColorBlend(ቖ.GetColor("FFFEBD61"), 1f)
		});
		crumbBarItemViewStateColorTable.Border = ቖ.GetColor("FF8B7654");
		crumbBarItemViewStateColorTable.BorderLight = ቖ.GetColor("408B7654");
		ቯ.StyleClasses.Clear();
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonGalleryContainerKey;
		elementStyle.BorderColor = ቖ.GetColor(11316396);
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerDiameter = 2;
		elementStyle.CornerType = eCornerType.Rounded;
		elementStyle.BackColor = ቖ.GetColor(14344930);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዸ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዹ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዺ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዻ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዼ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዽ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ዿ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጀ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጂ(ቖ.GetColor(color));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጋ(ቖ.GetColor(color), SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጌ(ቖ, eOffice2007ColorScheme.Black);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጏ(ቯ.ListViewEx);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _1311(ቯ.Bar);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጅ(ቖ.GetColor(color), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጆ(ቖ.GetColor(16316664), ቖ.GetColor(14606046), ቖ.GetColor(11974326));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጉ(ቖ.GetColor(16316664), ቖ.GetColor(14606046), ቖ.GetColor(11974326));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጊ(ቖ.GetColor(0));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጒ(ቖ.GetColor(Color.White), ቖ.GetColor("FF333333"), ቖ.GetColor("FF252525"));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጕ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _1317();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = _1316();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጘ();
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጙ(ቯ.CheckBoxItem.Default.CheckBorder);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጃ(ቖ.GetColor(Color.White));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጄ(ቖ.GetColor(Color.DarkGray));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጚ(SystemColors.Window);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጛ(ቯ);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጜ(ቖ.GetColor(5460819));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጝ(ቖ.GetColor(16119543));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጞ(ቖ.GetColor(ቯ.LegacyColors.PanelBorder), Color.Empty);
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጟ(ቖ.GetColor(0));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጠ(ቖ.GetColor(0), ቖ.GetColor(14536603));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		elementStyle = ጡ(ቖ.GetColor(0), ቖ.GetColor(11974840));
		ቯ.StyleClasses.Add(elementStyle.Class, elementStyle);
		ቯ.SideBar.Background = new LinearGradientColorTable(ቖ.GetColor(Color.White));
		ቯ.SideBar.Border = ቖ.GetColor(10988982);
		ቯ.SideBar.SideBarPanelItemText = ቖ.GetColor(Color.Black);
		ቯ.SideBar.SideBarPanelItemDefault = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16316665), 0f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14672612), 0.4f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(13093841), 0.4f));
		ቯ.SideBar.SideBarPanelItemDefault.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(14409442), 1f));
		ቯ.SideBar.SideBarPanelItemExpanded = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16505781), 0f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16697208), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16692310), 0.4f));
		ቯ.SideBar.SideBarPanelItemExpanded.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16640927), 1f));
		ቯ.SideBar.SideBarPanelItemMouseOver = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16776409), 0f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770957), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16766792), 0.4f));
		ቯ.SideBar.SideBarPanelItemMouseOver.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16770963), 1f));
		ቯ.SideBar.SideBarPanelItemPressed = new GradientColorTable();
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16300137), 0f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16622433), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16484924), 0.4f));
		ቯ.SideBar.SideBarPanelItemPressed.Colors.Add(new BackgroundColorBlend(ቖ.GetColor(16694112), 1f));
		ቯ.WarningBox.BackColor = ቖ.GetColor(Color.FromArgb(64, 64, 64));
		ቯ.WarningBox.WarningBorderColor = ቖ.GetColor(Color.FromArgb(86, 86, 86));
		ቯ.WarningBox.WarningBackColor1 = ቖ.GetColor(Color.FromArgb(255, 255, 255));
		ቯ.WarningBox.WarningBackColor2 = ቖ.GetColor(Color.FromArgb(234, 234, 234));
		ቯ.CalendarView.WeekDayViewColors = new ColorDef[22]
		{
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(5002076)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(11844289)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11844289)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(16119285)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.TimeRulerColors = new ColorDef[6]
		{
			new ColorDef(ቖ.GetColor(15790578)),
			new ColorDef(ቖ.GetColor(3355443)),
			new ColorDef(ቖ.GetColor(3355443)),
			new ColorDef(ቖ.GetColor(3355443)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(15436032))
		};
		ቯ.CalendarView.MonthViewColors = new ColorDef[24]
		{
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }, 0f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(13883098),
				ቖ.GetColor(11844289),
				ቖ.GetColor(13356756)
			}, new float[4] { 0f, 0.55f, 0.58f, 1f }),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11581118)),
			new ColorDef(ቖ.GetColor(15264492)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(13093841)),
			new ColorDef(ቖ.GetColor(9542052)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(14475234),
				ቖ.GetColor(11844289)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(11844289)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(15436032)),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(new Color[4]
			{
				ቖ.GetColor(16772473),
				ቖ.GetColor(16767083),
				ቖ.GetColor(16759552),
				ቖ.GetColor(16771703)
			}, new float[4] { 0f, 0.55f, 58f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(0)),
			new ColorDef(ቖ.GetColor(16777215))
		};
		ቯ.CalendarView.AppointmentColors = new ColorDef[17]
		{
			new ColorDef(ቖ.GetColor(4944290)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16645887),
				ቖ.GetColor(12702698)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2642318)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(11650540),
				ቖ.GetColor(7708122)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(2909476)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12773564),
				ቖ.GetColor(8704379)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(9125386)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16369568),
				ቖ.GetColor(16029528)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(4073329)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(12957158),
				ቖ.GetColor(9796562)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8787740)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(15837868),
				ቖ.GetColor(15034222)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(8157204)),
			new ColorDef(new Color[2]
			{
				ቖ.GetColor(16776362),
				ቖ.GetColor(16775512)
			}, new float[2] { 0f, 1f }, 90f),
			new ColorDef(ቖ.GetColor(-1)),
			new ColorDef(ቖ.GetColor(16777215)),
			new ColorDef(ቖ.GetColor(8388736))
		};
		ቯ.SuperTab.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(13554393));
		ቯ.SuperTab.InnerBorder = ቖ.GetColor(15988213);
		ቯ.SuperTab.OuterBorder = ቖ.GetColor(12500670);
		ቯ.SuperTab.ControlBoxDefault.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.ControlBoxMouseOver.Background = ቖ.GetColor(16770978);
		ቯ.SuperTab.ControlBoxMouseOver.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxMouseOver.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.ControlBoxPressed.Background = ቖ.GetColor(16485436);
		ቯ.SuperTab.ControlBoxPressed.Border = ቖ.GetColor(16760169);
		ቯ.SuperTab.ControlBoxPressed.Image = ቖ.GetColor(255, 0);
		ቯ.SuperTab.InsertMarker = ቖ.GetColor(255, 128);
		ቯ.SuperTabItem.Default.Normal.Background = new SuperTabLinearGradientColorTable();
		ቯ.SuperTabItem.Default.Normal.Text = ቖ.GetColor(0);
		ቯ.SuperTabItem.Default.Normal.CloseMarker = ቖ.GetColor(0);
		ቯ.SuperTabItem.Default.Disabled.Text = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Disabled.Background.AdaptiveGradient = false;
		ቯ.SuperTabItem.Default.Disabled.CloseMarker = ቖ.GetColor(9276813);
		ቯ.SuperTabItem.Default.Selected.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(15658735),
			ቖ.GetColor(13554393)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.Selected.InnerBorder = ቖ.GetColor(15988213);
		ቯ.SuperTabItem.Default.Selected.OuterBorder = ቖ.GetColor(12500670);
		ቯ.SuperTabItem.Default.Selected.Text = ቖ.GetColor(0);
		ቯ.SuperTabItem.Default.Selected.CloseMarker = ቖ.GetColor(0);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(15593199),
			ቖ.GetColor(13291478)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.SelectedMouseOver.InnerBorder = ቖ.GetColor(16444354);
		ቯ.SuperTabItem.Default.SelectedMouseOver.OuterBorder = ቖ.GetColor(15252338);
		ቯ.SuperTabItem.Default.SelectedMouseOver.Text = ቖ.GetColor(0);
		ቯ.SuperTabItem.Default.SelectedMouseOver.CloseMarker = ቖ.GetColor(0);
		ቯ.SuperTabItem.Default.MouseOver.Background = new SuperTabLinearGradientColorTable(new Color[2]
		{
			ቖ.GetColor(14406851),
			ቖ.GetColor(15322765)
		}, new float[2] { 0f, 1f });
		ቯ.SuperTabItem.Default.MouseOver.InnerBorder = ቖ.GetColor(15264494);
		ቯ.SuperTabItem.Default.MouseOver.OuterBorder = ቖ.GetColor(12435137);
		ቯ.SuperTabItem.Default.MouseOver.Text = ቖ.GetColor(0);
		ቯ.SuperTabItem.Default.MouseOver.CloseMarker = ቖ.GetColor(0);
		ቯ.SuperTabItem.Left = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Bottom = ቯ.SuperTabItem.Default;
		ቯ.SuperTabItem.Right = ቯ.SuperTabItem.Default;
		ቯ.SuperTabPanel.Default.Background = new SuperTabLinearGradientColorTable(ቖ.GetColor(13554393));
		ቯ.SuperTabPanel.Default.InnerBorder = ቖ.GetColor(15988213);
		ቯ.SuperTabPanel.Default.OuterBorder = ቖ.GetColor(12500670);
		ቯ.SuperTabPanel.Left = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Bottom = ቯ.SuperTabPanel.Default;
		ቯ.SuperTabPanel.Right = ቯ.SuperTabPanel.Default;
		ጦ.ጰ(ቯ.Backstage, ቖ);
		ጦ.ጷ(ቯ.BackstageItem, ቖ);
		ጦ.ጽ(ቯ.BackstagePanel, ቖ);
		SwitchButtonColorTable switchButtonColorTable = new SwitchButtonColorTable();
		switchButtonColorTable.BorderColor = ቖ.GetColor(color);
		switchButtonColorTable.OffBackColor = ቖ.GetColor(13225686);
		switchButtonColorTable.OffTextColor = ቖ.GetColor(0);
		switchButtonColorTable.OnBackColor = ቖ.GetColor(9621584);
		switchButtonColorTable.OnTextColor = ቖ.GetColor(0);
		switchButtonColorTable.SwitchBackColor = ቖ.GetColor(11843254);
		switchButtonColorTable.SwitchBorderColor = ቖ.GetColor(9013899);
		switchButtonColorTable.TextColor = ቖ.GetColor(0);
		ቯ.SwitchButton = new SwitchButtonColors();
		ቯ.SwitchButton.Default = switchButtonColorTable;
		ቯ.SwitchButton.Disabled.BorderColor = ቯ.CheckBoxItem.Disabled.CheckBorder;
		ቯ.SwitchButton.Disabled.SwitchBorderColor = ቯ.SwitchButton.Disabled.BorderColor;
		ቯ.SwitchButton.Disabled.OffTextColor = ቯ.CheckBoxItem.Disabled.Text;
		ቯ.SwitchButton.Disabled.OnTextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.TextColor = ቯ.SwitchButton.Disabled.OffTextColor;
		ቯ.SwitchButton.Disabled.SwitchBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OffBackColor = ቯ.CheckBoxItem.Disabled.CheckInnerBackground.Start;
		ቯ.SwitchButton.Disabled.OnBackColor = ቯ.SwitchButton.Disabled.OffBackColor;
		ቯ.StepIndicator.BackgroundColor = ቖ.GetColor(ColorFunctions.GetShade(ቯ.Form.BackColor, 10));
		ቯ.StepIndicator.IndicatorColor = ቖ.GetColor(10800168);
		ቯ.RadialMenu = new RadialMenuColorTable();
		ቯ.RadialMenu.CircularBackColor = ቖ.GetColor(13913902);
		ቯ.RadialMenu.CircularBorderColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.CircularForeColor = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuBorder = ቖ.GetColor(3882566);
		ቯ.RadialMenu.RadialMenuButtonBackground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuButtonBorder = ቖ.GetColor(3882566);
		ቯ.RadialMenu.RadialMenuExpandForeground = ቖ.GetColor(16777215);
		ቯ.RadialMenu.RadialMenuInactiveBorder = Color.FromArgb(128, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.RadialMenu.RadialMenuItemForeground = ቖ.GetColor(3882566);
		ቯ.RadialMenu.RadialMenuItemDisabledForeground = ቖ.GetColor(9276813);
		ቯ.RadialMenu.RadialMenuItemMouseOverBackground = Color.FromArgb(72, ቯ.RadialMenu.RadialMenuItemForeground);
		ቯ.RadialMenu.RadialMenuItemMouseOverForeground = ቖ.GetColor(3882566);
		ቯ.RadialMenu.RadialMenuMouseOverBorder = Color.FromArgb(200, ቯ.RadialMenu.RadialMenuBorder);
		ቯ.StepItem.Default = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16316665),
			ቖ.GetColor(14672612),
			ቖ.GetColor(13093841),
			ቖ.GetColor(14409442)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(0), new Color[1] { ቖ.GetColor(9013125) });
		ቯ.StepItem.Progress = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(14218615),
			ቖ.GetColor(7717632),
			ቖ.GetColor(7717632),
			ቖ.GetColor(11135267)
		}, 90, new float[4] { 0f, 0.6f, 0.6f, 1f }, Color.Empty, new Color[0]);
		ቯ.StepItem.MouseOver = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16776409),
			ቖ.GetColor(16770957),
			ቖ.GetColor(16766792),
			ቖ.GetColor(16770963)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(0), new Color[1] { ቖ.GetColor(14536603) });
		ቯ.StepItem.Pressed = new OfficeStepItemStateColorTable(new Color[4]
		{
			ቖ.GetColor(16299370),
			ቖ.GetColor(16556128),
			ቖ.GetColor(16484924),
			ቖ.GetColor(16694626)
		}, 90, new float[4] { 0f, 0.5f, 0.5f, 1f }, ቖ.GetColor(0), new Color[1] { ቖ.GetColor(9139796) });
		ቯ.ListBoxItem.Default = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0));
		ቯ.ListBoxItem.MouseOver = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0), new Color[2]
		{
			ቖ.GetColor(16776409),
			ቖ.GetColor(16770963)
		}, 90, new float[2] { 0f, 1f });
		ቯ.ListBoxItem.Selected = new OfficeListBoxItemStateColorTable(ቖ.GetColor(0), new Color[2]
		{
			ቖ.GetColor(16569720),
			ቖ.GetColor(16500815)
		}, 90, new float[2] { 0f, 1f });
		ቯ.TokenEditor = new TokenEditorColorTable(16777215, 3949137, 16777215, 1842726, 16777215, 3104425);
		ቯ.Flyout.BorderColor = ቯ.Form.Active.BorderColors[0];
		ቯ.SideNav.SideNavItem.Default = new SideNavItemStateColorTable(ቖ.GetColor(0));
		ቯ.SideNav.SideNavItem.MouseOver = new SideNavItemStateColorTable(ቖ.GetColor(0), new Color[4]
		{
			ቖ.GetColor(16776409),
			ቖ.GetColor(16770957),
			ቖ.GetColor(16766792),
			ቖ.GetColor(16770963)
		}, new Color[2]
		{
			ቖ.GetColor(12625782),
			ቖ.GetColor(16773822)
		}, 2);
		ቯ.SideNav.SideNavItem.Pressed = new SideNavItemStateColorTable(ቖ.GetColor(0), new Color[4]
		{
			ቖ.GetColor(16300137),
			ቖ.GetColor(16622433),
			ቖ.GetColor(16484924),
			ቖ.GetColor(16694112)
		}, new Color[2]
		{
			ቖ.GetColor(6258614),
			ቖ.GetColor(16624913)
		}, 2);
		ቯ.SideNav.SideNavItem.Selected = new SideNavItemStateColorTable(ቖ.GetColor(0), new Color[1] { ቯ.Form.BackColor }, new Color[4]
		{
			Color.Empty,
			ቖ.GetColor(12500670),
			Color.Empty,
			ቖ.GetColor(12500670)
		});
		ቯ.SideNav.BorderColors = new Color[1] { ቖ.GetColor(12500670) };
		ቯ.SideNav.TitleBackColor = ቖ.GetColor(4276545);
		ቯ.SideNav.TitleBorderColors = new Color[4]
		{
			Color.Empty,
			Color.Empty,
			Color.Empty,
			ቖ.GetColor(12500670)
		};
		ቯ.SideNav.PanelBackColor = ቯ.Form.BackColor;
		ቯ.TabForm.Active.CaptionText = ቖ.GetColor(0);
		ቯ.TabForm.Inactive.CaptionText = ቖ.GetColor(8224125);
		ቯ.TabForm.Active.CaptionBackColors = new Color[1] { ቖ.GetColor(16119543) };
		ቯ.TabForm.Inactive.CaptionBackColors = new Color[1] { ቖ.GetColor(16119543) };
		ቯ.TabFormItemColorTables.Add(TabFormItem.DefaultColorTableKey, Office2007ColorTable._1257(ቖ, 0));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueColorTableKey, Office2007ColorTable._125E(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.YellowColorTableKey, Office2007ColorTable._125F(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleColorTableKey, Office2007ColorTable.ቡ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.CyanColorTableKey, Office2007ColorTable.ቢ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.BlueMistColorTableKey, Office2007ColorTable.ቦ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.PurpleMistColorTableKey, Office2007ColorTable.ቧ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TanColorTableKey, Office2007ColorTable.ቨ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.LemonLimeColorTableKey, Office2007ColorTable.ቩ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.AppleColorTableKey, Office2007ColorTable.ቪ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.TealColorTableKey, Office2007ColorTable.ቫ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedChalkColorTableKey, Office2007ColorTable.ቭ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.SilverColorTableKey, Office2007ColorTable.ቬ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.OrangeColorTableKey, Office2007ColorTable.ባ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.GreenColorTableKey, Office2007ColorTable.ቤ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.MagentaColorTableKey, Office2007ColorTable.ብ(ቖ));
		ቯ.TabFormItemColorTables.Add(TabFormItem.RedColorTableKey, Office2007ColorTable.በ(ቖ));
	}

	public static void ዷ(ColorScheme _0BDE, ColorFactory ቖ)
	{
		_0BDE.BarBackground = ቖ.GetColor(13488341);
		_0BDE.BarBackground2 = ቖ.GetColor(9147035);
		_0BDE.BarStripeColor = ቖ.GetColor(3619907);
		_0BDE.BarCaptionBackground = ቖ.GetColor(7766158);
		_0BDE.BarCaptionBackground2 = ቖ.GetColor(5002076);
		_0BDE.BarCaptionInactiveBackground = ቖ.GetColor(15790578);
		_0BDE.BarCaptionInactiveBackground2 = ቖ.GetColor(12436169);
		_0BDE.BarCaptionInactiveText = ቖ.GetColor(3619907);
		_0BDE.BarCaptionText = ቖ.GetColor(16777215);
		_0BDE.BarFloatingBorder = ቖ.GetColor(3619907);
		_0BDE.BarPopupBackground = ቖ.GetColor(16185078);
		_0BDE.BarPopupBorder = ቖ.GetColor(9542052);
		_0BDE.ItemBackground = Color.Empty;
		_0BDE.ItemBackground2 = Color.Empty;
		_0BDE.ItemCheckedBackground = ቖ.GetColor(16765802);
		_0BDE.ItemCheckedBackground2 = ቖ.GetColor(16500815);
		_0BDE.ItemCheckedBorder = ቖ.GetColor(12276995);
		_0BDE.ItemCheckedText = ቖ.GetColor(0);
		_0BDE.ItemDisabledBackground = Color.Empty;
		_0BDE.ItemDisabledText = ቖ.GetColor(9276813);
		_0BDE.ItemExpandedShadow = Color.Empty;
		_0BDE.ItemExpandedBackground = ቖ.GetColor(9542052);
		_0BDE.ItemExpandedBackground2 = ቖ.GetColor(6647418);
		_0BDE.ItemExpandedText = ቖ.GetColor(0);
		_0BDE.ItemHotBackground = ቖ.GetColor(16774604);
		_0BDE.ItemHotBackground2 = ቖ.GetColor(16767861);
		_0BDE.ItemHotBorder = ቖ.GetColor(16760169);
		_0BDE.ItemHotText = ቖ.GetColor(0);
		_0BDE.ItemPressedBackground = ቖ.GetColor(16553789);
		_0BDE.ItemPressedBackground2 = ቖ.GetColor(16758878);
		_0BDE.ItemPressedBorder = ቖ.GetColor(16485436);
		_0BDE.ItemPressedText = ቖ.GetColor(0);
		_0BDE.ItemSeparator = Color.FromArgb(225, ቖ.GetColor(9542052));
		_0BDE.ItemSeparatorShade = Color.FromArgb(180, ቖ.GetColor(16777215));
		_0BDE.ItemText = ቖ.GetColor(0);
		_0BDE.MenuBackground = ቖ.GetColor(16185078);
		_0BDE.MenuBackground2 = Color.Empty;
		_0BDE.MenuBarBackground = ቖ.GetColor(5460819);
		_0BDE.MenuBorder = ቖ.GetColor(9542052);
		_0BDE.ItemExpandedBorder = _0BDE.MenuBorder;
		_0BDE.MenuSide = ቖ.GetColor(15724527);
		_0BDE.MenuSide2 = Color.Empty;
		_0BDE.MenuUnusedBackground = _0BDE.MenuBackground;
		_0BDE.MenuUnusedSide = ቖ.GetColor(15329769);
		_0BDE.MenuUnusedSide2 = Color.Empty;
		_0BDE.ItemDesignTimeBorder = Color.Black;
		_0BDE.BarDockedBorder = ቖ.GetColor(5002076);
		_0BDE.DockSiteBackColor = ቖ.GetColor(5460819);
		_0BDE.DockSiteBackColor2 = Color.Empty;
		_0BDE.CustomizeBackground = ቖ.GetColor(11712447);
		_0BDE.CustomizeBackground2 = ቖ.GetColor(5331042);
		_0BDE.CustomizeText = ቖ.GetColor(0);
		_0BDE.PanelBackground = ቖ.GetColor(15790578);
		_0BDE.PanelBackground2 = ቖ.GetColor(12436169);
		_0BDE.PanelText = Color.Black;
		_0BDE.PanelBorder = ቖ.GetColor(10988982);
		_0BDE.ExplorerBarBackground = ቖ.GetColor(15790578);
		_0BDE.ExplorerBarBackground2 = ቖ.GetColor(12436169);
		_0BDE.MdiSystemItemForeground = Color.LightGray;
		_0BDE.SplitterBackground = _0BDE.PanelBackground;
		_0BDE.SplitterBackground2 = _0BDE.PanelBackground2;
		_0BDE.SplitterText = _0BDE.PanelText;
		_0BDE.SplitterBorder = _0BDE.PanelBorder;
	}

	public static ElementStyle ዸ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.PaddingBottom = 3;
		elementStyle.PaddingLeft = 3;
		elementStyle.PaddingRight = 3;
		elementStyle.PaddingTop = 12;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBackgroundBlend.Count];
		menu.FileBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዹ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuTwoColumnContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
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

	public static ElementStyle ዺ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnOneContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnOneBackground;
		elementStyle.BorderRight = eStyleBorderType.Solid;
		elementStyle.BorderRightColor = menu.FileColumnOneBorder;
		elementStyle.BorderRightWidth = 1;
		elementStyle.PaddingRight = 1;
		return elementStyle;
	}

	public static ElementStyle ዻ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuColumnTwoContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		elementStyle.BackColor = menu.FileColumnTwoBackground;
		return elementStyle;
	}

	public static ElementStyle ዼ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonFileMenuBottomContainerKey;
		Office2007MenuColorTable menu = ቯ.Menu;
		BackgroundColorBlend[] array = new BackgroundColorBlend[menu.FileBottomContainerBackgroundBlend.Count];
		menu.FileBottomContainerBackgroundBlend._0CD0(array);
		elementStyle.BackColorBlend.Clear();
		elementStyle.BackColorBlend.AddRange(array);
		elementStyle.BackColorGradientAngle = 90;
		return elementStyle;
	}

	public static ElementStyle ዽ(Color ዾ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.TextBoxBorderKey;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.PaddingBottom = 2;
		elementStyle.PaddingTop = 2;
		elementStyle.PaddingLeft = 2;
		elementStyle.PaddingRight = 2;
		return elementStyle;
	}

	public static ElementStyle ዿ(Color ዾ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RichTextBoxBorderKey;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		return elementStyle;
	}

	public static ElementStyle ጀ(Color ዾ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.ItemPanelKey;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.PaddingBottom = 1;
		elementStyle.BackColor = ጁ;
		return elementStyle;
	}

	public static ElementStyle ጂ(Color ዾ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.ListBoxAdvKey;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.PaddingBottom = 1;
		elementStyle.BackColor = ጁ;
		return elementStyle;
	}

	public static ElementStyle ጃ(Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.MetroTilePanelKey;
		elementStyle.PaddingLeft = 16;
		elementStyle.PaddingRight = 16;
		elementStyle.PaddingTop = 16;
		elementStyle.PaddingBottom = 16;
		elementStyle.BackColor = ጁ;
		return elementStyle;
	}

	public static ElementStyle ጄ(Color ໆ)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.MetroTileGroupTitleKey;
		elementStyle.TextColor = ໆ;
		elementStyle.Font = new Font("Segoe UI Semibold", 27.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		return elementStyle;
	}

	public static ElementStyle ጀ(Color ዾ)
	{
		return ጀ(ዾ, Color.White);
	}

	public static ElementStyle ጂ(Color ዾ)
	{
		return ጂ(ዾ, Color.White);
	}

	public static ElementStyle ጅ(Color ዾ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.TreeBorderKey;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BackColor = ጁ;
		return elementStyle;
	}

	public static ElementStyle ጆ(Color ጇ, Color ገ, Color ዾ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.TreeColumnsHeaderKey;
		elementStyle.BackColor = ጇ;
		elementStyle.BackColor2 = ገ;
		elementStyle.BackColorGradientAngle = 90;
		elementStyle.TextColor = Color.Black;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		return elementStyle;
	}

	public static ElementStyle ጉ(Color ጇ, Color ገ, Color ዾ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.TreeNodesColumnsHeaderKey;
		elementStyle.BackColor = ጇ;
		elementStyle.BackColor2 = ገ;
		elementStyle.BackColorGradientAngle = 90;
		elementStyle.TextColor = Color.Black;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderBottom = eStyleBorderType.Solid;
		elementStyle.BorderBottomWidth = 1;
		elementStyle.BorderLeft = eStyleBorderType.Solid;
		elementStyle.BorderLeftWidth = 1;
		elementStyle.BorderTop = eStyleBorderType.Solid;
		elementStyle.BorderTopWidth = 1;
		return elementStyle;
	}

	public static ElementStyle ጊ(Color ໆ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.TreeColumnKey;
		elementStyle.TextColor = ໆ;
		return elementStyle;
	}

	public static ElementStyle ጋ(Color ዾ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.DateTimeInputBackgroundKey;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BackColor = ጁ;
		elementStyle.PaddingLeft = 1;
		elementStyle.PaddingRight = 1;
		elementStyle.PaddingTop = 1;
		elementStyle.PaddingBottom = 1;
		return elementStyle;
	}

	public static ElementStyle ጌ(ColorFactory ግ, eOffice2007ColorScheme ጎ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.RibbonClientPanelKey;
		elementStyle.BackColorGradientAngle = 90;
		switch (ጎ)
		{
		case eOffice2007ColorScheme.Blue:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(12506352), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(5668273), 0.8f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(6656461), 1f));
			break;
		case eOffice2007ColorScheme.Silver:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(13422552), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(11251382), 0.8f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(10198950), 1f));
			break;
		case eOffice2007ColorScheme.Black:
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(8289918), 0f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(3552822), 0.8f));
			elementStyle.BackColorBlend.Add(new BackgroundColorBlend(ግ.GetColor(657930), 1f));
			break;
		}
		return elementStyle;
	}

	public static ElementStyle ጏ(Office2007ListViewColorTable ጐ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.ListViewBorderKey;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		elementStyle.CornerType = eCornerType.Square;
		elementStyle.BorderColor = ጐ.Border;
		return elementStyle;
	}

	public static ElementStyle _1311(Office2007BarColorTable ዲ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.Office2007StatusBarBackground2Key;
		elementStyle.PaddingLeft = 4;
		elementStyle.BorderLeft = eStyleBorderType.Etched;
		elementStyle.BorderLeftWidth = 1;
		elementStyle.BorderLeftColor = Color.FromArgb(196, ዲ.StatusBarTopBorder);
		elementStyle.BorderColorLight = Color.FromArgb(128, Color.White);
		if (ዲ.StatusBarAltBackground.Count > 0)
		{
			elementStyle.BackColorBlend.CopyFrom(ዲ.StatusBarAltBackground);
			elementStyle.BackColorGradientAngle = 90;
		}
		return elementStyle;
	}

	public static ElementStyle ጒ(Color ጁ, Color ጓ, Color ጔ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.CrumbBarBackgroundKey;
		elementStyle.BackColor = ጁ;
		elementStyle.BorderColor = ጓ;
		elementStyle.BorderColor2 = ጔ;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderWidth = 1;
		return elementStyle;
	}

	public static ElementStyle ጕ()
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

	public static ElementStyle _1316()
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

	public static ElementStyle _1317()
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

	public static ElementStyle ጘ()
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

	public static ElementStyle ጙ(Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SlideOutButtonKey);
		elementStyle.BackColor = ጁ;
		elementStyle.TextColor = Color.White;
		return elementStyle;
	}

	public static ElementStyle ጚ(Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.MonthCalendarAdvKey);
		elementStyle.BackColor = ጁ;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		elementStyle.BorderWidth = 1;
		return elementStyle;
	}

	public static ElementStyle ጛ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.ProgressStepsKey);
		elementStyle.BackColor = ቯ.Form.BackColor;
		return elementStyle;
	}

	public static ElementStyle ጜ(Office2007ColorTable ቯ)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SideNavStripKey);
		elementStyle.BackColor = ቯ.RibbonControl.TabDividerBorder;
		return elementStyle;
	}

	public static ElementStyle ጜ(Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.SideNavStripKey);
		elementStyle.BackColor = ጁ;
		return elementStyle;
	}

	public static ElementStyle ጝ(Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ElementStyleClassKeys.TabFormPanelKey);
		elementStyle.BackColor = ጁ;
		return elementStyle;
	}

	public static ElementStyle ጞ(Color ዾ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle();
		elementStyle.Class = ElementStyleClassKeys.ToolboxControlKey;
		elementStyle.BorderColor = ዾ;
		elementStyle.BorderWidth = 1;
		elementStyle.Border = eStyleBorderType.Solid;
		elementStyle.BackColor = ጁ;
		return elementStyle;
	}

	public static ElementStyle ጟ(Color ໆ)
	{
		ElementStyle elementStyle = new ElementStyle(ໆ);
		elementStyle.Class = ElementStyleClassKeys.ToolboxGroupTitleKey;
		elementStyle.PaddingBottom = 5;
		elementStyle.PaddingTop = 5;
		elementStyle.PaddingLeft = 5;
		elementStyle.PaddingRight = 5;
		elementStyle.MarginBottom = 5;
		return elementStyle;
	}

	public static ElementStyle ጠ(Color ໆ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ໆ, ጁ);
		elementStyle.Class = ElementStyleClassKeys.ToolboxGroupTitleMouseOverKey;
		elementStyle.PaddingBottom = 5;
		elementStyle.PaddingTop = 5;
		elementStyle.PaddingLeft = 5;
		elementStyle.PaddingRight = 5;
		elementStyle.MarginBottom = 5;
		return elementStyle;
	}

	public static ElementStyle ጡ(Color ໆ, Color ጁ)
	{
		ElementStyle elementStyle = new ElementStyle(ໆ, ጁ);
		elementStyle.Class = ElementStyleClassKeys.ToolboxGroupExpandedTitleKey;
		elementStyle.PaddingBottom = 5;
		elementStyle.PaddingTop = 5;
		elementStyle.PaddingLeft = 5;
		elementStyle.PaddingRight = 5;
		elementStyle.MarginBottom = 5;
		return elementStyle;
	}
}
