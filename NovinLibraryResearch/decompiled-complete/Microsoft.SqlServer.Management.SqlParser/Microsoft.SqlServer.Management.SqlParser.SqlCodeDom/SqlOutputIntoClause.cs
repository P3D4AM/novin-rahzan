using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlOutputIntoClause : SqlOutputClause
{
	private readonly SqlIntoClause intoClause;

	internal override bool IsVisitable => true;

	internal override bool IsSqlOutputIntoClause => true;

	internal override SqlOutputIntoClause AsSqlOutputIntoClause => this;

	public SqlIntoClause IntoClause => intoClause;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlSelectExpression outputExpression in base.OutputExpressions)
			{
				yield return outputExpression;
			}
			yield return intoClause;
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
		base.Bind(bindingContext);
		intoClause.Bind(bindingContext);
		base.BindingState |= intoClause.BindingState;
		if (base.HasBindingErrors)
		{
			return;
		}
		int count = bindingContext.LocalColumnExpressions.Count;
		string text = null;
		if (intoClause.Columns == null)
		{
			ITabular boundTable = intoClause.Target.BoundTable;
			bool flag = false;
			foreach (IColumn column in boundTable.Columns)
			{
				if (BindUtils.Column.IsIdentity(column))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int num = boundTable.Columns.Count - 1;
				if (count > num)
				{
					text = SR.ExplicitValueForIdentityColumn(intoClause.Target.ExpressionName.ToString());
				}
			}
			else
			{
				int count2 = boundTable.Columns.Count;
				if (count > count2)
				{
					text = SR.NumberOfValuesDoesNotMatchTableDef;
				}
			}
		}
		else
		{
			int count3 = intoClause.Columns.Count;
			if (count < count3)
			{
				text = SR.SelectListOfInsertHasFewerItems;
			}
			else if (count > count3)
			{
				text = SR.SelectListOfInsertHasMoreItems;
			}
		}
		if (text != null)
		{
			SqlCodeObject.ReportBindError(base.OutputExpressions, text);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal SqlOutputIntoClause(SqlOutputClause outputClause, SqlIntoClause intoClause)
		: base(outputClause)
	{
		this.intoClause = intoClause;
	}
}
