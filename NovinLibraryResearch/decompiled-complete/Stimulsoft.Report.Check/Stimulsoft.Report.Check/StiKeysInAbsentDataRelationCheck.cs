using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiKeysInAbsentDataRelationCheck : StiDataRelationCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiKeysInAbsentDataRelationCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiKeysInAbsentDataRelationCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataRelation stiDataRelation && stiDataRelation.ParentColumns.Length == 0 && stiDataRelation.ChildColumns.Length == 0)
		{
			StiKeysInAbsentDataRelationCheck stiKeysInAbsentDataRelationCheck = new StiKeysInAbsentDataRelationCheck();
			stiKeysInAbsentDataRelationCheck.Element = obj;
			return stiKeysInAbsentDataRelationCheck;
		}
		return null;
	}
}
