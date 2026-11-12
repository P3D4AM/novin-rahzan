namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableSpatialIndex : ISpatialIndex, IIndex, IMetadataObject, IMutableIndex, IMutableMetadataObject
{
	new double BoundingBoxXMax { get; set; }

	new double BoundingBoxXMin { get; set; }

	new double BoundingBoxYMax { get; set; }

	new double BoundingBoxYMin { get; set; }

	new int CellsPerObject { get; set; }

	new IColumn IndexedColumn { get; set; }

	new GridDensity Level1Density { get; set; }

	new GridDensity Level2Density { get; set; }

	new GridDensity Level3Density { get; set; }

	new GridDensity Level4Density { get; set; }

	new bool NoAutomaticRecomputation { get; set; }
}
