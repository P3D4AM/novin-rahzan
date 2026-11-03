using System.Drawing;

namespace DevComponents.DotNetBar;

internal class ḛ
{
	public Color ᙴ = Color.Empty;

	public Color ཥ = Color.Empty;

	public Color Ḝ = Color.Empty;

	public Color ḝ = Color.Empty;

	public Color _0FAE = Color.Empty;

	public Color Ḟ = Color.Empty;

	public Color ḟ = Color.Empty;

	public Color Ḡ = Color.Empty;

	public Color ḡ = Color.Empty;

	public Color Ḣ = Color.Empty;

	public Color ḣ = Color.Empty;

	public Color Ḥ = Color.Empty;

	public Color ḥ = Color.Empty;

	public Color Ḧ = Color.Empty;

	public Color ḧ = Color.Empty;

	public Color Ḩ = Color.Empty;

	public int ḩ = 90;

	public Color Ḫ = Color.Empty;

	public Color ḫ = Color.Empty;

	public Color Ḭ = Color.Empty;

	public Color ḭ = Color.Empty;

	public Color Ḯ = Color.Empty;

	public Color ḯ = Color.Empty;

	public int Ḱ = 90;

	public Color ḱ = Color.Empty;

	public Color Ḳ = Color.Empty;

	public Color ḳ = Color.Empty;

	public Color Ḵ = Color.Empty;

	public int ḵ;

	public Color Ḷ = Color.Empty;

	public Color ḷ = Color.Empty;

	public Color Ḹ = Color.Empty;

	public int ḹ;

	public Color Ḻ = Color.Empty;

	public Color ḻ = Color.Empty;

	public int Ḽ = 90;

	public Color ḽ = Color.Empty;

	public Color Ḿ = Color.Empty;

	public void ᙘ(SideBarPanelItem _06EC)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		_06EC.ᥘ(new ItemStyle());
		_06EC.BackgroundStyle.BackColor1.Color = ཥ;
		_06EC.BackgroundStyle.BackColor2.Color = Ḝ;
		_06EC.BackgroundStyle.BorderColor.Color = ᙴ;
		_06EC.BackgroundStyle.Border = eBorderType.SingleLine;
		_06EC.BackgroundStyle.BorderSide = eBorderSide.All;
		_06EC.BackgroundStyle.ForeColor.Color = ḝ;
		_06EC.ᥚ(new ItemStyle());
		_06EC.HeaderStyle.BackColor1.Color = _0FAE;
		_06EC.HeaderStyle.BackColor2.Color = Ḟ;
		_06EC.HeaderStyle.GradientAngle = 90;
		_06EC.HeaderStyle.BorderSide = eBorderSide.Right | eBorderSide.Top | eBorderSide.Bottom;
		_06EC.HeaderStyle.Border = eBorderType.SingleLine;
		_06EC.HeaderStyle.BorderColor.Color = ᙴ;
		_06EC.HeaderStyle.ForeColor.Color = Ḧ;
		_06EC.HeaderStyle.Font = new Font("Arial", 9f, (FontStyle)1);
		if (!ḡ.IsEmpty)
		{
			_06EC.ᥛ(new ItemStyle());
			_06EC.HeaderHotStyle.BackColor1.Color = ḡ;
			_06EC.HeaderHotStyle.BackColor2.Color = Ḣ;
			_06EC.HeaderHotStyle.GradientAngle = 90;
			if (!ḣ.IsEmpty)
			{
				_06EC.HeaderHotStyle.ForeColor.Color = ḣ;
			}
		}
		else
		{
			_06EC.ᥛ(null);
		}
		_06EC.ᥜ(null);
		_06EC.ᥝ(new ItemStyle());
		_06EC.HeaderSideStyle.Border = eBorderType.SingleLine;
		_06EC.HeaderSideStyle.BorderColor.Color = ᙴ;
		_06EC.HeaderSideStyle.BorderSide = eBorderSide.Left | eBorderSide.Top | eBorderSide.Bottom;
		_06EC.HeaderSideStyle.BackColor1.Color = ḟ;
		_06EC.HeaderSideStyle.BackColor2.Color = Ḡ;
		_06EC.HeaderSideStyle.GradientAngle = 90;
		if (!Ḥ.IsEmpty)
		{
			_06EC.ᥞ(new ItemStyle());
			_06EC.HeaderSideHotStyle.BackColor1.Color = Ḥ;
			_06EC.HeaderSideHotStyle.BackColor2.Color = ḥ;
			_06EC.HeaderSideHotStyle.GradientAngle = 90;
		}
		else
		{
			_06EC.ᥞ(null);
		}
		_06EC.ᥟ(null);
	}

	public void ᙘ(ColorScheme _1DF0)
	{
		_1DF0.ItemBackground = Ḭ;
		_1DF0.ItemCheckedBackground = Ḻ;
		_1DF0.ItemCheckedBackground2 = ḻ;
		_1DF0.ItemCheckedBackgroundGradientAngle = Ḽ;
		_1DF0.ItemCheckedBorder = ḽ;
		_1DF0.ItemCheckedText = Ḿ;
		_1DF0.ItemHotBackground = ḧ;
		_1DF0.ItemHotBackground2 = Ḩ;
		_1DF0.ItemHotBackgroundGradientAngle = ḩ;
		_1DF0.ItemHotBorder = Ḫ;
		_1DF0.ItemHotText = ḫ;
		_1DF0.ItemPressedBackground = Ḯ;
		_1DF0.ItemPressedBackground2 = ḯ;
		_1DF0.ItemPressedBackgroundGradientAngle = Ḱ;
		_1DF0.ItemPressedBorder = ḱ;
		_1DF0.ItemPressedText = Ḳ;
		_1DF0.ItemText = ḭ;
		_1DF0.ItemExpandedBackground = Ḯ;
		_1DF0.ItemExpandedBackground2 = ḯ;
		_1DF0.ItemExpandedBackgroundGradientAngle = Ḱ;
		_1DF0.ItemExpandedShadow = Color.Empty;
		_1DF0.ItemExpandedText = Ḳ;
		_1DF0.MenuBackground = ḳ;
		_1DF0.MenuBackground2 = Ḵ;
		_1DF0.MenuBackgroundGradientAngle = ḵ;
		_1DF0.MenuBorder = Ḷ;
		_1DF0.MenuSide = ḷ;
		_1DF0.MenuSide2 = Ḹ;
		_1DF0.MenuSideGradientAngle = ḹ;
	}
}
