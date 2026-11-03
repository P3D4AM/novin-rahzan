using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiCode128bBarCodeType : StiCode128BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode128bBarCodeType;

	public override string ServiceName => "Code128b";

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
		code = CheckCodeSymbols(code, " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u007f\u0081\u00a0¡¢£¤¥¦§\u00a8©ª«¬\u00ad®\u00af°±²³\u00b4µ¶·\u00b8¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ");
		char value = '∙';
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = code;
		foreach (char c in text)
		{
			if (c < '\u0080')
			{
				stringBuilder.Append((char)(c - 32));
				stringBuilder2.Append(c);
			}
			else if (c == '\u0081')
			{
				stringBuilder.Append('f');
				stringBuilder2.Append(value);
			}
			else
			{
				stringBuilder.Append('d');
				stringBuilder.Append((char)(c - 160));
				stringBuilder2.Append(c);
			}
		}
		code = stringBuilder.ToString();
		string textString = stringBuilder2.ToString();
		int[] array = new int[code.Length + 3];
		array[0] = 104;
		int num = array[0];
		for (int j = 0; j < code.Length; j++)
		{
			num += (array[j + 1] = code[j]) * (j + 1);
		}
		array[^2] = num % 103;
		array[^1] = 106;
		StringBuilder stringBuilder3 = new StringBuilder();
		for (int k = 0; k < array.Length; k++)
		{
			stringBuilder3.Append(CodeToBar(Code128Table[array[k]]));
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, 2f, zoom, code, textString, stringBuilder3.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode128bBarCodeType();
	}

	public StiCode128bBarCodeType()
		: this(13f, 1f)
	{
	}

	public StiCode128bBarCodeType(float module, float height)
		: base(module, height)
	{
	}
}
