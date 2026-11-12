using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiSourcesInAbsentDataRelationCheck : StiDataRelationCheck
{
	public override bool PreviewVisible => false;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiSourcesInAbsentDataRelationCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiSourcesInAbsentDataRelationCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataRelation stiDataRelation && (stiDataRelation.ParentSource == null || stiDataRelation.ChildSource == null))
		{
			StiSourcesInAbsentDataRelationCheck stiSourcesInAbsentDataRelationCheck = new StiSourcesInAbsentDataRelationCheck();
			stiSourcesInAbsentDataRelationCheck.Actions.Add(new StiDeleteDataRelationAction());
			stiSourcesInAbsentDataRelationCheck.Element = obj;
			return stiSourcesInAbsentDataRelationCheck;
		}
		return null;
	}
}
