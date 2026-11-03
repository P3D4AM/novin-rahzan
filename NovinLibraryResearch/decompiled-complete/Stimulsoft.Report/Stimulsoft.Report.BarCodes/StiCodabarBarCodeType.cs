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

[TypeConverter(typeof(StiCodabarBarCodeTypeConverter))]
public class StiCodabarBarCodeType : StiBarCodeTypeService
{
	private string CodabarSymbols = "0123456789-$:/.+ABCD";

	private string[] CodabarTable = new string[20]
	{
		"11111221", "11112211", "11121121", "22111111", "11211211", "21111211", "12111121", "12112111", "12211111", "21121111",
		"11122111", "11221111", "21112121", "21211121", "21212111", "11222221", "11221211", "11121221", "12121121", "11122211"
	};

	private float module = 13f;

	private float height = 1f;

	private float ratio = 2.2f;

	protected const float CodabarSpaceLeft = 7f;

	protected const float CodabarSpaceRight = 7f;

	protected const float CodabarSpaceTop = 0f;

	protected const float CodabarSpaceBottom = 1f;

	protected const float CodabarLineHeightShort = 45f;

	protected const float CodabarLineHeightLong = 45f;

	protected const float CodabarTextPosition = 46f;

	protected const float CodabarTextHeight = 8.33f;

	protected const float CodabarMainHeight = 55f;

	protected const float CodabarLineHeightForCut = 45f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCodabarBarCodeType;

	public override string ServiceName => "Codabar";

	public override string DefaultCodeValue => "A12345678B";

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

	[Description("Get or sets value, which indicates WideToNarrow ratio.")]
	[DefaultValue(2.2f)]
	[StiSerializable]
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
		code = CheckCodeSymbols(code, CodabarSymbols);
		int[] array = new int[code.Length];
		for (int i = 0; i < code.Length; i++)
		{
			array[i] = CodabarSymbols.IndexOf(code[i]);
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < array.Length; j++)
		{
			stringBuilder.Append(CodeToBar(CodabarTable[array[j]]));
		}
		CalculateSizeFull(7f, 7f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, Ratio, zoom, code, code, stringBuilder.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCodabarBarCodeType();
	}

	public StiCodabarBarCodeType()
		: this(13f, 1f, 2.2f)
	{
	}

	public StiCodabarBarCodeType(float module, float height, float ratio)
	{
		this.module = module;
		this.height = height;
		this.ratio = ratio;
	}
}
