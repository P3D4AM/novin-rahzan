using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal abstract class RowWiseCachedMorph : MultidimensionalCachedMorph
{
	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		return TransformRows(source);
	}

	internal IEnumerable<IDataRow> TransformRows(IEnumerable<IDataRow> source)
	{
		return source.SelectMany(Transform);
	}

	protected abstract IEnumerable<IDataRow> Transform(IDataRow sourceRow);
}
