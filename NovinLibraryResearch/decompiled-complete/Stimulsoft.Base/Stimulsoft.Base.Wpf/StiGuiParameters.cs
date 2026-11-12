using System;
using System.IO;

namespace Stimulsoft.Base.Wpf;

public class StiGuiParameters
{
	private class StiOldGuiParametersInternal
	{
		public StiWpfOffice2013Theme Theme { get; set; }

		public StiSkinForeground Style { get; set; }
	}

	private class StiGuiParametersInternal
	{
		public StiSkinBackground Theme { get; set; }

		public StiSkinForeground Style { get; set; }
	}

	public const StiSkinBackground DefaultBackground = StiSkinBackground.White;

	public const StiSkinForeground DefaultForeground = StiSkinForeground.Blue;

	public StiSkinBackground Theme { get; private set; }

	public StiSkinForeground Style { get; private set; }

	public static string GetDefaultGuiParametersPath()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft", "Stimulsoft.Designer.GUI.json");
	}

	public static void SaveParameters(StiGuiParameters parameters)
	{
		try
		{
			string defaultGuiParametersPath = GetDefaultGuiParametersPath();
			string contents = parameters.SaveToString();
			File.WriteAllText(defaultGuiParametersPath, contents);
		}
		catch
		{
		}
	}

	public static StiGuiParameters LoadParameters()
	{
		try
		{
			string defaultGuiParametersPath = GetDefaultGuiParametersPath();
			if (!File.Exists(defaultGuiParametersPath))
			{
				return null;
			}
			string text = File.ReadAllText(defaultGuiParametersPath);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			StiGuiParameters stiGuiParameters = new StiGuiParameters();
			stiGuiParameters.LoadFromString(text);
			return stiGuiParameters;
		}
		catch
		{
		}
		return null;
	}

	public static StiGuiParameters LoadParametersOrDefault()
	{
		StiGuiParameters stiGuiParameters = LoadParameters();
		if (stiGuiParameters == null)
		{
			return new StiGuiParameters();
		}
		return stiGuiParameters;
	}

	public static void SaveThemeAndStyle(StiSkinBackground theme, StiSkinForeground style)
	{
		StiGuiParameters parameters = new StiGuiParameters
		{
			Theme = theme,
			Style = style
		};
		SaveParameters(parameters);
	}

	public string SaveToString()
	{
		return StiJsonHelper.SaveToJsonString(this);
	}

	public void LoadFromString(string str)
	{
		if (!string.IsNullOrEmpty(str))
		{
			if (str.Contains("Office2013"))
			{
				StiOldGuiParametersInternal stiOldGuiParametersInternal = new StiOldGuiParametersInternal();
				StiJsonHelper.LoadFromJsonString(str, stiOldGuiParametersInternal);
				Theme = ConvertToSkinBackground(stiOldGuiParametersInternal.Theme);
				Style = stiOldGuiParametersInternal.Style;
			}
			else
			{
				StiGuiParametersInternal stiGuiParametersInternal = new StiGuiParametersInternal();
				StiJsonHelper.LoadFromJsonString(str, stiGuiParametersInternal);
				Theme = stiGuiParametersInternal.Theme;
				Style = stiGuiParametersInternal.Style;
			}
		}
	}

	private static StiSkinBackground ConvertToSkinBackground(StiWpfOffice2013Theme theme)
	{
		return theme switch
		{
			StiWpfOffice2013Theme.Office2013DarkGray => StiSkinBackground.DarkGray, 
			StiWpfOffice2013Theme.Office2013LightGray => StiSkinBackground.LightGray, 
			StiWpfOffice2013Theme.Office2013VeryDarkGray => StiSkinBackground.VeryDarkGray, 
			StiWpfOffice2013Theme.Office2013White => StiSkinBackground.White, 
			_ => StiSkinBackground.White, 
		};
	}

	public StiGuiParameters()
	{
		Theme = StiSkinBackground.White;
		Style = StiSkinForeground.Blue;
	}
}
