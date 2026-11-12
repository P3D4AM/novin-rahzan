using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiFilterValueCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiFilterValueCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiFilterValueCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (!(base.Element is IStiFilter { FilterOn: not false } stiFilter))
		{
			return false;
		}
		foreach (StiFilter filter in stiFilter.Filters)
		{
			if ((filter.DataType == StiFilterDataType.Numeric || filter.DataType == StiFilterDataType.Expression) && filter.Item == StiFilterItem.Value && string.IsNullOrWhiteSpace(filter.Value1) && filter.Condition != StiFilterCondition.IsNull && filter.Condition != StiFilterCondition.IsNotNull)
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
				StiFilterValueCheck stiFilterValueCheck = new StiFilterValueCheck();
				stiFilterValueCheck.Element = obj;
				return stiFilterValueCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
