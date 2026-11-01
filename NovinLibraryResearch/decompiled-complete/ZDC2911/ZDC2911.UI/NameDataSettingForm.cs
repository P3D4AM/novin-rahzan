using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Riss.Devices;
using ZDC2911.IConvert;

namespace ZDC2911.UI;

public class NameDataSettingForm : Form
{
	private string operationType;

	private Zd2911EnrollFileManagement fileMgr;

	private IContainer components = null;

	private ListView lvw_UserList;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private ColumnHeader columnHeader3;

	private Button btn_OpenFile;

	private Button btn_SaveFile;

	private GroupBox groupBox1;

	private Button btn_Cancle;

	private Button btn_Save;

	private Button btn_Edit;

	private Button btn_Add;

	private TextBox txt_UserName;

	private Label lbl_UserName;

	private Label lbl_UserId;

	private SaveFileDialog sfd_SaveFile;

	private OpenFileDialog ofd_OpenFile;

	private Button btn_Delete;

	private TextBox txt_UserId;

	public NameDataSettingForm()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		fileMgr = new Zd2911EnrollFileManagement();
	}

	private void ClearText()
	{
		((Control)txt_UserId).Text = string.Empty;
		((Control)txt_UserName).Text = string.Empty;
	}

	private bool CheckData()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)txt_UserId).Text.Trim().Equals(string.Empty))
		{
			MessageBox.Show("Please Input the User ID", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txt_UserId).Focus();
			return false;
		}
		if (!ConvertObject.IsInt(((Control)txt_UserId).Text.Trim()))
		{
			MessageBox.Show("User ID can only enter a number", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txt_UserId).Focus();
			return false;
		}
		foreach (ListViewItem item in lvw_UserList.Items)
		{
			ListViewItem val = item;
			if (((Control)txt_UserId).Text.Trim().Equals(val.SubItems[1].Text))
			{
				MessageBox.Show("The user ID already exists, please modify", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				((Control)txt_UserId).Focus();
				return false;
			}
		}
		if (((Control)txt_UserName).Text.Trim().Equals(string.Empty))
		{
			MessageBox.Show("Please input the user name", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			((Control)txt_UserName).Focus();
			return false;
		}
		return true;
	}

	private void NameDataSettingForm_Load(object sender, EventArgs e)
	{
		((TextBoxBase)txt_UserId).ReadOnly = true;
		((TextBoxBase)txt_UserName).ReadOnly = true;
		((Control)btn_Save).Enabled = false;
		((Control)btn_Cancle).Enabled = false;
	}

	private void btn_Add_Click(object sender, EventArgs e)
	{
		operationType = "Add";
		((TextBoxBase)txt_UserId).ReadOnly = false;
		((TextBoxBase)txt_UserName).ReadOnly = false;
		((Control)btn_Add).Enabled = false;
		((Control)btn_Edit).Enabled = false;
		((Control)btn_Save).Enabled = true;
		((Control)btn_Cancle).Enabled = true;
		((Control)btn_Delete).Enabled = false;
		ClearText();
	}

	private void btn_Edit_Click(object sender, EventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (lvw_UserList.SelectedItems.Count == 0)
		{
			MessageBox.Show("Please select the record after the operation", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return;
		}
		operationType = "Edit";
		((TextBoxBase)txt_UserId).ReadOnly = true;
		((TextBoxBase)txt_UserName).ReadOnly = false;
		((Control)btn_Add).Enabled = false;
		((Control)btn_Edit).Enabled = false;
		((Control)btn_Save).Enabled = true;
		((Control)btn_Cancle).Enabled = true;
		((Control)btn_Delete).Enabled = false;
	}

	private void btn_Save_Click(object sender, EventArgs e)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		if (operationType.Equals("Add"))
		{
			if (!CheckData())
			{
				return;
			}
			int num = 1;
			if (lvw_UserList.Items.Count > 0)
			{
				num = Convert.ToInt32(lvw_UserList.Items[lvw_UserList.Items.Count - 1].SubItems[0].Text) + 1;
			}
			ListViewItem val = new ListViewItem(new string[3]
			{
				num.ToString(),
				((Control)txt_UserId).Text.Trim(),
				((Control)txt_UserName).Text.Trim()
			});
			lvw_UserList.Items.Add(val);
			ClearText();
		}
		else
		{
			if (((Control)txt_UserName).Text.Trim().Equals(string.Empty))
			{
				MessageBox.Show("Please input the user name", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				((Control)txt_UserName).Focus();
				return;
			}
			lvw_UserList.SelectedItems[0].SubItems[2].Text = ((Control)txt_UserName).Text.Trim();
		}
		((TextBoxBase)txt_UserId).ReadOnly = true;
		((TextBoxBase)txt_UserName).ReadOnly = true;
		((Control)btn_Add).Enabled = true;
		((Control)btn_Edit).Enabled = true;
		((Control)btn_Save).Enabled = false;
		((Control)btn_Cancle).Enabled = false;
		((Control)btn_Delete).Enabled = true;
	}

	private void btn_Cancle_Click(object sender, EventArgs e)
	{
		if (operationType.Equals("Add"))
		{
			ClearText();
		}
		else
		{
			((Control)txt_UserId).Text = lvw_UserList.SelectedItems[0].SubItems[1].Text;
			((Control)txt_UserName).Text = lvw_UserList.SelectedItems[0].SubItems[2].Text;
		}
		((TextBoxBase)txt_UserId).ReadOnly = true;
		((TextBoxBase)txt_UserName).ReadOnly = true;
		((Control)btn_Add).Enabled = true;
		((Control)btn_Edit).Enabled = true;
		((Control)btn_Save).Enabled = false;
		((Control)btn_Cancle).Enabled = false;
		((Control)btn_Delete).Enabled = true;
	}

	private void btn_Delete_Click(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (lvw_UserList.SelectedItems.Count == 0)
		{
			MessageBox.Show("Please select the record after the operation", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			return;
		}
		DialogResult val = MessageBox.Show("Are you sure you want to delete", "Prompt", (MessageBoxButtons)4, (MessageBoxIcon)64);
		if (((object)(DialogResult)(ref val)).Equals((object)(DialogResult)6))
		{
			lvw_UserList.Items.Remove(lvw_UserList.SelectedItems[0]);
			ClearText();
		}
	}

	private void lvw_UserList_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (lvw_UserList.SelectedItems.Count > 0)
		{
			((Control)txt_UserId).Text = lvw_UserList.SelectedItems[0].SubItems[1].Text;
			((Control)txt_UserName).Text = lvw_UserList.SelectedItems[0].SubItems[2].Text;
		}
	}

	private void btn_SaveFile_Click(object sender, EventArgs e)
	{
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		if (lvw_UserList.Items.Count == 0)
		{
			return;
		}
		try
		{
			List<User> list = new List<User>();
			foreach (ListViewItem item in lvw_UserList.Items)
			{
				ListViewItem val = item;
				User val2 = new User();
				val2.DIN = Convert.ToUInt64(val.SubItems[1].Text);
				val2.UserName = val.SubItems[2].Text;
				list.Add(val2);
			}
			DialogResult val3 = ((CommonDialog)sfd_SaveFile).ShowDialog();
			if (((object)(DialogResult)(ref val3)).Equals((object)(DialogResult)1) && !fileMgr.SaveUserNameData(((FileDialog)sfd_SaveFile).FileName, list))
			{
				MessageBox.Show("Save File Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Save File Occur Error：" + ex.Message, "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
	}

	private void btn_OpenFile_Click(object sender, EventArgs e)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		try
		{
			List<User> list = new List<User>();
			DialogResult val = ((CommonDialog)ofd_OpenFile).ShowDialog();
			if (!((object)(DialogResult)(ref val)).Equals((object)(DialogResult)1))
			{
				return;
			}
			if (!fileMgr.LoadUserNameData(((FileDialog)ofd_OpenFile).FileName, ref list))
			{
				MessageBox.Show("Open File Fail", "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)48);
				return;
			}
			lvw_UserList.Items.Clear();
			int num = 1;
			foreach (User item in list)
			{
				ListViewItem val2 = new ListViewItem(new string[3]
				{
					num.ToString(),
					item.DIN.ToString(),
					item.UserName
				});
				lvw_UserList.Items.Add(val2);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Open File Occur Error：" + ex.Message, "Prompt", (MessageBoxButtons)0, (MessageBoxIcon)16);
		}
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		lvw_UserList = new ListView();
		columnHeader1 = new ColumnHeader();
		columnHeader2 = new ColumnHeader();
		columnHeader3 = new ColumnHeader();
		btn_OpenFile = new Button();
		btn_SaveFile = new Button();
		groupBox1 = new GroupBox();
		btn_Delete = new Button();
		btn_Cancle = new Button();
		btn_Save = new Button();
		btn_Edit = new Button();
		btn_Add = new Button();
		txt_UserName = new TextBox();
		lbl_UserName = new Label();
		lbl_UserId = new Label();
		sfd_SaveFile = new SaveFileDialog();
		ofd_OpenFile = new OpenFileDialog();
		txt_UserId = new TextBox();
		((Control)groupBox1).SuspendLayout();
		((Control)this).SuspendLayout();
		lvw_UserList.Columns.AddRange((ColumnHeader[])(object)new ColumnHeader[3] { columnHeader1, columnHeader2, columnHeader3 });
		lvw_UserList.FullRowSelect = true;
		lvw_UserList.GridLines = true;
		lvw_UserList.HideSelection = false;
		((Control)lvw_UserList).Location = new Point(12, 12);
		lvw_UserList.MultiSelect = false;
		((Control)lvw_UserList).Name = "lvw_UserList";
		((Control)lvw_UserList).Size = new Size(591, 286);
		((Control)lvw_UserList).TabIndex = 0;
		lvw_UserList.UseCompatibleStateImageBehavior = false;
		lvw_UserList.View = (View)1;
		lvw_UserList.SelectedIndexChanged += lvw_UserList_SelectedIndexChanged;
		columnHeader1.Text = "SN";
		columnHeader1.Width = 90;
		columnHeader2.Text = "User ID";
		columnHeader2.TextAlign = (HorizontalAlignment)2;
		columnHeader2.Width = 150;
		columnHeader3.Text = "Name";
		columnHeader3.TextAlign = (HorizontalAlignment)2;
		columnHeader3.Width = 150;
		((Control)btn_OpenFile).Location = new Point(447, 304);
		((Control)btn_OpenFile).Name = "btn_OpenFile";
		((Control)btn_OpenFile).Size = new Size(75, 23);
		((Control)btn_OpenFile).TabIndex = 1;
		((Control)btn_OpenFile).Text = "Open";
		((ButtonBase)btn_OpenFile).UseVisualStyleBackColor = true;
		((Control)btn_OpenFile).Click += btn_OpenFile_Click;
		((Control)btn_SaveFile).Location = new Point(528, 304);
		((Control)btn_SaveFile).Name = "btn_SaveFile";
		((Control)btn_SaveFile).Size = new Size(75, 23);
		((Control)btn_SaveFile).TabIndex = 2;
		((Control)btn_SaveFile).Text = "Save";
		((ButtonBase)btn_SaveFile).UseVisualStyleBackColor = true;
		((Control)btn_SaveFile).Click += btn_SaveFile_Click;
		((Control)groupBox1).Controls.Add((Control)(object)btn_Delete);
		((Control)groupBox1).Controls.Add((Control)(object)btn_Cancle);
		((Control)groupBox1).Controls.Add((Control)(object)btn_Save);
		((Control)groupBox1).Controls.Add((Control)(object)btn_Edit);
		((Control)groupBox1).Controls.Add((Control)(object)btn_Add);
		((Control)groupBox1).Controls.Add((Control)(object)txt_UserName);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_UserName);
		((Control)groupBox1).Controls.Add((Control)(object)txt_UserId);
		((Control)groupBox1).Controls.Add((Control)(object)lbl_UserId);
		((Control)groupBox1).Location = new Point(12, 333);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(591, 77);
		((Control)groupBox1).TabIndex = 3;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Detail Information";
		((Control)btn_Delete).Location = new Point(510, 48);
		((Control)btn_Delete).Name = "btn_Delete";
		((Control)btn_Delete).Size = new Size(75, 23);
		((Control)btn_Delete).TabIndex = 8;
		((Control)btn_Delete).Text = "Delete";
		((ButtonBase)btn_Delete).UseVisualStyleBackColor = true;
		((Control)btn_Delete).Click += btn_Delete_Click;
		((Control)btn_Cancle).Location = new Point(429, 47);
		((Control)btn_Cancle).Name = "btn_Cancle";
		((Control)btn_Cancle).Size = new Size(75, 23);
		((Control)btn_Cancle).TabIndex = 7;
		((Control)btn_Cancle).Text = "Cancel";
		((ButtonBase)btn_Cancle).UseVisualStyleBackColor = true;
		((Control)btn_Cancle).Click += btn_Cancle_Click;
		((Control)btn_Save).Location = new Point(348, 47);
		((Control)btn_Save).Name = "btn_Save";
		((Control)btn_Save).Size = new Size(75, 23);
		((Control)btn_Save).TabIndex = 6;
		((Control)btn_Save).Text = "Save";
		((ButtonBase)btn_Save).UseVisualStyleBackColor = true;
		((Control)btn_Save).Click += btn_Save_Click;
		((Control)btn_Edit).Location = new Point(267, 47);
		((Control)btn_Edit).Name = "btn_Edit";
		((Control)btn_Edit).Size = new Size(75, 23);
		((Control)btn_Edit).TabIndex = 5;
		((Control)btn_Edit).Text = "Edit";
		((ButtonBase)btn_Edit).UseVisualStyleBackColor = true;
		((Control)btn_Edit).Click += btn_Edit_Click;
		((Control)btn_Add).Location = new Point(186, 47);
		((Control)btn_Add).Name = "btn_Add";
		((Control)btn_Add).Size = new Size(75, 23);
		((Control)btn_Add).TabIndex = 4;
		((Control)btn_Add).Text = "Add";
		((ButtonBase)btn_Add).UseVisualStyleBackColor = true;
		((Control)btn_Add).Click += btn_Add_Click;
		((Control)txt_UserName).Location = new Point(435, 20);
		((TextBoxBase)txt_UserName).MaxLength = 12;
		((Control)txt_UserName).Name = "txt_UserName";
		((Control)txt_UserName).Size = new Size(150, 21);
		((Control)txt_UserName).TabIndex = 3;
		((Control)lbl_UserName).AutoSize = true;
		((Control)lbl_UserName).Location = new Point(394, 23);
		((Control)lbl_UserName).Name = "lbl_UserName";
		((Control)lbl_UserName).Size = new Size(35, 12);
		((Control)lbl_UserName).TabIndex = 2;
		((Control)lbl_UserName).Text = "Name:";
		lbl_UserName.TextAlign = (ContentAlignment)16;
		((Control)lbl_UserId).AutoSize = true;
		((Control)lbl_UserId).Location = new Point(6, 23);
		((Control)lbl_UserId).Name = "lbl_UserId";
		((Control)lbl_UserId).Size = new Size(53, 12);
		((Control)lbl_UserId).TabIndex = 0;
		((Control)lbl_UserId).Text = "User ID:";
		lbl_UserId.TextAlign = (ContentAlignment)16;
		((FileDialog)sfd_SaveFile).Filter = "Text File|*.nm";
		((FileDialog)ofd_OpenFile).Filter = "Text File|*.nm";
		((Control)txt_UserId).Location = new Point(65, 20);
		((TextBoxBase)txt_UserId).MaxLength = 18;
		((Control)txt_UserId).Name = "txt_UserId";
		((Control)txt_UserId).Size = new Size(150, 21);
		((Control)txt_UserId).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(615, 419);
		((Control)this).Controls.Add((Control)(object)groupBox1);
		((Control)this).Controls.Add((Control)(object)btn_SaveFile);
		((Control)this).Controls.Add((Control)(object)btn_OpenFile);
		((Control)this).Controls.Add((Control)(object)lvw_UserList);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "NameDataSettingForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Name Data Settings";
		((Form)this).Load += NameDataSettingForm_Load;
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
