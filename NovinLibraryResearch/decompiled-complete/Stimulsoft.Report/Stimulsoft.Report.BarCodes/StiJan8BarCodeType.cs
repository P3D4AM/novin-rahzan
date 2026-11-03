using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiEAN13BarCodeTypeConverter))]
public class StiJan8BarCodeType : StiEAN8BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiJan8BarCodeType;

	public override string ServiceName => "JAN-8";

	public override string DefaultCodeValue => "4523456789012";

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, false,
		false, true, false, true, false, false, false, true, false, true,
		true, false, false, false, false, false, false
	};

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

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		string text = CheckCodeSymbols(code, "0123456789") + new string('0', 13);
		string code2 = CheckCodeSymbols(SupplementCode, "0123456789") + new string('0', 5);
		if (!text.StartsWith("45", StringComparison.InvariantCulture) && !text.StartsWith("49", StringComparison.InvariantCulture))
		{
			DrawBarCodeError(context, rect, barCode);
			return;
		}
		List<EanBarInfo> list;
		if (SupplementType == StiEanSupplementType.None)
		{
			list = MakeEan8Bars(text, isLast: true);
		}
		else
		{
			list = MakeEan8Bars(text, isLast: false);
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
		return new StiJan8BarCodeType();
	}

	public StiJan8BarCodeType()
		: this(13f, 1f, StiEanSupplementType.None, null, showQuietZoneIndicator: true)
	{
	}

	public StiJan8BarCodeType(float module, float height, StiEanSupplementType supplementType, string supplementCodeValue, bool showQuietZoneIndicator)
		: base(module, height, supplementType, supplementCodeValue, showQuietZoneIndicator)
	{
	}
}
