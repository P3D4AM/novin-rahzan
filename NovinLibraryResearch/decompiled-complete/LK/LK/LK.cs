using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Resources;
using TINYLib;

namespace LK;

public class LK
{
	private static long _SerialNumber = 0L;

	private static DateTime _lastRequest = DateTime.Now.AddMinutes(-100.0);

	public static long GetSerialNumber(ILkUiFactory owner, Package package)
	{
		try
		{
			if (_SerialNumber <= 0 && DateTime.Now.AddMinutes(-5.0) > _lastRequest)
			{
				_lastRequest = DateTime.Now;
				Demo.IsDemo(owner, package, ignoreHttpExceptions: true);
			}
			return _SerialNumber;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 30);
			return 0L;
		}
	}

	public static ReturnedSaveFuncInfo SetSerialNumber(long value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (value <= 0)
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "value <= 0", "", "SetSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 41);
				return returnedSaveFuncInfo;
			}
			CLSRegistry.WriteLKForErrorHandler("LKSerial", value.ToString());
			if (value.ToString().Length != 12 && value != 1234567890)
			{
				_SerialNumber = 0L;
				returnedSaveFuncInfo.AddError(EnSystem.None, "value.ToString().Length != 12 && value != 1234567890", "", "SetSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 48);
				return returnedSaveFuncInfo;
			}
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 52);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetLkSerial(value, package), ReturnedState.None, "SetSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 54);
			CLSRegistry.SetRegObject(value2, package);
			_SerialNumber = value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 60);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetSerialNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 61);
		}
		return returnedSaveFuncInfo;
	}

	private static void CreateOcxAndRegister(byte[] data, string name, string dirPath)
	{
		try
		{
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			name = Path.Combine(dirPath, name);
			try
			{
				File.WriteAllBytes(name, data);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CreateOcxAndRegister", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 72);
			}
			Interaction.Shell("regsvr32.exe \"" + name + "\"", (AppWinStyle)0, false, 60000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CreateOcxAndRegister", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 75);
		}
	}

	public static void CheckOCX(ILkUiFactory owner, Package package, string path)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (CheckTiny(owner, package))
			{
				return;
			}
			if (!File.Exists(Path.Combine(path, "notRegisterTiny.txt")))
			{
				CreateOcxAndRegister(global::Resources.Resources.AccTinyX64, "AccTiny.ocx", Path.Combine(path, "x64"));
				CreateOcxAndRegister(global::Resources.Resources.AccTinyX86, "AccTiny.ocx", Path.Combine(path, "x86"));
			}
			UpdateNpManifest(path);
			try
			{
				if (Environment.Is64BitProcess)
				{
					CreateOcxAndRegister(global::Resources.Resources.AccTinyX64, "AccTiny.ocx", path);
				}
				else
				{
					CreateOcxAndRegister(global::Resources.Resources.AccTinyX86, "AccTiny.ocx", path);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckOCX", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 97);
			}
			if (!File.Exists(path + "\\AccTiny.ocx"))
			{
				MessageBox.Show((IWin32Window)owner, Convert.ToString("خطادر شناسایی قفل سخت افزاری کد 110\r\nفایل AccTiny.ocxدر محل اجرای برنامه موجود نمی باشد"), "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
			}
			try
			{
				Interaction.Shell("bcdedit /set nointegritychecks ON", (AppWinStyle)0, false, 60000);
			}
			catch
			{
			}
			try
			{
				Interaction.Shell("bcdedit /set loadoptions DDISABLE_INTEGRITY_CHECKS", (AppWinStyle)0, false, 60000);
			}
			catch
			{
			}
			try
			{
				File.WriteAllText(path + "\\64bitTiny.txt", Convert.ToString("bcdedit /set nointegritychecks ON\r\nbcdedit /set loadoptions DDISABLE_INTEGRITY_CHECKS\r\n" + Information.Err().Description));
			}
			catch
			{
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckOCX", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 105);
		}
	}

	private static void UpdateNpManifest(string path)
	{
		try
		{
			string text = Process.GetCurrentProcess().ProcessName.ToLower().Replace(".exe", "");
			File.Exists(Path.Combine(path, text + ".exe.manifest"));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateNpManifest", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 118);
		}
	}

	private static bool CheckTiny(ILkUiFactory owner, Package package)
	{
		try
		{
			_ = (Tiny)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0CA5A93D-F6BD-4398-809A-9F767276EF77")));
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckTiny", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\LK.cs", 130);
			if (package != Package.Accounting && package != Package.baje)
			{
				Demo.SetTinyErrCode("Tiny Not Register(culd not create instance of tiny)//" + ex.Message, owner, package);
			}
			return false;
		}
	}
}
