namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Constant("Type", IndexType.Spatial)]
[Metadata]
public interface ISpatialIndex : IIndex, IMetadataObject
{
	[ReadWrite]
	double BoundingBoxXMax { get; }

	[ReadWrite]
	double BoundingBoxXMin { get; }

	[ReadWrite]
	double BoundingBoxYMax { get; }

	[ReadWrite]
	double BoundingBoxYMin { get; }

	[ReadWrite]
	int CellsPerObject { get; }

	[ReadWrite]
	[Reference]
	IColumn IndexedColumn { get; }

	[ReadWrite]
	GridDensity Level1Density { get; }

	[ReadWrite]
	GridDensity Level2Density { get; }

	[ReadWrite]
	GridDensity Level3Density { get; }

	[ReadWrite]
	GridDensity Level4Density { get; }

	[ReadWrite]
	bool NoAutomaticRecomputation { get; }
}
