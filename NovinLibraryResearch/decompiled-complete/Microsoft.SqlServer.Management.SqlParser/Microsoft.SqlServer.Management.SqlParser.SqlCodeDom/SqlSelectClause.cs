using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlSelectClause : SqlCodeObject
{
	private readonly SqlTopSpecification topSpecification;

	private readonly bool distinct;

	private readonly SqlSelectExpressionCollection selectExpressions;

	internal override bool IsVisitable => true;

	public SqlTopSpecification Top => topSpecification;

	public bool IsDistinct => distinct;

	public SqlSelectExpressionCollection SelectExpressions => selectExpressions;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (topSpecification != null)
			{
				yield return topSpecification;
			}
			foreach (SqlSelectExpression selectExpression in selectExpressions)
			{
				yield return selectExpression;
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

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			if (sqlCodeObject == topSpecification && caretLocation.startTokenNumber == topSpecification.Position.endTokenNumber)
			{
				sqlCodeObject = selectExpressions[0];
				caretLocation = new LexLocation(sqlCodeObject.Position.startTokenNumber, sqlCodeObject.Position.startTokenNumber);
			}
			result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.SelectScalar);
		}
		return result;
	}

	internal SqlSelectClause(bool isDistinct, SqlTopSpecification top, List<SqlSelectExpression> expressions)
	{
		topSpecification = top;
		distinct = isDistinct;
		selectExpressions = new SqlSelectExpressionCollection(expressions);
		int num = 0;
		foreach (SqlSelectExpression selectExpression in selectExpressions)
		{
			if (selectExpression.IsVariableAssignment)
			{
				num++;
			}
		}
		if (num <= 0 || num >= selectExpressions.Count)
		{
			return;
		}
		foreach (SqlSelectExpression selectExpression2 in selectExpressions)
		{
			if (!selectExpression2.IsVariableAssignment)
			{
				selectExpression2.ReportParseError(SR.SelectAssignmentError);
				break;
			}
		}
	}
}
