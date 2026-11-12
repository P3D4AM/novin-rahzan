using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlQuerySpecification : SqlQueryExpression
{
	private LocalBindingContext bindingContext;

	private readonly SqlSelectClause selectClause;

	private readonly SqlSelectIntoClause intoClause;

	private readonly SqlFromClause fromClause;

	private readonly SqlWhereClause whereClause;

	private readonly SqlGroupByClause groupByClause;

	private readonly SqlHavingClause havingClause;

	private readonly SqlOrderByClause orderByClause;

	private readonly SqlForClause forClause;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => bindingContext;

	internal override LocalBindingContext LocalBindingContext => bindingContext;

	internal override bool HasAnyBindSkipped
	{
		get
		{
			if (fromClause != null)
			{
				return (fromClause.BindingState & BindingState.BindSkipped) == BindingState.BindSkipped;
			}
			return false;
		}
	}

	internal override bool HasUnionAll => false;

	public SqlSelectClause SelectClause => selectClause;

	public SqlSelectIntoClause IntoClause => intoClause;

	public SqlFromClause FromClause => fromClause;

	public SqlWhereClause WhereClause => whereClause;

	public SqlGroupByClause GroupByClause => groupByClause;

	public SqlHavingClause HavingClause => havingClause;

	public SqlOrderByClause OrderByClause => orderByClause;

	public SqlForClause ForClause => forClause;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return selectClause;
			if (intoClause != null)
			{
				yield return intoClause;
			}
			if (fromClause != null)
			{
				yield return fromClause;
			}
			if (whereClause != null)
			{
				yield return whereClause;
			}
			if (groupByClause != null)
			{
				yield return groupByClause;
			}
			if (havingClause != null)
			{
				yield return havingClause;
			}
			if (orderByClause != null)
			{
				yield return orderByClause;
			}
			if (forClause != null)
			{
				yield return forClause;
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
		LocalBindingContext localBindingContext = (this.bindingContext = bindingContext.CreateLocalBindingContext());
		if (fromClause != null)
		{
			fromClause.Bind(localBindingContext);
			base.BindingState |= fromClause.BindingState;
		}
		selectClause.Bind(localBindingContext);
		base.BindingState |= selectClause.BindingState;
		if (whereClause != null)
		{
			whereClause.Bind(localBindingContext);
			base.BindingState |= whereClause.BindingState;
		}
		if (havingClause != null)
		{
			havingClause.Bind(localBindingContext);
			base.BindingState |= havingClause.BindingState;
		}
		if (groupByClause != null)
		{
			groupByClause.Bind(localBindingContext);
			base.BindingState |= groupByClause.BindingState;
		}
		if (orderByClause != null)
		{
			orderByClause.Bind(localBindingContext);
			base.BindingState |= orderByClause.BindingState;
		}
	}

	internal override void ShallowBind(CteBindingContext bindingContext)
	{
		Bind(bindingContext);
	}

	internal override void DeepBind(CteBindingContext bindingContext, SqlIdentifier cteName)
	{
	}

	internal override void Unbind()
	{
		base.Unbind();
		bindingContext = null;
	}

	private SqlQuerySpecification(SqlSelectClause selectClause, SqlSelectIntoClause intoClause, SqlFromClause fromClause, SqlWhereClause whereClause, SqlGroupByClause groupByClause, SqlHavingClause havingClause, SqlOrderByClause orderByClause, SqlForClause forClause)
	{
		this.selectClause = selectClause;
		this.intoClause = intoClause;
		this.fromClause = fromClause;
		this.whereClause = whereClause;
		this.havingClause = havingClause;
		this.forClause = forClause;
		this.orderByClause = orderByClause;
		this.groupByClause = groupByClause;
	}

	internal SqlQuerySpecification(SqlSelectClause selectClause, SqlFromClause fromClause, SqlWhereClause whereClause, SqlGroupByClause groupByClause, SqlHavingClause havingClause, SqlOrderByClause orderByClause, SqlForClause forClause)
		: this(selectClause, null, fromClause, whereClause, groupByClause, havingClause, orderByClause, forClause)
	{
	}

	internal SqlQuerySpecification(SqlSelectClause selectClause, SqlSelectIntoClause intoClause, SqlFromClause fromClause, SqlWhereClause whereClause, SqlGroupByClause groupByClause, SqlHavingClause havingClause)
		: this(selectClause, intoClause, fromClause, whereClause, groupByClause, havingClause, null, null)
	{
	}

	internal override SqlSelectIntoClause GetIntoClause()
	{
		return intoClause;
	}

	internal override void Validate(QueryExpressionValidateOption option)
	{
		if (intoClause != null)
		{
			switch (option)
			{
			case QueryExpressionValidateOption.IntoClauseSyntaxError:
				intoClause.ReportSyntaxErrorOnFirstToken();
				break;
			case QueryExpressionValidateOption.IntoClauseDisallow:
				intoClause.ReportParseError(SR.SelectIntoMustBeFirstQuery);
				break;
			case QueryExpressionValidateOption.IntoClauseAllowLeftmost:
				break;
			}
		}
	}
}
