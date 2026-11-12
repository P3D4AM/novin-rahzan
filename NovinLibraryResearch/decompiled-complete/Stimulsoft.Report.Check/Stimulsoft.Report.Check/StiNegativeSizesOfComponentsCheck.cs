using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNegativeSizesOfComponentsCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiNegativeSizesOfComponentsCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiNegativeSizesOfComponentsCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiComponent stiComponent && (stiComponent.Width < 0.0 || stiComponent.Height < 0.0))
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
				StiNegativeSizesOfComponentsCheck stiNegativeSizesOfComponentsCheck = new StiNegativeSizesOfComponentsCheck();
				stiNegativeSizesOfComponentsCheck.Element = obj;
				stiNegativeSizesOfComponentsCheck.Actions.Add(new StiNegativeSizesOfComponentsAction());
				return stiNegativeSizesOfComponentsCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
