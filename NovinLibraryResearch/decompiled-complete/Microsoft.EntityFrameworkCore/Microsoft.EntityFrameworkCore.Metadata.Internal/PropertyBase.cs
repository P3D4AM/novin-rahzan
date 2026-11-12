using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public abstract class PropertyBase : ConventionAnnotatable, IMutablePropertyBase, IPropertyBase, IAnnotatable, IMutableAnnotatable, IConventionPropertyBase, IConventionAnnotatable
{
	private FieldInfo _fieldInfo;

	private ConfigurationSource? _fieldInfoConfigurationSource;

	private IClrPropertyGetter _getter;

	private IClrPropertySetter _setter;

	private IClrPropertySetter _materializationSetter;

	private PropertyAccessors _accessors;

	private PropertyIndexes _indexes;

	public virtual string Name
	{
		[DebuggerStepThrough]
		get;
	}

	public abstract TypeBase DeclaringType { get; }

	public virtual PropertyInfo PropertyInfo
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual FieldInfo FieldInfo
	{
		[DebuggerStepThrough]
		get
		{
			return _fieldInfo;
		}
		[DebuggerStepThrough]
		set
		{
			SetField(value, ConfigurationSource.Explicit);
		}
	}

	public virtual PropertyIndexes PropertyIndexes
	{
		get
		{
			return NonCapturingLazyInitializer.EnsureInitialized(ref _indexes, this, delegate(PropertyBase property)
			{
				_ = (property.DeclaringType as EntityType)?.Counts;
			});
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			if (value == null)
			{
				_indexes = null;
			}
			else
			{
				NonCapturingLazyInitializer.EnsureInitialized(ref _indexes, value);
			}
		}
	}

	public abstract Type ClrType { get; }

	public virtual IClrPropertyGetter Getter => NonCapturingLazyInitializer.EnsureInitialized(ref _getter, this, (PropertyBase p) => new ClrPropertyGetterFactory().Create(p));

	public virtual IClrPropertySetter Setter => NonCapturingLazyInitializer.EnsureInitialized(ref _setter, this, (PropertyBase p) => new ClrPropertySetterFactory().Create(p));

	public virtual IClrPropertySetter MaterializationSetter => NonCapturingLazyInitializer.EnsureInitialized(ref _materializationSetter, this, (PropertyBase p) => new ClrPropertyMaterializationSetterFactory().Create(p));

	public virtual PropertyAccessors Accessors => NonCapturingLazyInitializer.EnsureInitialized(ref _accessors, this, (PropertyBase p) => new PropertyAccessorsFactory().Create(p));

	ITypeBase IPropertyBase.DeclaringType => DeclaringType;

	IMutableTypeBase IMutablePropertyBase.DeclaringType => DeclaringType;

	IConventionTypeBase IConventionPropertyBase.DeclaringType => DeclaringType;

	protected PropertyBase([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] PropertyInfo propertyInfo, [JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Name = name;
		PropertyInfo = propertyInfo;
		_fieldInfo = fieldInfo;
	}

	public virtual void SetField([JetBrains.Annotations.CanBeNull] string fieldName, ConfigurationSource configurationSource)
	{
		if (fieldName == null)
		{
			SetField((FieldInfo)null, configurationSource);
			return;
		}
		if (FieldInfo?.GetSimpleMemberName() == fieldName)
		{
			SetField(FieldInfo, configurationSource);
			return;
		}
		FieldInfo fieldInfo = GetFieldInfo(fieldName, DeclaringType, Name, shouldThrow: true);
		if (fieldInfo != null)
		{
			SetField(fieldInfo, configurationSource);
		}
	}

	public static FieldInfo GetFieldInfo([JetBrains.Annotations.NotNull] string fieldName, [JetBrains.Annotations.NotNull] TypeBase type, [JetBrains.Annotations.CanBeNull] string propertyName, bool shouldThrow)
	{
		if (!type.GetRuntimeFields().TryGetValue(fieldName, out var value) && shouldThrow)
		{
			throw new InvalidOperationException(CoreStrings.MissingBackingField(fieldName, propertyName, type.DisplayName()));
		}
		return value;
	}

	public virtual void SetField([JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, ConfigurationSource configurationSource)
	{
		if (object.Equals(FieldInfo, fieldInfo))
		{
			UpdateFieldInfoConfigurationSource(configurationSource);
			return;
		}
		if (fieldInfo != null)
		{
			IsCompatible(fieldInfo, ClrType, DeclaringType.ClrType, Name, shouldThrow: true);
		}
		if (PropertyInfo == null && fieldInfo?.GetSimpleMemberName() != Name)
		{
			throw new InvalidOperationException(CoreStrings.FieldNameMismatch(fieldInfo?.GetSimpleMemberName(), DeclaringType.DisplayName(), Name));
		}
		UpdateFieldInfoConfigurationSource(configurationSource);
		FieldInfo fieldInfo2 = FieldInfo;
		_fieldInfo = fieldInfo;
		OnFieldInfoSet(fieldInfo, fieldInfo2);
	}

	public virtual void SetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("PropertyAccessMode", propertyAccessMode, configurationSource);
	}

	public static bool IsCompatible([JetBrains.Annotations.NotNull] FieldInfo fieldInfo, [JetBrains.Annotations.NotNull] Type propertyType, [JetBrains.Annotations.CanBeNull] Type entityClrType, [JetBrains.Annotations.CanBeNull] string propertyName, bool shouldThrow)
	{
		if (entityClrType == null || !fieldInfo.DeclaringType.GetTypeInfo().IsAssignableFrom(entityClrType.GetTypeInfo()))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.MissingBackingField(fieldInfo.Name, propertyName, entityClrType?.ShortDisplayName()));
			}
			return false;
		}
		TypeInfo typeInfo = fieldInfo.FieldType.GetTypeInfo();
		if (!typeInfo.IsAssignableFrom(propertyType.GetTypeInfo()) && !propertyType.GetTypeInfo().IsAssignableFrom(typeInfo))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(CoreStrings.BadBackingFieldType(fieldInfo.Name, fieldInfo.FieldType.ShortDisplayName(), entityClrType.ShortDisplayName(), propertyName, propertyType.ShortDisplayName()));
			}
			return false;
		}
		return true;
	}

	protected virtual void OnFieldInfoSet([JetBrains.Annotations.CanBeNull] FieldInfo newFieldInfo, [JetBrains.Annotations.CanBeNull] FieldInfo oldFieldInfo)
	{
	}

	public virtual ConfigurationSource? GetFieldInfoConfigurationSource()
	{
		return _fieldInfoConfigurationSource;
	}

	private void UpdateFieldInfoConfigurationSource(ConfigurationSource configurationSource)
	{
		_fieldInfoConfigurationSource = configurationSource.Max(_fieldInfoConfigurationSource);
	}

	void IConventionPropertyBase.SetField(FieldInfo fieldInfo, bool fromDataAnnotation)
	{
		SetField(fieldInfo, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
