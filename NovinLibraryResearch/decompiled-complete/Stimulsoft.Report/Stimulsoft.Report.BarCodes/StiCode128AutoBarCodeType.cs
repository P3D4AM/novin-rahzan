using System.ComponentModel;
using System.Drawing;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiCode128AutoBarCodeType : StiCode128BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCode128AutoBarCodeType;

	public override string ServiceName => "Code128 Auto";

	public override string DefaultCodeValue => "1234567890123";

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
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int num in code)
		{
			if ((num >= 32 && num < 129) || (num > 135 && num <= 255))
			{
				stringBuilder.Append((char)num);
			}
			else if (num < 133 || num > 135)
			{
				stringBuilder.Append('∙');
			}
		}
		string text = StiCode128BarCodeType.EncodeAuto(code, encodeAsEan: false);
		int[] array = new int[text.Length];
		for (int j = 0; j < text.Length; j++)
		{
			array[j] = text[j];
		}
		int num2 = array[0];
		for (int k = 0; k < text.Length - 3; k++)
		{
			num2 += array[k + 1] * (k + 1);
		}
		array[^2] = num2 % 103;
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int l = 0; l < array.Length; l++)
		{
			stringBuilder2.Append(CodeToBar(Code128Table[array[l]]));
		}
		string textString = stringBuilder.ToString();
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, 2f, zoom, code, textString, stringBuilder2.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiCode128AutoBarCodeType();
	}

	public StiCode128AutoBarCodeType()
		: this(13f, 1f)
	{
	}

	public StiCode128AutoBarCodeType(float module, float height)
		: base(module, height)
	{
	}
}
