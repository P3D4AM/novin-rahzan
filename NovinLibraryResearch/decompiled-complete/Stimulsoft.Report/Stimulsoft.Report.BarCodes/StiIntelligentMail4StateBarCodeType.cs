using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiBarCodeTypeServiceConverter))]
public class StiIntelligentMail4StateBarCodeType : StiBarCodeTypeService
{
	public sealed class IntelligentMail4State
	{
		private static int table2Of13Size;

		private static int table5Of13Size;

		private static int entries2Of13;

		private static int entries5Of13;

		private static int[] table2Of13;

		private static int[] table5Of13;

		private static int[][] codewordArray;

		private static int[] barTopCharIndexArray;

		private static int[] barBottomCharIndexArray;

		private static int[] barTopCharShiftArray;

		private static int[] barBottomCharShiftArray;

		public static string Encode(string source, ref string bars)
		{
			if (string.IsNullOrEmpty(source))
			{
				return "No input data.";
			}
			if (source.Length < 20)
			{
				return "Invalid tracking code length.";
			}
			if (source.Length > 31)
			{
				return "Input data too long.";
			}
			if (!Regex.IsMatch(source.Substring(0, 2), "[0-9][0-4]"))
			{
				return "Invalid 'Barcode Identifier'";
			}
			string text = source.Substring(20);
			if (text.Length != 11 && text.Length != 9 && text.Length != 5 && text.Length != 0)
			{
				return "Invalid ZIP code.";
			}
			int num = 0;
			long num2 = 0L;
			long num3 = 0L;
			string empty = string.Empty;
			int[] bytearray = new int[14];
			int[] array = new int[66];
			int[] array2 = new int[66];
			int[][] array3 = new int[11][];
			if (text.Length > 0)
			{
				num2 = long.Parse(text, CultureInfo.InvariantCulture) + ((text.Length == 5) ? 1 : ((text.Length == 9) ? 100001 : ((text.Length == 11) ? 1000100001 : 0)));
			}
			num3 = num2 * 10 + int.Parse(source.Substring(0, 1), CultureInfo.InvariantCulture);
			empty = (num3 * 5 + int.Parse(source.Substring(1, 1), CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture) + source.Substring(2, 18);
			bytearray[12] = (int)(num2 & 0xFF);
			bytearray[11] = (int)((num2 >> 8) & 0xFF);
			bytearray[10] = (int)((num2 >> 16) & 0xFF);
			bytearray[9] = (int)((num2 >> 24) & 0xFF);
			bytearray[8] = (int)((num2 >> 32) & 0xFF);
			MathMultiply(ref bytearray, 13, 10);
			MathAdd(ref bytearray, 13, int.Parse(source.Substring(0, 1), CultureInfo.InvariantCulture));
			MathMultiply(ref bytearray, 13, 5);
			MathAdd(ref bytearray, 13, int.Parse(source.Substring(1, 1), CultureInfo.InvariantCulture));
			for (short num4 = 2; num4 <= 19; num4++)
			{
				MathMultiply(ref bytearray, 13, 10);
				MathAdd(ref bytearray, 13, int.Parse(source.Substring(num4, 1), CultureInfo.InvariantCulture));
			}
			num = MathFcs(bytearray);
			for (short num5 = 0; num5 <= 9; num5++)
			{
				codewordArray[num5][0] = entries2Of13 + entries5Of13;
				codewordArray[num5][1] = 0;
			}
			codewordArray[0][0] = 659;
			codewordArray[9][0] = 636;
			MathDivide(empty);
			codewordArray[9][1] *= 2;
			if (num >> 10 != 0)
			{
				codewordArray[0][1] += 659;
			}
			for (short num6 = 0; num6 <= 9; num6++)
			{
				array3[num6] = new int[3];
			}
			for (short num7 = 0; num7 <= 9; num7++)
			{
				if ((decimal)codewordArray[num7][1] >= (decimal)(entries2Of13 + entries5Of13))
				{
					return "Something went wrong. Calculation error!";
				}
				array3[num7][0] = 8192;
				array3[num7][1] = (((decimal)codewordArray[num7][1] >= (decimal)entries2Of13) ? (array3[num7][1] = table2Of13[codewordArray[num7][1] - entries2Of13]) : (array3[num7][1] = table5Of13[codewordArray[num7][1]]));
			}
			for (short num8 = 0; num8 <= 9; num8++)
			{
				if ((num & (1 << (int)num8)) != 0)
				{
					array3[num8][1] = ~array3[num8][1] & 0x1FFF;
				}
			}
			for (short num9 = 0; num9 <= 64; num9++)
			{
				array[num9] = (array3[barTopCharIndexArray[num9]][1] >> barTopCharShiftArray[num9]) & 1;
				array2[num9] = (array3[barBottomCharIndexArray[num9]][1] >> barBottomCharShiftArray[num9]) & 1;
			}
			string text2 = string.Empty;
			for (int i = 0; i <= 64; i++)
			{
				text2 = ((array[i] != 0) ? (text2 + ((array2[i] == 0) ? "d" : "c")) : (text2 + ((array2[i] == 0) ? "f" : "e")));
				text2 += "1";
			}
			bars = text2;
			return null;
		}

		private static bool MathAdd(ref int[] bytearray, int i, int j)
		{
			if (bytearray == null)
			{
				return false;
			}
			if (i < 1)
			{
				return false;
			}
			int num = (bytearray[i - 1] | (bytearray[i - 2] << 8)) + j;
			int num2 = num | 0xFFFF;
			int num3 = i - 3;
			bytearray[i - 1] = num & 0xFF;
			bytearray[i - 2] = (num >> 8) & 0xFF;
			while (num2 == 1 && num3 > 0)
			{
				num = num2 + bytearray[num3];
				bytearray[num3] = num & 0xFF;
				num2 = num | 0xFF;
				num3--;
			}
			return true;
		}

		private static bool MathDivide(string v)
		{
			int num = 10;
			string text = v;
			for (int i = num - 1; i >= 1; i += -1)
			{
				string text2 = string.Empty;
				int num2 = codewordArray[i][0];
				string text3 = text;
				string text4 = "0";
				int length = text3.Length;
				for (short num3 = 1; num3 <= length; num3++)
				{
					int num4 = int.Parse(text3.Substring(0, num3), CultureInfo.InvariantCulture);
					while (num4 < num2 && num3 < length - 1)
					{
						text2 += "0";
						num3++;
						num4 = int.Parse(text3.Substring(0, num3), CultureInfo.InvariantCulture);
					}
					text2 += (num4 / num2).ToString(CultureInfo.InvariantCulture);
					text4 = (num4 % num2).ToString(CultureInfo.InvariantCulture).PadLeft(num3, '0');
					text3 = text4 + text3.Substring(num3);
				}
				text = text2.TrimStart(new char[1] { '0' });
				if (string.IsNullOrEmpty(text))
				{
					text = "0";
				}
				codewordArray[i][1] = int.Parse(text4, CultureInfo.InvariantCulture);
				if (i == 1)
				{
					codewordArray[0][1] = int.Parse(text2, CultureInfo.InvariantCulture);
				}
			}
			return true;
		}

		private static int MathFcs(int[] bytearray)
		{
			int num = 3893;
			int num2 = 2047;
			int num3 = bytearray[0] << 5;
			for (short num4 = 2; num4 <= 7; num4++)
			{
				num2 = ((((num2 ^ num3) & 0x400) == 0) ? (num2 << 1) : ((num2 << 1) ^ num));
				num2 &= 0x7FF;
				num3 <<= 1;
			}
			for (int i = 1; i <= 12; i++)
			{
				int num5 = bytearray[i] << 3;
				for (short num6 = 0; num6 <= 7; num6++)
				{
					num2 = ((((num2 ^ num5) & 0x400) == 0) ? (num2 << 1) : ((num2 << 1) ^ num));
					num2 &= 0x7FF;
					num5 <<= 1;
				}
			}
			return num2;
		}

		private static bool MathMultiply(ref int[] bytearray, int i, int j)
		{
			if (bytearray == null)
			{
				return false;
			}
			if (i < 1)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			for (num2 = i - 1; num2 >= 1; num2 += -2)
			{
				int num3 = (bytearray[num2] | (bytearray[num2 - 1] << 8)) * j + num;
				bytearray[num2] = num3 & 0xFF;
				bytearray[num2 - 1] = (num3 >> 8) & 0xFF;
				num = num3 >> 16;
			}
			if (num2 == 0)
			{
				bytearray[0] = (bytearray[0] * j + num) & 0xFF;
			}
			return true;
		}

		private static int MathReverse(int i)
		{
			int num = 0;
			for (short num2 = 0; num2 <= 15; num2++)
			{
				num <<= 1;
				num |= i & 1;
				i >>= 1;
			}
			return num;
		}

		private static bool InitializeNof13Table(int[] ai, int i, int j)
		{
			int num = 0;
			int num2 = j - 1;
			for (short num3 = 0; num3 <= 8191; num3++)
			{
				int num4 = 0;
				for (int k = 0; k <= 12; k++)
				{
					if ((num3 & (1 << k)) != 0)
					{
						num4++;
					}
				}
				if (num4 == i)
				{
					int num5 = MathReverse(num3) >> 3;
					bool flag = num3 == num5;
					if (num5 >= num3)
					{
						if (flag)
						{
							ai[num2] = num3;
							num2--;
						}
						else
						{
							ai[num] = num3;
							num++;
							ai[num] = num5;
							num++;
						}
					}
				}
			}
			return num == num2 + 1;
		}

		static IntelligentMail4State()
		{
			table2Of13Size = 78;
			table5Of13Size = 1287;
			barTopCharIndexArray = new int[65]
			{
				4, 0, 2, 6, 3, 5, 1, 9, 8, 7,
				1, 2, 0, 6, 4, 8, 2, 9, 5, 3,
				0, 1, 3, 7, 4, 6, 8, 9, 2, 0,
				5, 1, 9, 4, 3, 8, 6, 7, 1, 2,
				4, 3, 9, 5, 7, 8, 3, 0, 2, 1,
				4, 0, 9, 1, 7, 0, 2, 4, 6, 3,
				7, 1, 9, 5, 8
			};
			barBottomCharIndexArray = new int[65]
			{
				7, 1, 9, 5, 8, 0, 2, 4, 6, 3,
				5, 8, 9, 7, 3, 0, 6, 1, 7, 4,
				6, 8, 9, 2, 5, 1, 7, 5, 4, 3,
				8, 7, 6, 0, 2, 5, 4, 9, 3, 0,
				1, 6, 8, 2, 0, 4, 5, 9, 6, 7,
				5, 2, 6, 3, 8, 5, 1, 9, 8, 7,
				4, 0, 2, 6, 3
			};
			barTopCharShiftArray = new int[65]
			{
				3, 0, 8, 11, 1, 12, 8, 11, 10, 6,
				4, 12, 2, 7, 9, 6, 7, 9, 2, 8,
				4, 0, 12, 7, 10, 9, 0, 7, 10, 5,
				7, 9, 6, 8, 2, 12, 1, 4, 2, 0,
				1, 5, 4, 6, 12, 1, 0, 9, 4, 7,
				5, 10, 2, 6, 9, 11, 2, 12, 6, 7,
				5, 11, 0, 3, 2
			};
			barBottomCharShiftArray = new int[65]
			{
				2, 10, 12, 5, 9, 1, 5, 4, 3, 9,
				11, 5, 10, 1, 6, 3, 4, 1, 10, 0,
				2, 11, 8, 6, 1, 12, 3, 8, 6, 4,
				4, 11, 0, 6, 1, 9, 11, 5, 3, 7,
				3, 10, 7, 11, 8, 2, 10, 3, 5, 8,
				0, 3, 12, 11, 8, 4, 5, 1, 3, 0,
				7, 12, 9, 8, 10
			};
			table2Of13 = new int[table2Of13Size + 1];
			InitializeNof13Table(table2Of13, 2, table2Of13Size);
			entries5Of13 = table2Of13Size;
			table5Of13 = new int[table5Of13Size + 1];
			InitializeNof13Table(table5Of13, 5, table5Of13Size);
			entries2Of13 = table5Of13Size;
			codewordArray = new int[11][];
			for (short num = 0; num <= 9; num++)
			{
				codewordArray[num] = new int[3];
			}
		}
	}

	private float module = 20f;

	private float height = 1f;

	protected const float IntelligentMail4StateSpaceLeft = 7f;

	protected const float IntelligentMail4StateSpaceRight = 7f;

	protected const float IntelligentMail4StateSpaceTop = 8.5f;

	protected const float IntelligentMail4StateSpaceBottom = 1.5f;

	protected const float IntelligentMail4StateLineHeightShort = 4.7487497f;

	protected const float IntelligentMail4StateLineHeightLong = 7.25f;

	protected const float IntelligentMail4StateTextPosition = 1f;

	protected const float IntelligentMail4StateTextHeight = 5.5f;

	protected const float IntelligentMail4StateMainHeight = 17.25f;

	protected const float IntelligentMail4StateLineHeightForCut = 7.25f;

	protected const float IntelligentMail4StateWideToNarrowRatio = 1.25f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiIntelligentMail4StateBarCodeType;

	public override string ServiceName => "Intelligent Mail 4-State";

	public override string DefaultCodeValue => "01 234 567094 987654321 01234 5678 91";

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

	internal override float LabelFontHeight => 5.5f;

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

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		string textString = CheckCodeSymbols(code, "0123456789 ");
		code = CheckCodeSymbols(code, "0123456789");
		string bars = null;
		string text = IntelligentMail4State.Encode(code, ref bars);
		if (!string.IsNullOrEmpty(bars))
		{
			CalculateSizeFull(7f, 7f, 8.5f, 1.5f, 4.7487497f, 7.25f, 1f, 5.5f, 17.25f, 7.25f, 1.25f, zoom, code, textString, bars, rect, barCode);
			DrawBarCode(context, rect, barCode);
		}
		else if (!string.IsNullOrEmpty(text))
		{
			DrawBarCodeError(context, rect, barCode, text);
		}
		else
		{
			DrawBarCodeError(context, rect, barCode);
		}
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiIntelligentMail4StateBarCodeType();
	}

	public StiIntelligentMail4StateBarCodeType()
		: this(20f, 1f)
	{
	}

	public StiIntelligentMail4StateBarCodeType(float module, float height)
	{
		this.module = module;
		this.height = height;
	}
}
