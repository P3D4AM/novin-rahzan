using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiFilterCircularDependencyElementCheck : StiComponentCheck
{
	public override bool DefaultStateEnabled => true;

	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiFilterCircularDependencyElementCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiFilterCircularDependencyElementCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		return StiCrossLinkedFilterHelper.IsCrossLinkedFilter(base.Element as IStiFilterElement);
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			return Check() ? new StiFilterCircularDependencyElementCheck
			{
				Element = obj
			} : null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
