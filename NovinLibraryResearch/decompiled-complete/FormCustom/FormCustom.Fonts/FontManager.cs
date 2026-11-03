using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using PacketParser.Entities;

namespace FormCustom.Fonts;

public class FontManager
{
	[DllImport("gdi32", EntryPoint = "AddFontResource")]
	public static extern int AddFontResourceA(string lpFileName);

	[DllImport("gdi32.dll")]
	private static extern int AddFontResource(string lpszFilename);

	[DllImport("gdi32.dll")]
	private static extern int CreateScalableFontResource(uint fdwHidden, string lpszFontRes, string lpszFontFile, string lpszCurrentPath);

	private static bool IsFontExists(string fontName)
	{
		try
		{
			return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), fontName));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsFontExists", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Fonts\\FontManager.cs", 33);
			throw;
		}
	}

	private static void RegisterFont(string path, string contentFontName)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), contentFontName);
			if (!File.Exists(text))
			{
				File.Copy(path, text);
				PrivateFontCollection val = new PrivateFontCollection();
				val.AddFontFile(text);
				string name = ((FontCollection)val).Families[0].Name;
				AddFontResource(text);
				Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", name, contentFontName, RegistryValueKind.String);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RegisterFont", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Fonts\\FontManager.cs", 50);
		}
	}

	public static Task InstallFontsAsync(string sourcePath, List<string> fontList)
	{
		try
		{
			if (fontList == null || !fontList.Any())
			{
				return Task.CompletedTask;
			}
			foreach (string font in fontList)
			{
				if (!IsFontExists(font))
				{
					string path = Path.Combine(sourcePath, font);
					if (File.Exists(path))
					{
						RegisterFont(path, font);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InstallFontsAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Fonts\\FontManager.cs", 65);
		}
		return Task.CompletedTask;
	}
}
