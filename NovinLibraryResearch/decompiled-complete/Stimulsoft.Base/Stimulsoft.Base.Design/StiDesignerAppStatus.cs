using System.IO;
using System.Reflection;

namespace Stimulsoft.Base.Design;

public static class StiDesignerAppStatus
{
	public static bool IsRunning
	{
		get
		{
			try
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly == null)
				{
					return false;
				}
				if (entryAssembly.FullName != null && !entryAssembly.FullName.Contains("PublicKeyToken=ebe6666cba19647a"))
				{
					return false;
				}
				string codeBase = entryAssembly.GetName().CodeBase;
				if (string.IsNullOrWhiteSpace(codeBase))
				{
					return false;
				}
				return codeBase.EndsWithInvariantIgnoreCase("designer.exe") || codeBase.EndsWithInvariantIgnoreCase("designer.wpf.exe") || codeBase.EndsWithInvariantIgnoreCase("designerv2.wpf.exe");
			}
			catch
			{
				return false;
			}
		}
	}

	public static bool IsAvailable(StiPlatformType type)
	{
		string designerPath = GetDesignerPath(type);
		if (designerPath == null)
		{
			return false;
		}
		try
		{
			return File.Exists(designerPath);
		}
		catch
		{
			return false;
		}
	}

	public static string GetDesignerPath(StiPlatformType type)
	{
		try
		{
			string text = null;
			try
			{
				text = Assembly.GetEntryAssembly()?.Location;
			}
			catch
			{
			}
			try
			{
				if (text == null)
				{
					text = Assembly.GetExecutingAssembly().Location;
				}
			}
			catch
			{
			}
			try
			{
				if (text == null)
				{
					text = Assembly.GetCallingAssembly().Location;
				}
			}
			catch
			{
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			string directoryName = Path.GetDirectoryName(text);
			return type switch
			{
				StiPlatformType.WinForms => Path.Combine(directoryName, "Designer.exe"), 
				StiPlatformType.Wpf => Path.Combine(directoryName, "DesignerV2.Wpf.exe"), 
				StiPlatformType.Js => Path.Combine(directoryName, "..", "JS", "Designer.exe"), 
				StiPlatformType.Flex => Path.Combine(directoryName, "..", "Flex", "DesignerFx.exe"), 
				_ => null, 
			};
		}
		catch
		{
		}
		return null;
	}
}
