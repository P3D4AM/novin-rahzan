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

[TypeConverter(typeof(StiStandard2of5BarCodeTypeConverter))]
public class StiStandard2of5BarCodeType : StiBarCodeTypeService
{
	private float module = 13f;

	private float height = 1f;

	private float ratio = 2.2f;

	protected string[] symTableSet = new string[12]
	{
		"00110", "10001", "01001", "11000", "00101", "10100", "01100", "00011", "10010", "01010",
		"101000", "10001"
	};

	private const float Standard2of5SpaceLeft = 10f;

	private const float Standard2of5SpaceRight = 10f;

	private const float Standard2of5SpaceTop = 0f;

	private const float Standard2of5SpaceBottom = 1f;

	private const float Standard2of5LineHeightShort = 40f;

	private const float Standard2of5LineHeightLong = 40f;

	private const float Standard2of5TextHeight = 8.33f;

	private const float Standard2of5MainHeight = 50f;

	private const float Standard2of5TextPosition = 41f;

	private const float Standard2of5LineHeightForCut = 40f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiStandard2of5BarCodeType;

	public override string ServiceName => "2of5 Standard";

	public override string DefaultCodeValue => "12345678";

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
			if (value < 7.5f)
			{
				module = 7.5f;
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

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[11] = true;
			array[13] = true;
			array[15] = true;
			return array;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 13f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
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
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.fRatio()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, "0123456789");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(symTableSet[10]);
		if (code.Length > 0)
		{
			for (int i = 0; i < code.Length; i++)
			{
				string text = symTableSet[int.Parse(code[i].ToString())];
				StringBuilder stringBuilder2 = new StringBuilder("0000000000");
				stringBuilder2[0] = text[0];
				stringBuilder2[2] = text[1];
				stringBuilder2[4] = text[2];
				stringBuilder2[6] = text[3];
				stringBuilder2[8] = text[4];
				stringBuilder.Append((object?)stringBuilder2);
			}
		}
		stringBuilder.Append(symTableSet[11]);
		bool flag = true;
		for (int j = 0; j < stringBuilder.Length; j++)
		{
			char value = ((!flag) ? ((stringBuilder[j] != '0') ? '1' : '0') : ((stringBuilder[j] != '0') ? '5' : '4'));
			stringBuilder[j] = value;
			flag = !flag;
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 40f, 40f, 41f, 8.33f, 50f, 40f, Ratio, zoom, code, code, stringBuilder.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiStandard2of5BarCodeType();
	}

	public StiStandard2of5BarCodeType()
		: this(13f, 1f, 2.2f)
	{
	}

	public StiStandard2of5BarCodeType(float module, float height, float ratio)
	{
		this.module = module;
		this.height = height;
		this.ratio = ratio;
	}
}
