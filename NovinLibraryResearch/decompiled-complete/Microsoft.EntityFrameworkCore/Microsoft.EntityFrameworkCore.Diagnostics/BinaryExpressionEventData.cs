using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class BinaryExpressionEventData : EventData
{
	public virtual Expression Left { get; }

	public virtual Expression Right { get; }

	public BinaryExpressionEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] Expression left, [JetBrains.Annotations.NotNull] Expression right)
		: base(eventDefinition, messageGenerator)
	{
		Left = left;
		Right = right;
	}
}
