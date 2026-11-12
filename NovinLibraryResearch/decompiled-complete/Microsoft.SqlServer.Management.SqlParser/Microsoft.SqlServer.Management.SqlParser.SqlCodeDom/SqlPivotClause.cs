using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlPivotClause : SqlCodeObject
{
	private readonly SqlObjectIdentifier aggregateFunctionName;

	private readonly SqlColumnRefExpressionCollection aggregatedColumns;

	private readonly SqlColumnRefExpression pivotedColumn;

	private readonly SqlColumnRefExpressionCollection columnList;

	internal override bool IsVisitable => true;

	public SqlObjectIdentifier AggregateFunctionName => aggregateFunctionName;

	public SqlColumnRefExpressionCollection AggregatedColumns => aggregatedColumns;

	public SqlColumnRefExpression PivotedColumn => pivotedColumn;

	public SqlColumnRefExpressionCollection ColumnList => columnList;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return aggregateFunctionName;
			foreach (SqlColumnRefExpression aggregatedColumn in aggregatedColumns)
			{
				yield return aggregatedColumn;
			}
			yield return pivotedColumn;
			foreach (SqlColumnRefExpression column in columnList)
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
		if (!SqlIdentifier.IsNullOrEmpty(aggregateFunctionName.ObjectName))
		{
			IScalarFunction scalarFunction = null;
			int num = 0;
			int num2 = 0;
			string value = aggregateFunctionName.DatabaseName.Value;
			string value2 = aggregateFunctionName.SchemaName.Value;
			string value3 = aggregateFunctionName.ObjectName.Value;
			if (aggregateFunctionName.IsMultiPartName)
			{
				scalarFunction = bindingContext.Batch.FindUserDefinedAggregate(value, value2, value3, GetLexicalScope());
				num = scalarFunction.Parameters.Count;
				num2 = num;
			}
			else
			{
				IBuiltInFunction builtInFunction = bindingContext.Batch.FindBuiltInFunction(value3);
				if (builtInFunction != null && builtInFunction.IsAggregateFunction)
				{
					scalarFunction = builtInFunction;
					num = builtInFunction.MinNumberOfArgs;
					num2 = builtInFunction.MaxNumberOfArgs;
				}
			}
			if (scalarFunction != null)
			{
				int count = aggregatedColumns.Count;
				if (count < num)
				{
					ReportBindError(SR.InsufficientArguments(value3));
				}
				else if (count > num2)
				{
					ReportBindError(SR.TooManyArguments(value3));
				}
			}
			else
			{
				aggregateFunctionName.ReportBindError(SR.InvalidAggregateFunction(value3), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
		}
		foreach (SqlColumnRefExpression aggregatedColumn in aggregatedColumns)
		{
			aggregatedColumn.Bind(bindingContext);
			base.BindingState |= aggregatedColumn.BindingState;
		}
		pivotedColumn.Bind(bindingContext);
		base.BindingState |= pivotedColumn.BindingState;
	}

	internal SqlPivotClause(SqlObjectIdentifier aggregationFunction, List<SqlColumnRefExpression> aggregatedColumnList, SqlColumnRefExpression pivotedColumn, List<SqlColumnRefExpression> columnList)
	{
		aggregateFunctionName = aggregationFunction;
		aggregatedColumns = new SqlColumnRefExpressionCollection(aggregatedColumnList);
		this.pivotedColumn = pivotedColumn;
		this.columnList = new SqlColumnRefExpressionCollection(columnList);
	}
}
