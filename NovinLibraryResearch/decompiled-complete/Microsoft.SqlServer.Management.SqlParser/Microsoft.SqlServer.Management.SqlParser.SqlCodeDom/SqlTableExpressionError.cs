using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableExpressionError : SqlTableExpression, ISqlErrorObject
{
	internal List<SqlCodeObject> children;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName => default(TableExpressionName);

	private IEnumerable<SqlIdentifier> Identifiers
	{
		get
		{
			foreach (SqlCodeObject child in children)
			{
				if (child is SqlIdentifier sqlIdentifier)
				{
					yield return sqlIdentifier;
				}
			}
		}
	}

	public override IEnumerable<SqlCodeObject> Children => children.AsReadOnly();

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

	internal override void BindDmlTarget(DmlSpecificationBindingContext bindingContext)
	{
		base.BindingState |= BindingState.BindFailed;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation lexLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> metadataCollection = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref lexLocation);
		if (sqlCodeObject == null || sqlCodeObject is SqlIdentifier)
		{
			return RetrieveCompletionListItems(lexLocation, Identifiers, completionContext);
		}
		return sqlCodeObject.GetCompletionList(lexLocation, completionContext);
	}

	internal SqlTableExpressionError(SqlErrorCodeObject errorCodeObject)
	{
		children = new List<SqlCodeObject>();
		children.AddRange(errorCodeObject.Children);
	}
}
