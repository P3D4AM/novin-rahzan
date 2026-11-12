using System;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SequenceBuilder : IConventionSequenceBuilder
{
	private readonly Sequence _sequence;

	public virtual IMutableSequence Metadata => _sequence;

	IConventionSequence IConventionSequenceBuilder.Metadata => (IConventionSequence)Metadata;

	public SequenceBuilder([JetBrains.Annotations.NotNull] IMutableSequence sequence)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sequence, "sequence");
		_sequence = (Sequence)sequence;
	}

	IConventionSequenceBuilder IConventionSequenceBuilder.HasType(Type type, bool fromDataAnnotation)
	{
		if (Overrides(fromDataAnnotation, _sequence.GetClrTypeConfigurationSource()) || _sequence.ClrType == type)
		{
			((IConventionSequence)_sequence).SetClrType(type, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionSequenceBuilder.CanSetType(Type type, bool fromDataAnnotation)
	{
		if (type == null || Sequence.SupportedTypes.Contains(type))
		{
			if (!Overrides(fromDataAnnotation, _sequence.GetClrTypeConfigurationSource()))
			{
				return _sequence.ClrType == type;
			}
			return true;
		}
		return false;
	}

	public virtual SequenceBuilder IncrementsBy(int increment)
	{
		_sequence.IncrementBy = increment;
		return this;
	}

	IConventionSequenceBuilder IConventionSequenceBuilder.IncrementsBy(int? increment, bool fromDataAnnotation)
	{
		if (((IConventionSequenceBuilder)this).CanSetIncrementsBy(increment, fromDataAnnotation))
		{
			((IConventionSequence)_sequence).SetIncrementBy(increment, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionSequenceBuilder.CanSetIncrementsBy(int? increment, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _sequence.GetIncrementByConfigurationSource()))
		{
			return _sequence.IncrementBy == increment;
		}
		return true;
	}

	public virtual SequenceBuilder StartsAt(long startValue)
	{
		_sequence.StartValue = startValue;
		return this;
	}

	IConventionSequenceBuilder IConventionSequenceBuilder.StartsAt(long? startValue, bool fromDataAnnotation)
	{
		if (((IConventionSequenceBuilder)this).CanSetStartsAt(startValue, fromDataAnnotation))
		{
			((IConventionSequence)_sequence).SetStartValue(startValue, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionSequenceBuilder.CanSetStartsAt(long? startValue, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _sequence.GetStartValueConfigurationSource()))
		{
			return _sequence.StartValue == startValue;
		}
		return true;
	}

	public virtual SequenceBuilder HasMax(long maximum)
	{
		_sequence.MaxValue = maximum;
		return this;
	}

	IConventionSequenceBuilder IConventionSequenceBuilder.HasMax(long? maximum, bool fromDataAnnotation)
	{
		if (((IConventionSequenceBuilder)this).CanSetMax(maximum, fromDataAnnotation))
		{
			((IConventionSequence)_sequence).SetMaxValue(maximum, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionSequenceBuilder.CanSetMax(long? maximum, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _sequence.GetMaxValueConfigurationSource()))
		{
			return _sequence.MaxValue == maximum;
		}
		return true;
	}

	public virtual SequenceBuilder HasMin(long minimum)
	{
		_sequence.MinValue = minimum;
		return this;
	}

	IConventionSequenceBuilder IConventionSequenceBuilder.HasMin(long? minimum, bool fromDataAnnotation)
	{
		if (((IConventionSequenceBuilder)this).CanSetMin(minimum, fromDataAnnotation))
		{
			((IConventionSequence)_sequence).SetMinValue(minimum, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionSequenceBuilder.CanSetMin(long? minimum, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _sequence.GetMinValueConfigurationSource()))
		{
			return _sequence.MinValue == minimum;
		}
		return true;
	}

	public virtual SequenceBuilder IsCyclic(bool cyclic = true)
	{
		_sequence.IsCyclic = cyclic;
		return this;
	}

	IConventionSequenceBuilder IConventionSequenceBuilder.IsCyclic(bool? cyclic, bool fromDataAnnotation)
	{
		if (((IConventionSequenceBuilder)this).CanSetCyclic(cyclic, fromDataAnnotation))
		{
			((IConventionSequence)_sequence).SetIsCyclic(cyclic, fromDataAnnotation);
			return this;
		}
		return null;
	}

	bool IConventionSequenceBuilder.CanSetCyclic(bool? cyclic, bool fromDataAnnotation)
	{
		if (!Overrides(fromDataAnnotation, _sequence.GetIsCyclicConfigurationSource()))
		{
			return _sequence.IsCyclic == cyclic;
		}
		return true;
	}

	private bool Overrides(bool fromDataAnnotation, ConfigurationSource? configurationSource)
	{
		return (fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention).Overrides(configurationSource);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
