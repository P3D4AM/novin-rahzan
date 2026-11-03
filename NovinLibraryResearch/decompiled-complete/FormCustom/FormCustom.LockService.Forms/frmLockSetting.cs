using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.LockService.Class;
using FormCustom.ServiceManagement;
using FormCustom.UC;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.LockService.Forms;

public class frmLockSetting : AccountingBaseForm
{
	private bool _isDispose;

	private LockServiceManager _lockService;

	private LockClientSetting _setting;

	private LockClientSetting _settingOrginal;

	private CancellationTokenSource _token;

	private IContainer components;

	private UcButtons btnCancel;

	private UcButtons btnOk;

	private UcServiceManager ucServiceManager1;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDispose)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDispose)
			{
				_isDispose = true;
				_token?.Cancel();
				_token?.Dispose();
				_token = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 38);
		}
	}

	private async Task SetDataAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
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
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					if (string.IsNullOrEmpty(_setting?.Ip) || (_setting?.Port ?? 0) <= 0)
					{
						_setting = new LockClientSetting("127.0.0.1", "0.0.0.0", 7596);
					}
					ServiceManagerConfig config = new ServiceManagerConfig
					{
						Ip = _setting.Ip,
						Port = _setting.Port,
						Manager = _lockService,
						HandleConnectionString = false,
						SetUpIp = _setting.SetUpIp
					};
					Task.Run(() => ucServiceManager1.InitAsync(config, token));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 68);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 71);
		}
	}

	public frmLockSetting(string contextGuid, ENSource source)
		: base(contextGuid)
	{
		frmLockSetting frmLockSetting2 = this;
		try
		{
			InitializeComponent();
			_setting = LockClientSetting.GetSetting();
			_settingOrginal = new LockClientSetting
			{
				Ip = _setting.Ip,
				Port = _setting.Port
			};
			_lockService = new LockServiceManager(contextGuid, this, source);
			_token = new CancellationTokenSource(60000);
			CancellationToken token = _token.Token;
			Task.Run(() => frmLockSetting2.SetDataAsync(token));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 93);
		}
	}

	private void frmLockSetting_KeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "frmLockSetting_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 106);
		}
	}

	private async void btnOk_OnClick(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ucServiceManager1.EndEdit();
			ServiceManagerConfig config = ucServiceManager1.Config;
			if (PacketParser.Utilities.ParsIP(config.Ip))
			{
				_setting.Ip = config.Ip;
			}
			else
			{
				res.AddError("Ip وارد شده نا معتبر نیست".GetTranslate(), "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 116);
			}
			_setting.Port = config.Port;
			_setting.SetUpIp = config.SetUpIp;
			await LockClientSetting.SetSettingAsync(_setting);
			if ((_lockService.CheckService() != ServiceStatus.NotFound && _settingOrginal.Ip != _setting.Ip) || _settingOrginal.Port != _setting.Port)
			{
				res.AddWarning("با توجه به تغییر تنظیمات، سرویس قفل را حذف و مجددا نصب نمایید ".GetTranslate(), "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 123);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs", 125);
		}
		finally
		{
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs");
			}
			else
			{
				if (res.HasWarning)
				{
					await this.ShowReturnedSavedFuncInfoAsync(res, "هشدار سیستم".GetTranslate(), "btnOk_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockSetting.cs");
				}
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	private void btnCancel_OnClick(object sender, EventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void frmLockSetting_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmLockSetting));
		btnCancel = new UcButtons();
		btnOk = new UcButtons();
		ucServiceManager1 = new UcServiceManager();
		((Control)this).SuspendLayout();
		((Control)btnCancel).Anchor = (AnchorStyles)6;
		((Control)btnCancel).BackColor = Color.Transparent;
		btnCancel.CornerType = EnButtonCornerType.Round;
		((Control)btnCancel).Cursor = Cursors.Hand;
		btnCancel.DisplayText = "[انصراف](Esc)";
		((Control)btnCancel).Font = new Font("B Yekan", 9.75f);
		btnCancel.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnCancel.IsShowMoreDetails = false;
		((Control)btnCancel).Location = new Point(9, 383);
		((Control)btnCancel).Margin = new Padding(0);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).RightToLeft = (RightToLeft)1;
		((Control)btnCancel).Size = new Size(135, 39);
		((Control)btnCancel).TabIndex = 3;
		btnCancel.TitleFont = new Font("PeydaWeb", 9.749999f);
		btnCancel.Type = EnButtonType.Normal;
		btnCancel.OnClick += btnCancel_OnClick;
		((Control)btnOk).Anchor = (AnchorStyles)6;
		((Control)btnOk).BackColor = Color.Transparent;
		btnOk.CornerType = EnButtonCornerType.Round;
		((Control)btnOk).Cursor = Cursors.Hand;
		btnOk.DisplayText = "[تایید](F5)";
		((Control)btnOk).Font = new Font("B Yekan", 9.75f);
		btnOk.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnOk.IsShowMoreDetails = false;
		((Control)btnOk).Location = new Point(153, 383);
		((Control)btnOk).Margin = new Padding(0);
		((Control)btnOk).Name = "btnOk";
		((Control)btnOk).RightToLeft = (RightToLeft)1;
		((Control)btnOk).Size = new Size(174, 39);
		((Control)btnOk).TabIndex = 2;
		btnOk.TitleFont = new Font("PeydaWeb", 9.749999f);
		btnOk.Type = EnButtonType.Bold;
		btnOk.OnClick += btnOk_OnClick;
		((ScrollableControl)ucServiceManager1).AutoScroll = true;
		ucServiceManager1.Config = null;
		((Control)ucServiceManager1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)ucServiceManager1).Location = new Point(3, 3);
		((Control)ucServiceManager1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucServiceManager1).Name = "ucServiceManager1";
		((Control)ucServiceManager1).RightToLeft = (RightToLeft)1;
		((Control)ucServiceManager1).Size = new Size(527, 357);
		((Control)ucServiceManager1).TabIndex = 4;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 17f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(533, 428);
		((Control)this).Controls.Add((Control)(object)ucServiceManager1);
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOk);
		((Control)this).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4);
		((Form)this).MaximizeBox = false;
		((Control)this).MaximumSize = new Size(549, 467);
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmLockSetting";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[تنظیمات قفل]";
		((Form)this).FormClosed += new FormClosedEventHandler(frmLockSetting_FormClosed);
		((Control)this).KeyDown += new KeyEventHandler(frmLockSetting_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
