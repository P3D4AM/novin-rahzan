using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNoNameDataSourceCheck : StiDataSourceCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataSource", "StiNoNameDataSourceCheckShort"));

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataSource", "StiNoNameDataSourceCheckLong"));

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataSource stiDataSource && string.IsNullOrEmpty(stiDataSource.Name))
		{
			StiNoNameDataSourceCheck stiNoNameDataSourceCheck = new StiNoNameDataSourceCheck();
			stiNoNameDataSourceCheck.Element = obj;
			stiNoNameDataSourceCheck.Actions.Add(new StiGenerateNewNameDataSourceAction());
			stiNoNameDataSourceCheck.Actions.Add(new StiDeleteDataSourceAction());
			return stiNoNameDataSourceCheck;
		}
		return null;
	}
}
