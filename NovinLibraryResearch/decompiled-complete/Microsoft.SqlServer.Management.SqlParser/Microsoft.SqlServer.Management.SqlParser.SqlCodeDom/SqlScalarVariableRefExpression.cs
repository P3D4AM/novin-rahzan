using System;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlScalarVariableRefExpression : SqlScalarExpression
{
	private ILocalVariable boundVariable;

	private readonly string variableName;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public ILocalVariable BoundVariable => boundVariable;

	public override IScalar BoundScalar => boundVariable as IScalar;

	public override IMetadataObject BoundObject => boundVariable;

	public string VariableName => variableName;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.VariableReference;

	internal override bool IsConstantExpression => false;

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
		Bind(bindingContext, allowTableVariables: false);
	}

	internal void Bind(IBindingContext bindingContext, bool allowTableVariables)
	{
		string name = variableName;
		LexicalScope lexicalScope = GetLexicalScope();
		if (allowTableVariables)
		{
			boundVariable = bindingContext.FindLocalVariable(name, lexicalScope);
		}
		else
		{
			boundVariable = bindingContext.FindLocalScalarVariable(name, lexicalScope);
			if (boundVariable == null)
			{
				boundVariable = bindingContext.FindLocalCursorVariable(name, lexicalScope);
			}
		}
		if (boundVariable == null)
		{
			ReportBindError(SR.ScalarVariableRequired(variableName), ErrorFlags.UnresolvedReference);
			base.BindingState = BindingState.BindFailed;
		}
		else
		{
			base.BindingState = BindingState.BindSucceeded;
		}
	}

	internal override void Unbind()
	{
		boundVariable = null;
		base.Unbind();
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		if (boundVariable == null)
		{
			return null;
		}
		return displayInfoProvider.GetDescription(boundVariable);
	}

	internal SqlScalarVariableRefExpression(string variableName)
	{
		this.variableName = variableName;
	}

	public static SqlScalarVariableRefExpression Create(string variableName)
	{
		if (variableName == null)
		{
			return new SqlScalarVariableRefExpressionError();
		}
		return new SqlScalarVariableRefExpression(variableName);
	}
}
