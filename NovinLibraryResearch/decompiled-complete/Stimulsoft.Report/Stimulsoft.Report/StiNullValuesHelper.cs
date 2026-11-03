using System;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public sealed class StiNullValuesHelper
{
	public static bool IsNull(StiReport report, string dataColumn)
	{
		object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(report.Dictionary, dataColumn);
		if (dataFromDataColumn != null)
		{
			return dataFromDataColumn == DBNull.Value;
		}
		return true;
	}
}
