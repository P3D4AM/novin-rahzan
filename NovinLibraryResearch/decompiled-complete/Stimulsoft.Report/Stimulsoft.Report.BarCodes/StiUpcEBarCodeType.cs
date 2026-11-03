using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiUpcEBarCodeType : StiEAN13BarCodeType
{
	protected string[] symParitySet = new string[10] { "eeeooo", "eeoeoo", "eeooeo", "eeoooe", "eoeeoo", "eooeeo", "eoooee", "eoeoeo", "eoeooe", "eooeoe" };

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiUpcEBarCodeType;

	public override string ServiceName => "UPC-E";

	protected override float EanSpaceLeft => 9f;

	protected override float EanSpaceRight => 7f;

	public override string DefaultCodeValue => "01234567";

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, false,
		false, true, false, true, false, false, false, false, false, true,
		true, false, false, false, false, false, false
	};

	[Browsable(false)]
	[StiNonSerialized]
	public override bool ShowQuietZoneIndicator
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.SupplementCode(),
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.SupplementType()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	protected List<EanBarInfo> MakeUpcEBars(string code, bool isLast)
	{
		List<EanBarInfo> list = new List<EanBarInfo>
		{
			new EanBarInfo(Ean13Symbol.SpaceLeft, code[0], textAtTop: false),
			new EanBarInfo(Ean13Symbol.GuardLeft, ' ', textAtTop: false)
		};
		string text = symParitySet[int.Parse(code[7].ToString())];
		for (int i = 0; i < 6; i++)
		{
			int num = int.Parse(code[1 + i].ToString());
			char c = text[i];
			if (code[0] != '0')
			{
				c = ((c != 'o') ? 'o' : 'e');
			}
			EanBarInfo eanBarInfo = new EanBarInfo((Ean13Symbol)num, code[1 + i], textAtTop: false);
			if (c != 'o')
			{
				eanBarInfo.SymbolType = (Ean13Symbol)(10 + num);
			}
			list.Add(eanBarInfo);
		}
		list.Add(new EanBarInfo(Ean13Symbol.GuardSpecial, ' ', textAtTop: false));
		list.Add(new EanBarInfo(isLast ? Ean13Symbol.SpaceRight : Ean13Symbol.SpaceBeforeAdd, code[7], textAtTop: false));
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
			list = MakeUpcEBars(code, isLast: true);
		}
		else
		{
			list = MakeUpcEBars(code, isLast: false);
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
		return new StiUpcEBarCodeType();
	}

	public StiUpcEBarCodeType()
		: this(13f, 1f, StiEanSupplementType.None, null, showQuietZoneIndicator: false)
	{
	}

	public StiUpcEBarCodeType(float module, float height, StiEanSupplementType supplementType, string supplementCodeValue, bool showQuietZoneIndicator)
		: base(module, height, supplementType, supplementCodeValue, showQuietZoneIndicator)
	{
	}
}
