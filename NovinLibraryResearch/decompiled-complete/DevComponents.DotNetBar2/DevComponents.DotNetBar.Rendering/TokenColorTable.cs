using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class TokenColorTable
{
	public Color TextColor = Color.Black;

	public LinearGradientColorTable Background = new LinearGradientColorTable(15068920);

	public TokenColorTable()
	{
	}

	public TokenColorTable(Color textColor, Color background)
	{
		TextColor = textColor;
		Background = new LinearGradientColorTable(background);
	}

	public TokenColorTable(int textColor, int background)
	{
		TextColor = ColorScheme.GetColor(textColor);
		Background = new LinearGradientColorTable(background);
	}
}
