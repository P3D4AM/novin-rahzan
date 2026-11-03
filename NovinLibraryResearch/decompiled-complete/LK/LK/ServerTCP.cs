using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.LkService;
using PacketParser.Entities;

namespace LK;

public class ServerTCP
{
	private static CancellationTokenSource token = new CancellationTokenSource();

	private static string Xpass = "NovinPHTToozandehjani";

	private static string Ypass = "NovinP123456789HT";

	public static void StartServer(Package package)
	{
		token?.Cancel();
		token = new CancellationTokenSource();
		CancellationToken tkn = token.Token;
		Task.Run(() => server(tkn, package));
	}

	public static void StopServer()
	{
		token?.Cancel();
	}

	private static async Task<ReturnedSaveFuncInfo> server(CancellationToken token, Package package)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		while (true)
		{
			try
			{
				await Task.Delay(5000, token);
				TcpListener tcpListener = new TcpListener(IPAddress.Any, 9090);
				bool flag = false;
				try
				{
					tcpListener.Start();
				}
				catch (Exception)
				{
					flag = true;
				}
				if (flag)
				{
					await Task.Delay(60000, token);
					continue;
				}
				token.ThrowIfCancellationRequested();
				try
				{
					while (true)
					{
						TcpClient clientSocket = null;
						clientSocket = tcpListener.AcceptTcpClient();
						Task.Run(() => AcceptClientAsync(clientSocket, token, package));
					}
				}
				catch (SocketException exception)
				{
					ret.AddReturnedValue(exception, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 63);
				}
				catch (OperationCanceledException exception2)
				{
					ret.AddReturnedValue(exception2, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 66);
					throw;
				}
				catch (Exception ex2)
				{
					ret.AddReturnedValue(ex2, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 71);
					WebErrorLog.Instance.StartLog(ex2, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 72);
					token.ThrowIfCancellationRequested();
				}
			}
			catch (SocketException exception3)
			{
				ret.AddReturnedValue(exception3, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 76);
			}
			catch (OperationCanceledException exception4)
			{
				ret.AddReturnedValue(exception4, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 77);
			}
			catch (Exception ex3)
			{
				ret.AddReturnedValue(ex3, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 80);
				WebErrorLog.Instance.StartLog(ex3, "", "server", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 81);
			}
			break;
		}
		return ret;
	}

	private static async Task AcceptClientAsync(TcpClient clientSocket, CancellationToken token, Package package)
	{
		try
		{
			NetworkStream stream = clientSocket.GetStream();
			stream.ReadTimeout = 10000;
			byte[] array = new byte[1025];
			int num = stream.Read(array, 0, 1024);
			token.ThrowIfCancellationRequested();
			if (num == 0)
			{
				stream.Dispose();
				clientSocket.Close();
				return;
			}
			token.ThrowIfCancellationRequested();
			string message = Encoding.ASCII.GetString(array).Substring(0, num).DecryptString(Xpass);
			IPAddress address = ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address;
			Tuple<string, LkClientServerErrors> value = ServerResponseFunc(message, package, address.ToString()).value;
			string message2 = ((value.Item2 == LkClientServerErrors.None) ? value.Item1 : ("ERROR" + (int)value.Item2 + "@" + value.Item1));
			message2 = message2.EncryptData(Ypass);
			byte[] bytes = Encoding.ASCII.GetBytes(message2);
			token.ThrowIfCancellationRequested();
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();
			clientSocket.Close();
			stream.Dispose();
			token.ThrowIfCancellationRequested();
		}
		catch (TaskCanceledException)
		{
		}
		catch (SocketException)
		{
			clientSocket.Close();
			token.ThrowIfCancellationRequested();
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "AcceptClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 148);
			clientSocket.Close();
			token.ThrowIfCancellationRequested();
		}
	}

	private static ReturnedSaveFuncInfoWithValue<Tuple<string, LkClientServerErrors>> ServerResponseFunc(string Message, Package package, string ipAddress)
	{
		ReturnedSaveFuncInfoWithValue<Tuple<string, LkClientServerErrors>> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<Tuple<string, LkClientServerErrors>>();
		int num = 1;
		try
		{
			if (string.IsNullOrEmpty(Message) || !Message.Contains("@"))
			{
				return returnedSaveFuncInfoWithValue.AddError("message is null or empty or message is not valid!", "ServerResponseFunc", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 160);
			}
			num = 1;
			string message = Message;
			num = 2;
			string type = Message.Substring(0, Message.IndexOf("@"));
			num = 3;
			Message = Message.Remove(0, Message.IndexOf("@") + 1);
			num = 4;
			if (Message == "**##**")
			{
				num = 5;
				string item = NumberOfClientRedirect(type);
				num = 6;
				returnedSaveFuncInfoWithValue.value = new Tuple<string, LkClientServerErrors>(item, LkClientServerErrors.None);
				return returnedSaveFuncInfoWithValue;
			}
			num = 7;
			string input = Message.Substring(0, Message.IndexOf("@"));
			num = 8;
			if (!Guid.TryParse(input, out var result))
			{
				num = 9;
				returnedSaveFuncInfoWithValue.value = new Tuple<string, LkClientServerErrors>("", LkClientServerErrors.CanNotParseGuidClient);
				return returnedSaveFuncInfoWithValue;
			}
			num = 10;
			ReturnedSaveFuncInfoWithValue<clsLkError> returnedSaveFuncInfoWithValue2 = Add2List(result, type, package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "ServerResponseFunc", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 188);
			clsLkError value = returnedSaveFuncInfoWithValue2.value;
			num = 11;
			if (value.HasError)
			{
				num = 12;
				message = AddSecurityOption(message, type).value;
				num = 13;
				num = 13;
				if (!message.Contains("ERROR"))
				{
					returnedSaveFuncInfoWithValue.value = new Tuple<string, LkClientServerErrors>(message, LkClientServerErrors.None);
				}
				else
				{
					returnedSaveFuncInfoWithValue.value = new Tuple<string, LkClientServerErrors>(message, LkClientServerErrors.UnKnown);
				}
				return returnedSaveFuncInfoWithValue;
			}
			num = 14;
			string display = value.Error.GetDisplay();
			num = 15;
			returnedSaveFuncInfoWithValue.value = new Tuple<string, LkClientServerErrors>(display, value.Error);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = new Tuple<string, LkClientServerErrors>(ex.Message, LkClientServerErrors.UnKnown);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ServerResponseFunc", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 214);
			WebErrorLog.Instance.StartLog(ex, $"Line in Line {num}\r\n" + "Ip:" + ipAddress + "\r\nmessage: " + Message, "ServerResponseFunc", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 215);
		}
		return returnedSaveFuncInfoWithValue;
	}

	private static ReturnedSaveFuncInfoWithValue<clsLkError> Add2List(Guid clientGuid, string type, Package package)
	{
		ReturnedSaveFuncInfoWithValue<clsLkError> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<clsLkError>();
		try
		{
			string text = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "NovinP" + type, "").ToString();
			if (string.IsNullOrEmpty(text))
			{
				if (package == Package.HoghoghVaDastmozd)
				{
					text = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "NovinPHoghoghVaDastmozd", "").ToString();
				}
				if (package == Package.Accounting)
				{
					text = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "NovinPAccounting", "").ToString();
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				returnedSaveFuncInfoWithValue.value = new clsLkError(hasError: false, LkClientServerErrors.ServerNotActive);
				return returnedSaveFuncInfoWithValue;
			}
			RegistryObject registryObject = new RegistryObject();
			registryObject = RegistryObject.FromJson(text.DecryptString());
			if (registryObject == null)
			{
				returnedSaveFuncInfoWithValue.value = new clsLkError(hasError: false, LkClientServerErrors.ServerNotActive);
				return returnedSaveFuncInfoWithValue;
			}
			if (registryObject.GetClientNetNumber().ParseToInt() <= 0)
			{
				returnedSaveFuncInfoWithValue.value = new clsLkError(hasError: false, LkClientServerErrors.ClientNumberNotSet);
				return returnedSaveFuncInfoWithValue;
			}
			if (registryObject.ListOfClient.Contains(clientGuid))
			{
				returnedSaveFuncInfoWithValue.value = new clsLkError(hasError: true, LkClientServerErrors.None);
				return returnedSaveFuncInfoWithValue;
			}
			if (registryObject.ListOfClient.Count < registryObject.GetClientNetNumber().ParseToInt())
			{
				registryObject.ListOfClient.Add(clientGuid);
				RegistryObject.WriteRegistryKey(registryObject.ToString(), type);
				returnedSaveFuncInfoWithValue.value = new clsLkError(hasError: true, LkClientServerErrors.None);
				return returnedSaveFuncInfoWithValue;
			}
			returnedSaveFuncInfoWithValue.value = new clsLkError(hasError: false, LkClientServerErrors.ClientListIsFull);
			return returnedSaveFuncInfoWithValue;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Add2List", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 289);
			returnedSaveFuncInfoWithValue.value = new clsLkError(hasError: false, LkClientServerErrors.UnKnown);
			return returnedSaveFuncInfoWithValue;
		}
	}

	private static string RemoveChar(string obj)
	{
		try
		{
			List<string> list = new List<string>(new string[62]
			{
				"A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
				"K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
				"U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d",
				"e", "f", "g", "h", "i", "j", "k", "l", "m", "n",
				"o", "p", "q", "r", "s", "t", "u", "v", "w", "x",
				"y", "z", "0", "1", "2", "3", "4", "5", "6", "7",
				"8", "9"
			});
			List<string> list2 = new List<string>();
			char[] array = obj.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				char c = array[i];
				if (list.Contains(c.ToString()))
				{
					list2.Add(c.ToString());
				}
			}
			return string.Join("", list2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveChar", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 309);
			return obj;
		}
	}

	private static ReturnedSaveFuncInfoWithValue<string> AddSecurityOption(string message, string type)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			RegistryObject registryObject = new RegistryObject();
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = RegistryObject.ReadListFromObject(type);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "AddSecurityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 320);
			registryObject = RegistryObject.FromJson(returnedSaveFuncInfoWithValue2.value);
			switch (type)
			{
			case "HoghoghVaDastmozd":
				message = registryObject.GetHoghoghActivationCode() + "," + registryObject.GetLkSerial() + "," + registryObject.GetSoftwareSerial_Hozor() + "$" + message;
				break;
			case "Accounting":
				message = registryObject.GetAccountingActivationCode() + "," + registryObject.GetLkSerial() + "," + registryObject.GetSoftwareSerial_Accounting() + "," + registryObject.GetTolidActivationCode() + "," + registryObject.GetBarcodeActivationCode()?.ToString() + "," + registryObject.GetSpecialActivationCode() + "$" + message;
				break;
			case "BarCode":
			{
				RegistryObject registryObject3 = new RegistryObject();
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue4 = RegistryObject.ReadListFromObject("Accounting");
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue4, ReturnedState.None, "AddSecurityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 340);
				registryObject3 = RegistryObject.FromJson(returnedSaveFuncInfoWithValue4.value.DecryptString());
				ReturnedSaveFuncInfoWithValue<string> barcodeActivationCode = registryObject3.GetBarcodeActivationCode();
				returnedSaveFuncInfoWithValue.AddReturnedValue(barcodeActivationCode, ReturnedState.None, "AddSecurityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 344);
				message = ((barcodeActivationCode.value.Length != 10) ? (registryObject.GetBarcodeActivationCode()?.ToString() + "," + registryObject.GetLkSerial() + "," + registryObject.GetSoftwareSerial_Accounting() + "$" + message) : (registryObject3.GetBarcodeActivationCode()?.ToString() + "," + registryObject3.GetLkSerial() + "," + registryObject3.GetSoftwareSerial_Accounting() + "$" + message));
				break;
			}
			case "Special":
			{
				RegistryObject registryObject4 = new RegistryObject();
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue5 = RegistryObject.ReadListFromObject("Accounting");
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue5, ReturnedState.None, "AddSecurityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 353);
				registryObject4 = RegistryObject.FromJson(returnedSaveFuncInfoWithValue5.value.DecryptString());
				message = ((registryObject4.GetSpecialActivationCode().Length != 10) ? (registryObject.GetSpecialActivationCode() + "," + registryObject.GetLkSerial() + "," + registryObject.GetSoftwareSerial_Accounting() + "$" + message) : (registryObject4.GetSpecialActivationCode() + "," + registryObject4.GetLkSerial() + "," + registryObject4.GetSoftwareSerial_Accounting() + "$" + message));
				break;
			}
			case "Tolid":
			{
				RegistryObject registryObject2 = new RegistryObject();
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue3 = RegistryObject.ReadListFromObject("Accounting");
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "AddSecurityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 365);
				registryObject2 = RegistryObject.FromJson(returnedSaveFuncInfoWithValue3.value.DecryptString());
				message = ((registryObject2.GetTolidActivationCode().Length != 10) ? (registryObject.GetTolidActivationCode() + "," + registryObject.GetLkSerial() + "," + registryObject.GetSoftwareSerial_Accounting() + "$" + message) : (registryObject2.GetTolidActivationCode() + "," + registryObject2.GetLkSerial() + "," + registryObject2.GetSoftwareSerial_Accounting() + "$" + message));
				break;
			}
			default:
				returnedSaveFuncInfoWithValue.value = "ERROR" + LkClientServerErrors.TypeUnknown.GetDisplay() + "$" + message;
				return returnedSaveFuncInfoWithValue;
			}
			returnedSaveFuncInfoWithValue.value = message;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "AddSecurityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 383);
			WebErrorLog.Instance.StartLog(ex, "", "AddSecurityOption", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 385);
			returnedSaveFuncInfoWithValue.value = "ERROR" + ex.Message + "$" + message;
		}
		return returnedSaveFuncInfoWithValue;
	}

	private static string NumberOfClientRedirect(string type)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			new RegistryObject();
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = RegistryObject.ReadListFromObject(type);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "NumberOfClientRedirect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 397);
			RegistryObject registryObject = RegistryObject.FromJson(returnedSaveFuncInfoWithValue2.value);
			string clientNetNumber = registryObject.GetClientNetNumber();
			string text = Convert.ToString(registryObject.ListOfClient.Count);
			return clientNetNumber + "\\" + text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NumberOfClientRedirect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\ServerTCP.cs", 407);
			return "";
		}
	}
}
