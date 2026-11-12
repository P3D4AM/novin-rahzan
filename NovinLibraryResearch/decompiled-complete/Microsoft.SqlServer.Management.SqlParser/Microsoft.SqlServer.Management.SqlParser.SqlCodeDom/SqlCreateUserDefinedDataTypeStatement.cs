using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCreateUserDefinedDataTypeStatement : SqlCreateTypeStatement
{
	private readonly SqlDataTypeSpecification dataType;

	private readonly SqlUserDefinedDataTypeNullState nullState;

	internal override bool IsVisitable => true;

	public SqlDataTypeSpecification DataType => dataType;

	public SqlUserDefinedDataTypeNullState NullState => nullState;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.Name != null)
			{
				yield return base.Name;
			}
			if (dataType != null)
			{
				yield return dataType;
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
		IMutableUserDefinedDataType userDefinedDataType = userDefinedType.UserDefinedDataType;
		DataType.Bind(bindingContext);
		ISystemDataType systemDataType = null;
		if (DataType.BoundType != null && DataType.BoundType.IsScalar && BindUtils.DataType.AsScalarDataType(DataType.BoundType).IsSystem)
		{
			ISystemDataType baseSystemDataType = BindUtils.DataType.AsScalarDataType(DataType.BoundType).BaseSystemDataType;
			if (!baseSystemDataType.IsClr && !baseSystemDataType.IsXml)
			{
				systemDataType = baseSystemDataType;
			}
		}
		userDefinedDataType.Nullable = NullState != SqlUserDefinedDataTypeNullState.NotNull;
		if (systemDataType != null)
		{
			userDefinedDataType.BaseSystemDataType = systemDataType;
			return;
		}
		userDefinedDataType.IsUnknown = true;
		DataType.ReportBindError(SR.InvalidBaseTypeForAlias(DataType.DataType.ObjectIdentifier.ObjectName.Value));
		base.BindingState |= BindingState.BindFailed;
	}

	internal override UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType> CreateType(IUserDefinedTypeFactory factory, ISchema schema)
	{
		return new UserDefinedTypeUnion<IMutableUserDefinedDataType, IMutableUserDefinedTableType, IMutableUserDefinedClrType, IUserDefinedType>(factory.CreateDataType(schema, base.Name.ObjectName.Value));
	}

	internal SqlCreateUserDefinedDataTypeStatement(SqlObjectIdentifier name, SqlDataTypeSpecification dataType, SqlUserDefinedDataTypeNullState nullState)
		: base(name)
	{
		this.dataType = dataType;
		this.nullState = nullState;
	}
}
