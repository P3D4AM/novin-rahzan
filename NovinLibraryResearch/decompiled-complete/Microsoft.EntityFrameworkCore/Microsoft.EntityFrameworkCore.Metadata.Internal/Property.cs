using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Property : PropertyBase, IMutableProperty, IProperty, IPropertyBase, IAnnotatable, IMutablePropertyBase, IMutableAnnotatable, IConventionProperty, IConventionPropertyBase, IConventionAnnotatable
{
	private bool? _isConcurrencyToken;

	private bool? _isNullable;

	private ValueGenerated? _valueGenerated;

	private ConfigurationSource _configurationSource;

	private ConfigurationSource? _typeConfigurationSource;

	private ConfigurationSource? _isNullableConfigurationSource;

	private ConfigurationSource? _isConcurrencyTokenConfigurationSource;

	private ConfigurationSource? _valueGeneratedConfigurationSource;

	public virtual EntityType DeclaringEntityType
	{
		[DebuggerStepThrough]
		get;
	}

	public override TypeBase DeclaringType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	public override Type ClrType
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual InternalPropertyBuilder Builder
	{
		[DebuggerStepThrough]
		get;
		[DebuggerStepThrough]
		[param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual bool IsNullable
	{
		get
		{
			return _isNullable ?? DefaultIsNullable;
		}
		set
		{
			SetIsNullable(value, ConfigurationSource.Explicit);
		}
	}

	private bool DefaultIsNullable => ClrType.IsNullableType();

	public virtual ValueGenerated ValueGenerated
	{
		get
		{
			return _valueGenerated ?? DefaultValueGenerated;
		}
		set
		{
			SetValueGenerated(value, ConfigurationSource.Explicit);
		}
	}

	private static ValueGenerated DefaultValueGenerated => ValueGenerated.Never;

	public virtual bool IsConcurrencyToken
	{
		get
		{
			return _isConcurrencyToken ?? DefaultIsConcurrencyToken;
		}
		set
		{
			SetIsConcurrencyToken(value, ConfigurationSource.Explicit);
		}
	}

	private static bool DefaultIsConcurrencyToken => false;

	public virtual IKey PrimaryKey
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual List<Key> Keys
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual List<ForeignKey> ForeignKeys
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual List<Index> Indexes
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual DebugView<Property> DebugView => new DebugView<Property>(this, (Property m) => m.ToDebugString(singleLine: false));

	IConventionPropertyBuilder IConventionProperty.Builder
	{
		[DebuggerStepThrough]
		get
		{
			return Builder;
		}
	}

	IEntityType IProperty.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IMutableEntityType IMutableProperty.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IConventionEntityType IConventionProperty.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	public Property([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.CanBeNull] PropertyInfo propertyInfo, [JetBrains.Annotations.CanBeNull] FieldInfo fieldInfo, [JetBrains.Annotations.NotNull] EntityType declaringEntityType, ConfigurationSource configurationSource, ConfigurationSource? typeConfigurationSource)
		: base(name, propertyInfo, fieldInfo)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(clrType, "clrType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(declaringEntityType, "declaringEntityType");
		DeclaringEntityType = declaringEntityType;
		ClrType = clrType;
		_configurationSource = configurationSource;
		_typeConfigurationSource = typeConfigurationSource;
		Builder = new InternalPropertyBuilder(this, declaringEntityType.Model.Builder);
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual bool UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		ConfigurationSource configurationSource2 = _configurationSource;
		_configurationSource = configurationSource.Max(_configurationSource);
		return _configurationSource != configurationSource2;
	}

	public virtual void SetConfigurationSource(ConfigurationSource configurationSource)
	{
		_configurationSource = configurationSource;
	}

	public virtual ConfigurationSource? GetTypeConfigurationSource()
	{
		return _typeConfigurationSource;
	}

	public virtual void UpdateTypeConfigurationSource(ConfigurationSource configurationSource)
	{
		_typeConfigurationSource = _typeConfigurationSource.Max(configurationSource);
	}

	public virtual void SetIsNullable(bool? nullable, ConfigurationSource configurationSource)
	{
		bool flag = (nullable ?? DefaultIsNullable) != IsNullable;
		if (!nullable.HasValue)
		{
			_isNullable = null;
			_isNullableConfigurationSource = null;
			if (flag)
			{
				DeclaringEntityType.Model.ConventionDispatcher.OnPropertyNullableChanged(Builder);
			}
			return;
		}
		if (nullable.Value)
		{
			if (!ClrType.IsNullableType())
			{
				throw new InvalidOperationException(CoreStrings.CannotBeNullable(Name, DeclaringEntityType.DisplayName(), ClrType.ShortDisplayName()));
			}
			if (Keys != null)
			{
				throw new InvalidOperationException(CoreStrings.CannotBeNullablePK(Name, DeclaringEntityType.DisplayName()));
			}
		}
		UpdateIsNullableConfigurationSource(configurationSource);
		_isNullable = nullable;
		if (flag)
		{
			DeclaringEntityType.Model.ConventionDispatcher.OnPropertyNullableChanged(Builder);
		}
	}

	public virtual ConfigurationSource? GetIsNullableConfigurationSource()
	{
		return _isNullableConfigurationSource;
	}

	private void UpdateIsNullableConfigurationSource(ConfigurationSource configurationSource)
	{
		_isNullableConfigurationSource = configurationSource.Max(_isNullableConfigurationSource);
	}

	protected override void OnFieldInfoSet(FieldInfo newFieldInfo, FieldInfo oldFieldInfo)
	{
		DeclaringEntityType.Model.ConventionDispatcher.OnPropertyFieldChanged(Builder, newFieldInfo, oldFieldInfo);
	}

	public virtual void SetValueGenerated(ValueGenerated? valueGenerated, ConfigurationSource configurationSource)
	{
		_valueGenerated = valueGenerated;
		if (!valueGenerated.HasValue)
		{
			_valueGeneratedConfigurationSource = null;
		}
		else
		{
			UpdateValueGeneratedConfigurationSource(configurationSource);
		}
	}

	public virtual ConfigurationSource? GetValueGeneratedConfigurationSource()
	{
		return _valueGeneratedConfigurationSource;
	}

	private void UpdateValueGeneratedConfigurationSource(ConfigurationSource configurationSource)
	{
		_valueGeneratedConfigurationSource = configurationSource.Max(_valueGeneratedConfigurationSource);
	}

	public virtual void SetIsConcurrencyToken(bool? concurrencyToken, ConfigurationSource configurationSource)
	{
		if (IsConcurrencyToken != concurrencyToken)
		{
			_isConcurrencyToken = concurrencyToken;
		}
		if (!concurrencyToken.HasValue)
		{
			_isConcurrencyTokenConfigurationSource = null;
		}
		else
		{
			UpdateIsConcurrencyTokenConfigurationSource(configurationSource);
		}
	}

	public virtual ConfigurationSource? GetIsConcurrencyTokenConfigurationSource()
	{
		return _isConcurrencyTokenConfigurationSource;
	}

	private void UpdateIsConcurrencyTokenConfigurationSource(ConfigurationSource configurationSource)
	{
		_isConcurrencyTokenConfigurationSource = configurationSource.Max(_isConcurrencyTokenConfigurationSource);
	}

	public virtual void SetMaxLength(int? maxLength, ConfigurationSource configurationSource)
	{
		if (maxLength.HasValue && maxLength < 0)
		{
			throw new ArgumentOutOfRangeException("maxLength");
		}
		this.SetOrRemoveAnnotation("MaxLength", maxLength, configurationSource);
	}

	public virtual void SetIsUnicode(bool? unicode, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("Unicode", unicode, configurationSource);
	}

	public virtual void SetBeforeSaveBehavior(PropertySaveBehavior? beforeSaveBehavior, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("BeforeSaveBehavior", beforeSaveBehavior, configurationSource);
	}

	public virtual void SetAfterSaveBehavior(PropertySaveBehavior? afterSaveBehavior, ConfigurationSource configurationSource)
	{
		if (afterSaveBehavior.HasValue)
		{
			string text = CheckAfterSaveBehavior(afterSaveBehavior.Value);
			if (text != null)
			{
				throw new InvalidOperationException(text);
			}
		}
		this.SetOrRemoveAnnotation("AfterSaveBehavior", afterSaveBehavior, configurationSource);
	}

	public virtual string CheckAfterSaveBehavior(PropertySaveBehavior behavior)
	{
		if (behavior != PropertySaveBehavior.Throw && this.IsKey())
		{
			return CoreStrings.KeyPropertyMustBeReadOnly(Name, DeclaringEntityType.DisplayName());
		}
		return null;
	}

	public virtual void SetValueGeneratorFactory([JetBrains.Annotations.CanBeNull] Func<IProperty, IEntityType, ValueGenerator> factory, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("ValueGeneratorFactory", factory, configurationSource);
	}

	public virtual void SetValueConverter([JetBrains.Annotations.CanBeNull] ValueConverter converter, ConfigurationSource configurationSource)
	{
		string text = CheckValueConverter(converter);
		if (text != null)
		{
			throw new InvalidOperationException(text);
		}
		this.SetOrRemoveAnnotation("ValueConverter", converter, configurationSource);
	}

	public virtual string CheckValueConverter([JetBrains.Annotations.CanBeNull] ValueConverter converter)
	{
		if (converter != null && converter.ModelClrType.UnwrapNullableType() != ClrType.UnwrapNullableType())
		{
			return CoreStrings.ConverterPropertyMismatch(converter.ModelClrType.ShortDisplayName(), DeclaringEntityType.DisplayName(), Name, ClrType.ShortDisplayName());
		}
		return null;
	}

	public virtual void SetProviderClrType([JetBrains.Annotations.CanBeNull] Type providerClrType, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("ProviderClrType", providerClrType, configurationSource);
	}

	public virtual void SetValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource configurationSource)
	{
		string text = CheckValueComparer(comparer);
		if (text != null)
		{
			throw new InvalidOperationException(text);
		}
		this.SetOrRemoveAnnotation("ValueComparer", comparer, configurationSource);
	}

	public virtual void SetKeyValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource configurationSource)
	{
		string text = CheckValueComparer(comparer);
		if (text != null)
		{
			throw new InvalidOperationException(text);
		}
		this.SetOrRemoveAnnotation("KeyValueComparer", comparer, configurationSource);
	}

	public virtual void SetStructuralValueComparer([JetBrains.Annotations.CanBeNull] ValueComparer comparer, ConfigurationSource configurationSource)
	{
		string text = CheckValueComparer(comparer);
		if (text != null)
		{
			throw new InvalidOperationException(text);
		}
		this.SetOrRemoveAnnotation("StructuralValueComparer", comparer, configurationSource);
	}

	public virtual string CheckValueComparer(ValueComparer comparer)
	{
		if (comparer != null && comparer.Type != ClrType)
		{
			return CoreStrings.ComparerPropertyMismatch(comparer.Type.ShortDisplayName(), DeclaringEntityType.DisplayName(), Name, ClrType.ShortDisplayName());
		}
		return null;
	}

	public virtual IEnumerable<ForeignKey> GetContainingForeignKeys()
	{
		IEnumerable<ForeignKey> foreignKeys = ForeignKeys;
		return foreignKeys ?? Enumerable.Empty<ForeignKey>();
	}

	public virtual IEnumerable<Key> GetContainingKeys()
	{
		IEnumerable<Key> keys = Keys;
		return keys ?? Enumerable.Empty<Key>();
	}

	public virtual IEnumerable<Index> GetContainingIndexes()
	{
		IEnumerable<Index> indexes = Indexes;
		return indexes ?? Enumerable.Empty<Index>();
	}

	protected override IConventionAnnotation OnAnnotationSet(string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
	{
		return DeclaringType.Model.ConventionDispatcher.OnPropertyAnnotationChanged(Builder, name, annotation, oldAnnotation);
	}

	public static string Format([JetBrains.Annotations.NotNull] IEnumerable<string> properties)
	{
		return "{" + string.Join(", ", properties.Select((string p) => (!string.IsNullOrEmpty(p)) ? ("'" + p + "'") : "")) + "}";
	}

	public static bool AreCompatible([JetBrains.Annotations.NotNull] IReadOnlyList<Property> properties, [JetBrains.Annotations.NotNull] EntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		return properties.All((Property property) => property.IsShadowProperty() || (entityType.HasClrType() && ((property.PropertyInfo != null && entityType.GetRuntimeProperties().ContainsKey(property.Name)) || (property.FieldInfo != null && entityType.GetRuntimeFields().ContainsKey(property.Name)))));
	}

	public override string ToString()
	{
		return this.ToDebugString();
	}

	void IConventionProperty.SetIsNullable(bool? nullable, bool fromDataAnnotation)
	{
		SetIsNullable(nullable, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionProperty.SetValueGenerated(ValueGenerated? valueGenerated, bool fromDataAnnotation)
	{
		SetValueGenerated(valueGenerated, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionProperty.SetIsConcurrencyToken(bool? concurrencyToken, bool fromDataAnnotation)
	{
		SetIsConcurrencyToken(concurrencyToken, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
