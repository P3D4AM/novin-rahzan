using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Cache;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Issabel;

namespace FormCustom.Activation;

public class FrmPublicSetting : CacheBaseForm
{
	private ActivationSettings _programSetting;

	private IContainer components;

	private Label label1;

	private TextBox txtLocaConnectionString;

	private Label label3;

	private TextBox txtAddressApi;

	private Label label4;

	private TextBox txtAddressSignalR;

	private ButtonX BtnSave;

	private Label label2;

	private TextBox txtApiUserName;

	private TextBox txtApiPassword;

	private Label label5;

	private TextBox txtSignalRPassword;

	private Label label6;

	private TextBox txtSignalRUserName;

	private Label label7;

	private NumericUpDown nudPort;

	internal TextBox txtIP;

	private Label label8;

	private Label label9;

	private CheckBox chbIsServiseIssabel;

	public FrmPublicSetting(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
	}

	private void FrmPublicSetting_Load(object sender, EventArgs e)
	{
		FillControl();
	}

	private async void BtnSave_Click(object sender, EventArgs e)
	{
		try
		{
			if ((int)MessageBox.Show((IWin32Window)(object)this, "با انجام تغییرات ممکن است در روند کار برنامه مشکلی پیش آید آیا از صحت تغییرات اطمینان دارید؟", "هشدار", (MessageBoxButtons)4, (MessageBoxIcon)48) != 7 && ChekValidation())
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = Save();
				if (!returnedSaveFuncInfo.HasError)
				{
					Application.Restart();
					return;
				}
				await this.ShowReturnedSavedFuncInfoAsync(returnedSaveFuncInfo, "پیغام سیستم", "BtnSave_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\FrmPublicSetting.cs");
				FillControl();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BtnSave_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\FrmPublicSetting.cs", 35);
		}
	}

	private void FillControl()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			_programSetting = ActivationSettings.Get(base.ContextGuid, displayUiIfSettingIsNull: false) ?? new ActivationSettings();
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(_programSetting.SqlServerDatabaseConnectionString);
			((Control)txtLocaConnectionString).Text = ((DbConnectionStringBuilder)(object)val).ConnectionString;
			((Control)txtAddressApi).Text = _programSetting.ApiHostAddress;
			((Control)txtAddressSignalR).Text = _programSetting.SignalrHostAddress;
			((Control)txtApiUserName).Text = _programSetting.ApiUser;
			((Control)txtApiPassword).Text = _programSetting.ApiPass;
			((Control)txtSignalRUserName).Text = _programSetting.SignalrUser;
			((Control)txtSignalRPassword).Text = _programSetting.SignalrPass;
			chbIsServiseIssabel.Checked = _programSetting.IsabelleClientSettings?.IsabelleSettingEnable ?? false;
			((Control)txtIP).Text = _programSetting.IsabelleClientSettings?.Ip;
			nudPort.Value = _programSetting.IsabelleClientSettings?.Port ?? 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\FrmPublicSetting.cs", 54);
		}
	}

	private bool ChekValidation()
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		string text = "";
		try
		{
			if (string.IsNullOrWhiteSpace(((Control)txtAddressApi).Text) || ((TextBoxBase)txtAddressApi).TextLength < 4)
			{
				text = "آدرس اتصال سرور Api صحیح نیست";
				flag = false;
				((Control)txtAddressApi).Focus();
			}
			if (string.IsNullOrWhiteSpace(((Control)txtLocaConnectionString).Text) || ((TextBoxBase)txtLocaConnectionString).TextLength < 4)
			{
				text = "آدرس دیتا بیس صحیح نیست";
				flag = false;
				((Control)txtLocaConnectionString).Focus();
			}
			if (string.IsNullOrWhiteSpace(((Control)txtLocaConnectionString).Text) || ((TextBoxBase)txtLocaConnectionString).TextLength < 15)
			{
				text = "نام دیتابیس صحیح نیست";
				flag = false;
				((Control)txtLocaConnectionString).Focus();
			}
			if (string.IsNullOrWhiteSpace(((Control)txtAddressSignalR).Text) || ((TextBoxBase)txtAddressSignalR).TextLength < 4)
			{
				text = "ادرس ارتباط با Signalr صحیح نیست";
				flag = false;
				((Control)txtAddressSignalR).Focus();
			}
			if (!flag)
			{
				MessageBox.Show((IWin32Window)(object)this, text, "خطا در ورود اطلاعات", (MessageBoxButtons)0, (MessageBoxIcon)16);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChekValidation", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\FrmPublicSetting.cs", 90);
			flag = false;
		}
		return flag;
	}

	private ReturnedSaveFuncInfo Save()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(((Control)txtLocaConnectionString).Text);
			_programSetting.SqlServerDatabaseConnectionString = ((DbConnectionStringBuilder)(object)val).ConnectionString;
			_programSetting.SignalrHostAddress = ((Control)txtAddressSignalR).Text.Trim().Replace("//HubCore", "/HubCore");
			_programSetting.ApiHostAddress = ((Control)txtAddressApi).Text.Trim();
			_programSetting.ApiUser = ((Control)txtApiUserName).Text.Trim();
			_programSetting.ApiPass = ((Control)txtApiPassword).Text.Trim();
			_programSetting.SignalrUser = ((Control)txtSignalRUserName).Text.Trim();
			_programSetting.SignalrPass = ((Control)txtSignalRPassword).Text.Trim();
			ActivationSettings programSetting = _programSetting;
			if (programSetting.IsabelleClientSettings == null)
			{
				IsabelleClientSettings isabelleClientSettings2 = (programSetting.IsabelleClientSettings = new IsabelleClientSettings());
			}
			_programSetting.IsabelleClientSettings.Ip = ((Control)txtIP).Text;
			_programSetting.IsabelleClientSettings.Port = (int)nudPort.Value;
			_programSetting.IsabelleClientSettings.IsabelleSettingEnable = chbIsServiseIssabel.Checked;
			returnedSaveFuncInfo.AddReturnedValue(_programSetting.CreateSetting(), ReturnedState.None, "Save", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\FrmPublicSetting.cs", 110);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\FrmPublicSetting.cs", 114);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\FrmPublicSetting.cs", 115);
		}
		return returnedSaveFuncInfo;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
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
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Expected O, but got Unknown
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Expected O, but got Unknown
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Expected O, but got Unknown
		//IL_0bb1: Unknown result type (might be due to invalid IL or missing references)
		txtSignalRPassword = new TextBox();
		label6 = new Label();
		txtSignalRUserName = new TextBox();
		label7 = new Label();
		txtApiPassword = new TextBox();
		label5 = new Label();
		txtApiUserName = new TextBox();
		label2 = new Label();
		BtnSave = new ButtonX();
		txtAddressSignalR = new TextBox();
		label4 = new Label();
		txtAddressApi = new TextBox();
		label3 = new Label();
		txtLocaConnectionString = new TextBox();
		label1 = new Label();
		nudPort = new NumericUpDown();
		txtIP = new TextBox();
		label8 = new Label();
		label9 = new Label();
		chbIsServiseIssabel = new CheckBox();
		((ISupportInitialize)nudPort).BeginInit();
		((Control)this).SuspendLayout();
		((Control)txtSignalRPassword).Location = new Point(24, 297);
		((Control)txtSignalRPassword).Name = "txtSignalRPassword";
		txtSignalRPassword.PasswordChar = '*';
		((Control)txtSignalRPassword).Size = new Size(387, 26);
		((Control)txtSignalRPassword).TabIndex = 16;
		((Control)label6).AutoSize = true;
		((Control)label6).Location = new Point(432, 303);
		((Control)label6).Margin = new Padding(4, 0, 4, 0);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(174, 20);
		((Control)label6).TabIndex = 15;
		((Control)label6).Text = ": SignalRرمز عبور ارتباط با";
		((Control)txtSignalRUserName).Location = new Point(24, 250);
		((Control)txtSignalRUserName).Name = "txtSignalRUserName";
		((Control)txtSignalRUserName).Size = new Size(386, 26);
		((Control)txtSignalRUserName).TabIndex = 14;
		((Control)label7).AutoSize = true;
		((Control)label7).Location = new Point(419, 253);
		((Control)label7).Margin = new Padding(4, 0, 4, 0);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(187, 20);
		((Control)label7).TabIndex = 13;
		((Control)label7).Text = ": SignalR نام کاربری ارتباط با ";
		((Control)txtApiPassword).Location = new Point(18, 153);
		((Control)txtApiPassword).Name = "txtApiPassword";
		txtApiPassword.PasswordChar = '*';
		((Control)txtApiPassword).Size = new Size(393, 26);
		((Control)txtApiPassword).TabIndex = 12;
		((Control)label5).AutoSize = true;
		((Control)label5).Location = new Point(461, 159);
		((Control)label5).Margin = new Padding(4, 0, 4, 0);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(145, 20);
		((Control)label5).TabIndex = 11;
		((Control)label5).Text = ": Api رمز عبور ارتباط با";
		((Control)txtApiUserName).Location = new Point(18, 106);
		((Control)txtApiUserName).Name = "txtApiUserName";
		((Control)txtApiUserName).Size = new Size(393, 26);
		((Control)txtApiUserName).TabIndex = 10;
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(457, 106);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(154, 20);
		((Control)label2).TabIndex = 9;
		((Control)label2).Text = ": Api نام کاربری ارتباط با ";
		((Control)BtnSave).AccessibleRole = (AccessibleRole)43;
		BtnSave.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)BtnSave).Dock = (DockStyle)2;
		((Control)BtnSave).Location = new Point(0, 392);
		((Control)BtnSave).Name = "BtnSave";
		((Control)BtnSave).Size = new Size(623, 34);
		BtnSave.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BtnSave).TabIndex = 8;
		((Control)BtnSave).Text = "Save";
		((Control)BtnSave).Click += BtnSave_Click;
		((Control)txtAddressSignalR).Location = new Point(21, 205);
		((Control)txtAddressSignalR).Name = "txtAddressSignalR";
		((Control)txtAddressSignalR).Size = new Size(389, 26);
		((Control)txtAddressSignalR).TabIndex = 7;
		((Control)label4).AutoSize = true;
		((Control)label4).Location = new Point(451, 208);
		((Control)label4).Margin = new Padding(4, 0, 4, 0);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(155, 20);
		((Control)label4).TabIndex = 6;
		((Control)label4).Text = ":SignalR آدرس ارتباطات";
		((Control)txtAddressApi).Location = new Point(17, 56);
		((Control)txtAddressApi).Name = "txtAddressApi";
		((Control)txtAddressApi).Size = new Size(393, 26);
		((Control)txtAddressApi).TabIndex = 5;
		((Control)label3).AutoSize = true;
		((Control)label3).Location = new Point(492, 59);
		((Control)label3).Margin = new Padding(4, 0, 4, 0);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(114, 20);
		((Control)label3).TabIndex = 4;
		((Control)label3).Text = ": Api آدرس سرور ";
		((Control)txtLocaConnectionString).Location = new Point(17, 12);
		((Control)txtLocaConnectionString).Name = "txtLocaConnectionString";
		txtLocaConnectionString.PasswordChar = '*';
		((Control)txtLocaConnectionString).Size = new Size(393, 26);
		((Control)txtLocaConnectionString).TabIndex = 1;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(468, 15);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(138, 20);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = " :Local کانکشن دیتابیس";
		((Control)nudPort).Anchor = (AnchorStyles)9;
		((Control)nudPort).Cursor = Cursors.Default;
		((Control)nudPort).Font = new Font("B Yekan", 9.75f);
		((Control)nudPort).Location = new Point(256, 349);
		((Control)nudPort).Margin = new Padding(4, 3, 4, 3);
		nudPort.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		((Control)nudPort).Name = "nudPort";
		((Control)nudPort).Size = new Size(69, 27);
		((Control)nudPort).TabIndex = 1039;
		nudPort.Value = new decimal(new int[4] { 1000, 0, 0, 0 });
		((Control)txtIP).Anchor = (AnchorStyles)9;
		((Control)txtIP).Font = new Font("B Yekan", 9.75f);
		((Control)txtIP).Location = new Point(28, 349);
		((Control)txtIP).Margin = new Padding(4, 3, 4, 3);
		((Control)txtIP).Name = "txtIP";
		((Control)txtIP).Size = new Size(156, 27);
		((Control)txtIP).TabIndex = 1038;
		txtIP.TextAlign = (HorizontalAlignment)1;
		((Control)label8).AutoSize = true;
		((Control)label8).Location = new Point(182, 352);
		((Control)label8).Margin = new Padding(4, 0, 4, 0);
		((Control)label8).Name = "label8";
		((Control)label8).Size = new Size(66, 20);
		((Control)label8).TabIndex = 1042;
		((Control)label8).Text = ": IPاتصال";
		((Control)label9).AutoSize = true;
		((Control)label9).Location = new Point(333, 352);
		((Control)label9).Margin = new Padding(4, 0, 4, 0);
		((Control)label9).Name = "label9";
		((Control)label9).Size = new Size(78, 20);
		((Control)label9).TabIndex = 1043;
		((Control)label9).Text = ": Portسرور";
		((Control)chbIsServiseIssabel).Anchor = (AnchorStyles)9;
		((Control)chbIsServiseIssabel).AutoSize = true;
		((Control)chbIsServiseIssabel).Location = new Point(418, 348);
		((Control)chbIsServiseIssabel).Name = "chbIsServiseIssabel";
		((Control)chbIsServiseIssabel).RightToLeft = (RightToLeft)1;
		((Control)chbIsServiseIssabel).Size = new Size(188, 24);
		((Control)chbIsServiseIssabel).TabIndex = 17;
		((Control)chbIsServiseIssabel).Text = "ارتباط با سرویس برقرار باشد ";
		((ButtonBase)chbIsServiseIssabel).UseVisualStyleBackColor = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(9f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(623, 426);
		((Control)this).Controls.Add((Control)(object)label9);
		((Control)this).Controls.Add((Control)(object)label8);
		((Control)this).Controls.Add((Control)(object)nudPort);
		((Control)this).Controls.Add((Control)(object)txtIP);
		((Control)this).Controls.Add((Control)(object)chbIsServiseIssabel);
		((Control)this).Controls.Add((Control)(object)txtSignalRPassword);
		((Control)this).Controls.Add((Control)(object)label6);
		((Control)this).Controls.Add((Control)(object)txtSignalRUserName);
		((Control)this).Controls.Add((Control)(object)label7);
		((Control)this).Controls.Add((Control)(object)txtApiPassword);
		((Control)this).Controls.Add((Control)(object)label5);
		((Control)this).Controls.Add((Control)(object)txtApiUserName);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)BtnSave);
		((Control)this).Controls.Add((Control)(object)txtAddressSignalR);
		((Control)this).Controls.Add((Control)(object)label4);
		((Control)this).Controls.Add((Control)(object)txtAddressApi);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)txtLocaConnectionString);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmPublicSetting";
		((Form)this).RightToLeftLayout = true;
		((Control)this).Text = "تنظیمات اصلی دپارتمان";
		((Form)this).Load += FrmPublicSetting_Load;
		((ISupportInitialize)nudPort).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
