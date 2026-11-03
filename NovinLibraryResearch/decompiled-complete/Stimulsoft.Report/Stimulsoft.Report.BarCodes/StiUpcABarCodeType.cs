using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiUpcABarCodeType : StiEAN13BarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiUpcABarCodeType;

	public override string ServiceName => "UPC-A";

	protected override float EanSpaceLeft => 9f;

	protected override float EanSpaceRight => 9f;

	public override string DefaultCodeValue => "0123456789012";

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

	protected List<EanBarInfo> MakeUpcABars(string code, bool isLast)
	{
		int[] array = new int[12];
		for (int i = 0; i < 12; i++)
		{
			array[i] = int.Parse(code[i].ToString());
		}
		int num = (array[1] + array[3] + array[5] + array[7] + array[9] + array[11]) * 3 + array[0] + array[2] + array[4] + array[6] + array[8] + array[10];
		int num2 = 10 - num % 10;
		if (num2 == 10)
		{
			num2 = 0;
		}
		code = code.Substring(0, 12) + (char)(num2 + 48);
		List<EanBarInfo> list = new List<EanBarInfo>
		{
			new EanBarInfo(Ean13Symbol.SpaceLeft, code[1], textAtTop: false),
			new EanBarInfo(Ean13Symbol.GuardLeft, ' ', textAtTop: false),
			new EanBarInfo((Ean13Symbol)int.Parse(code[1].ToString()), ' ', textAtTop: false, makeLonger: true)
		};
		for (int j = 0; j < 5; j++)
		{
			int symbolType = int.Parse(code[2 + j].ToString());
			EanBarInfo item = new EanBarInfo((Ean13Symbol)symbolType, code[2 + j], textAtTop: false);
			list.Add(item);
		}
		list.Add(new EanBarInfo(Ean13Symbol.GuardCenter, ' ', textAtTop: false));
		for (int k = 0; k < 5; k++)
		{
			int num3 = int.Parse(code[7 + k].ToString());
			EanBarInfo item2 = new EanBarInfo((Ean13Symbol)(20 + num3), code[7 + k], textAtTop: false);
			list.Add(item2);
		}
		list.Add(new EanBarInfo((Ean13Symbol)(20 + int.Parse(code[12].ToString())), ' ', textAtTop: false, makeLonger: true));
		list.Add(new EanBarInfo(Ean13Symbol.GuardRight, ' ', textAtTop: false));
		if (isLast)
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceRight, code[12], textAtTop: false));
		}
		else
		{
			list.Add(new EanBarInfo(Ean13Symbol.SpaceBeforeAdd, code[12], textAtTop: false));
		}
		return list;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		code = "0" + CheckCodeSymbols(code, "0123456789") + new string('0', 12);
		string code2 = CheckCodeSymbols(SupplementCode, "0123456789") + new string('0', 5);
		List<EanBarInfo> list;
		if (SupplementType == StiEanSupplementType.None)
		{
			list = MakeUpcABars(code, isLast: true);
		}
		else
		{
			list = MakeUpcABars(code, isLast: false);
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
		return new StiUpcABarCodeType();
	}

	public StiUpcABarCodeType()
		: this(13f, 1f, StiEanSupplementType.None, null, showQuietZoneIndicator: false)
	{
	}

	public StiUpcABarCodeType(float module, float height, StiEanSupplementType supplementType, string supplementCodeValue, bool showQuietZoneIndicator)
		: base(module, height, supplementType, supplementCodeValue, showQuietZoneIndicator)
	{
	}
}
