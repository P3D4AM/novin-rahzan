using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using LK;
using PacketParser;
using PacketParser.Assistance.MultiLingual;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using SharedActivation.LKLogics;

namespace FormCustom.Froms.LkManage;

public class FRMActivation : LkUiFactory
{
	private string softwareSerial;

	private string lkSerial;

	private bool _isDisposed;

	private Package _Package;

	public int Counter;

	private bool Timer1_Tick_x = true;

	private CancellationTokenSource _tryToActivateToken;

	private Container components;

	internal Label Label1;

	internal Label Label2;

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

	internal Panel Panel1;

	internal Panel Panel4;

	internal Panel Panel3;

	internal Panel Panel2;

	internal Label Label5;

	internal Timer Timer1;

	internal PictureBox Pbox_ActivationCode;

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

	private async Task<ReturnedSaveFuncInfo> TryToActivate(long lk, string soft, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 1;
		try
		{
			Console.WriteLine($"{DateTime.Now})try to activate started...");
			LkActivationCodeModel model = new LkActivationCodeModel
			{
				LkSerial = lk,
				SerialSoft = soft,
				UserName = "novinSepanta1402",
				Password = "dfg36@$%^&dsfsdft"
			};
			line = 2;
			line = 3;
			if (lk == 0L)
			{
				return ret.AddError(EnSystem.None, "سریال قفل تشخیص داده نشد.", "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 40);
			}
			line = 4;
			((Form)(object)this).CheckVisibility("TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs");
			if ((int)MessageBox.Show((IWin32Window)(object)this, "مایل به فعال سازی اتوماتیک نرم افزار" + GetPackageName(_Package) + "از طریق اینترنت هستید؟", "پیغام سیستم", (MessageBoxButtons)4, (MessageBoxIcon)32, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288) == 7)
			{
				return ret.AddError(EnSystem.None, "کاربر به درخواست فعالسازی اینترنتی پاسخ خیر داده است. به همین دلیل فعالسازی انجام نشد.", "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 47);
			}
			line = 5;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckExpireLock(lk, ignoreHttpExceptions, token), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 49);
			line = 6;
			ret.AddReturnedValue(CLSActivation.SetLKSerial(_Package, lk, ignoreHttpExceptions: true), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 51);
			line = 7;
			string activationCode;
			if (_Package == Package.HoghoghVaDastmozd)
			{
				line = 8;
				ret.AddReturnedValue(SoftwareSerialManager.SetSoftwareSerial_Hozor(soft), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 57);
				line = 9;
				activationCode = await LkClient.ActivationCodePacageAsync(model, Package.HoghoghVaDastmozd);
				if (IsDisposed)
				{
					return ret.AddError(EnSystem.None, "frmActivation has been disposed!", "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 60);
				}
				line = 9;
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = CLSActivation.RemoveDays(activationCode, _Package);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 64);
				activationCode = returnedSaveFuncInfoWithValue.value;
				line = 11;
				ret.AddReturnedValue(CLSActivation.SetHoghoghActivationCode(activationCode, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 67);
				line = 12;
				ReturnedSaveFuncInfoWithValue<Level_Hozor> returnedSaveFuncInfoWithValue2 = CLSActivation.ApplicationHozorType(soft, lk.ToString(), activationCode, this, ignoreHttpExceptions: true);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 70);
				switch (returnedSaveFuncInfoWithValue2.value)
				{
				case Level_Hozor.Level1:
					line = 13;
					ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 75);
					MessageBox.Show((IWin32Window)(object)this, "نرم افزار حضور، غیاب و حقوق، دستمزد(سطح1) با موفقیت فعال گردید", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return ret;
				case Level_Hozor.Level2:
					line = 14;
					ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 82);
					MessageBox.Show((IWin32Window)(object)this, "نرم افزار حضور، غیاب و حقوق، دستمزد(سطح2) با موفقیت فعال گردید", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return ret;
				case Level_Hozor.Level3:
					line = 15;
					ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 89);
					MessageBox.Show((IWin32Window)(object)this, "نرم افزار حضور، غیاب و حقوق، دستمزد(سطح3) با موفقیت فعال گردید", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return ret;
				case Level_Hozor.Level4:
					line = 16;
					ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 96);
					MessageBox.Show((IWin32Window)(object)this, "نرم افزار حضور، غیاب و حقوق، دستمزد(سطح4) با موفقیت فعال گردید", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return ret;
				case Level_Hozor.Level5:
					line = 17;
					ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 103);
					MessageBox.Show((IWin32Window)(object)this, "نرم افزار حضور، غیاب و حقوق، دستمزد(سطح5) با موفقیت فعال گردید", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return ret;
				default:
					line = 18;
					MessageBox.Show((IWin32Window)(object)this, "خطا در فعالسازی اینترنتی نرم افزار حقوق و دستمزد\r\n" + ret.GetErrorMessage(), "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					return ret.AddError(EnSystem.None, "خطا در فعالسازی اینترنتی نرم افزار حضورغیاب\r\n" + activationCode, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 114);
				}
			}
			line = 53;
			ret.AddReturnedValue(SoftwareSerialManager.SetSoftwareSerial_Accounting(soft), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 270);
			line = 54;
			activationCode = await LkClient.ActivationCode(model);
			if (activationCode.RemoveNoNumbers().ToString() != activationCode)
			{
				ret.AddError(activationCode, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 273);
			}
			line = 55;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue3 = CLSActivation.RemoveDays(activationCode, _Package);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 277);
			_ = returnedSaveFuncInfoWithValue3.value;
			line = 56;
			line = 57;
			line = 60;
			string text = "";
			string text2 = "";
			string text3 = "";
			line = 61;
			line = 64;
			ReturnedSaveFuncInfoWithValue<string> accountingActivationCode = CLSActivation.GetAccountingActivationCode();
			ret.AddReturnedValue(accountingActivationCode, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 327);
			string value = accountingActivationCode.value;
			ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue4 = CLSActivation.ApplicationType(soft, lk.ToString(), value, this);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue4, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 330);
			if (returnedSaveFuncInfoWithValue4.value == Level_Accounting.ForoshgahiSade)
			{
				line = 65;
				ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 334);
				MessageBox.Show((IWin32Window)(object)this, "نرم افزار حسابداری نسخه ساده ".GetTranslate() + text + text2 + text3 + "با موفقیت فعالسازی شد".GetTranslate(), "پیغام سیستم".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				return ret;
			}
			returnedSaveFuncInfoWithValue4 = CLSActivation.ApplicationType(soft, lk.ToString(), value, this);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue4, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 343);
			if (returnedSaveFuncInfoWithValue4.value == Level_Accounting.ForoshgahiMotavaset)
			{
				line = 66;
				ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 347);
				MessageBox.Show((IWin32Window)(object)this, "نرم افزار حسابداری نسخه متوسط ".GetTranslate() + text + text2 + text3 + "با موفقیت فعالسازی شد".GetTranslate(), "پیغام سیستم".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				return ret;
			}
			returnedSaveFuncInfoWithValue4 = CLSActivation.ApplicationType(soft, lk.ToString(), value, this);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue4, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 356);
			if (returnedSaveFuncInfoWithValue4.value == Level_Accounting.ForoshgahiPishrafteh)
			{
				line = 670;
				ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 360);
				line = 671;
				if (IsDisposed)
				{
					return ret;
				}
				MessageBox.Show((IWin32Window)(object)this, "نرم افزار حسابداری نسخه پیشرفته ".GetTranslate() + text + text2 + text3 + "با موفقیت فعالسازی شد".GetTranslate(), "پیغام سیستم".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				line = 672;
				return ret;
			}
			returnedSaveFuncInfoWithValue4 = CLSActivation.ApplicationType(soft, lk.ToString(), value, this);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue4, ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 372);
			if (returnedSaveFuncInfoWithValue4.value == Level_Accounting.Jame)
			{
				line = 68;
				ret.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 376);
				if (!IsDisposed)
				{
					MessageBox.Show((IWin32Window)(object)this, "نرم افزار حسابداری نسخه جامع ".GetTranslate() + text + text2 + text3 + "با موفقیت فعالسازی شد".GetTranslate(), "پیغام سیستم".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				}
				return ret;
			}
			line = 69;
			if (!IsDisposed)
			{
				ret.AddError(EnSystem.None, "خطا در فعالسازی اینترنتی نرم افزار حسابداری".GetTranslate() + " \r\n " + ret.GetErrorMessage(), "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 387);
			}
		}
		catch (WebException exception)
		{
			ret.AddReturnedValue(exception, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 389);
		}
		catch (HttpListenerException exception2)
		{
			ret.AddReturnedValue(exception2, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 390);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//IsDisposed:{IsDisposed}//lk:{lk}//soft:{soft}", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 393);
			if (!IsDisposed)
			{
				MessageBox.Show((IWin32Window)(object)this, ex.Message + "\r\n" + ret.GetErrorMessage(), "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			}
			ret.AddReturnedValue(ex, "", "TryToActivate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 396);
		}
		finally
		{
			Console.WriteLine($"{DateTime.Now})try to activate finished.\r\n" + $"Result:{!ret.HasError}");
		}
		return ret;
	}

	internal string GetPackageName(Package packageName)
	{
		try
		{
			return packageName switch
			{
				Package.Accounting => "حسابداری  ", 
				Package.HoghoghVaDastmozd => "  حقوق و دستمزد ", 
				Package.TamdideAccounting => "  حسابداری ", 
				Package.TamdideScale => " ترازو ", 
				Package.TamdideHozor => "حضور و غیاب   ", 
				_ => "", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPackageName", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 427);
			return "";
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckExpireLock(long lkSerial, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		short line = 0;
		try
		{
			Console.WriteLine($"{DateTime.Now})CheckExpireLock...");
			line = 2;
			new LKInfo();
			line = 3;
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = await LkClient.GetLkAsync(lkSerial, ignoreHttpExceptions, token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 443);
			}
			if (!res.HasError)
			{
				LKInfo lKInfo = returnedSaveFuncInfoWithValue.value;
				line = 4;
				if (lKInfo == null)
				{
					lKInfo = new LKInfo();
					lKInfo.ExpireDate = DateTime.Now.AddDays(-365.0);
				}
				DateTime dateTime = Convert.ToDateTime(lKInfo?.ExpireDate);
				line = 10;
				if (dateTime < DateTime.Now)
				{
					line = 11;
					res.AddError(EnSystem.None, "اعتبار قفل سخت افزاری شما در تاریخ".GetTranslate() + lKInfo.ExpireDate.MiladiToCalture() + "\r\n" + "به پایان رسیده است.".GetTranslate() + "\r\n" + $"{lKInfo.LKSerial}\r\n" + "لطفا نسبت به تمدید قفل اقدام فرمائید".GetTranslate(), "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 458);
					line = 12;
					ServerTCP.StopServer();
					line = 13;
				}
				else if (lKInfo.IsFailed)
				{
					res.AddError(EnSystem.None, "قفل سخت افزاری شما مجاز به استفاده از نرم افزار نمی باشد".GetTranslate() + "\r\n" + "لطفا با پشتیبانی تماس حاصل فرمائید".GetTranslate() + "\r\n", "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 468);
					line = 14;
					ServerTCP.StopServer();
				}
			}
		}
		catch (WebException exception)
		{
			res.AddReturnedValue(exception, "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 475);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 478);
			res.AddReturnedValue(ex, "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 479);
		}
		finally
		{
			Console.WriteLine($"{DateTime.Now})CheckExpireLock finished.\r\nResult:{!res.HasError}");
		}
		return res;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 493);
		}
	}

	public FRMActivation(string contextGuid, Package package)
		: base(contextGuid)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			InitializeComponent();
			_Package = package;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 506);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 507);
		}
		finally
		{
			if (returnedSaveFuncInfo.HasError)
			{
				Application.Run((Form)(object)new FrmShowErrorMessage(base.ContextGuid, returnedSaveFuncInfo, "خطا در بارگذاری صفحه فعالسازی".GetTranslate(), (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs"));
			}
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
			WebErrorLog.Instance.StartLog(ex, "", "TXTActive_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 522);
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
			WebErrorLog.Instance.StartLog(ex, "", "FRMActivation_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 533);
		}
	}

	public async void FRMActivation_Load(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				if (_Package == Package.Accounting)
				{
					ReturnedSaveFuncInfoWithValue<string> softwareSerial_Accounting = SoftwareSerialManager.GetSoftwareSerial_Accounting();
					ret.AddReturnedValue(softwareSerial_Accounting, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 543);
					softwareSerial = softwareSerial_Accounting.value.Replace("-", "");
					((Control)this).Text = "فعال سازی نرم افزارحسابداری ".GetTranslate();
					goto IL_0221;
				}
				if (_Package == Package.TamdideAccounting)
				{
					ReturnedSaveFuncInfoWithValue<string> softwareSerial_Accounting2 = SoftwareSerialManager.GetSoftwareSerial_Accounting();
					ret.AddReturnedValue(softwareSerial_Accounting2, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 550);
					softwareSerial = softwareSerial_Accounting2.value.Replace("-", "");
					((Control)this).Text = "تمدید نسخه آزمایشی نرم افزار حسابداری".GetTranslate();
					goto IL_0221;
				}
				if (_Package == Package.HoghoghVaDastmozd)
				{
					ReturnedSaveFuncInfoWithValue<string> softwareSerial_Hozor = SoftwareSerialManager.GetSoftwareSerial_Hozor();
					ret.AddReturnedValue(softwareSerial_Hozor, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 557);
					softwareSerial = softwareSerial_Hozor.value.Replace("-", "");
					((Control)this).Text = "فعال سازی سیستم حقوق و دستمزد".GetTranslate();
					goto IL_0221;
				}
				if (_Package == Package.TamdideHozor)
				{
					ReturnedSaveFuncInfoWithValue<string> softwareSerial_Hozor2 = SoftwareSerialManager.GetSoftwareSerial_Hozor();
					ret.AddReturnedValue(softwareSerial_Hozor2, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 564);
					softwareSerial = softwareSerial_Hozor2.value.Replace("-", "");
					((Control)this).Text = "تمدید نسخه آزمایشی سیستم حضورغیاب".GetTranslate();
					goto IL_0221;
				}
				if (_Package == Package.TamdideScale)
				{
					ReturnedSaveFuncInfoWithValue<string> softwareSerial_Scale = SoftwareSerialManager.GetSoftwareSerial_Scale();
					ret.AddReturnedValue(softwareSerial_Scale, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 571);
					softwareSerial = softwareSerial_Scale.value.Replace("-", "");
					((Control)this).Text = "تمدید نسخه آزمایشی زیر بسته ارتباط با ترازوهای دیجیتال".GetTranslate();
					goto IL_0221;
				}
				((Form)this).DialogResult = (DialogResult)2;
				((Form)this).Close();
				goto end_IL_003b;
				IL_0221:
				ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = Demo.IsDemo(this, _Package, ignoreHttpExceptions: false);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 583);
				if (returnedSaveFuncInfoWithValue.value)
				{
					lkSerial = Demo.LKSerial;
				}
				else
				{
					if (global::LK.LK.GetSerialNumber(this, _Package) == 0L)
					{
						Demo.IsDemo(this, _Package, ignoreHttpExceptions: false);
					}
					lkSerial = global::LK.LK.GetSerialNumber(this, _Package).ToString();
				}
				if ((_Package == Package.None) & (lkSerial == "" || lkSerial == "1234567890"))
				{
					((Control)LBLCode).Text = "1234567890" + softwareSerial;
				}
				else if (lkSerial == "1234567890")
				{
					((Control)LBLCode).Text = "1234567890" + softwareSerial;
				}
				else
				{
					((Control)LBLCode).Text = lkSerial + softwareSerial;
				}
				if (!((_Package != Package.TamdideAccounting) & (_Package != Package.TamdideHozor) & (_Package != Package.TamdideScale)))
				{
					return;
				}
				_tryToActivateToken?.Cancel();
				_tryToActivateToken = new CancellationTokenSource(180000);
				CancellationToken token = _tryToActivateToken.Token;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await TryToActivate(lkSerial.ParseToLong(0L), softwareSerial, ignoreHttpExceptions: false, token), ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 605);
				if (!ret.HasError)
				{
					Package package = _Package;
					if ((package == Package.HoghoghVaDastmozd || package == Package.Accounting) ? true : false)
					{
						ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(_Package);
						ret.AddReturnedValue(regObject, ReturnedState.None, "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 611);
						RegistryObject value = regObject.value;
						value.ListOfClient = new List<Guid>();
						CLSRegistry.SetRegObject(value, _Package);
					}
					((Form)this).DialogResult = (DialogResult)1;
					((Form)this).Close();
				}
				goto end_IL_0032;
				end_IL_003b:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 623);
				ret.AddReturnedValue(ex, "", "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 624);
				goto end_IL_0032;
			}
			end_IL_0032:;
		}
		finally
		{
			if (!IsDisposed && ret.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(ret, "خطا در فعالسازی نرم افزار", "FRMActivation_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs");
			}
		}
	}

	public async void BTNOK_Click(object sender, EventArgs e)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			try
			{
				string text = softwareSerial;
				string text2 = lkSerial;
				Counter++;
				if (_Package == Package.Accounting)
				{
					ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = CLSActivation.RemoveDays(((Control)TXTActive).Text, _Package);
					returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 643);
					ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue2 = CLSActivation.ApplicationType(softwareSerial, lkSerial, returnedSaveFuncInfoWithValue.value, this);
					returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 646);
					if (returnedSaveFuncInfoWithValue2.value != 0)
					{
						returnedSaveFuncInfo.AddReturnedValue(CLSActivation.SetLKSerial(_Package, long.Parse(lkSerial), ignoreHttpExceptions: true), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 649);
						returnedSaveFuncInfo.AddReturnedValue(SoftwareSerialManager.SetSoftwareSerial_Accounting(softwareSerial), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 650);
						returnedSaveFuncInfo.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 652);
						MessageBox.Show((IWin32Window)(object)this, "نرم افزار حسابداری با موفقیت فعال گردید".GetTranslate(), "پیغام سیستم".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
						ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(_Package);
						returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 656);
						RegistryObject value = regObject.value;
						value.ListOfClient = new List<Guid>();
						CLSRegistry.SetRegObject(value, _Package);
						returnedSaveFuncInfo.AddReturnedValue(CLSRegistry.SaveObject(_Package), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 661);
						((Form)this).DialogResult = (DialogResult)1;
						FRM_Activation_Barcod_Tolid_Special fRM_Activation_Barcod_Tolid_Special = new FRM_Activation_Barcod_Tolid_Special(base.ContextGuid);
						string text3 = "";
						((Form)(object)this).CheckVisibility("BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs");
						((Form)fRM_Activation_Barcod_Tolid_Special).ShowDialog((IWin32Window)(object)this);
						_ = 1;
						if (!string.IsNullOrEmpty(text3))
						{
							MessageBox.Show((IWin32Window)(object)this, text3, "پیغام سیستم".GetTranslate(), (MessageBoxButtons)0);
						}
						((Form)this).Close();
					}
					else
					{
						MessageBox.Show((IWin32Window)(object)this, "کد فعال سازی وارد شده معتبر نمیباشد . ".GetTranslate(), "خطا".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					}
				}
				else if (_Package == Package.HoghoghVaDastmozd)
				{
					ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue3 = CLSActivation.RemoveDays(((Control)TXTActive).Text, _Package);
					returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 781);
					string text4 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level1(text, text2, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
					string text5 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level2(text, text2, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
					string text6 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level3(text, text2, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
					string text7 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level4(text, text2, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
					string text8 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level5(text, text2, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
					if (text4 == returnedSaveFuncInfoWithValue3.value || text5 == returnedSaveFuncInfoWithValue3.value || text6 == returnedSaveFuncInfoWithValue3.value || text7 == returnedSaveFuncInfoWithValue3.value || text8 == returnedSaveFuncInfoWithValue3.value)
					{
						returnedSaveFuncInfo.AddReturnedValue(CLSActivation.SetLKSerial(_Package, long.Parse(lkSerial), ignoreHttpExceptions: false), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 794);
						returnedSaveFuncInfo.AddReturnedValue(SoftwareSerialManager.SetSoftwareSerial_Hozor(softwareSerial), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 795);
						returnedSaveFuncInfo.AddReturnedValue(CLSActivation.SetHoghoghActivationCode(returnedSaveFuncInfoWithValue3.value, _Package), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 796);
						returnedSaveFuncInfo.AddReturnedValue(CLSActivation.SetActiveDate(DateTime.Now, _Package), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 797);
						MessageBox.Show((IWin32Window)(object)this, "بسته نرم افزاری حقوق و دستمزد با موفقیت فعال گردید".GetTranslate(), "پیغام سیستم".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)64, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
						ReturnedSaveFuncInfoWithValue<RegistryObject> regObject2 = CLSRegistry.GetRegObject(_Package);
						returnedSaveFuncInfo.AddReturnedValue(regObject2, ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 801);
						RegistryObject value2 = regObject2.value;
						value2.ListOfClient = new List<Guid>();
						CLSRegistry.SetRegObject(value2, _Package);
						returnedSaveFuncInfo.AddReturnedValue(CLSRegistry.SaveObject(_Package), ReturnedState.None, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 806);
						((Form)this).DialogResult = (DialogResult)1;
						((Form)this).Close();
					}
					else
					{
						MessageBox.Show((IWin32Window)(object)this, "کد فعال سازی وارد شده معتبر نمیباشد".GetTranslate(), "خطا".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
						((Form)this).DialogResult = (DialogResult)2;
						((Form)this).Close();
					}
				}
				else
				{
					MessageBox.Show((IWin32Window)(object)this, "کد فعال سازی وارد شده معتبر نمیباشد".GetTranslate(), "خطا".GetTranslate(), (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
					CheckActivation.RunProcess(lkSerial + softwareSerial);
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 982);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 983);
				goto end_IL_0028;
			}
			end_IL_0028:;
		}
		finally
		{
			if (returnedSaveFuncInfo.HasError)
			{
				if (!IsDisposed)
				{
					await this.ShowReturnedSavedFuncInfoAsync(returnedSaveFuncInfo, "خطا در فعالسازی".GetTranslate(), "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs");
				}
				else
				{
					Application.Run((Form)(object)new FrmShowErrorMessage(base.ContextGuid, returnedSaveFuncInfo, "خطا در فعالسازی".GetTranslate(), (MessageBoxButtons)0, isAskQuestion: false, LanguageTypes.Farsi, "BTNOK_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs"));
				}
			}
		}
	}

	public void Timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (Timer1_Tick_x)
			{
				Timer1_Tick_x = false;
				((Control)LBLCode).ForeColor = Color.Red;
				((Control)LBLSamane).ForeColor = Color.DarkBlue;
			}
			else
			{
				Timer1_Tick_x = true;
				((Control)LBLCode).ForeColor = Color.DarkRed;
				((Control)LBLSamane).ForeColor = Color.Blue;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Timer1_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 1014);
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

	public void TXTActive_TextChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)Pbox_ActivationCode).Visible = true;
			_ = _Package;
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
			WebErrorLog.Instance.StartLog(ex, "", "TXTActive_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\LkManage\\FRMActivation.cs", 1046);
		}
	}

	[DebuggerStepThrough]
	private void InitializeComponent()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Expected O, but got Unknown
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Expected O, but got Unknown
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Expected O, but got Unknown
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Expected O, but got Unknown
		//IL_0bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Expected O, but got Unknown
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Expected O, but got Unknown
		components = new Container();
		((Control)this).KeyDown += new KeyEventHandler(FRMActivation_KeyDown);
		((Form)this).Load += FRMActivation_Load;
		((Form)this).FormClosed += new FormClosedEventHandler(FRMActivation_FormClosed);
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FRMActivation));
		Label1 = new Label();
		Label2 = new Label();
		Label3 = new Label();
		TXTActive = new TextBox();
		((Control)TXTActive).KeyDown += new KeyEventHandler(TXTActive_KeyDown);
		((Control)TXTActive).TextChanged += TXTActive_TextChanged;
		BTNOK = new Button();
		((Control)BTNOK).Click += BTNOK_Click;
		GroupBox1 = new GroupBox();
		Panel4 = new Panel();
		Panel3 = new Panel();
		Panel2 = new Panel();
		LBLCode = new Label();
		((Control)LBLCode).Click += LBLCode_Click;
		LBLSamane = new Label();
		Label12 = new Label();
		Label4 = new Label();
		Label9 = new Label();
		Label10 = new Label();
		Label5 = new Label();
		Panel1 = new Panel();
		Timer1 = new Timer((IContainer)components);
		Timer1.Tick += Timer1_Tick;
		Pbox_ActivationCode = new PictureBox();
		((Control)GroupBox1).SuspendLayout();
		((Control)Panel4).SuspendLayout();
		((Control)Panel3).SuspendLayout();
		((Control)Panel2).SuspendLayout();
		((Control)Panel1).SuspendLayout();
		((ISupportInitialize)Pbox_ActivationCode).BeginInit();
		((Control)this).SuspendLayout();
		((Control)Label1).Anchor = (AnchorStyles)9;
		((Control)Label1).BackColor = Color.Transparent;
		((Control)Label1).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label1).Location = new Point(168, 19);
		((Control)Label1).Name = "Label1";
		((Control)Label1).Size = new Size(91, 27);
		((Control)Label1).TabIndex = 0;
		((Control)Label1).Text = "[سریال قفل] :";
		Label1.TextAlign = (ContentAlignment)64;
		((Control)Label1).Visible = false;
		((Control)Label2).Anchor = (AnchorStyles)9;
		((Control)Label2).BackColor = Color.Transparent;
		((Control)Label2).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label2).Location = new Point(168, 55);
		((Control)Label2).Name = "Label2";
		((Control)Label2).Size = new Size(91, 27);
		((Control)Label2).TabIndex = 1;
		((Control)Label2).Text = "[سریال نرم افزار] :";
		Label2.TextAlign = (ContentAlignment)64;
		((Control)Label2).Visible = false;
		((Control)Label3).Anchor = (AnchorStyles)10;
		((Control)Label3).BackColor = Color.Transparent;
		((Control)Label3).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label3).Location = new Point(472, 284);
		((Control)Label3).Name = "Label3";
		((Control)Label3).Size = new Size(91, 32);
		((Control)Label3).TabIndex = 2;
		((Control)Label3).Text = "[کد فعال سازی] :";
		Label3.TextAlign = (ContentAlignment)64;
		((Control)TXTActive).Anchor = (AnchorStyles)14;
		((Control)TXTActive).Font = new Font("Arial", 15.75f, (FontStyle)0, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)TXTActive).Location = new Point(39, 319);
		((Control)TXTActive).Name = "TXTActive";
		((Control)TXTActive).Size = new Size(519, 32);
		((Control)TXTActive).TabIndex = 0;
		TXTActive.TextAlign = (HorizontalAlignment)1;
		((Control)BTNOK).Anchor = (AnchorStyles)14;
		((Control)BTNOK).Font = new Font("B Titr", 12f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((ButtonBase)BTNOK).Image = (Image)componentResourceManager.GetObject("BTNOK.Image");
		((ButtonBase)BTNOK).ImageAlign = (ContentAlignment)64;
		((Control)BTNOK).Location = new Point(11, 422);
		((Control)BTNOK).Margin = new Padding(3, 4, 3, 4);
		((Control)BTNOK).Name = "BTNOK";
		((Control)BTNOK).Size = new Size(567, 38);
		((Control)BTNOK).TabIndex = 6;
		((Control)BTNOK).TabStop = false;
		((Control)BTNOK).Text = "[تـایید]";
		((ButtonBase)BTNOK).UseVisualStyleBackColor = true;
		((Control)GroupBox1).Anchor = (AnchorStyles)15;
		((Control)GroupBox1).Controls.Add((Control)(object)Pbox_ActivationCode);
		((Control)GroupBox1).Controls.Add((Control)(object)Panel4);
		((Control)GroupBox1).Controls.Add((Control)(object)Label3);
		((Control)GroupBox1).Controls.Add((Control)(object)TXTActive);
		((Control)GroupBox1).Controls.Add((Control)(object)Label12);
		((Control)GroupBox1).Controls.Add((Control)(object)Label4);
		((Control)GroupBox1).Controls.Add((Control)(object)Label9);
		((Control)GroupBox1).Controls.Add((Control)(object)Label10);
		((Control)GroupBox1).Controls.Add((Control)(object)Label5);
		((Control)GroupBox1).Location = new Point(12, 12);
		((Control)GroupBox1).Name = "GroupBox1";
		((Control)GroupBox1).Size = new Size(566, 403);
		((Control)GroupBox1).TabIndex = 8;
		GroupBox1.TabStop = false;
		((Control)Panel4).Anchor = (AnchorStyles)15;
		Panel4.BorderStyle = (BorderStyle)1;
		((Control)Panel4).Controls.Add((Control)(object)Panel3);
		((Control)Panel4).Location = new Point(6, 101);
		((Control)Panel4).Name = "Panel4";
		((Control)Panel4).Size = new Size(554, 180);
		((Control)Panel4).TabIndex = 13;
		Panel3.BorderStyle = (BorderStyle)1;
		((Control)Panel3).Controls.Add((Control)(object)Panel2);
		((Control)Panel3).Dock = (DockStyle)5;
		((Control)Panel3).Location = new Point(0, 0);
		((Control)Panel3).Name = "Panel3";
		((Control)Panel3).Size = new Size(552, 178);
		((Control)Panel3).TabIndex = 12;
		Panel2.BorderStyle = (BorderStyle)1;
		((Control)Panel2).Controls.Add((Control)(object)LBLCode);
		((Control)Panel2).Controls.Add((Control)(object)LBLSamane);
		((Control)Panel2).Dock = (DockStyle)5;
		((Control)Panel2).Location = new Point(0, 0);
		((Control)Panel2).Name = "Panel2";
		((Control)Panel2).Size = new Size(550, 176);
		((Control)Panel2).TabIndex = 11;
		((Control)LBLCode).Anchor = (AnchorStyles)13;
		((Control)LBLCode).BackColor = Color.Transparent;
		((Control)LBLCode).Font = new Font("B Titr", 24f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)LBLCode).ForeColor = Color.Red;
		((Control)LBLCode).Location = new Point(-1, 0);
		((Control)LBLCode).Name = "LBLCode";
		((Control)LBLCode).Size = new Size(544, 112);
		((Control)LBLCode).TabIndex = 10;
		((Control)LBLCode).Text = "1234567890";
		LBLCode.TextAlign = (ContentAlignment)32;
		((Control)LBLSamane).Anchor = (AnchorStyles)13;
		((Control)LBLSamane).BackColor = Color.Transparent;
		((Control)LBLSamane).Font = new Font("B Titr", 24f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)LBLSamane).ForeColor = Color.Blue;
		((Control)LBLSamane).Location = new Point(1, 112);
		((Control)LBLSamane).Name = "LBLSamane";
		((Control)LBLSamane).Size = new Size(544, 51);
		((Control)LBLSamane).TabIndex = 7;
		((Control)LBLSamane).Text = "سامانه پیامک] : 30006154]";
		LBLSamane.TextAlign = (ContentAlignment)32;
		((Control)Label12).Anchor = (AnchorStyles)13;
		((Control)Label12).BackColor = Color.Transparent;
		((Control)Label12).Font = new Font("B Titr", 14.25f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label12).ForeColor = Color.Red;
		((Control)Label12).Location = new Point(8, 16);
		((Control)Label12).Name = "Label12";
		((Control)Label12).Size = new Size(552, 88);
		((Control)Label12).TabIndex = 7;
		((Control)Label12).Text = "[جهت دریافت کد فعال سازی ، کد ذیل را به سامانه پیامک هوشمند 24 ساعته ارسال نمایید ].";
		Label12.TextAlign = (ContentAlignment)32;
		((Control)Label4).Anchor = (AnchorStyles)10;
		((Control)Label4).BackColor = Color.Transparent;
		((Control)Label4).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label4).Location = new Point(290, 371);
		((Control)Label4).Name = "Label4";
		((Control)Label4).RightToLeft = (RightToLeft)0;
		((Control)Label4).Size = new Size(180, 25);
		((Control)Label4).TabIndex = 6;
		((Control)Label4).Text = ResourceManagement.TextBrandResourceManagement.Tel;
		Label4.TextAlign = (ContentAlignment)16;
		((Control)Label9).Anchor = (AnchorStyles)10;
		((Control)Label9).BackColor = Color.Transparent;
		((Control)Label9).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label9).Location = new Point(466, 371);
		((Control)Label9).Name = "Label9";
		((Control)Label9).Size = new Size(185, 25);
		((Control)Label9).TabIndex = 7;
		((Control)Label9).Text = "[پشتیبانی فنی] :";
		Label9.TextAlign = (ContentAlignment)64;
		((Control)Label10).Anchor = (AnchorStyles)14;
		((Control)Label10).BackColor = Color.Transparent;
		((Control)Label10).Font = new Font("B Titr", 14.25f, (FontStyle)3, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label10).ForeColor = Color.Blue;
		((Control)Label10).Location = new Point(0, 337);
		((Control)Label10).Name = "Label10";
		((Control)Label10).Size = new Size(555, 30);
		((Control)Label10).TabIndex = 8;
		((Control)Label10).Text = "___________________________________";
		Label10.TextAlign = (ContentAlignment)32;
		((Control)Label5).Anchor = (AnchorStyles)6;
		((Control)Label5).BackColor = Color.Transparent;
		((Control)Label5).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, Convert.ToByte(178));
		((Control)Label5).Location = new Point(7, 371);
		((Control)Label5).Name = "Label5";
		((Control)Label5).RightToLeft = (RightToLeft)0;
		((Control)Label5).Size = new Size(180, 25);
		((Control)Label5).TabIndex = 6;
		((Control)Label5).Text = ResourceManagement.TextBrandResourceManagement.Url1;
		Label5.TextAlign = (ContentAlignment)16;
		((Control)Panel1).Controls.Add((Control)(object)Label1);
		((Control)Panel1).Controls.Add((Control)(object)Label2);
		((Control)Panel1).Location = new Point(-2, -1);
		((Control)Panel1).Name = "Panel1";
		((Control)Panel1).Size = new Size(200, 100);
		((Control)Panel1).TabIndex = 12;
		((Control)Panel1).Visible = false;
		Timer1.Enabled = true;
		Timer1.Interval = 1000;
		((Control)Pbox_ActivationCode).Location = new Point(6, 319);
		((Control)Pbox_ActivationCode).Name = "Pbox_ActivationCode";
		((Control)Pbox_ActivationCode).Size = new Size(34, 32);
		Pbox_ActivationCode.TabIndex = 14;
		Pbox_ActivationCode.TabStop = false;
		((Control)Pbox_ActivationCode).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(590, 473);
		((Control)this).Controls.Add((Control)(object)GroupBox1);
		((Control)this).Controls.Add((Control)(object)Panel1);
		((Control)this).Controls.Add((Control)(object)BTNOK);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FRMActivation";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "[فعالسازی]";
		((Control)GroupBox1).ResumeLayout(false);
		((Control)GroupBox1).PerformLayout();
		((Control)Panel4).ResumeLayout(false);
		((Control)Panel3).ResumeLayout(false);
		((Control)Panel2).ResumeLayout(false);
		((Control)Panel1).ResumeLayout(false);
		((Control)Panel1).PerformLayout();
		((ISupportInitialize)Pbox_ActivationCode).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
