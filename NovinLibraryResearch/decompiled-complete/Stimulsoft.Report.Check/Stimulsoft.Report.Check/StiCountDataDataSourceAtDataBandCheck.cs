using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiCountDataDataSourceAtDataBandCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiCountDataDataSourceAtDataBandShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiCountDataDataSourceAtDataBandLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Information;

	private bool Check()
	{
		if (base.Element is StiDataBand { CountData: 0, DataSource: null, BusinessObject: null })
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
				StiCountDataDataSourceAtDataBandCheck stiCountDataDataSourceAtDataBandCheck = new StiCountDataDataSourceAtDataBandCheck();
				stiCountDataDataSourceAtDataBandCheck.Element = obj;
				return stiCountDataDataSourceAtDataBandCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
