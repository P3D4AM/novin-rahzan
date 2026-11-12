using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

public class MetroRendererInfo
{
	public object Control;

	public PaintEventArgs PaintEventArgs;

	public MetroColorTable ColorTable;

	public Font DefaultFont = SystemFonts.DefaultFont;

	public bool RightToLeft;

	public ItemPaintArgs ItemPaintArgs;
}
