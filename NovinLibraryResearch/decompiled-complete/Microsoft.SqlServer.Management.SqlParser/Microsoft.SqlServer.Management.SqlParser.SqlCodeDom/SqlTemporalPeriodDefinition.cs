using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTemporalPeriodDefinition : SqlCodeObject, ISqlTableElement
{
	private readonly string name;

	private readonly TemporalPeriodType type;

	private readonly SqlIdentifier rowStartColumnName;

	private readonly SqlIdentifier rowEndColumnName;

	internal override bool IsVisitable => true;

	public string Name => name;

	public TemporalPeriodType Type => type;

	public SqlIdentifier RowStartColumnName => rowStartColumnName;

	public SqlIdentifier RowEndColumnName => rowEndColumnName;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return rowStartColumnName;
			yield return rowEndColumnName;
		}
	}

	bool ISqlTableElement.IsColumnDefinition => false;

	SqlColumnDefinition ISqlTableElement.AsColumnDefinition => null;

	bool ISqlTableElement.IsConstraint => false;

	SqlConstraint ISqlTableElement.AsConstraint => null;

	SqlTemporalPeriodDefinition ISqlTableElement.AsTemporalPeriodDefinition => this;

	bool ISqlTableElement.IsTemporalPeriodDefinition => true;

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
	}

	internal void Bind(BatchBindingContext bindingContext, IMutableTable boundTable, IColumn colRowStart, IColumn colRowEnd)
	{
		if (!RowStartColumnName.Value.Equals(colRowStart.Name, StringComparison.OrdinalIgnoreCase))
		{
			ReportBindError(SR.GeneratedAlwaysAsRowStartColumnWrongName);
			base.BindingState |= BindingState.BindFailed;
		}
		if (!RowEndColumnName.Value.Equals(colRowEnd.Name, StringComparison.OrdinalIgnoreCase))
		{
			ReportBindError(SR.GeneratedAlwaysAsRowEndColumnWrongName);
			base.BindingState |= BindingState.BindFailed;
		}
		base.BindingState |= BindingState.BindSucceeded;
	}

	internal SqlTemporalPeriodDefinition(SqlIdentifier rowStartColumnName, SqlIdentifier rowEndColumnName)
	{
		name = "system_time";
		type = TemporalPeriodType.SystemTime;
		this.rowStartColumnName = rowStartColumnName;
		this.rowEndColumnName = rowEndColumnName;
	}

	public override string ToString()
	{
		return type.ToString().ToUpperInvariant();
	}
}
