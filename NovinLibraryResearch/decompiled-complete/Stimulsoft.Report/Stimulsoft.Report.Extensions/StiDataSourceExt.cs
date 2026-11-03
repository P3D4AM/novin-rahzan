using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Extensions;

public static class StiDataSourceExt
{
	public static IEnumerable<StiDataRelation> ParentRelationList(this StiDataSource dataSource, bool activePreferred = false)
	{
		IEnumerable<StiDataRelation> enumerable = dataSource.GetParentRelations().Cast<StiDataRelation>();
		if (!activePreferred || !enumerable.Any((StiDataRelation r) => r.Active))
		{
			return enumerable;
		}
		return enumerable.Where((StiDataRelation r) => r.Active);
	}

	public static IEnumerable<StiDataRelation> ChildRelationList(this StiDataSource dataSource, bool activePreferred = false)
	{
		IEnumerable<StiDataRelation> enumerable = dataSource.GetChildRelations().Cast<StiDataRelation>();
		if (!activePreferred || !enumerable.Any((StiDataRelation r) => r.Active))
		{
			return enumerable;
		}
		return enumerable.Where((StiDataRelation r) => r.Active);
	}
}
