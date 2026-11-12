using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.SqlServer.Internal;

public class SqlServerModelValidator : RelationalModelValidator
{
	public SqlServerModelValidator([JetBrains.Annotations.NotNull] ModelValidatorDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalModelValidatorDependencies relationalDependencies)
		: base(dependencies, relationalDependencies)
	{
	}

	public override void Validate(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		base.Validate(model, logger);
		ValidateDefaultDecimalMapping(model, logger);
		ValidateByteIdentityMapping(model, logger);
		ValidateNonKeyValueGeneration(model, logger);
		ValidateIndexIncludeProperties(model, logger);
	}

	protected virtual void ValidateDefaultDecimalMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		foreach (IProperty item in from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties())
			where p.ClrType.UnwrapNullableType() == typeof(decimal) && !p.IsForeignKey()
			select p)
		{
			ConfigurationSource? oldConfigurationSource = (item as IConventionProperty)?.GetColumnTypeConfigurationSource();
			ConfigurationSource? oldConfigurationSource2 = (item as IConventionProperty)?.GetTypeMappingConfigurationSource();
			if ((!oldConfigurationSource.HasValue && ConfigurationSource.Convention.Overrides(oldConfigurationSource2)) || (oldConfigurationSource.HasValue && ConfigurationSource.Convention.Overrides(oldConfigurationSource)))
			{
				logger.DecimalTypeDefaultWarning(item);
			}
		}
	}

	protected virtual void ValidateByteIdentityMapping([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		foreach (IProperty item in from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties())
			where p.ClrType.UnwrapNullableType() == typeof(byte) && p.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn
			select p)
		{
			logger.ByteIdentityColumnWarning(item);
		}
	}

	protected virtual void ValidateNonKeyValueGeneration([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		using IEnumerator<IProperty> enumerator = (from p in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredProperties())
			where p.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.SequenceHiLo && ((IConventionProperty)p).GetValueGenerationStrategyConfigurationSource().HasValue && !p.IsKey() && p.ValueGenerated != 0 && (!(p.FindAnnotation("SqlServer:ValueGenerationStrategy") is ConventionAnnotation conventionAnnotation) || !ConfigurationSource.Convention.Overrides(conventionAnnotation.GetConfigurationSource()))
			select p).GetEnumerator();
		if (enumerator.MoveNext())
		{
			IProperty current = enumerator.Current;
			throw new InvalidOperationException(SqlServerStrings.NonKeyValueGeneration(current.Name, current.DeclaringEntityType.DisplayName()));
		}
	}

	protected virtual void ValidateIndexIncludeProperties([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		foreach (IIndex index in model.GetEntityTypes().SelectMany((IEntityType t) => t.GetDeclaredIndexes()))
		{
			IReadOnlyList<string> includeProperties = index.GetIncludeProperties();
			if (includeProperties == null || includeProperties.Count <= 0)
			{
				continue;
			}
			string text = includeProperties.FirstOrDefault((string i) => index.DeclaringEntityType.FindProperty(i) == null);
			if (text != null)
			{
				throw new InvalidOperationException(SqlServerStrings.IncludePropertyNotFound(index.DeclaringEntityType.DisplayName(), text));
			}
			string text2 = (from i in includeProperties
				group i by i into g
				where g.Count() > 1
				select g into y
				select y.Key).FirstOrDefault();
			if (text2 != null)
			{
				throw new InvalidOperationException(SqlServerStrings.IncludePropertyDuplicated(index.DeclaringEntityType.DisplayName(), text2));
			}
			string text3 = includeProperties.FirstOrDefault((string i) => index.Properties.Any((IProperty p) => i == p.Name));
			if (text3 != null)
			{
				throw new InvalidOperationException(SqlServerStrings.IncludePropertyInIndex(index.DeclaringEntityType.DisplayName(), text3));
			}
		}
	}

	protected override void ValidateSharedTableCompatibility(IReadOnlyList<IEntityType> mappedTypes, string tableName, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		IEntityType entityType = mappedTypes[0];
		bool flag = entityType.IsMemoryOptimized();
		foreach (IEntityType item in mappedTypes.Skip(1))
		{
			if (flag != item.IsMemoryOptimized())
			{
				throw new InvalidOperationException(SqlServerStrings.IncompatibleTableMemoryOptimizedMismatch(tableName, entityType.DisplayName(), item.DisplayName(), flag ? entityType.DisplayName() : item.DisplayName(), (!flag) ? entityType.DisplayName() : item.DisplayName()));
			}
		}
		base.ValidateSharedTableCompatibility(mappedTypes, tableName, logger);
	}

	protected override void ValidateSharedColumnsCompatibility(IReadOnlyList<IEntityType> mappedTypes, string tableName, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		base.ValidateSharedColumnsCompatibility(mappedTypes, tableName, logger);
		List<IProperty> list = new List<IProperty>();
		Dictionary<string, IProperty> dictionary = new Dictionary<string, IProperty>();
		foreach (IProperty item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredProperties()))
		{
			string columnName = item.GetColumnName();
			if (dictionary.TryGetValue(columnName, out var value))
			{
				SqlServerValueGenerationStrategy valueGenerationStrategy = item.GetValueGenerationStrategy();
				SqlServerValueGenerationStrategy valueGenerationStrategy2 = value.GetValueGenerationStrategy();
				if (valueGenerationStrategy != valueGenerationStrategy2 && (valueGenerationStrategy == SqlServerValueGenerationStrategy.IdentityColumn || valueGenerationStrategy2 == SqlServerValueGenerationStrategy.IdentityColumn))
				{
					throw new InvalidOperationException(SqlServerStrings.DuplicateColumnNameValueGenerationStrategyMismatch(value.DeclaringEntityType.DisplayName(), value.Name, item.DeclaringEntityType.DisplayName(), item.Name, columnName, tableName));
				}
			}
			else
			{
				dictionary[columnName] = item;
				if (item.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn)
				{
					list.Add(item);
				}
			}
		}
		if (list.Count > 1)
		{
			throw new InvalidOperationException(SqlServerStrings.MultipleIdentityColumns(new StringBuilder().AppendJoin(list.Select((IProperty p) => "'" + p.DeclaringEntityType.DisplayName() + "." + p.Name + "'")), tableName));
		}
	}

	protected override void ValidateSharedKeysCompatibility(IReadOnlyList<IEntityType> mappedTypes, string tableName, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
	{
		base.ValidateSharedKeysCompatibility(mappedTypes, tableName, logger);
		Dictionary<string, IKey> dictionary = new Dictionary<string, IKey>();
		foreach (IKey item in mappedTypes.SelectMany((IEntityType et) => et.GetDeclaredKeys()))
		{
			string name = item.GetName();
			if (!dictionary.TryGetValue(name, out var value))
			{
				dictionary[name] = item;
			}
			else if (item.IsClustered() != value.IsClustered())
			{
				throw new InvalidOperationException(SqlServerStrings.DuplicateKeyMismatchedClustering(item.Properties.Format(), item.DeclaringEntityType.DisplayName(), value.Properties.Format(), value.DeclaringEntityType.DisplayName(), tableName, name));
			}
		}
	}
}
