using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class GradientColorTable
{
	public BackgroundColorBlendCollection Colors = new BackgroundColorBlendCollection();

	public eGradientType GradientType;

	public int LinearGradientAngle = 90;

	public bool IsEmpty => Colors.Count == 0;

	public GradientColorTable()
	{
	}

	public GradientColorTable(Color color)
	{
		BackgroundColorBlendCollection.InitializeCollection(Colors, color);
	}

	public GradientColorTable(int color1, int color2)
		: this(color1, color2, 90)
	{
	}

	public GradientColorTable(Color color1, Color color2)
		: this(color1, color2, 90)
	{
	}

	public GradientColorTable(int color1, int color2, int linearGradientAngle)
	{
		BackgroundColorBlendCollection.InitializeCollection(Colors, color1, color2);
		LinearGradientAngle = linearGradientAngle;
	}

	public GradientColorTable(Color color1, Color color2, int linearGradientAngle)
	{
		BackgroundColorBlendCollection.InitializeCollection(Colors, color1, color2);
		LinearGradientAngle = linearGradientAngle;
	}
}
