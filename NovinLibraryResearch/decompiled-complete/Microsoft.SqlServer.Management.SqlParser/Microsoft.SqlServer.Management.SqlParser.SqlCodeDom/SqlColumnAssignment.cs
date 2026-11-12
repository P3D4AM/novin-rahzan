using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlColumnAssignment : SqlAssignment
{
	private readonly SqlScalarRefExpression column;

	private readonly SqlScalarExpression value;

	internal override bool IsVisitable => true;

	public SqlScalarRefExpression Column => column;

	public SqlScalarExpression Value => value;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return column;
			yield return value;
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
		bindingContext.EnterTempBindMode(TemporaryBindMode.ColumnAssignmentTarget);
		column.Bind(bindingContext);
		bindingContext.LeaveTempBindMode(TemporaryBindMode.ColumnAssignmentTarget);
		base.BindingState = column.BindingState;
		Value.Bind(bindingContext);
		base.BindingState = Value.BindingState;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == column)
		{
			IBindingContext bindingContext = BindingContext;
			bindingContext.EnterTempBindMode(TemporaryBindMode.ColumnAssignmentTarget);
			result = column.GetCompletionList(caretLocation, CompletionContext.ScalarVariables | CompletionContext.Columns);
			bindingContext.LeaveTempBindMode(TemporaryBindMode.ColumnAssignmentTarget);
		}
		else if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.SelectScalar);
		}
		return result;
	}

	internal SqlColumnAssignment(SqlScalarRefExpression column, SqlAssignmentOperatorType operatorType, SqlScalarExpression expr)
		: base(operatorType)
	{
		this.column = column;
		value = expr;
	}
}
