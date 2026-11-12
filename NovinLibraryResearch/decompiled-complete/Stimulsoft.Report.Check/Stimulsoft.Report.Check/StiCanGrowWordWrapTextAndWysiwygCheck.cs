using System.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCanGrowWordWrapTextAndWysiwygCheck : StiComponentCheck
{
	public override bool DefaultStateEnabled => false;

	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiCanGrowWordWrapTextAndWysiwygCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiCanGrowWordWrapTextAndWysiwygCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiText { CanGrow: not false, WordWrap: not false, TextQuality: not StiTextQuality.Wysiwyg } stiText)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			if (stiText.GetPaintRectangle(convertToHInches: true, convertByZoom: true).Height > (double)(globalMeasureGraphics.MeasureString("p\ni", stiText.Font).Height + 2f))
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
				StiCanGrowWordWrapTextAndWysiwygCheck stiCanGrowWordWrapTextAndWysiwygCheck = new StiCanGrowWordWrapTextAndWysiwygCheck();
				stiCanGrowWordWrapTextAndWysiwygCheck.Element = obj;
				stiCanGrowWordWrapTextAndWysiwygCheck.Actions.Add(new StiCanGrowWordWrapTextAndWysiwygAction());
				return stiCanGrowWordWrapTextAndWysiwygCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
