using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlScalarVariableAssignment : SqlVariableAssignment
{
	private readonly SqlScalarVariableRefExpression variable;

	private readonly SqlScalarExpression value;

	internal override bool IsVisitable => true;

	public SqlScalarVariableRefExpression Variable => variable;

	public SqlScalarExpression Value => value;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return variable;
			yield return Value;
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

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject == variable)
		{
			return RetrieveCompletionListItems(CompletionContext.ScalarVariables);
		}
		if (sqlCodeObject == Value)
		{
			return Value.GetCompletionList(caretLocation, completionContext | CompletionContext.Scalar);
		}
		return null;
	}

	internal SqlScalarVariableAssignment(SqlScalarVariableRefExpression variable, SqlAssignmentOperatorType operatorType, SqlScalarExpression value)
		: base(operatorType)
	{
		this.variable = variable;
		this.value = value;
	}
}
