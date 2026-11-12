using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlDefaultConstraint : SqlConstraint, ISqlTableElement
{
	private readonly SqlScalarExpression expression;

	internal override bool IsVisitable => true;

	public SqlScalarExpression Expression => expression;

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
		base.BindingState |= BindingState.BindFailed;
	}

	internal override void Bind(BatchBindingContext bindingContext, ConstraintBindingHelper bindingHelper, IMutableDatabaseTable boundTable, IMutableColumn boundColumn)
	{
		expression.Bind(bindingContext);
		base.BindingState |= base.BindingState;
		string text;
		bool isSystemNamed;
		if (SqlIdentifier.IsNullOrEmpty(base.Name))
		{
			text = BindUtils.Constraint.GenerateDefaultName(boundTable, boundColumn.Name);
			isSystemNamed = true;
		}
		else
		{
			text = base.Name.Value;
			isSystemNamed = false;
		}
		IMutableDefaultConstraint mutableDefaultConstraint = bindingContext.MetadataFactory.Constraint.CreateDefaultConstraint(boundColumn, text);
		mutableDefaultConstraint.IsSystemNamed = isSystemNamed;
		mutableDefaultConstraint.Text = expression.Sql;
		boundColumn.DefaultValue = mutableDefaultConstraint;
	}

	internal SqlDefaultConstraint(SqlScalarExpression expression)
		: base(SqlConstraintType.Default)
	{
		this.expression = expression;
	}
}
