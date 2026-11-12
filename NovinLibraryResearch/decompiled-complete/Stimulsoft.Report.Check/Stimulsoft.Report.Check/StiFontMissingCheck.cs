using System.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiFontMissingCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiFontMissingCheckShort");

	public override string LongMessage
	{
		get
		{
			Font val = ((base.Element is IStiFont stiFont) ? stiFont.Font : null);
			string arg = ((val != null) ? val.OriginalFontName : string.Empty);
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiFontMissingCheckLong"), arg, ElementName);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is IStiFont stiFont && !string.IsNullOrEmpty(stiFont.Font.OriginalFontName) && stiFont.Font.Name != stiFont.Font.OriginalFontName)
		{
			return true;
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
				StiFontMissingCheck stiFontMissingCheck = new StiFontMissingCheck();
				stiFontMissingCheck.Element = obj;
				return stiFontMissingCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
