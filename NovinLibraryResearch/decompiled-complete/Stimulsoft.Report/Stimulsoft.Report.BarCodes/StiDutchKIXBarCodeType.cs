using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiDutchKIXBarCodeType : StiBarCodeTypeService
{
	protected string DutchKIXSymbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private string[] DutchKIXCodes = new string[36]
	{
		"3300", "3210", "3201", "2310", "2301", "2211", "3120", "3030", "3021", "2130",
		"2121", "2031", "3102", "3012", "3003", "2112", "2103", "2013", "1320", "1230",
		"1221", "0330", "0321", "0231", "1302", "1212", "1203", "0312", "0303", "0213",
		"1122", "1032", "1023", "0132", "0123", "0033"
	};

	private float module = 20f;

	private float height = 1f;

	protected const float DutchKIXSpaceLeft = 3.9f;

	protected const float DutchKIXSpaceRight = 3.9f;

	protected const float DutchKIXSpaceTop = 3.9f;

	protected const float DutchKIXSpaceBottom = 2f;

	protected const float DutchKIXLineHeightShort = 6.2f;

	protected const float DutchKIXLineHeightLong = 10f;

	protected const float DutchKIXTextPosition = 16.9f;

	protected const float DutchKIXTextHeight = 5f;

	protected const float DutchKIXMainHeight = 22.9f;

	protected const float DutchKIXLineHeightForCut = 10f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDutchKIXBarCodeType;

	public override string ServiceName => "Royal TPG Post KIX 4-State";

	public override string DefaultCodeValue => "2500GG30250";

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
			if (value < 15f)
			{
				module = 15f;
			}
			if (value > 25f)
			{
				module = 25f;
			}
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
			height = value;
			if (value < 1f)
			{
				height = 1f;
			}
			if (value > 1f)
			{
				height = 1f;
			}
		}
	}

	internal override float LabelFontHeight => 5f;

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[13] = true;
			return array;
		}
	}

	protected override StringAlignment TextAlignment => (StringAlignment)1;

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
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Module() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	private string CharTo4State(char inputChar)
	{
		int num = DutchKIXSymbols.IndexOf(inputChar);
		return DutchKIXCodes[num];
	}

	private string StateToBar(string inputCode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < inputCode.Length; i++)
		{
			switch (inputCode[i])
			{
			case '0':
				stringBuilder.Append("c");
				break;
			case '1':
				stringBuilder.Append("d");
				break;
			case '2':
				stringBuilder.Append("e");
				break;
			case '3':
				stringBuilder.Append("f");
				break;
			}
			stringBuilder.Append("0");
		}
		return stringBuilder.ToString();
	}

	private bool MakeBarsArray(ref string code, ref string barsArray, ref string errorString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < code.Length; i++)
		{
			stringBuilder.Append(CharTo4State(code[i]));
		}
		barsArray = StateToBar(stringBuilder.ToString());
		return true;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, DutchKIXSymbols);
		string barsArray = string.Empty;
		string errorString = string.Empty;
		if (MakeBarsArray(ref code, ref barsArray, ref errorString))
		{
			CalculateSizeFull(3.9f, 3.9f, 3.9f, 2f, 6.2f, 10f, 16.9f, 5f, 22.9f, 10f, 1f, zoom, code, code, barsArray, rect, barCode);
			DrawBarCode(context, rect, barCode);
		}
		else if (errorString.Length > 0)
		{
			DrawBarCodeError(context, rect, barCode, errorString);
		}
		else
		{
			DrawBarCodeError(context, rect, barCode);
		}
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiDutchKIXBarCodeType();
	}

	public StiDutchKIXBarCodeType()
		: this(20f, 1f)
	{
	}

	public StiDutchKIXBarCodeType(float module, float height)
	{
		this.module = module;
		this.height = height;
	}
}
