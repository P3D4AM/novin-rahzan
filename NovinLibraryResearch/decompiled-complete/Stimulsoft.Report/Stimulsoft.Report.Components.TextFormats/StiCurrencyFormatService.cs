using System;
using System.ComponentModel;
using System.Globalization;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.TextFormats.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components.TextFormats;

[TypeConverter(typeof(StiCurrencyFormatConverter))]
[StiServiceBitmap(typeof(StiCurrencyFormatService), "Stimulsoft.Report.Images.TextFormats.FormatCurrency.png")]
[StiFormatEditor("Stimulsoft.Report.Components.TextFormats.Design.StiCurrencyEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfFormatEditor("Stimulsoft.Report.WpfDesign.StiCurrencyEditor, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiCurrencyFormatService : StiNumberFormatService
{
	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "Currency");

	[JsonIgnore]
	public override int Position => 3;

	[JsonIgnore]
	public override object Sample => -1234.12345679;

	[JsonIgnore]
	public override string NativeFormatString => "{0:C2}";

	[StiNonSerialized]
	[JsonIgnore]
	public override string StringFormat
	{
		get
		{
			return base.StringFormat;
		}
		set
		{
			base.StringFormat = value;
		}
	}

	[StiSerializable]
	public int PositivePattern { get; set; }

	[StiSerializable]
	[DefaultValue("$")]
	public string Symbol { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "StringFormat");
		JsonExtensions.AddPropertyIntNoDefaultValue(val, "PositivePattern", PositivePattern);
		JsonExtensions.AddPropertyString(val, "Symbol", Symbol, "$");
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "PositivePattern"))
			{
				if (name == "Symbol")
				{
					Symbol = StiJsonReportObjectHelper.DeserializeString(item);
				}
			}
			else
			{
				PositivePattern = StiJsonReportObjectHelper.DeserializeInt(item);
			}
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiCurrencyFormatService stiCurrencyFormatService))
		{
			return false;
		}
		if (!base.Equals((object)stiCurrencyFormatService))
		{
			return false;
		}
		if (PositivePattern != stiCurrencyFormatService.PositivePattern)
		{
			return false;
		}
		if (Symbol != stiCurrencyFormatService.Symbol)
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string Format(string stringFormat, object arg)
	{
		if (arg == null || arg is DBNull)
		{
			return base.NullDisplay;
		}
		NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
		if (base.UseLocalSetting)
		{
			StiNumberFormatService.FillLocalSetting(numberFormatInfo);
			if ((base.State & StiTextFormatState.DecimalDigits) > StiTextFormatState.None)
			{
				numberFormatInfo.CurrencyDecimalDigits = base.DecimalDigits;
			}
			if ((base.State & StiTextFormatState.DecimalSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(base.DecimalSeparator))
			{
				numberFormatInfo.CurrencyDecimalSeparator = base.DecimalSeparator;
			}
			if ((base.State & StiTextFormatState.GroupSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(base.GroupSeparator))
			{
				numberFormatInfo.CurrencyGroupSeparator = base.GroupSeparator;
			}
			if ((base.State & StiTextFormatState.GroupSize) > StiTextFormatState.None)
			{
				numberFormatInfo.CurrencyGroupSizes = new int[1] { base.GroupSize };
			}
			if ((base.State & StiTextFormatState.NegativePattern) > StiTextFormatState.None)
			{
				numberFormatInfo.CurrencyNegativePattern = base.NegativePattern;
			}
			if ((base.State & StiTextFormatState.PositivePattern) > StiTextFormatState.None)
			{
				numberFormatInfo.CurrencyPositivePattern = PositivePattern;
			}
			if ((base.State & StiTextFormatState.CurrencySymbol) > StiTextFormatState.None)
			{
				numberFormatInfo.CurrencySymbol = Symbol;
			}
		}
		else
		{
			numberFormatInfo.CurrencyDecimalSeparator = (string.IsNullOrEmpty(base.DecimalSeparator) ? CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator : base.DecimalSeparator);
			numberFormatInfo.CurrencyDecimalDigits = base.DecimalDigits;
			if (base.GroupSeparator != null)
			{
				numberFormatInfo.CurrencyGroupSeparator = base.GroupSeparator;
			}
			numberFormatInfo.CurrencyGroupSizes = new int[1] { base.GroupSize };
			numberFormatInfo.CurrencyPositivePattern = PositivePattern;
			numberFormatInfo.CurrencyNegativePattern = base.NegativePattern;
			if (Symbol != null)
			{
				numberFormatInfo.CurrencySymbol = Symbol;
			}
		}
		if (!base.UseGroupSeparator)
		{
			numberFormatInfo.CurrencyGroupSeparator = string.Empty;
		}
		if (StiOptions.Engine.AllowConvertingInFormatting && arg?.GetType() != null && !TypeExt.IsNumericType(arg.GetType()))
		{
			decimal? num = StiValueHelper.TryToNullableDecimal(arg);
			if (num.HasValue)
			{
				arg = num;
			}
		}
		if ((base.State & StiTextFormatState.Abbreviation) > StiTextFormatState.None)
		{
			decimal? num2 = StiValueHelper.TryToNullableDecimal(arg);
			if (num2.HasValue)
			{
				if (base.UseLocalSetting)
				{
					if ((base.State & StiTextFormatState.DecimalDigits) > StiTextFormatState.None)
					{
						numberFormatInfo.NumberDecimalDigits = base.DecimalDigits;
					}
					if ((base.State & StiTextFormatState.DecimalSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(base.DecimalSeparator))
					{
						numberFormatInfo.NumberDecimalSeparator = base.DecimalSeparator;
					}
					if ((base.State & StiTextFormatState.GroupSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(base.GroupSeparator))
					{
						numberFormatInfo.NumberGroupSeparator = base.GroupSeparator;
					}
					if ((base.State & StiTextFormatState.GroupSize) > StiTextFormatState.None)
					{
						numberFormatInfo.NumberGroupSizes = new int[1] { base.GroupSize };
					}
				}
				num2 = StiAbbreviationNumberFormatHelper.Format(num2.Value, out var postfix, numberFormatInfo.NumberDecimalDigits, base.TotalNumberCapacity);
				string str = ((!base.TotalNumberCapacity.HasValue) ? (string.Format(numberFormatInfo, "{0:N}", new object[1] { num2 }) + postfix) : $"{num2}{postfix}");
				return FormatAsCurrency(num2.Value, str);
			}
		}
		return string.Format(numberFormatInfo, "{0:C}", new object[1] { arg });
	}

	private string FormatAsCurrency(decimal value, string str)
	{
		string currencySymbol = GetCurrencySymbol();
		if (value >= 0m)
		{
			string positivePattern = GetPositivePattern();
			return positivePattern.Replace("n", str).Replace("$", currencySymbol);
		}
		str = str.Replace("-", "");
		string negativePattern = GetNegativePattern();
		return negativePattern.Replace("n", str).Replace("$", currencySymbol);
	}

	private string GetCurrencySymbol()
	{
		if (!base.UseLocalSetting || (base.State & StiTextFormatState.CurrencySymbol) != 0)
		{
			return Symbol;
		}
		return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
	}

	private string GetPositivePattern()
	{
		int num = ((base.UseLocalSetting && (base.State & StiTextFormatState.PositivePattern) == 0) ? CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern : PositivePattern);
		string[] array = new string[4] { "$n", "n$", "$ n", "n $" };
		if (num >= 0 && num < array.Length)
		{
			return array[num];
		}
		return "$n";
	}

	private string GetNegativePattern()
	{
		int num = ((base.UseLocalSetting && (base.State & StiTextFormatState.NegativePattern) == 0) ? CultureInfo.CurrentCulture.NumberFormat.CurrencyNegativePattern : base.NegativePattern);
		string[] array = new string[16]
		{
			"($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n",
			"n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)"
		};
		if (num >= 0 && num < array.Length)
		{
			return array[num];
		}
		return "($n)";
	}

	public override StiFormatService CreateNew()
	{
		return new StiCurrencyFormatService();
	}

	public StiCurrencyFormatService()
		: this(CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern, CultureInfo.CurrentCulture.NumberFormat.CurrencyNegativePattern, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits, CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSizes[0], CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, useGroupSeparator: true, useLocalSetting: true, " ")
	{
	}

	public StiCurrencyFormatService(int positivePattern, int negativePattern, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, string symbol, bool useGroupSeparator, bool useLocalSetting, string nullDisplay)
		: this(positivePattern, negativePattern, 0, decimalSeparator, decimalDigits, groupSeparator, groupSize, symbol, useGroupSeparator, useLocalSetting, nullDisplay)
	{
	}

	public StiCurrencyFormatService(int positivePattern, int negativePattern, int decimalPlaces, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, string symbol, bool useGroupSeparator, bool useLocalSetting, string nullDisplay)
		: this(positivePattern, negativePattern, decimalPlaces, decimalSeparator, decimalDigits, groupSeparator, groupSize, symbol, useGroupSeparator, useLocalSetting, nullDisplay, StiTextFormatState.None)
	{
	}

	public StiCurrencyFormatService(int positivePattern, int negativePattern, int decimalPlaces, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, string symbol, bool useGroupSeparator, bool useLocalSetting, string nullDisplay, StiTextFormatState state)
	{
		base.DecimalDigits = decimalDigits;
		base.DecimalSeparator = decimalSeparator;
		base.GroupSeparator = groupSeparator;
		base.GroupSize = groupSize;
		base.NegativePattern = negativePattern;
		PositivePattern = positivePattern;
		Symbol = symbol;
		base.UseGroupSeparator = useGroupSeparator;
		base.UseLocalSetting = useLocalSetting;
		base.NullDisplay = nullDisplay;
		base.State = state;
	}
}
