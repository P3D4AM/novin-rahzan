using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace LK;

public class Client
{
	private static string Xpass = "NovinPHTToozandehjani";

	private static string Ypass = "NovinP123456789HT";

	public static string ActiveCode = "";

	public static string SoftSerial = "";

	private static long _lkSerial = 0L;

	public static string ScaleNo = "";

	public static string BarcodeActiveCode = "";

	public static string TolidActiveCode = "";

	public static string SpecialActiveCode = "";

	public static bool Showmessage;

	public static string errorText = "";

	public static bool notvalid;

	public static TaskCompletionSource<bool> NetworkLockTask = new TaskCompletionSource<bool>();

	public static long LkSerial
	{
		get
		{
			return _lkSerial;
		}
		set
		{
			_lkSerial = value;
			CLSRegistry.WriteLKForErrorHandler("LKSerial", value.ToString());
		}
	}

	public static async Task<bool> ConnectAsync(Package package, string ip, short port, bool PreCon = false)
	{
		notvalid = false;
		TcpClient clientSocket = new TcpClient();
		try
		{
			NetworkLockTask?.TrySetCanceled();
			NetworkLockTask = new TaskCompletionSource<bool>();
			string SendMessage = package.ToString() + "@" + ReturnId(package).value + "@" + DateTime.Now.ToString() + DateTime.Now.Millisecond;
			SendMessage = SendMessage.FixString();
			string temp = SendMessage;
			NetworkStream Stream;
			try
			{
				await clientSocket.ConnectAsync(ip, port);
				Stream = clientSocket.GetStream();
			}
			catch (Exception ex)
			{
				clientSocket.Close();
				notvalid = true;
				if (!PreCon)
				{
					Showmessage = true;
				}
				errorText = "برقراری ارتباط  با سرورامکان پذیر نمی باشد خطا در ایجاد کانکشن" + Environment.NewLine + ex.Message;
				NetworkLockTask?.TrySetResult(result: false);
				return false;
			}
			SendMessage = SendMessage.EncryptData(Xpass);
			byte[] bytes = Encoding.ASCII.GetBytes(SendMessage);
			Stream.Write(bytes, 0, bytes.Length);
			Stream.Flush();
			await Task.Delay(1000);
			Stream.ReadTimeout = 10000;
			byte[] array = new byte[1025];
			int num = Stream.Read(array, 0, 1024);
			if (num == 0)
			{
				notvalid = true;
				errorText = "خطا در سمت کلاینت عدم دریافت  داده از سرور ";
				Stream.Dispose();
				clientSocket.Close();
				NetworkLockTask?.TrySetResult(result: false);
				return false;
			}
			string @string = Encoding.ASCII.GetString(array);
			@string = @string.Substring(0, num);
			if (string.IsNullOrEmpty(@string))
			{
				errorText = "خطا در تبدیل داده دریافتی از سرور ";
				notvalid = true;
				Stream.Dispose();
				clientSocket.Close();
				NetworkLockTask?.TrySetResult(result: false);
				return false;
			}
			@string = @string.DecryptString(Ypass);
			if (string.IsNullOrEmpty(@string))
			{
				errorText = "خطا در رمز گشایی داده ارسال شده از سرور ";
				notvalid = true;
				Stream.Dispose();
				clientSocket.Close();
				NetworkLockTask?.TrySetResult(result: false);
				return false;
			}
			@string = RemoveSequrityOption(@string, package);
			if (@string != temp)
			{
				notvalid = true;
				errorText = "از بروز بودن نرم افزار سرور اطمینان حاصل فرمائید" + Environment.NewLine + @string;
				Stream.Dispose();
				clientSocket.Close();
				notvalid = true;
				NetworkLockTask?.TrySetResult(result: false);
				return false;
			}
			clientSocket.Close();
			Stream.Dispose();
			NetworkLockTask?.TrySetResult(result: true);
			return true;
		}
		catch (Exception ex2)
		{
			notvalid = true;
			errorText = ex2.Message;
			WebErrorLog.Instance.StartLog(ex2, "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\Client.cs", 137);
			clientSocket.Close();
			NetworkLockTask?.TrySetResult(result: false);
			return false;
		}
	}

	private static string RemoveSequrityOption(string dataFromServer, Package package)
	{
		string text = dataFromServer;
		try
		{
			if (dataFromServer.Contains("ERROR"))
			{
				notvalid = true;
				return dataFromServer;
			}
			string text2 = "";
			text2 = dataFromServer.Substring(0, dataFromServer.IndexOf("$"));
			dataFromServer = dataFromServer.Remove(0, dataFromServer.IndexOf("$") + 1);
			switch (package)
			{
			case Package.HoghoghVaDastmozd:
			{
				string[] array = text2.Split(",".ToCharArray());
				if (array.Length < 3)
				{
					notvalid = true;
					return "";
				}
				ActiveCode = array[0];
				LkSerial = array[1]?.ParseToLong(0L) ?? 0;
				SoftSerial = array[2];
				break;
			}
			case Package.Accounting:
			{
				string[] array = text2.Split(",".ToCharArray());
				if (array.Length < 6)
				{
					notvalid = true;
					return "";
				}
				ActiveCode = array[0];
				LkSerial = array[1]?.ParseToLong(0L) ?? 0;
				SoftSerial = array[2];
				TolidActiveCode = array[3];
				BarcodeActiveCode = array[4];
				SpecialActiveCode = array[5];
				break;
			}
			}
			if ((ActiveCode.Length == 0) | (LkSerial == 0) | (SoftSerial.Length == 0))
			{
				notvalid = true;
			}
			return dataFromServer;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "dataFromServer:" + text + "\r\ncurrent_dataFromServer:" + dataFromServer, "RemoveSequrityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\Client.cs", 239);
			notvalid = true;
			return "خطا در بازیابی اطلاعات فرستاده شده از سمت سرور" + ex.Message;
		}
	}

	private static ReturnedSaveFuncInfoWithValue<string> ReturnId(Package package)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "ReturnId", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\Client.cs", 251);
			RegistryObject value = regObject.value;
			if (value.GetGuidClient() == Guid.Empty)
			{
				returnedSaveFuncInfoWithValue.AddReturnedValue(value.SetGuidClient(Guid.NewGuid(), package), ReturnedState.None, "ReturnId", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\Client.cs", 254);
			}
			returnedSaveFuncInfoWithValue.value = value.GetGuidClient().ToString();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ReturnId", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\Client.cs", 260);
			WebErrorLog.Instance.StartLog(ex, "", "ReturnId", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Client\\Client.cs", 261);
		}
		return returnedSaveFuncInfoWithValue;
	}
}
