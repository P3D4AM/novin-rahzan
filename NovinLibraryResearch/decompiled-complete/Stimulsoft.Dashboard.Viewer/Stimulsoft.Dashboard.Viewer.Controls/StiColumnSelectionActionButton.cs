using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Viewer.Images;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiColumnSelectionActionButton : StiActionButton
{
	public StiColumnSelectionActionButton(StiControlColorCollection colors, Point? position = null)
		: base(colors, position)
	{
		((ButtonBase)this).Image = StiDashboardViewerImages.Actions.ColumnSelection(colors.IsDarkMode);
		base.ToolTip = Loc.Get("Wizards", "SelectColumns");
	}
}
