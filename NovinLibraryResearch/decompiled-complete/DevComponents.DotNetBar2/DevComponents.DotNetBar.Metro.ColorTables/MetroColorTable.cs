using System.Drawing;

namespace DevComponents.DotNetBar.Metro.ColorTables;

public class MetroColorTable
{
	public Color CanvasColor = Color.White;

	public Color CanvasColorShadeLight = Color.White;

	public Color CanvasColorShadeLighter = Color.White;

	public Color ForeColor = Color.Black;

	public Color BaseColor = Color.Black;

	public Color EditControlBackColor;

	public MetroAppFormColorTable MetroAppForm = new MetroAppFormColorTable();

	public MetroFormColorTable MetroForm = new MetroFormColorTable();

	public MetroTabColorTable MetroTab = new MetroTabColorTable();

	public MetroStatusBarColorTable MetroStatusBar = new MetroStatusBarColorTable();

	public MetroToolbarColorTable MetroToolbar = new MetroToolbarColorTable();

	public MetroTileColorTable MetroTile = new MetroTileColorTable();

	public MetroPartColors MetroPartColors = new MetroPartColors();
}
