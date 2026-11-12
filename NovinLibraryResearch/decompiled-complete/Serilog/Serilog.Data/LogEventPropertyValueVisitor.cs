using System;
using Serilog.Events;

namespace Serilog.Data;

public abstract class LogEventPropertyValueVisitor<TState, TResult>
{
	protected virtual TResult Visit(TState state, LogEventPropertyValue value)
	{
		Guard.AgainstNull(value, "value");
		if (value is ScalarValue scalar)
		{
			return VisitScalarValue(state, scalar);
		}
		if (value is SequenceValue sequence)
		{
			return VisitSequenceValue(state, sequence);
		}
		if (value is StructureValue structure)
		{
			return VisitStructureValue(state, structure);
		}
		if (value is DictionaryValue dictionary)
		{
			return VisitDictionaryValue(state, dictionary);
		}
		return VisitUnsupportedValue(state, value);
	}

	protected abstract TResult VisitScalarValue(TState state, ScalarValue scalar);

	protected abstract TResult VisitSequenceValue(TState state, SequenceValue sequence);

	protected abstract TResult VisitStructureValue(TState state, StructureValue structure);

	protected abstract TResult VisitDictionaryValue(TState state, DictionaryValue dictionary);

	protected virtual TResult VisitUnsupportedValue(TState state, LogEventPropertyValue value)
	{
		Guard.AgainstNull(value, "value");
		throw new NotSupportedException($"The value {value} is not of a type supported by this visitor.");
	}
}
