using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiKeysNotFoundRelationCheck : StiDataRelationCheck
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

	public override string ShortMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiKeysNotFoundRelationCheckShort"), ElementName, Columns);

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckDataRelation", "StiKeysNotFoundRelationCheckLong"), ElementName, Columns);

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
		if (obj is StiDataRelation { ParentSource: not null, ChildSource: not null } stiDataRelation && !IsColumnsExist(stiDataRelation))
		{
			bool flag = false;
			string text = string.Empty;
			string[] parentColumns = stiDataRelation.ParentColumns;
			foreach (string text2 in parentColumns)
			{
				if (!stiDataRelation.ParentSource.Columns.Contains(text2))
				{
					text = ((text.Length != 0) ? (text + "; " + text2) : (text + text2));
					flag = true;
				}
			}
			string[] childColumns = stiDataRelation.ChildColumns;
			foreach (string text3 in childColumns)
			{
				if (!stiDataRelation.ChildSource.Columns.Contains(text3))
				{
					text = ((text.Length != 0) ? (text + "; " + text3) : (text + text3));
					flag = true;
				}
			}
			if (flag)
			{
				StiKeysNotFoundRelationCheck stiKeysNotFoundRelationCheck = new StiKeysNotFoundRelationCheck();
				stiKeysNotFoundRelationCheck.columns = text;
				stiKeysNotFoundRelationCheck.Element = obj;
				return stiKeysNotFoundRelationCheck;
			}
		}
		return null;
	}
}
