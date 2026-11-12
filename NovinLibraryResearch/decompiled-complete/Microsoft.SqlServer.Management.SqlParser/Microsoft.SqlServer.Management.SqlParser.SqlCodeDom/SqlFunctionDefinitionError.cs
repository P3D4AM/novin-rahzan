using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlFunctionDefinitionError : SqlFunctionDefinition, ISqlErrorObject
{
	private readonly SqlFunctionReturnType returnType;

	private readonly SqlFunctionBodyDefinition bodyDefinition;

	internal override bool IsVisitable => true;

	internal sealed override bool IsTableValuedFunction
	{
		get
		{
			if (returnType == null || returnType.AsSqlTableFunctionReturnType == null)
			{
				if (BodyDefinition != null)
				{
					return BodyDefinition.AsSqlInlineFunctionBodyDefinition != null;
				}
				return false;
			}
			return true;
		}
	}

	internal sealed override bool IsInlineTableFunction
	{
		get
		{
			if (BodyDefinition != null)
			{
				return BodyDefinition.AsSqlInlineFunctionBodyDefinition != null;
			}
			return false;
		}
	}

	internal sealed override bool IsClrFunction
	{
		get
		{
			if (BodyDefinition != null)
			{
				return BodyDefinition.AsSqlClrFunctionBodyDefinition != null;
			}
			return false;
		}
	}

	public SqlFunctionReturnType ReturnType => returnType;

	public SqlFunctionBodyDefinition BodyDefinition => bodyDefinition;

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
			if (returnType != null)
			{
				yield return returnType;
			}
			if (options != null)
			{
				foreach (SqlModuleOption option in options)
				{
					yield return option;
				}
			}
			if (bodyDefinition != null)
			{
				yield return bodyDefinition;
			}
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

	internal SqlFunctionDefinitionError(SqlObjectIdentifier name, List<SqlParameterDeclaration> parameters, SqlFunctionReturnType returnType, List<SqlModuleOption> options, SqlFunctionBodyDefinition bodyDefinition)
		: base(name, parameters, options, (SqlModuleOptionMask)(-1))
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
