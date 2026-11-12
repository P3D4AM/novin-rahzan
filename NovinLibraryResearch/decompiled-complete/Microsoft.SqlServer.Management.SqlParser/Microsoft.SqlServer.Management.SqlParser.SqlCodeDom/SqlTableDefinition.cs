using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableDefinition : SqlCodeObject
{
	private readonly List<ISqlTableElement> elements;

	internal override bool IsVisitable => true;

	public IEnumerable<SqlColumnDefinition> ColumnDefinitions
	{
		get
		{
			foreach (ISqlTableElement element in elements)
			{
				SqlColumnDefinition asColumnDefinition = element.AsColumnDefinition;
				if (asColumnDefinition != null)
				{
					yield return asColumnDefinition;
				}
			}
		}
	}

	public IEnumerable<SqlConstraint> Constraints
	{
		get
		{
			foreach (ISqlTableElement element in elements)
			{
				SqlConstraint asConstraint = element.AsConstraint;
				if (asConstraint != null)
				{
					yield return asConstraint;
				}
			}
		}
	}

	public IEnumerable<SqlTemporalPeriodDefinition> TemporalPeriodDefinitions
	{
		get
		{
			foreach (ISqlTableElement element in elements)
			{
				SqlTemporalPeriodDefinition asTemporalPeriodDefinition = element.AsTemporalPeriodDefinition;
				if (asTemporalPeriodDefinition != null)
				{
					yield return asTemporalPeriodDefinition;
				}
			}
		}
	}

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			foreach (SqlCodeObject element in elements)
			{
				yield return element;
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
	}

	internal void Bind(BatchBindingContext bindingContext, IMutableDatabaseTable boundTable)
	{
		_ = boundTable.Name;
		SqlConstraint.ConstraintBindingHelper bindingHelper = new SqlConstraint.ConstraintBindingHelper(new Dictionary<IColumn, bool>());
		foreach (SqlColumnDefinition columnDefinition in ColumnDefinitions)
		{
			columnDefinition.BindPhase1(bindingContext, boundTable);
			base.BindingState |= columnDefinition.BindingState;
		}
		foreach (ISqlTableElement element in elements)
		{
			if (element.IsColumnDefinition)
			{
				SqlColumnDefinition asColumnDefinition = element.AsColumnDefinition;
				asColumnDefinition.BindPhase2(bindingContext, bindingHelper, boundTable);
				base.BindingState |= asColumnDefinition.BindingState;
			}
			else if (!element.IsTemporalPeriodDefinition)
			{
				SqlConstraint asConstraint = element.AsConstraint;
				if (asConstraint.Type != SqlConstraintType.ForeignKey)
				{
					asConstraint.Bind(bindingContext, bindingHelper, boundTable);
					base.BindingState |= asConstraint.BindingState;
				}
			}
		}
		foreach (ISqlTableElement element2 in elements)
		{
			if (element2.IsColumnDefinition)
			{
				SqlColumnDefinition asColumnDefinition2 = element2.AsColumnDefinition;
				asColumnDefinition2.BindPhase3(bindingContext, bindingHelper, boundTable);
				base.BindingState |= asColumnDefinition2.BindingState;
			}
			else if (!element2.IsTemporalPeriodDefinition)
			{
				SqlConstraint asConstraint2 = element2.AsConstraint;
				if (asConstraint2.Type == SqlConstraintType.ForeignKey)
				{
					asConstraint2.Bind(bindingContext, bindingHelper, boundTable);
					base.BindingState |= asConstraint2.BindingState;
				}
			}
		}
		bool flag = false;
		SqlColumnDefinition sqlColumnDefinition = null;
		SqlColumnDefinition sqlColumnDefinition2 = null;
		foreach (SqlColumnDefinition columnDefinition2 in ColumnDefinitions)
		{
			columnDefinition2.BindPhase4(bindingContext, bindingHelper, boundTable);
			if (columnDefinition2.SparseOption == SqlSparseOption.ColumnSet)
			{
				if (flag)
				{
					ReportBindError(SR.CannotCreateMoreThanOneColumnSetOnTable(columnDefinition2.Name.Value, boundTable.Name));
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					flag = true;
				}
			}
			if (columnDefinition2.GeneratedAlwaysType == SqlGeneratedAlwaysType.AsRowStart)
			{
				if (sqlColumnDefinition != null)
				{
					ReportBindError(SR.CannotCreateMoreThanOneGeneratedAlwaysAsRowStartColumnOnTable);
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					sqlColumnDefinition = columnDefinition2;
				}
			}
			if (columnDefinition2.GeneratedAlwaysType == SqlGeneratedAlwaysType.AsRowEnd)
			{
				if (sqlColumnDefinition2 != null)
				{
					ReportBindError(SR.CannotCreateMoreThanOneGeneratedAlwaysAsRowEndColumnOnTable);
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					sqlColumnDefinition2 = columnDefinition2;
				}
			}
			base.BindingState |= columnDefinition2.BindingState;
		}
		bool flag2 = false;
		foreach (SqlTemporalPeriodDefinition temporalPeriodDefinition in TemporalPeriodDefinitions)
		{
			if (temporalPeriodDefinition.Type == TemporalPeriodType.SystemTime)
			{
				if (flag2)
				{
					ReportBindError(SR.CannotCreateMoreThanOneTemporalSystemTimePeriodOnTable);
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					if (sqlColumnDefinition == null)
					{
						ReportBindError(SR.GeneratedAlwaysAsRowStartColumnDefinitionMissing);
						base.BindingState |= BindingState.BindFailed;
					}
					else if (sqlColumnDefinition2 == null)
					{
						ReportBindError(SR.GeneratedAlwaysAsRowEndColumnDefinitionMissing);
						base.BindingState |= BindingState.BindFailed;
					}
					else
					{
						temporalPeriodDefinition.Bind(bindingContext, (IMutableTable)boundTable, sqlColumnDefinition.BoundColumn, sqlColumnDefinition2.BoundColumn);
					}
					flag2 = true;
				}
			}
			base.BindingState |= temporalPeriodDefinition.BindingState;
		}
		if (!flag2 && (sqlColumnDefinition != null || sqlColumnDefinition2 != null))
		{
			ReportBindError(SR.TemporalSystemTimePeriodDefinitionMissing);
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return null;
	}

	internal SqlTableDefinition(List<ISqlTableElement> elements)
	{
		this.elements = new List<ISqlTableElement>(elements.Count);
		this.elements.AddRange(elements);
	}
}
