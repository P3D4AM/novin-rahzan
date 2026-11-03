using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.ManagementPassword;

public class FRMSetPass : AccountingBaseForm
{
	private bool _isDisposed;

	private Container components;

	internal TableLayoutPanel TableLayoutPanel1;

	internal Button Cancel_Button;

	internal Label Label1;

	internal TextBox TXTPas;

	internal TextBox New1;

	internal Label Label2;

	internal TextBox New2;

	internal Label Label3;

	internal Label Label4;

	internal Button OK_Button;

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

	public FRMSetPass(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMSetPass.cs", 20);
		}
	}

	private void OK_Button_Click(object sender, EventArgs e)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((Control)TXTPas).Text != CLSSystem.GlobalPass)
			{
				((Control)TXTPas).Focus();
				MessageBox.Show("رمز وارد شده صحیح نیست".GetTranslate(), "خطا".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			}
			else if (((Control)New1).Text.Length < 3)
			{
				((Control)New1).Focus();
				MessageBox.Show("کلمه عبور باید حداقل 3 حرف باشد".GetTranslate(), "خطا".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			}
			else if (((Control)New1).Text != ((Control)New2).Text)
			{
				((Control)New1).Focus();
				MessageBox.Show("کلمه عبور وارد شده مطابقت ندارد".GetTranslate(), "خطا".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			}
			else
			{
				CLSSystem.GlobalPass = ((Control)New1).Text;
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OK_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMSetPass.cs", 52);
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
			WebErrorLog.Instance.StartLog(ex, "", "Cancel_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMSetPass.cs", 62);
		}
	}

	private void TXTPas_Leave(dynamic sender, EventArgs e)
	{
		sender.BackColor = Color.White;
	}

	private void TXTPas_GotFocus(dynamic sender, EventArgs e)
	{
		try
		{
			sender.BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTPas_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMSetPass.cs", 75);
		}
	}

	private void FRMSetPass_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode == 27)
				{
					Cancel_Button.PerformClick();
				}
			}
			else
			{
				SendKeys.Send("{Tab}");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMSetPass_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMSetPass.cs", 92);
		}
	}

	private void FRMSetPass_FormClosed(object sender, FormClosedEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\FRMSetPass.cs", 113);
		}
	}

	[DebuggerStepThrough]
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		TableLayoutPanel1 = new TableLayoutPanel();
		OK_Button = new Button();
		Cancel_Button = new Button();
		Label1 = new Label();
		TXTPas = new TextBox();
		New1 = new TextBox();
		Label2 = new Label();
		New2 = new TextBox();
		Label3 = new Label();
		Label4 = new Label();
		((Control)TableLayoutPanel1).SuspendLayout();
		((Control)this).SuspendLayout();
		TableLayoutPanel1.ColumnCount = 2;
		TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 50f));
		TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 50f));
		TableLayoutPanel1.Controls.Add((Control)(object)OK_Button, 0, 0);
		TableLayoutPanel1.Controls.Add((Control)(object)Cancel_Button, 1, 0);
		((Control)TableLayoutPanel1).Location = new Point(8, 114);
		((Control)TableLayoutPanel1).Name = "TableLayoutPanel1";
		TableLayoutPanel1.RowCount = 1;
		TableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 50f));
		((Control)TableLayoutPanel1).Size = new Size(177, 29);
		((Control)TableLayoutPanel1).TabIndex = 7;
		((Control)OK_Button).Dock = (DockStyle)5;
		((Control)OK_Button).ForeColor = Color.White;
		((Control)OK_Button).Location = new Point(92, 3);
		((Control)OK_Button).Name = "OK_Button";
		((Control)OK_Button).Size = new Size(82, 23);
		((Control)OK_Button).TabIndex = 0;
		((Control)OK_Button).Text = "[تایید]";
		((Control)OK_Button).Click += OK_Button_Click;
		Cancel_Button.DialogResult = (DialogResult)2;
		((Control)Cancel_Button).Dock = (DockStyle)5;
		((Control)Cancel_Button).ForeColor = Color.White;
		((Control)Cancel_Button).Location = new Point(3, 3);
		((Control)Cancel_Button).Name = "Cancel_Button";
		((Control)Cancel_Button).Size = new Size(83, 23);
		((Control)Cancel_Button).TabIndex = 1;
		((Control)Cancel_Button).Text = "[انصراف]";
		((Control)Cancel_Button).Click += Cancel_Button_Click;
		((Control)Label1).ForeColor = Color.Red;
		((Control)Label1).Location = new Point(186, 12);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(100, 23);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "[رمز فعلی] :";
		Label1.TextAlign = (ContentAlignment)64;
		((Control)TXTPas).Location = new Point(12, 12);
		((Control)TXTPas).Name = "TXTPas";
		((Control)TXTPas).Size = new Size(168, 20);
		((Control)TXTPas).TabIndex = 1;
		TXTPas.UseSystemPasswordChar = true;
		((Control)TXTPas).GotFocus += TXTPas_GotFocus;
		((Control)TXTPas).LostFocus += TXTPas_Leave;
		((Control)New1).Location = new Point(12, 56);
		((Control)New1).Name = "New1";
		((Control)New1).Size = new Size(168, 20);
		((Control)New1).TabIndex = 4;
		New1.UseSystemPasswordChar = true;
		((Control)New1).GotFocus += TXTPas_GotFocus;
		((Control)New1).LostFocus += TXTPas_Leave;
		((Control)Label2).ForeColor = Color.Red;
		((Control)Label2).Location = new Point(185, 56);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(100, 23);
		((Control)Label2).TabIndex = 3;
		((Control)Label2).Text = "[رمز جدید] :";
		Label2.TextAlign = (ContentAlignment)64;
		((Control)New2).Location = new Point(12, 84);
		((Control)New2).Name = "New2";
		((Control)New2).Size = new Size(168, 20);
		((Control)New2).TabIndex = 6;
		New2.UseSystemPasswordChar = true;
		((Control)New2).GotFocus += TXTPas_GotFocus;
		((Control)New2).LostFocus += TXTPas_Leave;
		((Control)Label3).ForeColor = Color.Red;
		((Control)Label3).Location = new Point(186, 82);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(100, 23);
		((Control)Label3).TabIndex = 5;
		((Control)Label3).Text = "[تکرار رمز] :";
		Label3.TextAlign = (ContentAlignment)64;
		((Control)Label4).ForeColor = Color.Red;
		((Control)Label4).Location = new Point(12, 33);
		((Control)Label4).Name = "Label4";
		((Control)Label4).Size = new Size(250, 23);
		((Control)Label4).TabIndex = 2;
		((Control)Label4).Text = "__________________________________________";
		Label4.TextAlign = (ContentAlignment)64;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		((Form)this).CancelButton = (IButtonControl)(object)Cancel_Button;
		((Form)this).ClientSize = new Size(274, 155);
		((Control)this).Controls.Add((Control)(object)New2);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)New1);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)TXTPas);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)TableLayoutPanel1);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMSetPass";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMGetPass";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMSetPass_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(FRMSetPass_KeyDown);
		((Control)TableLayoutPanel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
