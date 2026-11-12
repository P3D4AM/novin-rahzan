using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public struct LabelColors
{
	public Color Text;

	public Color DisabledText;

	public LabelColors(Color text, Color disabledText)
	{
		Text = text;
		DisabledText = disabledText;
	}
}
