using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class ColumnAlias : MetadataObjectBase, IColumn, IScalar, IMetadataObject
{
	private readonly ITabular m_parent;

	private readonly IColumn m_aliasedColumn;

	public bool InPrimaryKey => m_aliasedColumn.InPrimaryKey;

	public IdentityColumnInfo IdentityColumnInfo => m_aliasedColumn.IdentityColumnInfo;

	public ICollation Collation => m_aliasedColumn.Collation;

	public ComputedColumnInfo ComputedColumnInfo => m_aliasedColumn.ComputedColumnInfo;

	public IDefaultConstraint DefaultValue => m_aliasedColumn.DefaultValue;

	public bool RowGuidCol => m_aliasedColumn.RowGuidCol;

	public bool IsSparse => m_aliasedColumn.IsSparse;

	public bool IsColumnSet => m_aliasedColumn.IsColumnSet;

	public bool IsGeneratedAlwaysAsRowStart => m_aliasedColumn.IsGeneratedAlwaysAsRowStart;

	public bool IsGeneratedAlwaysAsRowEnd => m_aliasedColumn.IsGeneratedAlwaysAsRowEnd;

	public ITabular Parent => m_parent;

	public bool IsGeneratedAlwaysAsTransactionIdStart => m_aliasedColumn.IsGeneratedAlwaysAsTransactionIdStart;

	public bool IsGeneratedAlwaysAsTransactionIdEnd => m_aliasedColumn.IsGeneratedAlwaysAsTransactionIdEnd;

	public bool IsGeneratedAlwaysAsSequenceNumberStart => m_aliasedColumn.IsGeneratedAlwaysAsSequenceNumberStart;

	public bool IsGeneratedAlwaysAsSequenceNumberEnd => m_aliasedColumn.IsGeneratedAlwaysAsSequenceNumberEnd;

	public ScalarType ScalarType => ScalarType.Column;

	public IScalarDataType DataType => m_aliasedColumn.DataType;

	public bool Nullable => m_aliasedColumn.Nullable;

	public ColumnAlias(ITabular parent, IColumn aliasedColumn, string alias)
		: base(alias)
	{
		m_parent = parent;
		m_aliasedColumn = aliasedColumn;
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
