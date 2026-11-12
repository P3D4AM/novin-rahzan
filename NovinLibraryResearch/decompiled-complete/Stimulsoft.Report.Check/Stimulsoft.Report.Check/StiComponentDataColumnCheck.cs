using System.Collections.Generic;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiComponentDataColumnCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiComponentExpressionCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiComponentExpressionCheckLong"), "DataColumn", ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiImage { DataColumn: not null } stiImage && stiImage.DataColumn.Length > 0)
		{
			try
			{
				bool storeToPrint = false;
				object obj = StiParser.ParseTextValue("{" + stiImage.DataColumn + "}", stiImage, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true);
				if (obj is List<StiParser.StiAsmCommand> { Count: >1 })
				{
					return true;
				}
			}
			catch
			{
				return true;
			}
		}
		if (base.Element is StiRichText { DataColumn: not null } stiRichText && stiRichText.DataColumn.Length > 0)
		{
			try
			{
				bool storeToPrint2 = false;
				object obj3 = StiParser.ParseTextValue("{" + stiRichText.DataColumn + "}", stiRichText, ref storeToPrint2, executeIfStoreToPrint: false, returnAsmList: true);
				if (obj3 is List<StiParser.StiAsmCommand> { Count: >1 })
				{
					return true;
				}
			}
			catch
			{
				return true;
			}
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (Check())
			{
				StiComponentDataColumnCheck stiComponentDataColumnCheck = new StiComponentDataColumnCheck();
				stiComponentDataColumnCheck.Element = obj;
				return stiComponentDataColumnCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
