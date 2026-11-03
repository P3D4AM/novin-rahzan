using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiUpcSup2BarCodeType : StiEAN13BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiUpcSup2BarCodeType;

	public override string ServiceName => "UPC-Supp2";

	public override string DefaultCodeValue => "02";

	public override bool[] VisibleProperties
	{
		get
		{
			bool[] array = new bool[27];
			array[11] = true;
			array[13] = true;
			array[17] = true;
			return array;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiEanSupplementType SupplementType
	{
		get
		{
			return base.SupplementType;
		}
		set
		{
			base.SupplementType = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override string SupplementCode
	{
		get
		{
			return base.SupplementCode;
		}
		set
		{
			base.SupplementCode = value;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.ShowQuietZoneIndicator()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = CheckCodeSymbols(code, "0123456789") + new string('0', 2);
		List<EanBarInfo> barsArray = MakeEanAdd2Bars(code, null, isLast: true);
		CalculateSizeEan(0f, zoom, barsArray, rect, barCode);
		TranslateRect(context, rect, barCode);
		DrawEanBars(context, barsArray, barCode);
		RollbackTransform(context);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiUpcSup2BarCodeType();
	}

	public StiUpcSup2BarCodeType()
		: this(13f, 1f, StiEanSupplementType.None, null, showQuietZoneIndicator: true)
	{
	}

	public StiUpcSup2BarCodeType(float module, float height, StiEanSupplementType supplementType, string supplementCodeValue, bool showQuietZoneIndicator)
		: base(module, height, supplementType, supplementCodeValue, showQuietZoneIndicator)
	{
	}
}
