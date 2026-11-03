using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FormCustom.Properties;
using LK;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Froms.LkManage;

public class FrmLkClientSetting : LkUiFactory
{
	private bool _isDisposed;

	private Package _package;

	private static short _wasClose;

	private Container components;

	internal CheckBox ChBox_enabled;

	internal Button BTN_OK;

	internal Button BTN_Cancel;

	internal TextBox Txt_Port;

	internal TextBox Txt_IP;

	internal Label Label2;

	internal Label Label3;

	internal Panel Panel1;

	internal CheckBox CheckBox1;

	internal Label Label1;

	public bool IsNotValid { get; private set; }

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

	public FrmLkClientSetting(string contextGuid, Package package, string message = "", bool isNotValid = false)
		: base(contextGuid)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InitializeComponent();
			if (_wasClose >= 1)
			{
				Application.ExitThread();
			}
			_package = package;
			IsNotValid = isNotValid;
			if (!string.IsNullOrEmpty(message))
			{
				((Control)Label1).Text = message;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 31);
			MessageBox.Show((IWin32Window)(object)this, ex.Message);
		}
	}

	private static string GetIpFileName(Package package)
	{
		try
		{
			string text = package.ToString();
			switch (text)
			{
			case "BarCode":
			case "Tolid":
			case "Special":
				text = "Accounting";
				break;
			}
			return Path.Combine(Application.StartupPath, "IpServer" + text + ".txt");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetIpFileName", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 45);
		}
		return "";
	}

	public async void BTN_OK_Click(object sender, EventArgs e)
	{
		int line = 0;
		try
		{
			if (!((Control)BTN_OK).Enabled || !((Control)BTN_OK).Visible)
			{
				return;
			}
			((Control)BTN_OK).Enabled = false;
			_wasClose++;
			if (!parsIP())
			{
				line = 1;
				MessageBox.Show((IWin32Window)(object)this, "آدرس سرور وارد شده صحیح نمی باشد", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				line = 2;
				Client.notvalid = true;
				IsNotValid = true;
				line = 3;
				((Form)this).DialogResult = (DialogResult)2;
				line = 4;
				return;
			}
			line = 5;
			WriteData(_package);
			line = 6;
			if (!(await Client.ConnectAsync(_package, ((Control)Txt_IP).Text, ((Control)Txt_Port).Text.ParseToShort(0))) && !Client.Showmessage && IsNotValid)
			{
				line = 7;
				string text = ErrorSelect(Client.errorText);
				line = 8;
				MessageBox.Show((IWin32Window)(object)(IsDisposed ? null : this), Convert.ToString("سیستم شما مجاز به استفاده از قفل سخت افزاری از طریق شبکه نمی باشد\r\n" + text), "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				line = 9;
				base.LkSerial = 0L;
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			else
			{
				base.LkSerial = Client.LkSerial;
				if (!string.IsNullOrEmpty(Client.errorText))
				{
					MessageBox.Show(Client.errorText);
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
				else
				{
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"IsDisposed:{IsDisposed}", "BTN_OK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 99);
		}
		finally
		{
			((Control)BTN_OK).Enabled = true;
		}
	}

	private string ErrorSelect(string dataFromServer)
	{
		try
		{
			return ((LkClientServerErrors)dataFromServer.Substring(dataFromServer.IndexOf("ERROR") + 5, dataFromServer.IndexOf("@") - (dataFromServer.IndexOf("ERROR") + 5)).ParseToInt()).GetDisplay().GetTranslate() + Environment.NewLine + dataFromServer;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ErrorSelect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 115);
			return " خطا در تشخیص کد خطای سرور".GetTranslate() + Environment.NewLine + ex.Message;
		}
	}

	private bool parsIP()
	{
		try
		{
			Regex regex = new Regex("((([2][5][0-5])|([2][0-4][0-9])|([1][0-9][0-9])|([0-9][0-9])|([0-9]))[.]){3}((([2][5][0-5])|([2][0-4][0-9])|([1][0-9][0-9])|([0-9][0-9])|([0-9])))");
			Match match = null;
			match = regex.Match(((Control)Txt_IP).Text);
			if (match.Success && match.Value.ToString().Length == ((Control)Txt_IP).Text.Length)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "parsIP", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 135);
			return false;
		}
	}

	private void WriteData(Package package)
	{
		try
		{
			if (!File.Exists(GetIpFileName(package)))
			{
				File.Create(GetIpFileName(package)).Close();
			}
			File.WriteAllText(GetIpFileName(package), ((Control)Txt_IP).Text + "@" + ((Control)Txt_Port).Text + "$");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 148);
		}
	}

	public void ChBox_enabled_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Panel1).Enabled = ChBox_enabled.Checked;
			((Control)BTN_OK).Enabled = ChBox_enabled.Checked;
			if (((Control)Panel1).Enabled)
			{
				LoadData(_package);
				if (((Control)Txt_Port).Text == "")
				{
					((Control)Txt_Port).Text = "9090";
				}
				Client.notvalid = false;
				IsNotValid = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChBox_enabled_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 166);
		}
	}

	private void LoadData(Package package)
	{
		try
		{
			string text = "";
			if (File.Exists(GetIpFileName(package)))
			{
				text = File.ReadAllText(GetIpFileName(package));
				if (!string.IsNullOrEmpty(text))
				{
					((Control)Txt_IP).Text = text.Substring(0, text.IndexOf("@"));
					((Control)Txt_Port).Text = text.Substring(text.IndexOf("@") + 1, text.IndexOf("$") - text.IndexOf("@") - 1);
					return;
				}
			}
			((Control)Txt_IP).Text = "";
			((Control)Txt_Port).Text = "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 189);
			((Control)Txt_IP).Text = "";
			((Control)Txt_Port).Text = "";
		}
	}

	public void BTN_Cancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	public void FRMClient_Load(object sender, EventArgs e)
	{
		ChBox_enabled.Checked = true;
	}

	public void FRMClient_KeyDown(object sender, KeyEventArgs e)
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
					BTN_Cancel.PerformClick();
				}
			}
			else
			{
				BTN_OK.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMClient_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 213);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FrmLkClientSetting.cs", 228);
		}
	}

	private void FRMClient_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
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
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Expected O, but got Unknown
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Expected O, but got Unknown
		//IL_07be: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Expected O, but got Unknown
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Expected O, but got Unknown
		Label1 = new Label();
		ChBox_enabled = new CheckBox();
		Txt_Port = new TextBox();
		Txt_IP = new TextBox();
		Label2 = new Label();
		Label3 = new Label();
		Panel1 = new Panel();
		CheckBox1 = new CheckBox();
		BTN_OK = new Button();
		BTN_Cancel = new Button();
		((Control)Panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)Label1).AutoSize = true;
		((Control)Label1).Font = new Font("B Titr", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label1).ForeColor = Color.Red;
		((Control)Label1).Location = new Point(98, 19);
		((Control)Label1).Name = "Label1";
		((Control)Label1).RightToLeft = (RightToLeft)1;
		((Control)Label1).Size = new Size(223, 33);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "[قفل سخت افزاری شناسایی نشد]";
		((Control)ChBox_enabled).AutoSize = true;
		((Control)ChBox_enabled).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ChBox_enabled).Location = new Point(280, 71);
		((Control)ChBox_enabled).Name = "ChBox_enabled";
		((Control)ChBox_enabled).RightToLeft = (RightToLeft)1;
		((Control)ChBox_enabled).Size = new Size(162, 18);
		((Control)ChBox_enabled).TabIndex = 0;
		((Control)ChBox_enabled).Text = "[کلاینت شبکه فعال شود]";
		((ButtonBase)ChBox_enabled).UseVisualStyleBackColor = true;
		ChBox_enabled.CheckedChanged += ChBox_enabled_CheckedChanged;
		((Control)Txt_Port).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Txt_Port).Location = new Point(246, 58);
		((Control)Txt_Port).Name = "Txt_Port";
		((Control)Txt_Port).RightToLeft = (RightToLeft)0;
		((Control)Txt_Port).Size = new Size(66, 22);
		((Control)Txt_Port).TabIndex = 3;
		((Control)Txt_IP).AccessibleRole = (AccessibleRole)63;
		((Control)Txt_IP).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Txt_IP).Location = new Point(138, 23);
		((Control)Txt_IP).Name = "Txt_IP";
		((Control)Txt_IP).RightToLeft = (RightToLeft)0;
		((Control)Txt_IP).Size = new Size(174, 22);
		((Control)Txt_IP).TabIndex = 2;
		((Control)Label2).AutoSize = true;
		((Control)Label2).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label2).Location = new Point(335, 24);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(85, 14);
		((Control)Label2).TabIndex = 0;
		((Control)Label2).Text = "[آدرس سرور]: ";
		((Control)Label3).AutoSize = true;
		((Control)Label3).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)Label3).Location = new Point(331, 61);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(88, 14);
		((Control)Label3).TabIndex = 0;
		((Control)Label3).Text = "[شماره درگاه]: ";
		Panel1.BorderStyle = (BorderStyle)2;
		((Control)Panel1).Controls.Add((Control)(object)Txt_IP);
		((Control)Panel1).Controls.Add((Control)(object)Label3);
		((Control)Panel1).Controls.Add((Control)(object)Txt_Port);
		((Control)Panel1).Controls.Add((Control)(object)CheckBox1);
		((Control)Panel1).Controls.Add((Control)(object)Label2);
		((Control)Panel1).Enabled = false;
		((Control)Panel1).Location = new Point(14, 109);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).RightToLeft = (RightToLeft)1;
		((Control)Panel1).Size = new Size(428, 108);
		((Control)Panel1).TabIndex = 0;
		((Control)CheckBox1).AutoSize = true;
		((Control)CheckBox1).Location = new Point(306, -38);
		((Control)CheckBox1).Name = "CheckBox1";
		((Control)CheckBox1).RightToLeft = (RightToLeft)1;
		((Control)CheckBox1).Size = new Size(162, 18);
		((Control)CheckBox1).TabIndex = 1;
		((Control)CheckBox1).Text = "[کلاینت شبکه فعال شود]";
		((ButtonBase)CheckBox1).UseVisualStyleBackColor = true;
		CheckBox1.CheckedChanged += ChBox_enabled_CheckedChanged;
		((Control)BTN_OK).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ButtonBase)BTN_OK).Image = (Image)(object)FormCustom.Properties.Resources.accept;
		((ButtonBase)BTN_OK).ImageAlign = (ContentAlignment)16;
		((Control)BTN_OK).Location = new Point(126, 242);
		((Control)BTN_OK).Name = "BTN_OK";
		((Control)BTN_OK).Size = new Size(113, 43);
		((Control)BTN_OK).TabIndex = 4;
		((Control)BTN_OK).Text = "[تائید]";
		((ButtonBase)BTN_OK).TextAlign = (ContentAlignment)64;
		((ButtonBase)BTN_OK).UseVisualStyleBackColor = true;
		((Control)BTN_OK).Click += BTN_OK_Click;
		BTN_Cancel.DialogResult = (DialogResult)2;
		((Control)BTN_Cancel).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((ButtonBase)BTN_Cancel).Image = (Image)(object)FormCustom.Properties.Resources.stop;
		((ButtonBase)BTN_Cancel).ImageAlign = (ContentAlignment)16;
		((Control)BTN_Cancel).Location = new Point(7, 242);
		((Control)BTN_Cancel).Name = "BTN_Cancel";
		((Control)BTN_Cancel).Size = new Size(113, 43);
		((Control)BTN_Cancel).TabIndex = 5;
		((Control)BTN_Cancel).Text = "[انصراف]";
		((ButtonBase)BTN_Cancel).TextAlign = (ContentAlignment)64;
		((ButtonBase)BTN_Cancel).UseVisualStyleBackColor = true;
		((Control)BTN_Cancel).Click += BTN_Cancel_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)BTN_OK;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 14f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.WhiteSmoke;
		((Form)this).CancelButton = (IButtonControl)(object)BTN_Cancel;
		((Form)this).ClientSize = new Size(461, 289);
		((Control)this).Controls.Add((Control)(object)Panel1);
		((Control)this).Controls.Add((Control)(object)BTN_Cancel);
		((Control)this).Controls.Add((Control)(object)BTN_OK);
		((Control)this).Controls.Add((Control)(object)ChBox_enabled);
		((Control)this).Controls.Add((Control)(object)Label1);
		((Control)this).Font = new Font("Tahoma", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)2;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmLkClientSetting";
		((Form)this).RightToLeftLayout = true;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "`";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMClient_FormClosed);
		((Form)this).Load += FRMClient_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMClient_KeyDown);
		((Control)Panel1).ResumeLayout(false);
		((Control)Panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
