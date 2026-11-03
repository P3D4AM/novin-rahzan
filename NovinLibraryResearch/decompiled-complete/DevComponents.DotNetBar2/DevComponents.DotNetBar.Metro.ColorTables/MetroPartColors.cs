using System.Drawing;

namespace DevComponents.DotNetBar.Metro.ColorTables;

public class MetroPartColors
{
	public Color CanvasColor;

	public Color BaseColor;

	public Color BaseTextColor;

	public Color TextColor;

	public Color TextInactiveColor;

	public Color TextDisabledColor;

	public Color TextLightColor;

	public Color CanvasColorLight;

	public Color CanvasColorDarkShade;

	public Color CanvasColorDarkerShade;

	public Color CanvasColorLightShade;

	public Color CanvasColorLighterShade;

	public Color BaseColorLight;

	public Color BaseColorLight1;

	public Color BaseColorLightText;

	public Color BaseColorLighter;

	public Color BaseColorLightest;

	public Color BaseColorDark;

	public Color BaseColorDarker;

	public Color ComplementColor;

	public Color ComplementColorLight;

	public Color ComplementColorLightText;

	public Color ComplementColorDark;

	public Color ComplementColorDarker;

	public Color ComplementColorText;

	public Color BaseButtonGradientStart;

	public Color BaseButtonGradientEnd;

	public Color EditControlBackColor;

	internal bool ፚ
	{
		get
		{
			if (!(CanvasColor == Color.White))
			{
				return CanvasColor == Color.Black;
			}
			return true;
		}
	}
}
