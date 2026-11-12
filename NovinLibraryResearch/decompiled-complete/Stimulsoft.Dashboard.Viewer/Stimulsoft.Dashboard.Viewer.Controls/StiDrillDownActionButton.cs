using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Viewer.Images;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiDrillDownActionButton : StiActionButton
{
	public StiDrillDownActionButton(StiControlColorCollection colors, Point? position = null)
		: base(colors, position)
	{
		((ButtonBase)this).Image = StiDashboardViewerImages.Actions.DrillDown(colors.IsDarkMode);
		base.ToolTip = Loc.GetCleaned("Dashboard", "DrillDown");
	}
}
