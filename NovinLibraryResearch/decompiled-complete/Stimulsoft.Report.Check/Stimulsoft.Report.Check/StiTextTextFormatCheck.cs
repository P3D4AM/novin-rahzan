using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiTextTextFormatCheck : StiComponentCheck
{
	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiTextTextFormatCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiTextTextFormatCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiText stiText && !(stiText.TextFormat is StiGeneralFormatService))
		{
			string value = stiText.Text.Value;
			if (!string.IsNullOrEmpty(value) && value.Contains("{") && value.Contains("}") && (!value.StartsWith("{") || !value.EndsWith("}")))
			{
				return true;
			}
			if (!string.IsNullOrEmpty(value) && value.Contains("{") && value.Contains("}") && value.Contains("ToString"))
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
				StiTextTextFormatCheck stiTextTextFormatCheck = new StiTextTextFormatCheck();
				stiTextTextFormatCheck.Element = obj;
				stiTextTextFormatCheck.Actions.Add(new StiApplyGeneralTextFormat());
				return stiTextTextFormatCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
