using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Viewer.Images;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiEditDashboardActionButton : StiActionButton
{
	public StiEditDashboardActionButton(StiControlColorCollection colors, Point? position = null)
		: base(colors, position)
	{
		((ButtonBase)this).Image = StiDashboardViewerImages.Actions.Edit(colors.IsDarkMode);
		base.ToolTip = Loc.Get("QueryBuilder", "Edit");
	}
}
