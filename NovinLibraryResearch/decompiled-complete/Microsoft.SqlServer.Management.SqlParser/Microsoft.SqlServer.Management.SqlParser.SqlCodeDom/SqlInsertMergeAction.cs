using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInsertMergeAction : SqlMergeAction
{
	private readonly SqlColumnRefExpressionCollection targetColumns;

	private readonly SqlInsertMergeActionSource source;

	internal override bool IsVisitable => true;

	public SqlColumnRefExpressionCollection TargetColumns => targetColumns;

	public SqlInsertMergeActionSource Source => source;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (targetColumns != null)
			{
				foreach (SqlColumnRefExpression targetColumn in targetColumns)
				{
					yield return targetColumn;
				}
			}
			yield return source;
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
		if (targetColumns != null)
		{
			bindingContext.EnterTempBindMode(TemporaryBindMode.MergeActionTarget);
			for (int i = 0; i < targetColumns.Count; i++)
			{
				SqlColumnRefExpression sqlColumnRefExpression = targetColumns[i];
				sqlColumnRefExpression.Bind(bindingContext);
				base.BindingState |= sqlColumnRefExpression.BindingState;
			}
			bindingContext.LeaveTempBindMode(TemporaryBindMode.MergeActionTarget);
		}
		bindingContext.EnterTempBindMode(TemporaryBindMode.MergeActionSource);
		source.Bind(bindingContext);
		bindingContext.LeaveTempBindMode(TemporaryBindMode.MergeActionSource);
		base.BindingState |= source.BindingState;
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref caretLocation);
		if (sqlCodeObject != null)
		{
			IBindingContext bindingContext = BindingContext;
			if (sqlCodeObject != source)
			{
				bindingContext.EnterTempBindMode(TemporaryBindMode.MergeActionTarget);
				result = sqlCodeObject.GetCompletionList(caretLocation, CompletionContext.Columns);
				bindingContext.LeaveTempBindMode(TemporaryBindMode.MergeActionTarget);
			}
			else
			{
				bindingContext.EnterTempBindMode(TemporaryBindMode.MergeActionSource);
				result = sqlCodeObject.GetCompletionList(caretLocation, completionContext);
				bindingContext.LeaveTempBindMode(TemporaryBindMode.MergeActionSource);
			}
		}
		return result;
	}

	internal SqlInsertMergeAction(List<SqlColumnRefExpression> targetColumns, SqlInsertMergeActionSource source)
	{
		if (targetColumns != null)
		{
			this.targetColumns = new SqlColumnRefExpressionCollection(targetColumns);
		}
		this.source = source;
	}
}
