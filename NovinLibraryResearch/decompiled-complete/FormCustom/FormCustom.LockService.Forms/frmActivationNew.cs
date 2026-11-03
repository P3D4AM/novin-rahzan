using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.LockService;
using FormCustom.LockService.Class;
using FormCustom.Properties;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.LkService;

namespace FormCustom.LockService.Forms;

public class frmActivationNew : AccountingBaseForm
{
	private bool _isDisposed;

	private bool timer1_Tick_x = true;

	private CancellationTokenSource _tryToActivateToken;

	internal Label Label3;

	internal TextBox TXTActive;

	internal Button BTNOK;

	internal GroupBox GroupBox1;

	internal Label Label4;

	internal Label Label9;

	internal Label Label12;

	internal Label Label10;

	internal Label LBLSamane;

	internal Label LBLCode;

	internal Timer Timer1;

	internal PictureBox Pbox_ActivationCode;

	private IContainer components;

	internal Button btnActiveBySite;

	internal Panel Panel4;

	private LinkLabel linkLabel1;

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

	public string SoftwareSerial { get; private set; }

	private static string VersionAccounting
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(AppContext.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	private static string VersionScale
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(AppContext.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	private static string VersionHozor
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(AppContext.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	public new long LkSerial
	{
		get
		{
			if (PackageClass != null)
			{
				return PackageClass.LkSerial;
			}
			return PackageClassAtt.LkSerial;
		}
	}

	public Package Package { get; private set; }

	public AttendancePackageClass PackageClassAtt { get; private set; }

	public AccountingPackageClass PackageClass { get; private set; }

	public LockInformation LockInformation { get; private set; }

	public bool IsDemo { get; private set; }

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_tryToActivateToken?.Cancel();
				_tryToActivateToken?.Dispose();
				_tryToActivateToken = null;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 85);
		}
	}

	private async Task<ReturnedSaveFuncInfo> TryToActivate(long lk, string soft, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 6;
			try
			{
				if (lk == 0L)
				{
					result = ret.AddError("سریال قفل تشخیص داده نشد.".GetTranslate(), "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 92);
					goto IL_07a3;
				}
				((Form)(object)this).CheckVisibility("TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs");
				LockClientSetting setting = LockClientSetting.GetSetting();
				if (setting.LkSerial != lk)
				{
					setting.LkSerial = lk;
					await LockClientSetting.SetSettingAsync(setting);
				}
				ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = await LkClient.GetLkAsync(lk, ignoreHttpExceptions, token);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 103);
				if (((returnedSaveFuncInfoWithValue?.value)?.TafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					ret.AddError("خطا در برقراری ارتباط با سرور".GetTranslate(), "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 107);
					result = ret;
					goto IL_07a3;
				}
				ReturnedSaveFuncInfoWithValue<ActivationPackage> returnedSaveFuncInfoWithValue2 = ((Package == Package.Accounting) ? (await ActivationPackage.GetFactoryAsync(base.ContextGuid, VersionAccounting, PackageClass, lk, Package, soft, ignoreHttpExceptions, token)) : ((Package != Package.HoghoghVaDastmozd) ? (await ActivationPackage.GetFactoryAsync(base.ContextGuid, VersionScale, PackageClass, lk, Package, soft, ignoreHttpExceptions, token)) : (await ActivationPackage.GetFactoryAsync(base.ContextGuid, VersionHozor, PackageClass, lk, Package, soft, ignoreHttpExceptions, token))));
				LkActivationCodeModel model = new LkActivationCodeModel
				{
					LkSerial = lk,
					SerialSoft = soft,
					UserName = "novinSepanta1402",
					Password = "dfg36@$%^&dsfsdft"
				};
				string activationCode = "";
				if (Package == Package.HoghoghVaDastmozd)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await returnedSaveFuncInfoWithValue2.value.HoghoghVaDastmozdActivationAsync(activationCode, token), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 127);
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await returnedSaveFuncInfoWithValue2.value.AccountingActivationAsync(model, activationCode, null, token), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 129);
				}
			}
			catch (WebException exception)
			{
				ret.AddReturnedValue(exception, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 131);
			}
			catch (HttpListenerException exception2)
			{
				ret.AddReturnedValue(exception2, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 132);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 135);
				if (!IsDisposed)
				{
					MessageBox.Show((IWin32Window)(object)this, ex.Message + "\r\n" + ret.GetErrorMessage(), "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				}
				ret.AddReturnedValue(ex, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 138);
			}
		}
		finally
		{
			if (ret.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(ret, "پیغام سیستم", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs");
			}
			if (ret.HasInformation)
			{
				await this.ShowReturnedSavedFuncInfoAsync(ret, "پیغام سیستم", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs");
			}
		}
		return ret;
		IL_07a3:
		return result;
	}

	public frmActivationNew(string contextGuid, AccountingPackageClass cls, Package package, bool isDemo)
		: base(contextGuid)
	{
		if (cls == null)
		{
			package = Package.HoghoghVaDastmozd;
		}
		try
		{
			InitializeComponent();
			Package = package;
			PackageClass = cls;
			IsDemo = isDemo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 159);
		}
	}

	public frmActivationNew(string contextGuid, AttendancePackageClass cls, Package package)
		: base(contextGuid)
	{
		if (cls == null)
		{
			package = Package.HoghoghVaDastmozd;
		}
		try
		{
			InitializeComponent();
			Package = package;
			PackageClassAtt = cls;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 170);
		}
	}

	public void TXTActive_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				BTNOK.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTActive_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 179);
		}
	}

	public void FRMActivation_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMActivation_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 189);
		}
	}

	public async void FRMActivation_Load(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			LockInformation = (await NpLockHubInstances.Instance.Environment.GetLockInformationAsync(Package, default(CancellationToken))).value;
			switch (Package)
			{
			case Package.HoghoghVaDastmozd:
			case Package.TamdideHozor:
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await NpLockHubInstances.Instance.SoftwareSerialManager.GetSoftwareSerial_HozorAsync(VersionHozor, default(CancellationToken));
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 203);
				SoftwareSerial = returnedSaveFuncInfoWithValue.value?.Replace("-", "");
				if (Package == Package.HoghoghVaDastmozd)
				{
					((Control)this).Text = "فعال سازی سیستم حقوق و دستمزد".GetTranslate();
				}
				else
				{
					((Control)this).Text = "تمدید نسخه آزمایشی سیستم حضورغیاب".GetTranslate();
				}
				break;
			}
			case Package.TamdideAccounting:
			case Package.Accounting:
				SoftwareSerial = PackageClass.SoftwareSerial?.Replace("-", "");
				if (Package == Package.TamdideAccounting)
				{
					((Control)this).Text = "تمدید نسخه آزمایشی نرم افزار حسابداری".GetTranslate();
				}
				else
				{
					((Control)this).Text = "فعال سازی نرم افزارحسابداری ".GetTranslate();
				}
				break;
			default:
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
				return;
			}
			if ((Package == Package.None) & (string.IsNullOrEmpty(LkSerial.ToString()) || LkSerial == 1234567890))
			{
				((Control)LBLCode).Text = "1234567890" + SoftwareSerial;
			}
			else if (LkSerial == 1234567890)
			{
				((Control)LBLCode).Text = "1234567890" + SoftwareSerial;
			}
			else
			{
				((Control)LBLCode).Text = LkSerial + SoftwareSerial;
			}
			if ((Package != Package.TamdideAccounting) & (Package != Package.TamdideHozor) & (Package != Package.TamdideScale))
			{
				if ((int)MessageBox.Show((IWin32Window)(object)this, " مایل به فعال سازی اتوماتیک نرم افزار از طریق اینترنت هستید؟ ".GetTranslate(), "پیغام سیستم".GetTranslate(), (MessageBoxButtons)4, (MessageBoxIcon)32, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288) == 7)
				{
					return;
				}
				((Control)BTNOK).Enabled = false;
				_tryToActivateToken?.Cancel();
				_tryToActivateToken = new CancellationTokenSource(180000);
				CancellationToken token = _tryToActivateToken.Token;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await TryToActivate(LkSerial, SoftwareSerial, ignoreHttpExceptions: false, token), ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 233);
				if (ret.HasError)
				{
					((Form)this).DialogResult = (DialogResult)7;
					((Form)this).Close();
					return;
				}
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
			((Control)BTNOK).Enabled = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 250);
			ret.AddReturnedValue(ex, "", "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 251);
		}
	}

	public async void BTNOK_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 16;
			try
			{
				((Control)BTNOK).Enabled = false;
				ReturnedSaveFuncInfoWithValue<string> activationCode = await NpLockHubInstances.Instance.CheckActivation.SetRemoveDaysAsync(((Control)TXTActive).Text, Package, default(CancellationToken));
				ret.AddReturnedValue(activationCode, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 261);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.SetLkSerialAsync(VersionAccounting, Package, LkSerial, IsDemo, default(CancellationToken)), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 262);
				ReturnedSaveFuncInfoWithValue<ActivationPackage> obj = await ActivationPackage.GetFactoryAsync(base.ContextGuid, VersionAccounting, PackageClass, LkSerial, Package, SoftwareSerial, ignoreHttpExceptions: false, default(CancellationToken));
				if (Package == Package.Accounting)
				{
					ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue = await NpLockHubInstances.Instance.CheckActivation.GetApplicationTypeAsync(SoftwareSerial, LkSerial.ToString(), activationCode.value, default(CancellationToken));
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 268);
					if (returnedSaveFuncInfoWithValue.value == Level_Accounting.none)
					{
						await this.ShowErrorAsync("کد فعال سازی وارد شده معتبر نمیباشد . ".GetTranslate());
						goto end_IL_00ca;
					}
					frmActivationPackages frm = new frmActivationPackages(base.ContextGuid);
					((Form)(object)this).CheckVisibility("BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs");
					if ((int)(await frm.ShowAsync((IWin32Window)(object)this)) == 1)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await obj.value.AccountingActivationAsync(null, activationCode.value, frm.Packages, default(CancellationToken)), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 276);
						((Form)this).DialogResult = (DialogResult)1;
						((Form)this).Close();
					}
				}
				else if (Package == Package.HoghoghVaDastmozd)
				{
					ReturnedSaveFuncInfoWithValue<string> level1 = await NpLockHubInstances.Instance.ActivationCode.GetAttendanceActivationCodeAsync(1, SoftwareSerial, LkSerial.ToString(), HozoorPackage.General, 0, 0, DateTime.Now, default(CancellationToken));
					ReturnedSaveFuncInfoWithValue<string> level2 = await NpLockHubInstances.Instance.ActivationCode.GetAttendanceActivationCodeAsync(2, SoftwareSerial, LkSerial.ToString(), HozoorPackage.General, 0, 0, DateTime.Now, default(CancellationToken));
					ReturnedSaveFuncInfoWithValue<string> level3 = await NpLockHubInstances.Instance.ActivationCode.GetAttendanceActivationCodeAsync(3, SoftwareSerial, LkSerial.ToString(), HozoorPackage.General, 0, 0, DateTime.Now, default(CancellationToken));
					ReturnedSaveFuncInfoWithValue<string> level4 = await NpLockHubInstances.Instance.ActivationCode.GetAttendanceActivationCodeAsync(4, SoftwareSerial, LkSerial.ToString(), HozoorPackage.General, 0, 0, DateTime.Now, default(CancellationToken));
					ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await NpLockHubInstances.Instance.ActivationCode.GetAttendanceActivationCodeAsync(5, SoftwareSerial, LkSerial.ToString(), HozoorPackage.General, 0, 0, DateTime.Now, default(CancellationToken));
					if (level1.value == activationCode.value || level2.value == activationCode.value || level3.value == activationCode.value || level4.value == activationCode.value || returnedSaveFuncInfoWithValue2.value == activationCode.value)
					{
						await obj.value.HoghoghVaDastmozdActivationAsync(((Control)TXTActive).Text.RemoveNoNumbers(), default(CancellationToken));
					}
				}
				else if (Package == Package.TamdideAccounting)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj.value.TamdideAccountingActivationAsync(((Control)TXTActive).Text.RemoveNoNumbers(), LockInformation, default(CancellationToken)), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 299);
				}
				else if (Package == Package.TamdideHozor)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj.value.TamdideHozorActivationAsync(((Control)TXTActive).Text.RemoveNoNumbers(), LockInformation, default(CancellationToken)), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 301);
				}
				else
				{
					if (Package != Package.TamdideScale)
					{
						await this.ShowErrorAsync("کد فعال سازی وارد شده معتبر نمیباشد".GetTranslate());
						LockServiceContainter.RunProcess(LkSerial + SoftwareSerial);
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
						goto end_IL_00ca;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj.value.TamdideScaleActivationAsync(((Control)TXTActive).Text.RemoveNoNumbers(), LockInformation, default(CancellationToken)), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 303);
				}
				goto end_IL_007a;
				end_IL_00ca:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 316);
				ret.AddReturnedValue(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 317);
				goto end_IL_007a;
			}
			end_IL_007a:;
		}
		finally
		{
			((Control)BTNOK).Enabled = true;
			if (ret.HasError)
			{
				await this.ShowInvokedReturnedSavedFuncInfoAsync(ret, "پیغام سیستم", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs");
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
			}
			else if (ret.HasInformation)
			{
				await this.ShowInvokedReturnedSavedFuncInfoAsync(ret, "پیغام سیستم", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs");
				((Form)this).DialogResult = (DialogResult)1;
				((Form)this).Close();
			}
		}
	}

	public void Timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (timer1_Tick_x)
			{
				timer1_Tick_x = false;
				((Control)LBLCode).ForeColor = Color.Red;
				((Control)LBLSamane).ForeColor = Color.DarkBlue;
			}
			else
			{
				timer1_Tick_x = true;
				((Control)LBLCode).ForeColor = Color.DarkRed;
				((Control)LBLSamane).ForeColor = Color.Blue;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer1_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 353);
		}
	}

	public void LBLCode_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(((Control)LBLCode).Text);
	}

	public void FRMActivation_FormClosed(object sender, FormClosedEventArgs e)
	{
		Timer1.Enabled = false;
		((Component)(object)this).Dispose(disposing: true);
	}

	private void btnActiveBySite_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = "https://crm.novinp.ir/extend/code/" + ((Control)LBLCode).Text,
				UseShellExecute = true
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnActiveBySite_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 371);
		}
	}

	public void TXTActive_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_ActivationCode).Visible = true;
			if (((Control)TXTActive).Text.Length != 14)
			{
				Pbox_ActivationCode.Image = (Image)(object)FormCustom.Properties.Resources.stop;
			}
			else
			{
				Pbox_ActivationCode.Image = (Image)(object)FormCustom.Properties.Resources.accept;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTActive_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmActivationNew.cs", 383);
		}
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Expected O, but got Unknown
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Expected O, but got Unknown
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Expected O, but got Unknown
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Expected O, but got Unknown
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Expected O, but got Unknown
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Expected O, but got Unknown
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Expected O, but got Unknown
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca8: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmActivationNew));
		Label3 = new Label();
		TXTActive = new TextBox();
		BTNOK = new Button();
		GroupBox1 = new GroupBox();
		linkLabel1 = new LinkLabel();
		btnActiveBySite = new Button();
		Pbox_ActivationCode = new PictureBox();
		Panel4 = new Panel();
		LBLSamane = new Label();
		LBLCode = new Label();
		Label12 = new Label();
		Label4 = new Label();
		Label9 = new Label();
		Label10 = new Label();
		Timer1 = new Timer(components);
		((Control)GroupBox1).SuspendLayout();
		((ISupportInitialize)Pbox_ActivationCode).BeginInit();
		((Control)Panel4).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)Label3).Anchor = (AnchorStyles)10;
		((Control)Label3).AutoSize = true;
		((Control)Label3).BackColor = Color.Transparent;
		((Control)Label3).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label3).Location = new Point(477, 249);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(145, 23);
		((Control)Label3).TabIndex = 2;
		((Control)Label3).Text = "[کد فعال سازی را وارد کنید]:";
		Label3.TextAlign = (ContentAlignment)64;
		((Control)TXTActive).Anchor = (AnchorStyles)14;
		((Control)TXTActive).Font = new Font("Arial", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)TXTActive).Location = new Point(46, 283);
		((Control)TXTActive).Margin = new Padding(3, 4, 3, 4);
		((Control)TXTActive).Name = "TXTActive";
		((Control)TXTActive).Size = new Size(572, 32);
		((Control)TXTActive).TabIndex = 0;
		TXTActive.TextAlign = (HorizontalAlignment)1;
		((Control)TXTActive).TextChanged += TXTActive_TextChanged;
		((Control)TXTActive).KeyDown += new KeyEventHandler(TXTActive_KeyDown);
		((Control)BTNOK).Anchor = (AnchorStyles)14;
		((Control)BTNOK).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((ButtonBase)BTNOK).Image = (Image)componentResourceManager.GetObject("BTNOK.Image");
		((ButtonBase)BTNOK).ImageAlign = (ContentAlignment)64;
		((Control)BTNOK).Location = new Point(16, 386);
		((Control)BTNOK).Margin = new Padding(3, 5, 3, 5);
		((Control)BTNOK).Name = "BTNOK";
		((Control)BTNOK).Size = new Size(601, 50);
		((Control)BTNOK).TabIndex = 6;
		((Control)BTNOK).TabStop = false;
		((Control)BTNOK).Text = "[تـایید]";
		((ButtonBase)BTNOK).UseVisualStyleBackColor = true;
		((Control)BTNOK).Click += BTNOK_Click;
		((Control)GroupBox1).Anchor = (AnchorStyles)15;
		((Control)GroupBox1).Controls.Add((Control)(object)linkLabel1);
		((Control)GroupBox1).Controls.Add((Control)(object)btnActiveBySite);
		((Control)GroupBox1).Controls.Add((Control)(object)Pbox_ActivationCode);
		((Control)GroupBox1).Controls.Add((Control)(object)Panel4);
		((Control)GroupBox1).Controls.Add((Control)(object)Label3);
		((Control)GroupBox1).Controls.Add((Control)(object)TXTActive);
		((Control)GroupBox1).Controls.Add((Control)(object)Label12);
		((Control)GroupBox1).Controls.Add((Control)(object)Label4);
		((Control)GroupBox1).Controls.Add((Control)(object)Label9);
		((Control)GroupBox1).Controls.Add((Control)(object)Label10);
		((Control)GroupBox1).Location = new Point(3, -6);
		((Control)GroupBox1).Margin = new Padding(3, 4, 3, 4);
		((Control)GroupBox1).Name = "GroupBox1";
		((Control)GroupBox1).Padding = new Padding(3, 4, 3, 4);
		((Control)GroupBox1).Size = new Size(631, 383);
		((Control)GroupBox1).TabIndex = 8;
		GroupBox1.TabStop = false;
		((Control)linkLabel1).AutoSize = true;
		((Control)linkLabel1).Font = new Font("B Titr", 9.75f, (FontStyle)1);
		((Control)linkLabel1).Location = new Point(13, 348);
		((Control)linkLabel1).Name = "linkLabel1";
		((Control)linkLabel1).RightToLeft = (RightToLeft)0;
		((Control)linkLabel1).Size = new Size(142, 23);
		((Control)linkLabel1).TabIndex = 16;
		linkLabel1.TabStop = true;
		((Control)linkLabel1).Text = "www.NovinP.com";
		((Label)linkLabel1).TextAlign = (ContentAlignment)16;
		((Control)btnActiveBySite).Anchor = (AnchorStyles)14;
		((Control)btnActiveBySite).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((ButtonBase)btnActiveBySite).ImageAlign = (ContentAlignment)64;
		((Control)btnActiveBySite).Location = new Point(182, 199);
		((Control)btnActiveBySite).Margin = new Padding(3, 5, 3, 5);
		((Control)btnActiveBySite).Name = "btnActiveBySite";
		((Control)btnActiveBySite).Size = new Size(251, 43);
		((Control)btnActiveBySite).TabIndex = 15;
		((Control)btnActiveBySite).TabStop = false;
		((Control)btnActiveBySite).Text = "[دریافت کد فعال سازی از طریق سایت]";
		((ButtonBase)btnActiveBySite).UseVisualStyleBackColor = true;
		((Control)btnActiveBySite).Click += btnActiveBySite_Click;
		((Control)Pbox_ActivationCode).Anchor = (AnchorStyles)6;
		((Control)Pbox_ActivationCode).Location = new Point(9, 283);
		((Control)Pbox_ActivationCode).Margin = new Padding(0, 0, 0, 0);
		((Control)Pbox_ActivationCode).Name = "Pbox_ActivationCode";
		((Control)Pbox_ActivationCode).Size = new Size(33, 32);
		Pbox_ActivationCode.TabIndex = 14;
		Pbox_ActivationCode.TabStop = false;
		((Control)Pbox_ActivationCode).Visible = false;
		((Control)Panel4).Anchor = (AnchorStyles)13;
		Panel4.BorderStyle = (BorderStyle)1;
		((Control)Panel4).Controls.Add((Control)(object)LBLSamane);
		((Control)Panel4).Controls.Add((Control)(object)LBLCode);
		((Control)Panel4).Location = new Point(13, 59);
		((Control)Panel4).Margin = new Padding(3, 4, 3, 4);
		((Control)Panel4).Name = "Panel4";
		((Control)Panel4).Size = new Size(605, 132);
		((Control)Panel4).TabIndex = 13;
		((Control)LBLSamane).Anchor = (AnchorStyles)13;
		((Control)LBLSamane).BackColor = Color.Transparent;
		((Control)LBLSamane).Font = new Font("B Titr", 24f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLSamane).ForeColor = Color.Blue;
		((Control)LBLSamane).Location = new Point(-1, 75);
		((Control)LBLSamane).Name = "LBLSamane";
		((Control)LBLSamane).Size = new Size(605, 47);
		((Control)LBLSamane).TabIndex = 7;
		((Control)LBLSamane).Text = "سامانه پیامک] : 30006154]";
		LBLSamane.TextAlign = (ContentAlignment)32;
		((Control)LBLCode).Anchor = (AnchorStyles)13;
		((Control)LBLCode).BackColor = Color.Transparent;
		((Control)LBLCode).Font = new Font("B Titr", 24f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)LBLCode).ForeColor = Color.Red;
		((Control)LBLCode).Location = new Point(-1, 0);
		((Control)LBLCode).Name = "LBLCode";
		((Control)LBLCode).Size = new Size(605, 75);
		((Control)LBLCode).TabIndex = 10;
		((Control)LBLCode).Text = "1234567890";
		LBLCode.TextAlign = (ContentAlignment)32;
		((Control)LBLCode).Click += LBLCode_Click;
		((Control)Label12).Anchor = (AnchorStyles)13;
		((Control)Label12).BackColor = Color.Transparent;
		((Control)Label12).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label12).ForeColor = Color.Red;
		((Control)Label12).Location = new Point(39, 17);
		((Control)Label12).Name = "Label12";
		((Control)Label12).Size = new Size(556, 29);
		((Control)Label12).TabIndex = 7;
		((Control)Label12).Text = "[جهت دریافت کد فعال سازی ، کد ذیل را به سامانه پیامک هوشمند 24 ساعته ارسال نمایید] .";
		Label12.TextAlign = (ContentAlignment)32;
		((Control)Label4).Anchor = (AnchorStyles)10;
		((Control)Label4).BackColor = Color.Transparent;
		((Control)Label4).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label4).Location = new Point(379, 343);
		((Control)Label4).Name = "Label4";
		((Control)Label4).RightToLeft = (RightToLeft)0;
		((Control)Label4).Size = new Size(166, 23);
		((Control)Label4).TabIndex = 6;
		((Control)Label4).Text = "05137597590-05191007590";
		Label4.TextAlign = (ContentAlignment)64;
		((Control)Label9).Anchor = (AnchorStyles)10;
		((Control)Label9).BackColor = Color.Transparent;
		((Control)Label9).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)Label9).Location = new Point(545, 343);
		((Control)Label9).Name = "Label9";
		((Control)Label9).Size = new Size(77, 23);
		((Control)Label9).TabIndex = 7;
		((Control)Label9).Text = "[پشتیبانی فنی] :";
		Label9.TextAlign = (ContentAlignment)64;
		((Control)Label10).Anchor = (AnchorStyles)14;
		((Control)Label10).BackColor = Color.Transparent;
		((Control)Label10).Font = new Font("B Titr", 14.25f, (FontStyle)3, (GraphicsUnit)3, (byte)178);
		((Control)Label10).ForeColor = Color.Blue;
		((Control)Label10).Location = new Point(70, 305);
		((Control)Label10).Margin = new Padding(10, 0, 10, 0);
		((Control)Label10).Name = "Label10";
		((Control)Label10).Size = new Size(496, 33);
		((Control)Label10).TabIndex = 8;
		((Control)Label10).Text = "____________________________________";
		Label10.TextAlign = (ContentAlignment)32;
		Timer1.Enabled = true;
		Timer1.Interval = 1000;
		Timer1.Tick += Timer1_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 17f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(635, 442);
		((Control)this).Controls.Add((Control)(object)GroupBox1);
		((Control)this).Controls.Add((Control)(object)BTNOK);
		((Control)this).Font = new Font("B Titr", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).Margin = new Padding(4);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmActivationNew";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فعالسازی]";
		((Form)this).FormClosed += new FormClosedEventHandler(FRMActivation_FormClosed);
		((Form)this).Load += FRMActivation_Load;
		((Control)this).KeyDown += new KeyEventHandler(FRMActivation_KeyDown);
		((Control)GroupBox1).ResumeLayout(false);
		((Control)GroupBox1).PerformLayout();
		((ISupportInitialize)Pbox_ActivationCode).EndInit();
		((Control)Panel4).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
