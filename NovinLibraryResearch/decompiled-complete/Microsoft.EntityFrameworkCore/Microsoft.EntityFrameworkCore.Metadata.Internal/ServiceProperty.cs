using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ServiceProperty : PropertyBase, IMutableServiceProperty, IServiceProperty, IPropertyBase, IAnnotatable, IMutablePropertyBase, IMutableAnnotatable, IConventionServiceProperty, IConventionPropertyBase, IConventionAnnotatable
{
	private ServiceParameterBinding _parameterBinding;

	private ConfigurationSource _configurationSource;

	private ConfigurationSource? _parameterBindingConfigurationSource;

	public virtual EntityType DeclaringEntityType { get; }

	public override TypeBase DeclaringType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	public override Type ClrType { get; }

	public virtual InternalServicePropertyBuilder Builder
	{
		[DebuggerStepThrough]
		get;
		[DebuggerStepThrough]
		[param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual ServiceParameterBinding ParameterBinding
	{
		get
		{
			return _parameterBinding;
		}
		set
		{
			SetParameterBinding(value, ConfigurationSource.Explicit);
		}
	}

	IEntityType IServiceProperty.DeclaringEntityType => DeclaringEntityType;

	IMutableEntityType IMutableServiceProperty.DeclaringEntityType => DeclaringEntityType;

	IConventionServicePropertyBuilder IConventionServiceProperty.Builder => Builder;

	IConventionEntityType IConventionServiceProperty.DeclaringEntityType => DeclaringEntityType;

	public virtual DebugView<ServiceProperty> DebugView => new DebugView<ServiceProperty>(this, (ServiceProperty m) => m.ToDebugString(singleLine: false));

	public ServiceProperty([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] PropertyInfo propertyInfo, [JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, [JetBrains.Annotations.NotNull] EntityType declaringEntityType, ConfigurationSource configurationSource)
		: base(name, propertyInfo, fieldInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(declaringEntityType, "declaringEntityType");
		DeclaringEntityType = declaringEntityType;
		ClrType = propertyInfo?.PropertyType ?? fieldInfo?.FieldType;
		_configurationSource = configurationSource;
		Builder = new InternalServicePropertyBuilder(this, declaringEntityType.Model.Builder);
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		_configurationSource = configurationSource.Max(_configurationSource);
	}

	public virtual void SetParameterBinding(ServiceParameterBinding parameterBinding, ConfigurationSource configurationSource)
	{
		_parameterBinding = parameterBinding;
		UpdateParameterBindingConfigurationSource(configurationSource);
	}

	void IConventionServiceProperty.SetParameterBinding(ServiceParameterBinding parameterBinding, bool fromDataAnnotation)
	{
		SetParameterBinding(parameterBinding, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	public virtual ConfigurationSource? GetParameterBindingConfigurationSource()
	{
		return _parameterBindingConfigurationSource;
	}

	private void UpdateParameterBindingConfigurationSource(ConfigurationSource configurationSource)
	{
		_parameterBindingConfigurationSource = configurationSource.Max(_parameterBindingConfigurationSource);
	}

	public override string ToString()
	{
		return this.ToDebugString();
	}
}
