using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCheckConstraint : SqlConstraint, ISqlTableElement
{
	private LocalBindingContext localBindingContext;

	private readonly bool notForReplication;

	private readonly SqlBooleanExpression expression;

	internal override bool IsVisitable => true;

	internal override IBindingContext BindingContext => localBindingContext;

	public bool NotForReplication => notForReplication;

	public SqlBooleanExpression Expression => expression;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.Name != null)
			{
				yield return base.Name;
			}
			yield return expression;
		}
	}

	bool ISqlTableElement.IsConstraint => true;

	SqlConstraint ISqlTableElement.AsConstraint => this;

	bool ISqlTableElement.IsColumnDefinition => false;

	SqlColumnDefinition ISqlTableElement.AsColumnDefinition => null;

	SqlTemporalPeriodDefinition ISqlTableElement.AsTemporalPeriodDefinition => null;

	bool ISqlTableElement.IsTemporalPeriodDefinition => false;

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

	internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable)
	{
		Bind(bindingContext, bindingHelper, boundTable, null);
	}

	internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable, IMutableColumn boundColumn)
	{
		localBindingContext = bindingContext.CreateLocalBindingContext();
		localBindingContext.AddLocalTableExpression(boundTable);
		expression.Bind(localBindingContext);
		base.BindingState |= expression.BindingState;
		string text;
		bool isSystemNamed;
		if (SqlIdentifier.IsNullOrEmpty(base.Name))
		{
			string constraintText = ExpressionCanonicalizer.Instance.Canonicalize(expression.Sql, ExpressionCanonicalizer.ExpressionType.Check);
			text = BindUtils.Constraint.GenerateCheckName(boundTable, constraintText);
			isSystemNamed = true;
		}
		else
		{
			text = base.Name.Value;
			isSystemNamed = false;
		}
		IMutableCheckConstraint mutableCheckConstraint = bindingContext.MetadataFactory.Constraint.CreateCheckConstraint(boundTable, text);
		mutableCheckConstraint.IsSystemNamed = isSystemNamed;
		mutableCheckConstraint.IsChecked = true;
		mutableCheckConstraint.IsEnabled = true;
		mutableCheckConstraint.NotForReplication = notForReplication;
		mutableCheckConstraint.Text = expression.Sql;
		boundTable.Constraints.Add(mutableCheckConstraint);
	}

	internal override void Unbind()
	{
		base.Unbind();
		localBindingContext = null;
	}

	internal SqlCheckConstraint(bool notForReplication, SqlBooleanExpression expression)
		: base(SqlConstraintType.Check)
	{
		this.notForReplication = notForReplication;
		this.expression = expression;
	}
}
