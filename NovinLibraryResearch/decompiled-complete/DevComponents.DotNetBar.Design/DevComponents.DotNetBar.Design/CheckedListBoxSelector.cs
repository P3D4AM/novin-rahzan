using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design;

public class CheckedListBoxSelector : UserControl
{
	private IContainer components;

	private CheckedListBox checkedListBox1;

	private Panel panel1;

	private ButtonX buttonMoveDown;

	private ButtonX buttonMoveUp;

	public CheckedListBox ListBox => checkedListBox1;

	public CheckedListBoxSelector()
	{
		InitializeComponent();
	}

	private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)buttonMoveUp).Enabled = ((ListControl)checkedListBox1).SelectedIndex >= 0;
		((Control)buttonMoveDown).Enabled = ((Control)buttonMoveUp).Enabled;
	}

	private void buttonMoveUp_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (((ListControl)checkedListBox1).SelectedIndex > 0)
		{
			int selectedIndex = ((ListControl)checkedListBox1).SelectedIndex;
			object obj = ((ObjectCollection)checkedListBox1.Items)[selectedIndex];
			CheckState itemCheckState = checkedListBox1.GetItemCheckState(selectedIndex);
			((ObjectCollection)checkedListBox1.Items).RemoveAt(selectedIndex);
			selectedIndex--;
			((ObjectCollection)checkedListBox1.Items).Insert(selectedIndex, obj);
			((ListControl)checkedListBox1).SelectedIndex = selectedIndex;
			checkedListBox1.SetItemCheckState(selectedIndex, itemCheckState);
		}
	}

	private void buttonMoveDown_Click(object sender, EventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (((ListControl)checkedListBox1).SelectedIndex < ((ObjectCollection)checkedListBox1.Items).Count - 1)
		{
			int selectedIndex = ((ListControl)checkedListBox1).SelectedIndex;
			object obj = ((ObjectCollection)checkedListBox1.Items)[selectedIndex];
			CheckState itemCheckState = checkedListBox1.GetItemCheckState(selectedIndex);
			((ObjectCollection)checkedListBox1.Items).RemoveAt(selectedIndex);
			selectedIndex++;
			((ObjectCollection)checkedListBox1.Items).Insert(selectedIndex, obj);
			((ListControl)checkedListBox1).SelectedIndex = selectedIndex;
			checkedListBox1.SetItemCheckState(selectedIndex, itemCheckState);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		checkedListBox1 = new CheckedListBox();
		panel1 = new Panel();
		buttonMoveDown = new ButtonX();
		buttonMoveUp = new ButtonX();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)checkedListBox1).Dock = (DockStyle)5;
		((ListControl)checkedListBox1).FormattingEnabled = true;
		((ListBox)checkedListBox1).IntegralHeight = false;
		((Control)checkedListBox1).Location = new Point(0, 0);
		((Control)checkedListBox1).Name = "checkedListBox1";
		((Control)checkedListBox1).Size = new Size(231, 196);
		((Control)checkedListBox1).TabIndex = 0;
		((ListBox)checkedListBox1).SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
		((Control)panel1).Controls.Add((Control)(object)buttonMoveDown);
		((Control)panel1).Controls.Add((Control)(object)buttonMoveUp);
		((Control)panel1).Dock = (DockStyle)4;
		((Control)panel1).Location = new Point(231, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(44, 196);
		((Control)panel1).TabIndex = 1;
		((Control)buttonMoveDown).AccessibleRole = (AccessibleRole)43;
		buttonMoveDown.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonMoveDown).Enabled = false;
		((Control)buttonMoveDown).Location = new Point(6, 89);
		((Control)buttonMoveDown).Name = "buttonMoveDown";
		((Control)buttonMoveDown).Size = new Size(30, 33);
		buttonMoveDown.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonMoveDown.Symbol = "\uf063";
		((Control)buttonMoveDown).TabIndex = 1;
		((Control)buttonMoveDown).Click += buttonMoveDown_Click;
		((Control)buttonMoveUp).AccessibleRole = (AccessibleRole)43;
		buttonMoveUp.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonMoveUp).Enabled = false;
		((Control)buttonMoveUp).Location = new Point(6, 50);
		((Control)buttonMoveUp).Name = "buttonMoveUp";
		((Control)buttonMoveUp).Size = new Size(30, 33);
		buttonMoveUp.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonMoveUp.Symbol = "\uf062";
		((Control)buttonMoveUp).TabIndex = 0;
		((Control)buttonMoveUp).Click += buttonMoveUp_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)checkedListBox1);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Name = "CheckedListBoxSelector";
		((Control)this).Size = new Size(275, 196);
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
