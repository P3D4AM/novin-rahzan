using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Dashboard.Viewer;

[ToolboxItem(false)]
public class StiDashboardTabsPanel : Panel
{
	public StiDashboardTabsPanel()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).Margin = new Padding(0);
		((Control)this).Padding = new Padding(StiScale.I4);
		((Control)this).Anchor = (AnchorStyles)15;
	}
}
