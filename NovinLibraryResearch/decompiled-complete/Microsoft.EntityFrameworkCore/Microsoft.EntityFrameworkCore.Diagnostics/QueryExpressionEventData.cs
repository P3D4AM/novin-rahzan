using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class QueryExpressionEventData : EventData
{
	public virtual Expression Expression { get; }

	public virtual ExpressionPrinter ExpressionPrinter { get; }

	public QueryExpressionEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] Expression queryExpression, [JetBrains.Annotations.NotNull] ExpressionPrinter expressionPrinter)
		: base(eventDefinition, messageGenerator)
	{
		Expression = queryExpression;
		ExpressionPrinter = expressionPrinter;
	}
}
