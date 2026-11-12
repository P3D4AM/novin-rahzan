using System;
using System.ComponentModel;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PropertyBuilder : IInfrastructure<InternalPropertyBuilder>
{
	InternalPropertyBuilder IInfrastructure<InternalPropertyBuilder>.Instance => Builder;

	public virtual IMutableProperty Metadata => Builder.Metadata;

	private InternalPropertyBuilder Builder { get; }

	[EntityFrameworkInternal]
	public PropertyBuilder([JetBrains.Annotations.NotNull] IMutableProperty property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
		Builder = ((Property)property).Builder;
	}

	public virtual PropertyBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder IsRequired(bool required = true)
	{
		Builder.IsRequired(required, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder HasMaxLength(int maxLength)
	{
		Builder.HasMaxLength(maxLength, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder IsUnicode(bool unicode = true)
	{
		Builder.IsUnicode(unicode, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder IsRowVersion()
	{
		Builder.ValueGenerated(ValueGenerated.OnAddOrUpdate, ConfigurationSource.Explicit);
		Builder.IsConcurrencyToken(true, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder HasValueGenerator<TGenerator>() where TGenerator : ValueGenerator
	{
		Builder.HasValueGenerator(typeof(TGenerator), ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder HasValueGenerator([JetBrains.Annotations.CanBeNull] Type valueGeneratorType)
	{
		Builder.HasValueGenerator(valueGeneratorType, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder HasValueGenerator([JetBrains.Annotations.NotNull] Func<IProperty, IEntityType, ValueGenerator> factory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(factory, "factory");
		Builder.HasValueGenerator(factory, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder IsConcurrencyToken(bool concurrencyToken = true)
	{
		Builder.IsConcurrencyToken(concurrencyToken, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder ValueGeneratedNever()
	{
		Builder.ValueGenerated(ValueGenerated.Never, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder ValueGeneratedOnAdd()
	{
		Builder.ValueGenerated(ValueGenerated.OnAdd, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder ValueGeneratedOnAddOrUpdate()
	{
		Builder.ValueGenerated(ValueGenerated.OnAddOrUpdate, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder ValueGeneratedOnUpdate()
	{
		Builder.ValueGenerated(ValueGenerated.OnUpdate, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder HasField([JetBrains.Annotations.NotNull] string fieldName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(fieldName, "fieldName");
		Builder.HasField(fieldName, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
	{
		Builder.UsePropertyAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder HasConversion<TProvider>()
	{
		return HasConversion(typeof(TProvider));
	}

	public virtual PropertyBuilder HasConversion([JetBrains.Annotations.CanBeNull] Type providerClrType)
	{
		Builder.HasConversion(providerClrType, ConfigurationSource.Explicit);
		return this;
	}

	public virtual PropertyBuilder HasConversion([JetBrains.Annotations.CanBeNull] ValueConverter converter)
	{
		Builder.HasConversion(converter, ConfigurationSource.Explicit);
		return this;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
public class PropertyBuilder<TProperty> : PropertyBuilder
{
	[EntityFrameworkInternal]
	public PropertyBuilder([JetBrains.Annotations.NotNull] IMutableProperty property)
		: base(property)
	{
	}

	public new virtual PropertyBuilder<TProperty> HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		return (PropertyBuilder<TProperty>)base.HasAnnotation(annotation, value);
	}

	public new virtual PropertyBuilder<TProperty> IsRequired(bool required = true)
	{
		return (PropertyBuilder<TProperty>)base.IsRequired(required);
	}

	public new virtual PropertyBuilder<TProperty> HasMaxLength(int maxLength)
	{
		return (PropertyBuilder<TProperty>)base.HasMaxLength(maxLength);
	}

	public new virtual PropertyBuilder<TProperty> IsUnicode(bool unicode = true)
	{
		return (PropertyBuilder<TProperty>)base.IsUnicode(unicode);
	}

	public new virtual PropertyBuilder<TProperty> IsRowVersion()
	{
		return (PropertyBuilder<TProperty>)base.IsRowVersion();
	}

	public new virtual PropertyBuilder<TProperty> HasValueGenerator<TGenerator>() where TGenerator : ValueGenerator
	{
		return (PropertyBuilder<TProperty>)base.HasValueGenerator<TGenerator>();
	}

	public new virtual PropertyBuilder<TProperty> HasValueGenerator([JetBrains.Annotations.CanBeNull] Type valueGeneratorType)
	{
		return (PropertyBuilder<TProperty>)base.HasValueGenerator(valueGeneratorType);
	}

	public new virtual PropertyBuilder<TProperty> HasValueGenerator([JetBrains.Annotations.NotNull] Func<IProperty, IEntityType, ValueGenerator> factory)
	{
		return (PropertyBuilder<TProperty>)base.HasValueGenerator(factory);
	}

	public new virtual PropertyBuilder<TProperty> IsConcurrencyToken(bool concurrencyToken = true)
	{
		return (PropertyBuilder<TProperty>)base.IsConcurrencyToken(concurrencyToken);
	}

	public new virtual PropertyBuilder<TProperty> ValueGeneratedNever()
	{
		return (PropertyBuilder<TProperty>)base.ValueGeneratedNever();
	}

	public new virtual PropertyBuilder<TProperty> ValueGeneratedOnAdd()
	{
		return (PropertyBuilder<TProperty>)base.ValueGeneratedOnAdd();
	}

	public new virtual PropertyBuilder<TProperty> ValueGeneratedOnAddOrUpdate()
	{
		return (PropertyBuilder<TProperty>)base.ValueGeneratedOnAddOrUpdate();
	}

	public new virtual PropertyBuilder<TProperty> ValueGeneratedOnUpdate()
	{
		return (PropertyBuilder<TProperty>)base.ValueGeneratedOnUpdate();
	}

	public new virtual PropertyBuilder<TProperty> HasField([JetBrains.Annotations.NotNull] string fieldName)
	{
		return (PropertyBuilder<TProperty>)base.HasField(fieldName);
	}

	public new virtual PropertyBuilder<TProperty> HasConversion<TProvider>()
	{
		return (PropertyBuilder<TProperty>)base.HasConversion<TProvider>();
	}

	public new virtual PropertyBuilder<TProperty> HasConversion([JetBrains.Annotations.CanBeNull] Type providerClrType)
	{
		return (PropertyBuilder<TProperty>)base.HasConversion(providerClrType);
	}

	public virtual PropertyBuilder<TProperty> HasConversion<TProvider>([JetBrains.Annotations.NotNull] Expression<Func<TProperty, TProvider>> convertToProviderExpression, [JetBrains.Annotations.NotNull] Expression<Func<TProvider, TProperty>> convertFromProviderExpression)
	{
		return HasConversion(new ValueConverter<TProperty, TProvider>(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(convertToProviderExpression, "convertToProviderExpression"), Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(convertFromProviderExpression, "convertFromProviderExpression")));
	}

	public virtual PropertyBuilder<TProperty> HasConversion<TProvider>([JetBrains.Annotations.CanBeNull] ValueConverter<TProperty, TProvider> converter)
	{
		return HasConversion((ValueConverter)converter);
	}

	public new virtual PropertyBuilder<TProperty> HasConversion([JetBrains.Annotations.CanBeNull] ValueConverter converter)
	{
		return (PropertyBuilder<TProperty>)base.HasConversion(converter);
	}

	public new virtual PropertyBuilder<TProperty> UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
	{
		return (PropertyBuilder<TProperty>)base.UsePropertyAccessMode(propertyAccessMode);
	}
}
