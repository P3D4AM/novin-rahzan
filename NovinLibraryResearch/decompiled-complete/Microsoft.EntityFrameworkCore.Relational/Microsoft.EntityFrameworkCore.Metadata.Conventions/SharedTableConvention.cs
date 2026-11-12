using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class SharedTableConvention : IModelFinalizedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public SharedTableConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		int maxIdentifierLength = modelBuilder.Metadata.GetMaxIdentifierLength();
		Dictionary<(string, string), List<IConventionEntityType>> dictionary = new Dictionary<(string, string), List<IConventionEntityType>>();
		TryUniquifyTableNames(modelBuilder.Metadata, dictionary, maxIdentifierLength);
		Dictionary<string, IConventionProperty> dictionary2 = new Dictionary<string, IConventionProperty>(StringComparer.Ordinal);
		Dictionary<string, IConventionKey> dictionary3 = new Dictionary<string, IConventionKey>(StringComparer.Ordinal);
		Dictionary<string, IConventionForeignKey> dictionary4 = new Dictionary<string, IConventionForeignKey>(StringComparer.Ordinal);
		Dictionary<string, IConventionIndex> indexes = new Dictionary<string, IConventionIndex>(StringComparer.Ordinal);
		foreach (List<IConventionEntityType> value in dictionary.Values)
		{
			dictionary2.Clear();
			dictionary3.Clear();
			dictionary4.Clear();
			foreach (IConventionEntityType item in value)
			{
				TryUniquifyColumnNames(item, dictionary2, maxIdentifierLength);
				TryUniquifyKeyNames(item, dictionary3, maxIdentifierLength);
				TryUniquifyForeignKeyNames(item, dictionary4, maxIdentifierLength);
				TryUniquifyIndexNames(item, indexes, maxIdentifierLength);
			}
		}
	}

	private static void TryUniquifyTableNames(IConventionModel model, Dictionary<(string, string), List<IConventionEntityType>> tables, int maxLength)
	{
		foreach (IConventionEntityType entityType in model.GetEntityTypes())
		{
			(string Schema, string TableName) tableName = (Schema: entityType.GetSchema(), TableName: entityType.GetTableName());
			if (!tables.TryGetValue(tableName, out var value))
			{
				value = new List<IConventionEntityType>();
				tables[tableName] = value;
			}
			if (value.Count > 0 && ShouldUniquify(entityType, value))
			{
				if (entityType["Relational:TableName"] == null)
				{
					string text = Uniquifier.Uniquify(tableName.TableName, tables, (string n) => (tableName.Schema, n), maxLength);
					if (entityType.Builder.ToTable(text) != null)
					{
						tables[(tableName.Schema, text)] = new List<IConventionEntityType> { entityType };
						continue;
					}
				}
				if (value.Count == 1)
				{
					IConventionEntityType conventionEntityType = value.First();
					if (conventionEntityType["Relational:TableName"] == null)
					{
						string text2 = Uniquifier.Uniquify(tableName.TableName, tables, (string n) => (tableName.Schema, n), maxLength);
						if (conventionEntityType.Builder.ToTable(text2) != null)
						{
							value.Remove(conventionEntityType);
							tables[(tableName.Schema, text2)] = new List<IConventionEntityType> { conventionEntityType };
						}
					}
				}
			}
			value.Add(entityType);
		}
	}

	private static bool ShouldUniquify(IConventionEntityType entityType, ICollection<IConventionEntityType> entityTypes)
	{
		IConventionEntityType rootType = entityType.GetRootType();
		IEntityType entityType2 = (entityType.FindPrimaryKey()?.Properties[0])?.FindSharedTableRootPrimaryKeyProperty()?.DeclaringEntityType;
		foreach (IConventionEntityType entityType4 in entityTypes)
		{
			if (entityType2 == entityType4 || rootType == entityType4.GetRootType())
			{
				return false;
			}
			IEntityType entityType3 = (entityType4.FindPrimaryKey()?.Properties[0])?.FindSharedTableRootPrimaryKeyProperty()?.DeclaringEntityType;
			if (entityType3 == entityType || (entityType3 == entityType2 && entityType3 != null))
			{
				return false;
			}
		}
		return true;
	}

	private static void TryUniquifyColumnNames(IConventionEntityType entityType, Dictionary<string, IConventionProperty> properties, int maxLength)
	{
		foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
		{
			string columnName = declaredProperty.GetColumnName();
			if (!properties.TryGetValue(columnName, out var value))
			{
				properties[columnName] = declaredProperty;
				continue;
			}
			MemberInfo identifyingMemberInfo = declaredProperty.GetIdentifyingMemberInfo();
			if (identifyingMemberInfo != null && identifyingMemberInfo.IsSameAs(value.GetIdentifyingMemberInfo()))
			{
				continue;
			}
			bool usePrefix = declaredProperty.DeclaringEntityType != value.DeclaringEntityType || declaredProperty.IsPrimaryKey() || value.IsPrimaryKey();
			if (!declaredProperty.IsPrimaryKey())
			{
				string text = TryUniquify(declaredProperty, columnName, properties, usePrefix, maxLength);
				if (text != null)
				{
					properties[text] = declaredProperty;
					continue;
				}
			}
			if (!value.IsPrimaryKey())
			{
				string text2 = TryUniquify(value, columnName, properties, usePrefix, maxLength);
				if (text2 != null)
				{
					properties[columnName] = declaredProperty;
					properties[text2] = value;
				}
			}
		}
	}

	private static string TryUniquify(IConventionProperty property, string columnName, Dictionary<string, IConventionProperty> properties, bool usePrefix, int maxLength)
	{
		if (property.Builder.CanSetColumnName(null))
		{
			if (usePrefix)
			{
				string text = property.DeclaringEntityType.ShortName();
				if (!columnName.StartsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					columnName = text + "_" + columnName;
				}
			}
			columnName = Uniquifier.Uniquify(columnName, properties, maxLength);
			property.Builder.HasColumnName(columnName);
			properties[columnName] = property;
			return columnName;
		}
		return null;
	}

	private static void TryUniquifyKeyNames(IConventionEntityType entityType, Dictionary<string, IConventionKey> keys, int maxLength)
	{
		foreach (IConventionKey declaredKey in entityType.GetDeclaredKeys())
		{
			string name = declaredKey.GetName();
			if (!keys.TryGetValue(name, out var value))
			{
				keys[name] = declaredKey;
				continue;
			}
			if (!declaredKey.IsPrimaryKey())
			{
				string text = TryUniquify(declaredKey, name, keys, maxLength);
				if (text != null)
				{
					keys[text] = declaredKey;
					continue;
				}
			}
			if (!value.IsPrimaryKey())
			{
				string text2 = TryUniquify(value, name, keys, maxLength);
				if (text2 != null)
				{
					keys[name] = declaredKey;
					keys[text2] = value;
				}
			}
		}
	}

	private static string TryUniquify<T>(IConventionKey key, string keyName, Dictionary<string, T> keys, int maxLength)
	{
		if (key.Builder.CanSetName(null))
		{
			keyName = Uniquifier.Uniquify(keyName, keys, maxLength);
			key.Builder.HasName(keyName);
			return keyName;
		}
		return null;
	}

	private static void TryUniquifyIndexNames(IConventionEntityType entityType, Dictionary<string, IConventionIndex> indexes, int maxLength)
	{
		foreach (IConventionIndex declaredIndex in entityType.GetDeclaredIndexes())
		{
			string name = declaredIndex.GetName();
			if (!indexes.TryGetValue(name, out var value))
			{
				indexes[name] = declaredIndex;
			}
			else if (declaredIndex.Builder.CanSetName(null))
			{
				if (declaredIndex.GetConfigurationSource() == ConfigurationSource.Convention && value.GetConfigurationSource() == ConfigurationSource.Convention && value.Builder.CanSetName(null))
				{
					IConventionForeignKey conventionForeignKey = declaredIndex.DeclaringEntityType.FindDeclaredForeignKeys(declaredIndex.Properties).FirstOrDefault();
					IConventionForeignKey conventionForeignKey2 = value.DeclaringEntityType.FindDeclaredForeignKeys(declaredIndex.Properties).FirstOrDefault();
					if (conventionForeignKey != null && conventionForeignKey2 != null && conventionForeignKey.GetConstraintName() == conventionForeignKey2.GetConstraintName() && declaredIndex.AreCompatible(value, shouldThrow: false))
					{
						continue;
					}
				}
				string key = TryUniquify(declaredIndex, name, indexes, maxLength);
				indexes[key] = declaredIndex;
			}
			else
			{
				string text = TryUniquify(value, name, indexes, maxLength);
				if (text != null)
				{
					indexes[name] = declaredIndex;
					indexes[text] = value;
				}
			}
		}
	}

	private static string TryUniquify<T>(IConventionIndex index, string indexName, Dictionary<string, T> indexes, int maxLength)
	{
		if (index.Builder.CanSetName(null))
		{
			indexName = Uniquifier.Uniquify(indexName, indexes, maxLength);
			index.Builder.HasName(indexName);
			return indexName;
		}
		return null;
	}

	private static void TryUniquifyForeignKeyNames(IConventionEntityType entityType, Dictionary<string, IConventionForeignKey> foreignKeys, int maxLength)
	{
		foreach (IConventionForeignKey declaredForeignKey in entityType.GetDeclaredForeignKeys())
		{
			if (declaredForeignKey.DeclaringEntityType.GetTableName() == declaredForeignKey.PrincipalEntityType.GetTableName() && declaredForeignKey.DeclaringEntityType.GetSchema() == declaredForeignKey.PrincipalEntityType.GetSchema())
			{
				continue;
			}
			string constraintName = declaredForeignKey.GetConstraintName();
			if (!foreignKeys.TryGetValue(constraintName, out var value))
			{
				foreignKeys[constraintName] = declaredForeignKey;
				continue;
			}
			if (declaredForeignKey.Builder.CanSetConstraintName(null))
			{
				if (!value.Builder.CanSetConstraintName(null) || (declaredForeignKey.PrincipalToDependent == null && declaredForeignKey.DependentToPrincipal == null) || !(declaredForeignKey.PrincipalToDependent?.GetIdentifyingMemberInfo()).IsSameAs(value.PrincipalToDependent?.GetIdentifyingMemberInfo()) || !(declaredForeignKey.DependentToPrincipal?.GetIdentifyingMemberInfo()).IsSameAs(value.DependentToPrincipal?.GetIdentifyingMemberInfo()) || !declaredForeignKey.AreCompatible(value, shouldThrow: false))
				{
					string key = TryUniquify(declaredForeignKey, constraintName, foreignKeys, maxLength);
					foreignKeys[key] = declaredForeignKey;
				}
				continue;
			}
			string text = TryUniquify(value, constraintName, foreignKeys, maxLength);
			if (text != null)
			{
				foreignKeys[constraintName] = declaredForeignKey;
				foreignKeys[text] = value;
			}
		}
	}

	private static string TryUniquify<T>(IConventionForeignKey foreignKey, string foreignKeyName, Dictionary<string, T> foreignKeys, int maxLength)
	{
		if (foreignKey.Builder.CanSetConstraintName(null))
		{
			foreignKeyName = Uniquifier.Uniquify(foreignKeyName, foreignKeys, maxLength);
			foreignKey.Builder.HasConstraintName(foreignKeyName);
			return foreignKeyName;
		}
		return null;
	}
}
