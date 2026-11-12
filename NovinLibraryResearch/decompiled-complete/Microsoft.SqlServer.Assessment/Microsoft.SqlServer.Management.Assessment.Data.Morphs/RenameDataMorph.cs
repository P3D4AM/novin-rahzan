using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class RenameDataMorph : IDataMorph, IRowMorph
{
	public Dictionary<string, string> Map { get; } = new Dictionary<string, string>();


	public bool BenefitsFromCache => false;

	public IDataView ApplyTo(IDataView source, bool useCache = true)
	{
		return new RowMorphingView(this, source);
	}

	public bool TryGetData(IDataRow source, string column, out object value)
	{
		string column2;
		if (Map.TryGetValue(column, out var value2))
		{
			column2 = value2;
		}
		else
		{
			if (Map.Values.Contains(column))
			{
				value = null;
				return false;
			}
			column2 = column;
		}
		return source.TryGetData(column2, out value);
	}
}
