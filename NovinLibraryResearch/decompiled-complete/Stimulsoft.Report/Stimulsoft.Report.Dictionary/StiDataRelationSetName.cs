using System.Data;

namespace Stimulsoft.Report.Dictionary;

public class StiDataRelationSetName
{
	public static void SetName(StiDataRelation dataRelation, StiReport report, DataSet dataSet, string name)
	{
		dataRelation.NameInSource = name;
		if (dataRelation.ParentSource != null)
		{
			string alias = (dataRelation.Name = StiNameCreation.CreateRelationName(report, dataRelation, dataRelation.ParentSource.Name));
			dataRelation.Alias = alias;
		}
	}
}
