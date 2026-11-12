using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class TableExpression : TableExpressionBase
{
	public string Name { get; }

	public string Schema { get; }

	internal TableExpression(string name, string schema, [JetBrains.Annotations.NotNull] string alias)
		: base(alias)
	{
		Name = name;
		Schema = schema;
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		if (!string.IsNullOrEmpty(Schema))
		{
			expressionPrinter.Append(Schema).Append(".");
		}
		expressionPrinter.Append(Name).Append(" AS ").Append(Alias);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			return this == obj;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Name, Schema);
	}
}
