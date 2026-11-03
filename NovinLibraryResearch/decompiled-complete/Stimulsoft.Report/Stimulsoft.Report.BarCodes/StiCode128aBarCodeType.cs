using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiCode128aBarCodeType : StiCode128BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode128aBarCodeType;

	public override string ServiceName => "Code128a";

	public override string DefaultCodeValue => "ABC123";

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
		code = CheckCodeSymbols(code, " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\v\f\r\u000e\u000f\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001a\u001b\u001c\u001d\u001e\u001f\u0080\u0081\u0082\u0083\u0084\u0085\u0086\u0087\u0088\u0089\u008a\u008b\u008c\u008d\u008e\u008f\u0090\u0091\u0092\u0093\u0094\u0095\u0096\u0097\u0098\u0099\u009a\u009b\u009c\u009d\u009e\u009f\u00a0¡¢£¤¥¦§\u00a8©ª«¬\u00ad®\u00af°±²³\u00b4µ¶·\u00b8¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞß");
		char c = '∙';
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = code;
		foreach (char c2 in text)
		{
			if (c2 < '\u0080')
			{
				stringBuilder.Append((char)((c2 >= ' ') ? (c2 - 32) : (c2 + 64)));
				stringBuilder2.Append((c2 >= ' ') ? c2 : c);
			}
			else if (c2 == '\u0081')
			{
				stringBuilder.Append('f');
				stringBuilder2.Append(c);
			}
			else
			{
				stringBuilder.Append('e');
				stringBuilder.Append((char)((c2 >= '\u00a0') ? (c2 - 160) : (c2 - 64)));
				stringBuilder2.Append((c2 >= '\u00a0') ? c2 : c);
			}
		}
		code = stringBuilder.ToString();
		string textString = stringBuilder2.ToString();
		int[] array = new int[code.Length + 3];
		array[0] = 103;
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
		return new StiCode128aBarCodeType();
	}

	public StiCode128aBarCodeType()
		: this(13f, 1f)
	{
	}

	public StiCode128aBarCodeType(float module, float height)
		: base(module, height)
	{
	}
}
