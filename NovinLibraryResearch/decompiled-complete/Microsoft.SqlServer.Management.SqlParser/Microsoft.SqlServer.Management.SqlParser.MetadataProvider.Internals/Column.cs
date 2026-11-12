using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Column : MetadataObjectBase, IMutableColumn, IColumn, IScalar, IMetadataObject, IMutableScalar, IMutableMetadataObject
{
	private ITabular m_parent;

	private ICollation m_collation;

	private ComputedColumnInfo m_computedColumnInfo;

	private IScalarDataType m_dataType;

	private IDefaultConstraint m_defaultValue;

	private IdentityColumnInfo m_identityColumnInfo;

	private bool m_isSparse;

	private bool m_isColumnSet;

	private bool m_inPrimaryKey;

	private bool m_nullable;

	private bool m_rowGuidCol;

	private bool m_isGeneratedAlwaysAsRowStart;

	private bool m_isGeneratedAlwaysAsRowEnd;

	private bool m_isGeneratedAlwaysAsTransactionIdStart;

	private bool m_isGeneratedAlwaysAsTransactionIdEnd;

	private bool m_isGeneratedAlwaysAsSequenceNumberStart;

	private bool m_isGeneratedAlwaysAsSequenceNumberEnd;

	public ICollation Collation
	{
		get
		{
			return m_collation;
		}
		set
		{
			m_collation = value;
		}
	}

	public ComputedColumnInfo ComputedColumnInfo
	{
		get
		{
			return m_computedColumnInfo;
		}
		set
		{
			m_computedColumnInfo = value;
		}
	}

	public IDefaultConstraint DefaultValue
	{
		get
		{
			return m_defaultValue;
		}
		set
		{
			m_defaultValue = value;
		}
	}

	public IdentityColumnInfo IdentityColumnInfo
	{
		get
		{
			return m_identityColumnInfo;
		}
		set
		{
			m_identityColumnInfo = value;
		}
	}

	public bool InPrimaryKey
	{
		get
		{
			return m_inPrimaryKey;
		}
		set
		{
			m_inPrimaryKey = value;
		}
	}

	public bool RowGuidCol
	{
		get
		{
			return m_rowGuidCol;
		}
		set
		{
			m_rowGuidCol = value;
		}
	}

	public bool IsSparse
	{
		get
		{
			return m_isSparse;
		}
		set
		{
			m_isSparse = value;
		}
	}

	public bool IsColumnSet
	{
		get
		{
			return m_isColumnSet;
		}
		set
		{
			m_isColumnSet = value;
		}
	}

	public bool IsGeneratedAlwaysAsRowStart
	{
		get
		{
			return m_isGeneratedAlwaysAsRowStart;
		}
		set
		{
			m_isGeneratedAlwaysAsRowStart = value;
		}
	}

	public bool IsGeneratedAlwaysAsRowEnd
	{
		get
		{
			return m_isGeneratedAlwaysAsRowEnd;
		}
		set
		{
			m_isGeneratedAlwaysAsRowEnd = value;
		}
	}

	public bool IsGeneratedAlwaysAsTransactionIdStart
	{
		get
		{
			return m_isGeneratedAlwaysAsTransactionIdStart;
		}
		set
		{
			m_isGeneratedAlwaysAsTransactionIdStart = value;
		}
	}

	public bool IsGeneratedAlwaysAsTransactionIdEnd
	{
		get
		{
			return m_isGeneratedAlwaysAsTransactionIdEnd;
		}
		set
		{
			m_isGeneratedAlwaysAsTransactionIdEnd = value;
		}
	}

	public bool IsGeneratedAlwaysAsSequenceNumberStart
	{
		get
		{
			return m_isGeneratedAlwaysAsSequenceNumberStart;
		}
		set
		{
			m_isGeneratedAlwaysAsSequenceNumberStart = value;
		}
	}

	public bool IsGeneratedAlwaysAsSequenceNumberEnd
	{
		get
		{
			return m_isGeneratedAlwaysAsSequenceNumberEnd;
		}
		set
		{
			m_isGeneratedAlwaysAsSequenceNumberEnd = value;
		}
	}

	public ScalarType ScalarType => ScalarType.Column;

	public IScalarDataType DataType
	{
		get
		{
			return m_dataType;
		}
		set
		{
			m_dataType = value;
		}
	}

	public bool Nullable
	{
		get
		{
			return m_nullable;
		}
		set
		{
			m_nullable = value;
		}
	}

	public ITabular Parent => m_parent;

	public Column(ITabular parent, string name)
		: base(name)
	{
		m_parent = parent;
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
