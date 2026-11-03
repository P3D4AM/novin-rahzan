using System;
using System.IO;
using System.Windows.Forms;
using PacketParser.Entities;

namespace PayamakUi.Classes;

public class ClsStimul
{
	public static void CreateFile(byte[] Resource, string FileName)
	{
		try
		{
			string path = Path.Combine(Application.StartupPath, FileName);
			if (!File.Exists(path))
			{
				File.WriteAllBytes(path, Resource);
				return;
			}
			try
			{
				if (File.ReadAllBytes(path).Length != Resource.Length)
				{
					File.Delete(path);
					File.WriteAllBytes(path, Resource);
				}
			}
			catch (Exception)
			{
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CreateFile", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\ClsStimul.cs", 33);
		}
	}
}
