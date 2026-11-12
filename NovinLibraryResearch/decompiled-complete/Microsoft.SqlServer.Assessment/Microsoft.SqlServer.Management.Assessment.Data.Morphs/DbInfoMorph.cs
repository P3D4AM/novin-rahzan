using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class DbInfoMorph : MultidimensionalCachedMorph
{
	private readonly DateTime sqlDateTimeDefaultValue = new DateTime(1900, 1, 1);

	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		yield return new Measurement
		{
			{
				"LastKnownDBCCDaysAgo",
				(int)DateTime.Now.Subtract(FindValue<DateTime>(source, "dbi_dbccLastKnownGood")).TotalDays
			},
			{
				"UpdSysCatalog",
				(int)FindValue<DateTime>(source, "dbi_updSysCatalog").Subtract(sqlDateTimeDefaultValue).TotalDays
			},
			{
				"CreateVersion",
				FindValue<int>(source, "dbi_createVersion")
			},
			{
				"DBCCFlags",
				FindValue<int>(source, "dbi_dbccFlags")
			}
		};
	}

	private static T FindValue<T>(IDataView source, string key)
	{
		object value2;
		IDataRow dataRow2 = source.FirstOrDefault((IDataRow dataRow) => dataRow.TryGetData("Field", out value2) && ((string)value2).StartsWith(key, StringComparison.OrdinalIgnoreCase));
		if (dataRow2 != null && dataRow2.TryGetData("VALUE", out var value))
		{
			return value.Normalize<T>();
		}
		return default(T);
	}
}
