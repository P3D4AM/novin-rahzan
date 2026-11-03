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
public class StiAustraliaPost4StateBarCodeType : StiBarCodeTypeService
{
	protected string AustraliaPost4StateSymbolsC = "ABC DEF#GHIabcdeJKLfMNOgPQRhijklSTUmVWXnYZ0opqrs123t456u789vwxyz";

	protected string AustraliaPost4StateSymbolsN = "012_345_678_9";

	private string AustraliaPost4StateStartCode = "13";

	private string AustraliaPost4StateStopCode = "13";

	private float module = 20f;

	private float height = 1f;

	protected const float AustraliaPost4StateSpaceLeft = 11.8f;

	protected const float AustraliaPost4StateSpaceRight = 11.8f;

	protected const float AustraliaPost4StateSpaceTop = 10.9f;

	protected const float AustraliaPost4StateSpaceBottom = 3.9f;

	protected const float AustraliaPost4StateLineHeightShort = 6.2f;

	protected const float AustraliaPost4StateLineHeightLong = 10f;

	protected const float AustraliaPost4StateTextPosition = 1f;

	protected const float AustraliaPost4StateTextHeight = 5f;

	protected const float AustraliaPost4StateMainHeight = 24.8f;

	protected const float AustraliaPost4StateLineHeightForCut = 10f;

	private static uint[,] mult = new uint[64, 64];

	private static int[] gen = new int[5];

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiAustraliaPost4StateBarCodeType;

	public override string ServiceName => "Australia Post 4-state";

	public override string DefaultCodeValue => "1139987520";

	[DefaultValue(20f)]
	[Description("Gets or sets width of the most fine element of the bar code.")]
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

	[DefaultValue(1f)]
	[Browsable(false)]
	[Description("Gets os sets height factor of the bar code.")]
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

	protected override StringAlignment TextAlignment => (StringAlignment)0;

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

	private static void RSInitialise()
	{
		uint num = 67u;
		uint num2 = 64u;
		for (int i = 0; i < 64; i++)
		{
			mult[0, i] = 0u;
			mult[1, i] = (uint)i;
		}
		uint num3 = 1u;
		for (int j = 1; j < 64; j++)
		{
			uint num4 = num3 << 1;
			if ((num4 & num2) != 0)
			{
				num4 ^= num;
			}
			for (int k = 0; k < 64; k++)
			{
				mult[num4, k] = mult[num3, k] << 1;
				if ((mult[num4, k] & num2) != 0)
				{
					mult[num4, k] ^= num;
				}
			}
			num3 = num4;
		}
		gen[0] = 48;
		gen[1] = 17;
		gen[2] = 29;
		gen[3] = 30;
		gen[4] = 1;
	}

	private static uint[] RSEncode(uint[] infosymbols)
	{
		if (gen[0] == 0)
		{
			RSInitialise();
		}
		uint[] array = new uint[31];
		int num = infosymbols.Length;
		for (int i = 0; i < 4; i++)
		{
			array[i] = 0u;
		}
		for (int j = 0; j < num; j++)
		{
			array[j + 4] = infosymbols[num - 1 - j];
		}
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			for (int k = 0; k <= 4; k++)
			{
				array[num2 + k] = array[num2 + k] ^ mult[gen[k], array[4 + num2]];
			}
		}
		uint[] array2 = new uint[4];
		for (int l = 0; l < 4; l++)
		{
			array2[l] = array[3 - l];
		}
		return array2;
	}

	private string CharTo4State(char inputChar, bool useTableC)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (useTableC)
		{
			int num = AustraliaPost4StateSymbolsC.IndexOf(inputChar);
			stringBuilder.Append((char)(((num >> 4) & 3) + 48));
			stringBuilder.Append((char)(((num >> 2) & 3) + 48));
			stringBuilder.Append((char)((num & 3) + 48));
		}
		else
		{
			int num2 = AustraliaPost4StateSymbolsN.IndexOf(inputChar);
			stringBuilder.Append((char)(((num2 >> 2) & 3) + 48));
			stringBuilder.Append((char)((num2 & 3) + 48));
		}
		return stringBuilder.ToString();
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
		if (code.Length < 10)
		{
			errorString = "Data too short";
			return false;
		}
		string text = code.Substring(0, 2);
		string text2 = code.Substring(2, 8);
		string text3 = code.Substring(10);
		int num = -1;
		switch (text)
		{
		case "11":
		case "87":
		case "45":
		case "92":
			num = 0;
			break;
		case "59":
			num = 16;
			break;
		case "62":
		case "44":
			num = 31;
			break;
		}
		if (num < 0)
		{
			errorString = "Unknown FCC";
			return false;
		}
		int num2 = num / 3;
		if (text3.Length > num2)
		{
			errorString = "CustomerInfo too long";
			return false;
		}
		if (num2 > text3.Length)
		{
			num2 = text3.Length;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(CharTo4State(text[0], useTableC: false));
		stringBuilder.Append(CharTo4State(text[1], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[0], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[1], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[2], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[3], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[4], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[5], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[6], useTableC: false));
		stringBuilder.Append(CharTo4State(text2[7], useTableC: false));
		for (int i = 0; i < num2; i++)
		{
			stringBuilder.Append(CharTo4State(text3[i], useTableC: true));
		}
		if (stringBuilder.Length < 21 + num)
		{
			stringBuilder.Append('3', 21 + num - stringBuilder.Length);
		}
		int num3 = stringBuilder.Length / 3;
		uint[] array = new uint[num3];
		for (int j = 0; j < num3; j++)
		{
			array[j] = (uint)((stringBuilder[j * 3] - 48 << 4) + (stringBuilder[j * 3 + 1] - 48 << 2) + (stringBuilder[j * 3 + 2] - 48));
		}
		uint[] array2 = RSEncode(array);
		for (int k = 0; k < 4; k++)
		{
			stringBuilder.Append((char)(((array2[k] >> 4) & 3) + 48));
			stringBuilder.Append((char)(((array2[k] >> 2) & 3) + 48));
			stringBuilder.Append((char)((array2[k] & 3) + 48));
		}
		barsArray = StateToBar(AustraliaPost4StateStartCode + stringBuilder.ToString() + AustraliaPost4StateStopCode);
		code = $"{text} {text2} {text3.Substring(0, num2)} {array2[0]} {array2[1]} {array2[2]} {array2[3]}";
		return true;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, AustraliaPost4StateSymbolsC);
		string barsArray = string.Empty;
		string errorString = string.Empty;
		if (MakeBarsArray(ref code, ref barsArray, ref errorString))
		{
			CalculateSizeFull(11.8f, 11.8f, 10.9f, 3.9f, 6.2f, 10f, 1f, 5f, 24.8f, 10f, 1f, zoom, code, code, barsArray, rect, barCode);
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
		return new StiAustraliaPost4StateBarCodeType();
	}

	public StiAustraliaPost4StateBarCodeType()
		: this(20f, 1f)
	{
	}

	public StiAustraliaPost4StateBarCodeType(float module, float height)
	{
		this.module = module;
		this.height = height;
	}
}
