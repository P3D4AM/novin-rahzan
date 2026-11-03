using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace PacketParser.Entities;

public class InternalLogger
{
	private static DateTime _LastError = DateTime.Now.AddMinutes(-2000.0);

	public static void LogException(Exception ex, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (!(DateTime.Now.Subtract(_LastError).TotalMinutes < 5.0))
			{
				_LastError = DateTime.Now;
				string text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				if (string.IsNullOrEmpty(text))
				{
					text = "d:\\errors\\WebServiceCore";
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				text = Path.Combine(text, Guid.NewGuid().ToString() + ".txt");
				File.WriteAllText(text, ex.Message + "\r\n__________________\r\n" + ex.StackTrace.ToString());
			}
		}
		catch (Exception ex2)
		{
			_ = ex2.Message;
		}
	}
}
