using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiNoNameDataRelationCheck : StiDataRelationCheck
{
	private string dataSources;

	public override bool PreviewVisible => true;

	public string DataSources
	{
		get
		{
			return dataSources;
		}
		set
		{
			dataSources = value;
		}
	}

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiNoNameDataRelationCheckShort"), DataSources);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiNoNameDataRelationCheckLong"), DataSources);

	public override StiCheckStatus Status => StiCheckStatus.Error;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataRelation stiDataRelation && string.IsNullOrEmpty(stiDataRelation.Name))
		{
			StiNoNameDataRelationCheck stiNoNameDataRelationCheck = new StiNoNameDataRelationCheck();
			stiNoNameDataRelationCheck.Element = obj;
			stiNoNameDataRelationCheck.dataSources = "";
			if (stiDataRelation.ParentSource != null)
			{
				stiNoNameDataRelationCheck.dataSources = stiDataRelation.ParentSource.Name;
			}
			if (stiDataRelation.ParentSource != null)
			{
				if (string.IsNullOrEmpty(stiNoNameDataRelationCheck.DataSources))
				{
					stiNoNameDataRelationCheck.dataSources = stiDataRelation.ChildSource.Name;
				}
				else
				{
					stiNoNameDataRelationCheck.dataSources = stiNoNameDataRelationCheck.dataSources + "; " + stiDataRelation.ChildSource;
				}
			}
			stiNoNameDataRelationCheck.Actions.Add(new StiGenerateNewNameRelationAction());
			stiNoNameDataRelationCheck.Actions.Add(new StiDeleteDataRelationAction());
			return stiNoNameDataRelationCheck;
		}
		return null;
	}
}
