using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.ManagementPassword;

public class frmManagementPass : AccountingBaseForm
{
	private bool _isDisposed;

	private IContainer components;

	internal TextBox TXTPas;

	internal Label Label1;

	internal TableLayoutPanel TableLayoutPanel1;

	internal Button OK_Button;

	internal Button Cancel_Button;

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

	public bool CheckDifficultPassword { get; private set; }

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\frmManagementPass.cs", 26);
		}
	}

	public frmManagementPass(string contextGuid, bool checkDifficultPassword = false)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			CheckDifficultPassword = checkDifficultPassword;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\frmManagementPass.cs", 36);
		}
	}

	private void OK_Button_Click(object sender, EventArgs e)
	{
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			bool flag = ((Control)TXTPas).Text == (string)CLSSystem.GlobalManagerPass && !CheckDifficultPassword;
			if (flag || ((flag | (CheckDifficultPassword && ((Control)TXTPas).Text == CLSSystem.DifficultManagerPass)) ? true : false))
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			else
			{
				((CacheBaseForm)this).ShowError("رمز وارد شده صحیح نیست".GetTranslate(), "خطا".GetTranslate());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OK_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\frmManagementPass.cs", 50);
		}
	}

	private void Cancel_Button_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Cancel_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\frmManagementPass.cs", 59);
		}
	}

	private void TXTPas_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				OK_Button.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTPas_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\frmManagementPass.cs", 68);
		}
	}

	private void TXTPas_Enter(object sender, EventArgs e)
	{
		((Control)TXTPas).BackColor = Color.Yellow;
	}

	private void TXTPas_Leave(object sender, EventArgs e)
	{
		((Control)TXTPas).BackColor = Color.White;
	}

	private void frmManagementPass_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 3)
			{
				Cancel_Button.PerformClick();
			}
			if ((int)e.KeyCode == 27)
			{
				Cancel_Button.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmManagementPass_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\frmManagementPass.cs", 79);
		}
	}

	private void frmManagementPass_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
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
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Expected O, but got Unknown
		TXTPas = new TextBox();
		Label1 = new Label();
		TableLayoutPanel1 = new TableLayoutPanel();
		OK_Button = new Button();
		Cancel_Button = new Button();
		((Control)TableLayoutPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TXTPas).Location = new Point(14, 9);
		((Control)TXTPas).Name = "TXTPas";
		((Control)TXTPas).Size = new Size(226, 20);
		((Control)TXTPas).TabIndex = 5;
		TXTPas.UseSystemPasswordChar = true;
		((Control)TXTPas).Enter += TXTPas_Enter;
		((Control)TXTPas).KeyDown += new KeyEventHandler(TXTPas_KeyDown);
		((Control)TXTPas).Leave += TXTPas_Leave;
		((Control)Label1).Font = new Font("PeydaWeb", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = Color.Red;
		((Control)Label1).Location = new Point(246, 8);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(73, 23);
		((Control)Label1).TabIndex = 4;
		((Control)Label1).Text = "[رمــــــــز   ورود]:";
		Label1.TextAlign = (ContentAlignment)64;
		TableLayoutPanel1.ColumnCount = 2;
		TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 50f));
		TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 50f));
		TableLayoutPanel1.Controls.Add((Control)(object)OK_Button, 0, 0);
		TableLayoutPanel1.Controls.Add((Control)(object)Cancel_Button, 1, 0);
		((Control)TableLayoutPanel1).Location = new Point(12, 35);
		((Control)TableLayoutPanel1).Name = "TableLayoutPanel1";
		TableLayoutPanel1.RowCount = 1;
		TableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 50f));
		((Control)TableLayoutPanel1).Size = new Size(177, 29);
		((Control)TableLayoutPanel1).TabIndex = 6;
		((Control)OK_Button).Cursor = Cursors.Hand;
		((Control)OK_Button).Dock = (DockStyle)5;
		((Control)OK_Button).ForeColor = Color.White;
		((Control)OK_Button).Location = new Point(92, 3);
		((Control)OK_Button).Name = "OK_Button";
		((Control)OK_Button).Size = new Size(82, 23);
		((Control)OK_Button).TabIndex = 0;
		((Control)OK_Button).Text = "[تایید]";
		((Control)OK_Button).Click += OK_Button_Click;
		((Control)Cancel_Button).Cursor = Cursors.Hand;
		Cancel_Button.DialogResult = (DialogResult)2;
		((Control)Cancel_Button).Dock = (DockStyle)5;
		((Control)Cancel_Button).ForeColor = Color.White;
		((Control)Cancel_Button).Location = new Point(3, 3);
		((Control)Cancel_Button).Name = "Cancel_Button";
		((Control)Cancel_Button).Size = new Size(83, 23);
		((Control)Cancel_Button).TabIndex = 1;
		((Control)Cancel_Button).Text = "[انصراف]";
		((Control)Cancel_Button).Click += Cancel_Button_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		((Form)this).ClientSize = new Size(313, 69);
		((Control)this).Controls.Add((Control)(object)TXTPas);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)TableLayoutPanel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Control)this).Name = "frmManagementPass";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "frmManagementPass";
		((Form)this).FormClosed += new FormClosedEventHandler(frmManagementPass_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmManagementPass_KeyDown);
		((Control)TableLayoutPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
