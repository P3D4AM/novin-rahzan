using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public abstract class MigrationsCodeGenerator : IMigrationsCodeGenerator, ILanguageBasedService
{
	public abstract string FileExtension { get; }

	public virtual string Language => null;

	protected virtual MigrationsCodeGeneratorDependencies Dependencies { get; }

	protected MigrationsCodeGenerator([JetBrains.Annotations.NotNull] MigrationsCodeGeneratorDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public abstract string GenerateMigration(string migrationNamespace, string migrationName, IReadOnlyList<MigrationOperation> upOperations, IReadOnlyList<MigrationOperation> downOperations);

	public abstract string GenerateMetadata(string migrationNamespace, Type contextType, string migrationName, string migrationId, IModel targetModel);

	public abstract string GenerateSnapshot(string modelSnapshotNamespace, Type contextType, string modelSnapshotName, IModel model);

	protected virtual IEnumerable<string> GetNamespaces([JetBrains.Annotations.NotNull] IEnumerable<MigrationOperation> operations)
	{
		return operations.OfType<ColumnOperation>().SelectMany(GetColumnNamespaces).Concat(operations.OfType<CreateTableOperation>().SelectMany((CreateTableOperation o) => o.Columns).SelectMany(GetColumnNamespaces))
			.Concat((from o in operations.OfType<InsertDataOperation>()
				select o.Values).Concat(operations.OfType<UpdateDataOperation>().SelectMany((UpdateDataOperation o) => new object[2][,] { o.KeyValues, o.Values })).Concat(from o in operations.OfType<DeleteDataOperation>()
				select o.KeyValues).SelectMany(GetDataNamespaces))
			.Concat(GetAnnotationNamespaces(GetAnnotatables(operations)));
	}

	private static IEnumerable<string> GetColumnNamespaces(ColumnOperation columnOperation)
	{
		foreach (string @namespace in columnOperation.ClrType.GetNamespaces())
		{
			yield return @namespace;
		}
		AlterColumnOperation alterColumnOperation = columnOperation as AlterColumnOperation;
		if (alterColumnOperation?.OldColumn == null)
		{
			yield break;
		}
		foreach (string namespace2 in alterColumnOperation.OldColumn.ClrType.GetNamespaces())
		{
			yield return namespace2;
		}
	}

	private static IEnumerable<string> GetDataNamespaces(object[,] values)
	{
		for (int row = 0; row < values.GetLength(0); row++)
		{
			for (int column = 0; column < values.GetLength(1); column++)
			{
				object obj = values[row, column];
				if (obj == null)
				{
					continue;
				}
				foreach (string @namespace in obj.GetType().GetNamespaces())
				{
					yield return @namespace;
				}
			}
		}
	}

	private static IEnumerable<IAnnotatable> GetAnnotatables(IEnumerable<MigrationOperation> operations)
	{
		foreach (MigrationOperation operation in operations)
		{
			yield return operation;
			if (!(operation is CreateTableOperation createTableOperation))
			{
				continue;
			}
			foreach (AddColumnOperation column in createTableOperation.Columns)
			{
				yield return column;
			}
			if (createTableOperation.PrimaryKey != null)
			{
				yield return createTableOperation.PrimaryKey;
			}
			foreach (AddUniqueConstraintOperation uniqueConstraint in createTableOperation.UniqueConstraints)
			{
				yield return uniqueConstraint;
			}
			foreach (AddForeignKeyOperation foreignKey in createTableOperation.ForeignKeys)
			{
				yield return foreignKey;
			}
		}
	}

	protected virtual IEnumerable<string> GetNamespaces([JetBrains.Annotations.NotNull] IModel model)
	{
		return model.GetEntityTypes().SelectMany((IEntityType e) => e.GetDeclaredProperties().SelectMany((IProperty p) => (FindValueConverter(p)?.ProviderClrType ?? p.ClrType).GetNamespaces())).Concat(GetAnnotationNamespaces(GetAnnotatables(model)));
	}

	private static IEnumerable<IAnnotatable> GetAnnotatables(IModel model)
	{
		yield return model;
		foreach (IEntityType entityType in model.GetEntityTypes())
		{
			yield return entityType;
			foreach (IProperty declaredProperty in entityType.GetDeclaredProperties())
			{
				yield return declaredProperty;
			}
			foreach (IKey declaredKey in entityType.GetDeclaredKeys())
			{
				yield return declaredKey;
			}
			foreach (IForeignKey declaredForeignKey in entityType.GetDeclaredForeignKeys())
			{
				yield return declaredForeignKey;
			}
			foreach (IIndex declaredIndex in entityType.GetDeclaredIndexes())
			{
				yield return declaredIndex;
			}
		}
	}

	private IEnumerable<string> GetAnnotationNamespaces(IEnumerable<IAnnotatable> items)
	{
		List<string> ignoredAnnotations = new List<string>
		{
			"RelationshipDiscoveryConvention:NavigationCandidates", "RelationshipDiscoveryConvention:AmbiguousNavigations", "InversePropertyAttributeConvention:InverseNavigations", "ChangeDetector.SkipDetectChanges", "OwnedTypes", "ChangeTrackingStrategy", "BeforeSaveBehavior", "AfterSaveBehavior", "TypeMapping", "ValueComparer",
			"KeyValueComparer", "StructuralValueComparer", "ConstructorBinding", "NavigationAccessMode", "PropertyAccessMode", "ProviderClrType", "ValueConverter", "ValueGeneratorFactory", "DefiningQuery", "QueryFilter",
			"Relational:CheckConstraints"
		};
		List<string> ignoredAnnotationTypes = new List<string> { "Relational:DbFunction", "Relational:Sequence:" };
		return items.SelectMany((IAnnotatable i) => (from a in i.GetAnnotations()
			select new
			{
				Annotatable = i,
				Annotation = a
			} into a
			where a.Annotation.Value != null && !ignoredAnnotations.Contains(a.Annotation.Name) && !ignoredAnnotationTypes.Any((string p) => a.Annotation.Name.StartsWith(p, StringComparison.Ordinal))
			select a).SelectMany(a => GetProviderType(a.Annotatable, a.Annotation.Value.GetType()).GetNamespaces()));
	}

	private ValueConverter FindValueConverter(IProperty property)
	{
		return (property.FindTypeMapping() ?? Dependencies.RelationalTypeMappingSource.FindMapping(property))?.Converter;
	}

	private Type GetProviderType(IAnnotatable annotatable, Type valueType)
	{
		if (!(annotatable is IProperty property) || !(valueType.UnwrapNullableType() == property.ClrType.UnwrapNullableType()))
		{
			return valueType;
		}
		return FindValueConverter(property)?.ProviderClrType ?? valueType;
	}
}
