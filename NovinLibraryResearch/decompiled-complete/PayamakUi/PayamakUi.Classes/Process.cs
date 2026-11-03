using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PacketParser.Entities;
using PayamakSettings;

namespace PayamakUi.Classes;

public class Process
{
	public static void StartGSMEngine()
	{
		try
		{
			if (CLSSMS.GsmEnable)
			{
				string fileName = Application.StartupPath + "\\\\PayamakGSM.exe";
				StopEngine();
				System.Diagnostics.Process.Start(fileName, "START");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartGSMEngine", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Process.cs", 23);
		}
	}

	public static void StopEngine()
	{
		try
		{
			if (!CLSSMS.GsmEnable)
			{
				return;
			}
			System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName("PayamakGSM");
			foreach (System.Diagnostics.Process process in processesByName)
			{
				try
				{
					process.Kill();
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "StopEngine", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Process.cs", 33);
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "StopEngine", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Process.cs", 35);
		}
	}

	public static void StartWebEngine()
	{
		try
		{
			string text = Path.Combine(Application.StartupPath, "PayamakWeb");
			if (Directory.Exists(text))
			{
				string text2 = Path.Combine(text, "PayamakWeb.exe");
				if (!System.Diagnostics.Process.GetProcesses().Any((System.Diagnostics.Process p) => p.ProcessName.ToLower().Contains("payamakweb")) && File.Exists(text2))
				{
					System.Diagnostics.Process.Start(text2);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartWebEngine", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Process.cs", 49);
		}
	}

	public static void ExitPayamak()
	{
		try
		{
			if (!CLSSMS.GsmEnable)
			{
				return;
			}
			System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName("Payamak");
			foreach (System.Diagnostics.Process process in processesByName)
			{
				try
				{
					process.Kill();
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ExitPayamak", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Process.cs", 60);
				}
			}
			processesByName = System.Diagnostics.Process.GetProcessesByName("Payamak.vshost");
			foreach (System.Diagnostics.Process process2 in processesByName)
			{
				try
				{
					process2.Kill();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ExitPayamak", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Process.cs", 64);
				}
			}
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "ExitPayamak", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Process.cs", 66);
		}
	}
}
