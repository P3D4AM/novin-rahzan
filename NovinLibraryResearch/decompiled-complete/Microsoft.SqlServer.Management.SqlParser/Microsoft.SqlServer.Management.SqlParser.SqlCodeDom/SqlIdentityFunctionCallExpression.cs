using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlIdentityFunctionCallExpression : SqlBuiltinScalarFunctionCallExpression
{
	private readonly SqlDataTypeSpecification dataType;

	private readonly SqlLiteralExpression seed;

	private readonly SqlLiteralExpression increment;

	internal override bool IsVisitable => true;

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

	internal override void Bind(IBindingContext bindingContext)
	{
	}

	internal void Bind(IBindingContext bindingContext, SqlIdentifier alias)
	{
		dataType.Bind(bindingContext);
		IDataType boundType = dataType.BoundType;
		if (boundType != null)
		{
			IScalarDataType scalarType = BindUtils.DataType.AsScalarDataType(boundType);
			if (BindUtils.Column.BindColumnIdentitySpec(alias.Value, scalarType, seed, increment, this))
			{
				ISystemDataType baseSystemDataType = BindUtils.DataType.AsScalarDataType(dataType.BoundType).BaseSystemDataType;
				boundFunction = bindingContext.Batch.FindMultiTypeBuiltInFunction(base.FunctionName, baseSystemDataType);
				base.BindingState |= BindingState.BindSucceeded;
			}
		}
		else
		{
			dataType.ReportInvalidTypeBindError(alias.Value, isColumn: true);
		}
		if (boundFunction == null)
		{
			boundFunction = bindingContext.Batch.FindBuiltInFunction(base.FunctionName);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void Unbind()
	{
		boundFunction = null;
		base.Unbind();
	}

	internal SqlIdentityFunctionCallExpression(string name, SqlDataTypeSpecification dataType)
		: base(name)
	{
		this.dataType = dataType;
	}

	internal SqlIdentityFunctionCallExpression(string name, SqlDataTypeSpecification dataType, SqlLiteralExpression seed, SqlLiteralExpression increment)
		: this(name, dataType)
	{
		this.seed = seed;
		this.increment = increment;
	}
}
