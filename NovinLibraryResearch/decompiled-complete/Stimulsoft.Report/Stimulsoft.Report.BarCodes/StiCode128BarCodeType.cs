using System.ComponentModel;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.BarCodes.Design;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiBarCodeTypeServiceConverter))]
public abstract class StiCode128BarCodeType : StiBarCodeTypeService
{
	protected enum CodeSet
	{
		None,
		A,
		B,
		C
	}

	protected enum ControlCodes
	{
		FNC1 = 129,
		FNC2,
		FNC3,
		FNC4,
		CodeA,
		CodeB,
		CodeC,
		Shift
	}

	protected enum BarcodeCommands
	{
		FNC1 = 102,
		FNC2 = 97,
		FNC3 = 96,
		FNC4A = 101,
		FNC4B = 100,
		CodeA = 101,
		CodeB = 100,
		CodeC = 99,
		Shift = 98,
		StartA = 103,
		StartB = 104,
		StartC = 105,
		Stop = 106
	}

	protected string[] Code128Table = new string[107]
	{
		"212222", "222122", "222221", "121223", "121322", "131222", "122213", "122312", "132212", "221213",
		"221312", "231212", "112232", "122132", "122231", "113222", "123122", "123221", "223211", "221132",
		"221231", "213212", "223112", "312131", "311222", "321122", "321221", "312212", "322112", "322211",
		"212123", "212321", "232121", "111323", "131123", "131321", "112313", "132113", "132311", "211313",
		"231113", "231311", "112133", "112331", "132131", "113123", "113321", "133121", "313121", "211331",
		"231131", "213113", "213311", "213131", "311123", "311321", "331121", "312113", "312311", "332111",
		"314111", "221411", "431111", "111224", "111422", "121124", "121421", "141122", "141221", "112214",
		"112412", "122114", "122411", "142112", "142211", "241211", "221114", "413111", "241112", "134111",
		"111242", "121142", "121241", "114212", "124112", "124211", "411212", "421112", "421211", "212141",
		"214121", "412121", "111143", "111341", "131141", "114113", "114311", "411113", "411311", "113141",
		"114131", "311141", "411131", "211412", "211214", "211232", "2331112"
	};

	private float module = 13f;

	private float height = 1f;

	protected const float Code128SpaceLeft = 10f;

	protected const float Code128SpaceRight = 10f;

	protected const float Code128SpaceTop = 0f;

	protected const float Code128SpaceBottom = 1f;

	protected const float Code128LineHeightShort = 45f;

	protected const float Code128LineHeightLong = 45f;

	protected const float Code128TextPosition = 46f;

	protected const float Code128TextHeight = 8.33f;

	protected const float Code128MainHeight = 55f;

	protected const float Code128LineHeightForCut = 45f;

	protected const CodeSet DefaultCodeSetAB = CodeSet.B;

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
			if (value < 9.84f)
			{
				module = 9.84f;
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

	internal override float LabelFontHeight => 8.33f;

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[11] = true;
			array[13] = true;
			return array;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Module", Module, 13f);
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

	protected static string EncodeAuto(string inputText, bool encodeAsEan)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (char c in inputText)
		{
			int num = c;
			if (num < 256)
			{
				stringBuilder2.Append(c);
			}
		}
		CodeSet codeSet = GetSet(stringBuilder2, 0, CodeSet.None);
		switch (codeSet)
		{
		case CodeSet.A:
			stringBuilder.Append('g');
			break;
		case CodeSet.B:
			stringBuilder.Append('h');
			break;
		default:
			stringBuilder.Append('i');
			break;
		}
		if (encodeAsEan)
		{
			stringBuilder.Append('f');
		}
		int num2 = 0;
		while (num2 < stringBuilder2.Length)
		{
			CodeSet set = GetSet(stringBuilder2, num2, codeSet);
			if (set != codeSet)
			{
				switch (set)
				{
				case CodeSet.A:
					stringBuilder.Append('e');
					break;
				case CodeSet.B:
					stringBuilder.Append('d');
					break;
				default:
					stringBuilder.Append('c');
					break;
				}
				codeSet = set;
			}
			int num3 = stringBuilder2[num2];
			if (num3 >= 129 && num3 <= 132)
			{
				switch (num3)
				{
				case 129:
					stringBuilder.Append('f');
					break;
				case 130:
					stringBuilder.Append('a');
					break;
				case 131:
					stringBuilder.Append('`');
					break;
				default:
					if (codeSet == CodeSet.A)
					{
						stringBuilder.Append('e');
					}
					else
					{
						stringBuilder.Append('d');
					}
					break;
				}
				num2++;
				continue;
			}
			if (num3 >= 133 && num3 <= 135)
			{
				num2++;
				continue;
			}
			switch (codeSet)
			{
			case CodeSet.A:
				if (num3 < 128)
				{
					stringBuilder.Append((char)((num3 >= 32) ? (num3 - 32) : (num3 + 64)));
					break;
				}
				stringBuilder.Append('e');
				stringBuilder.Append((char)((num3 >= 160) ? (num3 - 160) : (num3 - 64)));
				break;
			case CodeSet.B:
				if (num3 < 128)
				{
					stringBuilder.Append((char)(num3 - 32));
					break;
				}
				stringBuilder.Append('d');
				stringBuilder.Append((char)(num3 - 160));
				break;
			default:
			{
				int num4 = (num3 - 48) * 10 + (stringBuilder2[num2 + 1] - 48);
				stringBuilder.Append((char)num4);
				num2++;
				break;
			}
			}
			num2++;
		}
		stringBuilder.Append('\0');
		stringBuilder.Append('j');
		return stringBuilder.ToString();
	}

	private static CodeSet GetSet(StringBuilder text, int pos, CodeSet prevSet)
	{
		if (text.Length == 0 || pos > text.Length - 1)
		{
			return CodeSet.A;
		}
		int num = text[pos];
		if (num >= 129 && num <= 132)
		{
			if (prevSet == CodeSet.None)
			{
				return CodeSet.B;
			}
			if (num == 129)
			{
				return prevSet;
			}
			if (prevSet == CodeSet.C)
			{
				return CodeSet.B;
			}
			return prevSet;
		}
		switch (num)
		{
		case 133:
			return CodeSet.A;
		case 134:
			return CodeSet.B;
		case 135:
			return CodeSet.C;
		default:
			if (num < 32 || (num >= 128 && num < 160))
			{
				return CodeSet.A;
			}
			if ((num >= 96 && num < 128) || (num >= 224 && num <= 255))
			{
				return CodeSet.B;
			}
			if (char.IsDigit(text[pos]))
			{
				if (prevSet == CodeSet.C)
				{
					if (pos + 1 < text.Length && char.IsDigit(text[pos + 1]))
					{
						return CodeSet.C;
					}
				}
				else if (pos + 3 < text.Length && char.IsDigit(text[pos + 1]) && char.IsDigit(text[pos + 2]) && char.IsDigit(text[pos + 3]))
				{
					return CodeSet.C;
				}
			}
			if (prevSet != 0 && prevSet != CodeSet.C)
			{
				return prevSet;
			}
			return CodeSet.B;
		}
	}

	public StiCode128BarCodeType(float module, float height)
	{
		this.module = module;
		this.height = height;
	}
}
