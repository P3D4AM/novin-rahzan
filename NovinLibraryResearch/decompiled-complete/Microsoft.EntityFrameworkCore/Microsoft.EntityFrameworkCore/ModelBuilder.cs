using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public class ModelBuilder : IInfrastructure<InternalModelBuilder>
{
	private readonly InternalModelBuilder _builder;

	public virtual IMutableModel Model => Builder.Metadata;

	InternalModelBuilder IInfrastructure<InternalModelBuilder>.Instance => _builder;

	private InternalModelBuilder Builder => this.GetInfrastructure();

	public ModelBuilder([JetBrains.Annotations.NotNull] ConventionSet conventions)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(conventions, "conventions");
		_builder = new InternalModelBuilder(new Model(conventions));
		_builder.Metadata.SetProductVersion(ProductInfo.GetVersion());
	}

	[EntityFrameworkInternal]
	public ModelBuilder([JetBrains.Annotations.NotNull] IMutableModel model)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		_builder = ((Model)model).Builder;
	}

	public virtual ModelBuilder HasAnnotation([JetBrains.Annotations.NotNull] string annotation, [JetBrains.Annotations.NotNull] object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotation, "annotation");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(value, "value");
		Builder.HasAnnotation(annotation, value, ConfigurationSource.Explicit);
		return this;
	}

	public virtual EntityTypeBuilder<TEntity> Entity<TEntity>() where TEntity : class
	{
		return new EntityTypeBuilder<TEntity>(Builder.Entity(typeof(TEntity), ConfigurationSource.Explicit, false).Metadata);
	}

	public virtual EntityTypeBuilder Entity([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		return new EntityTypeBuilder(Builder.Entity(type, ConfigurationSource.Explicit, false).Metadata);
	}

	public virtual EntityTypeBuilder Entity([JetBrains.Annotations.NotNull] string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		return new EntityTypeBuilder(Builder.Entity(name, ConfigurationSource.Explicit, false).Metadata);
	}

	public virtual ModelBuilder Entity<TEntity>([JetBrains.Annotations.NotNull] Action<EntityTypeBuilder<TEntity>> buildAction) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(Entity<TEntity>());
		return this;
	}

	public virtual ModelBuilder Entity([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] Action<EntityTypeBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(Entity(type));
		return this;
	}

	public virtual ModelBuilder Entity([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Action<EntityTypeBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(Entity(name));
		return this;
	}

	[Obsolete("Use Entity<TEntity>().HasNoKey() instead")]
	public virtual QueryTypeBuilder<TQuery> Query<TQuery>() where TQuery : class
	{
		InternalEntityTypeBuilder internalEntityTypeBuilder = Builder.Entity(typeof(TQuery), ConfigurationSource.Explicit, false);
		if (internalEntityTypeBuilder.Metadata.BaseType == null)
		{
			internalEntityTypeBuilder.HasNoKey(ConfigurationSource.Explicit);
		}
		return new QueryTypeBuilder<TQuery>(internalEntityTypeBuilder.Metadata);
	}

	[Obsolete("Use Entity(type).HasNoKey() instead")]
	public virtual EntityTypeBuilder Query([JetBrains.Annotations.NotNull] Type type)
	{
		InternalEntityTypeBuilder internalEntityTypeBuilder = Builder.Entity(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type"), ConfigurationSource.Explicit, false);
		if (internalEntityTypeBuilder.Metadata.BaseType == null)
		{
			internalEntityTypeBuilder.HasNoKey(ConfigurationSource.Explicit);
		}
		return new EntityTypeBuilder(internalEntityTypeBuilder.Metadata);
	}

	[Obsolete("Use Entity<TEntity>().HasNoKey() instead")]
	public virtual ModelBuilder Query<TQuery>([JetBrains.Annotations.NotNull] Action<QueryTypeBuilder<TQuery>> buildAction) where TQuery : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(Query<TQuery>());
		return this;
	}

	[Obsolete("Use Entity(type).HasNoKey() instead")]
	public virtual ModelBuilder Query([JetBrains.Annotations.NotNull] Type type, [JetBrains.Annotations.NotNull] Action<EntityTypeBuilder> buildAction)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(buildAction, "buildAction");
		buildAction(Query(type));
		return this;
	}

	public virtual ModelBuilder Ignore<TEntity>() where TEntity : class
	{
		return Ignore(typeof(TEntity));
	}

	public virtual ModelBuilder Ignore([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		Builder.Ignore(type, ConfigurationSource.Explicit);
		return this;
	}

	public virtual ModelBuilder ApplyConfiguration<TEntity>([JetBrains.Annotations.NotNull] IEntityTypeConfiguration<TEntity> configuration) where TEntity : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(configuration, "configuration");
		configuration.Configure(Entity<TEntity>());
		return this;
	}

	[Obsolete("Use IEntityTypeConfiguration<TEntity> instead")]
	public virtual ModelBuilder ApplyConfiguration<TQuery>([JetBrains.Annotations.NotNull] IQueryTypeConfiguration<TQuery> configuration) where TQuery : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(configuration, "configuration");
		configuration.Configure(Query<TQuery>());
		return this;
	}

	public virtual ModelBuilder ApplyConfigurationsFromAssembly([JetBrains.Annotations.NotNull] Assembly assembly, [JetBrains.Annotations.CanBeNull] Func<Type, bool> predicate = null)
	{
		MethodInfo methodInfo = typeof(ModelBuilder).GetMethods().Single((MethodInfo e) => e.Name == "ApplyConfiguration" && e.ContainsGenericParameters && e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
		MethodInfo methodInfo2 = typeof(ModelBuilder).GetMethods().Single((MethodInfo e) => e.Name == "ApplyConfiguration" && e.ContainsGenericParameters && e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IQueryTypeConfiguration<>));
		foreach (TypeInfo constructibleType in assembly.GetConstructibleTypes())
		{
			if (constructibleType.GetConstructor(Type.EmptyTypes) == null || (predicate != null && !predicate(constructibleType)))
			{
				continue;
			}
			Type[] interfaces = constructibleType.GetInterfaces();
			foreach (Type type in interfaces)
			{
				if (type.IsGenericType)
				{
					if (type.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
					{
						methodInfo.MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(this, new object[1] { Activator.CreateInstance(constructibleType) });
					}
					else if (type.GetGenericTypeDefinition() == typeof(IQueryTypeConfiguration<>))
					{
						methodInfo2.MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(this, new object[1] { Activator.CreateInstance(constructibleType) });
					}
				}
			}
		}
		return this;
	}

	public virtual OwnedEntityTypeBuilder<T> Owned<T>() where T : class
	{
		Builder.Owned(typeof(T), ConfigurationSource.Explicit);
		return new OwnedEntityTypeBuilder<T>();
	}

	public virtual OwnedEntityTypeBuilder Owned([JetBrains.Annotations.NotNull] Type type)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(type, "type");
		Builder.Owned(type, ConfigurationSource.Explicit);
		return new OwnedEntityTypeBuilder();
	}

	public virtual ModelBuilder HasChangeTrackingStrategy(ChangeTrackingStrategy changeTrackingStrategy)
	{
		MutableModelExtensions.SetChangeTrackingStrategy(Builder.Metadata, changeTrackingStrategy);
		return this;
	}

	public virtual ModelBuilder UsePropertyAccessMode(PropertyAccessMode propertyAccessMode)
	{
		Builder.UsePropertyAccessMode(propertyAccessMode, ConfigurationSource.Explicit);
		return this;
	}

	public virtual IModel FinalizeModel()
	{
		return Builder.Metadata.FinalizeModel();
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
