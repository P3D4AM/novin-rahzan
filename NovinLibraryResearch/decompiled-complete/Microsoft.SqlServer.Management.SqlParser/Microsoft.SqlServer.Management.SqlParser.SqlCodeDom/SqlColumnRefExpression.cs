using System;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlColumnRefExpression : SqlScalarRefExpression
{
	private readonly bool discardColumnPrefix;

	internal override bool IsVisitable => true;

	public IColumn BoundColumn
	{
		get
		{
			int index = base.MultipartIdentifier.Count - 1;
			return GetBoundObjectForIdentifier(index) as IColumn;
		}
	}

	internal ITabular BoundTable
	{
		get
		{
			ITabular result = null;
			if (!discardColumnPrefix)
			{
				int num = base.MultipartIdentifier.Count - 2;
				if (num >= 0)
				{
					result = GetBoundObjectForIdentifier(num) as ITabular;
				}
			}
			return result;
		}
	}

	public SqlIdentifier ColumnName => base.MultipartIdentifier[base.MultipartIdentifier.Count - 1];

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
		IColumn boundColumn = null;
		ITabular boundTable = null;
		BindUtils.Column.LookupResult lookupResult = BindUtils.Column.LookupResult.None;
		int num = base.MultipartIdentifier.Count - 1;
		if (discardColumnPrefix || num == 0)
		{
			lookupResult = BindUtils.Column.FindColumn(bindingContext, ColumnName, out boundColumn);
			CheckAndReportErrors(lookupResult, num, isPrefixed: false, boundColumn, null);
		}
		else
		{
			lookupResult = BindUtils.Column.FindColumn(bindingContext, base.MultipartIdentifier, num, out boundTable, out boundColumn);
			CheckAndReportErrors(lookupResult, num, isPrefixed: true, boundColumn, boundTable);
		}
		if (boundColumn != null)
		{
			bindingContext.Batch.SetBoundObject(base.MultipartIdentifier[num], boundColumn);
			base.BindingState |= BindingState.BindSucceeded;
		}
		if (boundTable != null)
		{
			int index = num - 1;
			bindingContext.Batch.SetBoundObject(base.MultipartIdentifier[index], boundTable);
			base.BindingState |= BindingState.BindSucceeded;
		}
	}

	internal SqlColumnRefExpression(SqlObjectIdentifier objectIdentifier)
		: this(objectIdentifier, discardColumnPrefix: false)
	{
	}

	internal SqlColumnRefExpression(SqlObjectIdentifier objectIdentifier, bool discardColumnPrefix)
		: base(objectIdentifier)
	{
		this.discardColumnPrefix = discardColumnPrefix;
	}
}
