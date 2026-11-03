using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence.AssistanceClasses;
using BackUpDLL;
using DevComponents.DotNetBar;
using FormCustom.Properties;
using FormCustom.Waiting;
using NPResourceManagement;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms;

public class FRMInitialSettings : AccountingBaseForm
{
	private bool _isDisposed;

	private CancellationTokenSource _GetDatabasesTokenSource = new CancellationTokenSource();

	private IContainer components;

	internal TextBox TextBox1;

	internal ComboBox InitialCategory;

	internal Label Label5;

	internal Panel PanelDB;

	internal TextBox Pass;

	internal Label Label6;

	internal TextBox UserName;

	internal Label Label4;

	internal Panel Panel1;

	internal Label Label2;

	internal GroupBox GroupBox1;

	internal Label Label3;

	internal ComboBox DataSource;

	internal Label Label1;

	internal GroupBox GroupBox2;

	internal CheckBox IntegratedSecuriry;

	private ButtonX BTNOK;

	private ButtonX BTNTestConnection;

	private ButtonX BTNCancel;

	internal Label lblErrorMessage;

	private Panel PanelWaiter;

	public new string ConnectionString { get; private set; } = "";


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

	public FRMInitialSettings(string contextGuid, string ConnectionString)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			this.ConnectionString = ConnectionString;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 28);
		}
	}

	private async Task CreateCNAsync()
	{
		try
		{
			_GetDatabasesTokenSource?.Cancel();
			_GetDatabasesTokenSource = new CancellationTokenSource(60000);
			CancellationToken token = _GetDatabasesTokenSource.Token;
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder
			{
				DataSource = ((Control)DataSource).Text,
				IntegratedSecurity = IntegratedSecuriry.Checked
			};
			if (!IntegratedSecuriry.Checked)
			{
				val.UserID = ((Control)UserName).Text;
				val.Password = ((Control)Pass).Text;
			}
			val.InitialCatalog = ((Control)InitialCategory).Text;
			((Control)TextBox1).Text = ((DbConnectionStringBuilder)(object)val).ConnectionString;
			await Task.Delay(1000);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (IsDisposed || token.IsCancellationRequested)
					{
						return;
					}
					string text = ((Control)TextBox1).Text;
					if (string.IsNullOrEmpty(text))
					{
						text = AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString;
					}
					((Control)PanelWaiter).Visible = true;
					((Control)PanelWaiter).BringToFront();
					using (new Waiter("در حال بارگذاری لیست بانکهای اطلاعاتی", (Control)(object)PanelWaiter, BindDatabasesAsync(text, token), freezeForm: false, EnWaiterType.Rectangle, null, "CreateCNAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs"))
					{
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "CreateCNAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 82);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateCNAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 85);
		}
	}

	private void FRMInitialSettings_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				BTNCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMInitialSettings_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 94);
		}
	}

	private void FRMInitialSettings_Load(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			SqlConnectionStringBuilder val = ((ConnectionString == "") ? new SqlConnectionStringBuilder(AccSqlServerPersistence.AssistanceClasses.Settings.ServerConnectionsString) : new SqlConnectionStringBuilder(ConnectionString));
			((Control)DataSource).Text = val.DataSource;
			IntegratedSecuriry.Checked = val.IntegratedSecurity;
			if (IntegratedSecuriry.Checked)
			{
				((Control)Panel1).Enabled = false;
			}
			else
			{
				((Control)Panel1).Enabled = true;
				((Control)UserName).Text = val.UserID;
				((Control)Pass).Text = val.Password;
			}
			((Control)InitialCategory).TextChanged += DataSource_TextChanged;
			((Control)InitialCategory).Text = val.InitialCatalog;
			((Control)Pass).TextChanged += DataSource_TextChanged;
			((Control)UserName).TextChanged += DataSource_TextChanged;
			((Control)DataSource).TextChanged += DataSource_TextChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMInitialSettings_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 136);
		}
	}

	private async void BTNOK_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (!((Control)BTNOK).Enabled || !((Control)BTNOK).Visible)
		{
			return;
		}
		try
		{
			((Control)BTNOK).Enabled = false;
			if (string.IsNullOrEmpty(((Control)InitialCategory).Text))
			{
				ret.AddError(EnSystem.None, "بانک اطلاعاتی را باید انتخاب نمایید", "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 149);
				return;
			}
			SqlConnection sqlcn = new SqlConnection(((Control)TextBox1).Text);
			try
			{
				await ((DbConnection)(object)sqlcn).OpenAsync(new CancellationTokenSource(60000).Token);
				((DbConnection)(object)sqlcn).Close();
				ConnectionString = ((Control)TextBox1).Text;
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			finally
			{
				((IDisposable)sqlcn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 162);
			ret.AddReturnedValue(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 163);
		}
		finally
		{
			((Control)BTNOK).Enabled = true;
			if (ret.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(ret, "خطا", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs");
			}
		}
	}

	private void BTNCancel_Click(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNCancel_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 180);
		}
	}

	private async void IntegratedSecuriry_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (IntegratedSecuriry.Checked)
			{
				((Control)Panel1).Enabled = false;
			}
			else
			{
				((Control)Panel1).Enabled = true;
			}
			await CreateCNAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IntegratedSecuriry_CheckedChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 202);
		}
	}

	private async void BTNTestConnection_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			GC.Collect();
			SqlConnection.ClearAllPools();
			if (string.IsNullOrEmpty(((Control)InitialCategory).Text))
			{
				MessageBox.Show((IWin32Window)(object)this, "بانک اطلاعاتی را باید انتخاب نمایید", "توجه", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				return;
			}
			await CreateCNAsync();
			SqlConnection cn = new SqlConnection(((Control)TextBox1).Text);
			try
			{
				_ = 1;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					((DbConnection)(object)cn).Close();
					MessageBox.Show((IWin32Window)(object)this, "ارتباط با موفقیت برقرار شد", "توجه", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "BTNTestConnection_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 225);
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "BTNTestConnection_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 227);
		}
	}

	private async void RBDB_CheckedChanged(object sender, EventArgs e)
	{
		await CreateCNAsync();
	}

	private async void DataSource_TextChanged(object sender, EventArgs e)
	{
		await CreateCNAsync();
	}

	private async Task BindDatabasesAsync(string connectionString, CancellationToken token)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<List<CLSServerDBs>> databasesListResult = await CLSServerDBs.GetAllAsync(connectionString, token);
			if (token.IsCancellationRequested || IsDisposed || IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						InitialCategory.Items.Clear();
						if (databasesListResult.HasError)
						{
							((Control)lblErrorMessage).Visible = true;
							((Control)lblErrorMessage).BringToFront();
							((Control)lblErrorMessage).Text = databasesListResult.GetErrorMessage();
							((Control)PanelWaiter).Visible = false;
						}
						else
						{
							((Control)lblErrorMessage).Text = "";
							((Control)lblErrorMessage).Visible = false;
							if (databasesListResult?.value != null && databasesListResult.value.Count > 0)
							{
								foreach (CLSServerDBs item in databasesListResult.value)
								{
									if (item.Name.ToLower() != "master" && item.Name.ToLower() != "model" && item.Name.ToLower() != "msdb" && item.Name.ToLower() != "tempdb")
									{
										InitialCategory.Items.Add((object)item.Name);
									}
								}
							}
							if (!token.IsCancellationRequested && !IsDisposed)
							{
								((Control)PanelWaiter).Visible = false;
							}
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "BindDatabasesAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 276);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BindDatabasesAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 279);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_GetDatabasesTokenSource?.Cancel();
				_GetDatabasesTokenSource?.Dispose();
				_GetDatabasesTokenSource = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\FRMInitialSettings.cs", 299);
		}
	}

	private void FRMInitialSettings_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private async void DataSource_TextChanged_1(object sender, EventArgs e)
	{
		await CreateCNAsync();
	}

	private async void UserName_TextChanged(object sender, EventArgs e)
	{
		await CreateCNAsync();
	}

	private async void Pass_TextChanged(object sender, EventArgs e)
	{
		await CreateCNAsync();
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
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Expected O, but got Unknown
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Expected O, but got Unknown
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Expected O, but got Unknown
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f66: Expected O, but got Unknown
		//IL_0f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc7: Expected O, but got Unknown
		//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0feb: Expected O, but got Unknown
		TextBox1 = new TextBox();
		InitialCategory = new ComboBox();
		Label5 = new Label();
		PanelDB = new Panel();
		lblErrorMessage = new Label();
		Pass = new TextBox();
		Label6 = new Label();
		UserName = new TextBox();
		Label4 = new Label();
		Panel1 = new Panel();
		Label2 = new Label();
		GroupBox1 = new GroupBox();
		Label3 = new Label();
		DataSource = new ComboBox();
		Label1 = new Label();
		GroupBox2 = new GroupBox();
		IntegratedSecuriry = new CheckBox();
		BTNOK = new ButtonX();
		BTNTestConnection = new ButtonX();
		BTNCancel = new ButtonX();
		PanelWaiter = new Panel();
		((Control)PanelDB).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((Control)GroupBox1).SuspendLayout();
		((Control)GroupBox2).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TextBox1).BackColor = Color.Gainsboro;
		((Control)TextBox1).Font = new Font("Tahoma", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TextBox1).ForeColor = Color.FromArgb(0, 0, 192);
		((Control)TextBox1).Location = new Point(-1, 472);
		((Control)TextBox1).Margin = new Padding(6, 8, 6, 8);
		((TextBoxBase)TextBox1).Multiline = true;
		((Control)TextBox1).Name = "TextBox1";
		((TextBoxBase)TextBox1).ReadOnly = true;
		((Control)TextBox1).RightToLeft = (RightToLeft)0;
		TextBox1.ScrollBars = (ScrollBars)3;
		((Control)TextBox1).Size = new Size(426, 32);
		((Control)TextBox1).TabIndex = 11;
		((ListControl)InitialCategory).FormattingEnabled = true;
		InitialCategory.Items.AddRange(new object[1] { "Hesab88" });
		((Control)InitialCategory).Location = new Point(8, 10);
		((Control)InitialCategory).Margin = new Padding(4, 5, 4, 5);
		((Control)InitialCategory).Name = "InitialCategory";
		((Control)InitialCategory).RightToLeft = (RightToLeft)0;
		((Control)InitialCategory).Size = new Size(244, 21);
		((Control)InitialCategory).TabIndex = 0;
		((Control)Label5).Location = new Point(260, 9);
		((Control)Label5).Margin = new Padding(4, 0, 4, 0);
		((Control)Label5).Name = "Label5";
		((Control)Label5).Size = new Size(121, 21);
		((Control)Label5).TabIndex = 1;
		((Control)Label5).Text = "[نام بانک اطلاعاتی] :";
		Label5.TextAlign = (ContentAlignment)64;
		((Control)PanelDB).Controls.Add((Control)(object)Label5);
		((Control)PanelDB).Controls.Add((Control)(object)InitialCategory);
		((Control)PanelDB).Controls.Add((Control)(object)lblErrorMessage);
		((Control)PanelDB).Location = new Point(13, 332);
		((Control)PanelDB).Name = "PanelDB";
		((Control)PanelDB).RightToLeft = (RightToLeft)1;
		((Control)PanelDB).Size = new Size(396, 71);
		((Control)PanelDB).TabIndex = 2;
		((Control)lblErrorMessage).BackColor = Color.Transparent;
		((Control)lblErrorMessage).ForeColor = Color.Red;
		((Control)lblErrorMessage).Location = new Point(-1, 0);
		((Control)lblErrorMessage).Margin = new Padding(4, 0, 4, 0);
		((Control)lblErrorMessage).Name = "lblErrorMessage";
		((Control)lblErrorMessage).Size = new Size(398, 71);
		((Control)lblErrorMessage).TabIndex = 2;
		((Control)lblErrorMessage).Text = "[متن خطا]";
		lblErrorMessage.TextAlign = (ContentAlignment)32;
		((Control)lblErrorMessage).Visible = false;
		((Control)Pass).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Pass).Location = new Point(3, 10);
		((Control)Pass).Name = "Pass";
		Pass.PasswordChar = '*';
		((Control)Pass).RightToLeft = (RightToLeft)0;
		((Control)Pass).Size = new Size(98, 21);
		((Control)Pass).TabIndex = 1;
		((Control)Pass).TextChanged += Pass_TextChanged;
		((Control)Label6).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label6).Location = new Point(107, 10);
		((Control)Label6).Name = "Label6";
		((Control)Label6).Size = new Size(86, 20);
		((Control)Label6).TabIndex = 2;
		((Control)Label6).Text = "[کلمه عبور] :";
		Label6.TextAlign = (ContentAlignment)64;
		((Control)UserName).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)UserName).Location = new Point(202, 10);
		((Control)UserName).Name = "UserName";
		((Control)UserName).RightToLeft = (RightToLeft)0;
		((Control)UserName).Size = new Size(98, 21);
		((Control)UserName).TabIndex = 0;
		((Control)UserName).TextChanged += UserName_TextChanged;
		((Control)Label4).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label4).Location = new Point(306, 10);
		((Control)Label4).Name = "Label4";
		((Control)Label4).Size = new Size(79, 20);
		((Control)Label4).TabIndex = 0;
		((Control)Label4).Text = "[نام کاربر] :";
		Label4.TextAlign = (ContentAlignment)64;
		((Control)Panel1).Controls.Add((Control)(object)Pass);
		((Control)Panel1).Controls.Add((Control)(object)Label6);
		((Control)Panel1).Controls.Add((Control)(object)UserName);
		((Control)Panel1).Controls.Add((Control)(object)Label4);
		((Control)Panel1).Enabled = false;
		((Control)Panel1).Location = new Point(5, 53);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).RightToLeft = (RightToLeft)1;
		((Control)Panel1).Size = new Size(391, 41);
		((Control)Panel1).TabIndex = 1;
		((Control)Label2).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)Label2).ForeColor = Color.FromArgb(192, 0, 0);
		((Control)Label2).Location = new Point(12, 9);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(401, 58);
		((Control)Label2).TabIndex = 0;
		((Control)Label2).Text = "[کاربر گرامی] ، \r\n[توصیه میشود جهت تنظیم پارامترهای این قسمت از تیم فنی گروه نرم افزاری]" + TextResourceManagement.TextResource.CompanyName + " [کمک بگیرید]";
		((Control)GroupBox1).BackColor = Color.FromArgb(192, 192, 255);
		((Control)GroupBox1).Controls.Add((Control)(object)Label3);
		((Control)GroupBox1).Controls.Add((Control)(object)DataSource);
		((Control)GroupBox1).Controls.Add((Control)(object)Label1);
		((Control)GroupBox1).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Control)GroupBox1).Location = new Point(13, 72);
		((Control)GroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)GroupBox1).Name = "GroupBox1";
		((Control)GroupBox1).Padding = new Padding(4, 5, 4, 5);
		((Control)GroupBox1).Size = new Size(396, 140);
		((Control)GroupBox1).TabIndex = 0;
		GroupBox1.TabStop = false;
		((Control)GroupBox1).Text = "[مشخصات سرور]";
		((Control)Label3).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label3).ForeColor = Color.Blue;
		((Control)Label3).Location = new Point(5, 63);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(374, 72);
		((Control)Label3).TabIndex = 2;
		((Control)Label3).Text = " [اگر بانک اطلاعاتی روی سیستم دیگری می باشد]\r\n[نام سرور را باید به صورت ServerIP\\SQLExpress] [پر کنید]\r\n\r\n[وگرنه به صورت  SQLExpress]\\.    [پر کنید]";
		((ListControl)DataSource).FormattingEnabled = true;
		DataSource.Items.AddRange(new object[3] { ".\\SQLExpress", ".", "(Local)" });
		((Control)DataSource).Location = new Point(8, 30);
		((Control)DataSource).Margin = new Padding(4, 5, 4, 5);
		((Control)DataSource).Name = "DataSource";
		((Control)DataSource).RightToLeft = (RightToLeft)0;
		((Control)DataSource).Size = new Size(291, 21);
		((Control)DataSource).TabIndex = 0;
		((Control)DataSource).TextChanged += DataSource_TextChanged_1;
		((Control)Label1).Location = new Point(305, 30);
		((Control)Label1).Margin = new Padding(4, 0, 4, 0);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(91, 21);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "[نام سرور] :";
		Label1.TextAlign = (ContentAlignment)64;
		((Control)GroupBox2).BackColor = Color.FromArgb(208, 228, 255);
		((Control)GroupBox2).Controls.Add((Control)(object)Panel1);
		((Control)GroupBox2).Controls.Add((Control)(object)IntegratedSecuriry);
		((Control)GroupBox2).Location = new Point(13, 222);
		((Control)GroupBox2).Margin = new Padding(4, 5, 4, 5);
		((Control)GroupBox2).Name = "GroupBox2";
		((Control)GroupBox2).Padding = new Padding(4, 5, 4, 5);
		((Control)GroupBox2).Size = new Size(396, 102);
		((Control)GroupBox2).TabIndex = 1;
		GroupBox2.TabStop = false;
		((Control)GroupBox2).Text = "[احراز هویت]";
		((Control)IntegratedSecuriry).Anchor = (AnchorStyles)13;
		IntegratedSecuriry.Checked = true;
		IntegratedSecuriry.CheckState = (CheckState)1;
		((Control)IntegratedSecuriry).Location = new Point(0, 23);
		((Control)IntegratedSecuriry).Name = "IntegratedSecuriry";
		((Control)IntegratedSecuriry).RightToLeft = (RightToLeft)1;
		((Control)IntegratedSecuriry).Size = new Size(390, 24);
		((Control)IntegratedSecuriry).TabIndex = 0;
		((Control)IntegratedSecuriry).Text = "[احراز هویت توسط ویندوز انجام شود]";
		((ButtonBase)IntegratedSecuriry).UseVisualStyleBackColor = true;
		IntegratedSecuriry.CheckedChanged += IntegratedSecuriry_CheckedChanged;
		((Control)BTNOK).AccessibleRole = (AccessibleRole)43;
		BTNOK.ColorTable = eButtonColor.OrangeWithBackground;
		BTNOK.Image = (Image)(object)FormCustom.Properties.Resources.accept;
		BTNOK.ImagePosition = eImagePosition.Right;
		((Control)BTNOK).Location = new Point(215, 413);
		((Control)BTNOK).Name = "BTNOK";
		((Control)BTNOK).Size = new Size(97, 34);
		BTNOK.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNOK).TabIndex = 3;
		((Control)BTNOK).Text = "[تایید]";
		((Control)BTNOK).Click += BTNOK_Click;
		((Control)BTNTestConnection).AccessibleRole = (AccessibleRole)43;
		BTNTestConnection.ColorTable = eButtonColor.OrangeWithBackground;
		BTNTestConnection.ImagePosition = eImagePosition.Right;
		((Control)BTNTestConnection).Location = new Point(114, 413);
		((Control)BTNTestConnection).Name = "BTNTestConnection";
		((Control)BTNTestConnection).Size = new Size(97, 34);
		BTNTestConnection.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNTestConnection).TabIndex = 4;
		((Control)BTNTestConnection).Text = "[تست ارتباط]";
		((Control)BTNTestConnection).Click += BTNTestConnection_Click;
		((Control)BTNCancel).AccessibleRole = (AccessibleRole)43;
		BTNCancel.ColorTable = eButtonColor.OrangeWithBackground;
		BTNCancel.DialogResult = (DialogResult)2;
		BTNCancel.Image = (Image)(object)FormCustom.Properties.Resources.stop;
		BTNCancel.ImagePosition = eImagePosition.Right;
		((Control)BTNCancel).Location = new Point(12, 413);
		((Control)BTNCancel).Name = "BTNCancel";
		((Control)BTNCancel).Size = new Size(97, 34);
		BTNCancel.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)BTNCancel).TabIndex = 5;
		((Control)BTNCancel).Text = "[انصراف]";
		((Control)BTNCancel).Click += BTNCancel_Click;
		((Control)PanelWaiter).Anchor = (AnchorStyles)13;
		((Control)PanelWaiter).Location = new Point(11, 322);
		((Control)PanelWaiter).Name = "PanelWaiter";
		((Control)PanelWaiter).Size = new Size(399, 85);
		((Control)PanelWaiter).TabIndex = 12;
		((Form)this).AcceptButton = (IButtonControl)(object)BTNOK;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(208, 228, 255);
		((Form)this).CancelButton = (IButtonControl)(object)BTNCancel;
		((Form)this).ClientSize = new Size(415, 454);
		((Control)this).Controls.Add((Control)(object)PanelDB);
		((Control)this).Controls.Add((Control)(object)BTNCancel);
		((Control)this).Controls.Add((Control)(object)BTNTestConnection);
		((Control)this).Controls.Add((Control)(object)TextBox1);
		((Control)this).Controls.Add((Control)(object)BTNOK);
		((Control)this).Controls.Add((Control)(object)Label2);
		((Control)this).Controls.Add((Control)(object)GroupBox1);
		((Control)this).Controls.Add((Control)(object)GroupBox2);
		((Control)this).Controls.Add((Control)(object)PanelWaiter);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1);
		((Form)this).FormBorderStyle = (FormBorderStyle)2;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMInitialSettings";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تنظیمات ارتباط با شبکه]";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMInitialSettings_FormClosed);
		((Form)this).Load += FRMInitialSettings_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMInitialSettings_KeyDown);
		((Control)PanelDB).ResumeLayout(false);
		((Control)Panel1).ResumeLayout(false);
		((Control)Panel1).PerformLayout();
		((Control)GroupBox1).ResumeLayout(false);
		((Control)GroupBox2).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
