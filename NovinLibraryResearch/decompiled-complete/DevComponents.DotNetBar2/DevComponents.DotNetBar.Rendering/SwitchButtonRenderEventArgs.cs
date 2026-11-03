using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class SwitchButtonRenderEventArgs
{
	public Graphics Graphics;

	public SwitchButtonItem SwitchButtonItem;

	public ColorScheme ColorScheme;

	public bool RightToLeft;

	public Font Font;

	internal ItemPaintArgs က;

	public SwitchButtonRenderEventArgs(Graphics g, SwitchButtonItem item, ColorScheme cs, Font f, bool rtl)
	{
		Graphics = g;
		SwitchButtonItem = item;
		ColorScheme = cs;
		RightToLeft = rtl;
		Font = f;
	}
}
