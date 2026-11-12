using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiComponentStyleIsNotFoundAtComponentCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiComponentStyleIsNotFoundCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiComponentStyleIsNotFoundCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiComponent stiComponent)
		{
			if (stiComponent.ComponentStyle != string.Empty && !stiComponent.Report.Styles.Contains(stiComponent.ComponentStyle))
			{
				return true;
			}
			if (stiComponent is IStiOddEvenStyles stiOddEvenStyles)
			{
				if (stiOddEvenStyles.EvenStyle != string.Empty && !stiComponent.Report.Styles.Contains(stiOddEvenStyles.EvenStyle))
				{
					return true;
				}
				if (stiOddEvenStyles.OddStyle != string.Empty && !stiComponent.Report.Styles.Contains(stiOddEvenStyles.OddStyle))
				{
					return true;
				}
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
				StiComponentStyleIsNotFoundAtComponentCheck stiComponentStyleIsNotFoundAtComponentCheck = new StiComponentStyleIsNotFoundAtComponentCheck();
				stiComponentStyleIsNotFoundAtComponentCheck.Element = obj;
				stiComponentStyleIsNotFoundAtComponentCheck.Actions.Add(new StiComponentStyleIsNotFoundAtComponentAction());
				return stiComponentStyleIsNotFoundAtComponentCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
