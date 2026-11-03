using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiCode128cBarCodeType : StiCode128BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode128cBarCodeType;

	public override string ServiceName => "Code128c";

	public override string DefaultCodeValue => "0123456789012345";

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
		code = CheckCodeSymbols(code, "0123456789\u0081");
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		string text = string.Empty;
		string text2 = code;
		for (int i = 0; i < text2.Length; i++)
		{
			char c = text2[i];
			if (c == '\u0081')
			{
				list.Add(text);
				text = string.Empty;
				stringBuilder.Append('∙');
			}
			else
			{
				text += c;
				stringBuilder.Append(c);
			}
		}
		if (text.Length > 0)
		{
			list.Add(text);
		}
		if (list.Count == 0)
		{
			list.Add(text);
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int j = 0; j < list.Count; j++)
		{
			string text3 = list[j];
			int num = text3.Length / 2;
			for (int k = 0; k < num; k++)
			{
				stringBuilder2.Append((char)int.Parse(text3.Substring(k * 2, 2)));
			}
			if (text3.Length % 2 == 1)
			{
				stringBuilder2.Append('d');
				stringBuilder2.Append((char)(text3[text3.Length - 1] - 32));
				if (j < list.Count - 1)
				{
					stringBuilder2.Append('c');
				}
			}
			if (j < list.Count - 1)
			{
				stringBuilder2.Append('f');
			}
		}
		int[] array = new int[stringBuilder2.Length + 3];
		array[0] = 105;
		for (int l = 0; l < stringBuilder2.Length; l++)
		{
			array[l + 1] = stringBuilder2[l];
		}
		int num2 = array[0];
		for (int m = 0; m < array.Length - 3; m++)
		{
			num2 += array[m + 1] * (m + 1);
		}
		array[^2] = num2 % 103;
		array[^1] = 106;
		StringBuilder stringBuilder3 = new StringBuilder();
		for (int n = 0; n < array.Length; n++)
		{
			stringBuilder3.Append(CodeToBar(Code128Table[array[n]]));
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, 2f, zoom, code, stringBuilder.ToString(), stringBuilder3.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode128cBarCodeType();
	}

	public StiCode128cBarCodeType()
		: this(13f, 1f)
	{
	}

	public StiCode128cBarCodeType(float module, float height)
		: base(module, height)
	{
	}
}
