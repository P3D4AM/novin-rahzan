using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlIntoClause : SqlCodeObject
{
	private LocalBindingContext localBindingContext;

	private readonly SqlTableExpression target;

	private readonly SqlColumnRefExpressionCollection columns;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => localBindingContext;

	public SqlTableExpression Target => target;

	public SqlColumnRefExpressionCollection Columns => columns;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return target;
			if (columns == null)
			{
				yield break;
			}
			foreach (SqlColumnRefExpression column in columns)
			{
				yield return column;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		localBindingContext = bindingContext.Batch.CreateLocalBindingContext();
		target.Bind(localBindingContext);
		base.BindingState |= target.BindingState;
		ITabular boundTable = target.BoundTable;
		if (boundTable == null)
		{
			return;
		}
		if (BindUtils.Table.IsView(boundTable) || BindUtils.Table.IsCte(boundTable))
		{
			target.ReportBindError(SR.OutputIntoTargetCannotBeViewOrCte(target.ExpressionName.ToString()));
			base.BindingState |= BindingState.BindFailed;
		}
		bool flag = BindUtils.Table.IsTableVariable(boundTable);
		if (columns == null)
		{
			return;
		}
		for (int i = 0; i < columns.Count; i++)
		{
			SqlColumnRefExpression sqlColumnRefExpression = columns[i];
			sqlColumnRefExpression.Bind(localBindingContext);
			IColumn boundColumn = sqlColumnRefExpression.BoundColumn;
			if (boundColumn != null)
			{
				for (int j = 0; j < i; j++)
				{
					if (boundColumn == columns[j].BoundColumn)
					{
						sqlColumnRefExpression.ReportBindError(SR.SetClauseColumnSpecifiedMultipleTimes(sqlColumnRefExpression.ColumnName.Value));
						sqlColumnRefExpression.BindingState |= BindingState.BindFailed;
					}
				}
				if (flag && BindUtils.Column.IsIdentity(boundColumn))
				{
					sqlColumnRefExpression.ReportBindError(SR.InsertIntoIdentityColumnNotAllowed);
					sqlColumnRefExpression.BindingState |= BindingState.BindFailed;
				}
			}
			base.BindingState |= sqlColumnRefExpression.BindingState;
		}
	}

	internal override void Unbind()
	{
		base.Unbind();
		localBindingContext = null;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			result = ((sqlCodeObject != target) ? sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.Columns) : sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.DatabaseQualifiedTables | CompletionContext.TableVariables));
		}
		else if ((columns == null || columns.Count == 0) && IsCaretBetweenEmptyParenthesis(caretLocation))
		{
			result = RetrieveCompletionListItems(CompletionContext.Columns);
		}
		return result;
	}

	internal SqlIntoClause(SqlTableExpression target, List<SqlColumnRefExpression> columns)
	{
		this.target = target;
		if (columns != null)
		{
			this.columns = new SqlColumnRefExpressionCollection(columns);
		}
	}
}
