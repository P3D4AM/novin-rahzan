using System;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiKeysTypesMismatchDataRelationCheck : StiDataRelationCheck
{
	private string columns;

	public override bool PreviewVisible => true;

	public string Columns
	{
		get
		{
			return columns;
		}
		set
		{
			columns = value;
		}
	}

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiKeysTypesMismatchDataRelationCheckShort"), ElementName, Columns);

	public override string LongMessage
	{
		get
		{
			string arg = ((base.Element != null) ? (((StiDataRelation)base.Element).Name + " ('" + ((StiDataRelation)base.Element).ChildSource.Name + "' data source)") : string.Empty);
			return string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiKeysTypesMismatchDataRelationCheckLong"), arg, Columns);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Error;

	private bool IsColumnsExist(StiDataRelation relation)
	{
		string[] parentColumns = relation.ParentColumns;
		foreach (string name in parentColumns)
		{
			if (!relation.ParentSource.Columns.Contains(name))
			{
				return false;
			}
		}
		string[] childColumns = relation.ChildColumns;
		foreach (string name2 in childColumns)
		{
			if (!relation.ChildSource.Columns.Contains(name2))
			{
				return false;
			}
		}
		return true;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (obj is StiDataRelation { ParentSource: not null, ChildSource: not null } stiDataRelation && stiDataRelation.ParentColumns.Length == stiDataRelation.ChildColumns.Length && IsColumnsExist(stiDataRelation))
		{
			bool flag = false;
			string text = string.Empty;
			for (int i = 0; i < stiDataRelation.ParentColumns.Length; i++)
			{
				string text2 = stiDataRelation.ParentColumns[i];
				string text3 = stiDataRelation.ChildColumns[i];
				Type type = stiDataRelation.ParentSource.Columns[text2].Type;
				Type type2 = stiDataRelation.ChildSource.Columns[text3].Type;
				if (type != type2)
				{
					text = ((text.Length != 0) ? (text + $"; {stiDataRelation.ParentSource}.{text2} - {stiDataRelation.ChildSource}.{text3}") : $"{stiDataRelation.ParentSource}.{text2} - {stiDataRelation.ChildSource}.{text3}");
					flag = true;
				}
			}
			if (flag)
			{
				StiKeysTypesMismatchDataRelationCheck stiKeysTypesMismatchDataRelationCheck = new StiKeysTypesMismatchDataRelationCheck();
				stiKeysTypesMismatchDataRelationCheck.columns = text;
				stiKeysTypesMismatchDataRelationCheck.Element = obj;
				return stiKeysTypesMismatchDataRelationCheck;
			}
		}
		return null;
	}
}
