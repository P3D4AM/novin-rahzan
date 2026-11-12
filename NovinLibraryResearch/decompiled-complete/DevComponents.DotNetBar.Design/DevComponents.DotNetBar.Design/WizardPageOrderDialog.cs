using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design;

[ToolboxItem(false)]
internal class WizardPageOrderDialog : Form
{
	private IContainer components;

	private ListView listView1;

	private Button buttonOK;

	private Button buttonCancel;

	private Bar bar1;

	private ButtonItem buttonPageUp;

	private ButtonItem buttonPageDown;

	private ColumnHeader columnHeaderName;

	private ColumnHeader columnHeaderTitle;

	private ColumnHeader columnHeaderDesc;

	private Wizard m_Wizard;

	private ColumnHeader columnInterior;

	internal bool OrderChanged;

	internal string SelectedPageName
	{
		get
		{
			if (listView1.SelectedItems.Count == 1)
			{
				return listView1.SelectedItems[0].Text;
			}
			return "";
		}
	}

	internal string[] OrderedPageNames
	{
		get
		{
			string[] array = new string[listView1.Items.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = listView1.Items[i].Text;
			}
			return array;
		}
	}

	public WizardPageOrderDialog()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		listView1 = new ListView();
		columnHeaderName = new ColumnHeader();
		columnHeaderTitle = new ColumnHeader();
		columnHeaderDesc = new ColumnHeader();
		buttonOK = new Button();
		buttonCancel = new Button();
		bar1 = new Bar();
		buttonPageUp = new ButtonItem();
		buttonPageDown = new ButtonItem();
		columnInterior = new ColumnHeader();
		((ISupportInitialize)bar1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)listView1).Anchor = (AnchorStyles)15;
		listView1.Columns.AddRange((ColumnHeader[])(object)new ColumnHeader[4] { columnHeaderName, columnInterior, columnHeaderTitle, columnHeaderDesc });
		listView1.FullRowSelect = true;
		listView1.HeaderStyle = (ColumnHeaderStyle)1;
		listView1.HideSelection = false;
		((Control)listView1).Location = new Point(12, 31);
		listView1.MultiSelect = false;
		((Control)listView1).Name = "listView1";
		((Control)listView1).Size = new Size(488, 233);
		((Control)listView1).TabIndex = 0;
		listView1.View = (View)1;
		((Control)listView1).DoubleClick += listView1_DoubleClick;
		((Control)listView1).Resize += listView1_Resize;
		columnHeaderName.Text = "Name";
		columnHeaderName.Width = 100;
		columnHeaderTitle.Text = "Page Title";
		columnHeaderTitle.Width = 150;
		columnHeaderDesc.Text = "Page Description";
		columnHeaderDesc.Width = 187;
		((Control)buttonOK).Anchor = (AnchorStyles)10;
		buttonOK.DialogResult = (DialogResult)1;
		((Control)buttonOK).Location = new Point(352, 270);
		((Control)buttonOK).Name = "buttonOK";
		((Control)buttonOK).Size = new Size(73, 25);
		((Control)buttonOK).TabIndex = 1;
		((Control)buttonOK).Text = "OK";
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(427, 270);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(73, 25);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		bar1.BackgroundImageAlpha = byte.MaxValue;
		((Control)bar1).Dock = (DockStyle)1;
		bar1.Items.AddRange(new BaseItem[2] { buttonPageUp, buttonPageDown });
		bar1.Location = new Point(0, 0);
		bar1.Name = "bar1";
		bar1.Size = new Size(512, 25);
		bar1.Stretch = true;
		bar1.Style = eDotNetBarStyle.VS2005;
		bar1.TabIndex = 3;
		bar1.TabStop = false;
		((Control)bar1).Text = "bar1";
		buttonPageUp.Name = "buttonPageUp";
		buttonPageUp.Shortcuts.Add(eShortcut.CtrlUp);
		buttonPageUp.Text = "Move Page Up";
		buttonPageUp.Tooltip = "Change selected page order by moving it up";
		buttonPageUp.Click += buttonPageUp_Click;
		buttonPageDown.Name = "buttonPageDown";
		buttonPageDown.Shortcuts.Add(eShortcut.CtrlDown);
		buttonPageDown.Text = "Move Page Down";
		buttonPageDown.Tooltip = "Change selected page order by moving it down";
		buttonPageDown.Click += buttonPageDown_Click;
		columnInterior.Text = "Interior";
		columnInterior.Width = 47;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOK;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(512, 303);
		((Control)this).Controls.Add((Control)(object)bar1);
		((Control)this).Controls.Add((Control)(object)buttonCancel);
		((Control)this).Controls.Add((Control)(object)buttonOK);
		((Control)this).Controls.Add((Control)(object)listView1);
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "WizardPageOrderDialog";
		((Control)this).Text = "Wizard Pages";
		((ISupportInitialize)bar1).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void listView1_DoubleClick(object sender, EventArgs e)
	{
		if (listView1.SelectedItems.Count == 1)
		{
			((Form)this).DialogResult = (DialogResult)1;
			((Form)this).Close();
		}
	}

	public void SetWizard(Wizard w)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		m_Wizard = w;
		foreach (WizardPage wizardPage in m_Wizard.WizardPages)
		{
			ListViewItem val = new ListViewItem(new string[4]
			{
				((Control)wizardPage).Name,
				wizardPage.InteriorPage ? "Yes" : "No",
				wizardPage.PageTitle,
				wizardPage.PageDescription
			});
			listView1.Items.Add(val);
		}
	}

	private void buttonPageUp_Click(object sender, EventArgs e)
	{
		if (listView1.SelectedItems.Count == 1)
		{
			int index = listView1.SelectedItems[0].Index;
			if (index != 0)
			{
				ListViewItem val = listView1.SelectedItems[0];
				listView1.Items.Remove(val);
				listView1.Items.Insert(index - 1, val);
				OrderChanged = true;
			}
		}
	}

	private void buttonPageDown_Click(object sender, EventArgs e)
	{
		if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems.Count == 1)
		{
			int index = listView1.SelectedItems[0].Index;
			if (index != listView1.Items.Count - 1)
			{
				ListViewItem val = listView1.SelectedItems[0];
				listView1.Items.Remove(val);
				listView1.Items.Insert(index + 1, val);
				OrderChanged = false;
			}
		}
	}

	private void listView1_Resize(object sender, EventArgs e)
	{
		int num = ((Control)listView1).Width - columnHeaderName.Width - columnInterior.Width - columnHeaderTitle.Width - 6;
		if (num > 64)
		{
			columnHeaderDesc.Width = num;
		}
	}
}
