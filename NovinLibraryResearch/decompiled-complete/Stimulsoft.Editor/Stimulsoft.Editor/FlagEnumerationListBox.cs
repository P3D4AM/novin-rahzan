using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
internal class FlagEnumerationListBox : CheckedListBox
{
	private int updateCount;

	public FlagEnumerationListBox()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
	}

	public void ListBoxBeginUpdate()
	{
		((ListBox)this).BeginUpdate();
		updateCount++;
	}

	public void ListBoxEndUpdate()
	{
		((ListBox)this).EndUpdate();
		updateCount--;
		if (updateCount == 0)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (updateCount == 0)
		{
			((Control)this).OnPaint(e);
		}
	}
}
