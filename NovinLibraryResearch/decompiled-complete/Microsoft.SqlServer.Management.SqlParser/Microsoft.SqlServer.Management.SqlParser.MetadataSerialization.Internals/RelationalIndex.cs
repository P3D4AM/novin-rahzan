using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class RelationalIndex : IRelationalIndex, IIndex, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly bool compactLargeObjects;

	private readonly bool disallowPageLocks;

	private readonly bool disallowRowLocks;

	private readonly int fileGroup;

	private readonly int fileStreamFileGroup;

	private readonly int fileStreamPartitionScheme;

	private readonly byte fillFactor;

	private readonly string filterDefinition;

	private readonly bool ignoreDuplicateKeys;

	private readonly int indexKey;

	private readonly int indexedColumns;

	private readonly bool isClustered;

	private readonly bool isDisabled;

	private readonly bool isSystemNamed;

	private readonly bool isUnique;

	private readonly string name;

	private readonly bool noAutomaticRecomputation;

	private readonly bool padIndex;

	private readonly int parent;

	private readonly int partitionScheme;

	bool IRelationalIndex.CompactLargeObjects => compactLargeObjects;

	IFileGroup IRelationalIndex.FileGroup => model.GetObjectById<IFileGroup>(fileGroup);

	IFileGroup IRelationalIndex.FileStreamFileGroup => model.GetObjectById<IFileGroup>(fileStreamFileGroup);

	IPartitionScheme IRelationalIndex.FileStreamPartitionScheme => model.GetObjectById<IPartitionScheme>(fileStreamPartitionScheme);

	string IRelationalIndex.FilterDefinition => filterDefinition;

	IMetadataOrderedCollection<IIndexedColumn> IRelationalIndex.IndexedColumns => model.GetOrderedCollectionById<IIndexedColumn>(indexedColumns);

	IUniqueConstraintBase IRelationalIndex.IndexKey => model.GetObjectById<IUniqueConstraintBase>(indexKey);

	bool IRelationalIndex.IsClustered => isClustered;

	bool IRelationalIndex.IsSystemNamed => isSystemNamed;

	bool IRelationalIndex.IsUnique => isUnique;

	bool IRelationalIndex.NoAutomaticRecomputation => noAutomaticRecomputation;

	IPartitionScheme IRelationalIndex.PartitionScheme => model.GetObjectById<IPartitionScheme>(partitionScheme);

	ITabular IIndex.Parent => model.GetObjectById<ITabular>(parent);

	IndexType IIndex.Type => IndexType.Relational;

	bool IIndex.DisallowPageLocks => disallowPageLocks;

	bool IIndex.DisallowRowLocks => disallowRowLocks;

	byte IIndex.FillFactor => fillFactor;

	bool IIndex.IgnoreDuplicateKeys => ignoreDuplicateKeys;

	bool IIndex.IsDisabled => isDisabled;

	bool IIndex.PadIndex => padIndex;

	string IMetadataObject.Name => name;

	public RelationalIndex(DeserializationModel model, bool compactLargeObjects, bool disallowPageLocks, bool disallowRowLocks, int fileGroup, int fileStreamFileGroup, int fileStreamPartitionScheme, byte fillFactor, string filterDefinition, bool ignoreDuplicateKeys, int indexKey, int indexedColumns, bool isClustered, bool isDisabled, bool isSystemNamed, bool isUnique, string name, bool noAutomaticRecomputation, bool padIndex, int parent, int partitionScheme)
	{
		this.model = model;
		this.compactLargeObjects = compactLargeObjects;
		this.disallowPageLocks = disallowPageLocks;
		this.disallowRowLocks = disallowRowLocks;
		this.fileGroup = fileGroup;
		this.fileStreamFileGroup = fileStreamFileGroup;
		this.fileStreamPartitionScheme = fileStreamPartitionScheme;
		this.fillFactor = fillFactor;
		this.filterDefinition = filterDefinition;
		this.ignoreDuplicateKeys = ignoreDuplicateKeys;
		this.indexKey = indexKey;
		this.indexedColumns = indexedColumns;
		this.isClustered = isClustered;
		this.isDisabled = isDisabled;
		this.isSystemNamed = isSystemNamed;
		this.isUnique = isUnique;
		this.name = name;
		this.noAutomaticRecomputation = noAutomaticRecomputation;
		this.padIndex = padIndex;
		this.parent = parent;
		this.partitionScheme = partitionScheme;
	}

	public RelationalIndex(DeserializationModel model, object[] values)
		: this(model, (bool)values[0], (bool)values[1], (bool)values[2], (int)values[3], (int)values[4], (int)values[5], (byte)values[6], (string)values[7], (bool)values[8], (int)values[9], (int)values[10], (bool)values[11], (bool)values[12], (bool)values[13], (bool)values[14], (string)values[15], (bool)values[16], (bool)values[17], (int)values[18], (int)values[19])
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
