using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiEAN128bBarCodeType : StiCode128BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiEAN128bBarCodeType;

	public override string ServiceName => "EAN-128b";

	public override string DefaultCodeValue => "ABCabc123";

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.fHeight(),
			propertiesHelper.Module()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u007f\u0081");
		StringBuilder stringBuilder = new StringBuilder();
		int[] array = new int[code.Length + 4];
		array[0] = 104;
		array[1] = 102;
		int num = array[0] + array[1];
		for (int i = 0; i < code.Length; i++)
		{
			int num2 = code[i];
			array[i + 2] = num2 - 32;
			if (num2 == 129)
			{
				array[i + 2] = 102;
			}
			num += array[i + 2] * (i + 2);
			stringBuilder.Append((num2 != 129) ? ((char)num2) : '∙');
		}
		array[^2] = num % 103;
		array[^1] = 106;
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int j = 0; j < array.Length; j++)
		{
			stringBuilder2.Append(CodeToBar(Code128Table[array[j]]));
		}
		string textString = stringBuilder.ToString();
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, 2f, zoom, code, textString, stringBuilder2.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiEAN128bBarCodeType();
	}

	public StiEAN128bBarCodeType()
		: this(13f, 1f)
	{
	}

	public StiEAN128bBarCodeType(float module, float height)
		: base(module, height)
	{
	}
}
