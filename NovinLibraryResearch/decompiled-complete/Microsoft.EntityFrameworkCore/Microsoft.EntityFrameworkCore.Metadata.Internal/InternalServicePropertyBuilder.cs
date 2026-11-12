using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalServicePropertyBuilder : InternalModelItemBuilder<ServiceProperty>, IConventionServicePropertyBuilder, IConventionAnnotatableBuilder
{
	IConventionServiceProperty IConventionServicePropertyBuilder.Metadata => Metadata;

	public InternalServicePropertyBuilder([JetBrains.Annotations.NotNull] ServiceProperty property, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(property, modelBuilder)
	{
	}

	public virtual InternalServicePropertyBuilder HasField([JetBrains.Annotations.CanBeNull] string fieldName, ConfigurationSource configurationSource)
	{
		if (Metadata.FieldInfo?.GetSimpleMemberName() == fieldName || configurationSource.Overrides(Metadata.GetFieldInfoConfigurationSource()))
		{
			Metadata.SetField(fieldName, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetField([JetBrains.Annotations.CanBeNull] string fieldName, ConfigurationSource? configurationSource)
	{
		if (fieldName != null && configurationSource.Overrides(Metadata.GetFieldInfoConfigurationSource()))
		{
			FieldInfo fieldInfo = PropertyBase.GetFieldInfo(fieldName, Metadata.DeclaringType, Metadata.Name, shouldThrow: false);
			if (fieldInfo != null)
			{
				return PropertyBase.IsCompatible(fieldInfo, Metadata.ClrType, Metadata.DeclaringType.ClrType, Metadata.Name, shouldThrow: false);
			}
			return false;
		}
		return Metadata.FieldInfo?.GetSimpleMemberName() == fieldName;
	}

	public virtual InternalServicePropertyBuilder HasField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, ConfigurationSource configurationSource)
	{
		if (configurationSource.Overrides(Metadata.GetFieldInfoConfigurationSource()) || object.Equals(Metadata.FieldInfo, fieldInfo))
		{
			Metadata.SetField(fieldInfo, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetFieldInfoConfigurationSource()) || (!(fieldInfo == null) && !PropertyBase.IsCompatible(fieldInfo, Metadata.ClrType, Metadata.DeclaringType.ClrType, Metadata.Name, shouldThrow: false)))
		{
			return object.Equals(Metadata.FieldInfo, fieldInfo);
		}
		return true;
	}

	public virtual InternalServicePropertyBuilder UsePropertyAccessMode(PropertyAccessMode propertyAccessMode, ConfigurationSource configurationSource)
	{
		return (InternalServicePropertyBuilder)HasAnnotation("PropertyAccessMode", propertyAccessMode, configurationSource);
	}

	public virtual InternalServicePropertyBuilder HasParameterBinding(ServiceParameterBinding parameterBinding, ConfigurationSource configurationSource)
	{
		if (CanSetParameterBinding(parameterBinding, configurationSource))
		{
			Metadata.SetParameterBinding(parameterBinding, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetParameterBinding(ServiceParameterBinding parameterBinding, ConfigurationSource configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetParameterBindingConfigurationSource()))
		{
			return Metadata.ParameterBinding == parameterBinding;
		}
		return true;
	}

	IConventionServicePropertyBuilder IConventionServicePropertyBuilder.HasField(string fieldName, bool fromDataAnnotation)
	{
		return HasField(fieldName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionServicePropertyBuilder IConventionServicePropertyBuilder.HasField(FieldInfo fieldInfo, bool fromDataAnnotation)
	{
		return HasField(fieldInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionServicePropertyBuilder.CanSetField(string fieldName, bool fromDataAnnotation)
	{
		return CanSetField(fieldName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionServicePropertyBuilder.CanSetField(FieldInfo fieldInfo, bool fromDataAnnotation)
	{
		return CanSetField(fieldInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionServicePropertyBuilder IConventionServicePropertyBuilder.HasParameterBinding(ServiceParameterBinding parameterBinding, bool fromDataAnnotation)
	{
		return HasParameterBinding(parameterBinding, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionServicePropertyBuilder.CanSetParameterBinding(ServiceParameterBinding parameterBinding, bool fromDataAnnotation)
	{
		return CanSetParameterBinding(parameterBinding, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
