using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalPropertyBuilder : InternalModelItemBuilder<Property>, IConventionPropertyBuilder, IConventionAnnotatableBuilder
{
	IConventionProperty IConventionPropertyBuilder.Metadata
	{
		[DebuggerStepThrough]
		get
		{
			return Metadata;
		}
	}

	public InternalPropertyBuilder([JetBrains.Annotations.NotNull] Property property, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(property, modelBuilder)
	{
	}

	public virtual InternalPropertyBuilder IsRequired(bool? required, ConfigurationSource configurationSource)
	{
		if (configurationSource != 0 && !CanSetIsRequired(required, configurationSource))
		{
			return null;
		}
		if (required == false)
		{
			using (Metadata.DeclaringEntityType.Model.ConventionDispatcher.DelayConventions())
			{
				foreach (Key item in Metadata.GetContainingKeys().ToList())
				{
					if (configurationSource == ConfigurationSource.Explicit && item.GetConfigurationSource() == ConfigurationSource.Explicit)
					{
						throw new InvalidOperationException(CoreStrings.KeyPropertyCannotBeNullable(Metadata.Name, Metadata.DeclaringEntityType.DisplayName(), item.Properties.Format()));
					}
					item.DeclaringEntityType.Builder.HasNoKey(item, configurationSource);
				}
				Metadata.SetIsNullable(true, configurationSource);
			}
		}
		else
		{
			Metadata.SetIsNullable(!required, configurationSource);
		}
		return this;
	}

	public virtual bool CanSetIsRequired(bool? required, ConfigurationSource? configurationSource)
	{
		if ((configurationSource.HasValue && configurationSource.Value.Overrides(Metadata.GetIsNullableConfigurationSource())) || Metadata.IsNullable == !required)
		{
			if (required == false)
			{
				return Metadata.ClrType.IsNullableType();
			}
			return true;
		}
		return false;
	}

	public virtual InternalPropertyBuilder ValueGenerated(ValueGenerated? valueGenerated, ConfigurationSource configurationSource)
	{
		if (CanSetValueGenerated(valueGenerated, configurationSource))
		{
			Metadata.SetValueGenerated(valueGenerated, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetValueGenerated(ValueGenerated? valueGenerated, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetValueGeneratedConfigurationSource()))
		{
			return Metadata.ValueGenerated == valueGenerated;
		}
		return true;
	}

	public virtual InternalPropertyBuilder IsConcurrencyToken(bool? concurrencyToken, ConfigurationSource configurationSource)
	{
		if (CanSetIsConcurrencyToken(concurrencyToken, configurationSource))
		{
			Metadata.SetIsConcurrencyToken(concurrencyToken, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetIsConcurrencyToken(bool? concurrencyToken, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetIsConcurrencyTokenConfigurationSource()))
		{
			return Metadata.IsConcurrencyToken == concurrencyToken;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasField([JetBrains.Annotations.CanBeNull] string fieldName, ConfigurationSource configurationSource)
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

	public virtual InternalPropertyBuilder HasField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, ConfigurationSource configurationSource)
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

	public virtual InternalPropertyBuilder UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		if (CanSetPropertyAccessMode(propertyAccessMode, configurationSource))
		{
			Metadata.SetPropertyAccessMode(propertyAccessMode, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetPropertyAccessModeConfigurationSource()))
		{
			return Metadata.GetPropertyAccessMode() == propertyAccessMode;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasMaxLength(int? maxLength, ConfigurationSource configurationSource)
	{
		if (CanSetMaxLength(maxLength, configurationSource))
		{
			Metadata.SetMaxLength(maxLength, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetMaxLength(int? maxLength, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetMaxLengthConfigurationSource()))
		{
			return Metadata.GetMaxLength() == maxLength;
		}
		return true;
	}

	public virtual InternalPropertyBuilder IsUnicode(bool? unicode, ConfigurationSource configurationSource)
	{
		if (CanSetIsUnicode(unicode, configurationSource))
		{
			Metadata.SetIsUnicode(unicode, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetIsUnicode(bool? unicode, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetIsUnicodeConfigurationSource()))
		{
			return Metadata.IsUnicode() == unicode;
		}
		return true;
	}

	public virtual InternalPropertyBuilder BeforeSave(PropertySaveBehavior? behavior, ConfigurationSource configurationSource)
	{
		if (CanSetBeforeSave(behavior, configurationSource))
		{
			Metadata.SetBeforeSaveBehavior(behavior, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetBeforeSave(PropertySaveBehavior? behavior, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetBeforeSaveBehaviorConfigurationSource()))
		{
			return Metadata.GetBeforeSaveBehavior() == behavior;
		}
		return true;
	}

	public virtual InternalPropertyBuilder AfterSave(PropertySaveBehavior? behavior, ConfigurationSource configurationSource)
	{
		if (configurationSource.Overrides(Metadata.GetAfterSaveBehaviorConfigurationSource()) || Metadata.GetAfterSaveBehavior() == behavior)
		{
			Metadata.SetAfterSaveBehavior(behavior, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetAfterSave(PropertySaveBehavior? behavior, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetAfterSaveBehaviorConfigurationSource()) || (behavior.HasValue && Metadata.CheckAfterSaveBehavior(behavior.Value) != null))
		{
			return Metadata.GetAfterSaveBehavior() == behavior;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasValueGenerator([JetBrains.Annotations.CanBeNull] Type valueGeneratorType, ConfigurationSource configurationSource)
	{
		if (valueGeneratorType == null)
		{
			return HasValueGenerator((Func<IProperty, IEntityType, ValueGenerator>)null, configurationSource);
		}
		if (!typeof(ValueGenerator).GetTypeInfo().IsAssignableFrom(valueGeneratorType.GetTypeInfo()))
		{
			throw new ArgumentException(CoreStrings.BadValueGeneratorType(valueGeneratorType.ShortDisplayName(), typeof(ValueGenerator).ShortDisplayName()));
		}
		return HasValueGenerator(delegate
		{
			try
			{
				return (ValueGenerator)Activator.CreateInstance(valueGeneratorType);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException(CoreStrings.CannotCreateValueGenerator(valueGeneratorType.ShortDisplayName()), innerException);
			}
		}, configurationSource);
	}

	public virtual InternalPropertyBuilder HasValueGenerator([JetBrains.Annotations.CanBeNull] Func<IProperty, IEntityType, ValueGenerator> factory, ConfigurationSource configurationSource)
	{
		if (CanSetValueGenerator(factory, configurationSource))
		{
			Metadata.SetValueGeneratorFactory(factory, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetValueGenerator(Func<IProperty, IEntityType, ValueGenerator> factory, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetValueGeneratorFactoryConfigurationSource()))
		{
			return Metadata.GetValueGeneratorFactory() == factory;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasConversion([JetBrains.Annotations.CanBeNull] ValueConverter converter, ConfigurationSource configurationSource)
	{
		if (configurationSource.Overrides(Metadata.GetValueConverterConfigurationSource()) || Metadata.GetValueConverter() == converter)
		{
			Metadata.SetValueConverter(converter, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetConversion([JetBrains.Annotations.CanBeNull] ValueConverter converter, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetValueConverterConfigurationSource()) || Metadata.CheckValueConverter(converter) != null)
		{
			return Metadata.GetValueConverter() == converter;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasConversion([JetBrains.Annotations.CanBeNull] Type providerClrType, ConfigurationSource configurationSource)
	{
		if (CanSetConversion(providerClrType, configurationSource))
		{
			Metadata.SetProviderClrType(providerClrType, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetConversion([JetBrains.Annotations.CanBeNull] Type providerClrType, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetProviderClrTypeConfigurationSource()))
		{
			return Metadata.GetProviderClrType() == providerClrType;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource configurationSource)
	{
		if (configurationSource.Overrides(Metadata.GetValueComparerConfigurationSource()) || Metadata.GetValueComparer() == comparer)
		{
			Metadata.SetValueComparer(comparer, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetValueComparerConfigurationSource()) || Metadata.CheckValueComparer(comparer) != null)
		{
			return Metadata.GetValueComparer() == comparer;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasKeyValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource configurationSource)
	{
		if (configurationSource.Overrides(Metadata.GetKeyValueComparerConfigurationSource()) || Metadata.GetKeyValueComparer() == comparer)
		{
			Metadata.SetKeyValueComparer(comparer, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetKeyValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetKeyValueComparerConfigurationSource()) || Metadata.CheckValueComparer(comparer) != null)
		{
			return Metadata.GetKeyValueComparer() == comparer;
		}
		return true;
	}

	public virtual InternalPropertyBuilder HasStructuralValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource configurationSource)
	{
		if (configurationSource.Overrides(Metadata.GetStructuralValueComparerConfigurationSource()) || Metadata.GetStructuralValueComparer() == comparer)
		{
			Metadata.SetStructuralValueComparer(comparer, configurationSource);
			return this;
		}
		return null;
	}

	public virtual bool CanSetStructuralValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource? configurationSource)
	{
		if (!configurationSource.Overrides(Metadata.GetStructuralValueComparerConfigurationSource()) || Metadata.CheckValueComparer(comparer) != null)
		{
			return Metadata.GetStructuralValueComparer() == comparer;
		}
		return true;
	}

	public virtual InternalPropertyBuilder Attach([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder entityTypeBuilder)
	{
		Property property = entityTypeBuilder.Metadata.FindProperty(Metadata.Name);
		ConfigurationSource configurationSource = Metadata.GetConfigurationSource();
		ConfigurationSource? typeConfigurationSource = Metadata.GetTypeConfigurationSource();
		InternalPropertyBuilder internalPropertyBuilder;
		if (property != null && (property.GetConfigurationSource().Overrides(configurationSource) || property.GetTypeConfigurationSource().Overrides(typeConfigurationSource) || (Metadata.ClrType == property.ClrType && Metadata.GetIdentifyingMemberInfo()?.Name == property.GetIdentifyingMemberInfo()?.Name)))
		{
			internalPropertyBuilder = property.Builder;
			property.UpdateConfigurationSource(configurationSource);
			if (typeConfigurationSource.HasValue)
			{
				property.UpdateTypeConfigurationSource(typeConfigurationSource.Value);
			}
		}
		else
		{
			internalPropertyBuilder = ((Metadata.GetIdentifyingMemberInfo() == null) ? entityTypeBuilder.Property(Metadata.ClrType, Metadata.Name, Metadata.GetTypeConfigurationSource(), configurationSource) : entityTypeBuilder.Property(Metadata.GetIdentifyingMemberInfo(), configurationSource));
		}
		if (property == Metadata)
		{
			return internalPropertyBuilder;
		}
		internalPropertyBuilder.MergeAnnotationsFrom(Metadata);
		ConfigurationSource? beforeSaveBehaviorConfigurationSource = Metadata.GetBeforeSaveBehaviorConfigurationSource();
		if (beforeSaveBehaviorConfigurationSource.HasValue)
		{
			internalPropertyBuilder.BeforeSave(Metadata.GetBeforeSaveBehavior(), beforeSaveBehaviorConfigurationSource.Value);
		}
		ConfigurationSource? afterSaveBehaviorConfigurationSource = Metadata.GetAfterSaveBehaviorConfigurationSource();
		if (afterSaveBehaviorConfigurationSource.HasValue)
		{
			internalPropertyBuilder.AfterSave(Metadata.GetAfterSaveBehavior(), afterSaveBehaviorConfigurationSource.Value);
		}
		ConfigurationSource? isNullableConfigurationSource = Metadata.GetIsNullableConfigurationSource();
		if (isNullableConfigurationSource.HasValue)
		{
			internalPropertyBuilder.IsRequired(!Metadata.IsNullable, isNullableConfigurationSource.Value);
		}
		ConfigurationSource? isConcurrencyTokenConfigurationSource = Metadata.GetIsConcurrencyTokenConfigurationSource();
		if (isConcurrencyTokenConfigurationSource.HasValue)
		{
			internalPropertyBuilder.IsConcurrencyToken(Metadata.IsConcurrencyToken, isConcurrencyTokenConfigurationSource.Value);
		}
		ConfigurationSource? valueGeneratedConfigurationSource = Metadata.GetValueGeneratedConfigurationSource();
		if (valueGeneratedConfigurationSource.HasValue)
		{
			internalPropertyBuilder.ValueGenerated(Metadata.ValueGenerated, valueGeneratedConfigurationSource.Value);
		}
		ConfigurationSource? fieldInfoConfigurationSource = Metadata.GetFieldInfoConfigurationSource();
		if (fieldInfoConfigurationSource.HasValue && internalPropertyBuilder.CanSetField(Metadata.FieldInfo, fieldInfoConfigurationSource))
		{
			internalPropertyBuilder.HasField(Metadata.FieldInfo, fieldInfoConfigurationSource.Value);
		}
		return internalPropertyBuilder;
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.IsRequired(bool? required, bool fromDataAnnotation)
	{
		return IsRequired(required, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetIsRequired(bool? required, bool fromDataAnnotation)
	{
		return CanSetIsRequired(required, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.ValueGenerated(ValueGenerated? valueGenerated, bool fromDataAnnotation)
	{
		return ValueGenerated(valueGenerated, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetValueGenerated(ValueGenerated? valueGenerated, bool fromDataAnnotation)
	{
		return CanSetValueGenerated(valueGenerated, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.IsConcurrencyToken(bool? concurrencyToken, bool fromDataAnnotation)
	{
		return IsConcurrencyToken(concurrencyToken, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetIsConcurrencyToken(bool? concurrencyToken, bool fromDataAnnotation)
	{
		return CanSetIsConcurrencyToken(concurrencyToken, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasField(string fieldName, bool fromDataAnnotation)
	{
		return HasField(fieldName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasField(FieldInfo fieldInfo, bool fromDataAnnotation)
	{
		return HasField(fieldInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetField(string fieldName, bool fromDataAnnotation)
	{
		return CanSetField(fieldName, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetField(FieldInfo fieldInfo, bool fromDataAnnotation)
	{
		return CanSetField(fieldInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.UsePropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation)
	{
		return UsePropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, bool fromDataAnnotation)
	{
		return CanSetPropertyAccessMode(propertyAccessMode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasMaxLength(int? maxLength, bool fromDataAnnotation)
	{
		return HasMaxLength(maxLength, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetMaxLength(int? maxLength, bool fromDataAnnotation)
	{
		return CanSetMaxLength(maxLength, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.IsUnicode(bool? unicode, bool fromDataAnnotation)
	{
		return IsUnicode(unicode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetIsUnicode(bool? unicode, bool fromDataAnnotation)
	{
		return CanSetIsUnicode(unicode, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.BeforeSave(PropertySaveBehavior? behavior, bool fromDataAnnotation)
	{
		return BeforeSave(behavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetBeforeSave(PropertySaveBehavior? behavior, bool fromDataAnnotation)
	{
		return CanSetBeforeSave(behavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.AfterSave(PropertySaveBehavior? behavior, bool fromDataAnnotation)
	{
		return AfterSave(behavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetAfterSave(PropertySaveBehavior? behavior, bool fromDataAnnotation)
	{
		return CanSetAfterSave(behavior, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasValueGenerator(Type valueGeneratorType, bool fromDataAnnotation)
	{
		return HasValueGenerator(valueGeneratorType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasValueGenerator(Func<IProperty, IEntityType, ValueGenerator> factory, bool fromDataAnnotation)
	{
		return HasValueGenerator(factory, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetValueGenerator(Func<IProperty, IEntityType, ValueGenerator> factory, bool fromDataAnnotation)
	{
		return CanSetValueGenerator(factory, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasConversion(ValueConverter converter, bool fromDataAnnotation)
	{
		return HasConversion(converter, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetConversion(ValueConverter converter, bool fromDataAnnotation)
	{
		return CanSetConversion(converter, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasConversion(Type providerClrType, bool fromDataAnnotation)
	{
		return HasConversion(providerClrType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetConversion(Type providerClrType, bool fromDataAnnotation)
	{
		return CanSetConversion(providerClrType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasValueComparer(ValueComparer comparer, bool fromDataAnnotation)
	{
		return HasValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetValueComparer(ValueComparer comparer, bool fromDataAnnotation)
	{
		return CanSetValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasKeyValueComparer(ValueComparer comparer, bool fromDataAnnotation)
	{
		return HasKeyValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetKeyValueComparer(ValueComparer comparer, bool fromDataAnnotation)
	{
		return CanSetKeyValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	IConventionPropertyBuilder IConventionPropertyBuilder.HasStructuralValueComparer(ValueComparer comparer, bool fromDataAnnotation)
	{
		return HasStructuralValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionPropertyBuilder.CanSetStructuralValueComparer(ValueComparer comparer, bool fromDataAnnotation)
	{
		return CanSetStructuralValueComparer(comparer, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
