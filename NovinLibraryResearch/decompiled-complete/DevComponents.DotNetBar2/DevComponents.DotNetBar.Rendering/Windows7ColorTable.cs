using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class Windows7ColorTable : Office2007ColorTable
{
	public Windows7ColorTable()
	{
		Windows7BlueFactory.InitializeBlueColorTable(this, ColorFactory.Empty);
	}

	public Windows7ColorTable(eWindows7ColorScheme color)
	{
		Windows7BlueFactory.InitializeBlueColorTable(this, ColorFactory.Empty);
	}

	public Windows7ColorTable(eWindows7ColorScheme color, Color blendColor)
	{
		Windows7BlueFactory.InitializeBlueColorTable(this, blendColor.IsEmpty ? ColorFactory.Empty : new ColorBlendFactory(blendColor));
	}
}
