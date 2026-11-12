using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class NavPaneOptions : Form
{
	internal Label labelListCaption;

	private NavigationBarContainer m_NavBar;

	private CheckedListBox lbButtons;

	internal Button cmdMoveUp;

	internal Button cmdMoveDown;

	internal Button cmdReset;

	internal Button cmdOK;

	internal Button cmdCancel;

	private Container components;

	public NavigationBarContainer NavBarContainer
	{
		get
		{
			return m_NavBar;
		}
		set
		{
			m_NavBar = value;
			ContainerChanged();
		}
	}

	public NavPaneOptions()
	{
		InitializeComponent();
		((Control)cmdReset).Visible = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void ContainerChanged()
	{
		((ObjectCollection)lbButtons.Items).Clear();
		if (m_NavBar == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_NavBar.SubItems)
		{
			lbButtons.Items.Add((object)subItem, subItem.Visible);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		labelListCaption = new Label();
		lbButtons = new CheckedListBox();
		cmdMoveUp = new Button();
		cmdMoveDown = new Button();
		cmdReset = new Button();
		cmdOK = new Button();
		cmdCancel = new Button();
		((Control)this).SuspendLayout();
		((Control)labelListCaption).AutoSize = true;
		((Control)labelListCaption).Location = new Point(8, 8);
		((Control)labelListCaption).Name = "labelListCaption";
		((Control)labelListCaption).Size = new Size(144, 13);
		((Control)labelListCaption).TabIndex = 0;
		((Control)labelListCaption).Text = "Display &buttons in this order";
		((Control)lbButtons).Location = new Point(8, 32);
		((Control)lbButtons).Name = "lbButtons";
		((Control)lbButtons).Size = new Size(210, 109);
		((Control)lbButtons).TabIndex = 1;
		((ListBox)lbButtons).SelectedIndexChanged += lbButtons_SelectedIndexChanged;
		((ButtonBase)cmdMoveUp).FlatStyle = (FlatStyle)3;
		((Control)cmdMoveUp).Location = new Point(224, 32);
		((Control)cmdMoveUp).Name = "cmdMoveUp";
		((Control)cmdMoveUp).TabIndex = 2;
		((Control)cmdMoveUp).Text = "Move &Up";
		((Control)cmdMoveUp).Click += cmdMoveUp_Click;
		((ButtonBase)cmdMoveDown).FlatStyle = (FlatStyle)3;
		((Control)cmdMoveDown).Location = new Point(224, 64);
		((Control)cmdMoveDown).Name = "cmdMoveDown";
		((Control)cmdMoveDown).TabIndex = 3;
		((Control)cmdMoveDown).Text = "Move &Down";
		((Control)cmdMoveDown).Click += cmdMoveDown_Click;
		((ButtonBase)cmdReset).FlatStyle = (FlatStyle)3;
		((Control)cmdReset).Location = new Point(224, 104);
		((Control)cmdReset).Name = "cmdReset";
		((Control)cmdReset).TabIndex = 4;
		((Control)cmdReset).Text = "&Reset";
		((Control)cmdReset).Click += cmdReset_Click;
		cmdOK.DialogResult = (DialogResult)1;
		((ButtonBase)cmdOK).FlatStyle = (FlatStyle)3;
		((Control)cmdOK).Location = new Point(144, 152);
		((Control)cmdOK).Name = "cmdOK";
		((Control)cmdOK).TabIndex = 5;
		((Control)cmdOK).Text = "OK";
		((Control)cmdOK).Click += cmdOK_Click;
		cmdCancel.DialogResult = (DialogResult)2;
		((ButtonBase)cmdCancel).FlatStyle = (FlatStyle)3;
		((Control)cmdCancel).Location = new Point(224, 152);
		((Control)cmdCancel).Name = "cmdCancel";
		((Control)cmdCancel).TabIndex = 6;
		((Control)cmdCancel).Text = "Cancel";
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(304, 182);
		((Control)this).Controls.AddRange((Control[])(object)new Control[7]
		{
			(Control)cmdCancel,
			(Control)cmdOK,
			(Control)cmdReset,
			(Control)cmdMoveDown,
			(Control)cmdMoveUp,
			(Control)lbButtons,
			(Control)labelListCaption
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "NavPaneOptions";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "Navigation Pane Options";
		((Control)this).ResumeLayout(false);
	}

	private void lbButtons_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (((ListControl)lbButtons).SelectedIndex == 0)
		{
			((Control)cmdMoveUp).Enabled = false;
		}
		else
		{
			((Control)cmdMoveUp).Enabled = true;
		}
		if (((ListControl)lbButtons).SelectedIndex == ((ObjectCollection)lbButtons.Items).Count - 1)
		{
			((Control)cmdMoveDown).Enabled = false;
		}
		else
		{
			((Control)cmdMoveDown).Enabled = true;
		}
	}

	private void cmdMoveUp_Click(object sender, EventArgs e)
	{
		if (((ListControl)lbButtons).SelectedIndex >= 0)
		{
			BaseItem baseItem = ((ListBox)lbButtons).SelectedItem as BaseItem;
			int selectedIndex = ((ListControl)lbButtons).SelectedIndex;
			bool itemChecked = lbButtons.GetItemChecked(selectedIndex);
			((ObjectCollection)lbButtons.Items).RemoveAt(selectedIndex);
			selectedIndex--;
			((ObjectCollection)lbButtons.Items).Insert(selectedIndex, (object)baseItem);
			lbButtons.SetItemChecked(selectedIndex, itemChecked);
			((ListControl)lbButtons).SelectedIndex = selectedIndex;
		}
	}

	private void cmdMoveDown_Click(object sender, EventArgs e)
	{
		if (((ListControl)lbButtons).SelectedIndex >= 0)
		{
			BaseItem baseItem = ((ListBox)lbButtons).SelectedItem as BaseItem;
			int selectedIndex = ((ListControl)lbButtons).SelectedIndex;
			bool itemChecked = lbButtons.GetItemChecked(selectedIndex);
			((ObjectCollection)lbButtons.Items).RemoveAt(selectedIndex);
			selectedIndex++;
			((ObjectCollection)lbButtons.Items).Insert(selectedIndex, (object)baseItem);
			lbButtons.SetItemChecked(selectedIndex, itemChecked);
			((ListControl)lbButtons).SelectedIndex = selectedIndex;
		}
	}

	private void cmdReset_Click(object sender, EventArgs e)
	{
	}

	private void cmdOK_Click(object sender, EventArgs e)
	{
		m_NavBar.SuspendLayout = true;
		m_NavBar.SubItems._Clear();
		for (int i = 0; i < ((ObjectCollection)lbButtons.Items).Count; i++)
		{
			BaseItem baseItem = ((ObjectCollection)lbButtons.Items)[i] as BaseItem;
			baseItem.Visible = lbButtons.GetItemChecked(i);
			m_NavBar.SubItems._Add(baseItem);
		}
		m_NavBar.SuspendLayout = false;
		if (m_NavBar.ContainerControl is BarBaseControl)
		{
			((BarBaseControl)m_NavBar.ContainerControl).RecalcLayout();
		}
		m_NavBar.OptionsDialogClosed();
	}
}
