using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.ErroHandler;

namespace PacketParser;

public class ErrorHandler
{
	public static ENSource Source;

	public static string Version;

	public static bool TakeScreenShot;

	public static string Path;

	public static string Username;

	public static string Password;

	public static bool ConsoleLog;

	private static bool inited = false;

	public static string FileSavePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ErrorHandler");

	private static Task _retryThread = null;

	private static CancellationTokenSource _retryToken = new CancellationTokenSource();

	public static short MaxCountPerTime { get; set; }

	public static long LKSerial
	{
		get
		{
			return SystemInfo.LKSerial;
		}
		set
		{
			SystemInfo.LKSerial = value;
		}
	}

	public static long AndroidIme { get; set; }

	public static bool WebServiceLog { get; set; } = true;


	public static void AddHandler(ILogger Logger, bool screenshot, bool showUser, bool kill, string version, ENSource source, string path, long LKserial = 0L, string username = "accountingerrorlog", string password = "accountingerrorlog", int minutes = 10, int minSecendsBetweenExeptions = 30, LogLevel minLevel = LogLevel.Error, bool exportToConsole = false, LogLevel minConsoleLevel = LogLevel.Debug, string consoleFormat = "", bool sendLogToSignalR = false, LogLevel signalRMinLogLevel = LogLevel.Error, bool sendLogToWebService = false, LogLevel webServiceMinLogLevel = LogLevel.Error, string clientUsername = "")
	{
		if (minSecendsBetweenExeptions <= 60)
		{
			WebErrorLog.MinMinutsBetweenExeptions = 1;
		}
		else
		{
			WebErrorLog.MinMinutsBetweenExeptions = minSecendsBetweenExeptions / 60;
		}
		WebErrorLog.MinLogLevel = minLevel;
		WebErrorLog.Logger = Logger;
		WebErrorLog.ExportToConsole = exportToConsole;
		WebErrorLog.ConsoleLogFormat = consoleFormat;
		WebErrorLog.ConsoleMinLogLevel = minConsoleLevel;
		WebErrorLog.ClientUsername = clientUsername;
		Username = username;
		Password = password;
		if (inited)
		{
			return;
		}
		inited = true;
		if (string.IsNullOrEmpty(version))
		{
			new ArgumentNullException("version", "The version can not be null or empty");
		}
		if (path == null)
		{
			new ArgumentNullException("path", "The path can not be null");
		}
		if (string.IsNullOrEmpty(username))
		{
			new ArgumentNullException("username", "The username can not be null or empty");
		}
		if (string.IsNullOrEmpty(password))
		{
			new ArgumentNullException("password", "The password can not be null or empty");
		}
		WebErrorLog.SetMinute(minutes);
		AppDomain.CurrentDomain.UnhandledException += WebErrorLog.Instance.UnhandledExceptionLog;
		WebErrorLog.Instance.OnExceptionHandle += ErrorLogInstance_OnExceptionHandle;
		Source = source;
		TakeScreenShot = false;
		Path = path;
		Version = version;
		Username = username;
		Password = password;
		LKSerial = LKserial;
		SystemInfo.LKSerial = LKserial;
		if (_retryThread == null)
		{
			_retryToken.Cancel();
			_retryToken = new CancellationTokenSource();
			_retryThread = Task.Run(() => RetrySend(_retryToken.Token));
		}
	}

	private static async Task RetrySend(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				_ = 3;
				try
				{
					if (!token.IsCancellationRequested)
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(FileSavePath);
						if (!directoryInfo.Exists)
						{
							WebErrorLog.Logger?.SaveLog(FileSavePath + " not exists RetrySend()", EnGroup.None, null, 10, "RetrySend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\ErrorHandler.cs", 149);
							await Task.Delay(60000);
							FileSavePath = System.IO.Path.Combine("C:\\", "ErrorHandler");
							continue;
						}
						FileInfo[] files = directoryInfo.GetFiles("*.txt");
						if (files != null && files.Length < 1)
						{
							await Task.Delay(30000);
							continue;
						}
						List<WebErrorLog> errors = new List<WebErrorLog>();
						FileInfo[] array = files;
						int num = 0;
						while (true)
						{
							if (num < array.Length)
							{
								FileInfo fileInfo = array[num];
								if (token.IsCancellationRequested || !File.Exists(fileInfo.FullName))
								{
									break;
								}
								WebErrorLog webErrorLog = ReadFileContexts(fileInfo.FullName);
								if (webErrorLog == null)
								{
									goto IL_029d;
								}
								try
								{
									File.Delete(fileInfo.FullName);
								}
								catch
								{
									await Task.Delay(60000);
									goto IL_029d;
								}
								if (errors.Count <= 20)
								{
									errors.Add(webErrorLog);
									goto IL_029d;
								}
							}
							if (errors?.Any() ?? false)
							{
								if (token.IsCancellationRequested)
								{
									break;
								}
								await SaveWebErrorLog(errors);
							}
							goto end_IL_0041;
							IL_029d:
							num++;
						}
					}
					end_IL_0041:;
				}
				catch (OperationCanceledException)
				{
				}
				catch (ThreadAbortException)
				{
				}
			}
			finally
			{
				await Task.Delay(300000);
			}
			int num2;
			if (num2 == 2)
			{
				break;
			}
		}
	}

	private static WebErrorLog ReadFileContexts(string fileInfoFullName)
	{
		try
		{
			try
			{
				using StreamReader streamReader = new StreamReader(fileInfoFullName);
				return JsonConvert.DeserializeObject<WebErrorLog>(streamReader.ReadToEnd());
			}
			catch (Exception ex)
			{
				File.Delete(fileInfoFullName);
				WebErrorLog.Instance.StartLog(ex, "fileInfoFullName:" + fileInfoFullName + " رو نتوستم deserialze کنم ولیکن با موفقیت تونستم حذف اش کنم\r\bError:" + ex.Message, "ReadFileContexts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\ErrorHandler.cs", 201);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "fileInfoFullName:" + fileInfoFullName, "ReadFileContexts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\ErrorHandler.cs", 205);
		}
		return null;
	}

	private static async Task ErrorLogInstance_OnExceptionHandle(object sender, WebErrorLog e)
	{
		string[] array = e.ClassName.Split("/\\".ToCharArray());
		e.ClassName = array[checked(array.Length - 1)];
		e.LKSerial = SystemInfo.LKSerial;
		e.CpuSerial = "";
		e.HardSerial = "";
		e.Source = Source;
		e.AndroidIme = "";
		e.Version = Version;
		e.Fatal = true;
		e.Path = Path;
		if (WebServiceLog)
		{
			await SaveWebErrorLog(new List<WebErrorLog> { e });
		}
	}

	private static async Task SaveWebErrorLog(IEnumerable<WebErrorLog> errors)
	{
		if (!(await SendToServer(errors)))
		{
			SaveToFile(errors, FileSavePath);
		}
	}

	private static async Task<bool> SendToServer(IEnumerable<WebErrorLog> errors)
	{
		try
		{
			WebErrorLogStandard content = new WebErrorLogStandard
			{
				AndroidIme = AndroidIme,
				Version = Version,
				Password = Password,
				Source = Source,
				CpuSerial = "",
				HardSerial = "",
				Username = Username,
				LkSerial = SystemInfo.LKSerial,
				ErrorLogs = errors.ToList()
			};
			string url = "http://webservice.novinp.ir/api/ErrorLogService/";
			await HttpHelpers.PostStreamAsync(content, url, default(CancellationToken));
			return true;
		}
		catch (Exception ex)
		{
			InternalLogger.LogException(ex, "", "SendToServer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\ErrorHandler.cs", 262);
			return false;
		}
	}

	private static void SaveToFile(IEnumerable<WebErrorLog> errors, string localSavePath)
	{
		try
		{
			if (localSavePath != null && localSavePath.Length < 1)
			{
				return;
			}
			if (!Directory.Exists(localSavePath))
			{
				Directory.CreateDirectory(localSavePath);
			}
			foreach (WebErrorLog error in errors)
			{
				using StreamWriter streamWriter = new StreamWriter(System.IO.Path.Combine(localSavePath, error.Guid.ToString() + ".txt"));
				HttpHelpers.SerializeJsonIntoStream(error, streamWriter.BaseStream);
				streamWriter.Close();
			}
			WebErrorLog.Logger?.SaveLog("Errors has been saved on " + localSavePath, EnGroup.None, null, 10, "SaveToFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\ErrorHandler.cs", 287);
		}
		catch (Exception ex)
		{
			WebErrorLog.Logger?.SaveLog("exception on SaveToFile localSavePath:" + localSavePath, EnGroup.None, null, 10, "SaveToFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\ErrorHandler.cs", 291);
			InternalLogger.LogException(ex, "", "SaveToFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\ErrorHandler.cs", 292);
		}
	}
}
