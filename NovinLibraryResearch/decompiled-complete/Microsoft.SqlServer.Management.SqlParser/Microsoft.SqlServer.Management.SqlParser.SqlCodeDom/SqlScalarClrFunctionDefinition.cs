using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlScalarClrFunctionDefinition : SqlFunctionDefinition
{
	private const SqlModuleOptionMask ValidOptions = SqlModuleOptionMask.NullInputOptions | SqlModuleOptionMask.ExecuteAs;

	private readonly SqlScalarFunctionReturnType returnType;

	private readonly SqlClrFunctionBodyDefinition bodyDefinition;

	internal override bool IsVisitable => true;

	internal sealed override bool IsTableValuedFunction => false;

	internal sealed override bool IsInlineTableFunction => false;

	internal sealed override bool IsClrFunction => true;

	public SqlScalarFunctionReturnType ReturnType => returnType;

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

	internal SqlScalarClrFunctionDefinition(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters, SqlScalarFunctionReturnType returnType, List<SqlModuleOption> options, SqlClrFunctionBodyDefinition bodyDefinition)
		: base(name, parameters, options, SqlModuleOptionMask.NullInputOptions | SqlModuleOptionMask.ExecuteAs)
	{
		this.returnType = returnType;
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
