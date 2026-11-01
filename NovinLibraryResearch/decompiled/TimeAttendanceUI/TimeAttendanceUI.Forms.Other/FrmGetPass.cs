using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using FormCustom.Cache;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendanceUI.Forms.Other;

public class FrmGetPass : BaseForm
{
	public enum GetPassEnum : short
	{
		GetPassDefault,
		GetPassDevice
	}

	private Dictionary<string, string> trDictionary = new Dictionary<string, string>();

	private GetPassEnum _getPass;

	private IContainer components;

	internal Label Label2;

	internal TextBox TXTPas;

	internal Label Label1;

	internal TableLayoutPanel TableLayoutPanel1;

	internal Button OK_Button;

	internal Button Cancel_Button;

	private async Task SetTrDictionaryAsync()
	{
		try
		{
			if (trDictionary == null)
			{
				trDictionary = new Dictionary<string, string>();
			}
			if (!trDictionary.TryGetValue("[رمز دستگاه]", out var value))
			{
				trDictionary.Add("[رمز دستگاه]", Utilities.GetTranslate("[رمز دستگاه]"));
			}
			if (!trDictionary.TryGetValue("[فرم رمز مدیریتی]", out value))
			{
				trDictionary.Add("[فرم رمز مدیریتی]", Utilities.GetTranslate("[فرم رمز مدیریتی]"));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTrDictionaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmGetPass.cs", 26);
		}
	}

	public FrmGetPass(string contextGuid, GetPassEnum getPass = GetPassEnum.GetPassDefault)
		: base(contextGuid, (BaseFormConfig)null)
	{
		Task.Run(async delegate
		{
			await SetTrDictionaryAsync();
		});
		_getPass = getPass;
		InitializeComponent();
		if (getPass == GetPassEnum.GetPassDevice)
		{
			((Control)Label2).Visible = false;
			((Control)Label1).Text = "رمز دستگاه :";
			if (trDictionary.TryGetValue("[رمز دستگاه]", out var value))
			{
				((Control)Label1).Text = value;
			}
			if (trDictionary.TryGetValue("[فرم رمز مدیریتی]", out var value2))
			{
				((Control)this).Text = value2;
			}
		}
	}

	private void OK_Button_Click(object sender, EventArgs e)
	{
		try
		{
			if (_getPass == GetPassEnum.GetPassDefault)
			{
				if (((Control)TXTPas).Text != ClsSystem.GetClsSystem(((CacheBaseForm)this).ContextGuid).GlobalPass)
				{
					((Control)TXTPas).Focus();
					((Control)TXTPas).Text = "";
					Utility.MessageBoxShowError("رمز وارد شده صحیح نیست", ((CacheBaseForm)this).ContextGuid);
				}
				else if (!(((Control)TXTPas).Text != ClsSystem.GetClsSystem(((CacheBaseForm)this).ContextGuid).GlobalPass))
				{
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OK_Button_Click", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmGetPass.cs", 75);
		}
	}

	private void Cancel_Button_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private void FRMGetPass_KeyDown(object sender, KeyEventArgs e)
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
			if ((int)keyCode != 27)
			{
				if ((int)keyCode == 116)
				{
					e.Handled = true;
					OK_Button.PerformClick();
				}
			}
			else
			{
				e.Handled = true;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FRMGetPass_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmGetPass.cs", 107);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TXTPas_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmGetPass.cs", 121);
		}
	}

	private void FrmGetPass_Shown(object sender, EventArgs e)
	{
		try
		{
			((Control)TXTPas).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FrmGetPass_Shown", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Other\\FrmGetPass.cs", 134);
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
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Expected O, but got Unknown
		Label2 = new Label();
		TXTPas = new TextBox();
		Label1 = new Label();
		TableLayoutPanel1 = new TableLayoutPanel();
		OK_Button = new Button();
		Cancel_Button = new Button();
		((Control)this).SuspendLayout();
		((Control)Label2).ForeColor = Color.White;
		((Control)Label2).Location = new Point(14, 35);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(252, 23);
		((Control)Label2).TabIndex = 2;
		((Control)Label2).Text = "[پیش فرض : 05137597590]";
		Label2.TextAlign = (ContentAlignment)32;
		((Control)TXTPas).Location = new Point(14, 9);
		((Control)TXTPas).Name = "TXTPas";
		TXTPas.PasswordChar = '*';
		((Control)TXTPas).Size = new Size(155, 21);
		((Control)TXTPas).TabIndex = 1;
		((Control)TXTPas).KeyDown += new KeyEventHandler(TXTPas_KeyDown);
		((Control)Label1).AutoSize = true;
		((Control)Label1).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label1).ForeColor = Color.Red;
		((Control)Label1).Location = new Point(175, 12);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(115, 13);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "[رمز مدیریت] :";
		Label1.TextAlign = (ContentAlignment)64;
		((Control)TableLayoutPanel1).Anchor = (AnchorStyles)6;
		TableLayoutPanel1.ColumnCount = 2;
		TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 50f));
		TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 50f));
		((Control)TableLayoutPanel1).Location = new Point(14, 2);
		((Control)TableLayoutPanel1).Name = "TableLayoutPanel1";
		TableLayoutPanel1.RowCount = 1;
		TableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 50f));
		((Control)TableLayoutPanel1).Size = new Size(155, 22);
		((Control)TableLayoutPanel1).TabIndex = 3;
		((Control)OK_Button).Anchor = (AnchorStyles)14;
		((Control)OK_Button).ForeColor = Color.White;
		((Control)OK_Button).Location = new Point(20, 65);
		((Control)OK_Button).Name = "OK_Button";
		((Control)OK_Button).Size = new Size(107, 23);
		((Control)OK_Button).TabIndex = 0;
		((Control)OK_Button).Text = "[تائید]";
		((Control)OK_Button).Click += OK_Button_Click;
		((Control)Cancel_Button).Anchor = (AnchorStyles)14;
		Cancel_Button.DialogResult = (DialogResult)2;
		((Control)Cancel_Button).ForeColor = Color.White;
		((Control)Cancel_Button).Location = new Point(149, 65);
		((Control)Cancel_Button).Name = "Cancel_Button";
		((Control)Cancel_Button).Size = new Size(107, 23);
		((Control)Cancel_Button).TabIndex = 0;
		((Control)Cancel_Button).Text = "[لغو]";
		((Control)Cancel_Button).Click += Cancel_Button_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		((Form)this).ClientSize = new Size(278, 97);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)OK_Button);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)Cancel_Button);
		((Control)this).Controls.Add((Control)(object)TXTPas);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Controls.Add((Control)(object)TableLayoutPanel1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)2;
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(298, 140);
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(298, 110);
		((Control)this).Name = "FrmGetPass";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فرم رمز مدیریتی]";
		((Form)this).Shown += FrmGetPass_Shown;
		((Control)this).KeyDown += new KeyEventHandler(FRMGetPass_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
