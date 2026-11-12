using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUdtStaticMethodExpression : SqlUdtStaticMemberExpression
{
	private readonly SqlScalarExpressionCollection arguments;

	internal override bool IsVisitable => true;

	public SqlScalarExpressionCollection Arguments => arguments;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Type;
			yield return base.MemberName;
			if (Arguments == null)
			{
				yield break;
			}
			foreach (SqlScalarExpression argument in Arguments)
			{
				yield return argument;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		IClrDataType clrDataType = BindType(bindingContext);
		if (clrDataType != null)
		{
			BindUdtMethod(bindingContext, clrDataType, isStatic: true, arguments);
		}
	}

	internal SqlUdtStaticMethodExpression(SqlDataTypeSpecification type, SqlIdentifier methodName, List<SqlScalarExpression> arguments)
		: base(type, methodName)
	{
		this.arguments = ((arguments != null) ? new SqlScalarExpressionCollection(arguments) : null);
	}
}
