using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Win.Parameters.Controls;

[ToolboxItem(false)]
public class StiParameterListBox : ListBox
{
	public StiParameterListBox()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).BackColor = Color.White;
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((Control)this).Dock = (DockStyle)5;
		((ListBox)this).IntegralHeight = false;
		((ListBox)this).DrawMode = (DrawMode)1;
		((ListBox)this).ItemHeight = StiScale.I(21);
	}
}
