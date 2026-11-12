using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class RelationalIndex : Index, IMutableRelationalIndex, IRelationalIndex, IIndex, IMetadataObject, IMutableIndex, IMutableMetadataObject
{
	[Flags]
	private enum IndexOptions
	{
		None = 0,
		CompactLargeObjects = 1,
		DisallowPageLocks = 2,
		DisallowRowLocks = 4,
		IgnoreDuplicateKeys = 8,
		IsClustered = 0x10,
		IsDisabled = 0x20,
		IsSystemNamed = 0x40,
		IsUnique = 0x80,
		NoAutomaticRecomputation = 0x100,
		OnlineIndexOperation = 0x200,
		PadIndex = 0x400,
		SortInTempdb = 0x800,
		ResumableIndexOperation = 0x1000,
		OptimizeForSequentialKey = 0x2000
	}

	private ITabular m_parent;

	private readonly IndexedColumnCollection m_columns;

	private IFileGroup m_fileGroup;

	private IFileGroup m_fileStreamFileGroup;

	private IPartitionScheme m_fileStreamPartitionScheme;

	private byte m_fillFactor;

	private string m_filterDefinition;

	private IUniqueConstraintBase m_indexKey;

	private int m_maximumDegreeOfParallelism;

	private IndexOptions m_options;

	private IPartitionScheme m_partitionScheme;

	public bool CompactLargeObjects
	{
		get
		{
			return GetOption(IndexOptions.CompactLargeObjects);
		}
		set
		{
			SetOption(IndexOptions.CompactLargeObjects, value);
		}
	}

	public bool DisallowPageLocks
	{
		get
		{
			return GetOption(IndexOptions.DisallowPageLocks);
		}
		set
		{
			SetOption(IndexOptions.DisallowPageLocks, value);
		}
	}

	public bool DisallowRowLocks
	{
		get
		{
			return GetOption(IndexOptions.DisallowRowLocks);
		}
		set
		{
			SetOption(IndexOptions.DisallowRowLocks, value);
		}
	}

	public IFileGroup FileGroup
	{
		get
		{
			return m_fileGroup;
		}
		set
		{
			m_fileGroup = value;
		}
	}

	public IFileGroup FileStreamFileGroup
	{
		get
		{
			return m_fileStreamFileGroup;
		}
		set
		{
			m_fileStreamFileGroup = value;
		}
	}

	public IPartitionScheme FileStreamPartitionScheme
	{
		get
		{
			return m_fileStreamPartitionScheme;
		}
		set
		{
			m_fileStreamPartitionScheme = value;
		}
	}

	public byte FillFactor
	{
		get
		{
			return m_fillFactor;
		}
		set
		{
			m_fillFactor = value;
		}
	}

	public string FilterDefinition
	{
		get
		{
			return m_filterDefinition;
		}
		set
		{
			m_filterDefinition = value;
		}
	}

	public bool IgnoreDuplicateKeys
	{
		get
		{
			return GetOption(IndexOptions.IgnoreDuplicateKeys);
		}
		set
		{
			SetOption(IndexOptions.IgnoreDuplicateKeys, value);
		}
	}

	public IMutableMetadataOrderedCollection<IIndexedColumn> IndexedColumns => m_columns;

	public IUniqueConstraintBase IndexKey
	{
		get
		{
			return m_indexKey;
		}
		set
		{
			m_indexKey = value;
		}
	}

	public bool IsClustered
	{
		get
		{
			return GetOption(IndexOptions.IsClustered);
		}
		set
		{
			SetOption(IndexOptions.IsClustered, value);
		}
	}

	public bool IsDisabled
	{
		get
		{
			return GetOption(IndexOptions.IsDisabled);
		}
		set
		{
			SetOption(IndexOptions.IsDisabled, value);
		}
	}

	public bool IsSystemNamed
	{
		get
		{
			return GetOption(IndexOptions.IsSystemNamed);
		}
		set
		{
			SetOption(IndexOptions.IsSystemNamed, value);
		}
	}

	public bool IsUnique
	{
		get
		{
			return GetOption(IndexOptions.IsUnique);
		}
		set
		{
			SetOption(IndexOptions.IsUnique, value);
		}
	}

	public int MaximumDegreeOfParallelism
	{
		get
		{
			return m_maximumDegreeOfParallelism;
		}
		set
		{
			m_maximumDegreeOfParallelism = value;
		}
	}

	public bool NoAutomaticRecomputation
	{
		get
		{
			return GetOption(IndexOptions.NoAutomaticRecomputation);
		}
		set
		{
			SetOption(IndexOptions.NoAutomaticRecomputation, value);
		}
	}

	public bool OnlineIndexOperation
	{
		get
		{
			return GetOption(IndexOptions.OnlineIndexOperation);
		}
		set
		{
			SetOption(IndexOptions.OnlineIndexOperation, value);
		}
	}

	public bool OptimizeForSequentialKey
	{
		get
		{
			return GetOption(IndexOptions.OptimizeForSequentialKey);
		}
		set
		{
			SetOption(IndexOptions.OptimizeForSequentialKey, value);
		}
	}

	public bool PadIndex
	{
		get
		{
			return GetOption(IndexOptions.PadIndex);
		}
		set
		{
			SetOption(IndexOptions.PadIndex, value);
		}
	}

	public IPartitionScheme PartitionScheme
	{
		get
		{
			return m_partitionScheme;
		}
		set
		{
			m_partitionScheme = value;
		}
	}

	public bool ResumableIndexOperation
	{
		get
		{
			return GetOption(IndexOptions.ResumableIndexOperation);
		}
		set
		{
			SetOption(IndexOptions.ResumableIndexOperation, value);
		}
	}

	public bool SortInTempdb
	{
		get
		{
			return GetOption(IndexOptions.SortInTempdb);
		}
		set
		{
			SetOption(IndexOptions.SortInTempdb, value);
		}
	}

	IMetadataOrderedCollection<IIndexedColumn> IRelationalIndex.IndexedColumns => m_columns;

	public ITabular Parent => m_parent;

	public IndexType Type => IndexType.Relational;

	public RelationalIndex(ITabular parent, string name, CollationInfo collationInfo)
		: base(name)
	{
		m_parent = parent;
		m_columns = new IndexedColumnCollection(collationInfo);
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	private bool GetOption(IndexOptions option)
	{
		return (m_options & option) == option;
	}

	private void SetOption(IndexOptions option, bool value)
	{
		m_options = (value ? (m_options | option) : (m_options & ~option));
	}
}
