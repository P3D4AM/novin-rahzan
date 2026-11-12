using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlNotBooleanExpression : SqlBooleanExpression
{
	private SqlBooleanExpression expression;

	internal override bool IsVisitable => true;

	public SqlBooleanExpression Expression => expression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return expression;
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
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		else if (caretLocation.StartsBefore(expression.Position) && caretLocation.startTokenNumber == caretLocation.endTokenNumber)
		{
			result = RetrieveCompletionListItems(completionContext);
		}
		return result;
	}

	internal SqlNotBooleanExpression(SqlBooleanExpression expression)
	{
		this.expression = expression;
	}
}
