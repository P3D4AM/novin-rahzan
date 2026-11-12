using System.Drawing;
using System.Linq;

namespace Stimulsoft.Base.Drawing;

public static class StiColor
{
	public static Color Get(string color)
	{
		if (string.IsNullOrWhiteSpace(color))
		{
			return Color.Transparent;
		}
		color = color.Trim();
		if (StiColors.NameColors.Contains(CapitalFirstLetter(color)))
		{
			return Color.FromName(color);
		}
		if (color.IndexOfInvariant(",") != -1)
		{
			string[] array = color.Split(new char[1] { ',' });
			if (array.Length == 4)
			{
				return Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()), int.Parse(array[3].Trim()));
			}
			return Color.FromArgb(int.Parse(array[0].Trim()), int.Parse(array[1].Trim()), int.Parse(array[2].Trim()));
		}
		if (!color.StartsWith("#"))
		{
			color = "#" + color;
		}
		try
		{
			return ColorTranslator.FromHtml(color);
		}
		catch
		{
		}
		return Color.Transparent;
	}

	public static Color[] Get(params string[] colors)
	{
		return colors.ToList().Select(Get).ToArray();
	}

	private static string CapitalFirstLetter(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
	}
}
