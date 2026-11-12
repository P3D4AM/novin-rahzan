using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlScalarRelationalFunctionDefinition : SqlFunctionDefinition
{
	private static readonly SqlModuleOptionMask ValidOptions = SqlModuleOptionMask.NullInputOptions | SqlModuleOptionMask.CreateTrigger | SqlModuleOptionMask.SchemaBinding;

	private readonly SqlScalarFunctionReturnType returnType;

	private readonly SqlMultistatementFunctionBodyDefinition bodyDefinition;

	internal override bool IsVisitable => true;

	internal sealed override bool IsTableValuedFunction => false;

	internal sealed override bool IsInlineTableFunction => false;

	internal sealed override bool IsClrFunction => false;

	public SqlScalarFunctionReturnType ReturnType => returnType;

	public SqlMultistatementFunctionBodyDefinition BodyDefinition => bodyDefinition;

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

	internal SqlScalarRelationalFunctionDefinition(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters, SqlScalarFunctionReturnType returnType, List<SqlModuleOption> options, SqlMultistatementFunctionBodyDefinition bodyDefinition)
		: base(name, parameters, options, ValidOptions)
	{
		this.returnType = returnType;
		this.bodyDefinition = bodyDefinition;
		BodyDefinition.CompoundStatement.Accept(ValidateModuleBodyVisitor.ScalarFunctionParserVisitor);
		SqlStatementCollection statements = BodyDefinition.CompoundStatement.Statements;
		if (!((statements.Count > 0) ? statements[statements.Count - 1] : null).IsReturnStatement)
		{
			base.Name.ReportParseError(SR.LastStatementWithinFunctionMustBeReturn);
		}
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
