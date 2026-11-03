using System;
using System.Globalization;
using System.Threading;

namespace Stimulsoft.Report.Helpers;

public static class StiAbbreviationNumberFormatHelper
{
	public static string Format(double value)
	{
		return Format((decimal)value);
	}

	public static string Format(decimal value)
	{
		string postfix;
		decimal num = Format(value, out postfix);
		if (num == 0m)
		{
			return "0";
		}
		if (postfix != null && postfix.Length == 0)
		{
			return Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
		}
		return $"{num}{postfix}";
	}

	public static decimal Format(decimal value, out string postfix, int decimalDigits = 0, int? totalNumberCapacity = null)
	{
		postfix = "";
		if (value == 0m)
		{
			return 0m;
		}
		bool flag = false;
		if (value < 0m)
		{
			flag = true;
			value = Math.Abs(value);
		}
		int num = 0;
		for (decimal num2 = value; num2 >= 1000m; num2 /= 1000m)
		{
			num++;
		}
		decimal num3;
		if (num < 0 || num > 21)
		{
			num3 = value;
		}
		else
		{
			bool flag2 = !Thread.CurrentThread.CurrentCulture.Name.Equals("ru-RU");
			switch (num)
			{
			case 1:
				postfix = (flag2 ? "K" : " тыс.");
				break;
			case 2:
				postfix = (flag2 ? "M" : " млн");
				break;
			case 3:
				postfix = (flag2 ? "B" : " млрд");
				break;
			case 4:
				postfix = (flag2 ? "T" : " трлн");
				break;
			case 5:
				postfix = (flag2 ? "q" : " квдрлн");
				break;
			case 6:
				postfix = (flag2 ? "Q" : " квнтлн");
				break;
			case 7:
				postfix = (flag2 ? "s" : " скстлн");
				break;
			case 8:
				postfix = (flag2 ? "S" : " сптлн");
				break;
			case 9:
				postfix = (flag2 ? "O" : " ктлн");
				break;
			}
			num3 = value / (decimal)Math.Pow(1000.0, num);
			if (decimalDigits == 0 && totalNumberCapacity.HasValue)
			{
				decimalDigits = ((num != 0) ? (num * 4 - totalNumberCapacity.GetValueOrDefault()) : totalNumberCapacity.GetValueOrDefault());
			}
			if (decimalDigits > 0)
			{
				num3 = Math.Truncate(num3);
				decimal num4 = Math.Round(value / (decimal)Math.Pow(1000.0, num), decimalDigits);
				decimal num5 = num4 - Math.Truncate(num4);
				if (num5 == 0m && num4 - num3 == 1m)
				{
					num5 = 1m;
				}
				num3 += num5;
			}
			else
			{
				num3 = Math.Round(num3);
			}
		}
		if (flag)
		{
			num3 = -num3;
		}
		return num3;
	}
}
