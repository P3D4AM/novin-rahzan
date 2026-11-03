using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Stimulsoft.Report;

public static class StiReportCompilationCache
{
	public static string GetPath(StiReport report)
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft", "CompiledReports", RuntimeEnvironment.GetSystemVersion());
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return Path.Combine(text, report.GetReportAssemblyCacheName());
	}
}
