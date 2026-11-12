using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInlineIndexConstraint : SqlConstraint, ISqlTableElement
{
	private readonly SqlIndexOptionCollection _options;

	internal override bool IsVisitable => true;

	public SqlIndexOptionCollection Options => _options;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Name;
			foreach (SqlCodeObject child in base.Children)
			{
				yield return child;
			}
			if (Options == null)
			{
				yield break;
			}
			foreach (SqlIndexOption option in Options)
			{
				yield return option;
			}
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
	}

	internal SqlInlineIndexConstraint(SqlIdentifier id, IEnumerable<SqlIndexOption> options = null)
		: base(SqlConstraintType.Default)
	{
		base.Name = id;
		if (options != null)
		{
			_options = new SqlIndexOptionCollection(options);
		}
	}
}
