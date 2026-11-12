using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionSequenceBuilder
{
	IConventionSequence Metadata { get; }

	IConventionSequenceBuilder HasType([JetBrains.Annotations.CanBeNull] Type type, bool fromDataAnnotation = false);

	bool CanSetType([JetBrains.Annotations.CanBeNull] Type type, bool fromDataAnnotation = false);

	IConventionSequenceBuilder IncrementsBy(int? increment, bool fromDataAnnotation = false);

	bool CanSetIncrementsBy(int? increment, bool fromDataAnnotation = false);

	IConventionSequenceBuilder StartsAt(long? startValue, bool fromDataAnnotation = false);

	bool CanSetStartsAt(long? startValue, bool fromDataAnnotation = false);

	IConventionSequenceBuilder HasMax(long? maximum, bool fromDataAnnotation = false);

	bool CanSetMax(long? maximum, bool fromDataAnnotation = false);

	IConventionSequenceBuilder HasMin(long? minimum, bool fromDataAnnotation = false);

	bool CanSetMin(long? minimum, bool fromDataAnnotation = false);

	IConventionSequenceBuilder IsCyclic(bool? cyclic, bool fromDataAnnotation = false);

	bool CanSetCyclic(bool? cyclic, bool fromDataAnnotation = false);
}
