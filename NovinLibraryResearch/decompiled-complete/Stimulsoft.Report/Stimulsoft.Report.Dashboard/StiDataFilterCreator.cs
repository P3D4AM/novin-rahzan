using System;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Dashboard;

public static class StiDataFilterCreator
{
	public static StiDataFilterRule CreateEqualBasedOnValue(object value, string columnName, StiComponent component)
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		StiDataColumn dataColumnFromColumnName = StiDataColumn.GetDataColumnFromColumnName(component.Report.Dictionary, columnName);
		if (dataColumnFromColumnName == null && (columnName.Contains("[") || columnName.Contains("]")))
		{
			columnName = columnName.Replace("[", "").Replace("]", "");
			dataColumnFromColumnName = StiDataColumn.GetDataColumnFromColumnName(component.Report.Dictionary, columnName);
		}
		Type type = dataColumnFromColumnName?.Type;
		if (type == typeof(string) && ((value is string && string.IsNullOrEmpty(value as string)) || value == null || value == DBNull.Value))
		{
			return new StiDataFilterRule(columnName, (StiDataFilterCondition)16, value?.ToString());
		}
		if (value != null && value != DBNull.Value)
		{
			return new StiDataFilterRule(columnName, (StiDataFilterCondition)0, value?.ToString());
		}
		return new StiDataFilterRule(columnName, (StiDataFilterCondition)12);
	}
}
