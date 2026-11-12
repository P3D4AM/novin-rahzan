using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableClrFunctionDefinition : SqlFunctionDefinition
{
	private const SqlModuleOptionMask ValidOptions = SqlModuleOptionMask.ExecuteAs;

	private readonly SqlTableFunctionReturnType returnType;

	private readonly SqlSimpleOrderByClause orderByClause;

	private readonly SqlClrFunctionBodyDefinition bodyDefinition;

	internal override bool IsVisitable => true;

	internal sealed override bool IsTableValuedFunction => true;

	internal sealed override bool IsInlineTableFunction => false;

	internal sealed override bool IsClrFunction => true;

	public SqlTableFunctionReturnType ReturnType => returnType;

	public SqlSimpleOrderByClause OrderByClause => orderByClause;

	public SqlClrFunctionBodyDefinition BodyDefinition => bodyDefinition;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			if (base.Parameters != null)
			{
				foreach (SqlParameterDeclaration parameter in base.Parameters)
				{
					yield return parameter;
				}
			}
			yield return returnType;
			if (options != null)
			{
				foreach (SqlModuleOption option in options)
				{
					yield return option;
				}
			}
			if (orderByClause != null)
			{
				yield return orderByClause;
			}
			yield return bodyDefinition;
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

	internal override void BindDefinition(BatchBindingContext bindingContext, FunctionUnion<IMutableScalarValuedFunction, IMutableTableValuedFunction, IMutableUserDefinedFunction> functionUnion)
	{
		base.BindDefinition(bindingContext, functionUnion);
		if (orderByClause != null)
		{
			orderByClause.Bind(bindingContext);
			base.BindingState |= orderByClause.BindingState;
		}
	}

	internal SqlTableClrFunctionDefinition(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters, SqlTableFunctionReturnType returnType, List<SqlModuleOption> options, SqlSimpleOrderByClause orderByClause, SqlClrFunctionBodyDefinition bodyDefinition)
		: base(name, parameters, options, SqlModuleOptionMask.ExecuteAs)
	{
		this.returnType = returnType;
		this.orderByClause = orderByClause;
		this.bodyDefinition = bodyDefinition;
	}

	internal override SqlFunctionReturnType GetReturnType()
	{
		return returnType;
	}

	internal override SqlFunctionBodyDefinition GetBodyDefinition()
	{
		return bodyDefinition;
	}
}
