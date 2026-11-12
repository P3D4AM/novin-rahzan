using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNoNameInSourceDataSourceCheck : StiDataSourceCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataSource", "StiNoNameInSourceDataSourceCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataSource", "StiNoNameInSourceDataSourceCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataStoreSource stiDataStoreSource && string.IsNullOrEmpty(stiDataStoreSource.NameInSource))
		{
			StiNoNameInSourceDataSourceCheck stiNoNameInSourceDataSourceCheck = new StiNoNameInSourceDataSourceCheck();
			stiNoNameInSourceDataSourceCheck.Actions.Add(new StiDeleteDataSourceAction());
			stiNoNameInSourceDataSourceCheck.Element = obj;
			return stiNoNameInSourceDataSourceCheck;
		}
		return null;
	}
}
