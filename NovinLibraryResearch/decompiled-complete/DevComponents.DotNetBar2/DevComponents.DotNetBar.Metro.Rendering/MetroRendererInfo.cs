using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro.ColorTables;

namespace DevComponents.DotNetBar.Metro.Rendering;

public class MetroRendererInfo
{
	public object Control;

	public PaintEventArgs PaintEventArgs;

	public MetroColorTable ColorTable;

	public Font DefaultFont = SystemFonts.DefaultFont;

	public Font DefaultPlainFont;

	public bool RightToLeft;

	public ItemPaintArgs ItemPaintArgs;
}
