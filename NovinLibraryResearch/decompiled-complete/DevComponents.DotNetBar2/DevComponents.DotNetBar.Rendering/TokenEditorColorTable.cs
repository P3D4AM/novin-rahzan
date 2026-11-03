using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class TokenEditorColorTable
{
	public TokenColorTable Normal = new TokenColorTable(0, 14412284);

	public TokenColorTable MouseOver = new TokenColorTable(0, 12573695);

	public TokenColorTable Focused = new TokenColorTable(16777215, 29382);

	public TokenEditorColorTable()
	{
	}

	public TokenEditorColorTable(int normalTextColor, int normalBackground, int mouseOverTextColor, int mouseOverBackground, int focusedTextColor, int focusedBackground)
	{
		Normal = new TokenColorTable(normalTextColor, normalBackground);
		MouseOver = new TokenColorTable(mouseOverTextColor, mouseOverBackground);
		Focused = new TokenColorTable(focusedTextColor, focusedBackground);
	}

	public TokenEditorColorTable(Color normalTextColor, Color normalBackground, Color mouseOverTextColor, Color mouseOverBackground, Color focusedTextColor, Color focusedBackground)
	{
		Normal = new TokenColorTable(normalTextColor, normalBackground);
		MouseOver = new TokenColorTable(mouseOverTextColor, mouseOverBackground);
		Focused = new TokenColorTable(focusedTextColor, focusedBackground);
	}
}
