using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.DataTransformation;

[ToolboxItem(false)]
public sealed class StiDataTransformCheckedListBox : ControlContainerItem
{
	private CheckedListBox listBox = new CheckedListBox
	{
		Height = 200,
		CheckOnClick = true
	};

	private void OnItemCheckedChanged(object sender, EventArgs e)
	{
		listBox.SetItemChecked(((ObjectCollection)listBox.Items).IndexOf(sender), ((ButtonItem)sender).Checked);
	}

	private void OnListBoxItemCheck(object sender, ItemCheckEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		bool flag = (int)e.NewValue == 1;
		ButtonItem buttonItem = (ButtonItem)((ObjectCollection)listBox.Items)[e.Index];
		if (buttonItem.Checked != flag)
		{
			buttonItem.RaiseClick();
		}
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		((ButtonItem)item).CheckedChanged += OnItemCheckedChanged;
		listBox.Items.Add((object)item, ((ButtonItem)item).Checked);
		base.OnItemAdded(item);
	}

	protected internal override void OnAfterItemRemoved(BaseItem item)
	{
		((ObjectCollection)listBox.Items).Remove((object)item);
		base.OnAfterItemRemoved(item);
	}

	public StiDataTransformCheckedListBox()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		listBox.ItemCheck += new ItemCheckEventHandler(OnListBoxItemCheck);
		base.Control = (Control)(object)listBox;
	}
}
