using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiColumnsWidthGreaterContainerWidthCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiColumnsWidthGreaterContainerWidthCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiColumnsWidthGreaterContainerWidthCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		StiDataBand stiDataBand = base.Element as StiDataBand;
		StiPanel stiPanel = base.Element as StiPanel;
		if (stiDataBand != null && stiDataBand.Columns > 0)
		{
			double num = stiDataBand.ColumnWidth * (double)stiDataBand.Columns + stiDataBand.ColumnGaps * (double)(stiDataBand.Columns - 1);
			if (num > stiDataBand.Width)
			{
				return true;
			}
		}
		else if (stiPanel != null && stiPanel.Columns > 0)
		{
			double num2 = stiPanel.ColumnWidth * (double)stiPanel.Columns + stiPanel.ColumnGaps * (double)(stiPanel.Columns - 1);
			if (num2 > stiPanel.Width)
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
				StiColumnsWidthGreaterContainerWidthCheck stiColumnsWidthGreaterContainerWidthCheck = new StiColumnsWidthGreaterContainerWidthCheck();
				stiColumnsWidthGreaterContainerWidthCheck.Element = obj;
				stiColumnsWidthGreaterContainerWidthCheck.Actions.Add(new StiColumnsWidthGreaterContainerWidthAction());
				return stiColumnsWidthGreaterContainerWidthCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
