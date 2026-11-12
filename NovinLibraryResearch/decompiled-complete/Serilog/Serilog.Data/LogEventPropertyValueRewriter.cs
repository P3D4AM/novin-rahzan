using System.Collections.Generic;
using Serilog.Events;

namespace Serilog.Data;

public abstract class LogEventPropertyValueRewriter<TState> : LogEventPropertyValueVisitor<TState, LogEventPropertyValue>
{
	protected override LogEventPropertyValue VisitScalarValue(TState state, ScalarValue scalar)
	{
		return Guard.AgainstNull(scalar, "scalar");
	}

	protected override LogEventPropertyValue VisitSequenceValue(TState state, SequenceValue sequence)
	{
		Guard.AgainstNull(sequence, "sequence");
		for (int i = 0; i < sequence.Elements.Count; i++)
		{
			LogEventPropertyValue logEventPropertyValue = sequence.Elements[i];
			if (logEventPropertyValue != Visit(state, logEventPropertyValue))
			{
				LogEventPropertyValue[] array = new LogEventPropertyValue[sequence.Elements.Count];
				for (int j = 0; j < i; j++)
				{
					array[j] = sequence.Elements[j];
				}
				for (int k = i; k < array.Length; k++)
				{
					array[k] = Visit(state, sequence.Elements[k]);
				}
				return new SequenceValue(array);
			}
		}
		return sequence;
	}

	protected override LogEventPropertyValue VisitStructureValue(TState state, StructureValue structure)
	{
		Guard.AgainstNull(structure, "structure");
		for (int i = 0; i < structure.Properties.Count; i++)
		{
			LogEventProperty logEventProperty = structure.Properties[i];
			if (logEventProperty.Value != Visit(state, logEventProperty.Value))
			{
				LogEventProperty[] array = new LogEventProperty[structure.Properties.Count];
				for (int j = 0; j < i; j++)
				{
					array[j] = structure.Properties[j];
				}
				for (int k = i; k < array.Length; k++)
				{
					LogEventProperty logEventProperty2 = structure.Properties[k];
					array[k] = new LogEventProperty(logEventProperty2.Name, Visit(state, logEventProperty2.Value));
				}
				return new StructureValue(array, structure.TypeTag);
			}
		}
		return structure;
	}

	protected override LogEventPropertyValue VisitDictionaryValue(TState state, DictionaryValue dictionary)
	{
		Guard.AgainstNull(dictionary, "dictionary");
		foreach (KeyValuePair<ScalarValue, LogEventPropertyValue> element in dictionary.Elements)
		{
			if (element.Value == Visit(state, element.Value))
			{
				continue;
			}
			Dictionary<ScalarValue, LogEventPropertyValue> dictionary2 = new Dictionary<ScalarValue, LogEventPropertyValue>(dictionary.Elements.Count);
			foreach (KeyValuePair<ScalarValue, LogEventPropertyValue> element2 in dictionary.Elements)
			{
				dictionary2[element2.Key] = Visit(state, element2.Value);
			}
			return new DictionaryValue(dictionary2);
		}
		return dictionary;
	}

	protected override LogEventPropertyValue VisitUnsupportedValue(TState state, LogEventPropertyValue value)
	{
		return value;
	}
}
