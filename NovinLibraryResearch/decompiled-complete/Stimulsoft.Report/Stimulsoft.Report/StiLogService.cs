#define TRACE
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report;

public class StiLogService : StiService, IDisposable
{
	private bool disposed;

	private static bool logEnabled;

	private string logPath = "Report.log";

	private FileStream logStream;

	private StreamWriter logWriter;

	private bool first = true;

	public override string ServiceCategory => StiLocalization.Get("Services", "categorySystem");

	public override Type ServiceType => typeof(StiLogService);

	[StiServiceParam]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool ClearLogOnStart { get; set; } = true;


	public static bool LogEnabled
	{
		get
		{
			return logEnabled;
		}
		set
		{
			logEnabled = value;
			if (value)
			{
				return;
			}
			foreach (StiLogService log in StiOptions.Services.Logs)
			{
				log.CloseWriter();
			}
		}
	}

	public static bool TraceEnabled { get; set; }

	[StiServiceParam]
	[DefaultValue("Report.log")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual string LogPath
	{
		get
		{
			return logPath;
		}
		set
		{
			if (logPath != value)
			{
				CloseWriter();
				logPath = value;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed && disposing)
		{
			CloseWriter();
		}
		disposed = true;
	}

	public virtual void WriteLogString(string s)
	{
		if (((StiService)this).ServiceEnabled)
		{
			try
			{
				OpenWriter();
				logStream.Seek(0L, SeekOrigin.End);
				logWriter.WriteLine(s);
			}
			catch
			{
			}
		}
	}

	public static void Write(Type type, string message)
	{
		Write(type.Name + ": " + message);
	}

	public static void Write(Type type, Exception e)
	{
		Write(type.Name + ": Method : [" + e.TargetSite.Name + "] : " + e.Message + "\r\n====================================================\r\n" + e.StackTrace + "\r\n====================================================");
	}

	public static void Write(string message)
	{
		try
		{
			message = DateTime.Today.ToString("yyyy:MM:dd") + ", " + DateTime.Now.ToString("HH:mm") + ": " + message;
		}
		catch (ArgumentOutOfRangeException)
		{
			message = DateTime.Today.ToString() + ", " + DateTime.Now.ToString() + ": " + message;
		}
		if (LogEnabled && !StiOptions.Configuration.IsWeb)
		{
			StiLogService stiLogService = StiOptions.Services.Logs.FirstOrDefault();
			if (stiLogService == null)
			{
				return;
			}
			stiLogService.WriteLogString(message);
		}
		if (TraceEnabled)
		{
			Trace.WriteLine(message);
		}
	}

	public static void Write(Exception exception)
	{
		try
		{
			_ = exception.InnerException;
		}
		catch
		{
		}
	}

	private void CloseWriter()
	{
		if (logWriter != null)
		{
			try
			{
				logWriter.Close();
				logWriter = null;
				logStream.Close();
				logStream = null;
			}
			catch
			{
			}
		}
	}

	private void OpenWriter()
	{
		if (logWriter != null)
		{
			return;
		}
		try
		{
			string path = StiPath.GetPath(LogPath);
			if (first && ClearLogOnStart)
			{
				first = false;
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			StiFileUtils.ProcessReadOnly(path);
			logStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
			logWriter = new StreamWriter(logStream);
			logWriter.AutoFlush = true;
		}
		catch
		{
		}
	}
}
