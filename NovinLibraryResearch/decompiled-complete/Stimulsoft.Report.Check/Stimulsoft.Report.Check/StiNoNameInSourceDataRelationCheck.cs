using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNoNameInSourceDataRelationCheck : StiDataRelationCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiNoNameInSourceDataRelationCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiNoNameInSourceDataRelationCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataRelation stiDataRelation && string.IsNullOrEmpty(stiDataRelation.NameInSource))
		{
			StiNoNameInSourceDataRelationCheck stiNoNameInSourceDataRelationCheck = new StiNoNameInSourceDataRelationCheck();
			stiNoNameInSourceDataRelationCheck.Actions.Add(new StiDeleteDataRelationAction());
			stiNoNameInSourceDataRelationCheck.Element = obj;
			return stiNoNameInSourceDataRelationCheck;
		}
		return null;
	}
}
