using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BackUpDLL;
using EntityCache;
using FormCustom.BackUp.Forms;
using FormCustom.ExtendedControls.Notification;
using FormCustom.ManagementPassword;
using FormCustom.PortStore;
using FormCustom.Properties;
using FormCustom.UrlBuilder;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.ServiceManagement;

public class UcServiceManager : UserControl, INpDisposable, IDisposable, IHasContext
{
	private Color _foreColor = Color.Black;

	private CancellationTokenSource _checkUrlToken;

	private IContainer components;

	private GroupBox grpAddressHub;

	private Button btnSetConnectionString;

	private NumericUpDown nudPort;

	internal Label label1;

	internal Label label400;

	private Label lbPortServer;

	internal TextBox txtConnectionString;

	internal TextBox txtIP;

	internal Panel pnlService;

	private PictureBox picRestart;

	private PictureBox picStop;

	internal Label lblInstall;

	internal Label lblUninstall;

	private PictureBox picUninstall;

	internal Label lblRestart;

	private PictureBox picStart;

	internal Label lblStop;

	private PictureBox picInstall;

	internal Label lblStart;

	private LinkLabel lblUrl;

	private UcPortStore ucPortStore1;

	internal Label label2;

	internal Label lblSetUpIp;

	internal Label lblSetUpIpHeader;

	private PictureBox picTestUrl;

	public string ContextGuid { get; private set; }

	public ServiceManagerConfig Config { get; set; }

	public string ConnectionString
	{
		get
		{
			return ((Control)txtConnectionString).Text;
		}
		private set
		{
			((Control)txtConnectionString).Text = value;
		}
	}

	public string ServiceUrl => $"http://{Config.Ip}:{Config.Port}/{Config.Manager.HubName}";

	public Color ForeColor
	{
		get
		{
			return _foreColor;
		}
		set
		{
			_foreColor = value;
			Label obj = label400;
			Label obj2 = label1;
			Color color = (((Control)lbPortServer).ForeColor = _foreColor);
			Color foreColor2 = (((Control)obj2).ForeColor = color);
			((Control)obj).ForeColor = foreColor2;
			Label obj3 = lblInstall;
			Label obj4 = lblUninstall;
			Label obj5 = lblStart;
			Label obj6 = lblStop;
			Color color3 = (((Control)lblRestart).ForeColor = _foreColor);
			Color color5 = (((Control)obj6).ForeColor = color3);
			color = (((Control)obj5).ForeColor = color5);
			foreColor2 = (((Control)obj4).ForeColor = color);
			((Control)obj3).ForeColor = foreColor2;
			Label obj7 = lblSetUpIp;
			foreColor2 = (((Control)lblSetUpIpHeader).ForeColor = _foreColor);
			((Control)obj7).ForeColor = foreColor2;
		}
	}

	public event EventHandler OnChangeText;

	public async Task InitAsync(ServiceManagerConfig config, CancellationToken token = default(CancellationToken), Font font = null)
	{
		try
		{
			Config = config;
			ContextGuid = Config.Manager.ContextGuid;
			if (font == null)
			{
				font = new Font(new FontFamily("B Titr"), "11.25".ParseTofloat(0f, 3));
			}
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)((Control)this).FindForm()).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					ucPortStore1.Init(ContextGuid, EnPortType.Service, font);
					ucPortStore1.OnPortChanged += UcPortStore1_OnPortChanged;
					ConnectionString = Config.ConnectionString;
					((Control)txtIP).Text = config.Ip;
					nudPort.Value = config.Port;
					((Control)lblSetUpIp).Text = config.SetUpIp;
					((Control)txtConnectionString).Text = ConnectionString;
					((Control)btnSetConnectionString).Enabled = config.HandleConnectionString;
					((Control)lblUrl).Text = ServiceUrl;
					((Control)lblUrl).Visible = !string.IsNullOrEmpty(config.Manager.HubName);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 76);
				}
				finally
				{
					UpdateServiceUi();
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 80);
		}
	}

	public void EndEdit()
	{
		nudPort.EndInit();
	}

	private void UcPortStore1_OnPortChanged(int port)
	{
		nudPort.Value = port;
	}

	private void UpdateServiceUi(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!((Control)this).IsDisposed && !token.IsCancellationRequested)
			{
				((Control)lblStart).Enabled = false;
				((Control)picStart).Enabled = false;
				picStart.Image = (Image)(object)FormCustom.Properties.Resources.WinService_start_disable;
				((Control)lblUninstall).Enabled = false;
				((Control)picUninstall).Enabled = false;
				picUninstall.Image = (Image)(object)FormCustom.Properties.Resources.WinService_Uninstall_disable;
				((Control)lblRestart).Enabled = false;
				((Control)picRestart).Enabled = false;
				picRestart.Image = (Image)(object)FormCustom.Properties.Resources.WinService_restart_disable;
				((Control)lblStop).Enabled = false;
				((Control)picStop).Enabled = false;
				picStop.Image = (Image)(object)FormCustom.Properties.Resources.WinService_stop_disable;
				((Control)lblInstall).Enabled = false;
				((Control)picInstall).Enabled = false;
				picInstall.Image = (Image)(object)FormCustom.Properties.Resources.WinService_install_disable;
				switch (Config.Manager.CheckService())
				{
				case ServiceStatus.Paused:
				case ServiceStatus.Stopped:
				case ServiceStatus.StopPending:
					((Control)lblStart).Enabled = true;
					((Control)picStart).Enabled = true;
					picStart.Image = (Image)(object)FormCustom.Properties.Resources.WinService_start;
					((Control)lblUninstall).Enabled = true;
					((Control)picUninstall).Enabled = true;
					picUninstall.Image = (Image)(object)FormCustom.Properties.Resources.WinService_Uninstall;
					((Control)lblRestart).Enabled = true;
					((Control)picRestart).Enabled = true;
					picRestart.Image = (Image)(object)FormCustom.Properties.Resources.WinService_restart;
					break;
				case ServiceStatus.NotFound:
					((Control)lblInstall).Enabled = true;
					((Control)picInstall).Enabled = true;
					picInstall.Image = (Image)(object)FormCustom.Properties.Resources.WinService_install;
					break;
				case ServiceStatus.Running:
				case ServiceStatus.StartPending:
					((Control)lblUninstall).Enabled = true;
					((Control)picUninstall).Enabled = true;
					picUninstall.Image = (Image)(object)FormCustom.Properties.Resources.WinService_Uninstall;
					((Control)lblRestart).Enabled = true;
					((Control)picRestart).Enabled = true;
					picRestart.Image = (Image)(object)FormCustom.Properties.Resources.WinService_restart;
					((Control)lblStop).Enabled = true;
					((Control)picStop).Enabled = true;
					picStop.Image = (Image)(object)FormCustom.Properties.Resources.WinService_stop;
					break;
				case ServiceStatus.PausePending:
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateServiceUi", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 148);
		}
	}

	private async Task CheckUrlAsync(string url, CancellationToken token)
	{
		try
		{
			ReturnedSaveFuncInfo validUrlStatus = new ReturnedSaveFuncInfo();
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = validUrlStatus;
			returnedSaveFuncInfo.AddReturnedValue(await UrlView.CheckUrlValidationAsync(url, token), ReturnedState.None, "CheckUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 156);
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested || ((Control)this).IsDisposed)
				{
					return;
				}
			}
			if (token.IsCancellationRequested || ((Control)this).IsDisposed)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					if (!token.IsCancellationRequested && !((Control)this).IsDisposed)
					{
						if (validUrlStatus.HasError)
						{
							picTestUrl.Image = picTestUrl.ErrorImage;
						}
						else
						{
							picTestUrl.Image = picTestUrl.InitialImage;
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "CheckUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 175);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 178);
		}
	}

	public UcServiceManager()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private async void picInstall_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		((Control)pnlService).Enabled = false;
		try
		{
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Config.Manager.InstallServiceAsync(((Control)txtConnectionString).Text, ((Control)lblSetUpIp).Text, (int)nudPort.Value), ReturnedState.None, "picInstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 193);
				if (((Control)this).IsDisposed)
				{
					goto end_IL_0060;
				}
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)lblUrl).Text;
				Task.Run(() => CheckUrlAsync(str, token));
				goto end_IL_005a;
				end_IL_0060:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "picInstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 204);
				ret.AddReturnedValue(ex, "", "picInstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 205);
				goto end_IL_005a;
			}
			end_IL_005a:;
		}
		finally
		{
			((Control)pnlService).Enabled = true;
			await this.ShowReturnedSavedFuncInfoForUserControlAsync(ret, ret.HasError ? "خطا در نصب سرویس".GetTranslate() : "سرویس با موفقیت نصب شد".GetTranslate(), "picInstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs");
			UpdateServiceUi();
		}
	}

	private async void picUninstall_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		((Control)pnlService).Enabled = false;
		try
		{
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Config.Manager.UnInstallServiceAsync(), ReturnedState.None, "picUninstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 220);
				if (((Control)this).IsDisposed)
				{
					goto end_IL_0060;
				}
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)lblUrl).Text;
				Task.Run(() => CheckUrlAsync(str, token));
				goto end_IL_005a;
				end_IL_0060:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "picUninstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 231);
				ret.AddReturnedValue(ex, "", "picUninstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 232);
				goto end_IL_005a;
			}
			end_IL_005a:;
		}
		finally
		{
			((Control)pnlService).Enabled = true;
			await this.ShowReturnedSavedFuncInfoForUserControlAsync(ret, ret.HasError ? "خطا در حذف سرویس".GetTranslate() : "سرویس با موفقیت حذف شد".GetTranslate(), "picUninstall_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs");
			UpdateServiceUi();
		}
	}

	private async void picStart_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		((Control)pnlService).Enabled = false;
		try
		{
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await Config.Manager.StartServiceAsync(), ReturnedState.None, "picStart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 247);
				if (((Control)this).IsDisposed)
				{
					goto end_IL_0060;
				}
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)lblUrl).Text;
				Task.Run(() => CheckUrlAsync(str, token));
				goto end_IL_005a;
				end_IL_0060:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "picStart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 258);
				res.AddReturnedValue(ex, "", "picStart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 259);
				goto end_IL_005a;
			}
			end_IL_005a:;
		}
		finally
		{
			((Control)pnlService).Enabled = true;
			UpdateServiceUi();
			await this.ShowReturnedSavedFuncInfoForUserControlAsync(res, res.HasError ? "خطا در اجرای سرویس".GetTranslate() : "سرویس با موفقیت اجرا شد".GetTranslate(), "picStart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs");
		}
	}

	private async void picStop_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		((Control)pnlService).Enabled = false;
		try
		{
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await Config.Manager.StopServiceAsync(), ReturnedState.None, "picStop_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 274);
				if (((Control)this).IsDisposed)
				{
					goto end_IL_0060;
				}
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)lblUrl).Text;
				Task.Run(() => CheckUrlAsync(str, token));
				goto end_IL_005a;
				end_IL_0060:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "picStop_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 285);
				res.AddReturnedValue(ex, "", "picStop_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 286);
				goto end_IL_005a;
			}
			end_IL_005a:;
		}
		finally
		{
			((Control)pnlService).Enabled = true;
			UpdateServiceUi();
			await this.ShowReturnedSavedFuncInfoForUserControlAsync(res, res.HasError ? "خطا در توقف سرویس".GetTranslate() : "سرویس با موفقیت اجرا شد ".GetTranslate(), "picStop_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs");
		}
	}

	private async void picRestart_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		((Control)pnlService).Enabled = false;
		try
		{
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Config.Manager.RestartServiceAsync(), ReturnedState.None, "picRestart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 301);
				if (((Control)this).IsDisposed)
				{
					goto end_IL_0068;
				}
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)lblUrl).Text;
				Task.Run(() => CheckUrlAsync(str, token));
				goto end_IL_0062;
				end_IL_0068:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "picRestart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 312);
				ret.AddReturnedValue(ex, "", "picRestart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 313);
				goto end_IL_0062;
			}
			end_IL_0062:;
		}
		finally
		{
			((Control)pnlService).Enabled = true;
			await Task.Delay(200);
			UpdateServiceUi();
			await this.ShowReturnedSavedFuncInfoForUserControlAsync(ret, ret.HasError ? "خطا در راه اندازی مجدد سرویس".GetTranslate() : "سرویس با موفقیت راه اندازی مجدد شد".GetTranslate(), "picRestart_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs");
		}
	}

	private void lblUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(((Control)lblUrl).Text))
			{
				Clipboard.SetText(((Control)lblUrl).Text);
				NotificationManager.MessageBoxShowByNotif(showPopUp: true, EnNotification.Information, "ادرس HUB در کلیپ بورد کپی شد".GetTranslate(), "", 1);
				Process.Start(((Control)lblUrl).Text);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblUrl_LinkClicked", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 332);
		}
	}

	private void nudPort_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			Config.Port = (int)nudPort.Value;
			if (((Control)lblUrl).Visible)
			{
				((Control)lblUrl).Text = ServiceUrl;
			}
			this.OnChangeText?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "nudPort_ValueChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 342);
		}
	}

	private async void btnSetConnectionString_Click(object sender, EventArgs e)
	{
		try
		{
			ENSource source = ClsCache.GetCache(ContextGuid)?.Source ?? ENSource.None;
			FrmSetConnectionString frmConnection = new FrmSetConnectionString(ContextGuid, source, ((Control)txtConnectionString).Text);
			if ((int)((Form)frmConnection).ShowDialog((IWin32Window)(object)this) != 1)
			{
				return;
			}
			if (await DataBaseAction.Instance.GetSqlAuthenticationModeAsync(frmConnection.ConnectionString) != EnSqlAuthenticationMode.SqlServerAuthentication)
			{
				((UserControl)(object)this).ShowError("رشته اتصال سرویس معتبر نمی باشد. \r\n لطفا نام کاربری و رمز عبور مربوط به اس کیو ال را وارد نمایید \r\n Please Use SqlServerAuthentication \r\n " + frmConnection.ConnectionString);
				((Control)txtConnectionString).Text = "";
				return;
			}
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(frmConnection.ConnectionString);
			if (Config.HandleConnectionStringEncrypt)
			{
				val.Encrypt = true;
			}
			if (Config.HandleConnectionStringTrustServerCertificate)
			{
				val.TrustServerCertificate = true;
			}
			if (Config.HandleConnectionStringPersistSecurityInfo)
			{
				val.PersistSecurityInfo = true;
			}
			if (Config.HandleConnectionStringMultiSubnetFailover)
			{
				val.MultiSubnetFailover = true;
			}
			((Control)txtConnectionString).Text = ((DbConnectionStringBuilder)(object)val).ConnectionString;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnSetConnectionString_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 365);
		}
	}

	private void txtIP_TextChanged_1(object sender, EventArgs e)
	{
		try
		{
			if (((Control)txtIP).Focused)
			{
				Config.Ip = ((Control)txtIP).Text;
			}
			if (((Control)lblUrl).Visible)
			{
				((Control)lblUrl).Text = ServiceUrl;
			}
			this.OnChangeText?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "txtIP_TextChanged_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 375);
		}
	}

	private void lblSetUpIp_Enter(object sender, EventArgs e)
	{
		((Control)lblSetUpIp).ForeColor = Color.Red;
	}

	private void lblSetUpIp_Leave(object sender, EventArgs e)
	{
		((Control)lblSetUpIp).ForeColor = ForeColor;
	}

	private async void lblSetUpIp_Click(object sender, EventArgs e)
	{
		try
		{
			if ((int)((Form)new frmManagementPass(ContextGuid)).ShowDialog() == 1)
			{
				frmChangeIp frm = new frmChangeIp(ContextGuid, ((Control)lblSetUpIp).Text);
				if ((int)(await frm.ShowAsync((IWin32Window)(object)this)) == 1 && !string.IsNullOrEmpty(frm.Ip))
				{
					((Control)lblSetUpIp).Text = frm.Ip;
					Config.SetUpIp = ((Control)lblSetUpIp).Text;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblSetUpIp_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 391);
		}
	}

	private void picTestUrl_Click(object sender, EventArgs e)
	{
		Process.Start(((Control)lblUrl).Text);
	}

	private void lblUrl_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (!((Control)this).IsDisposed)
			{
				_checkUrlToken?.Cancel();
				_checkUrlToken = new CancellationTokenSource(60000);
				CancellationToken token = _checkUrlToken.Token;
				string str = ((Control)lblUrl).Text;
				Task.Run(() => CheckUrlAsync(str, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblUrl_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\UserControls\\UcServiceManager.cs", 405);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
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
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Expected O, but got Unknown
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Expected O, but got Unknown
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Expected O, but got Unknown
		//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Expected O, but got Unknown
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Expected O, but got Unknown
		//IL_0e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3c: Expected O, but got Unknown
		//IL_0f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1018: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c4: Expected O, but got Unknown
		//IL_10e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_119c: Unknown result type (might be due to invalid IL or missing references)
		//IL_123e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1248: Expected O, but got Unknown
		//IL_1266: Unknown result type (might be due to invalid IL or missing references)
		//IL_1321: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cd: Expected O, but got Unknown
		//IL_13ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1479: Unknown result type (might be due to invalid IL or missing references)
		//IL_1483: Expected O, but got Unknown
		//IL_14b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1583: Unknown result type (might be due to invalid IL or missing references)
		//IL_158d: Expected O, but got Unknown
		//IL_1592: Unknown result type (might be due to invalid IL or missing references)
		grpAddressHub = new GroupBox();
		picTestUrl = new PictureBox();
		ucPortStore1 = new UcPortStore();
		nudPort = new NumericUpDown();
		lbPortServer = new Label();
		lblUrl = new LinkLabel();
		btnSetConnectionString = new Button();
		label1 = new Label();
		lblSetUpIp = new Label();
		lblSetUpIpHeader = new Label();
		label400 = new Label();
		txtConnectionString = new TextBox();
		txtIP = new TextBox();
		pnlService = new Panel();
		picRestart = new PictureBox();
		picStop = new PictureBox();
		lblInstall = new Label();
		lblUninstall = new Label();
		picUninstall = new PictureBox();
		lblRestart = new Label();
		picStart = new PictureBox();
		lblStop = new Label();
		picInstall = new PictureBox();
		lblStart = new Label();
		label2 = new Label();
		((Control)grpAddressHub).SuspendLayout();
		((ISupportInitialize)picTestUrl).BeginInit();
		((ISupportInitialize)nudPort).BeginInit();
		((Control)pnlService).SuspendLayout();
		((ISupportInitialize)picRestart).BeginInit();
		((ISupportInitialize)picStop).BeginInit();
		((ISupportInitialize)picUninstall).BeginInit();
		((ISupportInitialize)picStart).BeginInit();
		((ISupportInitialize)picInstall).BeginInit();
		((Control)this).SuspendLayout();
		((Control)grpAddressHub).Anchor = (AnchorStyles)13;
		((Control)grpAddressHub).Controls.Add((Control)(object)picTestUrl);
		((Control)grpAddressHub).Controls.Add((Control)(object)ucPortStore1);
		((Control)grpAddressHub).Controls.Add((Control)(object)nudPort);
		((Control)grpAddressHub).Controls.Add((Control)(object)lbPortServer);
		((Control)grpAddressHub).Controls.Add((Control)(object)lblUrl);
		((Control)grpAddressHub).Controls.Add((Control)(object)btnSetConnectionString);
		((Control)grpAddressHub).Controls.Add((Control)(object)label1);
		((Control)grpAddressHub).Controls.Add((Control)(object)lblSetUpIp);
		((Control)grpAddressHub).Controls.Add((Control)(object)lblSetUpIpHeader);
		((Control)grpAddressHub).Controls.Add((Control)(object)label400);
		((Control)grpAddressHub).Controls.Add((Control)(object)txtConnectionString);
		((Control)grpAddressHub).Controls.Add((Control)(object)txtIP);
		((Control)grpAddressHub).Location = new Point(2, 47);
		((Control)grpAddressHub).Margin = new Padding(4, 3, 4, 3);
		((Control)grpAddressHub).Name = "grpAddressHub";
		((Control)grpAddressHub).Padding = new Padding(4, 3, 4, 3);
		((Control)grpAddressHub).Size = new Size(502, 170);
		((Control)grpAddressHub).TabIndex = 9;
		grpAddressHub.TabStop = false;
		((Control)picTestUrl).BackColor = Color.Transparent;
		((Control)picTestUrl).Cursor = Cursors.Hand;
		((Control)picTestUrl).Enabled = false;
		picTestUrl.ErrorImage = (Image)(object)FormCustom.Properties.Resources.connection_failed;
		picTestUrl.Image = (Image)(object)FormCustom.Properties.Resources.connetion_error;
		picTestUrl.InitialImage = (Image)(object)FormCustom.Properties.Resources.connection_succsess;
		((Control)picTestUrl).Location = new Point(27, 132);
		((Control)picTestUrl).Name = "picTestUrl";
		((Control)picTestUrl).Size = new Size(33, 29);
		picTestUrl.SizeMode = (PictureBoxSizeMode)1;
		picTestUrl.TabIndex = 1041;
		picTestUrl.TabStop = false;
		((Control)picTestUrl).Click += picTestUrl_Click;
		((Control)ucPortStore1).Anchor = (AnchorStyles)9;
		((Control)ucPortStore1).BackColor = Color.Transparent;
		((Control)ucPortStore1).Location = new Point(289, 64);
		((Control)ucPortStore1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucPortStore1).Name = "ucPortStore1";
		((Control)ucPortStore1).Size = new Size(25, 18);
		((Control)ucPortStore1).TabIndex = 1040;
		((Control)nudPort).Anchor = (AnchorStyles)9;
		((Control)nudPort).Cursor = Cursors.Default;
		((Control)nudPort).Font = new Font("B Yekan", 9.75f);
		((Control)nudPort).Location = new Point(315, 59);
		((Control)nudPort).Margin = new Padding(4, 3, 4, 3);
		nudPort.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		((Control)nudPort).Name = "nudPort";
		((Control)nudPort).Size = new Size(83, 27);
		((Control)nudPort).TabIndex = 1;
		nudPort.Value = new decimal(new int[4] { 1000, 0, 0, 0 });
		nudPort.ValueChanged += nudPort_ValueChanged;
		((Control)lbPortServer).Anchor = (AnchorStyles)9;
		((Control)lbPortServer).AutoSize = true;
		((Control)lbPortServer).BackColor = Color.Transparent;
		((Control)lbPortServer).Font = new Font("B Yekan", 9.75f);
		((Control)lbPortServer).Location = new Point(411, 62);
		((Control)lbPortServer).Margin = new Padding(4, 0, 4, 0);
		((Control)lbPortServer).Name = "lbPortServer";
		((Control)lbPortServer).Size = new Size(80, 20);
		((Control)lbPortServer).TabIndex = 1037;
		((Control)lbPortServer).Text = "[ سرور]Port: ";
		((Control)lblUrl).Anchor = (AnchorStyles)13;
		((Control)lblUrl).Location = new Point(67, 132);
		((Control)lblUrl).Name = "lblUrl";
		((Control)lblUrl).Size = new Size(335, 29);
		((Control)lblUrl).TabIndex = 1039;
		lblUrl.TabStop = true;
		((Control)lblUrl).Text = "---";
		((Label)lblUrl).TextAlign = (ContentAlignment)64;
		lblUrl.LinkClicked += new LinkLabelLinkClickedEventHandler(lblUrl_LinkClicked);
		((Control)lblUrl).TextChanged += lblUrl_TextChanged;
		((Control)btnSetConnectionString).Location = new Point(27, 99);
		((Control)btnSetConnectionString).Name = "btnSetConnectionString";
		((Control)btnSetConnectionString).Size = new Size(33, 27);
		((Control)btnSetConnectionString).TabIndex = 1038;
		((Control)btnSetConnectionString).Text = "...";
		((ButtonBase)btnSetConnectionString).UseVisualStyleBackColor = true;
		((Control)btnSetConnectionString).Click += btnSetConnectionString_Click;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).Font = new Font("B Yekan", 9.75f);
		((Control)label1).Location = new Point(407, 102);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(86, 21);
		((Control)label1).TabIndex = 1034;
		((Control)label1).Text = "[رشته اتصال]:";
		label1.TextAlign = (ContentAlignment)64;
		((Control)lblSetUpIp).Anchor = (AnchorStyles)9;
		((Control)lblSetUpIp).Cursor = Cursors.Hand;
		((Control)lblSetUpIp).Font = new Font("B Yekan", 9.75f);
		((Control)lblSetUpIp).Location = new Point(210, 23);
		((Control)lblSetUpIp).Margin = new Padding(4, 0, 4, 0);
		((Control)lblSetUpIp).Name = "lblSetUpIp";
		((Control)lblSetUpIp).Size = new Size(170, 20);
		((Control)lblSetUpIp).TabIndex = 1034;
		((Control)lblSetUpIp).Text = "---";
		lblSetUpIp.TextAlign = (ContentAlignment)16;
		((Control)lblSetUpIp).Click += lblSetUpIp_Click;
		((Control)lblSetUpIp).MouseEnter += lblSetUpIp_Enter;
		((Control)lblSetUpIp).MouseLeave += lblSetUpIp_Leave;
		((Control)lblSetUpIpHeader).Anchor = (AnchorStyles)9;
		((Control)lblSetUpIpHeader).AutoSize = true;
		((Control)lblSetUpIpHeader).Font = new Font("B Yekan", 9.75f);
		((Control)lblSetUpIpHeader).Location = new Point(388, 23);
		((Control)lblSetUpIpHeader).Margin = new Padding(4, 0, 4, 0);
		((Control)lblSetUpIpHeader).Name = "lblSetUpIpHeader";
		((Control)lblSetUpIpHeader).Size = new Size(105, 20);
		((Control)lblSetUpIpHeader).TabIndex = 1034;
		((Control)lblSetUpIpHeader).Text = "[ نصب سرویس]Ip:";
		lblSetUpIpHeader.TextAlign = (ContentAlignment)64;
		((Control)label400).Anchor = (AnchorStyles)9;
		((Control)label400).AutoSize = true;
		((Control)label400).Font = new Font("B Yekan", 9.75f);
		((Control)label400).Location = new Point(210, 61);
		((Control)label400).Margin = new Padding(4, 0, 4, 0);
		((Control)label400).Name = "label400";
		((Control)label400).Size = new Size(67, 20);
		((Control)label400).TabIndex = 1034;
		((Control)label400).Text = "[ اتصال]Ip:";
		label400.TextAlign = (ContentAlignment)64;
		((Control)txtConnectionString).Anchor = (AnchorStyles)13;
		((Control)txtConnectionString).Font = new Font("B Yekan", 9.75f);
		((Control)txtConnectionString).Location = new Point(67, 99);
		((Control)txtConnectionString).Margin = new Padding(4, 3, 4, 3);
		((Control)txtConnectionString).Name = "txtConnectionString";
		((TextBoxBase)txtConnectionString).ReadOnly = true;
		((Control)txtConnectionString).Size = new Size(335, 27);
		((Control)txtConnectionString).TabIndex = 0;
		txtConnectionString.TextAlign = (HorizontalAlignment)1;
		((Control)txtConnectionString).TextChanged += txtIP_TextChanged_1;
		((Control)txtIP).Anchor = (AnchorStyles)13;
		((Control)txtIP).Font = new Font("B Yekan", 9.75f);
		((Control)txtIP).Location = new Point(27, 59);
		((Control)txtIP).Margin = new Padding(4, 3, 4, 3);
		((Control)txtIP).Name = "txtIP";
		((Control)txtIP).Size = new Size(175, 27);
		((Control)txtIP).TabIndex = 0;
		txtIP.TextAlign = (HorizontalAlignment)1;
		((Control)txtIP).TextChanged += txtIP_TextChanged_1;
		((Control)pnlService).Anchor = (AnchorStyles)15;
		((Control)pnlService).Controls.Add((Control)(object)picRestart);
		((Control)pnlService).Controls.Add((Control)(object)picStop);
		((Control)pnlService).Controls.Add((Control)(object)lblInstall);
		((Control)pnlService).Controls.Add((Control)(object)lblUninstall);
		((Control)pnlService).Controls.Add((Control)(object)picUninstall);
		((Control)pnlService).Controls.Add((Control)(object)lblRestart);
		((Control)pnlService).Controls.Add((Control)(object)picStart);
		((Control)pnlService).Controls.Add((Control)(object)lblStop);
		((Control)pnlService).Controls.Add((Control)(object)picInstall);
		((Control)pnlService).Controls.Add((Control)(object)lblStart);
		((Control)pnlService).Location = new Point(0, 189);
		((Control)pnlService).Margin = new Padding(4, 3, 4, 3);
		((Control)pnlService).Name = "pnlService";
		((Control)pnlService).Size = new Size(504, 183);
		((Control)pnlService).TabIndex = 10;
		((Control)picRestart).Anchor = (AnchorStyles)1;
		((Control)picRestart).Cursor = Cursors.Hand;
		picRestart.Image = (Image)(object)FormCustom.Properties.Resources.WinService_restart;
		((Control)picRestart).Location = new Point(19, 55);
		((Control)picRestart).Margin = new Padding(4, 3, 4, 3);
		((Control)picRestart).Name = "picRestart";
		((Control)picRestart).Size = new Size(86, 65);
		picRestart.SizeMode = (PictureBoxSizeMode)4;
		picRestart.TabIndex = 16;
		picRestart.TabStop = false;
		((Control)picRestart).Click += picRestart_Click;
		((Control)picStop).Anchor = (AnchorStyles)1;
		((Control)picStop).Cursor = Cursors.Hand;
		picStop.Image = (Image)(object)FormCustom.Properties.Resources.WinService_stop;
		((Control)picStop).Location = new Point(117, 55);
		((Control)picStop).Margin = new Padding(4, 3, 4, 3);
		((Control)picStop).Name = "picStop";
		((Control)picStop).Size = new Size(86, 65);
		picStop.SizeMode = (PictureBoxSizeMode)4;
		picStop.TabIndex = 16;
		picStop.TabStop = false;
		((Control)picStop).Click += picStop_Click;
		((Control)lblInstall).Anchor = (AnchorStyles)1;
		((Control)lblInstall).AutoSize = true;
		((Control)lblInstall).Cursor = Cursors.Hand;
		((Control)lblInstall).Font = new Font("B Yekan", 9.75f);
		((Control)lblInstall).Location = new Point(416, 125);
		((Control)lblInstall).Margin = new Padding(4, 0, 4, 0);
		((Control)lblInstall).Name = "lblInstall";
		((Control)lblInstall).Size = new Size(87, 20);
		((Control)lblInstall).TabIndex = 0;
		((Control)lblInstall).Text = "[نصب سرویس]";
		lblInstall.TextAlign = (ContentAlignment)32;
		((Control)lblInstall).Click += picInstall_Click;
		((Control)lblUninstall).Anchor = (AnchorStyles)1;
		((Control)lblUninstall).AutoSize = true;
		((Control)lblUninstall).Cursor = Cursors.Hand;
		((Control)lblUninstall).Font = new Font("B Yekan", 9.75f);
		((Control)lblUninstall).Location = new Point(315, 125);
		((Control)lblUninstall).Margin = new Padding(4, 0, 4, 0);
		((Control)lblUninstall).Name = "lblUninstall";
		((Control)lblUninstall).Size = new Size(89, 20);
		((Control)lblUninstall).TabIndex = 1;
		((Control)lblUninstall).Text = " [حذف سرویس]";
		lblUninstall.TextAlign = (ContentAlignment)32;
		((Control)lblUninstall).Click += picUninstall_Click;
		((Control)picUninstall).Anchor = (AnchorStyles)1;
		((Control)picUninstall).Cursor = Cursors.Hand;
		picUninstall.Image = (Image)(object)FormCustom.Properties.Resources.WinService_Uninstall;
		((Control)picUninstall).Location = new Point(317, 55);
		((Control)picUninstall).Margin = new Padding(4, 3, 4, 3);
		((Control)picUninstall).Name = "picUninstall";
		((Control)picUninstall).Size = new Size(86, 65);
		picUninstall.SizeMode = (PictureBoxSizeMode)4;
		picUninstall.TabIndex = 16;
		picUninstall.TabStop = false;
		((Control)picUninstall).Click += picUninstall_Click;
		((Control)lblRestart).Anchor = (AnchorStyles)1;
		((Control)lblRestart).AutoSize = true;
		((Control)lblRestart).Cursor = Cursors.Hand;
		((Control)lblRestart).Font = new Font("B Yekan", 9.75f);
		((Control)lblRestart).Location = new Point(1, 126);
		((Control)lblRestart).Margin = new Padding(4, 0, 4, 0);
		((Control)lblRestart).Name = "lblRestart";
		((Control)lblRestart).Size = new Size(119, 20);
		((Control)lblRestart).TabIndex = 4;
		((Control)lblRestart).Text = "[اجرای مجدد سرویس]";
		lblRestart.TextAlign = (ContentAlignment)32;
		((Control)lblRestart).Click += picRestart_Click;
		((Control)picStart).Anchor = (AnchorStyles)1;
		((Control)picStart).Cursor = Cursors.Hand;
		picStart.Image = (Image)(object)FormCustom.Properties.Resources.WinService_start;
		((Control)picStart).Location = new Point(217, 55);
		((Control)picStart).Margin = new Padding(4, 3, 4, 3);
		((Control)picStart).Name = "picStart";
		((Control)picStart).Size = new Size(86, 65);
		picStart.SizeMode = (PictureBoxSizeMode)4;
		picStart.TabIndex = 16;
		picStart.TabStop = false;
		((Control)picStart).Click += picStart_Click;
		((Control)lblStop).Anchor = (AnchorStyles)1;
		((Control)lblStop).AutoSize = true;
		((Control)lblStop).Cursor = Cursors.Hand;
		((Control)lblStop).Font = new Font("B Yekan", 9.75f);
		((Control)lblStop).Location = new Point(115, 126);
		((Control)lblStop).Margin = new Padding(4, 0, 4, 0);
		((Control)lblStop).Name = "lblStop";
		((Control)lblStop).Size = new Size(91, 20);
		((Control)lblStop).TabIndex = 3;
		((Control)lblStop).Text = " [توقف سرویس]";
		lblStop.TextAlign = (ContentAlignment)32;
		((Control)lblStop).Click += picStop_Click;
		((Control)picInstall).Anchor = (AnchorStyles)1;
		((Control)picInstall).Cursor = Cursors.Hand;
		picInstall.Image = (Image)(object)FormCustom.Properties.Resources.WinService_install;
		((Control)picInstall).Location = new Point(415, 55);
		((Control)picInstall).Margin = new Padding(4, 3, 4, 3);
		((Control)picInstall).Name = "picInstall";
		((Control)picInstall).Size = new Size(86, 65);
		picInstall.SizeMode = (PictureBoxSizeMode)4;
		picInstall.TabIndex = 16;
		picInstall.TabStop = false;
		((Control)picInstall).Click += picInstall_Click;
		((Control)lblStart).Anchor = (AnchorStyles)1;
		((Control)lblStart).AutoSize = true;
		((Control)lblStart).Cursor = Cursors.Hand;
		((Control)lblStart).Font = new Font("B Yekan", 9.75f);
		((Control)lblStart).Location = new Point(216, 126);
		((Control)lblStart).Margin = new Padding(4, 0, 4, 0);
		((Control)lblStart).Name = "lblStart";
		((Control)lblStart).Size = new Size(91, 20);
		((Control)lblStart).TabIndex = 2;
		((Control)lblStart).Text = "[اجرای سرویس]";
		lblStart.TextAlign = (ContentAlignment)32;
		((Control)lblStart).Click += picStart_Click;
		((Control)label2).Anchor = (AnchorStyles)13;
		((Control)label2).Font = new Font("B Yekan", 9.75f);
		((Control)label2).ForeColor = Color.Red;
		((Control)label2).Location = new Point(18, -3);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).RightToLeft = (RightToLeft)0;
		((Control)label2).Size = new Size(476, 52);
		((Control)label2).TabIndex = 1035;
		((Control)label2).Text = "[کاربر گرامی ، در صورتی که نرم افزار جاری به عنوان کلاینت استفاده می شود اکیدا از نصب سرویس خودداری کرده و کافی است مشخصات سرور را وارد کنید]";
		label2.TextAlign = (ContentAlignment)64;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)grpAddressHub);
		((Control)this).Controls.Add((Control)(object)pnlService);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcServiceManager";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(507, 374);
		((Control)grpAddressHub).ResumeLayout(false);
		((Control)grpAddressHub).PerformLayout();
		((ISupportInitialize)picTestUrl).EndInit();
		((ISupportInitialize)nudPort).EndInit();
		((Control)pnlService).ResumeLayout(false);
		((Control)pnlService).PerformLayout();
		((ISupportInitialize)picRestart).EndInit();
		((ISupportInitialize)picStop).EndInit();
		((ISupportInitialize)picUninstall).EndInit();
		((ISupportInitialize)picStart).EndInit();
		((ISupportInitialize)picInstall).EndInit();
		((Control)this).ResumeLayout(false);
	}

	bool INpDisposable.get_IsDisposed()
	{
		return ((Control)this).IsDisposed;
	}
}
