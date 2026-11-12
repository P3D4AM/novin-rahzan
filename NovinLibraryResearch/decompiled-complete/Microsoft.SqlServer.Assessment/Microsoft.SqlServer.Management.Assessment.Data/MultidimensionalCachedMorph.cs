using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data;

internal abstract class MultidimensionalCachedMorph : IDataMorph, IMultidimensionalMorph
{
	public bool BenefitsFromCache => true;

	public IDataView ApplyTo(IDataView source, bool useCache = true)
	{
		if (!useCache)
		{
			return new MorphingView(this, source);
		}
		return new CachedMorphingView(this, source);
	}

	public abstract IEnumerable<IDataRow> Transform(IDataView source);
}
