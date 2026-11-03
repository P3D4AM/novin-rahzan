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

[TypeConverter(typeof(StiPostnetBarCodeTypeConverter))]
public class StiPostnetBarCodeType : StiBarCodeTypeService
{
	protected string PostnetSymbols = "0123456789";

	private string[] PostnetTable = new string[10] { "11000", "00011", "00101", "00110", "01001", "01010", "01100", "10001", "10010", "10100" };

	private string PostnetStartCode = "1";

	private string PostnetStopCode = "1";

	private float module = 20f;

	private float space = 26f;

	private float height = 1f;

	protected const float PostnetSpaceLeft = 1f;

	protected const float PostnetSpaceRight = 1f;

	protected const float PostnetSpaceTop = 0f;

	protected const float PostnetSpaceBottom = 1f;

	protected const float PostnetLineHeightShort = 2.5f;

	protected const float PostnetLineHeightLong = 6.25f;

	protected const float PostnetTextPosition = 7.25f;

	protected const float PostnetTextHeight = 5.5f;

	protected const float PostnetMainHeight = 14f;

	protected const float PostnetLineHeightForCut = 6.25f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPostnetBarCodeType;

	public override string ServiceName => "Postnet";

	public override string DefaultCodeValue => "12345";

	[Description("Gets or sets width of the most fine element of the bar code.")]
	[DefaultValue(20f)]
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
			if (value < 20f)
			{
				module = 20f;
			}
			if (value > 20f)
			{
				module = 20f;
			}
		}
	}

	[Description("Gets or sets space between elements of bar code.")]
	[DefaultValue(26f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public float Space
	{
		get
		{
			return space;
		}
		set
		{
			space = value;
			if (value < 12f)
			{
				space = 12f;
			}
			if (value > 40f)
			{
				space = 40f;
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
			if (value < 1f)
			{
				height = 1f;
			}
			if (value > 4f)
			{
				height = 4f;
			}
		}
	}

	internal override float LabelFontHeight => 5.5f;

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[11] = true;
			array[13] = true;
			array[18] = true;
			return array;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 20f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyFloat(val, "Space", Space, 26f);
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
			case "Space":
				space = StiJsonReportObjectHelper.DeserializeFloat(item);
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
			propertiesHelper.Space()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	private string CodeToBar(string inputCode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < inputCode.Length; i++)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			if (inputCode[i] == '1')
			{
				stringBuilder2.Append("81");
			}
			else
			{
				stringBuilder2.Append("e1");
			}
			stringBuilder.Append((object?)stringBuilder2);
		}
		return stringBuilder.ToString();
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, PostnetSymbols);
		if (code.Length > 11)
		{
			code = code.Substring(0, 11);
		}
		switch (code.Length)
		{
		case 0:
			code = "00000";
			break;
		case 1:
			code += "0000";
			break;
		case 2:
		case 6:
			code += "000";
			break;
		case 3:
		case 7:
			code += "00";
			break;
		case 4:
		case 8:
		case 10:
			code += "0";
			break;
		}
		StringBuilder stringBuilder = new StringBuilder(code);
		if (code.Length == 11)
		{
			stringBuilder.Insert(9, "-");
		}
		if (code.Length > 5)
		{
			stringBuilder.Insert(5, "-");
		}
		int[] array = new int[code.Length + 1];
		for (int i = 0; i < code.Length; i++)
		{
			array[i] = PostnetSymbols.IndexOf(code[i]);
		}
		int num = 0;
		for (int j = 0; j < code.Length; j++)
		{
			num += array[j];
		}
		num %= 10;
		if (num != 0)
		{
			num = 10 - num;
		}
		array[code.Length] = num;
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(PostnetStartCode);
		for (int k = 0; k < array.Length; k++)
		{
			stringBuilder2.Append(PostnetTable[array[k]]);
		}
		stringBuilder2.Append(PostnetStopCode);
		StringBuilder stringBuilder3 = new StringBuilder(CodeToBar(stringBuilder2.ToString()));
		CalculateSizeFull(1f, 1f, 0f, 1f, 2.5f, 6.25f, 7.25f, 5.5f, 14f, 6.25f, Space / Module, zoom, code, stringBuilder.ToString(), stringBuilder3.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiPostnetBarCodeType();
	}

	public StiPostnetBarCodeType()
		: this(20f, 1f, 26f)
	{
	}

	public StiPostnetBarCodeType(float module, float height, float space)
	{
		this.module = module;
		this.height = height;
		this.space = space;
	}
}
