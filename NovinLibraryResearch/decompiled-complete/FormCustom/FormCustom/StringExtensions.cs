using System;
using System.Drawing;
using System.Globalization;
using PacketParser.Entities;

namespace FormCustom;

public static class StringExtensions
{
	public static Color ToColor(this string colorNameOrARGBNumber, Color defColor = default(Color))
	{
		int num = 0;
		try
		{
			if (string.IsNullOrEmpty(colorNameOrARGBNumber))
			{
				WebErrorLog.Instance.StartLog(".تو اکستنشن دریافت رنگ ، استرینگ داده شده خالی است بررسی شود\n ToColorFromColorName()", "", "ToColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\StringExtensions.cs", 17);
				return defColor;
			}
			num = 1;
			if (colorNameOrARGBNumber.Substring(0, 1) == "#")
			{
				return ColorTranslator.FromHtml(colorNameOrARGBNumber);
			}
			num = 2;
			if (colorNameOrARGBNumber.Length == 6)
			{
				num = 3;
				if (int.TryParse(colorNameOrARGBNumber.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
				{
					num = 4;
					if (int.TryParse(colorNameOrARGBNumber.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result2))
					{
						num = 5;
						if (int.TryParse(colorNameOrARGBNumber.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result3))
						{
							num = 6;
							return Color.FromArgb(255, result, result2, result3);
						}
					}
				}
			}
			num = 7;
			if (colorNameOrARGBNumber.Length == 8)
			{
				num = 8;
				if (int.TryParse(colorNameOrARGBNumber.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result4))
				{
					num = 9;
					if (int.TryParse(colorNameOrARGBNumber.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result5))
					{
						num = 10;
						if (int.TryParse(colorNameOrARGBNumber.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result6))
						{
							num = 11;
							if (int.TryParse(colorNameOrARGBNumber.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result7))
							{
								num = 12;
								return Color.FromArgb(result4, result5, result6, result7);
							}
						}
					}
				}
			}
			num = 13;
			return Color.FromName(colorNameOrARGBNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"colorNameOrRGBNumber={colorNameOrARGBNumber}, defaultColor={defColor},line={num}", "ToColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\StringExtensions.cs", 77);
			return defColor;
		}
	}
}
