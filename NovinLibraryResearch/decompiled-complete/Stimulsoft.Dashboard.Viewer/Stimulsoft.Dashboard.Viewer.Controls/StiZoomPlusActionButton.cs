using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Images;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiZoomPlusActionButton : StiActionButton
{
	public StiZoomPlusActionButton(StiControlColorCollection colors, Point? position = null)
		: base(colors, position)
	{
		((ButtonBase)this).Image = StiDashboardViewerImages.Actions.ZoomPlus(colors.IsDarkMode);
	}
}
