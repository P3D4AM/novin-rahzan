namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[ReadOnly("Name")]
public interface IStatistics : IMetadataObject
{
	[ReadWrite]
	StatisticsType Type { get; }

	[ReadOnly]
	[Parent]
	ITabular Parent { get; }

	[ReadWrite]
	string FilterDefinition { get; }

	[ReadWrite]
	bool NoAutomaticRecomputation { get; }

	[ReadWrite]
	[Reference]
	IMetadataOrderedCollection<IColumn> Columns { get; }
}
