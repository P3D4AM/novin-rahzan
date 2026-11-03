using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiEAN8BarCodeType : StiEAN13BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiEAN8BarCodeType;

	public override string ServiceName => "EAN-8";

	public override string DefaultCodeValue => "12345678";

	protected override float EanSpaceLeft => 7f;

	protected override float EanSpaceRight => 7f;

	protected override float EanLineHeightShort => 55.2f;

	protected override float EanMainHeight => 64.58f;

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.SupplementCode(),
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.ShowQuietZoneIndicator(),
			propertiesHelper.SupplementType()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	protected List<EanBarInfo> MakeEan8Bars(string code, bool isLast)
	{
		int[] array = new int[8];
		for (int i = 0; i < 7; i++)
		{
			array[i] = int.Parse(code[i].ToString());
		}
		int num = (array[0] + array[2] + array[4] + array[6]) * 3 + array[1] + array[3] + array[5];
		int num2 = 10 - num % 10;
		if (num2 == 10)
		{
			num2 = 0;
		}
		code = code.Substring(0, 7) + (char)(num2 + 48);
		List<EanBarInfo> list = new List<EanBarInfo>
		{
			new EanBarInfo(Ean13Symbol.SpaceLeft, ShowQuietZoneIndicator ? '<' : ' ', textAtTop: false),
			new EanBarInfo(Ean13Symbol.GuardLeft, ' ', textAtTop: false)
		};
		for (int j = 0; j < 4; j++)
		{
			int symbolType = int.Parse(code[j].ToString());
			EanBarInfo item = new EanBarInfo((Ean13Symbol)symbolType, code[j], textAtTop: false);
			list.Add(item);
		}
		list.Add(new EanBarInfo(Ean13Symbol.GuardCenter, ' ', textAtTop: false));
		for (int k = 0; k < 4; k++)
		{
			int num3 = int.Parse(code[4 + k].ToString());
			EanBarInfo item2 = new EanBarInfo((Ean13Symbol)(20 + num3), code[4 + k], textAtTop: false);
			list.Add(item2);
		}
		list.Add(new EanBarInfo(Ean13Symbol.GuardRight, ' ', textAtTop: false));
		if (isLast)
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceRight, ShowQuietZoneIndicator ? '>' : ' ', textAtTop: false));
		}
		else
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceBeforeAdd, ' ', textAtTop: false));
		}
		return list;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, "0123456789") + new string('0', 8);
		string code2 = CheckCodeSymbols(SupplementCode, "0123456789") + new string('0', 5);
		List<EanBarInfo> list;
		if (SupplementType == StiEanSupplementType.None)
		{
			list = MakeEan8Bars(code, isLast: true);
		}
		else
		{
			list = MakeEan8Bars(code, isLast: false);
			if (SupplementType == StiEanSupplementType.TwoDigit)
			{
				MakeEanAdd2Bars(code2, list, isLast: true);
			}
			else
			{
				MakeEanAdd5Bars(code2, list, isLast: true);
			}
		}
		CalculateSizeEan(0f, zoom, list, rect, barCode);
		TranslateRect(context, rect, barCode);
		DrawEanBars(context, list, barCode);
		RollbackTransform(context);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiEAN8BarCodeType();
	}

	public StiEAN8BarCodeType()
		: this(13f, 1f, StiEanSupplementType.None, null, showQuietZoneIndicator: true)
	{
	}

	public StiEAN8BarCodeType(float module, float height, StiEanSupplementType supplementType, string supplementCodeValue, bool showQuietZoneIndicator)
		: base(module, height, supplementType, supplementCodeValue, showQuietZoneIndicator)
	{
	}
}
