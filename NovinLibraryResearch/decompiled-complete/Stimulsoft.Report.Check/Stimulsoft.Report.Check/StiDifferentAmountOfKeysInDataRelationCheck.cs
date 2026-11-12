using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDifferentAmountOfKeysInDataRelationCheck : StiDataRelationCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiDifferentAmountOfKeysInDataRelationCheckShort"), ElementName);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiDifferentAmountOfKeysInDataRelationCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataRelation stiDataRelation && stiDataRelation.ParentColumns.Length != stiDataRelation.ChildColumns.Length)
		{
			StiDifferentAmountOfKeysInDataRelationCheck stiDifferentAmountOfKeysInDataRelationCheck = new StiDifferentAmountOfKeysInDataRelationCheck();
			stiDifferentAmountOfKeysInDataRelationCheck.Element = obj;
			return stiDifferentAmountOfKeysInDataRelationCheck;
		}
		return null;
	}
}
