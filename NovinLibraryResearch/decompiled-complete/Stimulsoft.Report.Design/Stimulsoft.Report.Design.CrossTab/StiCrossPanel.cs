using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.CrossTab;

[ToolboxItem(false)]
internal class StiCrossPanel : Panel
{
	public StiCrossPanel()
	{
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
	}
}
