using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiCode39ExtBarCodeType : StiCode39BarCodeType
{
	protected string[] Code39ExtTable = new string[128]
	{
		"%U", "$A", "$B", "$C", "$D", "$E", "$F", "$G", "$H", "$I",
		"$J", "$K", "$L", "$M", "$N", "$O", "$P", "$Q", "$R", "$S",
		"$T", "$U", "$V", "$W", "$X", "$Y", "$Z", "%A", "%B", "%C",
		"%D", "%E", " ", "/A", "/B", "/C", "/D", "/E", "/F", "/G",
		"/H", "/I", "/J", "/K", "/L", "-", ".", "/O", "0", "1",
		"2", "3", "4", "5", "6", "7", "8", "9", "/Z", "%F",
		"%G", "%H", "%I", "%J", "%V", "A", "B", "C", "D", "E",
		"F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
		"P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y",
		"Z", "%K", "%L", "%M", "%N", "%O", "%W", "+A", "+B", "+C",
		"+D", "+E", "+F", "+G", "+H", "+I", "+J", "+K", "+L", "+M",
		"+N", "+O", "+P", "+Q", "+R", "+S", "+T", "+U", "+V", "+W",
		"+X", "+Y", "+Z", "%P", "%Q", "%R", "%S", "%T"
	};

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode39ExtBarCodeType;

	public override string ServiceName => "Code39 Extended";

	public override string DefaultCodeValue => "Abc123";

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.CheckSum(),
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
				stringBuilder.Append(Code39ExtTable[num]);
				stringBuilder2.Append((char)num);
			}
		}
		text = stringBuilder.ToString();
		int[] array = new int[text.Length + ((base.CheckSum == StiCheckSum.Yes) ? 3 : 2)];
		array[0] = Code39StartStopSymbolIndex;
		int num2 = 0;
		for (int j = 0; j < text.Length; j++)
		{
			array[j + 1] = Code39Symbols.IndexOf(text[j]);
			num2 += array[j + 1];
		}
		if (base.CheckSum == StiCheckSum.Yes)
		{
			array[^2] = num2 % 43;
		}
		array[^1] = array[0];
		StringBuilder stringBuilder3 = new StringBuilder();
		for (int k = 0; k < array.Length; k++)
		{
			stringBuilder3.Append(CodeToBar(Code39Table[array[k]]));
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, base.Ratio, zoom, text, stringBuilder2.ToString(), stringBuilder3.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode39ExtBarCodeType();
	}

	public StiCode39ExtBarCodeType()
		: this(13f, 1f, 2.2f, StiCheckSum.Yes)
	{
	}

	public StiCode39ExtBarCodeType(float module, float height, float ratio, StiCheckSum checkSum)
		: base(module, height, ratio, checkSum)
	{
	}
}
