using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Data;

internal struct RowMorphingView : IDataView, IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
	private readonly IDataView source;

	private readonly IRowMorph morph;

	public int Count => source.Count;

	public IDataRow this[int index] => new MorphingRow(morph, source[index]);

	public RowMorphingView(IRowMorph morph, IDataView source)
	{
		this.morph = morph ?? throw new ArgumentNullException("morph");
		this.source = source ?? throw new ArgumentNullException("source");
	}

	public IEnumerator<IDataRow> GetEnumerator()
	{
		foreach (IDataRow item in source)
		{
			yield return new MorphingRow(morph, item);
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
