using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class DefaultValueMorph : IDataMorph, IRowMorph
{
	public Dictionary<string, object> Map { get; } = new Dictionary<string, object>();


	public bool BenefitsFromCache => false;

	public IDataView ApplyTo(IDataView source, bool useCache = true)
	{
		return new RowMorphingView(this, source);
	}

	public bool TryGetData(IDataRow source, string column, out object value)
	{
		if (!source.TryGetData(column, out value) && !Map.TryGetValue(column, out value))
		{
			return false;
		}
		value = value.Normalize();
		return true;
	}
}
