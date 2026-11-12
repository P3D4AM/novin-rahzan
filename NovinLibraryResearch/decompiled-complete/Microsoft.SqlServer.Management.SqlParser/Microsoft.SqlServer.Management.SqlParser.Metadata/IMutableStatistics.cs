namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[ReadOnly("Name")]
public interface IMutableStatistics : IStatistics, IMetadataObject, IMutableMetadataObject
{
	[ReadWrite]
	new StatisticsType Type { get; set; }

	[ReadOnly]
	[Parent]
	new ITabular Parent { get; set; }

	[ReadWrite]
	new string FilterDefinition { get; set; }

	[ReadWrite]
	new bool NoAutomaticRecomputation { get; set; }

	[ReadWrite]
	[Reference]
	new IMetadataCollection<IColumn> Columns { get; set; }
}
