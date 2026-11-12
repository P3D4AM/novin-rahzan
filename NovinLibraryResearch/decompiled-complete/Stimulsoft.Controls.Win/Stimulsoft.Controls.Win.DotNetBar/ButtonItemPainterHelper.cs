using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ButtonItemPainterHelper
{
	public static Color GetTextColor(ButtonItem button, ItemPaintArgs pa)
	{
		Color controlText = SystemColors.ControlText;
		if (!ButtonItemPainter.IsItemEnabled(button, pa))
		{
			if (!pa.Colors.ItemDisabledText.IsEmpty)
			{
				return pa.Colors.ItemDisabledText;
			}
			return SystemColors.ControlDark;
		}
		if (button.IsMouseDown && !pa.Colors.ItemPressedText.IsEmpty)
		{
			return pa.Colors.ItemPressedText;
		}
		if (button.IsMouseOver)
		{
			if (!button.HotForeColor.IsEmpty)
			{
				return button.HotForeColor;
			}
			return pa.Colors.ItemHotText;
		}
		if (button.Expanded && !pa.Colors.ItemExpandedText.IsEmpty)
		{
			return pa.Colors.ItemExpandedText;
		}
		if (button.Checked && !pa.Colors.ItemCheckedText.IsEmpty)
		{
			return pa.Colors.ItemCheckedText;
		}
		if (!button.ForeColor.IsEmpty)
		{
			return button.ForeColor;
		}
		if (button.IsThemed && button.IsOnMenuBar && pa.Colors.ItemText == SystemColors.ControlText)
		{
			return SystemColors.MenuText;
		}
		return pa.Colors.ItemText;
	}
}
