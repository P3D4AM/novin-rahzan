using System;
using System.IO;
using System.Windows.Forms;

namespace Tele;

public class FileManager
{
	public static void Read(string input)
	{
		try
		{
			ALL(input);
			if (!File.Exists(Application.StartupPath + "\\Input.txt"))
			{
				File.Create(Application.StartupPath + "\\Input.txt");
			}
			File.AppendAllText(Application.StartupPath + "\\Input.txt", "\r\n" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "\r\n" + input);
		}
		catch (Exception)
		{
		}
	}

	public static void Write(string output)
	{
		try
		{
			ALL(output);
			if (!File.Exists(Application.StartupPath + "\\Output.txt"))
			{
				File.Create(Application.StartupPath + "\\Output.txt");
			}
			File.AppendAllText(Application.StartupPath + "\\Output.txt", "\r\n" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "\r\n" + output);
		}
		catch (Exception)
		{
		}
	}

	public static void ALL(string text)
	{
		try
		{
			if (!File.Exists(Application.StartupPath + "\\ALL.txt"))
			{
				File.Create(Application.StartupPath + "\\ALL.txt");
			}
			File.AppendAllText(Application.StartupPath + "\\ALL.txt", "\r\n" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "\r\n" + text);
		}
		catch (Exception)
		{
		}
	}
}
