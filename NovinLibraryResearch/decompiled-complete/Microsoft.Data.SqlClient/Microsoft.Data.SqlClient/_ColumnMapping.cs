namespace Microsoft.Data.SqlClient;

internal sealed class _ColumnMapping
{
	internal readonly int _sourceColumnOrdinal;

	internal readonly _SqlMetaData _metadata;

	internal _ColumnMapping(int columnId, _SqlMetaData metadata)
	{
		_sourceColumnOrdinal = columnId;
		_metadata = metadata;
	}
}
