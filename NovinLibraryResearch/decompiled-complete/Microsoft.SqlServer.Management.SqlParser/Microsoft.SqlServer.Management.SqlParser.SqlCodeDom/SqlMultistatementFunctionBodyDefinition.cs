using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMultistatementFunctionBodyDefinition : SqlFunctionBodyDefinition
{
	private readonly SqlCompoundStatement compoundStatement;

	internal override bool IsVisitable => true;

	internal sealed override SqlClrFunctionBodyDefinition AsSqlClrFunctionBodyDefinition => null;

	internal sealed override SqlInlineFunctionBodyDefinition AsSqlInlineFunctionBodyDefinition => null;

	internal sealed override SqlMultistatementFunctionBodyDefinition AsSqlMultistatementFunctionBodyDefinition => this;

	public SqlCompoundStatement CompoundStatement => compoundStatement;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return compoundStatement;
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
		CompoundStatement.Bind(bindingContext);
		base.BindingState |= CompoundStatement.BindingState;
		CompoundStatement.Accept(ValidateModuleBodyVisitor.FunctionBinderVisitor);
	}

	internal SqlMultistatementFunctionBodyDefinition(SqlCompoundStatement compoundStatement)
	{
		this.compoundStatement = compoundStatement;
	}
}
