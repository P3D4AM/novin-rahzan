using System;

namespace Microsoft.SqlServer.Management.Assessment.Data;

internal struct MorphingRow : IDataRow
{
	private readonly IRowMorph morph;

	private readonly IDataRow source;

	public MorphingRow(IRowMorph morph, IDataRow source)
	{
		this.morph = morph ?? throw new ArgumentNullException("morph");
		this.source = source ?? throw new ArgumentNullException("source");
	}

	public bool TryGetData(string column, out object value)
	{
		return morph.TryGetData(source, column, out value);
	}
}
