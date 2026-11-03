using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using BackUpDLL;
using FormCustom.FormControls;
using FormCustom.Properties;
using Microsoft.Win32;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms;

public class FrmSetConnectionString : AccountingBaseForm
{
	private bool _isDisposed;

	public new string ConnectionString = AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString;

	private ENSource _source;

	private IContainer components;

	internal Button BTN_NetIntial;

	public RadioButton rbtnNetwork;

	public RadioButton rbtnCreate;

	internal BlinkingLabel Label2;

	public TextBox txtDatabaseConnectionString;

	internal Button Btn_Cansel;

	internal Button btn_OK;

	internal GroupBox GroupBox1;

	public TextBox txtDatabaseName;

	internal Label lblNewDatabaseNameCaption;

	private TextBox txtServerConnectionSting;

	private Button Btn_SelectServerSql;

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

	public FrmSetConnectionString(string contextGuid, ENSource source, string defaulConnection)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			if (string.IsNullOrEmpty(defaulConnection))
			{
				defaulConnection = AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString;
			}
			ConnectionString = defaulConnection;
			_source = source;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 32);
		}
	}

	private void frmSetConnectionString_Load(object sender, EventArgs e)
	{
		try
		{
			switch (_source)
			{
			case ENSource.ScaleDal:
				((Control)txtDatabaseName).Text = "Scale";
				((Control)this).Text = "تنظیم رشته اتصال بانک اطلاعاتی ترازو";
				break;
			case ENSource.PayamakUI:
				((Control)txtDatabaseName).Text = "Payamak";
				((Control)this).Text = "تنظیم رشته اتصال بانک اطلاعاتی پیامک";
				break;
			}
			((Control)txtDatabaseConnectionString).Text = ConnectionString;
			((Control)txtServerConnectionSting).Text = AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmSetConnectionString_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 53);
		}
	}

	private void Btn_Cansel_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		((Form)this).Close();
	}

	private async void btn_OK_Click(object sender, EventArgs e)
	{
		if (!((Control)btn_OK).Enabled)
		{
			return;
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			((Control)btn_OK).Enabled = false;
			ErrorProvider val = new ErrorProvider();
			val.Clear();
			if (string.IsNullOrEmpty(ConnectionString) || ConnectionString != ((Control)txtDatabaseConnectionString).Text)
			{
				ConnectionString = ((Control)txtDatabaseConnectionString).Text;
			}
			if (rbtnCreate.Checked)
			{
				if (string.IsNullOrEmpty(((Control)txtDatabaseName).Text))
				{
					val.SetError((Control)(object)txtDatabaseName, "نام بانک اطلاعاتی را وارد نمایید");
					ret.AddError(EnSystem.UserInterface, "نام بانک اطلاعاتی را وارد نمایید", "", "btn_OK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 78);
					return;
				}
				CancellationToken token = new CancellationTokenSource(300000).Token;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CreateDatabaseAsync(((Control)txtDatabaseName).Text, ((Control)txtDatabaseConnectionString).Text, token), ReturnedState.None, "btn_OK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 82);
				SqlConnectionStringBuilder val2 = new SqlConnectionStringBuilder(ConnectionString)
				{
					InitialCatalog = ((Control)txtDatabaseName).Text
				};
				ConnectionString = ((DbConnectionStringBuilder)(object)val2).ConnectionString;
			}
			if (!string.IsNullOrEmpty(ConnectionString))
			{
				SqlConnectionStringBuilder val3 = new SqlConnectionStringBuilder(ConnectionString);
				if (string.IsNullOrEmpty(val3.DataSource) || string.IsNullOrEmpty(val3.InitialCatalog))
				{
					ret.AddError(EnSystem.UserInterface, "نام بانک اطلاعاتی را وارد نمایید", "", "btn_OK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 92);
					return;
				}
				switch (_source)
				{
				case ENSource.PayamakUI:
					Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin\\Payamak", "SQLCN", ConnectionString);
					break;
				case ENSource.ScaleDal:
					Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin_Scale", "ScaleConnection", ConnectionString);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btn_OK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 110);
			ret.AddReturnedValue(ex, "", "btn_OK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 111);
		}
		finally
		{
			if (!IsDisposed)
			{
				if (ret.HasError)
				{
					((Control)btn_OK).Enabled = true;
					await this.ShowReturnedSavedFuncInfoAsync(ret, "خطا در ثبت رشته اتصال", "btn_OK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs");
				}
				else
				{
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> CreateDatabaseAsync(string databaseName, string DatabaseConnectionString, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 139);
				}
			}
			CreateDatabaseResult createDatabaseResult = await DataBaseAction.Instance.CreateDataBaseAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), databaseName, ConnectionString);
			string dbName = databaseName;
			res.AddReturnedValue(createDatabaseResult.ReturnedSaveFuncInfo, ReturnedState.None, "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 143);
			if (res.HasError)
			{
				for (int i = 1; i < 20; i++)
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return res.AddErrorTokenIsCancelled("", "", "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 149);
					}
					dbName = databaseName + i;
					if (!(await DataBaseAction.Instance.CreateDataBaseAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), dbName, DatabaseConnectionString)).ReturnedSaveFuncInfo.HasError)
					{
						break;
					}
				}
			}
			if (IsDisposed || token.IsCancellationRequested)
			{
				return res.AddErrorTokenIsCancelled("", "", "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 157);
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						((Control)txtDatabaseName).Text = dbName;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 165);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 170);
			res.AddReturnedValue(ex, "", "CreateDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 171);
		}
		return res;
	}

	private void BTN_NetIntial_Click(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		try
		{
			FRMInitialSettings fRMInitialSettings = new FRMInitialSettings(base.ContextGuid, ConnectionString);
			((Form)(object)this).CheckVisibility("BTN_NetIntial_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs");
			if ((int)((Form)fRMInitialSettings).ShowDialog((IWin32Window)(object)this) == 1)
			{
				ConnectionString = fRMInitialSettings.ConnectionString;
				((Control)txtDatabaseConnectionString).Text = fRMInitialSettings.ConnectionString;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTN_NetIntial_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 185);
		}
	}

	private void rbtnCreate_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)lblNewDatabaseNameCaption).Enabled = rbtnCreate.Checked;
			((Control)txtDatabaseName).Enabled = rbtnCreate.Checked;
			((Control)BTN_NetIntial).Enabled = !rbtnCreate.Checked;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "rbtnCreate_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 196);
		}
	}

	private void frmSetConnectionString_FormClosed(object sender, FormClosedEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 215);
		}
	}

	private void Btn_SelectServerSql_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			FRMSetSqlEngine fRMSetSqlEngine = new FRMSetSqlEngine(base.ContextGuid);
			((Form)(object)this).CheckVisibility("Btn_SelectServerSql_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs");
			((Form)fRMSetSqlEngine).ShowDialog((IWin32Window)(object)this);
			((Control)txtServerConnectionSting).Text = AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Btn_SelectServerSql_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 227);
		}
	}

	private void Txt_DatabaseName_TextChanged(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(((Control)txtDatabaseConnectionString).Text);
			val.InitialCatalog = ((Control)txtDatabaseName).Text;
			((Control)txtDatabaseConnectionString).Text = ((object)val).ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Txt_DatabaseName_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmSetConnectionString.cs", 238);
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Expected O, but got Unknown
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Expected O, but got Unknown
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a30: Expected O, but got Unknown
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Expected O, but got Unknown
		BTN_NetIntial = new Button();
		rbtnNetwork = new RadioButton();
		rbtnCreate = new RadioButton();
		Label2 = new BlinkingLabel();
		txtDatabaseConnectionString = new TextBox();
		GroupBox1 = new GroupBox();
		txtDatabaseName = new TextBox();
		lblNewDatabaseNameCaption = new Label();
		Btn_Cansel = new Button();
		btn_OK = new Button();
		txtServerConnectionSting = new TextBox();
		Btn_SelectServerSql = new Button();
		((Control)GroupBox1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)BTN_NetIntial).Font = new Font("Tahoma", 9.75f, (FontStyle)1);
		((Control)BTN_NetIntial).Location = new Point(6, 59);
		((Control)BTN_NetIntial).Name = "BTN_NetIntial";
		((Control)BTN_NetIntial).Size = new Size(356, 38);
		((Control)BTN_NetIntial).TabIndex = 0;
		((Control)BTN_NetIntial).Text = "[تنظیم رشته ی اتصال]";
		((ButtonBase)BTN_NetIntial).UseVisualStyleBackColor = true;
		((Control)BTN_NetIntial).Click += BTN_NetIntial_Click;
		((Control)rbtnNetwork).Anchor = (AnchorStyles)13;
		((Control)rbtnNetwork).Font = new Font("B Titr", 9.75f, (FontStyle)1);
		((Control)rbtnNetwork).Location = new Point(0, 26);
		((Control)rbtnNetwork).Name = "rbtnNetwork";
		((Control)rbtnNetwork).Size = new Size(377, 27);
		((Control)rbtnNetwork).TabIndex = 1;
		rbtnNetwork.TabStop = true;
		((Control)rbtnNetwork).Text = "[تنظیمات رشته اتصال  پیشرفته/شبکه]";
		((ButtonBase)rbtnNetwork).UseVisualStyleBackColor = true;
		rbtnNetwork.CheckedChanged += rbtnCreate_CheckedChanged;
		((Control)rbtnCreate).Anchor = (AnchorStyles)13;
		((Control)rbtnCreate).Font = new Font("B Titr", 9.75f, (FontStyle)1);
		((Control)rbtnCreate).Location = new Point(-2, 116);
		((Control)rbtnCreate).Name = "rbtnCreate";
		((Control)rbtnCreate).Size = new Size(377, 27);
		((Control)rbtnCreate).TabIndex = 0;
		((Control)rbtnCreate).Text = "[بانک اطلاعاتی ایجاد شود]";
		((ButtonBase)rbtnCreate).UseVisualStyleBackColor = true;
		rbtnCreate.CheckedChanged += rbtnCreate_CheckedChanged;
		((Control)Label2).BackColor = Color.Red;
		Label2.BackColor2 = Color.LightGray;
		((Label)Label2).BorderStyle = (BorderStyle)1;
		((Control)Label2).Dock = (DockStyle)1;
		((Control)Label2).Font = new Font("B Titr", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label2).ForeColor = Color.White;
		((Control)Label2).Location = new Point(0, 0);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(404, 38);
		((Control)Label2).TabIndex = 19;
		((Control)Label2).Text = "[بانک اطلاعاتی با رشته اتصال ذیل در دسترس نمی باشد]";
		((Label)Label2).TextAlign = (ContentAlignment)32;
		((Control)txtDatabaseConnectionString).Anchor = (AnchorStyles)13;
		((Control)txtDatabaseConnectionString).BackColor = SystemColors.InactiveCaptionText;
		((Control)txtDatabaseConnectionString).ForeColor = Color.Yellow;
		((Control)txtDatabaseConnectionString).Location = new Point(10, 41);
		((TextBoxBase)txtDatabaseConnectionString).Multiline = true;
		((Control)txtDatabaseConnectionString).Name = "txtDatabaseConnectionString";
		((TextBoxBase)txtDatabaseConnectionString).ReadOnly = true;
		((Control)txtDatabaseConnectionString).RightToLeft = (RightToLeft)0;
		((Control)txtDatabaseConnectionString).Size = new Size(385, 64);
		((Control)txtDatabaseConnectionString).TabIndex = 18;
		((Control)GroupBox1).Anchor = (AnchorStyles)15;
		((Control)GroupBox1).BackColor = Color.Transparent;
		((Control)GroupBox1).Controls.Add((Control)(object)BTN_NetIntial);
		((Control)GroupBox1).Controls.Add((Control)(object)txtDatabaseName);
		((Control)GroupBox1).Controls.Add((Control)(object)rbtnNetwork);
		((Control)GroupBox1).Controls.Add((Control)(object)lblNewDatabaseNameCaption);
		((Control)GroupBox1).Controls.Add((Control)(object)rbtnCreate);
		((Control)GroupBox1).Font = new Font("B Titr", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)GroupBox1).Location = new Point(12, 111);
		((Control)GroupBox1).Name = "GroupBox1";
		((Control)GroupBox1).Size = new Size(383, 187);
		((Control)GroupBox1).TabIndex = 15;
		GroupBox1.TabStop = false;
		((Control)GroupBox1).Text = "[تنظیمات بانک]";
		((Control)txtDatabaseName).Anchor = (AnchorStyles)13;
		((Control)txtDatabaseName).Font = new Font("B Titr", 8.25f, (FontStyle)1);
		((Control)txtDatabaseName).Location = new Point(6, 149);
		((Control)txtDatabaseName).Name = "txtDatabaseName";
		((Control)txtDatabaseName).RightToLeft = (RightToLeft)0;
		((Control)txtDatabaseName).Size = new Size(223, 27);
		((Control)txtDatabaseName).TabIndex = 0;
		((Control)txtDatabaseName).TextChanged += Txt_DatabaseName_TextChanged;
		((Control)lblNewDatabaseNameCaption).Anchor = (AnchorStyles)9;
		((Control)lblNewDatabaseNameCaption).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblNewDatabaseNameCaption).ForeColor = Color.Black;
		((Control)lblNewDatabaseNameCaption).Location = new Point(235, 149);
		((Control)lblNewDatabaseNameCaption).Name = "lblNewDatabaseNameCaption";
		((Control)lblNewDatabaseNameCaption).Size = new Size(127, 27);
		((Control)lblNewDatabaseNameCaption).TabIndex = 3;
		((Control)lblNewDatabaseNameCaption).Text = "[نام بانک اطلاعاتی] :";
		lblNewDatabaseNameCaption.TextAlign = (ContentAlignment)64;
		((Control)Btn_Cansel).Anchor = (AnchorStyles)6;
		Btn_Cansel.DialogResult = (DialogResult)2;
		((Control)Btn_Cansel).Font = new Font("B Titr", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((ButtonBase)Btn_Cansel).Image = (Image)(object)FormCustom.Properties.Resources.stop;
		((ButtonBase)Btn_Cansel).ImageAlign = (ContentAlignment)16;
		((Control)Btn_Cansel).Location = new Point(12, 345);
		((Control)Btn_Cansel).Name = "Btn_Cansel";
		((Control)Btn_Cansel).Size = new Size(95, 42);
		((Control)Btn_Cansel).TabIndex = 3;
		((Control)Btn_Cansel).Text = "[انصراف]";
		((ButtonBase)Btn_Cansel).TextAlign = (ContentAlignment)64;
		((ButtonBase)Btn_Cansel).UseVisualStyleBackColor = true;
		((Control)Btn_Cansel).Click += Btn_Cansel_Click;
		((Control)btn_OK).Anchor = (AnchorStyles)6;
		((Control)btn_OK).Font = new Font("B Titr", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((ButtonBase)btn_OK).Image = (Image)(object)FormCustom.Properties.Resources.accept;
		((ButtonBase)btn_OK).ImageAlign = (ContentAlignment)16;
		((Control)btn_OK).Location = new Point(109, 345);
		((Control)btn_OK).Name = "btn_OK";
		((Control)btn_OK).Size = new Size(95, 42);
		((Control)btn_OK).TabIndex = 2;
		((Control)btn_OK).Text = "[تائید]";
		((ButtonBase)btn_OK).TextAlign = (ContentAlignment)64;
		((ButtonBase)btn_OK).UseVisualStyleBackColor = true;
		((Control)btn_OK).Click += btn_OK_Click;
		((Control)txtServerConnectionSting).Anchor = (AnchorStyles)14;
		((Control)txtServerConnectionSting).BackColor = Color.FromArgb(236, 236, 236);
		((Control)txtServerConnectionSting).Cursor = Cursors.IBeam;
		((Control)txtServerConnectionSting).Enabled = false;
		((Control)txtServerConnectionSting).Font = new Font("Century Gothic", 9.75f);
		((Control)txtServerConnectionSting).ForeColor = Color.FromArgb(64, 64, 64);
		((Control)txtServerConnectionSting).Location = new Point(13, 305);
		((Control)txtServerConnectionSting).Margin = new Padding(4);
		((TextBoxBase)txtServerConnectionSting).Multiline = true;
		((Control)txtServerConnectionSting).Name = "txtServerConnectionSting";
		((Control)txtServerConnectionSting).Size = new Size(353, 33);
		((Control)txtServerConnectionSting).TabIndex = 10;
		((Control)txtServerConnectionSting).TabStop = false;
		txtServerConnectionSting.TextAlign = (HorizontalAlignment)2;
		((Control)Btn_SelectServerSql).Anchor = (AnchorStyles)10;
		((ButtonBase)Btn_SelectServerSql).Image = (Image)(object)FormCustom.Properties.Resources.Setting;
		((Control)Btn_SelectServerSql).Location = new Point(365, 304);
		((Control)Btn_SelectServerSql).Name = "Btn_SelectServerSql";
		((Control)Btn_SelectServerSql).Size = new Size(37, 34);
		((Control)Btn_SelectServerSql).TabIndex = 11;
		((Control)Btn_SelectServerSql).TabStop = false;
		((Control)Btn_SelectServerSql).Click += Btn_SelectServerSql_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(208, 228, 255);
		((Form)this).ClientSize = new Size(404, 399);
		((Control)this).Controls.Add((Control)(object)txtServerConnectionSting);
		((Control)this).Controls.Add((Control)(object)Btn_SelectServerSql);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)txtDatabaseConnectionString);
		((Control)this).Controls.Add((Control)(object)Btn_Cansel);
		((Control)this).Controls.Add((Control)(object)btn_OK);
		((Control)this).Controls.Add((Control)(object)GroupBox1);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmSetConnectionString";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تنظیم رشته اتصال بانک اطلاعاتی پیامک]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmSetConnectionString_FormClosed);
		((Form)this).Load += frmSetConnectionString_Load;
		((Control)GroupBox1).ResumeLayout(false);
		((Control)GroupBox1).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
