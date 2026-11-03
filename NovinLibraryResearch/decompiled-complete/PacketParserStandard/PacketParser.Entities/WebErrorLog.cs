using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using Serilog;
using Serilog.Events;

namespace PacketParser.Entities;

public class WebErrorLog : IErrorHandler, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private class NestedErrorHandler
	{
		internal static readonly WebErrorLog ErrorHandlerInstance;

		static NestedErrorHandler()
		{
			ErrorHandlerInstance = new WebErrorLog();
		}
	}

	private static string apiurl = HttpHelpers.ApiUrl + "crm/";

	private string _stackTrace = "";

	private string _description = "";

	private string _exceptionMessage = "";

	private static int _minutes = 30;

	private string _flowStackTrace = "";

	private static DateTime LastExeption = DateTime.Now.AddSeconds(-10000.0);

	private int CollectedErrors;

	private static bool ErrorHasBeenLogged = false;

	[XmlIgnore]
	[JsonIgnore]
	public virtual string ContextGuid { get; set; }

	public string FuncName { get; set; } = "";


	[JsonIgnore]
	public string DateSh => "";

	public DateTime Date { get; set; } = DateTime.Now;


	public string AndroidIme { get; set; } = "";


	public string CpuSerial { get; set; } = "";


	public string HardSerial { get; set; } = "";


	public long LKSerial { get; set; }

	public string CustInfo { get; set; }

	public string ClassName { get; set; } = "";


	public ENSource Source { get; set; } = ENSource.Accounting;


	public string Version { get; set; } = "";


	public string ScreenShot { get; set; } = "";


	public bool Fatal { get; set; }

	public int GroupingID { get; set; }

	public string ExceptionType { get; set; } = "";


	public PlaceError ExceptionPlace { get; set; }

	public string Ip { get; set; }

	public int HashAll { get; set; }

	public string Path { get; set; }

	public string LoggerVersion { get; set; } = "1.1";


	public string ExceptionMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_exceptionMessage))
			{
				return "";
			}
			_exceptionMessage.Replace("\r\n", "\n").Replace("\r", "\r\n").Replace("\n", "\r\n");
			return _exceptionMessage;
		}
		set
		{
			_exceptionMessage = value;
		}
	}

	public int RefrencedID { get; set; }

	public string ExceptionError { get; set; } = "";


	public string StackTrace
	{
		get
		{
			if (string.IsNullOrEmpty(_stackTrace))
			{
				return "";
			}
			_stackTrace = _stackTrace.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
			_stackTrace = _stackTrace.Replace(" at ", "\r\nat ");
			return _stackTrace;
		}
		set
		{
			_stackTrace = value;
		}
	}

	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(_description))
			{
				_description = "";
			}
			_description.Replace("\r\n", "\n").Replace("\r", "\r\n").Replace("\n", "\r\n");
			return _description;
		}
		set
		{
			_description = value;
		}
	}

	public string FlowStackTrace
	{
		get
		{
			if (string.IsNullOrEmpty(_flowStackTrace))
			{
				return "";
			}
			_flowStackTrace = _flowStackTrace.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
			_flowStackTrace = _flowStackTrace.Replace(" at ", "\r\nat ");
			return _flowStackTrace;
		}
		set
		{
			_flowStackTrace = value;
		}
	}

	[DefaultValue(4)]
	public LogLevel Level { get; set; } = LogLevel.Error;


	public string Username { get; set; } = "";


	[JsonIgnore]
	public static int MinMinutsBetweenExeptions { get; set; } = 1;


	[JsonIgnore]
	public static LogLevel MinLogLevel { get; set; } = LogLevel.Error;


	public static string ClientUsername { get; set; } = "";


	[JsonIgnore]
	[XmlIgnore]
	public static LogLevel ConsoleMinLogLevel { get; set; } = LogLevel.Debug;


	[JsonIgnore]
	[XmlIgnore]
	public static string ConsoleLogFormat { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public static bool ExportToConsole { get; set; } = false;


	[JsonIgnore]
	public static ILogger Logger { get; set; } = null;


	public string ScreenShot_FileName
	{
		get
		{
			string screenShot = ScreenShot;
			if (screenShot != null && screenShot.Length < 1)
			{
				return "";
			}
			return new FileInfo(ScreenShot).Name;
		}
	}

	public string PersianMessageForUi
	{
		get
		{
			try
			{
				return ExceptionMessageParser.Get(ExceptionMessage)?.PersianMessageForUi ?? "";
			}
			catch (Exception ex)
			{
				Instance.StartLog(ex, "", "PersianMessageForUi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\WebErrorLog.cs", 145);
			}
			return "";
		}
	}

	[JsonIgnore]
	private static DateTime StartBuffer { get; set; } = DateTime.Now;


	[JsonIgnore]
	private static ConcurrentDictionary<string, DateTime> LastErrors { get; set; } = new ConcurrentDictionary<string, DateTime>();


	public static WebErrorLog ErrorLogInstance => NestedErrorHandler.ErrorHandlerInstance;

	public static WebErrorLog Instance => ErrorLogInstance;

	public short MaxCountPerTime { get; set; } = 5;


	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public event Func<object, WebErrorLog, Task> OnExceptionHandle;

	public event Func<object, WebErrorLog, Task> ErrorForUser;

	public static void SetMinute(int minute)
	{
		if (minute < 0)
		{
			minute = 30;
		}
		_minutes = minute;
	}

	public void ThreadExceptionLog(object sender, ThreadExceptionEventArgs args)
	{
		try
		{
			if (ErrorLogInstance.OnExceptionHandle == null || args.Exception.Message.ToLower().Contains("abort"))
			{
				return;
			}
			if (args.Exception != null)
			{
				Console.WriteLine("Thread exception: " + args.Exception.Message);
			}
			StackTrace stackTrace = new StackTrace();
			StackFrame frame = stackTrace.GetFrame(0);
			string name = frame.GetMethod().ReflectedType.Name;
			if (string.IsNullOrEmpty(name))
			{
				name = frame.GetMethod().DeclaringType.Name;
			}
			string name2 = frame.GetMethod().Name;
			string.IsNullOrEmpty(frame.GetFileName());
			WebErrorLog errorLog = new WebErrorLog
			{
				Path = AppDomain.CurrentDomain.BaseDirectory,
				Guid = Guid.NewGuid(),
				ExceptionType = "ThreadException",
				StackTrace = stackTrace.ToString(),
				ClassName = name,
				FuncName = name2
			};
			string key = errorLog.ClassName + errorLog.FuncName + stackTrace.GetHashCode();
			DateTime value;
			bool flag = LastErrors.TryGetValue(key, out value);
			if (!flag || !((DateTime.Now - value).TotalMinutes < (double)_minutes))
			{
				if (flag)
				{
					LastErrors.TryRemove(key, out var _);
				}
				LastErrors.TryAdd(key, DateTime.Now);
				Task.Run(() => ErrorLogInstance.OnExceptionHandle?.Invoke(errorLog, errorLog));
			}
		}
		catch (Exception ex)
		{
			InternalLogger.LogException(ex, "", "ThreadExceptionLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\WebErrorLog.cs", 224);
		}
	}

	public void UnhandledExceptionLog(object sender, UnhandledExceptionEventArgs ex)
	{
		try
		{
			if (ErrorLogInstance.OnExceptionHandle == null || ex == null)
			{
				return;
			}
			StackTrace stackTrace = new StackTrace();
			StackFrame stackFrame = null;
			if (stackTrace.FrameCount > 1)
			{
				stackFrame = stackTrace.GetFrame(1);
			}
			WebErrorLog errorLog = new WebErrorLog
			{
				Path = AppDomain.CurrentDomain.BaseDirectory,
				Guid = Guid.NewGuid(),
				StackTrace = stackTrace.ToString(),
				ExceptionType = "UnhandledException",
				ClassName = "Is Unhandled Exception CantGet File Name",
				FuncName = stackFrame?.GetMethod().Name
			};
			string key = errorLog.ClassName + errorLog.FuncName;
			DateTime value;
			bool flag = LastErrors.TryGetValue(key, out value);
			if (!flag || !((DateTime.Now - value).TotalMinutes < (double)_minutes))
			{
				if (flag)
				{
					LastErrors.TryRemove(key, out var _);
				}
				LastErrors.TryAdd(key, DateTime.Now);
				Task.Run(() => ErrorLogInstance?.OnExceptionHandle?.Invoke(errorLog, errorLog));
			}
		}
		catch (Exception ex2)
		{
			InternalLogger.LogException(ex2, "", "UnhandledExceptionLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\WebErrorLog.cs", 265);
		}
	}

	public void Trace(string message, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		Log.Verbose(message);
	}

	public void Information(string message, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		Log.Information(message);
	}

	public void Debug(string message, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		Log.Debug(message);
	}

	public void Warning(string message, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		Log.Warning(message);
	}

	public void Warning(Exception ex, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		Log.Warning(ex, description);
	}

	public void Error(string message, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		StartLog(new Exception(message), description, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
	}

	public void Error(Exception ex, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		StartLog(ex, description, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
	}

	public void FatalLog(string message, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		StartLog(new Exception(message), description, LogLevel.Critical, memberName, sourceFilePath, sourceLineNumber);
	}

	public void FatalLog(Exception ex, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		StartLog(ex, description, LogLevel.Critical, memberName, sourceFilePath, sourceLineNumber);
	}

	public void StartLog(string ex, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		StartLog(new Exception(ex), description, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
	}

	public void StartLog(Exception ex, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		StartLog(ex, description, LogLevel.Error, memberName, sourceFilePath, sourceLineNumber);
	}

	public void StartLog(Exception ex, string description, LogLevel level, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		checked
		{
			try
			{
				Log.Error(ex, description);
				if (ExportToConsole && level >= ConsoleMinLogLevel)
				{
					WebErrorLog errorLog2 = BuildEntityForConsole(ex, description, level, memberName, sourceFilePath, sourceLineNumber);
					DebugWriteConsole(errorLog2);
				}
				if (level < MinLogLevel)
				{
					return;
				}
				ILogger logger = Logger;
				if (logger != null)
				{
					logger.SaveLog("\r\n***********************\r\ndescription:" + description + "\r\nexception:" + ExceptionMessage + "\r\nstackTrace:" + StackTrace + "\r\n**********\r\n\r\n", MapLogLevelToEnGroup(level), DateTime.Now, 10, memberName, sourceFilePath, sourceLineNumber);
				}
				if (DateTime.Now.Subtract(LastExeption).TotalMinutes > (double)MinMinutsBetweenExeptions)
				{
					LastExeption = DateTime.Now;
					CollectedErrors = 0;
				}
				if (DateTime.Now.Subtract(LastExeption).TotalMinutes <= (double)MinMinutsBetweenExeptions && CollectedErrors >= MaxCountPerTime)
				{
					return;
				}
				CollectedErrors++;
				if (ErrorLogInstance?.OnExceptionHandle == null)
				{
					return;
				}
				WebErrorLog errorLog = BuildEntity(ex, description, level, memberName, sourceFilePath, sourceLineNumber);
				if (errorLog == null || ErrorLogInstance?.OnExceptionHandle == null)
				{
					return;
				}
				Task.Run(delegate
				{
					try
					{
						ErrorLogInstance.OnExceptionHandle(this, errorLog);
					}
					catch (Exception ex4)
					{
						InternalLogger.LogException(ex4, "", "StartLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\WebErrorLog.cs", 467);
					}
				});
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex3)
			{
				InternalLogger.LogException(ex3, "", "StartLog", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\WebErrorLog.cs", 475);
			}
		}
	}

	private LogEventLevel MapLogLevel2SerilogLevel(LogLevel level)
	{
		return (LogEventLevel)(level switch
		{
			LogLevel.Trace => 0, 
			LogLevel.Debug => 1, 
			LogLevel.Information => 2, 
			LogLevel.Warning => 3, 
			LogLevel.Error => 4, 
			LogLevel.Critical => 4, 
			LogLevel.None => 4, 
			_ => 4, 
		});
	}

	private EnGroup MapLogLevelToEnGroup(LogLevel level)
	{
		return level switch
		{
			LogLevel.Trace => EnGroup.Trace, 
			LogLevel.Debug => EnGroup.Debug, 
			LogLevel.Information => EnGroup.Information, 
			LogLevel.Warning => EnGroup.Warning, 
			LogLevel.Error => EnGroup.Exception, 
			LogLevel.Critical => EnGroup.Exception, 
			LogLevel.None => EnGroup.Exception, 
			_ => EnGroup.Exception, 
		};
	}

	private WebErrorLog BuildEntity(Exception ex, string description, LogLevel level, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		string text = ex.Message ?? "";
		if (!string.IsNullOrEmpty(ex?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0finnerException:" + ex.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0finnerException2:" + ex.InnerException.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0f❗\ufe0finnerException3:" + ex.InnerException.InnerException.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0f❗\ufe0f❗\ufe0finnerException4:" + ex.InnerException.InnerException.InnerException.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.InnerException?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0f❗\ufe0f❗\ufe0f❗\ufe0finnerException5:" + ex.InnerException.InnerException.InnerException.InnerException.InnerException.Message;
		}
		string text2 = ((level >= LogLevel.Error) ? ex.StackTrace : "");
		Logger?.SaveLog("description:" + description + "\r\nexception:" + text + "\r\nstackTrace:" + text2, EnGroup.Exception, DateTime.Now, 10, memberName, sourceFilePath, sourceLineNumber);
		if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(memberName))
		{
			MethodBase method = new StackTrace().GetFrame(1).GetMethod();
			if (string.IsNullOrEmpty(memberName))
			{
				memberName = $"{Guid.NewGuid()}_Unhandled_" + method.Name;
			}
			if (string.IsNullOrEmpty(sourceFilePath))
			{
				sourceFilePath = "{Reflection}_" + method.ReflectedType.Name;
			}
		}
		if (DateTime.Now.Subtract(LastExeption).TotalMinutes > (double)MinMinutsBetweenExeptions)
		{
			LastExeption = DateTime.Now;
			CollectedErrors = 0;
		}
		if (DateTime.Now.Subtract(LastExeption).TotalMinutes <= (double)MinMinutsBetweenExeptions && CollectedErrors >= MaxCountPerTime)
		{
			return null;
		}
		checked
		{
			CollectedErrors++;
			FileInfo fileInfo = null;
			if (!string.IsNullOrEmpty(sourceFilePath) && File.Exists(sourceFilePath))
			{
				fileInfo = new FileInfo(sourceFilePath);
			}
			string key = fileInfo?.Name ?? (memberName + sourceLineNumber + ex.Message);
			DateTime value;
			bool flag = LastErrors.TryGetValue(key, out value);
			if (flag && (DateTime.Now - value).TotalMinutes < (double)_minutes)
			{
				return null;
			}
			if (flag)
			{
				LastErrors.TryRemove(key, out var _);
			}
			LastErrors.TryAdd(key, DateTime.Now);
			StackTrace stackTrace = null;
			if (level >= LogLevel.Error)
			{
				stackTrace = new StackTrace();
			}
			WebErrorLog webErrorLog = new WebErrorLog
			{
				ClassName = (fileInfo?.Name ?? ""),
				FuncName = memberName,
				Path = AppDomain.CurrentDomain.BaseDirectory,
				Guid = Guid.NewGuid(),
				ExceptionType = ex.GetType().ToString(),
				ExceptionMessage = text,
				StackTrace = text2,
				Description = description,
				FlowStackTrace = stackTrace?.ToString(),
				Level = level,
				Username = ClientUsername
			};
			if (string.IsNullOrEmpty(webErrorLog.ClassName))
			{
				webErrorLog.ClassName = sourceFilePath;
			}
			try
			{
				webErrorLog.ExceptionType = ex.GetType().ToString();
			}
			catch
			{
				webErrorLog.ExceptionType = "‼\ufe0f‼\ufe0fexceptio On get Type:" + ex.Message;
			}
			while (ex.InnerException != null)
			{
				WebErrorLog webErrorLog2 = webErrorLog;
				webErrorLog2.ExceptionMessage = webErrorLog2.ExceptionMessage + "\r\n_______________________\r\nType:" + ex.GetType().ToString() + "\r\n" + ex.Message;
				ex = ex.InnerException;
			}
			return webErrorLog;
		}
	}

	private WebErrorLog BuildEntityForConsole(Exception ex, string description, LogLevel level, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		string text = ex.Message ?? "";
		if (!string.IsNullOrEmpty(ex?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0finnerException:" + ex.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0finnerException2:" + ex.InnerException.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0f❗\ufe0finnerException3:" + ex.InnerException.InnerException.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0f❗\ufe0f❗\ufe0finnerException4:" + ex.InnerException.InnerException.InnerException.InnerException.Message;
		}
		if (!string.IsNullOrEmpty(ex?.InnerException?.InnerException?.InnerException?.InnerException?.InnerException?.Message))
		{
			text = text + "\r\n❗\ufe0f❗\ufe0f❗\ufe0f❗\ufe0f❗\ufe0finnerException5:" + ex.InnerException.InnerException.InnerException.InnerException.InnerException.Message;
		}
		string stackTrace = ((level >= LogLevel.Error) ? ex.StackTrace : "");
		if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(memberName))
		{
			MethodBase method = new StackTrace().GetFrame(1).GetMethod();
			if (string.IsNullOrEmpty(memberName))
			{
				memberName = "{Reflection}_" + method.Name;
			}
			if (string.IsNullOrEmpty(sourceFilePath))
			{
				sourceFilePath = "{Reflection}_" + method.ReflectedType.Name;
			}
		}
		FileInfo fileInfo = null;
		if (!string.IsNullOrEmpty(sourceFilePath) && File.Exists(sourceFilePath))
		{
			fileInfo = new FileInfo(sourceFilePath);
		}
		StackTrace stackTrace2 = null;
		if (level >= LogLevel.Error)
		{
			stackTrace2 = new StackTrace();
		}
		WebErrorLog webErrorLog = new WebErrorLog
		{
			ClassName = (fileInfo?.Name ?? ""),
			FuncName = memberName,
			Path = AppDomain.CurrentDomain.BaseDirectory,
			Guid = Guid.NewGuid(),
			ExceptionType = ex.GetType().ToString(),
			ExceptionMessage = text,
			StackTrace = stackTrace,
			Description = description,
			FlowStackTrace = stackTrace2?.ToString(),
			Level = level
		};
		if (string.IsNullOrEmpty(webErrorLog.ClassName))
		{
			webErrorLog.ClassName = sourceFilePath;
		}
		try
		{
			webErrorLog.ExceptionType = ex.GetType().ToString();
		}
		catch
		{
			webErrorLog.ExceptionType = "‼\ufe0f‼\ufe0fexception on get type:" + ex.Message;
		}
		while (ex.InnerException != null)
		{
			WebErrorLog webErrorLog2 = webErrorLog;
			webErrorLog2.ExceptionMessage = webErrorLog2.ExceptionMessage + "\r\n_______________________\r\nType:" + ex.GetType().ToString() + "\r\n" + ex.Message;
			ex = ex.InnerException;
		}
		return webErrorLog;
	}

	private void DebugWriteConsole(WebErrorLog errorLog)
	{
		try
		{
			if (ExportToConsole && errorLog.Level >= ConsoleMinLogLevel)
			{
				new Dictionary<string, string>
				{
					{
						"short_date",
						errorLog.Date.ToShortDateString()
					},
					{
						"short_time",
						errorLog.Date.ToString("H:mm:ss")
					},
					{
						"extra_short_time",
						errorLog.Date.ToShortTimeString()
					},
					{
						"datetime",
						errorLog.Date.ToString()
					},
					{ "class_name", errorLog.ClassName },
					{ "func_name", errorLog.FuncName },
					{ "message", errorLog.ExceptionMessage },
					{ "android_ime", errorLog.AndroidIme },
					{ "cpu_serial", errorLog.CpuSerial },
					{ "description", errorLog.Description },
					{ "hard_serial", errorLog.HardSerial },
					{
						"lk_serial",
						errorLog.LKSerial.ToString()
					},
					{
						"log_level",
						errorLog.Level.ToString()
					},
					{
						"log_level_justified",
						$"{errorLog.Level.ToString(),-12}"
					},
					{ "stack_trace", errorLog.StackTrace },
					{ "flow_stack_trace", errorLog.FlowStackTrace }
				};
			}
		}
		catch (Exception ex)
		{
			if (!ErrorHasBeenLogged)
			{
				ErrorHasBeenLogged = true;
				Instance.StartLog(ex, "", "DebugWriteConsole", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\WebErrorLog.cs", 727);
			}
		}
	}

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}

	public static async Task<List<WebErrorLog>> GetAllAsync(ErrorLogFilter filter, CancellationToken token = default(CancellationToken), int timeout = 15000)
	{
		List<WebErrorLog> res = new List<WebErrorLog>();
		try
		{
			if (filter == null)
			{
				return null;
			}
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"source",
					((short)filter.Source).ToString()
				},
				{
					"lkSerial",
					filter.LkSerial.ToString()
				},
				{ "ime", filter.AndroidIme },
				{
					"depId",
					filter.DepId.ToString()
				}
			};
			res = (await (apiurl + "ErrorLogFilter").GetFromApi<List<WebErrorLog>>(token, 3, headers, timeout)).value.Data;
		}
		catch (Exception ex)
		{
			Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\Entities\\WebErrorLog.cs", 753);
		}
		return res;
	}
}
