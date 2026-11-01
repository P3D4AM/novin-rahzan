using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser.Entities;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendanceUI.Forms.Other;

public class FRMSetPass : BaseForm
{
	private IContainer components;

	internal TextBox New2;

	internal Label Label3;

	internal TextBox New1;

	internal Label Label2;

	internal TextBox TXTPas;

	internal Label Label1;

	internal Label Label4;

	internal Button OK_Button;

	internal Button Cancel_Button;

	public FRMSetPass(string contextGuid)
		: base(contextGuid, (BaseFormConfig)null)
	{
		InitializeComponent();
	}

	private void OK_Button_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)TXTPas).Text != ClsSystem.GetClsSystem(((CacheBaseForm)this).ContextGuid).GlobalPass)
			{
				((Control)TXTPas).Focus();
				((Control)TXTPas).Text = "";
				Utility.MessageBoxShowError("رمز وارد شده صحیح نیست", ((CacheBaseForm)this).ContextGuid);
			}
			else if (((Control)New1).Text.Length < 3)
			{
				((Control)New1).Focus();
				Utility.MessageBoxShowError("کلمه عبور باید حداقل 3 حرف باشد", ((CacheBaseForm)this).ContextGuid);
			}
			else if (((Control)New1).Text != ((Control)New2).Text)
			{
				((Control)New1).Focus();
				((Control)New1).Text = "";
				((Control)New2).Text = "";
				Utility.MessageBoxShowError("کلمه عبور وارد شده مطابقت ندارد", ((CacheBaseForm)this).ContextGuid);
			}
			else
			{
				ClsSystem.GetClsSystem(((CacheBaseForm)this).ContextGuid).GlobalPass = ((Control)New1).Text;
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OK_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMSetPass.cs", 48);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Cancel_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMSetPass.cs", 61);
		}
	}

	private void TXTPas_Enter(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((TextBoxBase)(TextBox)sender).SelectAll();
			((Control)(TextBox)sender).BackColor = Color.Yellow;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTPas_Enter", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMSetPass.cs", 74);
		}
	}

	private void TXTPas_Leave(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)(TextBox)sender).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTPas_Leave", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMSetPass.cs", 86);
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
					e.Handled = true;
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			else
			{
				e.Handled = true;
				((Control)this).SelectNextControl(((ContainerControl)this).ActiveControl, true, true, true, true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMSetPass_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FRMSetPass.cs", 109);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((BaseForm)this).Dispose(disposing);
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
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		New2 = new TextBox();
		Label3 = new Label();
		New1 = new TextBox();
		Label2 = new Label();
		TXTPas = new TextBox();
		Label1 = new Label();
		Label4 = new Label();
		OK_Button = new Button();
		Cancel_Button = new Button();
		((Control)this).SuspendLayout();
		((Control)New2).Location = new Point(14, 90);
		((Control)New2).Name = "New2";
		New2.PasswordChar = '*';
		((Control)New2).Size = new Size(195, 22);
		((Control)New2).TabIndex = 5;
		((Control)New2).Enter += TXTPas_Enter;
		((Control)New2).Leave += TXTPas_Leave;
		((Control)Label3).ForeColor = Color.Red;
		((Control)Label3).Location = new Point(217, 88);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(117, 25);
		((Control)Label3).TabIndex = 4;
		((Control)Label3).Text = "[تکرار رمز] :";
		Label3.TextAlign = (ContentAlignment)64;
		((Control)New1).Location = new Point(14, 60);
		((Control)New1).Name = "New1";
		New1.PasswordChar = '*';
		((Control)New1).Size = new Size(195, 22);
		((Control)New1).TabIndex = 3;
		((Control)New1).Enter += TXTPas_Enter;
		((Control)New1).Leave += TXTPas_Leave;
		((Control)Label2).ForeColor = Color.Red;
		((Control)Label2).Location = new Point(216, 60);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(117, 25);
		((Control)Label2).TabIndex = 2;
		((Control)Label2).Text = "[رمز جدید] :";
		Label2.TextAlign = (ContentAlignment)64;
		((Control)TXTPas).Location = new Point(14, 13);
		((Control)TXTPas).Name = "TXTPas";
		TXTPas.PasswordChar = '*';
		((Control)TXTPas).Size = new Size(195, 22);
		((Control)TXTPas).TabIndex = 1;
		((Control)TXTPas).Enter += TXTPas_Enter;
		((Control)TXTPas).Leave += TXTPas_Leave;
		((Control)Label1).ForeColor = Color.Red;
		((Control)Label1).Location = new Point(217, 13);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(117, 25);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "[رمز فعلی] :";
		Label1.TextAlign = (ContentAlignment)64;
		((Control)Label4).ForeColor = Color.Red;
		((Control)Label4).Location = new Point(14, 36);
		((Control)Label4).Name = "Label4";
		((Control)Label4).Size = new Size(292, 25);
		((Control)Label4).TabIndex = 8;
		((Control)Label4).Text = "__________________________________________";
		Label4.TextAlign = (ContentAlignment)64;
		((Control)OK_Button).Anchor = (AnchorStyles)6;
		((Control)OK_Button).ForeColor = Color.White;
		((Control)OK_Button).Location = new Point(101, 131);
		((Control)OK_Button).Name = "OK_Button";
		((Control)OK_Button).Size = new Size(83, 23);
		((Control)OK_Button).TabIndex = 6;
		((Control)OK_Button).Text = "[تائید]";
		((Control)OK_Button).Click += OK_Button_Click;
		((Control)Cancel_Button).Anchor = (AnchorStyles)6;
		Cancel_Button.DialogResult = (DialogResult)2;
		((Control)Cancel_Button).ForeColor = Color.White;
		((Control)Cancel_Button).Location = new Point(12, 131);
		((Control)Cancel_Button).Name = "Cancel_Button";
		((Control)Cancel_Button).Size = new Size(83, 23);
		((Control)Cancel_Button).TabIndex = 7;
		((Control)Cancel_Button).TabStop = false;
		((Control)Cancel_Button).Text = "[لغو]";
		((Control)Cancel_Button).Click += Cancel_Button_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 14f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		((Form)this).ClientSize = new Size(306, 166);
		((Control)this).Controls.Add((Control)(object)Cancel_Button);
		((Control)this).Controls.Add((Control)(object)OK_Button);
		((Control)this).Controls.Add((Control)(object)New2);
		((Control)this).Controls.Add((Control)(object)Label3);
		((Control)this).Controls.Add((Control)(object)New1);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)TXTPas);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)Label4);
		((Control)this).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Control)this).MaximumSize = new Size(306, 166);
		((Control)this).MinimumSize = new Size(306, 166);
		((Control)this).Name = "FRMSetPass";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowInTaskbar = true;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FRMSetPass";
		((Control)this).KeyDown += new KeyEventHandler(FRMSetPass_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
