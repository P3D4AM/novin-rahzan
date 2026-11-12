using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

[DebuggerDisplay("{Metadata,nq}")]
public abstract class InternalAnnotatableBuilder : IConventionAnnotatableBuilder
{
	public virtual ConventionAnnotatable Metadata
	{
		[DebuggerStepThrough]
		get;
	}

	public abstract InternalModelBuilder ModelBuilder
	{
		[DebuggerStepThrough]
		get;
	}

	IConventionModelBuilder IConventionAnnotatableBuilder.ModelBuilder => ModelBuilder;

	IConventionAnnotatable IConventionAnnotatableBuilder.Metadata => Metadata;

	protected InternalAnnotatableBuilder([JetBrains.Annotations.NotNull] ConventionAnnotatable metadata)
	{
		Metadata = metadata;
	}

	public virtual InternalAnnotatableBuilder HasAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, ConfigurationSource configurationSource)
	{
		return HasAnnotation(name, value, configurationSource, canOverrideSameSource: true);
	}

	private InternalAnnotatableBuilder HasAnnotation(string name, object value, ConfigurationSource configurationSource, bool canOverrideSameSource)
	{
		ConventionAnnotation conventionAnnotation = Metadata.FindAnnotation(name);
		if (conventionAnnotation != null)
		{
			if (object.Equals(conventionAnnotation.Value, value))
			{
				conventionAnnotation.UpdateConfigurationSource(configurationSource);
				return this;
			}
			if (!CanSetAnnotationValue(conventionAnnotation, value, configurationSource, canOverrideSameSource))
			{
				return null;
			}
			Metadata.SetAnnotation(name, value, configurationSource);
			return this;
		}
		Metadata.AddAnnotation(name, value, configurationSource);
		return this;
	}

	public virtual InternalAnnotatableBuilder SetOrRemoveAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, ConfigurationSource configurationSource)
	{
		if (value != null)
		{
			return HasAnnotation(name, value, configurationSource, canOverrideSameSource: true);
		}
		return RemoveAnnotation(name, configurationSource);
	}

	public virtual bool CanSetAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, ConfigurationSource configurationSource)
	{
		ConventionAnnotation conventionAnnotation = Metadata.FindAnnotation(name);
		if (conventionAnnotation != null)
		{
			return CanSetAnnotationValue(conventionAnnotation, value, configurationSource, canOverrideSameSource: true);
		}
		return true;
	}

	private static bool CanSetAnnotationValue(ConventionAnnotation annotation, object value, ConfigurationSource configurationSource, bool canOverrideSameSource)
	{
		if (object.Equals(annotation.Value, value))
		{
			return true;
		}
		ConfigurationSource configurationSource2 = annotation.GetConfigurationSource();
		if (configurationSource.Overrides(configurationSource2))
		{
			return configurationSource != configurationSource2 || canOverrideSameSource;
		}
		return false;
	}

	public virtual InternalAnnotatableBuilder RemoveAnnotation([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		if (!CanRemoveAnnotation(name, configurationSource))
		{
			return null;
		}
		Metadata.RemoveAnnotation(name);
		return this;
	}

	public virtual bool CanRemoveAnnotation(string name, ConfigurationSource configurationSource)
	{
		ConventionAnnotation conventionAnnotation = Metadata.FindAnnotation(name);
		if (conventionAnnotation != null)
		{
			return configurationSource.Overrides(conventionAnnotation.GetConfigurationSource());
		}
		return true;
	}

	public virtual void MergeAnnotationsFrom([JetBrains.Annotations.NotNull] ConventionAnnotatable annotatable)
	{
		MergeAnnotationsFrom(annotatable, ConfigurationSource.Explicit);
	}

	public virtual void MergeAnnotationsFrom([JetBrains.Annotations.NotNull] ConventionAnnotatable annotatable, ConfigurationSource minimalConfigurationSource)
	{
		foreach (ConventionAnnotation annotation in annotatable.GetAnnotations())
		{
			ConfigurationSource configurationSource = annotation.GetConfigurationSource();
			if (configurationSource.Overrides(minimalConfigurationSource))
			{
				HasAnnotation(annotation.Name, annotation.Value, configurationSource, canOverrideSameSource: false);
			}
		}
	}

	IConventionAnnotatableBuilder IConventionAnnotatableBuilder.HasAnnotation(string name, object value, bool fromDataAnnotation)
	{
		return HasAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionAnnotatableBuilder IConventionAnnotatableBuilder.SetOrRemoveAnnotation(string name, object value, bool fromDataAnnotation)
	{
		return SetOrRemoveAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionAnnotatableBuilder.CanSetAnnotation(string name, object value, bool fromDataAnnotation)
	{
		return CanSetAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionAnnotatableBuilder IConventionAnnotatableBuilder.RemoveAnnotation(string name, bool fromDataAnnotation)
	{
		return RemoveAnnotation(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionAnnotatableBuilder.CanRemoveAnnotation(string name, bool fromDataAnnotation)
	{
		return CanRemoveAnnotation(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
public abstract class InternalAnnotatableBuilder<TMetadata> : InternalAnnotatableBuilder where TMetadata : ConventionAnnotatable
{
	public new virtual TMetadata Metadata
	{
		[DebuggerStepThrough]
		get
		{
			return (TMetadata)base.Metadata;
		}
	}

	protected InternalAnnotatableBuilder([JetBrains.Annotations.NotNull] TMetadata metadata)
		: base(metadata)
	{
	}
}
