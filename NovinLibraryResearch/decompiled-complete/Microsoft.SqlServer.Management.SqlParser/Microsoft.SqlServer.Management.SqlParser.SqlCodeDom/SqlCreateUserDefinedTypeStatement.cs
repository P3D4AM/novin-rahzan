using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateUserDefinedTypeStatement : SqlCreateTypeStatement
{
	private readonly SqlClrClassSpecifier clrClassSpecifier;

	internal override bool IsVisitable => true;

	public SqlClrClassSpecifier ClrClassSpecifier => clrClassSpecifier;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.Name != null)
			{
				yield return base.Name;
			}
			if (clrClassSpecifier != null)
			{
				yield return clrClassSpecifier;
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
		IMutableUserDefinedClrType userDefinedClrType = userDefinedType.UserDefinedClrType;
		userDefinedClrType.AssemblyName = ClrClassSpecifier.AssemblyName.Value;
		if (ClrClassSpecifier.ClassName != null)
		{
			userDefinedClrType.ClassName = ClrClassSpecifier.ClassName.Value;
		}
		else
		{
			userDefinedClrType.ClassName = base.Name.ObjectName.Value;
		}
		userDefinedClrType.IsNullable = true;
	}

	internal override UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> CreateType(IUserDefinedTypeFactory factory, ISchema schema)
	{
		return new UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType>(factory.CreateClrType(schema, base.Name.ObjectName.Value));
	}

	internal SqlCreateUserDefinedTypeStatement(SqlObjectIdentifier name, SqlClrClassSpecifier clrClassSpecifier)
		: base(name)
	{
		this.clrClassSpecifier = clrClassSpecifier;
	}
}
