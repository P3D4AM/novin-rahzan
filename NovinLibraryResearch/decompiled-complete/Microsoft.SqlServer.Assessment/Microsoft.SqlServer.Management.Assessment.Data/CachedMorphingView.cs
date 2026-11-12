using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data;

internal class CachedMorphingView : IDataView, IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
	private readonly IMultidimensionalMorph morph;

	private readonly IDataView source;

	private IDataRow[] cache;

	public int Count => GetData().Count();

	public IDataRow this[int index] => GetData().ElementAt(index);

	public CachedMorphingView(IMultidimensionalMorph morph, IDataView source)
	{
		this.morph = morph ?? throw new ArgumentNullException("morph");
		this.source = source ?? throw new ArgumentNullException("source");
	}

	public IEnumerator<IDataRow> GetEnumerator()
	{
		return GetData().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private IEnumerable<IDataRow> GetData()
	{
		if (cache == null)
		{
			cache = morph.Transform(source).ToArray();
		}
		return cache;
	}
}
