using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public abstract class ConventionAnnotatable : Annotatable, IConventionAnnotatable, IAnnotatable
{
	public new virtual IEnumerable<ConventionAnnotation> GetAnnotations()
	{
		return base.GetAnnotations().Cast<ConventionAnnotation>();
	}

	public virtual ConventionAnnotation AddAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, ConfigurationSource configurationSource)
	{
		return (ConventionAnnotation)base.AddAnnotation(name, CreateAnnotation(name, value, configurationSource));
	}

	public virtual ConventionAnnotation SetAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, ConfigurationSource configurationSource)
	{
		ConventionAnnotation conventionAnnotation = FindAnnotation(name);
		if (conventionAnnotation != null)
		{
			if (object.Equals(conventionAnnotation.Value, value))
			{
				conventionAnnotation.UpdateConfigurationSource(configurationSource);
				return conventionAnnotation;
			}
			configurationSource = configurationSource.Max(conventionAnnotation.GetConfigurationSource());
		}
		return (ConventionAnnotation)base.SetAnnotation(name, CreateAnnotation(name, value, configurationSource), conventionAnnotation);
	}

	protected override Annotation OnAnnotationSet(string name, Annotation annotation, Annotation oldAnnotation)
	{
		return (Annotation)OnAnnotationSet(name, (IConventionAnnotation)annotation, (IConventionAnnotation)oldAnnotation);
	}

	protected virtual IConventionAnnotation OnAnnotationSet([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation)
	{
		return annotation;
	}

	public new virtual ConventionAnnotation FindAnnotation(string name)
	{
		return (ConventionAnnotation)base.FindAnnotation(name);
	}

	public new virtual ConventionAnnotation RemoveAnnotation(string name)
	{
		return (ConventionAnnotation)base.RemoveAnnotation(name);
	}

	protected override Annotation CreateAnnotation(string name, object value)
	{
		return CreateAnnotation(name, value, ConfigurationSource.Explicit);
	}

	private static ConventionAnnotation CreateAnnotation(string name, object value, ConfigurationSource configurationSource)
	{
		return new ConventionAnnotation(name, value, configurationSource);
	}

	IEnumerable<IConventionAnnotation> IConventionAnnotatable.GetAnnotations()
	{
		return GetAnnotations();
	}

	void IConventionAnnotatable.SetAnnotation(string name, object value, bool fromDataAnnotation)
	{
		SetAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionAnnotation IConventionAnnotatable.AddAnnotation(string name, object value, bool fromDataAnnotation)
	{
		return AddAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionAnnotation IConventionAnnotatable.FindAnnotation(string name)
	{
		return FindAnnotation(name);
	}

	IConventionAnnotation IConventionAnnotatable.RemoveAnnotation(string name)
	{
		return RemoveAnnotation(name);
	}
}
