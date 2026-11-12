using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionSequence : ISequence
{
	new IConventionModel Model { get; }

	IConventionSequenceBuilder Builder { get; }

	ConfigurationSource GetConfigurationSource();

	void SetStartValue(long? startValue, bool fromDataAnnotation = false);

	ConfigurationSource? GetStartValueConfigurationSource();

	void SetIncrementBy(int? incrementBy, bool fromDataAnnotation = false);

	ConfigurationSource? GetIncrementByConfigurationSource();

	void SetMinValue(long? minValue, bool fromDataAnnotation = false);

	ConfigurationSource? GetMinValueConfigurationSource();

	void SetMaxValue(long? maxValue, bool fromDataAnnotation = false);

	ConfigurationSource? GetMaxValueConfigurationSource();

	void SetClrType([JetBrains.Annotations.CanBeNull] Type clrType, bool fromDataAnnotation = false);

	ConfigurationSource? GetClrTypeConfigurationSource();

	void SetIsCyclic(bool? cyclic, bool fromDataAnnotation = false);

	ConfigurationSource? GetIsCyclicConfigurationSource();
}
