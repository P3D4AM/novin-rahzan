using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Microsoft.Web.WebView2.Core;

namespace Stimulsoft.BlocklyEditor.Wpf;

public class StiWebView2Installer
{
	internal static string GetWebView2Version()
	{
		try
		{
			return CoreWebView2Environment.GetAvailableBrowserVersionString((string)null);
		}
		catch (Exception)
		{
			return "";
		}
	}

	public static bool InstallWebView2RuntimeAsync()
	{
		string path = "MicrosoftEdgeWebview2Setup.exe";
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		WebClient webClient = new WebClient();
		string text = Path.Combine(Path.GetTempPath(), path);
		webClient.DownloadFile("https://go.microsoft.com/fwlink/p/?LinkId=2124703", text);
		string fileName = Path.Combine(text);
		Process process = Process.Start(fileName);
		if (process != null)
		{
			process.WaitForExit();
			return process.ExitCode == 0;
		}
		return false;
	}
}
