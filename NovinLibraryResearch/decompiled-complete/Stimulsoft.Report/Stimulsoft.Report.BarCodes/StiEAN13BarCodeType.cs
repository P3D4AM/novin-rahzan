using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiEAN13BarCodeTypeConverter))]
public class StiEAN13BarCodeType : StiBarCodeTypeService
{
	public class EanBarInfo
	{
		public Ean13Symbol SymbolType;

		public char SymbolText;

		public bool TextAtTop;

		public bool MakeLonger;

		public EanBarInfo(Ean13Symbol symbolType, char symbolText, bool textAtTop)
			: this(symbolType, symbolText, textAtTop, makeLonger: false)
		{
		}

		public EanBarInfo(Ean13Symbol symbolType, char symbolText, bool textAtTop, bool makeLonger)
		{
			SymbolType = symbolType;
			SymbolText = symbolText;
			TextAtTop = textAtTop;
			MakeLonger = makeLonger;
		}
	}

	public enum Ean13Symbol
	{
		ComboA0,
		ComboA1,
		ComboA2,
		ComboA3,
		ComboA4,
		ComboA5,
		ComboA6,
		ComboA7,
		ComboA8,
		ComboA9,
		ComboB0,
		ComboB1,
		ComboB2,
		ComboB3,
		ComboB4,
		ComboB5,
		ComboB6,
		ComboB7,
		ComboB8,
		ComboB9,
		ComboC0,
		ComboC1,
		ComboC2,
		ComboC3,
		ComboC4,
		ComboC5,
		ComboC6,
		ComboC7,
		ComboC8,
		ComboC9,
		GuardLeft,
		GuardCenter,
		GuardRight,
		GuardSpecial,
		GuardAddLeft,
		GuardAddDelineator,
		SpaceLeft,
		SpaceRight,
		SpaceBeforeAdd
	}

	private float module = 13f;

	private float height = 1f;

	protected string[] symComboSet = new string[10] { "000000", "001011", "001101", "001110", "010011", "011001", "011100", "010101", "010110", "011010" };

	protected string[] symParitySetAdd2 = new string[4] { "oo", "oe", "eo", "ee" };

	protected string[] symParitySetAdd5 = new string[10] { "eeooo", "eoeoo", "eooeo", "eoooe", "oeeoo", "ooeeo", "oooee", "oeoeo", "oeooe", "ooeoe" };

	protected string[] ean13SymData = new string[39]
	{
		"2504", "1514", "1415", "0704", "0425", "0524", "0407", "0605", "0506", "2405",
		"0416", "0515", "1505", "0434", "1604", "0614", "3404", "1424", "2414", "1406",
		"6140", "5150", "5051", "4340", "4061", "4160", "4043", "4241", "4142", "6041",
		"808", "08080", "808", "080808", "809", "08", "x", "y", "z"
	};

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiEAN13BarCodeType;

	public override string ServiceName => "EAN-13";

	public override string DefaultCodeValue => "0123456789012";

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, false,
		false, true, false, true, false, false, false, true, false, true,
		true, false, false, false, false, false, false
	};

	[Description("Gets or sets width of the most fine element of the bar code.")]
	[DefaultValue(13f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override float Module
	{
		get
		{
			return module;
		}
		set
		{
			module = value;
			if (value < 10.4f)
			{
				module = 10.4f;
			}
			if (value > 26f)
			{
				module = 26f;
			}
		}
	}

	[Description("Gets os sets height factor of the bar code.")]
	[DefaultValue(1f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
			if (value < 0.5f)
			{
				height = 0.5f;
			}
			if (value > 1f)
			{
				height = 1f;
			}
		}
	}

	[DefaultValue(StiEanSupplementType.None)]
	[StiSerializable]
	[Description("Gets or sets type of supplement code.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual StiEanSupplementType SupplementType { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	[StiSerializable]
	[Description("Gets or sets the component supplement bar code.")]
	public virtual string SupplementCode { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[Description("Gets or sets value which indicates will show Quiet Zone Indicator or no.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowQuietZoneIndicator { get; set; } = true;


	internal override float LabelFontHeight => EanTextHeight;

	protected virtual float EanSpaceLeft => 11f;

	protected virtual float EanSpaceRight => 8f;

	protected virtual float EanSpaceTop => 0f;

	protected virtual float EanSpaceBottom => 1f;

	protected virtual float EanSpaceBeforeAdd => 10f;

	protected virtual float EanSpaceTextTop => 10f;

	protected virtual float EanLineHeightShort => 69.2f;

	protected virtual float EanLineHeightLong => EanLineHeightShort + 5f;

	protected virtual float EanTextPositionTop => 0.5f;

	protected virtual float EanTextPositionBottom => EanLineHeightShort + EanSpaceBottom;

	protected virtual float EanTextHeight => 8.33f;

	protected virtual float EanMainHeight => 78.58f;

	protected virtual float EanLineHeightForCut => EanLineHeightShort;

	protected virtual float EanWideToNarrowRatio => 2f;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 13f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyEnum(val, "SupplementType", (Enum)SupplementType, (Enum)StiEanSupplementType.None);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SupplementCode", SupplementCode);
		JsonExtensions.AddPropertyBool(val, "ShowQuietZoneIndicator", ShowQuietZoneIndicator, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Module":
				module = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Height":
				height = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "SupplementType":
				SupplementType = StiJsonReportObjectHelper.DeserializeEnum<StiEanSupplementType>(item);
				break;
			case "SupplementCode":
				SupplementCode = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ShowQuietZoneIndicator":
				ShowQuietZoneIndicator = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.SupplementCode(),
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.ShowQuietZoneIndicator(),
			propertiesHelper.SupplementType()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	protected void CalculateSizeEan(float offsetY, float Zoom, List<EanBarInfo> BarsArray, RectangleF rect, StiBarCode barCode)
	{
		base.BarCodeData.WideToNarrowRatio = 2f;
		base.BarCodeData.EanBarsArray = BarsArray;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (EanBarInfo item in BarsArray)
		{
			stringBuilder.Append(ean13SymData[(int)item.SymbolType]);
		}
		float symbolsStringWidth = GetSymbolsStringWidth(stringBuilder.ToString());
		RectWindow = new RectangleF(0f, 0f, rect.Width, rect.Height);
		if (barCode.Angle == StiAngle.Angle90 || barCode.Angle == StiAngle.Angle270)
		{
			RectWindow = new RectangleF(0f, 0f, rect.Height, rect.Width);
		}
		float num = barCode.Font.Size - 8f + 0.5f;
		float num2 = EanMainHeight + num;
		float num3 = EanTextHeight + num;
		float num4 = Module / 10f * Zoom;
		float num5 = num4;
		float num6 = EanLineHeightForCut * (1f - Height);
		float num7 = EanSpaceTextTop + num;
		if (barCode.AutoScale)
		{
			num4 = RectWindow.Width / symbolsStringWidth;
			num6 = 0f - (RectWindow.Height / num5 - (num2 + offsetY));
			if (!barCode.ShowLabelText)
			{
				num6 -= EanTextHeight;
				num7 -= EanTextHeight;
			}
		}
		base.BarCodeData.FullZoomY = num5;
		base.BarCodeData.SpaceLeft = EanSpaceLeft * num4;
		base.BarCodeData.SpaceRight = EanSpaceRight * num4;
		base.BarCodeData.SpaceBeforeAdd = EanSpaceBeforeAdd * num4;
		base.BarCodeData.LineWidth = 1f * num4;
		base.BarCodeData.MainWidth = symbolsStringWidth * num4;
		base.BarCodeData.SpaceTop = EanSpaceTop * num5;
		base.BarCodeData.SpaceBottom = EanSpaceBottom * num5;
		base.BarCodeData.SpaceTextTop = num7 * num5;
		base.BarCodeData.LineHeightShort = (EanLineHeightShort - num6) * num5;
		base.BarCodeData.LineHeightLong = (EanLineHeightLong - num6) * num5;
		base.BarCodeData.TextPositionTop = EanTextPositionTop * num5;
		base.BarCodeData.TextPositionBottom = (EanTextPositionBottom - num6) * num5;
		base.BarCodeData.MainHeight = (num2 + offsetY - num6) * num5;
		base.BarCodeData.TextHeight = num3 * num5;
		base.BarCodeData.OffsetY = offsetY * num5;
	}

	protected void DrawEanBars(object context, List<EanBarInfo> barsArray, StiBarCode barCode)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		StiSolidBrush brush = new StiSolidBrush(barCode.BackColor);
		StiSolidBrush brush2 = new StiSolidBrush(barCode.ForeColor);
		BaseFillRectangle(context, (StiBrush)(object)brush, 0f, 0f, base.BarCodeData.MainWidth, base.BarCodeData.MainHeight);
		float num = 0f;
		float num2 = base.BarCodeData.OffsetY + base.BarCodeData.SpaceTop;
		bool flag = context is Graphics;
		float num3 = (flag ? 1f : 0.75000006f);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = new Font(StiFontCollection.GetFontFamily(barCode.Font.Name, false), barCode.Font.Size * (barCode.BarCodeType.LabelFontHeight / 8f) * base.BarCodeData.FullZoomY * num3, barCode.Font.Style, (GraphicsUnit)(flag ? 2 : 3));
			try
			{
				val.Alignment = (StringAlignment)1;
				val.LineAlignment = (StringAlignment)0;
				foreach (EanBarInfo item in barsArray)
				{
					float num4 = 0f;
					string text = ean13SymData[(int)item.SymbolType];
					if (item.MakeLonger)
					{
						text = makeLonger(text);
					}
					foreach (char symbol in text)
					{
						if (!IsSymbolSpace(symbol))
						{
							float num5 = base.BarCodeData.LineHeightShort;
							if (IsSymbolLong(symbol) || item.TextAtTop)
							{
								num5 = base.BarCodeData.LineHeightLong;
							}
							float num6 = 0f;
							if (item.TextAtTop)
							{
								num6 = base.BarCodeData.SpaceTextTop;
							}
							BaseFillRectangle(context, (StiBrush)(object)brush2, num + base.BarCodeData.LineWidth * num4, num2 + num6, base.BarCodeData.LineWidth * GetSymbolWidth(symbol), num5 - num6);
						}
						num4 += GetSymbolWidth(symbol);
					}
					if (barCode.ShowLabelText && item.SymbolText != ' ')
					{
						BaseDrawString(rect: (!item.TextAtTop) ? new RectangleF(num, num2 + base.BarCodeData.TextPositionBottom, num4 * base.BarCodeData.LineWidth, base.BarCodeData.SpaceTextTop) : new RectangleF(num, num2 + base.BarCodeData.TextPositionTop, num4 * base.BarCodeData.LineWidth, base.BarCodeData.SpaceTextTop), context: context, st: item.SymbolText.ToString(), font: val2, brush: (StiBrush)(object)brush2, sf: val);
					}
					num += base.BarCodeData.LineWidth * num4;
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected List<EanBarInfo> MakeEan13Bars(ref string code, bool isLast)
	{
		int[] array = new int[12];
		for (int i = 0; i < 12; i++)
		{
			array[i] = int.Parse(code[i].ToString());
		}
		int num = (array[1] + array[3] + array[5] + array[7] + array[9] + array[11]) * 3 + array[0] + array[2] + array[4] + array[6] + array[8] + array[10];
		int num2 = 10 - num % 10;
		if (num2 == 10)
		{
			num2 = 0;
		}
		code = code.Substring(0, 12) + (char)(num2 + 48);
		List<EanBarInfo> list = new List<EanBarInfo>
		{
			new EanBarInfo(Ean13Symbol.SpaceLeft, code[0], textAtTop: false),
			new EanBarInfo(Ean13Symbol.GuardLeft, ' ', textAtTop: false)
		};
		int num3 = int.Parse(code[0].ToString());
		for (int j = 0; j < 6; j++)
		{
			int num4 = int.Parse(code[1 + j].ToString());
			EanBarInfo eanBarInfo = new EanBarInfo((Ean13Symbol)num4, code[1 + j], textAtTop: false);
			if (symComboSet[num3][j] != '0')
			{
				eanBarInfo.SymbolType = (Ean13Symbol)(10 + num4);
			}
			list.Add(eanBarInfo);
		}
		list.Add(new EanBarInfo(Ean13Symbol.GuardCenter, ' ', textAtTop: false));
		for (int k = 0; k < 6; k++)
		{
			int num5 = int.Parse(code[7 + k].ToString());
			EanBarInfo item = new EanBarInfo((Ean13Symbol)(20 + num5), code[7 + k], textAtTop: false);
			list.Add(item);
		}
		list.Add(new EanBarInfo(Ean13Symbol.GuardRight, ' ', textAtTop: false));
		if (isLast)
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceRight, ShowQuietZoneIndicator ? '>' : ' ', textAtTop: false));
		}
		else
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceBeforeAdd, ' ', textAtTop: false));
		}
		return list;
	}

	protected List<EanBarInfo> MakeEanAdd2Bars(string code, List<EanBarInfo> baseArray, bool isLast)
	{
		List<EanBarInfo> list = baseArray ?? new List<EanBarInfo>();
		list.Add(new EanBarInfo(Ean13Symbol.GuardAddLeft, ' ', textAtTop: true));
		int num = 2;
		string text = symParitySetAdd2[int.Parse(code.Substring(0, 2)) % 4];
		for (int i = 0; i < num; i++)
		{
			int num2 = int.Parse(code[i].ToString());
			char c = text[i];
			EanBarInfo eanBarInfo = new EanBarInfo((Ean13Symbol)num2, code[i], textAtTop: true);
			if (c != 'o')
			{
				eanBarInfo.SymbolType = (Ean13Symbol)(10 + num2);
			}
			list.Add(eanBarInfo);
			if (i < num - 1)
			{
				list.Add(new EanBarInfo(Ean13Symbol.GuardAddDelineator, ' ', textAtTop: true));
			}
		}
		if (isLast)
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceRight, ShowQuietZoneIndicator ? '>' : ' ', textAtTop: true));
		}
		else
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceBeforeAdd, ' ', textAtTop: true));
		}
		return list;
	}

	protected List<EanBarInfo> MakeEanAdd5Bars(string code, List<EanBarInfo> baseArray, bool isLast)
	{
		List<EanBarInfo> list = baseArray ?? new List<EanBarInfo>();
		list.Add(new EanBarInfo(Ean13Symbol.GuardAddLeft, ' ', textAtTop: true));
		int num = int.Parse(code[0].ToString()) * 3 + int.Parse(code[1].ToString()) * 9 + int.Parse(code[2].ToString()) * 3 + int.Parse(code[3].ToString()) * 9 + int.Parse(code[4].ToString()) * 3;
		string text = symParitySetAdd5[num % 10];
		int num2 = 5;
		for (int i = 0; i < num2; i++)
		{
			int num3 = int.Parse(code[i].ToString());
			char c = text[i];
			EanBarInfo eanBarInfo = new EanBarInfo((Ean13Symbol)num3, code[i], textAtTop: true);
			if (c != 'o')
			{
				eanBarInfo.SymbolType = (Ean13Symbol)(10 + num3);
			}
			list.Add(eanBarInfo);
			if (i < num2 - 1)
			{
				list.Add(new EanBarInfo(Ean13Symbol.GuardAddDelineator, ' ', textAtTop: true));
			}
		}
		if (isLast)
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceRight, ShowQuietZoneIndicator ? '>' : ' ', textAtTop: true));
		}
		else
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceBeforeAdd, ' ', textAtTop: true));
		}
		return list;
	}

	protected string makeLonger(string symString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < symString.Length; i++)
		{
			char c = symString[i];
			switch (c)
			{
			case '4':
				c = '8';
				break;
			case '5':
				c = '9';
				break;
			case '6':
				c = 'a';
				break;
			case '7':
				c = 'b';
				break;
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	protected override float GetSymbolWidth(char symbol)
	{
		switch (symbol)
		{
		case '0':
		case '4':
		case '8':
		case 'c':
		case 'd':
		case 'e':
		case 'f':
			return 1f;
		case '1':
		case '5':
		case '9':
			return 1f * EanWideToNarrowRatio;
		case '2':
		case '6':
		case 'a':
			return 1.5f * EanWideToNarrowRatio;
		case '3':
		case '7':
		case 'b':
			return 2f * EanWideToNarrowRatio;
		case 'x':
			return EanSpaceLeft;
		case 'y':
			return EanSpaceRight;
		case 'z':
			return EanSpaceBeforeAdd;
		default:
			return 1f;
		}
	}

	protected override bool IsSymbolSpace(char symbol)
	{
		switch (symbol)
		{
		case '0':
		case '1':
		case '2':
		case '3':
			return true;
		case 'x':
		case 'y':
		case 'z':
			return true;
		default:
			return false;
		}
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, "0123456789") + new string('0', 13);
		string code2 = CheckCodeSymbols(SupplementCode, "0123456789") + new string('0', 5);
		List<EanBarInfo> list;
		if (SupplementType == StiEanSupplementType.None)
		{
			list = MakeEan13Bars(ref code, isLast: true);
		}
		else
		{
			list = MakeEan13Bars(ref code, isLast: false);
			if (SupplementType == StiEanSupplementType.TwoDigit)
			{
				MakeEanAdd2Bars(code2, list, isLast: true);
			}
			else
			{
				MakeEanAdd5Bars(code2, list, isLast: true);
			}
		}
		CalculateSizeEan(0f, zoom, list, rect, barCode);
		TranslateRect(context, rect, barCode);
		DrawEanBars(context, list, barCode);
		RollbackTransform(context);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiEAN13BarCodeType();
	}

	public StiEAN13BarCodeType()
		: this(13f, 1f, StiEanSupplementType.None, null, showQuietZoneIndicator: true)
	{
	}

	public StiEAN13BarCodeType(float module, float height, StiEanSupplementType supplementType, string supplementCodeValue, bool showQuietZoneIndicator)
	{
		this.module = module;
		this.height = height;
		SupplementType = supplementType;
		SupplementCode = supplementCodeValue;
		ShowQuietZoneIndicator = showQuietZoneIndicator;
	}
}
