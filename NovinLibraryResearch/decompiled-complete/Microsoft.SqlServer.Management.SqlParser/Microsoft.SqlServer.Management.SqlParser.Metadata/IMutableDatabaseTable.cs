namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableDatabaseTable : IDatabaseTable, ITabular, IMetadataObject, IMutableTabular, IMutableMetadataObject
{
	new IMutableMetadataCollection<IConstraint> Constraints { get; }

	new IMutableMetadataCollection<IIndex> Indexes { get; }

	new IMutableMetadataCollection<IStatistics> Statistics { get; }
}
