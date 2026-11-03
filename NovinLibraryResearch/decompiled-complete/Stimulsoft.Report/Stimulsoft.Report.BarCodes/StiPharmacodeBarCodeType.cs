using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiBarCodeTypeServiceConverter))]
public class StiPharmacodeBarCodeType : StiBarCodeTypeService
{
	protected string PharmacodeSymbols = "0123456789";

	protected const float defaultPharmacodeModule = 20f;

	private float module = 20f;

	private float height = 1f;

	protected const float PharmacodeSpaceLeft = 1f;

	protected const float PharmacodeSpaceRight = 1f;

	protected const float PharmacodeSpaceTop = 0f;

	protected const float PharmacodeSpaceBottom = 0.5f;

	protected const float PharmacodeLineHeightShort = 16f;

	protected const float PharmacodeLineHeightLong = 16f;

	protected const float PharmacodeTextPosition = 16.5f;

	protected const float PharmacodeTextHeight = 5f;

	protected const float PharmacodeMainHeight = 22f;

	protected const float PharmacodeLineHeightForCut = 16f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPharmacodeBarCodeType;

	public override string ServiceName => "Pharmacode";

	public override string DefaultCodeValue => "1256";

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
			module = 20f;
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

	internal override float LabelFontHeight => 5f;

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[11] = true;
			return array;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 20f);
		JsonExtensions.AddPropertyFloat(val, "Height", Height, 1f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Module"))
			{
				if (name == "Height")
				{
					height = StiJsonReportObjectHelper.DeserializeFloat(item);
				}
			}
			else
			{
				module = StiJsonReportObjectHelper.DeserializeFloat(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.fHeight() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, PharmacodeSymbols);
		if (code.Length > 6)
		{
			code = "131070";
		}
		if (code.Length < 1)
		{
			code = "3";
		}
		int num = int.Parse(code);
		if (num > 131070)
		{
			num = 131070;
		}
		if (num < 3)
		{
			num = 3;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = num - 3;
		int num3 = 2;
		int num4 = 3;
		while (num2 > num4)
		{
			num2 -= num4 + 1;
			num3++;
			num4 = (1 << num3) - 1;
		}
		for (int num5 = num3 - 1; num5 >= 0; num5--)
		{
			if ((num2 & (1 << num5)) > 0)
			{
				stringBuilder.Append("6");
			}
			else
			{
				stringBuilder.Append("4");
			}
			if (num5 != 0)
			{
				stringBuilder.Append("1");
			}
		}
		CalculateSizeFull(1f, 1f, 0f, 0.5f, 16f, 16f, 16.5f, 5f, 22f, 16f, 2f, zoom, code, num.ToString(), stringBuilder.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiPharmacodeBarCodeType();
	}

	public StiPharmacodeBarCodeType()
		: this(20f, 1f)
	{
	}

	public StiPharmacodeBarCodeType(float module, float height)
	{
		this.module = module;
		this.height = height;
	}
}
