using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using PacketParser.Entities;

namespace PacketParser.Performance.ErroHandler;

public class Logger : ILogger
{
	private class tempClass
	{
		public string message { get; set; }

		public EnGroup group { get; set; }

		public DateTime? date { get; set; }

		public int trycount { get; set; }

		public string memberName { get; set; }

		public string sourceFilePath { get; set; }

		public int sourceLineNumber { get; set; }
	}

	private class NestedLogger
	{
		internal static readonly Logger instance;

		static NestedLogger()
		{
			instance = new Logger();
		}
	}

	private static bool initialized = false;

	private static SemaphoreSlim _WriteFileSemaphore = new SemaphoreSlim(1, 1);

	private static Task _saveTask = null;

	private static BufferBlock<tempClass> _buffer = new BufferBlock<tempClass>();

	public static string StartUpPath { get; private set; }

	public static string FileName { get; private set; }

	public static Logger LoggerInstance => NestedLogger.instance;

	public static void init(string startUpPath, string fileName = "AccountingEventLog.txt", bool archive = false)
	{
		int num = 0;
		try
		{
			num = 1;
			if (!Directory.Exists(startUpPath))
			{
				Directory.CreateDirectory(startUpPath);
			}
			num = 2;
			startUpPath = Path.Combine(startUpPath, "log");
			if (!Directory.Exists(startUpPath))
			{
				Directory.CreateDirectory(startUpPath);
			}
			num = 3;
			DateTime now = DateTime.Now;
			if (archive)
			{
				fileName = $"{now.Year}_{now.Month}_{now.Day} {now.Hour}_{now.Minute}_{now.Second} {fileName}";
			}
			num = 7;
			StartUpPath = startUpPath;
			FileName = fileName;
			num = 10;
			string[] files = Directory.GetFiles(StartUpPath);
			if (files != null && files.Any())
			{
				string[] array = files;
				foreach (string text in array)
				{
					num = 15;
					if (new FileInfo(text).CreationTime.AddDays(7.0) < DateTime.Now)
					{
						try
						{
							File.Delete(text);
						}
						catch
						{
						}
					}
					num = 20;
				}
			}
			num = 30;
			string path = Path.Combine(StartUpPath, FileName);
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch
			{
			}
			try
			{
				if (Directory.Exists(path))
				{
					Directory.Delete(path);
				}
			}
			catch
			{
			}
			num = 40;
			initialized = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "init", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\ErroHandler\\Logger.cs", 61);
		}
	}

	public void SaveLog(string message, EnGroup group = EnGroup.None, DateTime? date = null, short trycount = 10, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		tempClass item = new tempClass
		{
			message = message,
			group = group,
			date = date,
			trycount = trycount,
			memberName = memberName,
			sourceFilePath = sourceFilePath,
			sourceLineNumber = sourceLineNumber
		};
		_buffer.Post(item);
		if (_saveTask == null && initialized)
		{
			_saveTask = Task.Run((Func<Task?>)SaveLoopAsync);
		}
	}

	private async Task SaveLoopAsync()
	{
		_ = 1;
		try
		{
			while (true)
			{
				await SaveLog_(await _buffer.ReceiveAsync());
			}
		}
		catch
		{
		}
	}

	private async Task SaveLog_(tempClass data)
	{
		await _WriteFileSemaphore.WaitAsync();
		try
		{
			int num = 0;
			object obj = default(object);
			try
			{
				data.date = data.date ?? DateTime.Now;
				if (!string.IsNullOrEmpty(data.sourceFilePath))
				{
					new FileInfo(data.sourceFilePath);
					List<string> list = data.sourceFilePath.SplitString();
					data.sourceFilePath = list[checked(list.Count - 2)];
				}
				string text = $"\r\n{data.date}:\t{data.sourceFilePath}.{data.memberName} Line:{data.sourceLineNumber}\t{data.message}\r\n______________________________";
				string text2 = StartUpPath;
				if (data.group != EnGroup.None)
				{
					text2 = Path.Combine(text2, data.group.GetDisplay());
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				text2 = Path.Combine(text2, FileName);
				if (File.Exists(text2))
				{
					File.AppendAllLines(text2, new List<string> { text });
				}
				else
				{
					File.WriteAllText(text2, text);
				}
			}
			catch (Exception ex)
			{
				obj = ex;
				num = 1;
			}
			if (num == 1)
			{
				_ = (Exception)obj;
				_buffer.Post(data);
				await Task.Delay(60000);
			}
		}
		finally
		{
			_WriteFileSemaphore.Release();
		}
	}
}
