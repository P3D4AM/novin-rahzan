using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Viewer.Controls;

[ToolboxItem(false)]
public class StiDrillDownSelectedActionButton : StiActionButton
{
	public StiDrillDownSelectedActionButton(StiControlColorCollection colors, Point? position = null)
		: base(colors, position)
	{
		base.DrawBorder = true;
		((ButtonBase)this).ImageAlign = (ContentAlignment)16;
		((ButtonBase)this).TextAlign = (ContentAlignment)64;
		((Control)this).Text = Loc.GetCleaned("Dashboard", "DrillDownSelected");
		base.ToolTip = Loc.GetCleaned("Dashboard", "DrillDownSelected");
	}
}
