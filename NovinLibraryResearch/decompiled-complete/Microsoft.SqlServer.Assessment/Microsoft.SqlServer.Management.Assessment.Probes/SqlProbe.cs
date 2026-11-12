using System.Collections.Generic;
using System.Data.Common;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class SqlProbe : DbaProbe
{
	protected override IEnumerable<Measurement> GetData(DbDataReader reader)
	{
		if (!reader.HasRows)
		{
			yield break;
		}
		do
		{
			Measurement measurement = new Measurement();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				measurement.Add(reader.GetName(i), reader[i]);
			}
			yield return measurement;
		}
		while (reader.Read());
	}
}
