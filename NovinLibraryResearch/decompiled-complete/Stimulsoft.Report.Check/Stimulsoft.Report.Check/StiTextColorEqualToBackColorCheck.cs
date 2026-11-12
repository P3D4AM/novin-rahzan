using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiTextColorEqualToBackColorCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiTextColorEqualToBackColorCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiTextColorEqualToBackColorCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiText stiText)
		{
			if (stiText.Brush is StiEmptyBrush && stiText.TextBrush is StiEmptyBrush)
			{
				return true;
			}
			if (stiText.Brush is StiSolidBrush && stiText.TextBrush is StiSolidBrush)
			{
				if (((StiSolidBrush)stiText.Brush).Color == ((StiSolidBrush)stiText.TextBrush).Color)
				{
					return true;
				}
				return false;
			}
			if (stiText.Brush is StiHatchBrush && stiText.TextBrush is StiHatchBrush)
			{
				if (((StiHatchBrush)stiText.Brush).ForeColor == ((StiHatchBrush)stiText.TextBrush).ForeColor && ((StiHatchBrush)stiText.Brush).BackColor == ((StiHatchBrush)stiText.TextBrush).BackColor)
				{
					return true;
				}
				return false;
			}
			if (stiText.Brush is StiGradientBrush && stiText.TextBrush is StiGradientBrush)
			{
				if (((StiGradientBrush)stiText.Brush).StartColor == ((StiGradientBrush)stiText.TextBrush).StartColor && ((StiGradientBrush)stiText.Brush).EndColor == ((StiGradientBrush)stiText.TextBrush).EndColor)
				{
					return true;
				}
				return false;
			}
			if (stiText.Brush is StiGlareBrush && stiText.TextBrush is StiGlareBrush)
			{
				if (((StiGlareBrush)stiText.Brush).StartColor == ((StiGlareBrush)stiText.TextBrush).StartColor && ((StiGlareBrush)stiText.Brush).EndColor == ((StiGlareBrush)stiText.TextBrush).EndColor)
				{
					return true;
				}
				return false;
			}
			if (stiText.Brush is StiGlassBrush && stiText.TextBrush is StiGlassBrush && ((StiGlassBrush)stiText.Brush).Color == ((StiGlassBrush)stiText.TextBrush).Color)
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
				StiTextColorEqualToBackColorCheck stiTextColorEqualToBackColorCheck = new StiTextColorEqualToBackColorCheck();
				stiTextColorEqualToBackColorCheck.Element = obj;
				return stiTextColorEqualToBackColorCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
