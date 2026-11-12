using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiDuplicatedNameInSourceInDataRelationReportCheck : StiReportCheck
{
	private string relationsNames;

	private string relationsNameInSource;

	public override bool PreviewVisible => false;

	public string RelationsNames
	{
		get
		{
			return relationsNames;
		}
		set
		{
			relationsNames = value;
		}
	}

	public string RelationsNameInSource
	{
		get
		{
			return relationsNameInSource;
		}
		set
		{
			relationsNameInSource = value;
		}
	}

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckReport", "StiDuplicatedNameInSourceInDataRelationReportCheckShort"), RelationsNames, RelationsNameInSource);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckReport", "StiDuplicatedNameInSourceInDataRelationReportCheckLong"), RelationsNames, RelationsNameInSource);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		List<StiCheck> list = null;
		Hashtable hashtable = new Hashtable();
		foreach (StiDataRelation relation in report.Dictionary.Relations)
		{
			List<StiDataRelation> list2 = hashtable[relation.NameInSource] as List<StiDataRelation>;
			if (list2 == null)
			{
				list2 = new List<StiDataRelation>();
				hashtable[relation.NameInSource] = list2;
			}
			list2.Add(relation);
		}
		foreach (List<StiDataRelation> value2 in hashtable.Values)
		{
			if (value2.Count <= 1)
			{
				continue;
			}
			string text = string.Empty;
			string value = string.Empty;
			foreach (StiDataRelation item in value2)
			{
				text = ((text.Length != 0) ? (text + "; " + item.Name) : (text + item.Name));
				value = item.NameInSource;
			}
			if (!string.IsNullOrEmpty(value))
			{
				StiDuplicatedNameInSourceInDataRelationReportCheck stiDuplicatedNameInSourceInDataRelationReportCheck = new StiDuplicatedNameInSourceInDataRelationReportCheck();
				stiDuplicatedNameInSourceInDataRelationReportCheck.Element = obj;
				stiDuplicatedNameInSourceInDataRelationReportCheck.relationsNames = text;
				stiDuplicatedNameInSourceInDataRelationReportCheck.relationsNameInSource = value;
				if (list == null)
				{
					list = new List<StiCheck>();
				}
				list.Add(stiDuplicatedNameInSourceInDataRelationReportCheck);
			}
		}
		return list;
	}
}
