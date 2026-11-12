namespace Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

public class MetroFormColorTable
{
	public Thickness BorderThickness = new Thickness(0.0, 0.0, 1.0, 1.0);

	public Thickness BorderPlainThickness = new Thickness(1.0);

	public BorderColors[] BorderColors = new BorderColors[1]
	{
		new BorderColors(ColorScheme.GetColor("FF6B10"))
	};

	public BorderColors[] BorderColorsInactive;
}
