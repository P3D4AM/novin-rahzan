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

[TypeConverter(typeof(StiCode93BarCodeTypeConverter))]
public class StiCode93BarCodeType : StiBarCodeTypeService
{
	private string Code93Symbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";

	protected string[] Code93Table = new string[49]
	{
		"131112", "111213", "111312", "111411", "121113", "121212", "121311", "111114", "131211", "141111",
		"211113", "211212", "211311", "221112", "221211", "231111", "112113", "112212", "112311", "122112",
		"132111", "111123", "111222", "111321", "121122", "131121", "212112", "212211", "211122", "211221",
		"221121", "222111", "112122", "112221", "122121", "123111", "121131", "311112", "311211", "321111",
		"112131", "113121", "211131", "121221", "312111", "311121", "122211", "111141", "1111411"
	};

	private float module = 13f;

	private float height = 1f;

	private float ratio = 2.2f;

	protected const float Code93SpaceLeft = 10f;

	protected const float Code93SpaceRight = 10f;

	protected const float Code93SpaceTop = 0f;

	protected const float Code93SpaceBottom = 1f;

	protected const float Code93LineHeightShort = 45f;

	protected const float Code93LineHeightLong = 45f;

	protected const float Code93TextPosition = 46f;

	protected const float Code93TextHeight = 8.33f;

	protected const float Code93MainHeight = 55f;

	protected const float Code93LineHeightForCut = 45f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode93BarCodeType;

	public override string ServiceName => "Code93";

	public override string DefaultCodeValue => "ABC123";

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
			char value = ((!flag) ? (inputCode[i] switch
			{
				'1' => '0', 
				'2' => '1', 
				'3' => '2', 
				'4' => '3', 
				_ => '0', 
			}) : (inputCode[i] switch
			{
				'1' => '4', 
				'2' => '5', 
				'3' => '6', 
				'4' => '7', 
				_ => '4', 
			}));
			stringBuilder.Append(value);
			flag = !flag;
		}
		return stringBuilder.ToString();
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, Code93Symbols);
		int[] array = new int[code.Length + 4];
		for (int i = 0; i < code.Length; i++)
		{
			array[i + 1] = Code93Symbols.IndexOf(code[i]);
		}
		int num = 0;
		int num2 = 1;
		for (int num3 = code.Length - 1; num3 >= 0; num3--)
		{
			num += array[num3 + 1] * num2;
			num2++;
			if (num2 > 20)
			{
				num2 = 1;
			}
		}
		array[^3] = num % 47;
		int num4 = 0;
		num2 = 1;
		for (int num5 = code.Length; num5 >= 0; num5--)
		{
			num4 += array[num5 + 1] * num2;
			num2++;
			if (num2 > 15)
			{
				num2 = 1;
			}
		}
		array[^2] = num4 % 47;
		array[0] = Code93Table.Length - 2;
		array[^1] = Code93Table.Length - 1;
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < array.Length; j++)
		{
			stringBuilder.Append(CodeToBar(Code93Table[array[j]]));
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, Ratio, zoom, code, code, stringBuilder.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode93BarCodeType();
	}

	public StiCode93BarCodeType()
		: this(13f, 1f, 2.2f)
	{
	}

	public StiCode93BarCodeType(float module, float height, float ratio)
	{
		this.module = module;
		this.height = height;
		this.ratio = ratio;
	}
}
