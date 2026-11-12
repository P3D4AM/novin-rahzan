using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInlineTableRelationalFunctionDefinition : SqlFunctionDefinition
{
	private const SqlModuleOptionMask ValidOptions = SqlModuleOptionMask.Encryption | SqlModuleOptionMask.SchemaBinding | SqlModuleOptionMask.NativeCompilation;

	private readonly SqlInlineFunctionBodyDefinition bodyDefinition;

	internal override bool IsVisitable => true;

	internal sealed override bool IsTableValuedFunction => true;

	internal sealed override bool IsInlineTableFunction => true;

	internal sealed override bool IsClrFunction => false;

	public SqlInlineFunctionBodyDefinition BodyDefinition => bodyDefinition;

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
			if (options != null)
			{
				foreach (SqlModuleOption option in options)
				{
					yield return option;
				}
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
		bodyDefinition.Bind(bindingContext, functionUnion);
		base.BindingState |= bodyDefinition.BindingState;
		IMutableTableValuedFunction tableValuedFunction = functionUnion.TableValuedFunction;
		BindUtils.Table.PopulateTabularColumns(base.Name.ObjectName, null, bodyDefinition.QueryExpression, bindingContext, tableValuedFunction, BindUtils.Table.ColumnAliasOption.ScalarAlias);
	}

	internal SqlInlineTableRelationalFunctionDefinition(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters, List<SqlModuleOption> options, SqlInlineFunctionBodyDefinition bodyDefinition)
		: base(name, parameters, options, SqlModuleOptionMask.Encryption | SqlModuleOptionMask.SchemaBinding | SqlModuleOptionMask.NativeCompilation)
	{
		this.bodyDefinition = bodyDefinition;
	}

	internal override SqlFunctionReturnType GetReturnType()
	{
		return null;
	}

	internal override SqlFunctionBodyDefinition GetBodyDefinition()
	{
		return bodyDefinition;
	}
}
