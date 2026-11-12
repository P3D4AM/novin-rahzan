using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiUndefinedConnectionCheck : StiConnectionCheck
{
	private StiReport report;

	public override bool PreviewVisible => false;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckConnection", "StiUndefinedConnectionCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckConnection", "StiUndefinedConnectionCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiUndefinedDatabase)
		{
			StiUndefinedConnectionCheck stiUndefinedConnectionCheck = new StiUndefinedConnectionCheck();
			stiUndefinedConnectionCheck.Element = obj;
			stiUndefinedConnectionCheck.report = report;
			stiUndefinedConnectionCheck.Actions.Add(new StiDeleteConnectionAction());
			return stiUndefinedConnectionCheck;
		}
		return null;
	}
}
