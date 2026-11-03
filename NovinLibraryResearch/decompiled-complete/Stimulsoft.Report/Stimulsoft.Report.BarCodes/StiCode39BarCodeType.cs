using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiCode39BarCodeTypeConverter))]
public class StiCode39BarCodeType : StiBarCodeTypeService
{
	protected string Code39Symbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";

	protected int Code39StartStopSymbolIndex = 43;

	protected string[] Code39Table = new string[44]
	{
		"1112212111", "2112111121", "1122111121", "2122111111", "1112211121", "2112211111", "1122211111", "1112112121", "2112112111", "1122112111",
		"2111121121", "1121121121", "2121121111", "1111221121", "2111221111", "1121221111", "1111122121", "2111122111", "1121122111", "1111222111",
		"2111111221", "1121111221", "2121111211", "1111211221", "2111211211", "1121211211", "1111112221", "2111112211", "1121112211", "1111212211",
		"2211111121", "1221111121", "2221111111", "1211211121", "2211211111", "1221211111", "1211112121", "2211112111", "1221112111", "1212121111",
		"1212111211", "1211121211", "1112121211", "1211212111"
	};

	private float module = 13f;

	private float height = 1f;

	private float ratio = 2.2f;

	protected const float Code39SpaceLeft = 10f;

	protected const float Code39SpaceRight = 10f;

	protected const float Code39SpaceTop = 0f;

	protected const float Code39SpaceBottom = 1f;

	protected const float Code39LineHeightShort = 45f;

	protected const float Code39LineHeightLong = 45f;

	protected const float Code39TextPosition = 46f;

	protected const float Code39TextHeight = 8.33f;

	protected const float Code39MainHeight = 55f;

	protected const float Code39LineHeightForCut = 45f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode39BarCodeType;

	public override string ServiceName => "Code39";

	public override string DefaultCodeValue => "ABC123";

	[Description("Gets or sets a mode of the checksum.")]
	[DefaultValue(StiCheckSum.Yes)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiCheckSum CheckSum { get; set; }

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
			if (value < 2f)
			{
				module = 2f;
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

	[DefaultValue(2.2f)]
	[StiSerializable]
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
			float num = ((Module > 20f) ? 2f : 2.2f);
			if (value < num)
			{
				ratio = num;
			}
			if (value > 3f)
			{
				ratio = 3f;
			}
		}
	}

	internal override float LabelFontHeight => 8.33f;

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, false,
		false, true, false, true, false, true, false, false, false, false,
		false, false, false, true, false, false, false
	};

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 13f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyEnum(val, "CheckSum", (Enum)CheckSum, (Enum)StiCheckSum.Yes);
		JsonExtensions.AddPropertyFloat(val, "Ratio", Ratio, 2.2f);
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
			case "CheckSum":
				CheckSum = StiJsonReportObjectHelper.DeserializeEnum<StiCheckSum>(item);
				break;
			case "Ratio":
				ratio = StiJsonReportObjectHelper.DeserializeFloat(item);
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
			propertiesHelper.CheckSum(),
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.fRatio()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	protected string CodeToBar(string inputCode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		for (int i = 0; i < inputCode.Length; i++)
		{
			char value = ((!flag) ? ((inputCode[i] != '1') ? '1' : '0') : ((inputCode[i] != '1') ? '5' : '4'));
			stringBuilder.Append(value);
			flag = !flag;
		}
		return stringBuilder.ToString();
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, Code39Symbols);
		int[] array = new int[code.Length + ((CheckSum == StiCheckSum.Yes) ? 3 : 2)];
		array[0] = Code39StartStopSymbolIndex;
		int num = 0;
		for (int i = 0; i < code.Length; i++)
		{
			array[i + 1] = Code39Symbols.IndexOf(code[i]);
			num += array[i + 1];
		}
		if (CheckSum == StiCheckSum.Yes)
		{
			array[^2] = num % 43;
		}
		array[^1] = array[0];
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < array.Length; j++)
		{
			stringBuilder.Append(CodeToBar(Code39Table[array[j]]));
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, Ratio, zoom, code, code, stringBuilder.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode39BarCodeType();
	}

	public StiCode39BarCodeType()
		: this(13f, 1f, 2.2f, StiCheckSum.Yes)
	{
	}

	public StiCode39BarCodeType(float module, float height, float ratio, StiCheckSum checkSum)
	{
		this.module = module;
		this.height = height;
		this.ratio = ratio;
		CheckSum = checkSum;
	}
}
