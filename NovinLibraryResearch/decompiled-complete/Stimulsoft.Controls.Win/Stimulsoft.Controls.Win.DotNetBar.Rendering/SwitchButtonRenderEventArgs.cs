using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class SwitchButtonRenderEventArgs
{
	public Graphics Graphics;

	public SwitchButtonItem SwitchButtonItem;

	public ColorScheme ColorScheme;

	public bool RightToLeft;

	public Font Font;

	internal ItemPaintArgs ItemPaintArgs;

	public SwitchButtonRenderEventArgs(Graphics g, SwitchButtonItem item, ColorScheme cs, Font f, bool rtl)
	{
		Graphics = g;
		SwitchButtonItem = item;
		ColorScheme = cs;
		RightToLeft = rtl;
		Font = f;
	}
}
