using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiITF14BarCodeTypeConverter))]
public class StiITF14BarCodeType : StiBarCodeTypeService
{
	private float module = 40f;

	private float height = 1f;

	private float ratio = 2.5f;

	protected string[] symTableSet = new string[12]
	{
		"00110", "10001", "01001", "11000", "00101", "10100", "01100", "00011", "10010", "01010",
		"0000", "100"
	};

	private const float Itf14BearerBarWidth = 2f;

	private const float Itf14SpaceLeft = 10f;

	private const float Itf14SpaceRight = 10f;

	private const float Itf14SpaceTop = 2f;

	private const float Itf14SpaceBottom = 3f;

	private const float Itf14LineHeightShort = 31.25f;

	private const float Itf14LineHeightLong = 31.25f;

	private const float Itf14TextHeight = 8.33f;

	private const float Itf14MainHeight = 51.75f;

	private const float Itf14TextPosition = 36.25f;

	private const float Itf14LineHeightForCut = 31.25f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiITF14BarCodeType;

	public override string ServiceName => "ITF-14";

	public override string DefaultCodeValue => "15400141288763";

	[Description("Gets or sets width of the most fine element of the bar code.")]
	[DefaultValue(40f)]
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
			if (value < 10f)
			{
				module = 10f;
			}
			if (value > 40f)
			{
				module = 40f;
			}
			Ratio = Ratio;
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
			if (value > 2f)
			{
				height = 2f;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(2.5f)]
	[Description("Get or sets value, which indicates WideToNarrow ratio.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public float Ratio
	{
		get
		{
			return ratio;
		}
		set
		{
			ratio = value;
			if (value < 2.25f)
			{
				ratio = 2.25f;
			}
			if (value > 3f)
			{
				ratio = 3f;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[Description("Get or sets value, which indicates, print or not vertical sections.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool PrintVerticalBars { get; set; }

	internal override float LabelFontHeight => 8.33f;

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, false,
		false, true, false, true, true, true, false, false, false, false,
		false, false, false, false, false, false, false
	};

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 40f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyFloat(val, "Ratio", Ratio, 2.5f);
		JsonExtensions.AddPropertyBool(val, "PrintVerticalBars", PrintVerticalBars, false, false);
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
			case "Ratio":
				ratio = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "PrintVerticalBars":
				PrintVerticalBars = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.PrintVerticalBars(),
			propertiesHelper.fRatio()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, "0123456789") + new string('0', 14);
		int[] array = new int[14];
		for (int i = 0; i < 14; i++)
		{
			array[i] = int.Parse(code[i].ToString());
		}
		int num = (array[0] + array[2] + array[4] + array[6] + array[8] + array[10] + array[12]) * 3 + array[1] + array[3] + array[5] + array[7] + array[9] + array[11];
		int num2 = 10 - num % 10;
		if (num2 == 10)
		{
			num2 = 0;
		}
		array[13] = num2;
		code = code.Substring(0, 13) + (char)(num2 + 48);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(symTableSet[10]);
		for (int j = 0; j < 7; j++)
		{
			string text = symTableSet[array[j * 2]];
			string text2 = symTableSet[array[j * 2 + 1]];
			StringBuilder stringBuilder2 = new StringBuilder("0000000000");
			stringBuilder2[0] = text[0];
			stringBuilder2[2] = text[1];
			stringBuilder2[4] = text[2];
			stringBuilder2[6] = text[3];
			stringBuilder2[8] = text[4];
			stringBuilder2[1] = text2[0];
			stringBuilder2[3] = text2[1];
			stringBuilder2[5] = text2[2];
			stringBuilder2[7] = text2[3];
			stringBuilder2[9] = text2[4];
			stringBuilder.Append((object?)stringBuilder2);
		}
		stringBuilder.Append(symTableSet[11]);
		bool flag = true;
		for (int k = 0; k < stringBuilder.Length; k++)
		{
			char value = ((!flag) ? ((stringBuilder[k] != '0') ? '1' : '0') : ((stringBuilder[k] != '0') ? '5' : '4'));
			stringBuilder[k] = value;
			flag = !flag;
		}
		CalculateSizeFull(10f + (PrintVerticalBars ? 2f : 0f), 10f + (PrintVerticalBars ? 2f : 0f), 2f, 3f, 31.25f, 31.25f, 36.25f, 8.33f, 51.75f, 31.25f, Ratio, zoom, code, code, stringBuilder.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode, DrawBaseLines);
		StiSolidBrush val = new StiSolidBrush(barCode.ForeColor);
	}

	protected void DrawBaseLines(object context, StiBrush foreBrush)
	{
		BaseFillRectangle(context, foreBrush, 0f, 0f, base.BarCodeData.MainWidth, base.BarCodeData.SpaceTop);
		BaseFillRectangle(context, foreBrush, 0f, base.BarCodeData.SpaceTop + base.BarCodeData.LineHeightShort, base.BarCodeData.MainWidth, base.BarCodeData.SpaceTop);
		if (PrintVerticalBars)
		{
			BaseFillRectangle(context, foreBrush, 0f, 0f, base.BarCodeData.SpaceTop, base.BarCodeData.LineHeightShort + base.BarCodeData.SpaceTop * 2f);
			BaseFillRectangle(context, foreBrush, base.BarCodeData.MainWidth - base.BarCodeData.SpaceTop, 0f, base.BarCodeData.SpaceTop, base.BarCodeData.LineHeightShort + base.BarCodeData.SpaceTop * 2f);
		}
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiITF14BarCodeType();
	}

	public StiITF14BarCodeType()
		: this(40f, 1f, 2.5f, printVerticalBars: false)
	{
	}

	public StiITF14BarCodeType(float module, float height, float ratio, bool printVerticalBars)
	{
		this.module = module;
		this.height = height;
		this.ratio = ratio;
		PrintVerticalBars = printVerticalBars;
	}
}
