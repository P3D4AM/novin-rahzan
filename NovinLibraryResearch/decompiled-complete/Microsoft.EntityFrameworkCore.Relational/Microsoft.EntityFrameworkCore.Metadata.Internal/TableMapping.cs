using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class TableMapping
{
	public virtual string Schema { get; }

	public virtual string Name { get; }

	public virtual IReadOnlyList<IEntityType> EntityTypes { get; }

	public TableMapping([JetBrains.Annotations.CanBeNull] string schema, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> entityTypes)
	{
		Schema = schema;
		Name = name;
		EntityTypes = entityTypes;
	}

	public virtual IEntityType GetRootType()
	{
		return EntityTypes.SingleOrDefault((IEntityType t) => t.BaseType == null && (t.FindDeclaredPrimaryKey() == null || t.FindForeignKeys(t.FindDeclaredPrimaryKey().Properties).All((IForeignKey fk) => !fk.PrincipalKey.IsPrimaryKey() || fk.PrincipalEntityType.GetRootType() == t || t.GetTableName() != fk.PrincipalEntityType.GetTableName() || t.GetSchema() != fk.PrincipalEntityType.GetSchema())));
	}

	public virtual IEnumerable<IProperty> GetProperties()
	{
		return GetPropertyMap().Values;
	}

	public virtual Dictionary<string, IProperty> GetPropertyMap()
	{
		Dictionary<string, IProperty> dictionary = new Dictionary<string, IProperty>(StringComparer.Ordinal);
		foreach (IProperty item in EntityTypes.SelectMany(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDeclaredProperties))
		{
			string columnName = item.GetColumnName();
			if (!dictionary.TryGetValue(columnName, out var value) || (value.IsColumnNullable() && !item.IsColumnNullable()))
			{
				dictionary[columnName] = item;
			}
		}
		return dictionary;
	}

	public virtual IEnumerable<IKey> GetKeys()
	{
		return EntityTypes.SelectMany(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDeclaredKeys).Distinct((IKey x, IKey y) => x.GetName() == y.GetName());
	}

	public virtual IEnumerable<IIndex> GetIndexes()
	{
		return EntityTypes.SelectMany(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDeclaredIndexes).Distinct((IIndex x, IIndex y) => x.GetName() == y.GetName());
	}

	public virtual IEnumerable<IForeignKey> GetForeignKeys()
	{
		return from fk in EntityTypes.SelectMany(Microsoft.EntityFrameworkCore.EntityTypeExtensions.GetDeclaredForeignKeys).Distinct((IForeignKey x, IForeignKey y) => x.GetConstraintName() == y.GetConstraintName())
			where !EntityTypes.Contains(fk.PrincipalEntityType) || !fk.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(fk.PrincipalKey.Properties.Select((IProperty p) => p.GetColumnName()))
			select fk;
	}

	public virtual IEnumerable<ICheckConstraint> GetCheckConstraints()
	{
		return EntityTypes.SelectMany(CheckConstraint.GetCheckConstraints).Distinct((CheckConstraint x, CheckConstraint y) => x.Name == y.Name);
	}

	public static IReadOnlyList<TableMapping> GetTableMappings([JetBrains.Annotations.NotNull] IModel model)
	{
		Dictionary<(string, string), List<IEntityType>> dictionary = new Dictionary<(string, string), List<IEntityType>>();
		foreach (IEntityType item in from et in model.GetEntityTypes()
			where !et.IsIgnoredByMigrations()
			select et)
		{
			(string, string) key = (item.GetSchema(), item.GetTableName());
			if (!dictionary.TryGetValue(key, out var value))
			{
				value = new List<IEntityType>();
				dictionary.Add(key, value);
			}
			value.Add(item);
		}
		return (from kv in dictionary
			select new TableMapping(kv.Key.Schema, kv.Key.TableName, kv.Value) into t
			orderby t.Schema, t.Name
			select t).ToList();
	}

	public static TableMapping GetTableMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] string table, [JetBrains.Annotations.CanBeNull] string schema)
	{
		List<IEntityType> list = new List<IEntityType>();
		foreach (IEntityType item in from et in model.GetEntityTypes()
			where et.FindPrimaryKey() != null
			select et)
		{
			if (table == item.GetTableName() && schema == item.GetSchema())
			{
				list.Add(item);
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return new TableMapping(schema, table, list);
	}

	public virtual string GetComment()
	{
		return EntityTypes.Select((IEntityType e) => e.GetComment()).FirstOrDefault((string c) => c != null);
	}
}
