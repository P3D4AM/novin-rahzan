using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiToolboxControl : Panel
{
	public StiToolboxControl()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)512, true);
	}
}
