using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SideBarColors
{
	public Color Border = Color.Empty;

	public Color Background = Color.Empty;

	public Color Background2 = Color.Empty;

	public Color BackgroundText = Color.Empty;

	public Color HeaderBackground = Color.Empty;

	public Color HeaderBackground2 = Color.Empty;

	public Color HeaderSideBackground = Color.Empty;

	public Color HeaderSideBackground2 = Color.Empty;

	public Color HeaderHotBackground = Color.Empty;

	public Color HeaderHotBackground2 = Color.Empty;

	public Color HeaderHotText = Color.Empty;

	public Color HeaderSideHotBackground = Color.Empty;

	public Color HeaderSideHotBackground2 = Color.Empty;

	public Color HeaderText = Color.Empty;

	public Color ItemHotBackground = Color.Empty;

	public Color ItemHotBackground2 = Color.Empty;

	public int ItemHotBackgroundGradientAngle = 90;

	public Color ItemHotBorder = Color.Empty;

	public Color ItemHotText = Color.Empty;

	public Color ItemBackground = Color.Empty;

	public Color ItemText = Color.Empty;

	public Color ItemPressedBackground = Color.Empty;

	public Color ItemPressedBackground2 = Color.Empty;

	public int ItemPressedBackgroundGradientAngle = 90;

	public Color ItemPressedBorder = Color.Empty;

	public Color ItemPressedText = Color.Empty;

	public Color MenuBackground = Color.Empty;

	public Color MenuBackground2 = Color.Empty;

	public int MenuBackgroundGradientAngle;

	public Color MenuBorder = Color.Empty;

	public Color MenuSide = Color.Empty;

	public Color MenuSide2 = Color.Empty;

	public int MenuSideGradientAngle;

	public Color ItemCheckedBackground = Color.Empty;

	public Color ItemCheckedBackground2 = Color.Empty;

	public int ItemCheckedBackgroundGradientAngle = 90;

	public Color ItemCheckedBorder = Color.Empty;

	public Color ItemCheckedText = Color.Empty;

	public void Apply(SideBarPanelItem item)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		item.SetBackgroundStyle(new ItemStyle());
		item.BackgroundStyle.BackColor1.Color = Background;
		item.BackgroundStyle.BackColor2.Color = Background2;
		item.BackgroundStyle.BorderColor.Color = Border;
		item.BackgroundStyle.Border = eBorderType.SingleLine;
		item.BackgroundStyle.BorderSide = eBorderSide.All;
		item.BackgroundStyle.ForeColor.Color = BackgroundText;
		item.SetHeaderStyle(new ItemStyle());
		item.HeaderStyle.BackColor1.Color = HeaderBackground;
		item.HeaderStyle.BackColor2.Color = HeaderBackground2;
		item.HeaderStyle.GradientAngle = 90;
		item.HeaderStyle.BorderSide = eBorderSide.Right | eBorderSide.Top | eBorderSide.Bottom;
		item.HeaderStyle.Border = eBorderType.SingleLine;
		item.HeaderStyle.BorderColor.Color = Border;
		item.HeaderStyle.ForeColor.Color = HeaderText;
		item.HeaderStyle.Font = new Font("Arial", 9f, (FontStyle)1);
		if (!HeaderHotBackground.IsEmpty)
		{
			item.SetHeaderHotStyle(new ItemStyle());
			item.HeaderHotStyle.BackColor1.Color = HeaderHotBackground;
			item.HeaderHotStyle.BackColor2.Color = HeaderHotBackground2;
			item.HeaderHotStyle.GradientAngle = 90;
			if (!HeaderHotText.IsEmpty)
			{
				item.HeaderHotStyle.ForeColor.Color = HeaderHotText;
			}
		}
		else
		{
			item.SetHeaderHotStyle(null);
		}
		item.SetHeaderMouseDownStyle(null);
		item.SetHeaderSideStyle(new ItemStyle());
		item.HeaderSideStyle.Border = eBorderType.SingleLine;
		item.HeaderSideStyle.BorderColor.Color = Border;
		item.HeaderSideStyle.BorderSide = eBorderSide.Left | eBorderSide.Top | eBorderSide.Bottom;
		item.HeaderSideStyle.BackColor1.Color = HeaderSideBackground;
		item.HeaderSideStyle.BackColor2.Color = HeaderSideBackground2;
		item.HeaderSideStyle.GradientAngle = 90;
		if (!HeaderSideHotBackground.IsEmpty)
		{
			item.SetHeaderSideHotStyle(new ItemStyle());
			item.HeaderSideHotStyle.BackColor1.Color = HeaderSideHotBackground;
			item.HeaderSideHotStyle.BackColor2.Color = HeaderSideHotBackground2;
			item.HeaderSideHotStyle.GradientAngle = 90;
		}
		else
		{
			item.SetHeaderSideHotStyle(null);
		}
		item.SetHeaderSideMouseDownStyle(null);
	}

	public void Apply(ColorScheme scheme)
	{
		scheme.ItemBackground = ItemBackground;
		scheme.ItemCheckedBackground = ItemCheckedBackground;
		scheme.ItemCheckedBackground2 = ItemCheckedBackground2;
		scheme.ItemCheckedBackgroundGradientAngle = ItemCheckedBackgroundGradientAngle;
		scheme.ItemCheckedBorder = ItemCheckedBorder;
		scheme.ItemCheckedText = ItemCheckedText;
		scheme.ItemHotBackground = ItemHotBackground;
		scheme.ItemHotBackground2 = ItemHotBackground2;
		scheme.ItemHotBackgroundGradientAngle = ItemHotBackgroundGradientAngle;
		scheme.ItemHotBorder = ItemHotBorder;
		scheme.ItemHotText = ItemHotText;
		scheme.ItemPressedBackground = ItemPressedBackground;
		scheme.ItemPressedBackground2 = ItemPressedBackground2;
		scheme.ItemPressedBackgroundGradientAngle = ItemPressedBackgroundGradientAngle;
		scheme.ItemPressedBorder = ItemPressedBorder;
		scheme.ItemPressedText = ItemPressedText;
		scheme.ItemText = ItemText;
		scheme.ItemExpandedBackground = ItemPressedBackground;
		scheme.ItemExpandedBackground2 = ItemPressedBackground2;
		scheme.ItemExpandedBackgroundGradientAngle = ItemPressedBackgroundGradientAngle;
		scheme.ItemExpandedShadow = Color.Empty;
		scheme.ItemExpandedText = ItemPressedText;
		scheme.MenuBackground = MenuBackground;
		scheme.MenuBackground2 = MenuBackground2;
		scheme.MenuBackgroundGradientAngle = MenuBackgroundGradientAngle;
		scheme.MenuBorder = MenuBorder;
		scheme.MenuSide = MenuSide;
		scheme.MenuSide2 = MenuSide2;
		scheme.MenuSideGradientAngle = MenuSideGradientAngle;
	}
}
