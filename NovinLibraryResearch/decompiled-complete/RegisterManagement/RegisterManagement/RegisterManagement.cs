using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace RegisterManagement;

public class RegisterManagement
{
	private static void AxImage_OCX_NovinP(string strPath)
	{
		try
		{
			string text = strPath + "\\AxImage.ocx";
			Interaction.Shell("regsvr32.exe /s \"" + text + "\"", (AppWinStyle)0, false, 60000);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception value = ex;
			Console.WriteLine(value);
			ProjectData.ClearProjectError();
		}
	}

	private static void RealSvrOcxTcp_OCX_NovinP(string strPath)
	{
		try
		{
			string text = strPath + "\\RealSvrOcxTcp.ocx";
			Interaction.Shell("regsvr32.exe /s\"" + text + "\"", (AppWinStyle)0, false, 60000);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception value = ex;
			Console.WriteLine(value);
			ProjectData.ClearProjectError();
		}
	}

	public static void RegisterOCXs_NovinP(string strPath)
	{
		try
		{
			try
			{
				AxImage_OCX_NovinP(strPath);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception value = ex;
				Console.WriteLine(value);
				ProjectData.ClearProjectError();
			}
			try
			{
				RealSvrOcxTcp_OCX_NovinP(strPath);
			}
			catch (Exception ex2)
			{
				ProjectData.SetProjectError(ex2);
				Exception value2 = ex2;
				Console.WriteLine(value2);
				ProjectData.ClearProjectError();
			}
			Thread.Sleep(100);
			Thread.Sleep(100);
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToUpper().Contains("REGSVR"))
				{
					process.Kill();
					process.WaitForExit();
				}
			}
		}
		catch (Exception ex3)
		{
			ProjectData.SetProjectError(ex3);
			Exception value3 = ex3;
			Console.WriteLine(value3);
			ProjectData.ClearProjectError();
		}
	}
}
