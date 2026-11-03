using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using AccSqlServerPersistence.AssistanceClasses;
using DevComponents.DotNetBar;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms;

public class FRMSetSqlEngine : AccountingBaseForm
{
	private string _serverConnectionsString = "";

	private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

	private bool _isDisposed;

	private IContainer components;

	private TextBox TxtServerName;

	private Label label1;

	private RadioButton RBWindowsDetect;

	private RadioButton RBSqlServerDetect;

	private GroupBox GBoxSql;

	private Label label3;

	private Label label2;

	private TextBox TxtPassword;

	private TextBox TxtUserName;

	private Panel panel1;

	private ButtonX BtnTest;

	private ButtonX BTNCancel;

	private ButtonX BtnOk;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public FRMSetSqlEngine(string contextGuid)
		: base(contextGuid)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
	}

	private void FRMSetSqlEngine_Load(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		try
		{
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString);
			((Control)TxtServerName).Text = val.DataSource;
			RBWindowsDetect.Checked = val.IntegratedSecurity;
			RBSqlServerDetect.Checked = !val.IntegratedSecurity;
			((Control)TxtUserName).Text = val.UserID;
			((Control)TxtPassword).Text = val.Password;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMSetSqlEngine_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMSetSqlEngine.cs", 32);
		}
	}

	private void BTNCancel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void BtnOk_Click(object sender, EventArgs e)
	{
		try
		{
			if (TestConnection(swShowDialo: false))
			{
				AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString = ((DbConnectionStringBuilder)(object)builder).ConnectionString;
				_serverConnectionsString = ((DbConnectionStringBuilder)(object)builder).ConnectionString;
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnOk_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMSetSqlEngine.cs", 54);
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
	}

	private bool TestConnection(bool swShowDialo)
	{
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00e3: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			builder.DataSource = ((Control)TxtServerName).Text;
			if (RBSqlServerDetect.Checked)
			{
				builder.UserID = ((Control)TxtUserName).Text;
				builder.Password = ((Control)TxtPassword).Text;
				builder.IntegratedSecurity = false;
			}
			else
			{
				builder.UserID = "";
				builder.Password = "";
				builder.IntegratedSecurity = true;
			}
			builder.MultipleActiveResultSets = true;
			SqlConnection val = new SqlConnection();
			((DbConnection)val).ConnectionString = ((DbConnectionStringBuilder)(object)builder).ConnectionString;
			((DbConnection)val).Open();
			if (swShowDialo)
			{
				MessageBox.Show((IWin32Window)(object)this, "ارتباط با بانک اطلاعاتی با موفقیت برقرار شد", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			}
			AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString = ((DbConnectionStringBuilder)(object)builder).ConnectionString;
			((DbConnection)val).Close();
			return true;
		}
		catch (Exception)
		{
			MessageBox.Show((IWin32Window)(object)this, "ارتباط با بانک اطلاعاتی برقرار نیست", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			return false;
		}
	}

	private void BtnTest_Click(object sender, EventArgs e)
	{
		try
		{
			TestConnection(swShowDialo: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnTest_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMSetSqlEngine.cs", 106);
		}
	}

	private void RBSqlServerDetect_CheckedChanged(object sender, EventArgs e)
	{
		((Control)GBoxSql).Enabled = RBSqlServerDetect.Checked;
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData != 117)
		{
			return ((Form)this).ProcessCmdKey(ref msg, keyData);
		}
		return true;
	}

	private void FRMSetSqlEngine_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		Keys keyData = e.KeyData;
		if ((int)keyData != 27)
		{
			if ((int)keyData != 116)
			{
				if ((int)keyData == 117)
				{
					BtnTest.PerformClick();
				}
			}
			else
			{
				BtnOk.PerformClick();
			}
		}
		else
		{
			BTNCancel.PerformClick();
		}
	}

	private void FRMSetSqlEngine_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMSetSqlEngine.cs", 157);
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Expected O, but got Unknown
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Expected O, but got Unknown
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Expected O, but got Unknown
		TxtServerName = new TextBox();
		label1 = new Label();
		RBWindowsDetect = new RadioButton();
		RBSqlServerDetect = new RadioButton();
		GBoxSql = new GroupBox();
		label3 = new Label();
		label2 = new Label();
		TxtPassword = new TextBox();
		TxtUserName = new TextBox();
		panel1 = new Panel();
		BtnTest = new ButtonX();
		BTNCancel = new ButtonX();
		BtnOk = new ButtonX();
		((Control)GBoxSql).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TxtServerName).Font = new Font("B Nazanin", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtServerName).Location = new Point(21, 29);
		((Control)TxtServerName).Name = "TxtServerName";
		((Control)TxtServerName).RightToLeft = (RightToLeft)0;
		((Control)TxtServerName).Size = new Size(259, 31);
		((Control)TxtServerName).TabIndex = 0;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label1).Location = new Point(288, 29);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(63, 26);
		((Control)label1).TabIndex = 4;
		((Control)label1).Text = "[نام سرور]:";
		((Control)RBWindowsDetect).AutoSize = true;
		((Control)RBWindowsDetect).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)RBWindowsDetect).Location = new Point(158, 3);
		((Control)RBWindowsDetect).Name = "RBWindowsDetect";
		((Control)RBWindowsDetect).Size = new Size(167, 30);
		((Control)RBWindowsDetect).TabIndex = 1;
		RBWindowsDetect.TabStop = true;
		((Control)RBWindowsDetect).Text = "[احراز هویت توسط ویندوز]";
		((ButtonBase)RBWindowsDetect).UseVisualStyleBackColor = true;
		((Control)RBSqlServerDetect).AutoSize = true;
		((Control)RBSqlServerDetect).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)RBSqlServerDetect).Location = new Point(158, 38);
		((Control)RBSqlServerDetect).Name = "RBSqlServerDetect";
		((Control)RBSqlServerDetect).Size = new Size(169, 30);
		((Control)RBSqlServerDetect).TabIndex = 2;
		RBSqlServerDetect.TabStop = true;
		((Control)RBSqlServerDetect).Text = "[احراز هویت توسط] SQL";
		((ButtonBase)RBSqlServerDetect).UseVisualStyleBackColor = true;
		RBSqlServerDetect.CheckedChanged += RBSqlServerDetect_CheckedChanged;
		((Control)GBoxSql).Controls.Add((Control)(object)label3);
		((Control)GBoxSql).Controls.Add((Control)(object)label2);
		((Control)GBoxSql).Controls.Add((Control)(object)TxtPassword);
		((Control)GBoxSql).Controls.Add((Control)(object)TxtUserName);
		((Control)GBoxSql).Enabled = false;
		((Control)GBoxSql).Font = new Font("B Nazanin", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)GBoxSql).Location = new Point(21, 137);
		((Control)GBoxSql).Name = "GBoxSql";
		((Control)GBoxSql).Size = new Size(335, 102);
		((Control)GBoxSql).TabIndex = 8;
		GBoxSql.TabStop = false;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label3).Location = new Point(257, 62);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(69, 26);
		((Control)label3).TabIndex = 3;
		((Control)label3).Text = "[رمز عبور]:";
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("B Nazanin", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label2).Location = new Point(257, 22);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(75, 26);
		((Control)label2).TabIndex = 2;
		((Control)label2).Text = "[نام کاربری]:";
		((Control)TxtPassword).Font = new Font("B Nazanin", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtPassword).Location = new Point(16, 63);
		((Control)TxtPassword).Name = "TxtPassword";
		TxtPassword.PasswordChar = '*';
		((Control)TxtPassword).RightToLeft = (RightToLeft)0;
		((Control)TxtPassword).Size = new Size(230, 31);
		((Control)TxtPassword).TabIndex = 4;
		((Control)TxtUserName).Font = new Font("B Nazanin", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TxtUserName).Location = new Point(16, 20);
		((Control)TxtUserName).Name = "TxtUserName";
		((Control)TxtUserName).RightToLeft = (RightToLeft)0;
		((Control)TxtUserName).Size = new Size(230, 31);
		((Control)TxtUserName).TabIndex = 3;
		((Control)panel1).Controls.Add((Control)(object)RBWindowsDetect);
		((Control)panel1).Controls.Add((Control)(object)RBSqlServerDetect);
		((Control)panel1).Font = new Font("B Nazanin", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)panel1).Location = new Point(21, 68);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(335, 74);
		((Control)panel1).TabIndex = 9;
		((Control)BtnTest).AccessibleRole = (AccessibleRole)43;
		BtnTest.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnTest).Location = new Point(216, 260);
		((Control)BtnTest).Name = "BtnTest";
		((Control)BtnTest).Size = new Size(99, 34);
		BtnTest.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnTest).TabIndex = 10;
		((Control)BtnTest).Text = "[تست ارتباط](F6)";
		((Control)BtnTest).Click += BtnTest_Click;
		((Control)BTNCancel).AccessibleRole = (AccessibleRole)43;
		BTNCancel.ColorTable = eButtonColor.OrangeWithBackground;
		BTNCancel.Image = (Image)(object)FormCustom.Properties.Resources.stop;
		BTNCancel.ImagePosition = eImagePosition.Right;
		((Control)BTNCancel).Location = new Point(7, 260);
		((Control)BTNCancel).Name = "BTNCancel";
		((Control)BTNCancel).Size = new Size(99, 34);
		BTNCancel.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNCancel).TabIndex = 11;
		((Control)BTNCancel).Text = "[انصراف](Esc)";
		((Control)BTNCancel).Click += BTNCancel_Click;
		((Control)BtnOk).AccessibleRole = (AccessibleRole)43;
		BtnOk.ColorTable = eButtonColor.OrangeWithBackground;
		BtnOk.Image = (Image)(object)FormCustom.Properties.Resources.accept;
		BtnOk.ImagePosition = eImagePosition.Right;
		((Control)BtnOk).Location = new Point(111, 260);
		((Control)BtnOk).Name = "BtnOk";
		((Control)BtnOk).Size = new Size(99, 34);
		BtnOk.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnOk).TabIndex = 12;
		((Control)BtnOk).Text = "[تائید](F5)";
		((Control)BtnOk).Click += BtnOk_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(208, 228, 255);
		((Form)this).ClientSize = new Size(359, 300);
		((Control)this).Controls.Add((Control)(object)BtnOk);
		((Control)this).Controls.Add((Control)(object)BTNCancel);
		((Control)this).Controls.Add((Control)(object)BtnTest);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)GBoxSql);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)TxtServerName);
		((Control)this).ForeColor = SystemColors.ControlText;
		((Form)this).FormBorderStyle = (FormBorderStyle)2;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMSetSqlEngine";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "[تنظیمات ارتباط با شبکه]";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMSetSqlEngine_FormClosed);
		((Form)this).Load += FRMSetSqlEngine_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMSetSqlEngine_KeyDown);
		((Control)GBoxSql).ResumeLayout(false);
		((Control)GBoxSql).PerformLayout();
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
