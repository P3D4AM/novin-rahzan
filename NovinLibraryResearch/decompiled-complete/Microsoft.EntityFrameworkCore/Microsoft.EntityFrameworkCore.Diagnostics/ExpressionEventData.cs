using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ExpressionEventData : EventData
{
	public virtual Expression Expression { get; }

	public ExpressionEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] Expression expression)
		: base(eventDefinition, messageGenerator)
	{
		Expression = expression;
	}
}
