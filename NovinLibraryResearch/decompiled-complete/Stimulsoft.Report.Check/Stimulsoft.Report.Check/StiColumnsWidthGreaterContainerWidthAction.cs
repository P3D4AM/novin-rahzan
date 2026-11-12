using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiColumnsWidthGreaterContainerWidthAction : StiAction
{
	public override string Name => StiLocalizationExt.Get("CheckActions", "Change");

	public override string Description => StiLocalizationExt.Get("CheckActions", "StiColumnsWidthGreaterContainerWidthActionLong");

	public override void Invoke(StiReport report, object element, string elementName)
	{
		base.Invoke(report, null, null);
		StiPage stiPage = element as StiPage;
		StiDataBand stiDataBand = element as StiDataBand;
		StiPanel stiPanel = element as StiPanel;
		if (stiPage != null)
		{
			double num = stiPage.Width - (double)stiPage.Columns * stiPage.ColumnGaps;
			if (num > 0.0)
			{
				stiPage.ColumnWidth = num / (double)stiPage.Columns;
				return;
			}
			stiPage.ColumnGaps = 0.0;
			stiPage.ColumnWidth = stiPage.Width / (double)stiPage.Columns;
		}
		else if (stiDataBand != null)
		{
			double num2 = stiDataBand.Width - (double)stiDataBand.Columns * stiDataBand.ColumnGaps;
			if (num2 > 0.0)
			{
				stiDataBand.ColumnWidth = num2 / (double)stiDataBand.Columns;
				return;
			}
			stiDataBand.ColumnGaps = 0.0;
			stiDataBand.ColumnWidth = stiDataBand.Width / (double)stiDataBand.Columns;
		}
		else if (stiPanel != null)
		{
			double num3 = stiPanel.Width - (double)stiPanel.Columns * stiPanel.ColumnGaps;
			if (num3 > 0.0)
			{
				stiPanel.ColumnWidth = num3 / (double)stiPanel.Columns;
				return;
			}
			stiPanel.ColumnGaps = 0.0;
			stiPanel.ColumnWidth = stiPanel.Width / (double)stiPanel.Columns;
		}
	}
}
