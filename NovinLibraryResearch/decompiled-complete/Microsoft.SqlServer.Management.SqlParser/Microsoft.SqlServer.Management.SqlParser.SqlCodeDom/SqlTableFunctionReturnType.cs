using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableFunctionReturnType : SqlFunctionReturnType
{
	private readonly SqlTableDefinition tableDefinition;

	internal override bool IsVisitable => true;

	internal sealed override SqlScalarFunctionReturnType AsSqlScalarFunctionReturnType => null;

	internal sealed override SqlTableFunctionReturnType AsSqlTableFunctionReturnType => this;

	public SqlTableDefinition TableDefinition => tableDefinition;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return tableDefinition;
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
		IMutableTableValuedFunction tableValuedFunction = functionUnion.TableValuedFunction;
		tableDefinition.Bind(bindingContext, tableValuedFunction);
		base.BindingState |= tableDefinition.BindingState;
	}

	internal SqlTableFunctionReturnType(SqlTableDefinition tableDefinition)
	{
		this.tableDefinition = tableDefinition;
	}
}
