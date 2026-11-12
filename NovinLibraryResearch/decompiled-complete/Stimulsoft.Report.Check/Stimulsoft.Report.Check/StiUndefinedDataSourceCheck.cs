using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiUndefinedDataSourceCheck : StiDataSourceCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataSource", "StiUndefinedDataSourceCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataSource", "StiUndefinedDataSourceCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiUndefinedDataSource)
		{
			StiUndefinedDataSourceCheck stiUndefinedDataSourceCheck = new StiUndefinedDataSourceCheck();
			stiUndefinedDataSourceCheck.Element = obj;
			stiUndefinedDataSourceCheck.Actions.Add(new StiDeleteDataSourceAction());
			return stiUndefinedDataSourceCheck;
		}
		return null;
	}
}
