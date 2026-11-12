using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroToolbarPainter : MetroRenderer
{
	public override void Render(MetroRendererInfo renderingInfo)
	{
		MetroToolbar metroToolbar = (MetroToolbar)renderingInfo.Control;
		Graphics graphics = renderingInfo.PaintEventArgs.Graphics;
		MetroToolbarColorTable metroToolbar2 = renderingInfo.ColorTable.MetroToolbar;
		Rectangle clientRectangle = ((Control)metroToolbar).ClientRectangle;
		if (metroToolbar2.BackgroundStyle != null)
		{
			ElementStyleDisplayInfo e = new ElementStyleDisplayInfo(metroToolbar2.BackgroundStyle, graphics, clientRectangle);
			ElementStyleDisplay.PaintBackground(e);
		}
	}
}
