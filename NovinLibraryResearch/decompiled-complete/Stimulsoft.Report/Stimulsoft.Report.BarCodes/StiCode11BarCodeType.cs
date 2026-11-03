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

[TypeConverter(typeof(StiCode11BarCodeTypeConverter))]
public class StiCode11BarCodeType : StiBarCodeTypeService
{
	protected string Code11Symbols = "0123456789-";

	protected int Code11StartStopSymbolIndex = 11;

	protected string[] Code11Table = new string[12]
	{
		"40405", "50405", "41405", "51404", "40505", "50504", "41504", "40415", "50414", "50404",
		"40504", "40514"
	};

	private float module = 8f;

	private float height = 1f;

	protected const float Code11SpaceLeft = 5f;

	protected const float Code11SpaceRight = 5f;

	protected const float Code11SpaceTop = 0f;

	protected const float Code11SpaceBottom = 1f;

	protected const float Code11LineHeightShort = 40f;

	protected const float Code11LineHeightLong = 40f;

	protected const float Code11TextPosition = 41f;

	protected const float Code11TextHeight = 8f;

	protected const float Code11MainHeight = 50f;

	protected const float Code11LineHeightForCut = 40f;

	protected const float defaultCode11Module = 8f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode11BarCodeType;

	public override string ServiceName => "Code11";

	public override string DefaultCodeValue => "A12345678B";

	[Description("Gets or sets width of the most fine element of the bar code.")]
	[DefaultValue(8f)]
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
			if (value < 4f)
			{
				module = 4f;
			}
			if (value > 40f)
			{
				module = 40f;
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
			if (value < 0.2f)
			{
				height = 0.2f;
			}
			if (value > 2f)
			{
				height = 2f;
			}
		}
	}

	[Description("Gets or sets mode of checksum.")]
	[DefaultValue(StiCode11CheckSum.Auto)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiCode11CheckSum Checksum { get; set; } = StiCode11CheckSum.Auto;


	internal override float LabelFontHeight => 8f;

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[3] = true;
			array[11] = true;
			array[13] = true;
			return array;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 8f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyEnum(val, "Checksum", (Enum)Checksum, (Enum)StiCode11CheckSum.Auto);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Checksum":
				Checksum = StiJsonReportObjectHelper.DeserializeEnum<StiCode11CheckSum>(item);
				break;
			case "Module":
				module = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Height":
				height = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.BarCodeChecksum(),
			propertiesHelper.fHeight(),
			propertiesHelper.Module()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, Code11Symbols);
		int num = 0;
		if (Checksum == StiCode11CheckSum.Auto)
		{
			num = ((code.Length < 10) ? 1 : 2);
		}
		if (Checksum == StiCode11CheckSum.OneDigit)
		{
			num = 1;
		}
		if (Checksum == StiCode11CheckSum.TwoDigits)
		{
			num = 2;
		}
		int[] array = new int[code.Length + 2 + num];
		array[0] = Code11StartStopSymbolIndex;
		for (int i = 0; i < code.Length; i++)
		{
			array[i + 1] = Code11Symbols.IndexOf(code[i]);
		}
		int num2 = 0;
		int num3 = 1;
		for (int num4 = code.Length; num4 > 0; num4--)
		{
			num2 += array[num4] * num3;
			num3++;
			if (num3 > 10)
			{
				num3 = 1;
			}
		}
		array[code.Length + 1] = num2 % 11;
		if (num == 2)
		{
			num2 = 0;
			num3 = 1;
			for (int num5 = code.Length + 1; num5 > 0; num5--)
			{
				num2 += array[num5] * num3;
				num3++;
				if (num3 > 9)
				{
					num3 = 1;
				}
			}
			array[code.Length + 2] = num2 % 9;
		}
		array[^1] = array[0];
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < array.Length; j++)
		{
			stringBuilder.Append(Code11Table[array[j]]);
			if (j != array.Length - 1)
			{
				stringBuilder.Append("0");
			}
		}
		CalculateSizeFull(5f, 5f, 0f, 1f, 40f, 40f, 41f, 8f, 50f, 40f, 2f, zoom, code, code, stringBuilder.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode11BarCodeType();
	}

	public StiCode11BarCodeType()
		: this(8f, 1f, StiCode11CheckSum.Auto)
	{
	}

	public StiCode11BarCodeType(float module, float height, StiCode11CheckSum checksum)
	{
		this.module = module;
		this.height = height;
		Checksum = checksum;
	}
}
