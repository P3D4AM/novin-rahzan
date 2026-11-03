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

[TypeConverter(typeof(StiRoyalMail4StateBarCodeTypeConverter))]
public class StiRoyalMail4StateBarCodeType : StiBarCodeTypeService
{
	protected string RoyalMail4StateSymbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private string RoyalMail4StateStartCode = "1";

	private string RoyalMail4StateStopCode = "0";

	private string[] RoyalMail4StateCodes = new string[36]
	{
		"3300", "3210", "3201", "2310", "2301", "2211", "3120", "3030", "3021", "2130",
		"2121", "2031", "3102", "3012", "3003", "2112", "2103", "2013", "1320", "1230",
		"1221", "0330", "0321", "0231", "1302", "1212", "1203", "0312", "0303", "0213",
		"1122", "1032", "1023", "0132", "0123", "0033"
	};

	private float module = 20f;

	private float height = 1f;

	protected const float RoyalMail4StateSpaceLeft = 3.9f;

	protected const float RoyalMail4StateSpaceRight = 3.9f;

	protected const float RoyalMail4StateSpaceTop = 3.9f;

	protected const float RoyalMail4StateSpaceBottom = 3.9f;

	protected const float RoyalMail4StateLineHeightShort = 6.2f;

	protected const float RoyalMail4StateLineHeightLong = 10f;

	protected const float RoyalMail4StateTextPosition = 18.8f;

	protected const float RoyalMail4StateTextHeight = 5f;

	protected const float RoyalMail4StateMainHeight = 24.8f;

	protected const float RoyalMail4StateLineHeightForCut = 10f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRoyalMail4StateBarCodeType;

	public override string ServiceName => "Royal Mail 4-state";

	public override string DefaultCodeValue => "529508A";

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

	[Description("Gets or sets mode of checksum.")]
	[DefaultValue(StiCheckSum.Yes)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiCheckSum CheckSum { get; set; }

	internal override float LabelFontHeight => 5f;

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[13] = true;
			array[23] = true;
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
		JsonExtensions.AddPropertyEnum(val, "CheckSum", (Enum)CheckSum, (Enum)StiCheckSum.Yes);
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
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.CheckSum(),
			propertiesHelper.Module()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	private string CharTo4State(char inputChar)
	{
		int num = RoyalMail4StateSymbols.IndexOf(inputChar);
		return RoyalMail4StateCodes[num];
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
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < code.Length; i++)
		{
			string text = CharTo4State(code[i]);
			if (text[0] == '0' || text[0] == '1')
			{
				num += 4;
			}
			if (text[0] == '0' || text[0] == '2')
			{
				num2 += 4;
			}
			if (text[1] == '0' || text[1] == '1')
			{
				num += 2;
			}
			if (text[1] == '0' || text[1] == '2')
			{
				num2 += 2;
			}
			if (text[2] == '0' || text[2] == '1')
			{
				num++;
			}
			if (text[2] == '0' || text[2] == '2')
			{
				num2++;
			}
			stringBuilder.Append(text);
		}
		num %= 6;
		num2 %= 6;
		if (num == 0)
		{
			num = 6;
		}
		if (num2 == 0)
		{
			num2 = 6;
		}
		int index = (num - 1) * 6 + (num2 - 1);
		if (CheckSum == StiCheckSum.Yes)
		{
			stringBuilder.Append(CharTo4State(RoyalMail4StateSymbols[index]));
		}
		barsArray = StateToBar(RoyalMail4StateStartCode + stringBuilder.ToString() + RoyalMail4StateStopCode);
		return true;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, RoyalMail4StateSymbols);
		string barsArray = string.Empty;
		string errorString = string.Empty;
		if (MakeBarsArray(ref code, ref barsArray, ref errorString))
		{
			CalculateSizeFull(3.9f, 3.9f, 3.9f, 3.9f, 6.2f, 10f, 18.8f, 5f, 24.8f, 10f, 1f, zoom, code, code, barsArray, rect, barCode);
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
		return new StiRoyalMail4StateBarCodeType();
	}

	public StiRoyalMail4StateBarCodeType()
		: this(20f, 1f, StiCheckSum.Yes)
	{
	}

	public StiRoyalMail4StateBarCodeType(float module, float height, StiCheckSum checkSum)
	{
		this.module = module;
		this.height = height;
		CheckSum = checkSum;
	}
}
