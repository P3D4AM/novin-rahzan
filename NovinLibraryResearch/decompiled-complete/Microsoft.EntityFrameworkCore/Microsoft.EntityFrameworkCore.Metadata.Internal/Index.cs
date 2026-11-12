using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Index : ConventionAnnotatable, IMutableIndex, IIndex, IAnnotatable, IMutableAnnotatable, IConventionIndex, IConventionAnnotatable
{
	private bool? _isUnique;

	private ConfigurationSource _configurationSource;

	private ConfigurationSource? _isUniqueConfigurationSource;

	private object _nullableValueFactory;

	public virtual IReadOnlyList<Property> Properties
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual EntityType DeclaringEntityType
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual InternalIndexBuilder Builder
	{
		[DebuggerStepThrough]
		get;
		[DebuggerStepThrough]
		[param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual bool IsUnique
	{
		get
		{
			return _isUnique ?? DefaultIsUnique;
		}
		set
		{
			SetIsUnique(value, ConfigurationSource.Explicit);
		}
	}

	private static bool DefaultIsUnique => false;

	public virtual DebugView<Index> DebugView => new DebugView<Index>(this, (Index m) => m.ToDebugString(singleLine: false));

	IReadOnlyList<IProperty> IIndex.Properties => Properties;

	IEntityType IIndex.DeclaringEntityType => DeclaringEntityType;

	IReadOnlyList<IMutableProperty> IMutableIndex.Properties => Properties;

	IMutableEntityType IMutableIndex.DeclaringEntityType => DeclaringEntityType;

	IConventionIndexBuilder IConventionIndex.Builder => Builder;

	IReadOnlyList<IConventionProperty> IConventionIndex.Properties => Properties;

	IConventionEntityType IConventionIndex.DeclaringEntityType => DeclaringEntityType;

	public Index([JetBrains.Annotations.NotNull] IReadOnlyList<Property> properties, [JetBrains.Annotations.NotNull] EntityType declaringEntityType, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(declaringEntityType, "declaringEntityType");
		Properties = properties;
		DeclaringEntityType = declaringEntityType;
		_configurationSource = configurationSource;
		Builder = new InternalIndexBuilder(this, declaringEntityType.Model.Builder);
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		_configurationSource = configurationSource.Max(_configurationSource);
	}

	public virtual Index SetIsUnique(bool? unique, ConfigurationSource configurationSource)
	{
		bool num = IsUnique != unique;
		_isUnique = unique;
		if (!unique.HasValue)
		{
			_isUniqueConfigurationSource = null;
		}
		else
		{
			UpdateIsUniqueConfigurationSource(configurationSource);
		}
		if (!num)
		{
			return this;
		}
		return (Index)(DeclaringEntityType.Model.ConventionDispatcher.OnIndexUniquenessChanged(Builder)?.Metadata);
	}

	public virtual ConfigurationSource? GetIsUniqueConfigurationSource()
	{
		return _isUniqueConfigurationSource;
	}

	private void UpdateIsUniqueConfigurationSource(ConfigurationSource configurationSource)
	{
		_isUniqueConfigurationSource = configurationSource.Max(_isUniqueConfigurationSource);
	}

	protected override IConventionAnnotation OnAnnotationSet(string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
	{
		return Builder.ModelBuilder.Metadata.ConventionDispatcher.OnIndexAnnotationChanged(Builder, name, annotation, oldAnnotation);
	}

	public virtual INullableValueFactory<TKey> GetNullableValueFactory<TKey>()
	{
		return (INullableValueFactory<TKey>)NonCapturingLazyInitializer.EnsureInitialized(ref _nullableValueFactory, this, (Index i) => new CompositeNullableValueFactory(i.Properties));
	}

	public override string ToString()
	{
		return this.ToDebugString();
	}

	void IConventionIndex.SetIsUnique(bool? unique, bool fromDataAnnotation)
	{
		SetIsUnique(unique, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
