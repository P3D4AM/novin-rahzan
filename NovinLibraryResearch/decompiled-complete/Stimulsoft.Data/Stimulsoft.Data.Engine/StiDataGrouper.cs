using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Comparers;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Data.Parsers;

namespace Stimulsoft.Data.Engine;

public static class StiDataGrouper
{
	public static StiDataTable Group(IStiAppDictionary dictionary, DataTable joinedTable, List<IStiMeter> meters)
	{
		StiDimensionDataParser dimensionParser = new StiDimensionDataParser(dictionary, joinedTable, meters);
		List<IStiMeter> meterList = meters.ToList();
		if (meters.Any((IStiMeter m) => m is IStiSkipNormalizeDate))
		{
			meterList = null;
		}
		IOrderedEnumerable<IGrouping<object[], object[]>> grandRows = joinedTable.AsEnumerableArray().GroupBy((object[] row) => dimensionParser.Calculate(row, meterList), new StiArrayEqualityComparer()).OrderBy((IGrouping<object[], object[]> groupRow) => groupRow.Key, new StiArrayComparer());
		List<object[]> rows = new StiMeasureDataParser(dictionary, joinedTable, meters, grandRows).Calculate().ToList();
		return new StiDataTable(meters, rows);
	}
}
