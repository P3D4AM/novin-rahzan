using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom;
using FormCustom.ServiceManagement;
using FormCustom.UC;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PayamakUi.Classes.Service;

namespace PayamakUi.Forms.FRMOthers;

public class frmPayamakSignalRSetting : AccountingBaseForm
{
	private bool _isDisposed;

	private ServiceManager _serviceManager;

	private string _selectedConnectionString;

	private IContainer components;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	private UcServiceManager ucServiceManager1;

	public string Ip { get; private set; }

	public string SetupIp { get; private set; }

	public int Port { get; private set; }

	public string SelectedConnectionString
	{
		get
		{
			return _selectedConnectionString;
		}
		set
		{
			_selectedConnectionString = value;
			ServiceManagerConfig config = new ServiceManagerConfig
			{
				Manager = _serviceManager,
				Ip = Ip,
				SetUpIp = SetupIp,
				Port = Port,
				ConnectionString = _selectedConnectionString,
				HandleConnectionString = true,
				HandleConnectionStringEncrypt = true,
				HandleConnectionStringMultiSubnetFailover = true,
				HandleConnectionStringPersistSecurityInfo = true,
				HandleConnectionStringTrustServerCertificate = true
			};
			Task.Run(() => ucServiceManager1.InitAsync(config));
		}
	}

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 55);
		}
	}

	public frmPayamakSignalRSetting(string contextGuid, string ip, string setupIp, int port, string serviceConString)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			_serviceManager = new ServiceManager(contextGuid);
			Ip = ip;
			SetupIp = setupIp;
			Port = port;
			if (string.IsNullOrEmpty(ip) && port == 0)
			{
				Ip = "127.0.0.1";
				port = 7595;
			}
			SelectedConnectionString = serviceConString;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 76);
		}
	}

	private void frmPayamakSignalRSetting_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private async void btnOk_OnClick(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				if (string.IsNullOrEmpty(ucServiceManager1.Config.Ip))
				{
					returnedSaveFuncInfo.AddError("آدرس IP سرور معتبر نمی باشد", "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 85);
				}
				if (ucServiceManager1.Config.Port <= 0)
				{
					returnedSaveFuncInfo.AddError("پورت سرور معتبر نمی باشد", "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 86);
				}
				if (string.IsNullOrEmpty(ucServiceManager1.ConnectionString))
				{
					returnedSaveFuncInfo.AddError("رشته اتصال سرور معتبر نمی باشد", "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 87);
				}
				if (returnedSaveFuncInfo.HasError)
				{
					goto end_IL_0028;
				}
				Ip = ucServiceManager1.Config.Ip;
				SetupIp = ucServiceManager1.Config.SetUpIp;
				Port = ucServiceManager1.Config.Port;
				_selectedConnectionString = ucServiceManager1.ConnectionString;
				goto end_IL_0028_2;
				end_IL_0028:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 97);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 98);
				goto end_IL_0028_2;
			}
			end_IL_0028_2:;
		}
		finally
		{
			if (returnedSaveFuncInfo.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(returnedSaveFuncInfo, "پیغام سیستم", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs");
			}
			else
			{
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		try
		{
			((Form)this).DialogResult = (DialogResult)2;
			((Form)this).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnCancel_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 117);
		}
	}

	private void frmPayamakSignalRSetting_KeyDown(object sender, KeyEventArgs e)
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
					btnOk.PerformClick();
				}
			}
			else
			{
				btnCancel.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmPayamakSignalRSetting_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakSignalRSetting.cs", 129);
		}
	}

	private void InitializeComponent()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmPayamakSignalRSetting));
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		ucServiceManager1 = new UcServiceManager();
		((Control)this).SuspendLayout();
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = EnButtonCornerType.Round;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(17, 395);
		((Control)btnCancel).Margin = new Padding(0);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(177, 39);
		((Control)btnCancel).TabIndex = 10;
		btnCancel.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((Control)btnOk).Anchor = (AnchorStyles)6;
		((Control)btnOk).BackColor = Color.Transparent;
		btnOk.CornerType = EnButtonCornerType.Round;
		btnOk.DisplayText = "[تایید](F5)";
		((Control)btnOk).Font = new Font("B Yekan", 9.75f);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(207, 395);
		((Control)btnOk).Margin = new Padding(0);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(177, 39);
		((Control)btnOk).TabIndex = 9;
		btnOk.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((ScrollableControl)ucServiceManager1).AutoScroll = true;
		ucServiceManager1.Config = null;
		((Control)ucServiceManager1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucServiceManager1).Location = new Point(7, 5);
		((Control)ucServiceManager1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucServiceManager1).Name = "ucServiceManager1";
		((Control)ucServiceManager1).RightToLeft = (RightToLeft)1;
		((Control)ucServiceManager1).Size = new Size(526, 351);
		((Control)ucServiceManager1).TabIndex = 11;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(545, 443);
		((Control)this).Controls.Add((Control)(object)ucServiceManager1);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(561, 296);
		((Control)this).Name = "frmPayamakSignalRSetting";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تنظیمات سرویس پیامک]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmPayamakSignalRSetting_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmPayamakSignalRSetting_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
