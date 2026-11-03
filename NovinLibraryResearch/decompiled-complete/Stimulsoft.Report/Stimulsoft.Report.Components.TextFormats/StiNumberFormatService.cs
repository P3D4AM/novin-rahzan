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

[TypeConverter(typeof(StiNumberFormatConverter))]
[StiServiceBitmap(typeof(StiNumberFormatService), "Stimulsoft.Report.Images.TextFormats.FormatNumber.png")]
[StiFormatEditor("Stimulsoft.Report.Components.TextFormats.Design.StiNumberEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfFormatEditor("Stimulsoft.Report.WpfDesign.StiNumberEditor, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiNumberFormatService : StiFormatService
{
	private class bitsNumberFormatService : ICloneable
	{
		public int negativePattern;

		public string decimalSeparator;

		public int decimalDigits;

		public string groupSeparator;

		public int groupSize;

		public bool useGroupSeparator;

		public bool useLocalSetting;

		public string nullDisplay;

		public StiTextFormatState states;

		public object Clone()
		{
			return MemberwiseClone();
		}

		public bitsNumberFormatService(int negativePattern, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, bool useGroupSeparator, bool useLocalSetting, string nullDisplay, StiTextFormatState states)
		{
			this.negativePattern = negativePattern;
			this.decimalSeparator = decimalSeparator;
			this.decimalDigits = decimalDigits;
			this.groupSeparator = groupSeparator;
			this.groupSize = groupSize;
			this.useGroupSeparator = useGroupSeparator;
			this.useLocalSetting = useLocalSetting;
			this.nullDisplay = nullDisplay;
			this.states = states;
		}
	}

	private bitsNumberFormatService bits;

	[JsonIgnore]
	public override string ServiceName => Loc.Get("FormFormatEditor", "Number");

	[JsonIgnore]
	public override int Position => 2;

	[JsonIgnore]
	public override object Sample => -1234.123456789;

	[JsonIgnore]
	public override string NativeFormatString => "{0:N2}";

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

	[DefaultValue(" ")]
	[StiSerializable]
	public string NullDisplay
	{
		get
		{
			if (bits != null)
			{
				return bits.nullDisplay;
			}
			return " ";
		}
		set
		{
			if (!(value == " ") || bits != null)
			{
				if (bits != null)
				{
					bits.nullDisplay = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, DecimalSeparator, DecimalDigits, GroupSeparator, GroupSize, UseGroupSeparator, UseLocalSetting, value, State);
				}
			}
		}
	}

	[StiSerializable]
	public int NegativePattern
	{
		get
		{
			if (bits != null)
			{
				return bits.negativePattern;
			}
			return 1;
		}
		set
		{
			if (value != 1 || bits != null)
			{
				if (bits != null)
				{
					bits.negativePattern = value;
				}
				else
				{
					bits = new bitsNumberFormatService(value, DecimalSeparator, DecimalDigits, GroupSeparator, GroupSize, UseGroupSeparator, UseLocalSetting, NullDisplay, State);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(".")]
	public string DecimalSeparator
	{
		get
		{
			if (bits != null)
			{
				return bits.decimalSeparator;
			}
			return ".";
		}
		set
		{
			if (!(value == ".") || bits != null)
			{
				if (bits != null)
				{
					bits.decimalSeparator = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, value, DecimalDigits, GroupSeparator, GroupSize, UseGroupSeparator, UseLocalSetting, NullDisplay, State);
				}
			}
		}
	}

	[StiSerializable]
	public int DecimalDigits
	{
		get
		{
			if (bits != null)
			{
				return bits.decimalDigits;
			}
			return 2;
		}
		set
		{
			if (value != 2 || bits != null)
			{
				if (bits != null)
				{
					bits.decimalDigits = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, DecimalSeparator, value, GroupSeparator, GroupSize, UseGroupSeparator, UseLocalSetting, NullDisplay, State);
				}
			}
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	internal int? TotalNumberCapacity { get; set; }

	[StiSerializable]
	public string GroupSeparator
	{
		get
		{
			if (bits != null)
			{
				return bits.groupSeparator;
			}
			return " ";
		}
		set
		{
			if (!(value == " ") || bits != null)
			{
				if (bits != null)
				{
					bits.groupSeparator = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, DecimalSeparator, DecimalDigits, value, GroupSize, UseGroupSeparator, UseLocalSetting, NullDisplay, State);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(3)]
	public int GroupSize
	{
		get
		{
			if (bits != null)
			{
				return bits.groupSize;
			}
			return 3;
		}
		set
		{
			if (value != 3 || bits != null)
			{
				if (value > 9)
				{
					value = 9;
				}
				if (bits != null)
				{
					bits.groupSize = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, DecimalSeparator, DecimalDigits, GroupSeparator, value, UseGroupSeparator, UseLocalSetting, NullDisplay, State);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	public bool UseGroupSeparator
	{
		get
		{
			if (bits != null)
			{
				return bits.useGroupSeparator;
			}
			return true;
		}
		set
		{
			if (!value || bits != null)
			{
				if (bits != null)
				{
					bits.useGroupSeparator = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, DecimalSeparator, DecimalDigits, GroupSeparator, GroupSize, value, UseLocalSetting, NullDisplay, State);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	public bool UseLocalSetting
	{
		get
		{
			if (bits != null)
			{
				return bits.useLocalSetting;
			}
			return true;
		}
		set
		{
			if (!value || bits != null)
			{
				if (bits != null)
				{
					bits.useLocalSetting = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, DecimalSeparator, DecimalDigits, GroupSeparator, GroupSize, UseGroupSeparator, value, NullDisplay, State);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiTextFormatState.None)]
	public StiTextFormatState State
	{
		get
		{
			if (bits != null)
			{
				return bits.states;
			}
			return StiTextFormatState.None;
		}
		set
		{
			if (value != 0 || bits != null)
			{
				if (bits != null)
				{
					bits.states = value;
				}
				else
				{
					bits = new bitsNumberFormatService(NegativePattern, DecimalSeparator, DecimalDigits, GroupSeparator, GroupSize, UseGroupSeparator, UseLocalSetting, NullDisplay, value);
				}
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "StringFormat");
		JsonExtensions.AddPropertyIntNoDefaultValue(val, "NegativePattern", NegativePattern);
		JsonExtensions.AddPropertyString(val, "DecimalSeparator", DecimalSeparator, ".");
		JsonExtensions.AddPropertyInt(val, "DecimalDigits", DecimalDigits, 2);
		JsonExtensions.AddPropertyString(val, "GroupSeparator", GroupSeparator);
		JsonExtensions.AddPropertyInt(val, "GroupSize", GroupSize, 3);
		JsonExtensions.AddPropertyBool(val, "UseGroupSeparator", UseGroupSeparator, true, false);
		JsonExtensions.AddPropertyBool(val, "UseLocalSetting", UseLocalSetting, true, false);
		JsonExtensions.AddPropertyString(val, "NullDisplay", NullDisplay, " ");
		JsonExtensions.AddPropertyEnum(val, "State", (Enum)State, (Enum)StiTextFormatState.None);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "NegativePattern":
				NegativePattern = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "DecimalSeparator":
				DecimalSeparator = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DecimalDigits":
				DecimalDigits = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "GroupSeparator":
				GroupSeparator = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "GroupSize":
				GroupSize = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "UseGroupSeparator":
				UseGroupSeparator = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "UseLocalSetting":
				UseLocalSetting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "NullDisplay":
				NullDisplay = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "State":
				State = StiJsonReportObjectHelper.DeserializeEnum<StiTextFormatState>(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		StiNumberFormatService stiNumberFormatService = ((StiService)this).Clone() as StiNumberFormatService;
		if (bits != null)
		{
			stiNumberFormatService.bits = bits.Clone() as bitsNumberFormatService;
		}
		return stiNumberFormatService;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StiNumberFormatService stiNumberFormatService))
		{
			return false;
		}
		if (NullDisplay != stiNumberFormatService.NullDisplay)
		{
			return false;
		}
		if (NegativePattern != stiNumberFormatService.NegativePattern)
		{
			return false;
		}
		if (DecimalDigits != stiNumberFormatService.DecimalDigits)
		{
			return false;
		}
		if (DecimalSeparator != stiNumberFormatService.DecimalSeparator)
		{
			return false;
		}
		if (GroupSeparator != stiNumberFormatService.GroupSeparator)
		{
			return false;
		}
		if (GroupSize != stiNumberFormatService.GroupSize)
		{
			return false;
		}
		if (UseGroupSeparator != stiNumberFormatService.UseGroupSeparator)
		{
			return false;
		}
		if (UseLocalSetting != stiNumberFormatService.UseLocalSetting)
		{
			return false;
		}
		if (StringFormat != stiNumberFormatService.StringFormat)
		{
			return false;
		}
		if (State != stiNumberFormatService.State)
		{
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return ((object)this).GetHashCode();
	}

	internal static void FillLocalSetting(NumberFormatInfo format)
	{
		format.NumberDecimalDigits = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
		format.NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		format.NumberGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
		format.NumberGroupSizes = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSizes;
		format.NumberNegativePattern = CultureInfo.CurrentCulture.NumberFormat.NumberNegativePattern;
		format.CurrencyDecimalDigits = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits;
		format.CurrencyDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
		format.CurrencyGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
		format.CurrencyGroupSizes = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSizes;
		format.CurrencyNegativePattern = CultureInfo.CurrentCulture.NumberFormat.CurrencyNegativePattern;
		format.CurrencyPositivePattern = CultureInfo.CurrentCulture.NumberFormat.CurrencyPositivePattern;
		format.CurrencySymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
		format.PercentDecimalDigits = CultureInfo.CurrentCulture.NumberFormat.PercentDecimalDigits;
		format.PercentDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.PercentDecimalSeparator;
		format.PercentGroupSeparator = CultureInfo.CurrentCulture.NumberFormat.PercentGroupSeparator;
		format.PercentGroupSizes = CultureInfo.CurrentCulture.NumberFormat.PercentGroupSizes;
		format.PercentNegativePattern = CultureInfo.CurrentCulture.NumberFormat.PercentNegativePattern;
		format.PercentPositivePattern = CultureInfo.CurrentCulture.NumberFormat.PercentPositivePattern;
		format.PercentSymbol = CultureInfo.CurrentCulture.NumberFormat.PercentSymbol;
		format.PerMilleSymbol = CultureInfo.CurrentCulture.NumberFormat.PerMilleSymbol;
		format.PositiveInfinitySymbol = CultureInfo.CurrentCulture.NumberFormat.PositiveInfinitySymbol;
		format.PositiveSign = CultureInfo.CurrentCulture.NumberFormat.PositiveSign;
		format.NaNSymbol = CultureInfo.CurrentCulture.NumberFormat.NaNSymbol;
		format.NegativeInfinitySymbol = CultureInfo.CurrentCulture.NumberFormat.NegativeInfinitySymbol;
		format.NegativeSign = CultureInfo.CurrentCulture.NumberFormat.NegativeSign;
	}

	public override string Format(string stringFormat, object arg)
	{
		if (arg == null || arg is DBNull)
		{
			return NullDisplay;
		}
		NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
		if (UseLocalSetting)
		{
			FillLocalSetting(numberFormatInfo);
			if ((State & StiTextFormatState.DecimalDigits) > StiTextFormatState.None)
			{
				numberFormatInfo.NumberDecimalDigits = DecimalDigits;
			}
			if ((State & StiTextFormatState.DecimalSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(DecimalSeparator))
			{
				numberFormatInfo.NumberDecimalSeparator = DecimalSeparator;
			}
			if ((State & StiTextFormatState.GroupSeparator) > StiTextFormatState.None && !string.IsNullOrEmpty(GroupSeparator))
			{
				numberFormatInfo.NumberGroupSeparator = GroupSeparator;
			}
			if ((State & StiTextFormatState.GroupSize) > StiTextFormatState.None)
			{
				numberFormatInfo.NumberGroupSizes = new int[1] { GroupSize };
			}
			if ((State & StiTextFormatState.NegativePattern) > StiTextFormatState.None)
			{
				numberFormatInfo.NumberNegativePattern = NegativePattern;
			}
		}
		else
		{
			if (string.IsNullOrEmpty(DecimalSeparator))
			{
				numberFormatInfo.NumberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			}
			else
			{
				numberFormatInfo.NumberDecimalSeparator = DecimalSeparator;
			}
			numberFormatInfo.NumberDecimalDigits = DecimalDigits;
			if (GroupSeparator != null)
			{
				numberFormatInfo.NumberGroupSeparator = GroupSeparator;
			}
			numberFormatInfo.NumberGroupSizes = new int[1] { GroupSize };
			numberFormatInfo.NumberNegativePattern = NegativePattern;
		}
		if (!UseGroupSeparator)
		{
			numberFormatInfo.NumberGroupSeparator = string.Empty;
		}
		if (StiOptions.Engine.AllowConvertingInFormatting && arg?.GetType() != null && !TypeExt.IsNumericType(arg.GetType()))
		{
			decimal? num = StiValueHelper.TryToNullableDecimal(arg);
			if (num.HasValue)
			{
				arg = num;
			}
		}
		if ((State & StiTextFormatState.Abbreviation) > StiTextFormatState.None)
		{
			decimal? num2 = StiValueHelper.TryToNullableDecimal(arg);
			if (num2.HasValue)
			{
				int decimalDigits = (((State & StiTextFormatState.DecimalDigits) > StiTextFormatState.None) ? DecimalDigits : 0);
				num2 = StiAbbreviationNumberFormatHelper.Format(num2.Value, out var postfix, decimalDigits, TotalNumberCapacity);
				return $"{num2}{postfix}";
			}
		}
		return string.Format(numberFormatInfo, "{0:N}", new object[1] { arg });
	}

	public override StiFormatService CreateNew()
	{
		return new StiNumberFormatService();
	}

	public StiNumberFormatService()
		: this(CultureInfo.CurrentCulture.NumberFormat.NumberNegativePattern, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSizes[0], useGroupSeparator: true, useLocalSetting: true, " ")
	{
	}

	public StiNumberFormatService(int negativePattern, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, bool useGroupSeparator, bool useLocalSetting, string nullDisplay)
		: this(negativePattern, 0, decimalSeparator, decimalDigits, groupSeparator, groupSize, useGroupSeparator, useLocalSetting, nullDisplay)
	{
	}

	public StiNumberFormatService(int negativePattern, int decimalPlaces, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, bool useGroupSeparator, bool useLocalSetting, string nullDisplay)
		: this(negativePattern, decimalPlaces, decimalSeparator, decimalDigits, groupSeparator, groupSize, useGroupSeparator, useLocalSetting, nullDisplay, StiTextFormatState.None)
	{
	}

	public StiNumberFormatService(int negativePattern, int decimalPlaces, string decimalSeparator, int decimalDigits, string groupSeparator, int groupSize, bool useGroupSeparator, bool useLocalSetting, string nullDisplay, StiTextFormatState state)
	{
		DecimalDigits = decimalDigits;
		DecimalSeparator = decimalSeparator;
		GroupSeparator = groupSeparator;
		GroupSize = groupSize;
		NegativePattern = negativePattern;
		UseGroupSeparator = useGroupSeparator;
		UseLocalSetting = useLocalSetting;
		NullDisplay = nullDisplay;
		State = state;
	}
}
