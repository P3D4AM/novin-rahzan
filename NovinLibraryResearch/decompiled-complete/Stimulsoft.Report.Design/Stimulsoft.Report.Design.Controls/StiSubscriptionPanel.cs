using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiSubscriptionPanel : Panel
{
	public StiSubscriptionPanel()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
	}
}
