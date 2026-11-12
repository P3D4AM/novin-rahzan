using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiColumnsWidthGreaterPageWidthCheck : StiPageCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiColumnsWidthGreaterPageWidthCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiColumnsWidthGreaterPageWidthCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiPage { Columns: >0 } stiPage)
		{
			double num = stiPage.ColumnWidth * (double)stiPage.Columns + stiPage.ColumnGaps * (double)(stiPage.Columns - 1);
			if (num * 0.99 > stiPage.Width)
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
				StiColumnsWidthGreaterPageWidthCheck stiColumnsWidthGreaterPageWidthCheck = new StiColumnsWidthGreaterPageWidthCheck();
				stiColumnsWidthGreaterPageWidthCheck.Element = obj;
				stiColumnsWidthGreaterPageWidthCheck.Actions.Add(new StiColumnsWidthGreaterContainerWidthAction());
				return stiColumnsWidthGreaterPageWidthCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
