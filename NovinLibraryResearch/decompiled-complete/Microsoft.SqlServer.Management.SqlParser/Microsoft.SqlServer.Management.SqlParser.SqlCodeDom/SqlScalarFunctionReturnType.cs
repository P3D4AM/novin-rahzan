using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlScalarFunctionReturnType : SqlFunctionReturnType
{
	private readonly SqlDataTypeSpecification dataType;

	internal override bool IsVisitable => true;

	internal sealed override SqlScalarFunctionReturnType AsSqlScalarFunctionReturnType => this;

	internal sealed override SqlTableFunctionReturnType AsSqlTableFunctionReturnType => null;

	public SqlDataTypeSpecification DataType => dataType;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return dataType;
		}
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

	internal override void Bind(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion)
	{
		Bind(bindingContext, functionUnion.ScalarValuedFunction);
	}

	private void Bind(BatchBindingContext bindingContext, IMutableScalarValuedFunction function)
	{
		dataType.Bind(bindingContext);
		base.BindingState |= dataType.BindingState;
		IScalarDataType scalarDataType = null;
		if (dataType.BoundType != null)
		{
			scalarDataType = BindUtils.DataType.AsScalarDataType(dataType.BoundType);
		}
		if (scalarDataType == null)
		{
			scalarDataType = bindingContext.MetadataFactory.DataType.UnknownScalar;
		}
		if (function != null)
		{
			function.ReturnType = scalarDataType;
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal SqlScalarFunctionReturnType(SqlDataTypeSpecification dataType)
	{
		this.dataType = dataType;
	}
}
