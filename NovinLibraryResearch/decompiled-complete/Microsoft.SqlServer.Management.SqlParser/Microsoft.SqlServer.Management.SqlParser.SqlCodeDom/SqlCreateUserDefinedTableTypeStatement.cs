using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateUserDefinedTableTypeStatement : SqlCreateTypeStatement
{
	private readonly SqlTableDefinition tableDefinition;

	internal override bool IsVisitable => true;

	public SqlTableDefinition TableDefinition => tableDefinition;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.Name != null)
			{
				yield return base.Name;
			}
			if (tableDefinition != null)
			{
				yield return tableDefinition;
			}
		}
	}

	public override void Accept(ISqlStatementVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlStatementVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void BindType(BatchBindingContext bindingContext, UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> userDefinedType)
	{
		IMutableUserDefinedTableType userDefinedTableType = userDefinedType.UserDefinedTableType;
		TableDefinition.Bind(bindingContext, userDefinedTableType);
	}

	internal override UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> CreateType(IUserDefinedTypeFactory factory, ISchema schema)
	{
		return new UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType>(factory.CreateTableType(schema, base.Name.ObjectName.Value));
	}

	internal SqlCreateUserDefinedTableTypeStatement(SqlObjectIdentifier name, SqlTableDefinition tableDefinition)
		: base(name)
	{
		this.tableDefinition = tableDefinition;
	}
}
