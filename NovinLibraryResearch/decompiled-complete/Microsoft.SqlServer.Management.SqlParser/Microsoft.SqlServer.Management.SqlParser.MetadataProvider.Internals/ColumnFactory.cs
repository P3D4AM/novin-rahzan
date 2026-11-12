using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class ColumnFactory : ObjectFactoryBase<ColumnFactory>, IColumnFactory
{
	private sealed class SimpleColumn : MetadataObjectBase, IColumn, IScalar, IMetadataObject
	{
		private readonly ITabular parent;

		private readonly IScalarDataType dataType;

		private readonly bool nullable;

		public ITabular Parent => parent;

		public ICollation Collation => null;

		public ComputedColumnInfo ComputedColumnInfo => null;

		public IDefaultConstraint DefaultValue => null;

		public IdentityColumnInfo IdentityColumnInfo => null;

		public bool InPrimaryKey => false;

		public bool RowGuidCol => false;

		public bool IsSparse => false;

		public bool IsColumnSet => false;

		public bool IsGeneratedAlwaysAsRowStart => false;

		public bool IsGeneratedAlwaysAsRowEnd => false;

		public bool IsGeneratedAlwaysAsTransactionIdStart => false;

		public bool IsGeneratedAlwaysAsTransactionIdEnd => false;

		public bool IsGeneratedAlwaysAsSequenceNumberStart => false;

		public bool IsGeneratedAlwaysAsSequenceNumberEnd => false;

		public ScalarType ScalarType => ScalarType.Column;

		public IScalarDataType DataType => dataType;

		public bool Nullable => nullable;

		public SimpleColumn(ITabular parent, string name, IScalarDataType dataType, bool nullable)
			: base(name)
		{
			this.parent = parent;
			this.dataType = dataType;
			this.nullable = nullable;
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	private readonly IColumn m_nullColumn;

	public IColumn Null => m_nullColumn;

	public ColumnFactory()
	{
		m_nullColumn = new SimpleColumn(null, string.Empty, ObjectFactoryBase<DataTypeFactory>.Instance.UnknownScalar, nullable: true);
	}

	public IMutableColumn Create(ITabular parent, string name)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Column(parent, name);
	}

	public IColumn CreateSimpleColumn(ITabular parent, string name, IScalarDataType dataType, bool nullable)
	{
		if (parent == null)
		{
			throw new ArgumentNullException("parent");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		return new SimpleColumn(parent, name, dataType, nullable);
	}

	public IColumn CreateAnonymousColumn(IScalarDataType dataType, bool nullable)
	{
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		return new SimpleColumn(null, string.Empty, dataType, nullable);
	}

	public IColumn CreateColumnAlias(IColumn aliasedColumn, string alias)
	{
		return CreateColumnAlias(null, aliasedColumn, alias);
	}

	public IColumn CreateColumnAlias(ITabular parent, IColumn aliasedColumn, string alias)
	{
		if (aliasedColumn == null)
		{
			throw new ArgumentNullException("aliasedColumn");
		}
		if (alias == null)
		{
			throw new ArgumentNullException("alias");
		}
		return new ColumnAlias(parent, aliasedColumn, alias);
	}

	public IColumn CreateScalarAlias(IScalar aliasedScalar, string alias)
	{
		return CreateScalarAlias(null, aliasedScalar, alias);
	}

	public IColumn CreateScalarAlias(ITabular parent, IScalar aliasedScalar, string alias)
	{
		if (aliasedScalar == null)
		{
			throw new ArgumentNullException("aliasedScalar");
		}
		if (alias == null)
		{
			throw new ArgumentNullException("alias");
		}
		return new SimpleColumn(parent, alias, aliasedScalar.DataType, aliasedScalar.Nullable);
	}
}
