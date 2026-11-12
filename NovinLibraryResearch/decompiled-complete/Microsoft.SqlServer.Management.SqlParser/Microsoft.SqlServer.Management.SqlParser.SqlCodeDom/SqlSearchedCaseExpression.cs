using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSearchedCaseExpression : SqlCaseExpression
{
	private SqlSearchedWhenClauseCollection whenClauses;

	internal override bool IsVisitable => true;

	public SqlSearchedWhenClauseCollection WhenClauses => whenClauses;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlSearchedWhenClause whenClause in whenClauses)
			{
				yield return whenClause;
			}
			if (elseExpression != null)
			{
				yield return elseExpression;
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

	internal override IEnumerable<SqlScalarExpression> RetrieveResultScalarExpressions()
	{
		foreach (SqlSearchedWhenClause whenClause in whenClauses)
		{
			yield return whenClause;
		}
		if (elseExpression != null)
		{
			yield return elseExpression;
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> metadataCollection = null;
		if (caretLocation.startTokenNumber == caretLocation.endTokenNumber && caretLocation.startTokenNumber == base.Position.startTokenNumber + 1)
		{
			return RetrieveCompletionListItems(completionContext);
		}
		return base.GetCompletionList(caretLocation, completionContext);
	}

	internal SqlSearchedCaseExpression(List<SqlSearchedWhenClause> whenClauses, SqlScalarExpression elseExpression)
		: base(elseExpression)
	{
		this.whenClauses = new SqlSearchedWhenClauseCollection(whenClauses);
	}
}
