using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlOutputClause : SqlCodeObject
{
	private IBindingContext bindingContext;

	private readonly SqlSelectExpressionCollection outputExpressions;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => bindingContext;

	public SqlSelectExpressionCollection OutputExpressions => outputExpressions;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlSelectExpression outputExpression in outputExpressions)
			{
				yield return outputExpression;
			}
		}
	}

	internal virtual bool IsSqlOutputIntoClause => false;

	internal virtual SqlOutputIntoClause AsSqlOutputIntoClause => null;

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
		this.bindingContext = bindingContext;
		foreach (SqlSelectExpression outputExpression in outputExpressions)
		{
			outputExpression.Bind(bindingContext);
			base.BindingState |= outputExpression.BindingState;
			SqlScalarExpression sqlScalarExpression = outputExpression.AsSelectScalarExpression?.Expression;
			if (sqlScalarExpression == null)
			{
				continue;
			}
			if (sqlScalarExpression.IsSubQuery)
			{
				sqlScalarExpression.ReportBindError(SR.SubqueriesNotAllowedInOutput);
				base.BindingState |= BindingState.BindFailed;
			}
			else
			{
				if (!sqlScalarExpression.IsFunction || !(sqlScalarExpression.BoundScalar is IScalarFunction scalarFunction))
				{
					continue;
				}
				if (scalarFunction.IsAggregateFunction)
				{
					sqlScalarExpression.ReportBindError(SR.AggregateNotAllowedInOutput);
					base.BindingState |= BindingState.BindFailed;
					continue;
				}
				IUserDefinedFunction userDefinedFunction = BindUtils.ScalarFunction.AsUserDefinedFunction(scalarFunction);
				if (userDefinedFunction != null && !userDefinedFunction.IsSchemaBound)
				{
					SqlObjectIdentifier objectIdentifier = (sqlScalarExpression as SqlUserDefinedScalarFunctionCallExpression).ObjectIdentifier;
					objectIdentifier.ReportBindError(SR.FunctionNotAllowedInOutput(objectIdentifier.ToString()));
				}
			}
		}
	}

	internal override void Unbind()
	{
		base.Unbind();
		bindingContext = null;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return base.GetCompletionList(caretLocation, CompletionContext.SelectScalar | CompletionContext.LocalTableExpressionsOnly);
	}

	internal SqlOutputClause(List<SqlSelectExpression> outputExpressions)
	{
		this.outputExpressions = new SqlSelectExpressionCollection(outputExpressions);
	}

	internal SqlOutputClause(SqlOutputClause temporaryOutputClause)
	{
		outputExpressions = temporaryOutputClause.OutputExpressions;
	}
}
