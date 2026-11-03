using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.BarCodes.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[TypeConverter(typeof(StiEAN13BarCodeTypeConverter))]
public class StiIsbn13BarCodeType : StiEAN13BarCodeType
{
	protected const float IsbnOffsetY = 10f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiIsbn13BarCodeType;

	public override string ServiceName => "ISBN-13";

	public override string DefaultCodeValue => "978-0-7356-2153-4";

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
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		string code = GetCode(barCode);
		string code2 = CheckCodeSymbols(code, "0123456789") + new string('0', 13);
		string code3 = CheckCodeSymbols(SupplementCode, "0123456789") + new string('0', 5);
		List<EanBarInfo> list;
		if (SupplementType == StiEanSupplementType.None)
		{
			list = MakeEan13Bars(ref code2, isLast: true);
		}
		else
		{
			list = MakeEan13Bars(ref code2, isLast: false);
			if (SupplementType == StiEanSupplementType.TwoDigit)
			{
				MakeEanAdd2Bars(code3, list, isLast: true);
			}
			else
			{
				MakeEanAdd5Bars(code3, list, isLast: true);
			}
		}
		CalculateSizeEan(10f, zoom, list, rect, barCode);
		TranslateRect(context, rect, barCode);
		DrawEanBars(context, list, barCode);
		StiSolidBrush brush = new StiSolidBrush(barCode.ForeColor);
		bool flag = context is Graphics;
		float num = (flag ? 1f : 0.75000006f);
		StringFormat val = new StringFormat();
		try
		{
			Font val2 = new Font(StiFontCollection.GetFontFamily(barCode.Font.Name, false), barCode.Font.Size * (barCode.BarCodeType.LabelFontHeight / 8f) * base.BarCodeData.FullZoomY * num, barCode.Font.Style, (GraphicsUnit)(flag ? 2 : 3));
			try
			{
				string st = "ISBN " + code;
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)1;
				BaseDrawString(context, st, val2, (StiBrush)(object)brush, new RectangleF(0f, 0f, base.BarCodeData.MainWidth, base.BarCodeData.SpaceTextTop), val);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		RollbackTransform(context);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiIsbn13BarCodeType();
	}

	public StiIsbn13BarCodeType()
		: this(13f, 1f, StiEanSupplementType.None, null, showQuietZoneIndicator: true)
	{
	}

	public StiIsbn13BarCodeType(float module, float height, StiEanSupplementType supplementType, string supplementCodeValue, bool showQuietZoneIndicator)
		: base(module, height, supplementType, supplementCodeValue, showQuietZoneIndicator)
	{
	}
}
