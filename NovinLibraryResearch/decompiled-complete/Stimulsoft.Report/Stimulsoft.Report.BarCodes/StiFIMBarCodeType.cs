using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiFIMBarCodeTypeConverter))]
public class StiFIMBarCodeType : StiBarCodeTypeService
{
	protected string FIMSymbols = "abcd";

	protected string[] FIMTable = new string[4] { "4040114011404", "40140401404014", "40401401401404", "404040140140404" };

	protected const float defaultFIMModule = 31.25f;

	private float module = 31.25f;

	private float height = 1f;

	protected const float FIMSpaceTop = 0f;

	protected const float FIMSpaceBottom = 0f;

	protected const float FIMLineHeightShort = 20f;

	protected const float FIMLineHeightLong = 20f;

	protected const float FIMTextPosition = 0f;

	protected const float FIMTextHeight = 8f;

	protected const float FIMMainHeight = 20f;

	protected const float FIMLineHeightForCut = 20f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFIMBarCodeType;

	public override string ServiceName => "FIM";

	public override string DefaultCodeValue => "A";

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[22] = true;
			return array;
		}
	}

	[Description("Gets or sets width of the most fine element of the bar code.")]
	[DefaultValue(10f)]
	[Browsable(false)]
	public override float Module
	{
		get
		{
			return module;
		}
		set
		{
			module = 31.25f;
		}
	}

	[Description("Gets os sets height factor of the bar code.")]
	[DefaultValue(1f)]
	[Browsable(false)]
	public override float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = 1f;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[Description("Gets or sets value which indicates will show Clear Zone or no.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool AddClearZone { get; set; }

	internal override float LabelFontHeight => 8f;

	protected float FIMSpaceLeft
	{
		get
		{
			if (AddClearZone)
			{
				return 16f;
			}
			return 1f;
		}
	}

	protected float FIMSpaceRight
	{
		get
		{
			if (AddClearZone)
			{
				return 7f;
			}
			return 1f;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 31.25f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		JsonExtensions.AddPropertyBool(val, "AddClearZone", AddClearZone, false, false);
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
			case "AddClearZone":
				AddClearZone = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.AddClearZone() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string inputCode = GetCode(barCode).ToLower();
		inputCode = CheckCodeSymbols(inputCode, FIMSymbols) + "a";
		string barsArray = string.Empty;
		switch (inputCode[0])
		{
		case 'a':
			barsArray = FIMTable[0];
			break;
		case 'b':
			barsArray = FIMTable[1];
			break;
		case 'c':
			barsArray = FIMTable[2];
			break;
		case 'd':
			barsArray = FIMTable[3];
			break;
		}
		CalculateSizeFull(FIMSpaceLeft, FIMSpaceRight, 0f, 0f, 20f, 20f, 0f, 8f, 20f, 20f, 2f, zoom, inputCode, string.Empty, barsArray, rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiFIMBarCodeType();
	}

	public StiFIMBarCodeType()
		: this(31.25f, 1f, addClearZone: false)
	{
	}

	public StiFIMBarCodeType(float module, float height, bool addClearZone)
	{
		this.module = module;
		this.height = height;
		AddClearZone = addClearZone;
	}
}
