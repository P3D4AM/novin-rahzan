using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class RelationalModelValidator : ModelValidator
{
	protected virtual RelationalModelValidatorDependencies RelationalDependencies { get; }

	public RelationalModelValidator([JetBrains.Annotations.NotNull] ModelValidatorDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalModelValidatorDependencies relationalDependencies)
		: base(dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalDependencies, "relationalDependencies");
		RelationalDependencies = relationalDependencies;
	}

	public override void Validate(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		base.Validate(model, logger);
		ValidateSharedTableCompatibility(model, logger);
		ValidateInheritanceMapping(model, logger);
		ValidateDefaultValuesOnKeys(model, logger);
		ValidateBoolsWithDefaults(model, logger);
		ValidateDbFunctions(model, logger);
	}

	protected virtual void ValidateDbFunctions([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		foreach (IDbFunction dbFunction in model.GetDbFunctions())
		{
			MethodInfo methodInfo = dbFunction.MethodInfo;
			if (string.IsNullOrEmpty(dbFunction.Name))
			{
				throw new InvalidOperationException(RelationalStrings.DbFunctionNameEmpty(methodInfo.DisplayName()));
			}
			if (dbFunction.TypeMapping == null)
			{
				throw new InvalidOperationException(RelationalStrings.DbFunctionInvalidReturnType(methodInfo.DisplayName(), methodInfo.ReturnType.ShortDisplayName()));
			}
			foreach (IDbFunctionParameter parameter in dbFunction.Parameters)
			{
				if (parameter.TypeMapping == null)
				{
					throw new InvalidOperationException(RelationalStrings.DbFunctionInvalidParameterType(parameter.Name, methodInfo.DisplayName(), parameter.ClrType.ShortDisplayName()));
				}
			}
		}
	}

	protected virtual void ValidateBoolsWithDefaults([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		foreach (IProperty item in model.GetEntityTypes().SelectMany((IEntityType e) => e.GetDeclaredProperties()))
		{
			if (item.ClrType == typeof(bool) && item.ValueGenerated != 0 && (IsNotNullAndFalse(item.GetDefaultValue()) || item.GetDefaultValueSql() != null))
			{
				logger.BoolWithDefaultWarning(item);
			}
		}
	}

	private static bool IsNotNullAndFalse(object value)
	{
		if (value != null)
		{
			bool flag = default(bool);
			int num;
			if (value is bool)
			{
				flag = (bool)value;
				num = ((1 == 0) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			return (byte)((uint)num | (flag ? 1u : 0u)) != 0;
		}
		return false;
	}

	protected virtual void ValidateDefaultValuesOnKeys([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		foreach (IProperty item in from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredKeys().SelectMany((IKey k) => k.Properties))
			where p.GetDefaultValue() != null
			select p)
		{
			logger.ModelValidationKeyDefaultValueWarning(item);
		}
	}

	protected virtual void ValidateSharedTableCompatibility([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Dictionary<string, List<IEntityType>> dictionary = new Dictionary<string, List<IEntityType>>();
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			string key = Format(entityType.GetSchema(), entityType.GetTableName());
			if (!dictionary.TryGetValue(key, out var value))
			{
				value = (dictionary[key] = new List<IEntityType>());
			}
			value.Add(entityType);
		}
		foreach (KeyValuePair<string, List<IEntityType>> item in dictionary)
		{
			List<IEntityType> value2 = item.Value;
			string key2 = item.Key;
			ValidateSharedTableCompatibility(value2, key2, logger);
			ValidateSharedColumnsCompatibility(value2, key2, logger);
			ValidateSharedKeysCompatibility(value2, key2, logger);
			ValidateSharedForeignKeysCompatibility(value2, key2, logger);
			ValidateSharedIndexesCompatibility(value2, key2, logger);
		}
	}

	protected virtual void ValidateSharedTableCompatibility([JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> mappedTypes, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		if (mappedTypes.Count == 1)
		{
			return;
		}
		HashSet<IEntityType> unvalidatedTypes = new HashSet<IEntityType>(mappedTypes);
		IEntityType entityType2 = null;
		foreach (IEntityType mappedType in mappedTypes)
		{
			if (mappedType.BaseType == null && (mappedType.FindPrimaryKey() == null || !mappedType.FindForeignKeys(mappedType.FindPrimaryKey().Properties).Any((IForeignKey fk) => fk.PrincipalKey.IsPrimaryKey() && fk.PrincipalEntityType.GetRootType() != mappedType && unvalidatedTypes.Contains(fk.PrincipalEntityType))))
			{
				if (entityType2 != null)
				{
					throw new InvalidOperationException(RelationalStrings.IncompatibleTableNoRelationship(tableName, mappedType.DisplayName(), entityType2.DisplayName()));
				}
				entityType2 = mappedType;
			}
		}
		unvalidatedTypes.Remove(entityType2);
		Queue<IEntityType> queue = new Queue<IEntityType>();
		queue.Enqueue(entityType2);
		while (queue.Count > 0)
		{
			IEntityType entityType = queue.Dequeue();
			string text = entityType.GetComment();
			int count = queue.Count;
			foreach (IEntityType item in unvalidatedTypes.Where((IEntityType unvalidatedType) => entityType.IsAssignableFrom(unvalidatedType) || IsIdentifyingPrincipal(unvalidatedType, entityType)))
			{
				IKey key = entityType.FindPrimaryKey();
				IKey key2 = item.FindPrimaryKey();
				if (key?.GetName() != key2?.GetName())
				{
					throw new InvalidOperationException(RelationalStrings.IncompatibleTableKeyNameMismatch(tableName, entityType.DisplayName(), item.DisplayName(), key?.GetName(), key?.Properties.Format(), key2?.GetName(), key2?.Properties.Format()));
				}
				string comment = item.GetComment();
				if (text != null)
				{
					if (comment != null && !text.Equals(comment, StringComparison.Ordinal))
					{
						throw new InvalidOperationException(RelationalStrings.IncompatibleTableCommentMismatch(tableName, entityType.DisplayName(), item.DisplayName(), text, comment));
					}
				}
				else
				{
					text = comment;
				}
				queue.Enqueue(item);
			}
			foreach (IEntityType item2 in queue.Skip(count))
			{
				unvalidatedTypes.Remove(item2);
			}
		}
		if (unvalidatedTypes.Count == 0)
		{
			return;
		}
		using HashSet<IEntityType>.Enumerator enumerator2 = unvalidatedTypes.GetEnumerator();
		if (enumerator2.MoveNext())
		{
			IEntityType current3 = enumerator2.Current;
			throw new InvalidOperationException(RelationalStrings.IncompatibleTableNoRelationship(tableName, current3.DisplayName(), entityType2.DisplayName()));
		}
	}

	private static bool IsIdentifyingPrincipal(IEntityType dependentEntityType, IEntityType principalEntityType)
	{
		return dependentEntityType.FindForeignKeys(dependentEntityType.FindPrimaryKey().Properties).Any((IForeignKey fk) => fk.PrincipalKey.IsPrimaryKey() && fk.PrincipalEntityType == principalEntityType);
	}

	protected virtual void ValidateSharedColumnsCompatibility([JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> mappedTypes, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Dictionary<string, IProperty> dictionary = null;
		if (mappedTypes.Count > 1)
		{
			foreach (IProperty item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredProperties()))
			{
				if (item.IsConcurrencyToken && (item.ValueGenerated & ValueGenerated.OnUpdate) != 0)
				{
					if (dictionary == null)
					{
						dictionary = new Dictionary<string, IProperty>();
					}
					dictionary[item.GetColumnName()] = item;
				}
			}
		}
		Dictionary<string, IProperty> dictionary2 = new Dictionary<string, IProperty>();
		foreach (IEntityType mappedType in mappedTypes)
		{
			HashSet<string> hashSet = null;
			if (dictionary != null && dictionary.Count != 0)
			{
				hashSet = new HashSet<string>();
				foreach (KeyValuePair<string, IProperty> item2 in dictionary)
				{
					IEntityType declaringEntityType = item2.Value.DeclaringEntityType;
					if (!declaringEntityType.IsAssignableFrom(mappedType) && !declaringEntityType.IsInOwnershipPath(mappedType) && !mappedType.IsInOwnershipPath(declaringEntityType))
					{
						hashSet.Add(item2.Key);
					}
				}
			}
			foreach (IProperty declaredProperty in mappedType.GetDeclaredProperties())
			{
				string columnName = declaredProperty.GetColumnName();
				hashSet?.Remove(columnName);
				if (!dictionary2.TryGetValue(columnName, out var value))
				{
					dictionary2[columnName] = declaredProperty;
					continue;
				}
				string text = declaredProperty.GetColumnType() ?? declaredProperty.GetRelationalTypeMapping().StoreType;
				string text2 = value.GetColumnType() ?? value.GetRelationalTypeMapping().StoreType;
				if (!string.Equals(text, text2, StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException(RelationalStrings.DuplicateColumnNameDataTypeMismatch(value.DeclaringEntityType.DisplayName(), value.Name, declaredProperty.DeclaringEntityType.DisplayName(), declaredProperty.Name, columnName, tableName, text2, text));
				}
				if (declaredProperty.IsNullable != value.IsNullable)
				{
					throw new InvalidOperationException(RelationalStrings.DuplicateColumnNameNullabilityMismatch(value.DeclaringEntityType.DisplayName(), value.Name, declaredProperty.DeclaringEntityType.DisplayName(), declaredProperty.Name, columnName, tableName));
				}
				string text3 = declaredProperty.GetComputedColumnSql() ?? "";
				string text4 = value.GetComputedColumnSql() ?? "";
				if (!text3.Equals(text4, StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException(RelationalStrings.DuplicateColumnNameComputedSqlMismatch(value.DeclaringEntityType.DisplayName(), value.Name, declaredProperty.DeclaringEntityType.DisplayName(), declaredProperty.Name, columnName, tableName, text4, text3));
				}
				object defaultValue = declaredProperty.GetDefaultValue();
				object defaultValue2 = value.GetDefaultValue();
				if (!object.Equals(defaultValue, defaultValue2))
				{
					throw new InvalidOperationException(RelationalStrings.DuplicateColumnNameDefaultSqlMismatch(value.DeclaringEntityType.DisplayName(), value.Name, declaredProperty.DeclaringEntityType.DisplayName(), declaredProperty.Name, columnName, tableName, defaultValue2 ?? "NULL", defaultValue ?? "NULL"));
				}
				string text5 = declaredProperty.GetDefaultValueSql() ?? "";
				string text6 = value.GetDefaultValueSql() ?? "";
				if (!text5.Equals(text6, StringComparison.OrdinalIgnoreCase))
				{
					throw new InvalidOperationException(RelationalStrings.DuplicateColumnNameDefaultSqlMismatch(value.DeclaringEntityType.DisplayName(), value.Name, declaredProperty.DeclaringEntityType.DisplayName(), declaredProperty.Name, columnName, tableName, text6, text5));
				}
				string text7 = declaredProperty.GetComment() ?? "";
				string text8 = value.GetComment() ?? "";
				if (text7.Equals(text8, StringComparison.Ordinal))
				{
					continue;
				}
				throw new InvalidOperationException(RelationalStrings.DuplicateColumnNameCommentMismatch(value.DeclaringEntityType.DisplayName(), value.Name, declaredProperty.DeclaringEntityType.DisplayName(), declaredProperty.Name, columnName, tableName, text8, text7));
			}
			if (hashSet == null || hashSet.Count == 0)
			{
				continue;
			}
			foreach (string missingColumn in hashSet)
			{
				if (mappedType.GetAllBaseTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties()).All((IProperty p) => p.GetColumnName() != missingColumn))
				{
					throw new InvalidOperationException(RelationalStrings.MissingConcurrencyColumn(mappedType.DisplayName(), missingColumn, tableName));
				}
			}
		}
	}

	protected virtual void ValidateSharedForeignKeysCompatibility([JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> mappedTypes, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Dictionary<string, IForeignKey> dictionary = new Dictionary<string, IForeignKey>();
		foreach (IForeignKey item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredForeignKeys()))
		{
			string constraintName = item.GetConstraintName();
			if (!dictionary.TryGetValue(constraintName, out var value))
			{
				dictionary[constraintName] = item;
			}
			else
			{
				item.AreCompatible(value, shouldThrow: true);
			}
		}
	}

	protected virtual void ValidateSharedIndexesCompatibility([JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> mappedTypes, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Dictionary<string, IIndex> dictionary = new Dictionary<string, IIndex>();
		foreach (IIndex item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredIndexes()))
		{
			string name = item.GetName();
			if (!dictionary.TryGetValue(name, out var value))
			{
				dictionary[name] = item;
			}
			else
			{
				item.AreCompatible(value, shouldThrow: true);
			}
		}
	}

	protected virtual void ValidateSharedKeysCompatibility([JetBrains.Annotations.NotNull] IReadOnlyList<IEntityType> mappedTypes, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		Dictionary<string, IKey> dictionary = new Dictionary<string, IKey>();
		foreach (IKey item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredKeys()))
		{
			string name = item.GetName();
			if (!dictionary.TryGetValue(name, out var value))
			{
				dictionary[name] = item;
			}
			else if (!item.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(value.Properties.Select((IProperty p) => p.GetColumnName())))
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateKeyColumnMismatch(item.Properties.Format(), item.DeclaringEntityType.DisplayName(), value.Properties.Format(), value.DeclaringEntityType.DisplayName(), tableName, name, item.Properties.FormatColumns(), value.Properties.FormatColumns()));
			}
		}
	}

	protected virtual void ValidateInheritanceMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			if (entityType.BaseType != null && entityType["Relational:TableName"] != null && ((EntityType)entityType).FindAnnotation("Relational:TableName").GetConfigurationSource() == ConfigurationSource.Explicit)
			{
				throw new InvalidOperationException(RelationalStrings.DerivedTypeTable(entityType.DisplayName(), entityType.BaseType.DisplayName()));
			}
		}
	}

	private static string Format(string schema, string name)
	{
		return (string.IsNullOrEmpty(schema) ? "" : (schema + ".")) + name;
	}
}
