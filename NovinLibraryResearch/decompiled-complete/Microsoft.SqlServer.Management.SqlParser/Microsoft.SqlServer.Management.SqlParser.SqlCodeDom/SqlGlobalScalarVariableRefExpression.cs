using System;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlGlobalScalarVariableRefExpression : SqlScalarExpression
{
	private IBuiltInFunction boundVariable;

	private readonly string variableName;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public IBuiltInFunction BoundVariable => boundVariable;

	public override IScalar BoundScalar => boundVariable;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.GlobalVariableReference;

	internal override bool IsConstantExpression => false;

	public string VariableName => variableName;

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
		boundVariable = bindingContext.Batch.FindGlobalVariable(VariableName);
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

	internal SqlGlobalScalarVariableRefExpression(string variableName)
	{
		this.variableName = variableName;
	}
}
