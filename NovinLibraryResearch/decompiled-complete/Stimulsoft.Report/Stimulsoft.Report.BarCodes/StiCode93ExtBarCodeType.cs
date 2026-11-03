using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiCode93ExtBarCodeType : StiCode93BarCodeType
{
	private string Code93ExtSymbols = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%<]>[";

	private string[] Code93ExtTable = new string[128]
	{
		"]U", "<A", "<B", "<C", "<D", "<E", "<F", "<G", "<H", "<I",
		"<J", "<K", "<L", "<M", "<N", "<O", "<P", "<Q", "<R", "<S",
		"<T", "<U", "<V", "<W", "<X", "<Y", "<Z", "]A", "]B", "]C",
		"]D", "]E", " ", ">A", ">B", ">C", ">D", ">E", ">F", ">G",
		">H", ">I", ">J", ">K", ">L", "-", ".", ">O", "0", "1",
		"2", "3", "4", "5", "6", "7", "8", "9", ">Z", "]F",
		"]G", "]H", "]I", "]J", "]V", "A", "B", "C", "D", "E",
		"F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
		"P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y",
		"Z", "]K", "]L", "]M", "]N", "]O", "]W", "[A", "[B", "[C",
		"[D", "[E", "[F", "[G", "[H", "[I", "[J", "[K", "[L", "[M",
		"[N", "[O", "[P", "[Q", "[R", "[S", "[T", "[U", "[V", "[W",
		"[X", "[Y", "[Z", "]P", "]Q", "]R", "]S", "]T"
	};

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode93ExtBarCodeType;

	public override string ServiceName => "Code93 Extended";

	public override string DefaultCodeValue => "Abc123";

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

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string text = GetCode(barCode);
		if (text == null)
		{
			text = string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (int num in text)
		{
			if (num < 128)
			{
				stringBuilder.Append(Code93ExtTable[num]);
				stringBuilder2.Append((char)num);
			}
		}
		text = stringBuilder.ToString();
		int[] array = new int[text.Length + 4];
		for (int j = 0; j < text.Length; j++)
		{
			array[j + 1] = Code93ExtSymbols.IndexOf(text[j]);
		}
		int num2 = 0;
		int num3 = 1;
		for (int num4 = text.Length - 1; num4 >= 0; num4--)
		{
			num2 += array[num4 + 1] * num3;
			num3++;
			if (num3 > 20)
			{
				num3 = 1;
			}
		}
		array[^3] = num2 % 47;
		int num5 = 0;
		num3 = 1;
		for (int num6 = text.Length; num6 >= 0; num6--)
		{
			num5 += array[num6 + 1] * num3;
			num3++;
			if (num3 > 15)
			{
				num3 = 1;
			}
		}
		array[^2] = num5 % 47;
		array[0] = Code93Table.Length - 2;
		array[^1] = Code93Table.Length - 1;
		StringBuilder stringBuilder3 = new StringBuilder();
		for (int k = 0; k < array.Length; k++)
		{
			stringBuilder3.Append(CodeToBar(Code93Table[array[k]]));
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, base.Ratio, zoom, text, stringBuilder2.ToString(), stringBuilder3.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode93ExtBarCodeType();
	}

	public StiCode93ExtBarCodeType()
		: this(13f, 1f, 2.2f)
	{
	}

	public StiCode93ExtBarCodeType(float module, float height, float ratio)
		: base(module, height, ratio)
	{
	}
}
