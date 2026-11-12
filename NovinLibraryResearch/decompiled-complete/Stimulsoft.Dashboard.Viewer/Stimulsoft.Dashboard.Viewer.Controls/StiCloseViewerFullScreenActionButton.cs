using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Viewer.Images;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiCloseViewerFullScreenActionButton : StiActionButton
{
	public StiCloseViewerFullScreenActionButton(StiControlColorCollection colors, Point? position = null)
		: base(colors, position)
	{
		((ButtonBase)this).ImageAlign = (ContentAlignment)16;
		((ButtonBase)this).TextAlign = (ContentAlignment)64;
		((Control)this).Text = Loc.Get("Buttons", "Close");
		base.ToolTip = Loc.Get("FormViewer", "Close");
		((ButtonBase)this).Image = StiDashboardViewerImages.Actions.CloseFullScreen(colors.IsDarkMode);
	}
}
