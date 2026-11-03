using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public class StiOddEvenStylesHelper
{
	public static StiBaseStyle ApplyOddEvenStyles(StiReport report, IStiOddEvenStyles styles, StiContainer cont)
	{
		if (report == null || report.Styles.Count <= 0)
		{
			return null;
		}
		if (styles == null)
		{
			return null;
		}
		StiBaseStyle result = null;
		StiComponent stiComponent = styles as StiComponent;
		bool flag = true;
		if (((StiService)stiComponent).IsPropertyPresent(StiDataBand.PropertyEvenStyle) && ((styles is StiDataBand) ? ((((StiDataBand)styles).Position & 1) == 1) : ((report.LineThrough & 1) == 0)))
		{
			string evenStyle = styles.EvenStyle;
			if (!string.IsNullOrEmpty(evenStyle))
			{
				StiBaseStyle stiBaseStyle = report.Styles[evenStyle];
				if (stiBaseStyle != null)
				{
					stiBaseStyle.SetStyleToComponent(cont);
					result = stiBaseStyle;
					flag = false;
				}
			}
		}
		if (flag && ((StiService)stiComponent).IsPropertyPresent(StiDataBand.PropertyOddStyle) && !((styles is StiDataBand) ? ((((StiDataBand)styles).Position & 1) == 1) : ((report.LineThrough & 1) == 0)))
		{
			string oddStyle = styles.OddStyle;
			if (!string.IsNullOrEmpty(oddStyle))
			{
				StiBaseStyle stiBaseStyle2 = report.Styles[oddStyle];
				if (stiBaseStyle2 != null)
				{
					stiBaseStyle2.SetStyleToComponent(cont);
					result = stiBaseStyle2;
				}
			}
		}
		return result;
	}
}
