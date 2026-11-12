using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class ColumnExpression : Expression, IEquatable<ColumnExpression>
{
	public string Column { get; }

	public ColumnExpression(string column)
	{
		if (column == null)
		{
			throw new ArgumentNullException("column");
		}
		if (column.Length == 0)
		{
			throw new ArgumentException(Resources.ErrorEmtyString, "column");
		}
		Column = column;
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		if (!data.TryGetData(Column, out var value))
		{
			throw new SqlAssessmentMissingDataException(Column);
		}
		if (value is Expression expression)
		{
			return expression.Evaluate(data);
		}
		return value;
	}

	internal override IEnumerable<string> GetNames()
	{
		yield return Column;
	}

	public bool Equals(ColumnExpression other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return string.Equals(Column, other.Column, StringComparison.Ordinal);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((ColumnExpression)obj);
	}

	public override int GetHashCode()
	{
		return Column.GetHashCode();
	}
}
