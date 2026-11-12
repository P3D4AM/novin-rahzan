using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class ToStringMorph : IDataMorph, IRowMorph
{
	public Dictionary<string, Dictionary<string, object>> Map { get; } = new Dictionary<string, Dictionary<string, object>>();


	public bool BenefitsFromCache => false;

	public IDataView ApplyTo(IDataView source, bool useCache = true)
	{
		return new RowMorphingView(this, source);
	}

	public bool TryGetData(IDataRow source, string column, out object value)
	{
		if (!source.TryGetData(column, out value))
		{
			return false;
		}
		if (Map.TryGetValue(column, out var value2))
		{
			object obj = value.Normalize();
			foreach (KeyValuePair<string, object> item in value2)
			{
				object obj2 = item.Value.Normalize();
				if (obj2.Equals(obj))
				{
					value = item.Key;
					break;
				}
			}
		}
		return true;
	}
}
