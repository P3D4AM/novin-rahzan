using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlStatementError : SqlStatement, ISqlErrorObject
{
	internal List<SqlCodeObject> children;

	internal override bool IsVisitable => true;

	internal sealed override string StatementPhrase => null;

	private IEnumerable<SqlStatement> ChildStatements
	{
		get
		{
			for (int i = 0; i < children.Count; i++)
			{
				if (children[i] is SqlStatement sqlStatement)
				{
					yield return sqlStatement;
				}
			}
		}
	}

	public override IEnumerable<SqlCodeObject> Children => children;

	public override void Accept(ISqlStatementVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlStatementVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlStatementContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlStatementContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
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

	internal override void BindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		for (int i = 0; i < children.Count; i++)
		{
			SqlCodeObject sqlCodeObject = children[i];
			if (sqlCodeObject.CanBeBoundIndependently)
			{
				sqlCodeObject.Bind(bindingContext);
				base.BindingState |= sqlCodeObject.BindingState;
			}
		}
		base.BindingState |= BindingState.BindSucceeded;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null && sqlCodeObject.IsBound)
		{
			result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
		}
		return result;
	}

	internal SqlStatementError()
	{
		children = new List<SqlCodeObject>();
	}

	internal SqlStatementError(SqlErrorCodeObject errorCodeObject)
		: this()
	{
		children.AddRange(errorCodeObject.Children);
	}

	internal SqlStatementError(SqlCodeObject codeObject, SqlErrorCodeObject errorCodeObject)
		: this()
	{
		children.Add(codeObject);
		children.AddRange(errorCodeObject.Children);
	}
}
