using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class SpatialIndex : ISpatialIndex, IIndex, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly double boundingBoxXMax;

	private readonly double boundingBoxXMin;

	private readonly double boundingBoxYMax;

	private readonly double boundingBoxYMin;

	private readonly int cellsPerObject;

	private readonly bool disallowPageLocks;

	private readonly bool disallowRowLocks;

	private readonly byte fillFactor;

	private readonly bool ignoreDuplicateKeys;

	private readonly int indexedColumn;

	private readonly bool isDisabled;

	private readonly GridDensity level1Density;

	private readonly GridDensity level2Density;

	private readonly GridDensity level3Density;

	private readonly GridDensity level4Density;

	private readonly string name;

	private readonly bool noAutomaticRecomputation;

	private readonly bool padIndex;

	private readonly int parent;

	double ISpatialIndex.BoundingBoxXMax => boundingBoxXMax;

	double ISpatialIndex.BoundingBoxXMin => boundingBoxXMin;

	double ISpatialIndex.BoundingBoxYMax => boundingBoxYMax;

	double ISpatialIndex.BoundingBoxYMin => boundingBoxYMin;

	int ISpatialIndex.CellsPerObject => cellsPerObject;

	IColumn ISpatialIndex.IndexedColumn => model.GetObjectById<IColumn>(indexedColumn);

	GridDensity ISpatialIndex.Level1Density => level1Density;

	GridDensity ISpatialIndex.Level2Density => level2Density;

	GridDensity ISpatialIndex.Level3Density => level3Density;

	GridDensity ISpatialIndex.Level4Density => level4Density;

	bool ISpatialIndex.NoAutomaticRecomputation => noAutomaticRecomputation;

	ITabular IIndex.Parent => model.GetObjectById<ITabular>(parent);

	IndexType IIndex.Type => IndexType.Spatial;

	bool IIndex.DisallowPageLocks => disallowPageLocks;

	bool IIndex.DisallowRowLocks => disallowRowLocks;

	byte IIndex.FillFactor => fillFactor;

	bool IIndex.IgnoreDuplicateKeys => ignoreDuplicateKeys;

	bool IIndex.IsDisabled => isDisabled;

	bool IIndex.PadIndex => padIndex;

	string IMetadataObject.Name => name;

	public SpatialIndex(DeserializationModel model, double boundingBoxXMax, double boundingBoxXMin, double boundingBoxYMax, double boundingBoxYMin, int cellsPerObject, bool disallowPageLocks, bool disallowRowLocks, byte fillFactor, bool ignoreDuplicateKeys, int indexedColumn, bool isDisabled, GridDensity level1Density, GridDensity level2Density, GridDensity level3Density, GridDensity level4Density, string name, bool noAutomaticRecomputation, bool padIndex, int parent)
	{
		this.model = model;
		this.boundingBoxXMax = boundingBoxXMax;
		this.boundingBoxXMin = boundingBoxXMin;
		this.boundingBoxYMax = boundingBoxYMax;
		this.boundingBoxYMin = boundingBoxYMin;
		this.cellsPerObject = cellsPerObject;
		this.disallowPageLocks = disallowPageLocks;
		this.disallowRowLocks = disallowRowLocks;
		this.fillFactor = fillFactor;
		this.ignoreDuplicateKeys = ignoreDuplicateKeys;
		this.indexedColumn = indexedColumn;
		this.isDisabled = isDisabled;
		this.level1Density = level1Density;
		this.level2Density = level2Density;
		this.level3Density = level3Density;
		this.level4Density = level4Density;
		this.name = name;
		this.noAutomaticRecomputation = noAutomaticRecomputation;
		this.padIndex = padIndex;
		this.parent = parent;
	}

	public SpatialIndex(DeserializationModel model, object[] values)
		: this(model, (double)values[0], (double)values[1], (double)values[2], (double)values[3], (int)values[4], (bool)values[5], (bool)values[6], (byte)values[7], (bool)values[8], (int)values[9], (bool)values[10], (GridDensity)values[11], (GridDensity)values[12], (GridDensity)values[13], (GridDensity)values[14], (string)values[15], (bool)values[16], (bool)values[17], (int)values[18])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
