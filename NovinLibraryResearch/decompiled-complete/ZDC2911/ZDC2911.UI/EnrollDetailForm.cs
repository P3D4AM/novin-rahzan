using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Riss.Devices;

namespace ZDC2911.UI;

public class EnrollDetailForm : Form
{
	private User user;

	private IContainer components = null;

	private ComboBox cbo_Role;

	private TextBox txt_UserId;

	private Label lbl_Role;

	private Label lbl_UserId;

	private CheckedListBox clb_Fp;

	public EnrollDetailForm(User user)
	{
		InitializeComponent();
		this.user = user;
	}

	private void SetPrivilege()
	{
		switch (user.Privilege)
		{
		case 1:
			((ListControl)cbo_Role).SelectedIndex = 0;
			break;
		case 8:
			((ListControl)cbo_Role).SelectedIndex = 1;
			break;
		case 2:
			((ListControl)cbo_Role).SelectedIndex = 2;
			break;
		case 4:
			((ListControl)cbo_Role).SelectedIndex = 3;
			break;
		case 16:
			((ListControl)cbo_Role).SelectedIndex = 4;
			break;
		}
	}

	private void EnrollDetailForm_Load(object sender, EventArgs e)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected I4, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected I4, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected I4, but got Unknown
		((Control)txt_UserId).Text = user.DIN.ToString();
		Enroll val = user.Enrolls[0];
		SetPrivilege();
		for (int i = 0; i < 10; i++)
		{
			if (Zd2911Utils.BitCheck((int)val.EnrollType, i) != 0)
			{
				clb_Fp.SetItemChecked(i, true);
			}
		}
		if (Zd2911Utils.BitCheck((int)val.EnrollType, 10) != 0)
		{
			clb_Fp.SetItemChecked(10, true);
		}
		if (Zd2911Utils.BitCheck((int)val.EnrollType, 11) != 0)
		{
			clb_Fp.SetItemChecked(11, true);
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
		cbo_Role = new ComboBox();
		txt_UserId = new TextBox();
		lbl_Role = new Label();
		lbl_UserId = new Label();
		clb_Fp = new CheckedListBox();
		((Control)this).SuspendLayout();
		cbo_Role.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbo_Role).Enabled = false;
		((ListControl)cbo_Role).FormattingEnabled = true;
		cbo_Role.Items.AddRange(new object[4] { "普通用户", "超级管理员", "登记管理者", "查询管理者" });
		((Control)cbo_Role).Location = new Point(83, 33);
		((Control)cbo_Role).Name = "cbo_Role";
		((Control)cbo_Role).Size = new Size(173, 20);
		((Control)cbo_Role).TabIndex = 3;
		((Control)txt_UserId).BackColor = Color.White;
		((Control)txt_UserId).Location = new Point(83, 6);
		((Control)txt_UserId).Name = "txt_UserId";
		((TextBoxBase)txt_UserId).ReadOnly = true;
		((Control)txt_UserId).Size = new Size(173, 21);
		((Control)txt_UserId).TabIndex = 1;
		((Control)lbl_Role).AutoSize = true;
		((Control)lbl_Role).Location = new Point(12, 36);
		((Control)lbl_Role).Name = "lbl_Role";
		((Control)lbl_Role).Size = new Size(65, 12);
		((Control)lbl_Role).TabIndex = 2;
		((Control)lbl_Role).Text = "Privilege:";
		lbl_Role.TextAlign = (ContentAlignment)32;
		((Control)lbl_UserId).AutoSize = true;
		((Control)lbl_UserId).Location = new Point(24, 9);
		((Control)lbl_UserId).Name = "lbl_UserId";
		((Control)lbl_UserId).Size = new Size(53, 12);
		((Control)lbl_UserId).TabIndex = 0;
		((Control)lbl_UserId).Text = "User ID:";
		lbl_UserId.TextAlign = (ContentAlignment)32;
		((Control)clb_Fp).Enabled = false;
		((ListControl)clb_Fp).FormattingEnabled = true;
		((ObjectCollection)clb_Fp.Items).AddRange(new object[12]
		{
			"FP 0", "FP 1", "FP 2", "FP 3", "FP 4", "FP 5", "FP 6", "FP 7", "FP 8", "FP 9",
			"PWD", "Card"
		});
		((Control)clb_Fp).Location = new Point(12, 59);
		((ListBox)clb_Fp).MultiColumn = true;
		((Control)clb_Fp).Name = "clb_Fp";
		((Control)clb_Fp).Size = new Size(244, 116);
		((Control)clb_Fp).TabIndex = 4;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(269, 190);
		((Control)this).Controls.Add((Control)(object)clb_Fp);
		((Control)this).Controls.Add((Control)(object)cbo_Role);
		((Control)this).Controls.Add((Control)(object)txt_UserId);
		((Control)this).Controls.Add((Control)(object)lbl_Role);
		((Control)this).Controls.Add((Control)(object)lbl_UserId);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).MaximizeBox = false;
		((Control)this).Name = "EnrollDetailForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "User Enroll Information";
		((Form)this).Load += EnrollDetailForm_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
