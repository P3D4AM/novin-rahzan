using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata;

public interface IConventionPropertyBase : IPropertyBase, IAnnotatable, IConventionAnnotatable
{
	new IConventionTypeBase DeclaringType { get; }

	void SetField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, bool fromDataAnnotation = false);

	ConfigurationSource? GetFieldInfoConfigurationSource();
}
