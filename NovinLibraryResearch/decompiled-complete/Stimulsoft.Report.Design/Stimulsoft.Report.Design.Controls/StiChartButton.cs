using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiChartButton : ButtonX
{
	public StiChartButton()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).TabStop = false;
	}
}
