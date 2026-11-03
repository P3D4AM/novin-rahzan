using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Stimulsoft.Data.Functions;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report;

public sealed class Func
{
	public enum Gender
	{
		Masculine,
		Feminine,
		Neutral
	}

	public abstract class BaseCurrency
	{
		public abstract Gender Gender { get; }

		public abstract Gender CentsGender { get; }
	}

	public sealed class Ru
	{
		public abstract class Currency : BaseCurrency
		{
			protected abstract string[] Dollars { get; }

			protected abstract string[] Cents { get; }

			public string DollarOne => Dollars[0];

			public string DollarTwo => Dollars[1];

			public string DollarFive => Dollars[2];

			public string CentOne => Cents[0];

			public string CentTwo => Cents[1];

			public string CentFive => Cents[2];
		}

		public class RURCurrency : Currency
		{
			private string[] dollars = new string[3] { "рубль", "рубля", "рублей" };

			private string[] cents = new string[3] { "копейка", "копейки", "копеек" };

			public override Gender Gender => Gender.Masculine;

			public override Gender CentsGender => Gender.Feminine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		public class USDCurrency : Currency
		{
			private string[] dollars = new string[3] { "доллар", "доллара", "долларов" };

			private string[] cents = new string[3] { "цент", "цента", "центов" };

			public override Gender Gender => Gender.Masculine;

			public override Gender CentsGender => Gender.Masculine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		public class EURCurrency : Currency
		{
			private string[] dollars = new string[3] { "евро", "евро", "евро" };

			private string[] cents = new string[3] { "цент", "цента", "центов" };

			public override Gender Gender => Gender.Neutral;

			public override Gender CentsGender => Gender.Masculine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		public class UAHCurrency : Currency
		{
			private string[] dollars = new string[3] { "гривна", "гривны", "гривен" };

			private string[] cents = new string[3] { "копейка", "копейки", "копеек" };

			public override Gender Gender => Gender.Feminine;

			public override Gender CentsGender => Gender.Feminine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		public class KZTCurrency : Currency
		{
			private string[] dollars = new string[3] { "тенге", "тенге", "тенге" };

			private string[] cents = new string[3] { "тиын", "тиына", "тиынов" };

			public override Gender Gender => Gender.Masculine;

			public override Gender CentsGender => Gender.Feminine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		private static Hashtable Currencies;

		private static string[] months;

		private static string[] units;

		private static string[] tens;

		private static string[] hundreds;

		private static string[,] gendered;

		public static void RegisterCurrency(Currency currency, string currencyName)
		{
			Currencies[currencyName.ToUpper(CultureInfo.InvariantCulture)] = currency;
		}

		private static Currency GetCurrency(string currencyName)
		{
			if (!(Currencies[currencyName.ToUpper(CultureInfo.InvariantCulture)] is Currency result))
			{
				throw new ArgumentException($"Currency '{currencyName}' is not registered");
			}
			return result;
		}

		private static void AddUnits(StringBuilder sb, long value, Gender gender)
		{
			if (value != 0L)
			{
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				if (value < 3)
				{
					sb.Append(gendered[(int)checked((nint)unchecked(value - 1)), (int)gender]);
				}
				else
				{
					sb.Append(units[value - 1]);
				}
			}
		}

		private static void AddTens(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				sb.Append(tens[value - 1]);
			}
		}

		private static void AddHundreds(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				sb.Append(hundreds[value - 1]);
			}
		}

		private static void AddThousand(StringBuilder sb, long value, Gender gender)
		{
			AddHundreds(sb, value / 100);
			value %= 100;
			if (value < 20)
			{
				AddUnits(sb, value, gender);
				return;
			}
			AddTens(sb, value / 10);
			AddUnits(sb, value % 10, gender);
		}

		private static void AddRank(StringBuilder sb, ref long rank, ref long value, string one, string two, string five, Gender gender)
		{
			long num = value / rank;
			if (num > 0)
			{
				AddThousand(sb, num, gender);
				long num2 = num % 10;
				long num3 = num % 100;
				string empty = string.Empty;
				if (num3 >= 11 && num3 < 20)
				{
					empty = five;
				}
				else
				{
					switch (num2)
					{
					case 1L:
						empty = one;
						break;
					case 2L:
					case 3L:
					case 4L:
						empty = two;
						break;
					default:
						empty = five;
						break;
					}
				}
				if (num > 10 && num < 20)
				{
					empty = five;
				}
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				sb.Append(empty);
				value %= rank;
			}
			rank /= 1000L;
		}

		public static string NumToStr(double value)
		{
			return NumToStr((decimal)value);
		}

		public static string NumToStr(double value, bool uppercase)
		{
			return NumToStr((long)value, uppercase);
		}

		public static string NumToStr(double value, bool uppercase, Gender gender)
		{
			return NumToStr((long)value, uppercase, gender);
		}

		public static string NumToStr(decimal value)
		{
			return NumToStr((long)value);
		}

		public static string NumToStr(decimal value, bool uppercase)
		{
			return NumToStr((long)value, uppercase);
		}

		public static string NumToStr(decimal value, bool uppercase, Gender gender)
		{
			return NumToStr((long)value, uppercase, gender);
		}

		public static string NumToStr(long value)
		{
			return NumToStr(value, uppercase: true, Gender.Masculine);
		}

		public static string NumToStr(long value, bool uppercase)
		{
			return NumToStr(value, uppercase, Gender.Masculine);
		}

		public static string NumToStr(long value, bool uppercase, Gender gender)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (value == 0L)
			{
				stringBuilder.Append("ноль");
			}
			else
			{
				if (value < 0)
				{
					stringBuilder.Append("минус");
					value = Math.Abs(value);
				}
				long rank = 1000000000000000000L;
				AddRank(stringBuilder, ref rank, ref value, "квинтильон", "квинтильона", "квинтильонов", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "квадрильон", "квадрильона", "квадрильонов", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "триллион", "триллиона", "триллионов", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "миллиард", "миллиарда", "миллиардов", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "миллион", "миллиона", "миллионов", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "тысяча", "тысячи", "тысяч", Gender.Feminine);
				AddThousand(stringBuilder, value, gender);
			}
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}

		public static string CurrToStr(long value, bool cents)
		{
			return CurrToStr((decimal)value, "RUR", cents);
		}

		public static string CurrToStr(long value, string currency, bool cents)
		{
			return CurrToStr((decimal)value, uppercase: true, currency, cents);
		}

		public static string CurrToStr(double value, bool cents)
		{
			return CurrToStr(value, "RUR", cents);
		}

		public static string CurrToStr(decimal value, bool cents)
		{
			return CurrToStr(value, "RUR", cents);
		}

		public static string CurrToStr(double value, string currency, bool cents)
		{
			return CurrToStr((decimal)value, uppercase: true, currency, cents);
		}

		public static string CurrToStr(decimal value, string currency, bool cents)
		{
			return CurrToStr(value, uppercase: true, currency, cents);
		}

		public static string CurrToStr(double value, bool uppercase, string currency, bool cents)
		{
			return CurrToStr((decimal)value, uppercase, currency, cents);
		}

		public static string CurrToStr(decimal value, bool uppercase, string currency, bool cents)
		{
			decimal num = Math.Truncate(value);
			long num2 = 0L;
			if (StiOptions.Engine.UseRoundForToCurrencyWordsFunctions)
			{
				num2 = Math.Abs((long)Math.Round((value - (decimal)(long)num) * 100m));
				if (num2 > 99)
				{
					num2 = 0L;
					++num;
				}
			}
			else
			{
				num2 = Math.Abs((long)((value - (decimal)(long)num) * 100m));
			}
			string text = $"{NumToStr(num, uppercase, GetCurrency(currency).Gender)} {Decline(num, currency)}";
			if (cents)
			{
				text += $" {num2:d2}";
				text += $" {Decline(num2, currency, cents: true)}";
			}
			return text;
		}

		public static string CurrToStr(double value)
		{
			return CurrToStr((decimal)value, "RUR");
		}

		public static string CurrToStr(decimal value)
		{
			return CurrToStr(value, "RUR");
		}

		public static string CurrToStr(double value, string currency)
		{
			return CurrToStr((decimal)value, uppercase: true, currency);
		}

		public static string CurrToStr(decimal value, string currency)
		{
			return CurrToStr(value, uppercase: true, currency);
		}

		public static string CurrToStr(long value, bool uppercase, string currency)
		{
			return CurrToStr((decimal)value, uppercase, currency, cents: true);
		}

		public static string CurrToStr(double value, bool uppercase, string currency)
		{
			return CurrToStr((decimal)value, uppercase, currency, cents: true);
		}

		public static string CurrToStr(decimal value, bool uppercase, string currency)
		{
			return CurrToStr((double)value, uppercase, currency);
		}

		public static string Decline(decimal value, string one, string two, string five)
		{
			return Decline((long)value, one, two, five);
		}

		public static string Decline(double value, string one, string two, string five)
		{
			return Decline((long)value, one, two, five);
		}

		public static string Decline(long value, string one, string two, string five)
		{
			long num = value % 100;
			if (num >= 10 && num < 20)
			{
				return five;
			}
			switch (num % 10)
			{
			case 1L:
				return one;
			case 2L:
			case 3L:
			case 4L:
				return two;
			default:
				return five;
			}
		}

		public static string Decline(decimal value, string currency)
		{
			return Decline(value, currency, cents: false);
		}

		public static string Decline(double value, string currency)
		{
			return Decline(value, currency, cents: false);
		}

		public static string Decline(long value, string currency)
		{
			return Decline(value, currency, cents: false);
		}

		public static string Decline(decimal value, string currency, bool cents)
		{
			return Decline((long)value, currency, cents);
		}

		public static string Decline(double value, string currency, bool cents)
		{
			return Decline((long)value, currency, cents);
		}

		public static string Decline(long value, string currency, bool cents)
		{
			Currency currency2 = GetCurrency(currency);
			if (cents)
			{
				return Decline(value, currency2.CentOne, currency2.CentTwo, currency2.CentFive);
			}
			return Decline(value, currency2.DollarOne, currency2.DollarTwo, currency2.DollarFive);
		}

		public static string DateToStr(DateTime date)
		{
			return DateToStr(date, uppercase: false);
		}

		public static string DateToStr(DateTime date, bool uppercase)
		{
			StringBuilder stringBuilder = new StringBuilder($"{date.Day} {months[date.Month - 1]} {date.Year}");
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}

		static Ru()
		{
			Currencies = new Hashtable();
			months = new string[12]
			{
				"января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября",
				"ноября", "декабря"
			};
			units = new string[19]
			{
				"один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять",
				"одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
			};
			tens = new string[9] { "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
			hundreds = new string[9] { "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
			gendered = new string[2, 3]
			{
				{ "один", "одна", "одно" },
				{ "два", "две", "два" }
			};
			RegisterCurrency(new EURCurrency(), "EUR");
			RegisterCurrency(new USDCurrency(), "USD");
			RegisterCurrency(new RURCurrency(), "RUR");
			RegisterCurrency(new UAHCurrency(), "UAH");
			RegisterCurrency(new KZTCurrency(), "KZT");
		}

		private Ru()
		{
		}
	}

	public sealed class Ua
	{
		public abstract class Currency : BaseCurrency
		{
			protected abstract string[] Dollars { get; }

			protected abstract string[] Cents { get; }

			public string DollarOne => Dollars[0];

			public string DollarTwo => Dollars[1];

			public string DollarFive => Dollars[2];

			public string CentOne => Cents[0];

			public string CentTwo => Cents[1];

			public string CentFive => Cents[2];
		}

		public class RURCurrency : Currency
		{
			private string[] dollars = new string[3] { "рубль", "рубля", "рублів" };

			private string[] cents = new string[3] { "копійка", "копійки", "копійок" };

			public override Gender Gender => Gender.Masculine;

			public override Gender CentsGender => Gender.Feminine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		public class USDCurrency : Currency
		{
			private string[] dollars = new string[3] { "долар", "долара", "доларів" };

			private string[] cents = new string[3] { "цент", "цента", "центів" };

			public override Gender Gender => Gender.Masculine;

			public override Gender CentsGender => Gender.Masculine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		public class EURCurrency : Currency
		{
			private string[] dollars = new string[3] { "євро", "євро", "євро" };

			private string[] cents = new string[3] { "цент", "цента", "центів" };

			public override Gender Gender => Gender.Neutral;

			public override Gender CentsGender => Gender.Masculine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		public class UAHCurrency : Currency
		{
			private string[] dollars = new string[3] { "гривня", "гривні", "гривень" };

			private string[] cents = new string[3] { "копійка", "копійки", "копійок" };

			public override Gender Gender => Gender.Feminine;

			public override Gender CentsGender => Gender.Feminine;

			protected override string[] Dollars => dollars;

			protected override string[] Cents => cents;
		}

		private static Hashtable Currencies;

		private static string[] months;

		private static string[] units;

		private static string[] tens;

		private static string[] hundreds;

		private static string[,] gendered;

		public static void RegisterCurrency(Currency currency, string currencyName)
		{
			Currencies[currencyName.ToUpper(CultureInfo.InvariantCulture)] = currency;
		}

		private static Currency GetCurrency(string currencyName)
		{
			if (!(Currencies[currencyName.ToUpper(CultureInfo.InvariantCulture)] is Currency result))
			{
				throw new ArgumentException($"Currency '{currencyName}' is not registered");
			}
			return result;
		}

		private static void AddUnits(StringBuilder sb, long value, Gender gender)
		{
			if (value != 0L)
			{
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				if (value < 3)
				{
					sb.Append(gendered[(int)checked((nint)unchecked(value - 1)), (int)gender]);
				}
				else
				{
					sb.Append(units[value - 1]);
				}
			}
		}

		private static void AddTens(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				sb.Append(tens[value - 1]);
			}
		}

		private static void AddHundreds(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				sb.Append(hundreds[value - 1]);
			}
		}

		private static void AddThousand(StringBuilder sb, long value, Gender gender)
		{
			AddHundreds(sb, value / 100);
			value %= 100;
			if (value < 20)
			{
				AddUnits(sb, value, gender);
				return;
			}
			AddTens(sb, value / 10);
			AddUnits(sb, value % 10, gender);
		}

		private static void AddRank(StringBuilder sb, ref long rank, ref long value, string one, string two, string five, Gender gender)
		{
			long num = value / rank;
			if (num > 0)
			{
				AddThousand(sb, num, gender);
				long num2 = num % 10;
				string empty = string.Empty;
				switch (num2)
				{
				case 1L:
					empty = one;
					break;
				case 2L:
				case 3L:
				case 4L:
					empty = two;
					break;
				default:
					empty = five;
					break;
				}
				if (num > 10 && num < 20)
				{
					empty = five;
				}
				if (sb.Length > 0)
				{
					sb.Append(" ");
				}
				sb.Append(empty);
				value %= rank;
			}
			rank /= 1000L;
		}

		public static string NumToStr(double value)
		{
			return NumToStr((decimal)value);
		}

		public static string NumToStr(double value, bool uppercase)
		{
			return NumToStr((long)value, uppercase);
		}

		public static string NumToStr(double value, bool uppercase, Gender gender)
		{
			return NumToStr((long)value, uppercase, gender);
		}

		public static string NumToStr(decimal value)
		{
			return NumToStr((long)value);
		}

		public static string NumToStr(decimal value, bool uppercase)
		{
			return NumToStr((long)value, uppercase);
		}

		public static string NumToStr(decimal value, bool uppercase, Gender gender)
		{
			return NumToStr((long)value, uppercase, gender);
		}

		public static string NumToStr(long value)
		{
			return NumToStr(value, uppercase: true, Gender.Masculine);
		}

		public static string NumToStr(long value, bool uppercase)
		{
			return NumToStr(value, uppercase, Gender.Masculine);
		}

		public static string NumToStr(long value, bool uppercase, Gender gender)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (value == 0L)
			{
				stringBuilder.Append("ноль");
			}
			else
			{
				if (value < 0)
				{
					stringBuilder.Append("минус");
					value = Math.Abs(value);
				}
				long rank = 1000000000000000000L;
				AddRank(stringBuilder, ref rank, ref value, "квінтильйон", "квінтильйона", "квінтильйонів", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "квадрильйон", "квадрильйона", "квадрильйонів", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "трильйон", "трильйона", "трильйонів", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "мільярд", "мільярда", "мільярдів", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "мільйон", "мільйона", "мільйонів", Gender.Masculine);
				AddRank(stringBuilder, ref rank, ref value, "тисяча", "тисячі", "тисяч", Gender.Feminine);
				AddThousand(stringBuilder, value, gender);
			}
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}

		public static string CurrToStr(long value, bool cents)
		{
			return CurrToStr((decimal)value, "UAH", cents);
		}

		public static string CurrToStr(long value, string currency, bool cents)
		{
			return CurrToStr((decimal)value, uppercase: true, currency, cents);
		}

		public static string CurrToStr(double value, bool cents)
		{
			return CurrToStr(value, "UAH", cents);
		}

		public static string CurrToStr(decimal value, bool cents)
		{
			return CurrToStr(value, "UAH", cents);
		}

		public static string CurrToStr(double value, string currency, bool cents)
		{
			return CurrToStr((decimal)value, uppercase: true, currency, cents);
		}

		public static string CurrToStr(decimal value, string currency, bool cents)
		{
			return CurrToStr(value, uppercase: true, currency, cents);
		}

		public static string CurrToStr(double value, bool uppercase, string currency, bool cents)
		{
			return CurrToStr((decimal)value, uppercase, currency, cents);
		}

		public static string CurrToStr(decimal value, bool uppercase, string currency, bool cents)
		{
			decimal num = Math.Truncate(value);
			long num2 = 0L;
			if (StiOptions.Engine.UseRoundForToCurrencyWordsFunctions)
			{
				num2 = (long)Math.Round((value - (decimal)(long)num) * 100m);
				if (num2 > 99)
				{
					num2 = 0L;
					++num;
				}
			}
			else
			{
				num2 = (long)((value - (decimal)(long)num) * 100m);
			}
			string text = $"{NumToStr(num, uppercase, GetCurrency(currency).Gender)} {Decline(num, currency)}";
			if (cents)
			{
				text += $" {num2:d2}";
				text += $" {Decline(num2, currency, cents: true)}";
			}
			return text;
		}

		public static string CurrToStr(double value)
		{
			return CurrToStr((decimal)value, "UAH");
		}

		public static string CurrToStr(decimal value)
		{
			return CurrToStr(value, "UAH");
		}

		public static string CurrToStr(double value, string currency)
		{
			return CurrToStr((decimal)value, uppercase: true, currency);
		}

		public static string CurrToStr(decimal value, string currency)
		{
			return CurrToStr(value, uppercase: true, currency);
		}

		public static string CurrToStr(double value, bool uppercase, string currency)
		{
			return CurrToStr((decimal)value, uppercase, currency, cents: true);
		}

		public static string CurrToStr(decimal value, bool uppercase, string currency)
		{
			return CurrToStr((double)value, uppercase, currency);
		}

		public static string Decline(decimal value, string one, string two, string five)
		{
			return Decline((long)value, one, two, five);
		}

		public static string Decline(double value, string one, string two, string five)
		{
			return Decline((long)value, one, two, five);
		}

		public static string Decline(long value, string one, string two, string five)
		{
			long num = value % 100;
			if (num >= 10 && num < 20)
			{
				return five;
			}
			switch (num % 10)
			{
			case 1L:
				return one;
			case 2L:
			case 3L:
			case 4L:
				return two;
			default:
				return five;
			}
		}

		public static string Decline(decimal value, string currency)
		{
			return Decline(value, currency, cents: false);
		}

		public static string Decline(double value, string currency)
		{
			return Decline(value, currency, cents: false);
		}

		public static string Decline(long value, string currency)
		{
			return Decline(value, currency, cents: false);
		}

		public static string Decline(decimal value, string currency, bool cents)
		{
			return Decline((long)value, currency, cents);
		}

		public static string Decline(double value, string currency, bool cents)
		{
			return Decline((long)value, currency, cents);
		}

		public static string Decline(long value, string currency, bool cents)
		{
			Currency currency2 = GetCurrency(currency);
			if (cents)
			{
				return Decline(value, currency2.CentOne, currency2.CentTwo, currency2.CentFive);
			}
			return Decline(value, currency2.DollarOne, currency2.DollarTwo, currency2.DollarFive);
		}

		public static string DateToStr(DateTime date)
		{
			return DateToStr(date, uppercase: false);
		}

		public static string DateToStr(DateTime date, bool uppercase)
		{
			StringBuilder stringBuilder = new StringBuilder($"{date.Day} {months[date.Month - 1]} {date.Year}");
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}

		static Ua()
		{
			Currencies = new Hashtable();
			months = new string[12]
			{
				"січня", "лютого", "березня", "квітня", "травня", "червня", "липня", "серпня", "вересня", "жовтня",
				"листопада", "грудня"
			};
			units = new string[19]
			{
				"один", "два", "три", "чотири", "п'ять", "шість", "сім", "вісім", "дев'ять", "десять",
				"одинадцять", "дванадцять", "тринадцять", "чотирнадцять", "п'ятнадцять", "шістнадцять", "сімнадцять", "вісімнадцять", "дев'ятнадцять"
			};
			tens = new string[9] { "десять", "двадцять", "тридцять", "сорок", "п'ятдесят", "шістдесят", "сімдесят", "вісімдесят", "дев'яносто" };
			hundreds = new string[9] { "сто", "двісті", "триста", "чотириста", "п'ятсот", "шістсот", "сімсот", "вісімсот", "дев'ятсот" };
			gendered = new string[2, 3]
			{
				{ "один", "одна", "одне" },
				{ "два", "дві", "два" }
			};
			RegisterCurrency(new EURCurrency(), "EUR");
			RegisterCurrency(new USDCurrency(), "USD");
			RegisterCurrency(new RURCurrency(), "RUR");
			RegisterCurrency(new UAHCurrency(), "UAH");
		}

		private Ua()
		{
		}
	}

	public sealed class En
	{
		private static string[] months = new string[12]
		{
			"January", "February", "March", "April", "May", "June", "July", "August", "September", "October",
			"November", "December"
		};

		private static string[] units = new string[19]
		{
			"one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten",
			"eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
		};

		private static string[] tens = new string[9] { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

		private static void AddUnits(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				sb.Append(units[value - 1]);
			}
		}

		private static void AddTens(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				sb.Append(tens[value - 1]);
			}
		}

		private static void AddRank(StringBuilder sb, ref long rank, ref long value, string unit)
		{
			long num = value / rank;
			if (num > 0)
			{
				long num2 = num / 100;
				long num3 = num / 10 % 10;
				long num4 = num % 10;
				if (num3 == 1)
				{
					num3 = 0L;
					num4 = num % 100;
				}
				if (sb.Length > 0)
				{
					if (num2 > 0)
					{
						sb.Append(" ");
					}
					else if (num3 + num4 > 0)
					{
						sb.Append(" and ");
					}
				}
				if (num2 > 0)
				{
					AddUnits(sb, num2);
					sb.Append(" hundred");
					if (num3 + num4 > 0)
					{
						sb.Append(" and ");
					}
				}
				if (num3 > 0)
				{
					AddTens(sb, num3);
					if (num4 > 0)
					{
						sb.Append("-");
					}
				}
				if (num4 > 0)
				{
					AddUnits(sb, num4);
				}
				sb.Append(" ");
				sb.Append(unit);
				value %= rank;
			}
			rank /= 1000L;
		}

		public static string Decline(long value, string one, string two)
		{
			long num = value % 100;
			if (num == 1)
			{
				return one;
			}
			return two;
		}

		public static string Decline(long value, bool showCents, string dollars, string cents)
		{
			if (showCents)
			{
				string[] array = cents.Split(new char[1] { '/' });
				return Decline(value, array[0], array[1]);
			}
			string[] array2 = dollars.Split(new char[1] { '/' });
			return Decline(value, array2[0], array2[1]);
		}

		public static string NumToStr(long value)
		{
			return NumToStr(value, uppercase: true);
		}

		public static string NumToStr(long value, bool uppercase)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (value == 0L)
			{
				stringBuilder.Append("zero");
			}
			else
			{
				bool flag = false;
				if (value < 0)
				{
					flag = true;
					value = Math.Abs(value);
				}
				long rank = 1000000000000000000L;
				AddRank(stringBuilder, ref rank, ref value, "quintillion");
				AddRank(stringBuilder, ref rank, ref value, "quadrillion");
				AddRank(stringBuilder, ref rank, ref value, "trillion");
				AddRank(stringBuilder, ref rank, ref value, "billion");
				AddRank(stringBuilder, ref rank, ref value, "million");
				AddRank(stringBuilder, ref rank, ref value, "thousand");
				AddRank(stringBuilder, ref rank, ref value, "");
				if (flag)
				{
					stringBuilder.Insert(0, "minus ");
				}
			}
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}

		public static string NumToStr(double value, bool uppercase)
		{
			return NumToStr((long)value, uppercase);
		}

		public static string NumToStr(double value)
		{
			return NumToStr((long)value);
		}

		public static string NumToStr(decimal value, bool uppercase)
		{
			return NumToStr((long)value, uppercase);
		}

		public static string NumToStr(decimal value)
		{
			return NumToStr((long)value);
		}

		public static string CurrToStr(long value)
		{
			return CurrToStr((decimal)value);
		}

		public static string CurrToStr(long value, bool showCents)
		{
			return CurrToStr((decimal)value, showCents);
		}

		public static string CurrToStr(long value, bool uppercase, bool cents)
		{
			return CurrToStr((decimal)value, uppercase, cents);
		}

		public static string CurrToStr(double value)
		{
			return CurrToStr((decimal)value);
		}

		public static string CurrToStr(double value, bool showCents)
		{
			return CurrToStr((decimal)value, showCents);
		}

		public static string CurrToStr(double value, bool uppercase, bool cents)
		{
			return CurrToStr((decimal)value, uppercase, cents);
		}

		public static string CurrToStr(decimal value)
		{
			return CurrToStr(value, uppercase: true, showCents: true);
		}

		public static string CurrToStr(decimal value, bool showCents)
		{
			return CurrToStr(value, uppercase: true, showCents);
		}

		public static string CurrToStr(decimal value, bool uppercase, bool showCents)
		{
			return CurrToStr(value, uppercase, showCents, "dollar/dollars", "cent/cents");
		}

		public static string CurrToStr(long value, bool uppercase, bool showCents, string dollars, string cents)
		{
			return CurrToStr((decimal)value, uppercase, showCents, dollars, cents);
		}

		public static string CurrToStr(double value, bool uppercase, bool showCents, string dollars, string cents)
		{
			return CurrToStr((decimal)value, uppercase, showCents, dollars, cents);
		}

		public static string CurrToStr(decimal value, bool uppercase, bool showCents, string dollars, string cents)
		{
			decimal num = Math.Truncate(value);
			long num2 = 0L;
			if (StiOptions.Engine.UseRoundForToCurrencyWordsFunctions)
			{
				num2 = Math.Abs((long)Math.Round((value - (decimal)(long)num) * 100m));
				if (num2 > 99)
				{
					num2 = 0L;
					++num;
				}
			}
			else
			{
				num2 = Math.Abs((long)((value - (decimal)(long)num) * 100m));
			}
			string text = NumToStr((long)num, uppercase);
			if (value == 0m)
			{
				text += " ";
			}
			if (!text.EndsWith(" "))
			{
				text += " ";
			}
			text = $"{text}{Decline((long)num, showCents: false, dollars, cents)}";
			if (showCents)
			{
				text += " and ";
				text += $"{NumToStr(num2, uppercase: false)}";
				if (num2 == 0L)
				{
					text += " ";
				}
				text += $"{Decline(num2, showCents: true, dollars, cents)}";
			}
			return text;
		}

		public static string DateToStr(DateTime date)
		{
			return DateToStr(date, uppercase: false);
		}

		public static string DateToStr(DateTime date, bool uppercase)
		{
			StringBuilder stringBuilder = new StringBuilder($"{date.Day} {months[date.Month - 1]} {date.Year}");
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}
	}

	public sealed class Pl
	{
		private static string[] units = new string[19]
		{
			"jeden", "dwa", "trzy", "cztery", "pięć", "sześć", "siedem", "osiem", "dziewięć", "dziesięć",
			"jedenaście", "dwanaście", "trzynaście", "czternaście", "piętnaście", "szesnaście", "siedemnaście", "osiemnaście", "dziewiętnaście"
		};

		private static string[] tens = new string[9] { "dziesięć", "dwadzieścia", "trzydzieści", "czterdzieści", "pięćdziesiąt", "sześćdziesiąt", "siedemdziesiąt", "osiemdziesiąt", "dziewięćdziesiąt" };

		private static string[] hundreds = new string[9] { "sto", "dwieście", "trzysta", "czterysta", "pięćset", "sześćset", "siedemset", "osiemset", "dziewięćset" };

		private static string[] thousends = new string[3] { "tysiąc", "tysiące", "tysięcy" };

		private static string[] million = new string[3] { "milion", "miliony", "milionów" };

		private static string[] billion = new string[3] { "miliard", "miliardy", "miliardów" };

		private static string[] trillion = new string[3] { "bilion", "biliony", "bilionów" };

		private static string[] quadrillion = new string[3] { "biliard", "biliardy", "biliardów" };

		private static string[] quintillion = new string[3] { "trylion", "tryliony", "trylionów" };

		private static string[] zloty = new string[3] { "złoty", "złote", "złotych" };

		private static string[] grosz = new string[3] { "grosz", "grosze", "groszy" };

		private static string[] dollar = new string[3] { "dolar", "dolary", "dolarów" };

		private static string[] cent = new string[3] { "cent", "centy", "centów" };

		private static string[] euro = new string[3] { "euro", "euro", "euro" };

		private static string[] months = new string[12]
		{
			"Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik",
			"Listopad", "Grudzień"
		};

		public static string NumToStr(long value, bool uppercase)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (value == 0L)
			{
				stringBuilder.Append("zero ");
			}
			else
			{
				bool flag = false;
				if (value < 0)
				{
					flag = true;
					value = Math.Abs(value);
				}
				long rank = 1000000000000000000L;
				AddRank(stringBuilder, ref rank, ref value, quintillion);
				AddRank(stringBuilder, ref rank, ref value, quadrillion);
				AddRank(stringBuilder, ref rank, ref value, trillion);
				AddRank(stringBuilder, ref rank, ref value, billion);
				AddRank(stringBuilder, ref rank, ref value, million);
				AddRank(stringBuilder, ref rank, ref value, thousends);
				AddRank(stringBuilder, ref rank, ref value, null);
				if (flag)
				{
					stringBuilder.Insert(0, "minus ");
				}
			}
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString().TrimEnd(new char[0]);
		}

		private static void AddUnits(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				sb.Append(units[value - 1]);
			}
		}

		private static void AddTens(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				sb.Append(tens[value - 1]);
			}
		}

		private static void AddHundreds(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				sb.Append(hundreds[value - 1]);
			}
		}

		private static void AddRank(StringBuilder sb, ref long rank, ref long value, string[] unit)
		{
			long num = value / rank;
			if (num > 0)
			{
				long num2 = num / 100;
				long num3 = num / 10 % 10;
				long num4 = num % 10;
				if (num3 == 1)
				{
					num3 = 0L;
					num4 = num % 100;
				}
				if (sb.Length > 0)
				{
					if (num2 > 0)
					{
						sb.Append(" ");
					}
					else if (num3 + num4 > 0)
					{
						sb.Append(" ");
					}
				}
				if (num2 > 0)
				{
					AddHundreds(sb, num2);
					if (num3 + num4 > 0)
					{
						sb.Append(" ");
					}
				}
				if (num3 > 0)
				{
					AddTens(sb, num3);
					if (num4 > 0)
					{
						sb.Append(" ");
					}
				}
				if (num4 > 0)
				{
					AddUnits(sb, num4);
				}
				sb.Append(" ");
				if (unit != null)
				{
					switch (num4)
					{
					case 0L:
					case 1L:
						if (num3 == 0L && num2 == 0L)
						{
							sb.Append(unit[0]);
						}
						else
						{
							sb.Append(unit[2]);
						}
						break;
					case 2L:
					case 3L:
					case 4L:
						sb.Append(unit[1]);
						break;
					default:
						sb.Append(unit[2]);
						break;
					}
				}
				value %= rank;
			}
			rank /= 1000L;
		}

		private static string Decline(long value, string[] post)
		{
			long num = value % 10;
			long num2 = value / 10;
			switch (num)
			{
			case 1L:
				if (num2 != 1)
				{
					return post[0];
				}
				return post[2];
			case 2L:
			case 3L:
			case 4L:
				if (num2 != 1)
				{
					return post[1];
				}
				return post[2];
			default:
				return post[2];
			}
		}

		private static string Decline(long value, bool showCents, string[] dollars, string[] cents)
		{
			if (showCents)
			{
				return Decline(value, cents);
			}
			return Decline(value, dollars);
		}

		private static string CurrToStr(decimal value, bool uppercase, bool showCents, string[] dollars, string[] cents)
		{
			decimal num = Math.Truncate(value);
			long num2 = 0L;
			if (StiOptions.Engine.UseRoundForToCurrencyWordsFunctions)
			{
				num2 = (long)Math.Round((value - (decimal)(long)num) * 100m);
				if (num2 > 99)
				{
					num2 = 0L;
					++num;
				}
			}
			else
			{
				num2 = (long)((value - (decimal)(long)num) * 100m);
			}
			StringBuilder stringBuilder = new StringBuilder(NumToStr((long)num, uppercase)).Append(" ");
			stringBuilder.Append(Decline((long)num, showCents: false, dollars, cents));
			if (showCents)
			{
				stringBuilder.Append(" i ");
				stringBuilder.Append(NumToStr(num2, uppercase: false)).Append(" ");
				stringBuilder.Append(Decline(num2, showCents: true, dollars, cents));
			}
			return stringBuilder.ToString();
		}

		public static string CurrToStr(decimal value, string currencyISO, bool showCents, bool uppercase)
		{
			if (!(currencyISO == "USD"))
			{
				if (currencyISO == "EUR")
				{
					return CurrToStr(value, uppercase, showCents, euro, cent);
				}
				return CurrToStr(value, uppercase, showCents, zloty, grosz);
			}
			return CurrToStr(value, uppercase, showCents, dollar, cent);
		}

		public static string DateToStr(DateTime date, bool uppercase)
		{
			StringBuilder stringBuilder = new StringBuilder($"{date.Day} {months[date.Month - 1]} {date.Year}");
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}
	}

	public sealed class PtBr
	{
		private static string[] unid = new string[20]
		{
			"ZERO ", "UM ", "DOIS ", "TRÊS ", "QUATRO ", "CINCO ", "SEIS ", "SETE ", "OITO ", "NOVE ",
			"DEZ ", "ONZE ", "DOZE ", "TREZE ", "CATORZE ", "QUINZE ", "DEZESSEIS ", "DEZESSETE ", "DEZOITO ", "DEZENOVE "
		};

		private static string[] dezena = new string[10] { "ZERO ", "DEZ ", "VINTE ", "TRINTA ", "QUARENTA ", "CINQÜENTA ", "SESSENTA ", "SETENTA ", "OITENTA ", "NOVENTA " };

		private static string[] centena = new string[10] { "ZERO ", "CENTO ", "DUZENTOS ", "TREZENTOS ", "QUATROCENTOS ", "QUINHENTOS ", "SEISCENTOS ", "SETECENTOS ", "OITOCENTOS ", "NOVECENTOS " };

		public static string NumToStr(decimal value, bool uppercase = false, string dollars = "REAL/REAIS", string cents = "CENTAVO/CENTAVOS")
		{
			string[] array = dollars.Split(new char[1] { '/' });
			string[] array2 = cents.Split(new char[1] { '/' });
			string[] array3 = new string[5];
			string[] array4 = new string[5];
			string text = value.ToString("0000000000.00");
			array3[1] = text.Substring(1, 3);
			array3[2] = text.Substring(4, 3);
			array3[3] = text.Substring(7, 3);
			array3[4] = "0" + text.Substring(11, 2);
			for (int i = 1; i <= 4; i++)
			{
				string text2 = array3[i];
				int num = int.Parse(text2);
				int num2 = ((num < 10) ? 1 : ((num < 100) ? 2 : ((num < 1000) ? 3 : 0)));
				if (num2 == 3)
				{
					if (!text2.EndsWith("00", StringComparison.InvariantCulture))
					{
						ref string reference = ref array4[i];
						reference = reference + centena[int.Parse(text2.Substring(0, 1))] + "E ";
						num2 = 2;
					}
					else
					{
						array4[i] += (text2.StartsWith("1", StringComparison.InvariantCulture) ? "CEM " : centena[int.Parse(text2.Substring(0, 1))]);
					}
				}
				if (num2 == 2)
				{
					if (int.Parse(text2.Substring(1, 2)) < 20)
					{
						array4[i] += unid[int.Parse(text2.Substring(1, 2))];
					}
					else
					{
						array4[i] += dezena[int.Parse(text2.Substring(1, 1))];
						if (!text2.EndsWith("0", StringComparison.InvariantCulture))
						{
							array4[i] += "E ";
							num2 = 1;
						}
					}
				}
				if (num2 == 1)
				{
					array4[i] += unid[int.Parse(text2.Substring(2, 1))];
				}
			}
			string empty = string.Empty;
			if (int.Parse(array3[1] + array3[2] + array3[3]) == 0 && int.Parse(array3[4]) != 0)
			{
				empty = array4[4] + ((int.Parse(array3[4]) == 1) ? array2[0] : array2[1]);
			}
			else
			{
				empty = ((int.Parse(array3[1]) != 0) ? (array4[1] + ((int.Parse(array3[1]) > 1) ? "MILHÕES " : "MILHÃO ")) : "");
				empty = ((int.Parse(array3[2] + array3[3]) != 0) ? (empty + ((int.Parse(array3[2]) != 0) ? (array4[2] + "MIL ") : "")) : (empty + "DE "));
				empty += ((int.Parse(array3[3]) != 0) ? array4[3] : "");
				empty = empty + ((int.Parse(array3[1] + array3[2] + array3[3]) == 1) ? array[0] : array[1]) + " ";
				empty += ((int.Parse(array3[4]) != 0) ? ("E " + array4[4] + ((int.Parse(array3[4]) == 1) ? array2[0] : array2[1])) : "");
			}
			string empty2 = string.Empty;
			empty2 = ((!(value == 0m)) ? empty.Trim() : unid[0]);
			if (!uppercase)
			{
				return empty2.ToLowerInvariant();
			}
			return char.ToUpper(empty2[0]) + empty2.ToLowerInvariant();
		}
	}

	public sealed class Pt
	{
		private static string[] units = new string[19]
		{
			"um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez",
			"onze", "doze", "treze", "catorze", "quinze", "dezasseis", "dezassete", "dezoito", "dezanove"
		};

		private static string[] tens = new string[9] { "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa" };

		private static string[] months = new string[12]
		{
			"Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro",
			"Novembro", "Dezembro"
		};

		public static string NumToStr(long value, bool uppercase)
		{
			bool greaterOrEqualThanThousand = value >= 1000;
			StringBuilder stringBuilder = new StringBuilder();
			if (value == 0L)
			{
				stringBuilder.Append("zero");
			}
			else
			{
				bool flag = false;
				if (value < 0)
				{
					flag = true;
					value = Math.Abs(value);
				}
				long rank = 1000000000000000000L;
				AddRank(stringBuilder, ref rank, ref value, value.ToString().StartsWith("1") ? "quintilião" : "quintiliões", greaterOrEqualThanThousand);
				AddRank(stringBuilder, ref rank, ref value, value.ToString().StartsWith("1") ? "quadrilião" : "quadriliões", greaterOrEqualThanThousand);
				AddRank(stringBuilder, ref rank, ref value, value.ToString().StartsWith("1") ? "trilião" : "triliões", greaterOrEqualThanThousand);
				AddRank(stringBuilder, ref rank, ref value, value.ToString().StartsWith("1") ? "bilião" : "biliões", greaterOrEqualThanThousand);
				AddRank(stringBuilder, ref rank, ref value, value.ToString().StartsWith("1") ? "milhão" : "milhões", greaterOrEqualThanThousand);
				AddRank(stringBuilder, ref rank, ref value, "mil", greaterOrEqualThanThousand);
				AddRank(stringBuilder, ref rank, ref value, "", greaterOrEqualThanThousand);
				if (flag)
				{
					stringBuilder.Insert(0, "menos ");
				}
			}
			if (uppercase)
			{
				stringBuilder[0] = char.ToUpper(stringBuilder[0]);
			}
			return stringBuilder.ToString();
		}

		private static void AddRank(StringBuilder sb, ref long rank, ref long value, string unit, bool greaterOrEqualThanThousand)
		{
			long num = value / rank;
			if (num > 0)
			{
				long num2 = num / 100;
				long num3 = num / 10 % 10;
				long num4 = num % 10;
				if (num3 == 1)
				{
					num3 = 0L;
					num4 = num % 100;
				}
				if (sb.Length > 0)
				{
					if (num2 > 0)
					{
						if (greaterOrEqualThanThousand && num3 + num4 == 0L)
						{
							sb.Append(" e ");
						}
						else
						{
							sb.Append(" ");
						}
					}
					else if (num3 + num4 > 0)
					{
						sb.Append(" e ");
					}
				}
				if (num2 > 0)
				{
					long num5 = num2 - 1;
					if ((ulong)num5 <= 8uL)
					{
						switch (num5)
						{
						case 0L:
							if (num3 + num4 > 0)
							{
								sb.Append("cento");
							}
							else
							{
								sb.Append("cem");
							}
							break;
						case 1L:
							sb.Append("duzentos");
							break;
						case 2L:
							sb.Append("trezentos");
							break;
						case 3L:
							sb.Append("quatrocentos");
							break;
						case 4L:
							sb.Append("quinhentos");
							break;
						case 5L:
							sb.Append("seiscentos");
							break;
						case 6L:
							sb.Append("setecentos");
							break;
						case 7L:
							sb.Append("oitocentos");
							break;
						case 8L:
							sb.Append("novecentos");
							break;
						}
					}
					if (num3 + num4 > 0)
					{
						sb.Append(" e ");
					}
				}
				if (num3 > 0)
				{
					AddTens(sb, num3);
					if (unit != string.Empty && num4 == 0L)
					{
						sb.Append(" ");
					}
					if (num4 > 0)
					{
						sb.Append(" e ");
					}
				}
				if ((greaterOrEqualThanThousand && num4 > 1) || (num4 > 0 && unit == string.Empty))
				{
					AddUnits(sb, num4);
				}
				if (sb.Length != 0 && sb[sb.Length - 1] != ' ')
				{
					sb.Append(" ");
				}
				sb.Append(unit);
				value %= rank;
			}
			rank /= 1000L;
		}

		private static void AddUnits(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				sb.Append(units[value - 1]);
			}
		}

		private static void AddTens(StringBuilder sb, long value)
		{
			if (value != 0L)
			{
				sb.Append(tens[value - 1]);
			}
		}

		private static string Decline(long value, bool showCents, string dollars, string cents)
		{
			if (showCents)
			{
				string[] array = cents.Split(new char[1] { '/' });
				return Decline(value, array[0], array[1]);
			}
			string[] array2 = dollars.Split(new char[1] { '/' });
			return Decline(value, array2[0], array2[1]);
		}

		private static string Decline(long value, string one, string two)
		{
			long num = value % 100;
			if (num == 1)
			{
				return one;
			}
			return two;
		}

		public static string CurrToStr(decimal value, bool uppercase, bool showCents, string currencyISO, int decimals)
		{
			CultureInfo culture = new CultureInfo("pt");
			string dollars = Resource.ResourceManager.GetString(currencyISO + "Plural", culture) + "/" + Resource.ResourceManager.GetString(currencyISO + "Single", culture);
			string cents = Resource.ResourceManager.GetString(currencyISO + "CentPlural", culture) + "/" + Resource.ResourceManager.GetString(currencyISO + "CentSingle", culture);
			return CurrToStr(value, uppercase, showCents, dollars, cents, decimals);
		}

		public static string CurrToStr(decimal value, bool uppercase, bool showCents, string dollars = "euro/euros", string cents = "cêntimo/cêntimos", int decimals = 2)
		{
			string text = NumToStr((long)value, uppercase);
			if (value == 0m)
			{
				text += " ";
			}
			if (!text.EndsWith(" "))
			{
				text += " ";
			}
			text = $"{text}{Decline((long)value, showCents: false, dollars, cents)}";
			if (showCents)
			{
				text += " e ";
				long num = 0L;
				decimal num2 = (decimal)Math.Pow(10.0, decimals);
				num = ((!StiOptions.Engine.UseRoundForToCurrencyWordsFunctions) ? ((long)((value - (decimal)(long)value) * num2)) : ((long)Math.Round((value - (decimal)(long)value) * num2)));
				text += $"{NumToStr(num, uppercase: false)}";
				if (num == 0L)
				{
					text += " ";
				}
				text += $"{Decline(num, showCents: true, dollars, cents)}";
			}
			return text;
		}

		public static string DateToStr(DateTime value)
		{
			return $"{value.Day} de {months[value.Month - 1]} de {value.Year}";
		}
	}

	public sealed class Fr
	{
		private const string ZeroWord = "zéro";

		private const string LessWord = "moins";

		private static string[,] triplets;

		private static string[] lessTwentys;

		private static string[] tens;

		static Fr()
		{
			triplets = new string[7, 2];
			lessTwentys = new string[20];
			tens = new string[10];
			triplets[0, 0] = "";
			triplets[1, 0] = " mille";
			triplets[2, 0] = " million";
			triplets[3, 0] = " milliard";
			triplets[4, 0] = " trillion";
			triplets[5, 0] = " quadrillion";
			triplets[6, 0] = " qunintillion";
			triplets[0, 1] = "";
			triplets[1, 1] = " mille";
			triplets[2, 1] = " millions";
			triplets[3, 1] = " milliards";
			triplets[4, 1] = " trillions";
			triplets[5, 1] = " quadrillions";
			triplets[6, 1] = " qunintillions";
			lessTwentys[0] = "";
			lessTwentys[1] = "un";
			lessTwentys[2] = "deux";
			lessTwentys[3] = "trois";
			lessTwentys[4] = "quatre";
			lessTwentys[5] = "cinq";
			lessTwentys[6] = "six";
			lessTwentys[7] = "sept";
			lessTwentys[8] = "huit";
			lessTwentys[9] = "neuf";
			lessTwentys[10] = "dix";
			lessTwentys[11] = "onze";
			lessTwentys[12] = "douze";
			lessTwentys[13] = "treize";
			lessTwentys[14] = "quatorze";
			lessTwentys[15] = "quinze";
			lessTwentys[16] = "seize";
			lessTwentys[17] = "dix-sept";
			lessTwentys[18] = "dix-huit";
			lessTwentys[19] = "dix-neuf";
			tens[2] = "vingt";
			tens[3] = "trente";
			tens[4] = "quarante";
			tens[5] = "cinquante";
			tens[6] = "soixante";
			tens[7] = "soixante";
			tens[8] = "quatre-vingt";
			tens[9] = "quatre-vingt";
		}

		public static string ConvertToWord(decimal number, string currencyISO, int decimals)
		{
			CultureInfo culture = new CultureInfo("fr-FR");
			if (number > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), number);
			}
			decimal num = decimal.Truncate(number);
			decimal num2 = Math.Abs((number - num) * (decimal)Math.Pow(10.0, decimals));
			if (num2 > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), num2);
			}
			string integerString = ConvertToWord((long)num, int.Parse(Resource.ResourceManager.GetString(currencyISO + "Gender", culture)), 0);
			string decimalString = ConvertToWord((long)num2, int.Parse(Resource.ResourceManager.GetString(currencyISO + "CentGender", culture)), 0);
			string mainCurrency = "";
			string centCurrency = "";
			string postCurrency = "et";
			NumToWordHelper.determinateCurrencies(culture, currencyISO, num, num2, ref mainCurrency, ref centCurrency);
			string text = NumToWordHelper.AddWords(integerString, decimalString, mainCurrency, centCurrency, postCurrency);
			return text.Trim();
		}

		private static string ConvertToWord(long number, int Gender, int tranche)
		{
			string text = "";
			bool flag = number < 0;
			if (flag)
			{
				number = Math.Abs(number);
			}
			if (number == 0L)
			{
				text = "zéro";
			}
			else if (number < 20)
			{
				text = ((number != 1 || Gender != 2) ? lessTwentys[number] : (lessTwentys[number] + "e"));
			}
			else if (number < 100)
			{
				string text2 = "";
				switch (number % 10)
				{
				case 0L:
				{
					long num = number / 10;
					long num2 = num - 7;
					if ((ulong)num2 > 2uL)
					{
						goto IL_00bd;
					}
					switch (num2)
					{
					case 0L:
						break;
					case 1L:
						goto IL_00a9;
					case 2L:
						goto IL_00b5;
					default:
						goto IL_00bd;
					}
					text2 = "-";
					break;
				}
				case 1L:
					text2 = (number / 10) switch
					{
						8L => " ", 
						9L => " ", 
						_ => "-et-", 
					};
					break;
				default:
					{
						text2 = "-";
						break;
					}
					IL_00a9:
					if (tranche != 1)
					{
						text2 = "s";
					}
					break;
					IL_00bd:
					text2 = "";
					break;
					IL_00b5:
					text2 = "-";
					break;
				}
				text = tens[number / 10] + text2;
				if (number / 10 == 7 || number / 10 == 9)
				{
					if (text2 == "")
					{
						text2 = " ";
					}
					text += ConvertToWord(number % 10 + 10, Gender, 0);
				}
				else if (number % 10 != 0L)
				{
					text += ConvertToWord(number % 10, Gender, 0);
				}
			}
			else if (number < 1000)
			{
				long num3 = number / 100;
				if (num3 == 1)
				{
					text = "cent";
				}
				else
				{
					text = ConvertToWord(number / 100, Gender, 0) + " cent";
					if (number % 100 == 0L && tranche != 1)
					{
						text += "s";
					}
				}
				if (number % 100 > 0)
				{
					text = text + " " + ConvertToWord(number % 100, Gender, 0);
				}
			}
			else
			{
				text = CalculateOver(number, Gender);
			}
			if (flag)
			{
				text = "moins " + text;
			}
			return text.Trim();
		}

		private static string CalculateOver(long number, int gender)
		{
			string result = "";
			string text = "";
			int num = (number.ToString().Length - 1) / 3;
			long num2 = (long)Math.Pow(10.0, num * 3);
			if (number % num2 > 0)
			{
				text = ConvertToWord(number % num2, gender, num);
			}
			long num3 = number / num2;
			switch (num3)
			{
			case 1L:
				result = ((num != 1) ? (ConvertToWord(num3, gender, num) + triplets[num, 0] + " " + text) : (triplets[num, 0] + " " + text));
				break;
			default:
				result = ConvertToWord(num3, gender, num) + triplets[num, 1] + " " + text;
				break;
			case 0L:
				break;
			}
			return result;
		}
	}

	public sealed class Es
	{
		private const string ZeroWord = "cero";

		private const string LessWord = "menos";

		private static string[,] triplets;

		private static string[] lessTwenty;

		private static string[] tens;

		private static string[,] currencies;

		static Es()
		{
			currencies = new string[2, 4]
			{
				{ "dólar", "dólares", "centavo", "centavos" },
				{ "euro", "euros", "céntimo", "céntimos" }
			};
			triplets = new string[7, 2];
			lessTwenty = new string[20];
			tens = new string[10];
			triplets[0, 0] = "";
			triplets[1, 0] = " mil";
			triplets[2, 0] = " millón";
			triplets[3, 0] = " mil milliones";
			triplets[4, 0] = " billón";
			triplets[5, 0] = " mil billón";
			triplets[6, 0] = " trillón";
			triplets[0, 1] = "";
			triplets[1, 1] = " mil";
			triplets[2, 1] = " millones";
			triplets[3, 1] = " mil milliones";
			triplets[4, 1] = " billónes";
			triplets[5, 1] = " mil billónes";
			triplets[6, 1] = " trillónes";
			lessTwenty[0] = "";
			lessTwenty[1] = "uno";
			lessTwenty[2] = "dos";
			lessTwenty[3] = "tres";
			lessTwenty[4] = "cuatro";
			lessTwenty[5] = "cinco";
			lessTwenty[6] = "seis";
			lessTwenty[7] = "siete";
			lessTwenty[8] = "ocho";
			lessTwenty[9] = "nueve";
			lessTwenty[10] = "diez";
			lessTwenty[11] = "once";
			lessTwenty[12] = "doce";
			lessTwenty[13] = "trece";
			lessTwenty[14] = "catorce";
			lessTwenty[15] = "quince";
			lessTwenty[16] = "dieciséis";
			lessTwenty[17] = "diecisiete";
			lessTwenty[18] = "dieciocho";
			lessTwenty[19] = "diecinueve";
			tens[2] = "veinte";
			tens[3] = "treinta";
			tens[4] = "cuarenta";
			tens[5] = "cincuenta";
			tens[6] = "sesenta";
			tens[7] = "setenta";
			tens[8] = "ochenta";
			tens[9] = "noventa";
		}

		public static string ConvertToWord(decimal number, string currencyISO, int decimals, bool female = false)
		{
			CultureInfo culture = new CultureInfo("es-ES");
			if (number > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), number);
			}
			decimal num = decimal.Truncate(number);
			decimal num2 = Math.Abs((number - num) * (decimal)Math.Pow(10.0, decimals));
			if (num2 > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), num2);
			}
			int num3 = (female ? 2 : int.Parse(Resource.ResourceManager.GetString(currencyISO + "Gender", culture)));
			if (num3 == 0)
			{
				num3 = 1;
			}
			string integerString = ConvertToWord((long)num, num3, 0);
			int num4 = (female ? 2 : int.Parse(Resource.ResourceManager.GetString(currencyISO + "CentGender", culture)));
			if (num4 == 0)
			{
				num4 = 1;
			}
			string decimalString = ConvertToWord((long)num2, num4, 0);
			string mainCurrency = "";
			string centCurrency = "";
			string postCurrency = "y";
			NumToWordHelper.determinateCurrencies(culture, currencyISO, (long)num, num2, ref mainCurrency, ref centCurrency);
			determinateCurrenciesEurUsd(culture, currencyISO, (long)num, (long)num2, ref mainCurrency, ref centCurrency);
			string text = NumToWordHelper.AddWords(integerString, decimalString, mainCurrency, centCurrency, postCurrency);
			return text.Trim();
		}

		private static void determinateCurrenciesEurUsd(CultureInfo culture, string currencyISO, decimal integerPart, decimal decimalPart, ref string mainCurrency, ref string centCurrency)
		{
			int num = -1;
			if (currencyISO == "USD")
			{
				num = 0;
			}
			if (currencyISO == "EUR")
			{
				num = 1;
			}
			if (num != -1)
			{
				if (integerPart / 1000000m > 0m && integerPart % 1000000m == 0m)
				{
					mainCurrency = currencies[num, 1];
				}
				else if (integerPart != 1m)
				{
					mainCurrency = currencies[num, 1];
				}
				else
				{
					mainCurrency = currencies[num, 0];
				}
				if (decimalPart != 1m)
				{
					centCurrency = currencies[num, 3];
				}
				else
				{
					centCurrency = currencies[num, 2];
				}
			}
		}

		public static string NumToStr(long number, bool upperCase)
		{
			CultureInfo culture = new CultureInfo("es-ES");
			if ((decimal)number > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), number);
			}
			string text = ConvertToWord(number, 1, 0, alone: true);
			if (upperCase && text.Length > 1)
			{
				text = char.ToUpperInvariant(text[0]) + text.Substring(1);
			}
			return text.Trim();
		}

		public static string NumToStr(long number, bool upperCase, bool female)
		{
			CultureInfo culture = new CultureInfo("es-ES");
			if ((decimal)number > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), number);
			}
			string text = ConvertToWord(number, (!female) ? 1 : 2, 0, alone: true);
			if (upperCase && text.Length > 1)
			{
				text = char.ToUpperInvariant(text[0]) + text.Substring(1);
			}
			return text.Trim();
		}

		private static string ConvertToWord(long number, int Gender, int tranche, bool alone = false)
		{
			string text = "";
			bool flag = number < 0;
			if (flag)
			{
				number = Math.Abs(number);
			}
			if (number == 0L)
			{
				text = "cero";
			}
			else if (number < 20)
			{
				text = ((number == 1 && Gender == 2) ? "una" : ((number != 1 || Gender != 1 || alone) ? lessTwenty[number] : "un"));
			}
			else if (number < 100)
			{
				text = ((number % 10 == 0L) ? tens[number / 10] : ((number / 10 != 2) ? (tens[number / 10] + " y " + ConvertToWord(number % 10, Gender, 0, alone)) : ("veinti" + ConvertToWord(number % 10, Gender, 0, alone))));
			}
			else
			{
				if (number < 1000)
				{
					long num = number / 100;
					if (num != 1)
					{
						long num2 = num - 5;
						if ((ulong)num2 > 4uL)
						{
							goto IL_015f;
						}
						switch (num2)
						{
						case 0L:
							break;
						case 2L:
							goto IL_0139;
						case 4L:
							goto IL_014c;
						default:
							goto IL_015f;
						}
						text = ((Gender == 1) ? "quinientos" : "quinientas");
					}
					else
					{
						text = ((number == 100) ? "cien" : "ciento");
					}
					goto IL_0182;
				}
				text = ((number < 1000000) ? ((number / 1000 != 1) ? (ConvertToWord(number / 1000, Gender, 1) + triplets[1, 1] + ((number % 1000 == 0L) ? "" : (" " + ConvertToWord(number % 1000, Gender, 0, alone)))) : (triplets[1, 0] + ((number % 1000 == 0L) ? "" : (" " + ConvertToWord(number % 1000, Gender, 0, alone))))) : ((number < 1000000000000L) ? ((number / 1000000 != 1) ? (ConvertToWord(number / 1000000, Gender, 2) + triplets[2, 1] + ((number % 1000000 == 0L) ? "" : (" " + ConvertToWord(number % 1000000, Gender, 0, alone)))) : (ConvertToWord(number / 1000000, Gender, 2) + triplets[2, 0] + ((number % 1000000 == 0L) ? "" : (" " + ConvertToWord(number % 1000000, Gender, 0, alone))))) : ((number < 1000000000000000000L) ? ((number / 1000000000000L != 1) ? (ConvertToWord(number / 1000000000000L, Gender, 2) + triplets[4, 1] + ((number % 1000000000000L == 0L) ? "" : (" " + ConvertToWord(number % 1000000000000L, Gender, 0, alone)))) : (ConvertToWord(number / 1000000000000L, Gender, 2) + triplets[4, 0] + ((number % 1000000000000L == 0L) ? "" : (" " + ConvertToWord(number % 1000000000000L, Gender, 0, alone))))) : ((number / 1000000000000000000L != 1) ? (ConvertToWord(number / 1000000000000000000L, Gender, 2) + triplets[6, 1] + ((number % 1000000000000000000L == 0L) ? "" : (" " + ConvertToWord(number % 1000000000000000000L, Gender, 0, alone)))) : (ConvertToWord(number / 1000000000000000000L, Gender, 2) + triplets[6, 0] + ((number % 1000000000000000000L == 0L) ? "" : (" " + ConvertToWord(number % 1000000000000000000L, Gender, 0, alone))))))));
			}
			goto IL_04a8;
			IL_014c:
			text = ((Gender == 1) ? "novecientos" : "novecientas");
			goto IL_0182;
			IL_015f:
			text = ConvertToWord(number / 100, Gender, 0) + ((Gender == 1) ? "cientos" : "cientas");
			goto IL_0182;
			IL_0182:
			if (number % 100 > 0)
			{
				text = text + " " + ConvertToWord(number % 100, Gender, 0, alone);
			}
			goto IL_04a8;
			IL_04a8:
			if (flag)
			{
				text = "menos " + text;
			}
			return text.Trim();
			IL_0139:
			text = ((Gender == 1) ? "setecientos" : "setecientas");
			goto IL_0182;
		}
	}

	public sealed class Fa
	{
		public static string ConvertToWord(long Number)
		{
			List<long> list = new List<long>();
			List<string> list2 = new List<string>();
			string text = string.Empty;
			Number = Math.Abs(Number);
			if (Number == 0L)
			{
				return "صفر";
			}
			long num;
			while (true)
			{
				num = 0L;
				long num2 = 0L;
				num = Number / 1000;
				num2 = Number % 1000;
				list.Add(num2);
				if (num < 1000)
				{
					break;
				}
				Number = num;
			}
			if (num != 0L)
			{
				list.Add(num);
			}
			for (int i = 0; i <= list.Count - 1; i++)
			{
				list2.Add(ChangingNum(list[i]));
			}
			for (int j = list2.Count - 1; j >= 0; j += -1)
			{
				switch (j)
				{
				case 5:
					if (string.IsNullOrEmpty(list2[5]))
					{
						continue;
					}
					if (!string.IsNullOrEmpty(list2[4]) || !string.IsNullOrEmpty(list2[3]) || !string.IsNullOrEmpty(list2[2]) || !string.IsNullOrEmpty(list2[1]) || !string.IsNullOrEmpty(list2[0]))
					{
						text = text + list2[5] + " بيليارد و ";
						continue;
					}
					text = text + list2[5] + " بيليارد";
					break;
				case 4:
					if (string.IsNullOrEmpty(list2[4]))
					{
						continue;
					}
					if (!string.IsNullOrEmpty(list2[3]) || !string.IsNullOrEmpty(list2[2]) || !string.IsNullOrEmpty(list2[1]) || !string.IsNullOrEmpty(list2[0]))
					{
						text = text + list2[4] + " بيليون و ";
						continue;
					}
					text = text + list2[4] + " بيليون";
					break;
				case 3:
					if (string.IsNullOrEmpty(list2[3]))
					{
						continue;
					}
					if (!string.IsNullOrEmpty(list2[2]) || !string.IsNullOrEmpty(list2[1]) || !string.IsNullOrEmpty(list2[0]))
					{
						text = text + list2[3] + " ميليارد و ";
						continue;
					}
					text = text + list2[3] + " ميليارد";
					break;
				case 2:
					if (string.IsNullOrEmpty(list2[2]))
					{
						continue;
					}
					if (!string.IsNullOrEmpty(list2[1]) || !string.IsNullOrEmpty(list2[0]))
					{
						text = text + list2[2] + " ميليون و ";
						continue;
					}
					text = text + list2[2] + " ميليون";
					break;
				case 1:
					if (string.IsNullOrEmpty(list2[1]))
					{
						continue;
					}
					if (!string.IsNullOrEmpty(list2[0]))
					{
						text = text + list2[1] + " هزار و ";
						continue;
					}
					text = text + list2[1] + " هزار";
					break;
				default:
					text += list2[0];
					continue;
				}
				break;
			}
			return text;
		}

		private static string ChangingNum(long Number)
		{
			List<string> list = new List<string>();
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string empty = string.Empty;
			int num;
			while (true)
			{
				num = 0;
				int num2 = 0;
				num = (int)(Number / 10);
				list.Add(((int)(Number % 10)).ToString());
				if (num < 10)
				{
					break;
				}
				Number = num;
			}
			list.Add(num.ToString());
			if (list.Count == 3)
			{
				switch (list[2])
				{
				case "0":
					text3 = "";
					break;
				case "1":
					text3 = "صد";
					break;
				case "2":
					text3 = "دويست";
					break;
				case "3":
					text3 = "سيصد";
					break;
				case "4":
					text3 = "چهارصد";
					break;
				case "5":
					text3 = "پانصد";
					break;
				case "6":
					text3 = "ششصد";
					break;
				case "7":
					text3 = "هفتصد";
					break;
				case "8":
					text3 = "هشتصد";
					break;
				case "9":
					text3 = "نهصد";
					break;
				}
			}
			switch (list[0])
			{
			case "0":
				text = "";
				break;
			case "1":
				text = "يک";
				break;
			case "2":
				text = "دو";
				break;
			case "3":
				text = "سه";
				break;
			case "4":
				text = "چهار";
				break;
			case "5":
				text = "پنج";
				break;
			case "6":
				text = "شش";
				break;
			case "7":
				text = "هفت";
				break;
			case "8":
				text = "هشت";
				break;
			case "9":
				text = "نه";
				break;
			}
			switch (list[1])
			{
			case "0":
				text2 = "";
				break;
			case "1":
				switch (list[0])
				{
				case "0":
					text = "ده";
					break;
				case "1":
					text = "يازده";
					break;
				case "2":
					text = "دوازده";
					break;
				case "3":
					text = "سيزده";
					break;
				case "4":
					text = "چهارده";
					break;
				case "5":
					text = "پانزده";
					break;
				case "6":
					text = "شانزده";
					break;
				case "7":
					text = "هفده";
					break;
				case "8":
					text = "هيجده";
					break;
				case "9":
					text = "نوزده";
					break;
				}
				break;
			case "2":
				text2 = "بيست";
				break;
			case "3":
				text2 = "سي";
				break;
			case "4":
				text2 = "چهل";
				break;
			case "5":
				text2 = "پنجاه";
				break;
			case "6":
				text2 = "شصت";
				break;
			case "7":
				text2 = "هفتاد";
				break;
			case "8":
				text2 = "هشتاد";
				break;
			case "9":
				text2 = "نود";
				break;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				empty += text3;
				if (!string.IsNullOrEmpty(text2))
				{
					empty = empty + " و " + text2;
					if (!string.IsNullOrEmpty(text))
					{
						empty = empty + " و " + text;
					}
				}
				else if (!string.IsNullOrEmpty(text))
				{
					empty = empty + " و " + text;
				}
			}
			else if (!string.IsNullOrEmpty(text2))
			{
				empty += text2;
				if (!string.IsNullOrEmpty(text))
				{
					empty = empty + " و " + text;
				}
			}
			else
			{
				empty += text;
			}
			return empty;
		}
	}

	public sealed class Nl
	{
		private const string ZeroWord = "nul";

		private const string LessWord = "minus";

		private static string[,] triplets;

		private static string[] lessTwenty;

		private static string[] tens;

		static Nl()
		{
			triplets = new string[7, 2];
			lessTwenty = new string[20];
			tens = new string[10];
			triplets[0, 0] = "";
			triplets[1, 0] = " duizend";
			triplets[2, 0] = " miljoen";
			triplets[3, 0] = " miljard";
			triplets[4, 0] = " biljoen";
			triplets[5, 0] = " biljard";
			triplets[6, 0] = " triljoen";
			triplets[0, 1] = "";
			triplets[1, 1] = " duizend";
			triplets[2, 1] = " miljoen";
			triplets[3, 1] = " miljard";
			triplets[4, 1] = " biljoen";
			triplets[5, 1] = " biljard";
			triplets[6, 1] = " triljoen";
			lessTwenty[0] = "";
			lessTwenty[1] = "een";
			lessTwenty[2] = "twee";
			lessTwenty[3] = "drie";
			lessTwenty[4] = "vier";
			lessTwenty[5] = "vijf";
			lessTwenty[6] = "zes";
			lessTwenty[7] = "zeven";
			lessTwenty[8] = "acht";
			lessTwenty[9] = "negen";
			lessTwenty[10] = "tien";
			lessTwenty[11] = "elf";
			lessTwenty[12] = "twaalf";
			lessTwenty[13] = "dertien";
			lessTwenty[14] = "veertien";
			lessTwenty[15] = "vijftien";
			lessTwenty[16] = "zestien";
			lessTwenty[17] = "zeventien";
			lessTwenty[18] = "achtien";
			lessTwenty[19] = "negentien";
			tens[2] = "twintig";
			tens[3] = "dertig";
			tens[4] = "veertig";
			tens[5] = "vijftig";
			tens[6] = "zestig";
			tens[7] = "zeventig";
			tens[8] = "tachtig";
			tens[9] = "negentig";
		}

		public static string ConvertToWord(decimal number, string currencyISO, int decimals)
		{
			CultureInfo culture = new CultureInfo("nl-NL");
			if (number > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), number);
			}
			decimal num = decimal.Truncate(number);
			decimal num2 = Math.Abs((number - num) * (decimal)Math.Pow(10.0, decimals));
			if (num2 > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), num2);
			}
			string integerString = ConvertToWord((long)num, int.Parse(Resource.ResourceManager.GetString(currencyISO + "Gender", culture)), 0);
			string decimalString = ConvertToWord((long)num2, int.Parse(Resource.ResourceManager.GetString(currencyISO + "CentGender", culture)), 0);
			string mainCurrency = "";
			string centCurrency = "";
			string postCurrency = "en";
			NumToWordHelper.determinateCurrencies(culture, currencyISO, num, num2, ref mainCurrency, ref centCurrency);
			string text = NumToWordHelper.AddWords(integerString, decimalString, mainCurrency, centCurrency, postCurrency);
			return text.Trim();
		}

		private static string ConvertToWord(long number, int Gender, int tranche)
		{
			string text = "";
			bool flag = number < 0;
			if (flag)
			{
				number = Math.Abs(number);
			}
			if (number == 0L)
			{
				text = "nul";
			}
			else if (number < 20)
			{
				text = lessTwenty[number];
			}
			else if (number < 100)
			{
				text = ((number % 10 != 0L) ? (ConvertToWord(number % 10, Gender, 0) + " en " + tens[number / 10]) : tens[number / 10]);
			}
			else if (number < 1000)
			{
				text = ConvertToWord(number / 100, Gender, 0) + " Honderd";
				if (number % 100 > 0)
				{
					text = text + ", " + ConvertToWord(number % 100, Gender, 0);
				}
			}
			else
			{
				text = CalculateOver(number, Gender);
			}
			if (flag)
			{
				text = "minus " + text;
			}
			return text.Trim();
		}

		private static string CalculateOver(long number, int gender)
		{
			string result = "";
			string text = "";
			int num = (number.ToString().Length - 1) / 3;
			long num2 = (long)Math.Pow(10.0, num * 3);
			if (number % num2 > 0)
			{
				text = ConvertToWord(number % num2, gender, num);
			}
			long num3 = number / num2;
			if (num3 != 0L)
			{
				result = ConvertToWord(num3, gender, num) + triplets[num, 0] + " " + text;
			}
			return result;
		}
	}

	public sealed class Zh
	{
		private static string[] NumChineseCharacter = new string[10] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };

		public static string ToWordsZh(decimal num)
		{
			if (num <= 0m)
			{
				return "你输入的数字格式不正确或不是数字!";
			}
			string num2 = ((long)num).ToString(CultureInfo.InvariantCulture);
			string text = NumberString(num2);
			return text.Replace("零零", "零");
		}

		public static string ToCurrencyWordsZh(decimal num)
		{
			if (num <= 0m)
			{
				return "你输入的数字格式不正确或不是数字!";
			}
			num = Math.Round(num, 2);
			string text = num.ToString(CultureInfo.InvariantCulture);
			string[] array = text.Split(new char[1] { '.' });
			string text2;
			if (array.Length == 1)
			{
				text2 = NumberString(array[0]) + "元整";
				return text2.Replace("零零", "零");
			}
			text2 = NumberString(array[0]) + "元";
			text2 += FloatString(array[1]);
			return text2.Replace("零零", "零");
		}

		private static string FloatString(string num)
		{
			if (num.Length > 2)
			{
				num = num.Substring(0, 2);
			}
			string text = ConvertString(num);
			if (text.IndexOf("零") != 0)
			{
				text = text.Replace("零", "");
				if (text.Length == 1)
				{
					return text.Substring(0, 1) + "角整";
				}
				string text2 = text.Substring(0, 1) + "角";
				return text2 + text.Substring(1, 1) + "分";
			}
			return text + "分";
		}

		private static string NumberString(string num)
		{
			string result = "";
			if (num.Length <= 4)
			{
				result = Convert4(num);
			}
			else if (num.Length > 4 && num.Length <= 8)
			{
				result = Convert4(num.Substring(0, num.Length - 4)) + "万";
				result += Convert4(num.Substring(num.Length - 4, 4));
			}
			else if (num.Length > 8 && num.Length <= 12)
			{
				result = Convert4(num.Substring(0, num.Length - 8)) + "亿";
				result = ((!(Convert4(num.Substring(num.Length - 8, 4)) == "")) ? (result + Convert4(num.Substring(num.Length - 8, 4)) + "万") : ((!(Convert4(num.Substring(num.Length - 4, 4)) != "")) ? (result ?? "") : (result + "零")));
				result += Convert4(num.Substring(num.Length - 4, 4));
			}
			return result;
		}

		private static string Convert4(string num)
		{
			if (num.Length == 1)
			{
				return ConvertString(num);
			}
			string num2;
			if (num.Length == 2)
			{
				num2 = ConvertString(num);
				return Convert2(num2);
			}
			if (num.Length == 3)
			{
				num2 = ConvertString(num);
				return Convert3(num2);
			}
			num2 = ConvertString(num);
			string text = num2.Substring(0, 4);
			if (text != "零零零零")
			{
				text = num2.Substring(0, 3);
				if (text != "零零零")
				{
					num2 = num2.Replace("零零零", "");
					if (num2.Length == 1)
					{
						return num2.Substring(0, 1) + "仟";
					}
					string text2 = ((!(num2.Substring(0, 1) != "零") || !(num2.Substring(0, 2) != "零")) ? num2.Substring(0, 1) : (num2.Substring(0, 1) + "仟"));
					return text2 + Convert3(num2.Substring(1, 3));
				}
				return num2.Replace("零零零", "零");
			}
			return num2.Replace("零零零零", "");
		}

		private static string ConvertString(string num)
		{
			string text = string.Empty;
			for (int i = 0; i < num.Length; i++)
			{
				text += NumChineseCharacter[int.Parse(num.Substring(i, 1))];
			}
			return text;
		}

		private static string Convert2(string num)
		{
			string text = num.Substring(0, 1);
			if (text != "零")
			{
				string text2 = num.Replace("零", "");
				if (text2.Length == 1)
				{
					return text2.Substring(0, 1) + "拾";
				}
				string text3 = text2.Substring(0, 1) + "拾";
				return text3 + text2.Substring(1, 1);
			}
			return num;
		}

		private static string Convert3(string num)
		{
			string text = num.Substring(0, 2);
			if (text != "零零")
			{
				string text2 = num.Replace("零零", "");
				if (text2.Length == 1)
				{
					return text2.Substring(0, 1) + "佰";
				}
				string text3 = ((!(text2.Substring(0, 1) != "零")) ? text2.Substring(0, 1) : (text2.Substring(0, 1) + "佰"));
				return text3 + Convert2(text2.Substring(1, 2));
			}
			return num.Replace("零零", "零");
		}
	}

	public sealed class Tr
	{
		private static string[] Birler = new string[10] { "", "Bir", "İki", "Üç", "Dört", "Beş", "Altı", "Yedi", "Sekiz", "Dokuz" };

		private static string[] Onlar = new string[10] { "", "On", "Yirmi", "Otuz", "Kırk", "Elli", "Altmış", "Yetmiş", "Seksen", "Doksan" };

		private static string[] Binler = new string[6] { "Katrilyon", "Trilyon", "Milyar", "Milyon", "Bin", "" };

		public static string NumToStr(decimal value)
		{
			string text = value.ToString("000000000000000000.00").Replace('.', ',');
			string text2 = text.Substring(0, text.IndexOf(','));
			string text3 = "";
			int num = 6;
			for (int i = 0; i < num * 3; i += 3)
			{
				string text4 = "";
				if (text2.Substring(i, 1) != "0")
				{
					text4 = text4 + Birler[System.Convert.ToInt32(text2.Substring(i, 1))] + "Yüz";
				}
				if (text4 == "BirYüz")
				{
					text4 = "Yüz";
				}
				text4 += Onlar[System.Convert.ToInt32(text2.Substring(i + 1, 1))];
				text4 += Birler[System.Convert.ToInt32(text2.Substring(i + 2, 1))];
				if (text4 != "")
				{
					text4 += Binler[i / 3];
				}
				if (text4 == "BirBin")
				{
					text4 = "Bin";
				}
				text3 += text4;
			}
			return text3;
		}

		public static string CurrToStr(decimal value, string currencyName = "TL/Kr.", bool showZeroCents = true)
		{
			string text = value.ToString("F2").Replace('.', ',');
			string text2 = text.Substring(text.IndexOf(',') + 1, 2);
			string[] array = currencyName.Split(new char[1] { '/' });
			string text3 = NumToStr(value);
			if (text3.Length > 0)
			{
				text3 = text3 + " " + array[0] + " ";
			}
			int length = text3.Length;
			if (text2.Substring(0, 1) != "0")
			{
				text3 += Onlar[System.Convert.ToInt32(text2.Substring(0, 1))];
			}
			if (text2.Substring(1, 1) != "0")
			{
				text3 += Birler[System.Convert.ToInt32(text2.Substring(1, 1))];
			}
			if (text3.Length > length)
			{
				text3 = text3 + " " + array[1];
			}
			else if (showZeroCents)
			{
				text3 = text3 + "Sıfır " + array[1];
			}
			return text3;
		}
	}

	public sealed class EnGb
	{
		private const string ZeroWord = "zero";

		private const string LessWord = "minus";

		private static string[,] triplets;

		private static string[] lessTwenty;

		private static string[] tens;

		static EnGb()
		{
			triplets = new string[7, 2];
			lessTwenty = new string[20];
			tens = new string[10];
			triplets[0, 0] = "";
			triplets[1, 0] = " thousand";
			triplets[2, 0] = " million";
			triplets[3, 0] = " billion";
			triplets[4, 0] = " trillion";
			triplets[5, 0] = " quadrillion";
			triplets[6, 0] = " qunintillion";
			triplets[0, 1] = "";
			triplets[1, 1] = " thousands";
			triplets[2, 1] = " millions";
			triplets[3, 1] = " milliards";
			triplets[4, 1] = " trillions";
			triplets[5, 1] = " quadrillions";
			triplets[6, 1] = " qunintillions";
			lessTwenty[0] = "";
			lessTwenty[1] = "one";
			lessTwenty[2] = "two";
			lessTwenty[3] = "three";
			lessTwenty[4] = "four";
			lessTwenty[5] = "five";
			lessTwenty[6] = "six";
			lessTwenty[7] = "seven";
			lessTwenty[8] = "eight";
			lessTwenty[9] = "nine";
			lessTwenty[10] = "ten";
			lessTwenty[11] = "eleven";
			lessTwenty[12] = "twelve";
			lessTwenty[13] = "thirteen";
			lessTwenty[14] = "fourteen";
			lessTwenty[15] = "fifteen";
			lessTwenty[16] = "sixteen";
			lessTwenty[17] = "seventeen";
			lessTwenty[18] = "eighteen";
			lessTwenty[19] = "nineteen";
			tens[2] = "twenty";
			tens[3] = "thirty";
			tens[4] = "fourty";
			tens[5] = "fifty";
			tens[6] = "sixty";
			tens[7] = "seventy";
			tens[8] = "eighty";
			tens[9] = "ninety";
		}

		public static string ConvertToWord(decimal number, string currencyISO, int decimals)
		{
			CultureInfo culture = new CultureInfo("en-GB");
			if (number > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), number);
			}
			decimal num = decimal.Truncate(number);
			decimal num2 = Math.Abs((number - num) * (decimal)Math.Pow(10.0, decimals));
			if (num2 > 1000000000000000m)
			{
				throw new NumToWordException(Resource.ResourceManager.GetString("TooLongError", culture), num2);
			}
			string integerString = ConvertToWord((long)num, int.Parse(Resource.ResourceManager.GetString(currencyISO + "Gender", culture)), 0);
			string decimalString = ConvertToWord((long)num2, int.Parse(Resource.ResourceManager.GetString(currencyISO + "CentGender", culture)), 0);
			string mainCurrency = "";
			string centCurrency = "";
			string postCurrency = "and";
			NumToWordHelper.determinateCurrencies(culture, currencyISO, num, num2, ref mainCurrency, ref centCurrency);
			string text = NumToWordHelper.AddWords(integerString, decimalString, mainCurrency, centCurrency, postCurrency);
			return text.Trim();
		}

		private static string ConvertToWord(long number, int Gender, int tranche)
		{
			string text = "";
			bool flag = number < 0;
			if (flag)
			{
				number = Math.Abs(number);
			}
			if (number == 0L)
			{
				text = "zero";
			}
			else if (number < 20)
			{
				text = lessTwenty[number];
			}
			else if (number < 100)
			{
				text = ((number % 10 != 0L) ? (tens[number / 10] + "-" + ConvertToWord(number % 10, Gender, 0)) : tens[number / 10]);
			}
			else if (number < 1000)
			{
				text = ConvertToWord(number / 100, Gender, 0) + " hundred";
				if (number % 100 > 0)
				{
					text = text + " and " + ConvertToWord(number % 100, Gender, 0);
				}
			}
			else
			{
				text = CalculateOver(number, Gender);
			}
			if (flag)
			{
				text = "minus " + text;
			}
			return text.Trim();
		}

		private static string CalculateOver(long number, int gender)
		{
			string result = "";
			string text = "";
			int num = (number.ToString().Length - 1) / 3;
			long num2 = (long)Math.Pow(10.0, num * 3);
			if (number % num2 > 0)
			{
				text = ConvertToWord(number % num2, gender, num);
			}
			long num3 = number / num2;
			if (num3 != 0L)
			{
				result = ConvertToWord(num3, gender, num) + triplets[num, 0] + " " + text;
			}
			return result;
		}
	}

	public sealed class EnIn
	{
		private static Dictionary<string, string> _wordsDictionary;

		private static Dictionary<string, string> WordsDictionary
		{
			get
			{
				if (_wordsDictionary == null)
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary["K_0"] = "Zero";
					dictionary["K_1"] = "One";
					dictionary["K_2"] = "Two";
					dictionary["K_3"] = "Three";
					dictionary["K_4"] = "Four";
					dictionary["K_5"] = "Five";
					dictionary["K_6"] = "Six";
					dictionary["K_7"] = "Seven";
					dictionary["K_8"] = "Eight";
					dictionary["K_9"] = "Nine";
					dictionary["K_10"] = "Ten";
					dictionary["K_11"] = "Eleven";
					dictionary["K_12"] = "Twelve";
					dictionary["K_13"] = "Thirteen";
					dictionary["K_14"] = "Fourteen";
					dictionary["K_15"] = "Fifteen";
					dictionary["K_16"] = "Sixteen";
					dictionary["K_17"] = "Seventeen";
					dictionary["K_18"] = "Eighteen";
					dictionary["K_19"] = "Nineteen";
					dictionary["K_20"] = "Twenty";
					dictionary["K_30"] = "Thirty";
					dictionary["K_40"] = "Forty";
					dictionary["K_50"] = "Fifty";
					dictionary["K_60"] = "Sixty";
					dictionary["K_70"] = "Seventy";
					dictionary["K_80"] = "Eighty";
					dictionary["K_90"] = "Ninety";
					dictionary["K_100"] = "Hundred";
					dictionary["K_1000"] = "Thousand";
					dictionary["K_100000"] = "Lac";
					dictionary["K_10000000"] = "Crore";
					_wordsDictionary = dictionary;
				}
				return _wordsDictionary;
			}
		}

		public static string NumberToStr(long value, bool blankIfZero = false)
		{
			string str = NumberToWords(value, blankIfZero);
			TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
			return textInfo.ToTitleCase(str);
		}

		public static string CurrencyToStr(string currencyBasicUnit, string currencyFractionalUnit, decimal value, int decimalPlaces, bool blankIfZero = false)
		{
			string text = string.Empty;
			if (value < 0m)
			{
				text = "minus ";
				value = Math.Abs(value);
			}
			long number = 0L;
			long number2 = (long)value;
			int num = value.ToString().Replace(",", ".").LastIndexOf(".");
			if (num > 0)
			{
				string text2 = value.ToString().Substring(num + 1);
				text2 = text2.PadRight(decimalPlaces, '0');
				number = System.Convert.ToInt64(text2);
			}
			string value2 = NumberToWords(number2, blankIfZero).Trim();
			string text3 = NumberToWords(number, blankIfZero: true).Trim();
			if (value2.Length > 0 && text3.Length > 0)
			{
				value2 = value2 + " and " + currencyFractionalUnit + " " + text3;
			}
			else if (value2.Length == 0 && text3.Length > 0)
			{
				WordsDictionary.TryGetValue("K_0", out value2);
				value2 = value2 + " and " + currencyFractionalUnit.ToLower() + " " + text3;
			}
			TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
			return textInfo.ToTitleCase(text + currencyBasicUnit + " " + value2 + " Only");
		}

		private static string NumberToWords(long number, bool blankIfZero)
		{
			string value = "";
			string value2 = "";
			long num = 0L;
			long num2 = 0L;
			switch (number)
			{
			case 0L:
				WordsDictionary.TryGetValue("K_" + number, out value);
				if (blankIfZero)
				{
					value = "";
				}
				break;
			case 1L:
			case 2L:
			case 3L:
			case 4L:
			case 5L:
			case 6L:
			case 7L:
			case 8L:
			case 9L:
			case 10L:
			case 11L:
			case 12L:
			case 13L:
			case 14L:
			case 15L:
			case 16L:
			case 17L:
			case 18L:
			case 19L:
				WordsDictionary.TryGetValue("K_" + number, out value);
				break;
			default:
				if (number >= 20 && number < 100)
				{
					num = number / 10 * 10;
					WordsDictionary.TryGetValue("K_" + num, out value);
					num2 = number % 10;
					value = value + " " + NumberToWords(num2, blankIfZero: true);
				}
				else if (number >= 100 && number < 1000)
				{
					num = number / 100;
					value = NumberToWords(num, blankIfZero: true);
					WordsDictionary.TryGetValue("K_100", out value2);
					num2 = number % 100;
					value = value + " " + value2 + " " + NumberToWords(num2, blankIfZero: true);
				}
				else if (number >= 1000 && number < 100000)
				{
					num = number / 1000;
					value = NumberToWords(num, blankIfZero: true);
					WordsDictionary.TryGetValue("K_1000", out value2);
					num2 = number % 1000;
					value = value + " " + value2 + " " + NumberToWords(num2, blankIfZero: true);
				}
				else if (number >= 100000 && number < 10000000)
				{
					num = number / 100000;
					value = NumberToWords(num, blankIfZero: true);
					WordsDictionary.TryGetValue("K_100000", out value2);
					num2 = number % 100000;
					value = value + " " + value2 + " " + NumberToWords(num2, blankIfZero: true);
				}
				else if (number >= 10000000 && number < 1000000000)
				{
					num = number / 10000000;
					value = NumberToWords(num, blankIfZero: true);
					WordsDictionary.TryGetValue("K_10000000", out value2);
					num2 = number % 10000000;
					value = value + " " + value2 + " " + NumberToWords(num2, blankIfZero: true);
				}
				else
				{
					if (number < 1000000000 || number >= 1000000000000000L)
					{
						throw new Exception("number to big to convert.");
					}
					num = number / 10000000;
					value = NumberToWords(num, blankIfZero: true);
					WordsDictionary.TryGetValue("K_10000000", out value2);
					value = value + " " + value2;
					value2 = "";
					num2 = number % 10000000;
					value = value + " " + NumberToWords(num2, blankIfZero: true);
				}
				break;
			}
			return value;
		}
	}

	public sealed class Ar
	{
		private const string MyAnd = " و";

		private static readonly string[] MyArry1;

		private static readonly string[] MyArry2;

		private static readonly string[] MyArry3;

		static Ar()
		{
			MyArry1 = new string[10];
			MyArry2 = new string[10];
			MyArry3 = new string[10];
			MyArry1[0] = "";
			MyArry1[1] = "مائة";
			MyArry1[2] = "مائتان";
			MyArry1[3] = "ثلاثمائة";
			MyArry1[4] = "أربعمائة";
			MyArry1[5] = "خمسمائة";
			MyArry1[6] = "ستمائة";
			MyArry1[7] = "سبعمائة";
			MyArry1[8] = "ثمانمائة";
			MyArry1[9] = "تسعمائة";
			MyArry2[0] = "";
			MyArry2[1] = " عشر";
			MyArry2[2] = "عشرون";
			MyArry2[3] = "ثلاثون";
			MyArry2[4] = "أربعون";
			MyArry2[5] = "خمسون";
			MyArry2[6] = "ستون";
			MyArry2[7] = "سبعون";
			MyArry2[8] = "ثمانون";
			MyArry2[9] = "تسعون";
			MyArry3[0] = "";
			MyArry3[1] = "واحد";
			MyArry3[2] = "اثنان";
			MyArry3[3] = "ثلاثة";
			MyArry3[4] = "أربعة";
			MyArry3[5] = "خمسة";
			MyArry3[6] = "ستة";
			MyArry3[7] = "سبعة";
			MyArry3[8] = "ثمانية";
			MyArry3[9] = "تسعة";
		}

		public static string NumToStr(double number, string currency, string subCurrency)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			if (number > 999999999999.99)
			{
				return "";
			}
			if (number == 0.0)
			{
				return "صفر";
			}
			string text6 = number.ToString("000000000000.00");
			for (int i = 0; i < 15; i += 3)
			{
				string text7 = ((i < 12) ? text6.Substring(i, 3) : ("0" + text6.Substring(i + 1, 2)));
				int num = int.Parse(text7.Substring(0, 1));
				int num2 = int.Parse(text7.Substring(1, 1));
				int num3 = int.Parse(text7.Substring(2, 1));
				int num4 = int.Parse(text7.Substring(1, 2));
				int num5 = int.Parse(text7.Substring(0, 3));
				if (num5 <= 0)
				{
					continue;
				}
				string text8 = MyArry1[num];
				string text9 = MyArry2[num2];
				string text10 = MyArry3[num3];
				string text11 = "";
				string text12 = "";
				if (num4 == 11)
				{
					text11 = "إحدى عشر";
				}
				if (num4 == 12)
				{
					text12 = "إثنى عشر";
				}
				if (num4 == 10)
				{
					text9 = "عشرة";
				}
				if (num > 0 && num4 > 0)
				{
					text8 += " و";
				}
				if (num3 > 0 && num2 > 1)
				{
					text10 += " و";
				}
				string text13 = text8 + text10 + text9;
				if (num3 == 1 && num2 == 1)
				{
					text13 = text8 + text11;
					if (num == 0)
					{
						text13 = text11;
					}
				}
				if (num3 == 2 && num2 == 1)
				{
					text13 = text8 + text12;
					if (num == 0)
					{
						text13 = text12;
					}
				}
				if (i == 0 && !string.IsNullOrWhiteSpace(text13))
				{
					if (num5 > 10)
					{
						text = text13 + " مليار";
					}
					else
					{
						text = text13 + " مليارات";
						if (num5 == 1)
						{
							text = " مليار";
						}
						if (num5 == 2)
						{
							text = " ملياران";
						}
					}
				}
				if (i == 3 && !string.IsNullOrWhiteSpace(text13))
				{
					if (num5 > 10)
					{
						text2 = text13 + " مليون";
					}
					else
					{
						text2 = text13 + " ملايين";
						if (num5 == 1)
						{
							text2 = " مليون";
						}
						if (num5 == 2)
						{
							text2 = " مليونان";
						}
					}
				}
				if (i == 6 && !string.IsNullOrWhiteSpace(text13))
				{
					if (num5 > 10)
					{
						text3 = text13 + " ألف";
					}
					else
					{
						text3 = text13 + " آلاف";
						if (num3 == 1)
						{
							text3 = " ألف";
						}
						if (num3 == 2)
						{
							text3 = " ألفان";
						}
					}
				}
				if (i == 9 && !string.IsNullOrWhiteSpace(text13))
				{
					text4 = text13;
				}
				if (i == 12 && !string.IsNullOrWhiteSpace(text13))
				{
					text5 = text13;
				}
			}
			if (!string.IsNullOrWhiteSpace(text) && (text2.Length > 0 || text3.Length > 0 || text4.Length > 0))
			{
				text += " و";
			}
			if (!string.IsNullOrWhiteSpace(text2) && (text3.Length > 0 || text4.Length > 0))
			{
				text2 += " و";
			}
			if (!string.IsNullOrWhiteSpace(text3) && text4.Length > 0)
			{
				text3 += " و";
			}
			string text14 = "";
			if (!string.IsNullOrWhiteSpace(text5))
			{
				if (text.Length > 0 || text2.Length > 0 || text3.Length > 0 || text4.Length > 0)
				{
					return text + text2 + text3 + text4 + " " + currency + " و" + text5 + " " + subCurrency;
				}
				return text5 + " " + subCurrency;
			}
			return text + text2 + text3 + text4 + " " + currency;
		}
	}

	public static class NumToWordHelper
	{
		public const decimal MaxValue = 1000000000000000m;

		private static void AddWord(ref string value, string word, string separator)
		{
			if (!string.IsNullOrEmpty(word))
			{
				value = value + separator + word;
			}
		}

		public static string AddWords(string integerString, string decimalString, string mainCurrency, string centCurrency, string postCurrency)
		{
			string value = "";
			AddWord(ref value, integerString, " ");
			AddWord(ref value, mainCurrency, " ");
			AddWord(ref value, postCurrency, " ");
			AddWord(ref value, decimalString, " ");
			AddWord(ref value, centCurrency, " ");
			return value;
		}

		public static void determinateCurrencies(CultureInfo culture, string currencyISO, decimal integerPart, decimal decimalPart, ref string mainCurrency, ref string centCurrency)
		{
			if (integerPart / 1000000m > 0m && integerPart % 1000000m == 0m)
			{
				mainCurrency = Resource.ResourceManager.GetString(currencyISO + "BigSeparator", culture);
			}
			else if (integerPart != 1m)
			{
				mainCurrency = Resource.ResourceManager.GetString(currencyISO + "Plural", culture);
			}
			else
			{
				mainCurrency = Resource.ResourceManager.GetString(currencyISO + "Single", culture);
			}
			if (decimalPart != 1m)
			{
				centCurrency = Resource.ResourceManager.GetString(currencyISO + "CentPlural", culture);
			}
			else
			{
				centCurrency = Resource.ResourceManager.GetString(currencyISO + "CentSingle", culture);
			}
		}
	}

	public class NumToWordException : Exception
	{
		public NumToWordException(string message, decimal number)
			: base(message + " : " + number)
		{
		}
	}

	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resource
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					ResourceManager resourceManager = new ResourceManager("Stimulsoft.Report.Resource", typeof(Resource).Assembly);
					resourceMan = resourceManager;
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static string EURBigSeparator => ResourceManager.GetString("EURBigSeparator", resourceCulture);

		internal static string EURCentGender => ResourceManager.GetString("EURCentGender", resourceCulture);

		internal static string EURCentPlural => ResourceManager.GetString("EURCentPlural", resourceCulture);

		internal static string EURCentSingle => ResourceManager.GetString("EURCentSingle", resourceCulture);

		internal static string EURGender => ResourceManager.GetString("EURGender", resourceCulture);

		internal static string EURPlural => ResourceManager.GetString("EURPlural", resourceCulture);

		internal static string EURSingle => ResourceManager.GetString("EURSingle", resourceCulture);

		internal static string GBPBigSeparator => ResourceManager.GetString("GBPBigSeparator", resourceCulture);

		internal static string GBPCentGender => ResourceManager.GetString("GBPCentGender", resourceCulture);

		internal static string GBPCentPlural => ResourceManager.GetString("GBPCentPlural", resourceCulture);

		internal static string GBPCentSingle => ResourceManager.GetString("GBPCentSingle", resourceCulture);

		internal static string GBPGender => ResourceManager.GetString("GBPGender", resourceCulture);

		internal static string GBPPlural => ResourceManager.GetString("GBPPlural", resourceCulture);

		internal static string GBPSingle => ResourceManager.GetString("GBPSingle", resourceCulture);

		internal static string TooLongError => ResourceManager.GetString("TooLongError", resourceCulture);

		internal Resource()
		{
		}
	}

	public sealed class Convert
	{
		public static string ToRoman(int value)
		{
			return Funcs.ToRoman(value);
		}

		public static string ToABC(int value)
		{
			return Funcs.ToABC(value);
		}

		public static string ToABCNumeric(int value)
		{
			return Funcs.ToABCNumeric(value);
		}

		public static string ToABCRu(int value)
		{
			return Funcs.ToABCRu(value);
		}

		public static string ToArabic(int value, bool useEasternDigits)
		{
			return Funcs.ToArabic(value, useEasternDigits);
		}

		public static string ToArabic(string value, bool useEasternDigits)
		{
			return Funcs.ToArabic(value, useEasternDigits);
		}
	}

	public sealed class EngineHelper
	{
		public static string JoinColumnContent(StiDataSource dataSource, string columnName, string delimiter, bool distinct = false)
		{
			List<string> list = new List<string>();
			Hashtable hashtable = new Hashtable();
			if (!dataSource.IsEmpty)
			{
				StiParserParameters parameters = new StiParserParameters();
				StiCalcDataColumn stiCalcDataColumn = dataSource.Columns[columnName] as StiCalcDataColumn;
				StiPage stiPage = dataSource.Dictionary?.Report?.Pages[0];
				bool flag = stiCalcDataColumn != null && stiPage != null;
				dataSource.SaveState("JoinColumnContent");
				dataSource.First();
				while (!dataSource.IsEof)
				{
					string text = null;
					text = ((!flag) ? dataSource[columnName].ToString() : StiParser.ParseTextValue("{" + stiCalcDataColumn.Value + "}", stiPage, parameters).ToString());
					bool flag2 = true;
					if (distinct)
					{
						if (hashtable.ContainsKey(text))
						{
							flag2 = false;
						}
						else
						{
							hashtable[text] = null;
						}
					}
					if (flag2)
					{
						list.Add(text);
					}
					dataSource.Next();
				}
				dataSource.RestoreState("JoinColumnContent");
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append(list[i]);
				if (i < list.Count - 1)
				{
					stringBuilder.Append(delimiter);
				}
			}
			return stringBuilder.ToString();
		}

		public static string JoinColumnContent(StiBusinessObject businessObject, string columnName, string delimiter, bool distinct = false)
		{
			List<string> list = new List<string>();
			Hashtable hashtable = new Hashtable();
			if (!businessObject.IsEmpty && businessObject.Count > 0)
			{
				businessObject.SaveState("JoinColumnContent");
				businessObject.CreateEnumerator();
				string[] array = columnName.Split(new char[1] { '.' });
				while (!businessObject.IsEof)
				{
					StiBusinessObject stiBusinessObject = businessObject;
					string text = null;
					int i;
					for (i = 0; i < array.Length - 1; i++)
					{
						text = array[i];
						if (stiBusinessObject.Columns.Contains(text))
						{
							break;
						}
						stiBusinessObject = stiBusinessObject.BusinessObjects[text];
					}
					object obj = stiBusinessObject[array[i]];
					string text2 = obj.ToString();
					bool flag = true;
					if (distinct)
					{
						if (hashtable.ContainsKey(text2))
						{
							flag = false;
						}
						else
						{
							hashtable[text2] = null;
						}
					}
					if (flag)
					{
						list.Add(text2);
					}
					businessObject.Next();
				}
				businessObject.RestoreState("JoinColumnContent");
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < list.Count; j++)
			{
				stringBuilder.Append(list[j]);
				if (j < list.Count - 1)
				{
					stringBuilder.Append(delimiter);
				}
			}
			return stringBuilder.ToString();
		}

		public static string ToQueryString<T>(List<T> list, string quotationMark, string dateTimeFormat, bool needEscape = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (T item in list)
			{
				object obj = item;
				if (num > 0)
				{
					stringBuilder.Append(", ");
				}
				string text = ((!string.IsNullOrEmpty(dateTimeFormat)) ? System.Convert.ToDateTime(obj).ToString(dateTimeFormat) : obj.ToString());
				if (needEscape)
				{
					text = text.Replace("\\", "\\\\").Replace("'", "\\'");
					if (!string.IsNullOrEmpty(quotationMark) && quotationMark != "'")
					{
						text = text.Replace(quotationMark, quotationMark + quotationMark);
					}
				}
				else if (!string.IsNullOrEmpty(quotationMark))
				{
					text = text.Replace(quotationMark, quotationMark + quotationMark);
				}
				stringBuilder.Append(quotationMark + text + quotationMark);
				num++;
			}
			return stringBuilder.ToString();
		}

		public static int GetRealPageNumber(object sender)
		{
			int num = 0;
			if (sender is StiComponent stiComponent)
			{
				num = stiComponent.Report.PageNumber;
				StiPage page = stiComponent.Page;
				if (page != null)
				{
					int num2 = stiComponent.Report.RenderedPages.IndexOf(page);
					if (num2 != -1 && stiComponent.Report.Engine != null && stiComponent.Report.Engine.PageNumbers != null)
					{
						num = stiComponent.Report.Engine.PageNumbers.GetPageNumber(num2);
					}
				}
				decimal num3 = (decimal)(page.PageWidth - page.Margins.Left - page.Margins.Right);
				for (decimal num4 = (decimal)stiComponent.Left; num4 > num3; num4 -= num3)
				{
					num++;
				}
				decimal num5 = (decimal)(page.PageHeight - page.Margins.Top - page.Margins.Bottom);
				for (decimal num6 = (decimal)stiComponent.Top; num6 > num5; num6 -= num5)
				{
					num += page.SegmentPerWidth;
				}
			}
			return num;
		}
	}

	public sealed class MonthToStr
	{
		public static string MonthName(DateTime dateTime)
		{
			return StiMonthToStrHelper.MonthName(dateTime);
		}

		public static string MonthName(DateTime dateTime, bool localized)
		{
			return StiMonthToStrHelper.MonthName(dateTime, localized);
		}

		public static string MonthName(DateTime dateTime, string culture)
		{
			return StiMonthToStrHelper.MonthName(dateTime, culture);
		}

		public static string MonthName(DateTime dateTime, string culture, bool upperCase)
		{
			return StiMonthToStrHelper.MonthName(dateTime, culture, upperCase);
		}

		public static void AddCulture(string[] monthNames, string[] cultureNames, bool defaultUpperCase)
		{
			StiMonthToStrHelper.AddCulture(monthNames, cultureNames, defaultUpperCase);
		}
	}

	public sealed class DayOfWeekToStr
	{
		public static string DayOfWeek(DateTime date)
		{
			return StiDayOfWeekToStrHelper.DayOfWeek(date);
		}

		public static string DayOfWeek(DateTime date, bool localized)
		{
			return StiDayOfWeekToStrHelper.DayOfWeek(date, localized);
		}

		public static string DayOfWeek(DateTime dateTime, string culture)
		{
			return StiDayOfWeekToStrHelper.DayOfWeek(dateTime, culture);
		}

		public static string DayOfWeek(DateTime dateTime, string culture, bool upperCase)
		{
			return StiDayOfWeekToStrHelper.DayOfWeek(dateTime, culture, upperCase);
		}

		public static void AddCulture(string[] monthNames, string[] cultureNames, bool defaultUpperCase)
		{
			StiDayOfWeekToStrHelper.AddCulture(monthNames, cultureNames, defaultUpperCase);
		}
	}

	private Func()
	{
	}
}
