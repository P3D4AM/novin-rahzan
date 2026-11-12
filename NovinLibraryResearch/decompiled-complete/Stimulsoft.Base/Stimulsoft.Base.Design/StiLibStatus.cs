using System;
using System.IO;
using System.Reflection;

namespace Stimulsoft.Base.Design;

public static class StiLibStatus
{
	public static bool IsJsAvailable()
	{
		if (!IsAvailable(StiLibType.Php))
		{
			return IsAvailable(StiLibType.Js);
		}
		return true;
	}

	public static bool IsAvailable(StiLibType type)
	{
		string libPath = GetLibPath(type);
		if (libPath != null)
		{
			return Directory.Exists(libPath);
		}
		return false;
	}

	public static string GetJsLibPathIfExists()
	{
		if (IsAvailable(StiLibType.Js))
		{
			string libPath = GetLibPath(StiLibType.Js);
			if (libPath == null || !Directory.Exists(libPath))
			{
				return null;
			}
			return libPath;
		}
		if (IsAvailable(StiLibType.Php))
		{
			string text = Path.Combine(GetLibPath(StiLibType.Php), "JS");
			if (text == null || !Directory.Exists(text))
			{
				return null;
			}
			return text;
		}
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		directoryName = Path.Combine(directoryName, "Libs-JS");
		if (Directory.Exists(directoryName))
		{
			return directoryName;
		}
		return null;
	}

	public static string GetLibPath(StiLibType type)
	{
		string designerPath = StiDesignerAppStatus.GetDesignerPath(StiPlatformType.Wpf);
		if (designerPath == null)
		{
			return null;
		}
		string directoryName = Path.GetDirectoryName(designerPath);
		string path = Path.Combine(directoryName, "..", "..", "Libs");
		return type switch
		{
			StiLibType.Net => Path.Combine(path, "Reports.Net"), 
			StiLibType.Wpf => Path.Combine(path, "Reports.Wpf"), 
			StiLibType.Web => Path.Combine(path, "Reports.Web"), 
			StiLibType.Js => Path.Combine(path, "Reports.JS"), 
			StiLibType.Php => Path.Combine(path, "Reports.PHP"), 
			StiLibType.Java => Path.Combine(path, "Reports.Java"), 
			StiLibType.Flex => Path.Combine(path, "Reports.Flex"), 
			_ => throw new NotSupportedException(), 
		};
	}
}
