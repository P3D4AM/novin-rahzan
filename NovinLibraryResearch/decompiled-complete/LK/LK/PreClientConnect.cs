using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Entities;

namespace LK;

public class PreClientConnect
{
	public static string _IP;

	public static short _Port;

	private static SemaphoreSlim _connectSemaphore = new SemaphoreSlim(1);

	private static string GetIpFileName(Package package)
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

	internal static async Task<bool> ConnectAsync(string contextGuid, ILkUiFactory owner, Package package, CancellationToken token)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			DialogResult res = (DialogResult)0;
			LoadData(package);
			if (!parsIP() || _Port <= 0)
			{
				((Control)(Form)owner).BeginInvoke((Delegate)(MethodInvoker)delegate
				{
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Expected O, but got Unknown
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Unknown result type (might be due to invalid IL or missing references)
					ILkUiFactory lkUiFactory3 = owner.NewFrmLkClientSetting(contextGuid, package);
					Client.notvalid = false;
					res = ((Form)lkUiFactory3).ShowDialog((IWin32Window)owner);
				});
				return ((int)res == 1) & !Client.notvalid;
			}
			if (await Client.ConnectAsync(package, _IP, _Port, PreCon: true))
			{
				return !Client.notvalid;
			}
			((Control)(Form)owner).Invoke((Delegate)(MethodInvoker)delegate
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Expected O, but got Unknown
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				ILkUiFactory lkUiFactory2 = owner.NewFrmNotConnect(contextGuid, _IP, _Port, Client.errorText);
				res = ((Form)lkUiFactory2).ShowDialog((IWin32Window)(Form)owner);
			});
			if ((int)res != 1)
			{
				if ((int)res == 2)
				{
					Environment.Exit(0);
					Application.ExitThread();
					return false;
				}
				((Control)(Form)owner).Invoke((Delegate)(MethodInvoker)delegate
				{
					//IL_001f: Unknown result type (might be due to invalid IL or missing references)
					//IL_002a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Expected O, but got Unknown
					//IL_002f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Unknown result type (might be due to invalid IL or missing references)
					ILkUiFactory lkUiFactory = owner.NewFrmLkClientSetting(contextGuid, package, "  آدرس و درگاه سرور را در قسمت ذیل وارد کنید");
					res = ((Form)lkUiFactory).ShowDialog((IWin32Window)(Form)owner);
				});
				return ((int)res == 1) & !Client.notvalid;
			}
			await ConnectAsync(contextGuid, owner, package, token);
			return false;
		}
		catch (OperationCanceledException)
		{
			return false;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\PreClientConnect.cs", 81);
			return false;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectSemaphore?.Release();
			}
		}
	}

	private static bool parsIP()
	{
		try
		{
			Regex regex = new Regex("((([2][5][0-5])|([2][0-4][0-9])|([1][0-9][0-9])|([0-9][0-9])|([0-9]))[.]){3}((([2][5][0-5])|([2][0-4][0-9])|([1][0-9][0-9])|([0-9][0-9])|([0-9])))");
			Match match = null;
			match = regex.Match(_IP);
			if (match.Success && match.Value.ToString().Length == _IP.Length)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "parsIP", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\PreClientConnect.cs", 101);
			return false;
		}
	}

	private static void LoadData(Package package)
	{
		try
		{
			string text = "";
			if (File.Exists(GetIpFileName(package)))
			{
				text = File.ReadAllText(GetIpFileName(package));
				if (!string.IsNullOrEmpty(text))
				{
					_IP = text.Substring(0, text.IndexOf("@"));
					_Port = text.Substring(text.IndexOf("@") + 1, text.IndexOf("$") - text.IndexOf("@") - 1).ParseToShort(0);
					return;
				}
			}
			_IP = "";
			_Port = 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\PreClientConnect.cs", 123);
		}
	}
}
