using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Key : ConventionAnnotatable, IMutableKey, IMutableAnnotatable, IAnnotatable, IKey, IConventionKey, IConventionAnnotatable
{
	private ConfigurationSource _configurationSource;

	private Func<bool, IIdentityMap> _identityMapFactory;

	private object _principalKeyValueFactory;

	public virtual IReadOnlyList<Property> Properties
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual EntityType DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return Properties[0].DeclaringEntityType;
		}
	}

	public virtual InternalKeyBuilder Builder
	{
		[DebuggerStepThrough]
		get;
		[DebuggerStepThrough]
		[param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual Func<bool, IIdentityMap> IdentityMapFactory => NonCapturingLazyInitializer.EnsureInitialized(ref _identityMapFactory, this, (Key k) => new IdentityMapFactoryFactory().Create(k));

	public virtual ISet<ForeignKey> ReferencingForeignKeys
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual DebugView<Key> DebugView => new DebugView<Key>(this, (Key m) => m.ToDebugString(singleLine: false));

	IReadOnlyList<IProperty> IKey.Properties
	{
		[DebuggerStepThrough]
		get
		{
			return Properties;
		}
	}

	IEntityType IKey.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IReadOnlyList<IMutableProperty> IMutableKey.Properties
	{
		[DebuggerStepThrough]
		get
		{
			return Properties;
		}
	}

	IMutableEntityType IMutableKey.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	IConventionKeyBuilder IConventionKey.Builder
	{
		[DebuggerStepThrough]
		get
		{
			return Builder;
		}
	}

	IReadOnlyList<IConventionProperty> IConventionKey.Properties
	{
		[DebuggerStepThrough]
		get
		{
			return Properties;
		}
	}

	IConventionEntityType IConventionKey.DeclaringEntityType
	{
		[DebuggerStepThrough]
		get
		{
			return DeclaringEntityType;
		}
	}

	public Key([JetBrains.Annotations.NotNull] IReadOnlyList<Property> properties, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(properties, "properties");
		Microsoft.EntityFrameworkCore.Utilities.Check.HasNoNulls(properties, "properties");
		Properties = properties;
		_configurationSource = configurationSource;
		Builder = new InternalKeyBuilder(this, DeclaringEntityType.Model.Builder);
	}

	[DebuggerStepThrough]
	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		_configurationSource = configurationSource.Max(_configurationSource);
		foreach (Property property in Properties)
		{
			property.UpdateConfigurationSource(configurationSource);
		}
	}

	protected override IConventionAnnotation OnAnnotationSet(string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation)
	{
		return Builder.ModelBuilder.Metadata.ConventionDispatcher.OnKeyAnnotationChanged(Builder, name, annotation, oldAnnotation);
	}

	public virtual IEnumerable<ForeignKey> GetReferencingForeignKeys()
	{
		IEnumerable<ForeignKey> referencingForeignKeys = ReferencingForeignKeys;
		return referencingForeignKeys ?? Enumerable.Empty<ForeignKey>();
	}

	public virtual IPrincipalKeyValueFactory<TKey> GetPrincipalKeyValueFactory<TKey>()
	{
		return (IPrincipalKeyValueFactory<TKey>)NonCapturingLazyInitializer.EnsureInitialized(ref _principalKeyValueFactory, this, (Key k) => new KeyValueFactoryFactory().Create<TKey>(k));
	}

	public override string ToString()
	{
		return this.ToDebugString();
	}
}
