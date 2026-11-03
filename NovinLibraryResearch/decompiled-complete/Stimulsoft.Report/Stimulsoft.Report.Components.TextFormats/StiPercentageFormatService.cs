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

namespace Stimulsoft.Report.Components.TextFormats;

[TypeConverter(typeof(StiPercentageFormatConverter))]
[StiServiceBitmap(typeof(StiPercentageFormatService), "Stimulsoft.Report.Images.TextFormats.FormatPercentage.png")]
[StiFormatEditor("Stimulsoft.Report.Components.TextFormats.Design.StiPercentageEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfFormatEditor("Stimulsoft.Report.WpfDesign.StiPercentageEditor, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiPercentageFormatService : StiCurrencyFormatService
{
	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "Percentage");

	[JsonIgnore]
	public override int Position => 6;

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

	[JsonIgnore]
	public override object Sample => -1.2312;

	[JsonIgnore]
	public override string NativeFormatString => "{0:P2}";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "StringFormat");
		return val;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiPercentageFormatService obj2))
		{
			return false;
		}
		if (!base.Equals((object)obj2))
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
				numberFormatInfo.PercentDecimalDigits = base.DecimalDigits;
			}
			if ((base.State & StiTextFormatState.DecimalSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(base.DecimalSeparator))
			{
				numberFormatInfo.PercentDecimalSeparator = base.DecimalSeparator;
			}
			if ((base.State & StiTextFormatState.GroupSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(base.GroupSeparator))
			{
				numberFormatInfo.PercentGroupSeparator = base.GroupSeparator;
			}
			if ((base.State & StiTextFormatState.GroupSize) > StiTextFormatState.None)
			{
				numberFormatInfo.PercentGroupSizes = new int[1] { base.GroupSize };
			}
			if ((base.State & StiTextFormatState.NegativePattern) > StiTextFormatState.None)
			{
				numberFormatInfo.PercentNegativePattern = base.NegativePattern;
			}
			if ((base.State & StiTextFormatState.PositivePattern) > StiTextFormatState.None)
			{
				numberFormatInfo.PercentPositivePattern = base.PositivePattern;
			}
			if ((base.State & StiTextFormatState.PercentageSymbol) > StiTextFormatState.None)
			{
				numberFormatInfo.PercentSymbol = base.Symbol;
			}
		}
		else
		{
			numberFormatInfo.PercentDecimalSeparator = (string.IsNullOrEmpty(base.DecimalSeparator) ? CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator : base.DecimalSeparator);
			numberFormatInfo.PercentDecimalDigits = base.DecimalDigits;
			numberFormatInfo.PercentGroupSeparator = base.GroupSeparator;
			numberFormatInfo.PercentGroupSizes = new int[1] { base.GroupSize };
			numberFormatInfo.PercentPositivePattern = base.PositivePattern;
			numberFormatInfo.PercentNegativePattern = base.NegativePattern;
			numberFormatInfo.PercentSymbol = base.Symbol;
		}
		if (!base.UseGroupSeparator)
		{
			numberFormatInfo.PercentGroupSeparator = string.Empty;
		}
		if (StiOptions.Engine.AllowConvertingInFormatting && arg?.GetType() != null && !TypeExt.IsNumericType(arg.GetType()))
		{
			decimal? num = StiValueHelper.TryToNullableDecimal(arg);
			if (num.HasValue)
			{
				arg = num;
			}
		}
		return string.Format(numberFormatInfo, "{0:P}", new object[1] { arg });
	}

	public override StiFormatService CreateNew()
	{
		return new StiPercentageFormatService();
	}

	public StiPercentageFormatService()
		: this(CultureInfo.CurrentCulture.NumberFormat.PercentPositivePattern, CultureInfo.CurrentCulture.NumberFormat.PercentNegativePattern, CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits, CultureInfo.CurrentCulture.NumberFormat.PercentGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.PercentGroupSizes[0], CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, useGroupSeparator: true, useLocalSetting: true, " ")
	{
	}

	public StiPercentageFormatService(int positivePattern, int negativePattern, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, string symbol, bool useGroupSeparator, bool useLocalSetting, string nullDisplay)
		: this(positivePattern, negativePattern, 0, decimalSeparator, decimalDigits, groupSeparator, groupSize, symbol, useGroupSeparator, useLocalSetting, nullDisplay)
	{
	}

	public StiPercentageFormatService(int positivePattern, int negativePattern, int decimalPlaces, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, string symbol, bool useGroupSeparator, bool useLocalSetting, string nullDisplay)
		: this(positivePattern, negativePattern, decimalPlaces, decimalSeparator, decimalDigits, groupSeparator, groupSize, symbol, useGroupSeparator, useLocalSetting, nullDisplay, StiTextFormatState.None)
	{
	}

	public StiPercentageFormatService(int positivePattern, int negativePattern, int decimalPlaces, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, string symbol, bool useGroupSeparator, bool useLocalSetting, string nullDisplay, StiTextFormatState state)
	{
		base.DecimalDigits = decimalDigits;
		base.DecimalSeparator = decimalSeparator;
		base.GroupSeparator = groupSeparator;
		base.GroupSize = groupSize;
		base.NegativePattern = negativePattern;
		base.PositivePattern = positivePattern;
		base.Symbol = symbol;
		base.UseGroupSeparator = useGroupSeparator;
		base.UseLocalSetting = useLocalSetting;
		base.NullDisplay = nullDisplay;
		base.State = state;
	}
}
