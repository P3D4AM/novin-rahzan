using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class CheckConstraint : IConventionCheckConstraint, ICheckConstraint
{
	private ConfigurationSource _configurationSource;

	public virtual IEntityType EntityType { get; }

	public virtual string Name { get; }

	public virtual string Sql { get; }

	IConventionEntityType IConventionCheckConstraint.EntityType => (IConventionEntityType)EntityType;

	public CheckConstraint([JetBrains.Annotations.NotNull] IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] string sql, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(sql, "sql");
		EntityType = entityType;
		Name = name;
		Sql = sql;
		_configurationSource = configurationSource;
		Dictionary<string, CheckConstraint> dictionary = GetAnnotationsDictionary(EntityType);
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, CheckConstraint>();
			((IMutableEntityType)EntityType).SetOrRemoveAnnotation("Relational:CheckConstraints", dictionary);
		}
		if (dictionary.ContainsKey(Name))
		{
			throw new InvalidOperationException(RelationalStrings.DuplicateCheckConstraint(Name, EntityType.DisplayName()));
		}
		dictionary.Add(name, this);
	}

	public static IEnumerable<CheckConstraint> GetCheckConstraints([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		IEnumerable<CheckConstraint> enumerable = GetAnnotationsDictionary(entityType)?.Values;
		return enumerable ?? Enumerable.Empty<CheckConstraint>();
	}

	public static ICheckConstraint FindCheckConstraint([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		Dictionary<string, CheckConstraint> annotationsDictionary = GetAnnotationsDictionary(entityType);
		if (annotationsDictionary == null)
		{
			return null;
		}
		if (!annotationsDictionary.TryGetValue(name, out var value))
		{
			return null;
		}
		return value;
	}

	public static bool RemoveCheckConstraint([JetBrains.Annotations.NotNull] IMutableEntityType entityType, [JetBrains.Annotations.NotNull] string name)
	{
		return GetAnnotationsDictionary(entityType)?.Remove(name) ?? false;
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		_configurationSource = configurationSource.Max(_configurationSource);
	}

	private static Dictionary<string, CheckConstraint> GetAnnotationsDictionary(IEntityType entityType)
	{
		return (Dictionary<string, CheckConstraint>)entityType["Relational:CheckConstraints"];
	}
}
