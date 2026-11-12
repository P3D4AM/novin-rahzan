using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls.Menu;

[ToolboxItem(false)]
public sealed class StiCheckedListContextMenuItem : ToolStripControlHost
{
	public ObjectCollection Items => ((CheckedListBox)ListBox).Items;

	public StiMenuCheckedListBox ListBox => ((ToolStripControlHost)this).Control as StiMenuCheckedListBox;

	private static Control CreateControlInstance()
	{
		StiMenuCheckedListBox stiMenuCheckedListBox = new StiMenuCheckedListBox();
		((Control)stiMenuCheckedListBox).Left = 20;
		stiMenuCheckedListBox.BorderStyle = (BorderStyle)0;
		((CheckedListBox)stiMenuCheckedListBox).CheckOnClick = true;
		return (Control)(object)stiMenuCheckedListBox;
	}

	public void CalculateSize()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		int height = ((ListBox)ListBox).ItemHeight * Math.Min(((ObjectCollection)((CheckedListBox)ListBox).Items).Count, 15);
		int num = StiScaleUI.XXI(200.0);
		if (((ToolStripItem)this).Parent != null)
		{
			int num2 = num;
			Padding val = ((ToolStripItem)this).Margin;
			int num3 = num2 + ((Padding)(ref val)).Left;
			val = ((ToolStripItem)this).Margin;
			int num4 = num3 + ((Padding)(ref val)).Right;
			int width = ((Control)((ToolStripItem)this).Parent).Width;
			val = ((Control)((ToolStripItem)this).Parent).Padding;
			int num5 = width - ((Padding)(ref val)).Left;
			val = ((Control)((ToolStripItem)this).Parent).Padding;
			if (num4 < num5 - ((Padding)(ref val)).Right)
			{
				int width2 = ((Control)((ToolStripItem)this).Parent).Width;
				val = ((Control)((ToolStripItem)this).Parent).Padding;
				int num6 = width2 - ((Padding)(ref val)).Left;
				val = ((Control)((ToolStripItem)this).Parent).Padding;
				num = num6 - ((Padding)(ref val)).Right;
			}
		}
		StiMenuCheckedListBox listBox = ListBox;
		Size minimumSize = (((Control)ListBox).MaximumSize = new Size(num, height));
		((Control)listBox).MinimumSize = minimumSize;
	}

	private void ListBox_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		bool flag = (int)e.NewValue == 1;
		ToolStripMenuItem val = (ToolStripMenuItem)((ObjectCollection)Items)[e.Index];
		if (val.Checked != flag)
		{
			((ToolStripItem)val).PerformClick();
		}
		ToolStrip parent = ((ToolStripItem)this).Parent;
		if (parent != null)
		{
			((Control)parent).Focus();
		}
	}

	public StiCheckedListContextMenuItem()
		: base(CreateControlInstance())
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		((ToolStripItem)this).Margin = new Padding(StiScaleUI.XXI(16.0), StiScaleUI.I4, 0, StiScaleUI.I4);
		((ToolStripItem)this).AutoSize = false;
		((CheckedListBox)ListBox).ItemCheck += new ItemCheckEventHandler(ListBox_ItemCheck);
	}
}
