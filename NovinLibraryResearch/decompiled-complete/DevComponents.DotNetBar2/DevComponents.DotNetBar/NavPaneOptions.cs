using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class NavPaneOptions : Form
{
	internal Label _217B;

	private NavigationBarContainer _217C;

	private CheckedListBox _217D;

	internal Button _217E;

	internal Button _217F;

	internal Button _2180;

	internal Button _2181;

	internal Button _2182;

	private Container Պ;

	public NavigationBarContainer NavBarContainer
	{
		get
		{
			return _217C;
		}
		set
		{
			_217C = value;
			Ↄ();
		}
	}

	public NavPaneOptions()
	{
		ـ();
		((Control)_2180).Visible = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void Ↄ()
	{
		((ObjectCollection)_217D.Items).Clear();
		if (_217C == null)
		{
			return;
		}
		foreach (BaseItem subItem in _217C.SubItems)
		{
			_217D.Items.Add((object)subItem, subItem.Visible);
		}
	}

	private void ـ()
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
		_217B = new Label();
		_217D = new CheckedListBox();
		_217E = new Button();
		_217F = new Button();
		_2180 = new Button();
		_2181 = new Button();
		_2182 = new Button();
		((Control)this).SuspendLayout();
		((Control)_217B).AutoSize = true;
		((Control)_217B).Location = new Point(8, 8);
		((Control)_217B).Name = "labelListCaption";
		((Control)_217B).Size = new Size(144, 13);
		((Control)_217B).TabIndex = 0;
		((Control)_217B).Text = "Display &buttons in this order";
		((Control)_217D).Location = new Point(8, 32);
		((Control)_217D).Name = "lbButtons";
		((Control)_217D).Size = new Size(210, 109);
		((Control)_217D).TabIndex = 1;
		((ListBox)_217D).SelectedIndexChanged += ↄ;
		((ButtonBase)_217E).FlatStyle = (FlatStyle)3;
		((Control)_217E).Location = new Point(224, 32);
		((Control)_217E).Name = "cmdMoveUp";
		((Control)_217E).TabIndex = 2;
		((Control)_217E).Text = "Move &Up";
		((Control)_217E).Click += _2185;
		((ButtonBase)_217F).FlatStyle = (FlatStyle)3;
		((Control)_217F).Location = new Point(224, 64);
		((Control)_217F).Name = "cmdMoveDown";
		((Control)_217F).TabIndex = 3;
		((Control)_217F).Text = "Move &Down";
		((Control)_217F).Click += _2186;
		((ButtonBase)_2180).FlatStyle = (FlatStyle)3;
		((Control)_2180).Location = new Point(224, 104);
		((Control)_2180).Name = "cmdReset";
		((Control)_2180).TabIndex = 4;
		((Control)_2180).Text = "&Reset";
		((Control)_2180).Click += _2187;
		_2181.DialogResult = (DialogResult)1;
		((ButtonBase)_2181).FlatStyle = (FlatStyle)3;
		((Control)_2181).Location = new Point(144, 152);
		((Control)_2181).Name = "cmdOK";
		((Control)_2181).TabIndex = 5;
		((Control)_2181).Text = "OK";
		((Control)_2181).Click += _2188;
		_2182.DialogResult = (DialogResult)2;
		((ButtonBase)_2182).FlatStyle = (FlatStyle)3;
		((Control)_2182).Location = new Point(224, 152);
		((Control)_2182).Name = "cmdCancel";
		((Control)_2182).TabIndex = 6;
		((Control)_2182).Text = "Cancel";
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(304, 182);
		((Control)this).Controls.AddRange((Control[])(object)new Control[7]
		{
			(Control)_2182,
			(Control)_2181,
			(Control)_2180,
			(Control)_217F,
			(Control)_217E,
			(Control)_217D,
			(Control)_217B
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "NavPaneOptions";
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "Navigation Pane Options";
		((Control)this).ResumeLayout(false);
	}

	private void ↄ(object _٠, EventArgs ؿ)
	{
		if (((ListControl)_217D).SelectedIndex == 0)
		{
			((Control)_217E).Enabled = false;
		}
		else
		{
			((Control)_217E).Enabled = true;
		}
		if (((ListControl)_217D).SelectedIndex == ((ObjectCollection)_217D.Items).Count - 1)
		{
			((Control)_217F).Enabled = false;
		}
		else
		{
			((Control)_217F).Enabled = true;
		}
	}

	private void _2185(object _٠, EventArgs ؿ)
	{
		if (((ListControl)_217D).SelectedIndex >= 0)
		{
			BaseItem baseItem = ((ListBox)_217D).SelectedItem as BaseItem;
			int selectedIndex = ((ListControl)_217D).SelectedIndex;
			bool itemChecked = _217D.GetItemChecked(selectedIndex);
			((ObjectCollection)_217D.Items).RemoveAt(selectedIndex);
			selectedIndex--;
			((ObjectCollection)_217D.Items).Insert(selectedIndex, (object)baseItem);
			_217D.SetItemChecked(selectedIndex, itemChecked);
			((ListControl)_217D).SelectedIndex = selectedIndex;
		}
	}

	private void _2186(object _٠, EventArgs ؿ)
	{
		if (((ListControl)_217D).SelectedIndex >= 0)
		{
			BaseItem baseItem = ((ListBox)_217D).SelectedItem as BaseItem;
			int selectedIndex = ((ListControl)_217D).SelectedIndex;
			bool itemChecked = _217D.GetItemChecked(selectedIndex);
			((ObjectCollection)_217D.Items).RemoveAt(selectedIndex);
			selectedIndex++;
			((ObjectCollection)_217D.Items).Insert(selectedIndex, (object)baseItem);
			_217D.SetItemChecked(selectedIndex, itemChecked);
			((ListControl)_217D).SelectedIndex = selectedIndex;
		}
	}

	private void _2187(object _٠, EventArgs ؿ)
	{
	}

	private void _2188(object _٠, EventArgs ؿ)
	{
		_217C.SuspendLayout = true;
		_217C.SubItems.__0AF5();
		for (int i = 0; i < ((ObjectCollection)_217D.Items).Count; i++)
		{
			BaseItem baseItem = ((ObjectCollection)_217D.Items)[i] as BaseItem;
			baseItem.Visible = _217D.GetItemChecked(i);
			_217C.SubItems.__0AF3(baseItem);
		}
		_217C.SuspendLayout = false;
		if (_217C.ContainerControl is BarBaseControl)
		{
			((BarBaseControl)_217C.ContainerControl).RecalcLayout();
		}
		_217C.ᯞ();
	}
}
