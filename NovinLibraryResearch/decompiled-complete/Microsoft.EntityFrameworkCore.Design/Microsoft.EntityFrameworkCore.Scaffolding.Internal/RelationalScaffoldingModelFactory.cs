using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class RelationalScaffoldingModelFactory : IScaffoldingModelFactory
{
	internal const string NavigationNameUniquifyingPattern = "{0}Navigation";

	internal const string SelfReferencingPrincipalEndNavigationNamePattern = "Inverse{0}";

	private readonly IOperationReporter _reporter;

	private readonly ICandidateNamingService _candidateNamingService;

	private Dictionary<DatabaseTable, CSharpUniqueNamer<DatabaseColumn>> _columnNamers;

	private bool _useDatabaseNames;

	private readonly DatabaseTable _nullTable = new DatabaseTable();

	private CSharpUniqueNamer<DatabaseTable> _tableNamer;

	private CSharpUniqueNamer<DatabaseTable> _dbSetNamer;

	private readonly HashSet<DatabaseColumn> _unmappedColumns = new HashSet<DatabaseColumn>();

	private readonly IPluralizer _pluralizer;

	private readonly ICSharpUtilities _cSharpUtilities;

	private readonly IScaffoldingTypeMapper _scaffoldingTypeMapper;

	private readonly LoggingDefinitions _loggingDefinitions;

	private readonly Dictionary<IEntityType, List<string>> _entityTypeAndPropertyIdentifiers = new Dictionary<IEntityType, List<string>>();

	public RelationalScaffoldingModelFactory([JetBrains.Annotations.NotNull] IOperationReporter reporter, [JetBrains.Annotations.NotNull] ICandidateNamingService candidateNamingService, [JetBrains.Annotations.NotNull] IPluralizer pluralizer, [JetBrains.Annotations.NotNull] ICSharpUtilities cSharpUtilities, [JetBrains.Annotations.NotNull] IScaffoldingTypeMapper scaffoldingTypeMapper, [JetBrains.Annotations.NotNull] LoggingDefinitions loggingDefinitions)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reporter, "reporter");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(candidateNamingService, "candidateNamingService");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(pluralizer, "pluralizer");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cSharpUtilities, "cSharpUtilities");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(scaffoldingTypeMapper, "scaffoldingTypeMapper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(loggingDefinitions, "loggingDefinitions");
		_reporter = reporter;
		_candidateNamingService = candidateNamingService;
		_pluralizer = pluralizer;
		_cSharpUtilities = cSharpUtilities;
		_scaffoldingTypeMapper = scaffoldingTypeMapper;
		_loggingDefinitions = loggingDefinitions;
	}

	public virtual IModel Create(DatabaseModel databaseModel, bool useDatabaseNames)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseModel, "databaseModel");
		ModelBuilder modelBuilder = new ModelBuilder(new ConventionSet());
		_tableNamer = new CSharpUniqueNamer<DatabaseTable>(useDatabaseNames ? ((Func<DatabaseTable, string>)((DatabaseTable t) => t.Name)) : ((Func<DatabaseTable, string>)((DatabaseTable t) => _candidateNamingService.GenerateCandidateIdentifier(t))), _cSharpUtilities, useDatabaseNames ? null : new Func<string, string>(_pluralizer.Singularize));
		_dbSetNamer = new CSharpUniqueNamer<DatabaseTable>(useDatabaseNames ? ((Func<DatabaseTable, string>)((DatabaseTable t) => t.Name)) : ((Func<DatabaseTable, string>)((DatabaseTable t) => _candidateNamingService.GenerateCandidateIdentifier(t))), _cSharpUtilities, useDatabaseNames ? null : new Func<string, string>(_pluralizer.Pluralize));
		_columnNamers = new Dictionary<DatabaseTable, CSharpUniqueNamer<DatabaseColumn>>();
		_useDatabaseNames = useDatabaseNames;
		VisitDatabaseModel(modelBuilder, databaseModel);
		return modelBuilder.Model;
	}

	protected virtual string GetEntityTypeName([JetBrains.Annotations.NotNull] DatabaseTable table)
	{
		return _tableNamer.GetName(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(table, "table"));
	}

	protected virtual string GetDbSetName([JetBrains.Annotations.NotNull] DatabaseTable table)
	{
		return _dbSetNamer.GetName(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(table, "table"));
	}

	protected virtual string GetPropertyName([JetBrains.Annotations.NotNull] DatabaseColumn column)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(column, "column");
		DatabaseTable databaseTable = column.Table ?? _nullTable;
		List<string> list = new List<string>();
		if (column.Table != null)
		{
			list.Add(GetEntityTypeName(databaseTable));
		}
		if (!_columnNamers.ContainsKey(databaseTable))
		{
			if (_useDatabaseNames)
			{
				_columnNamers.Add(databaseTable, new CSharpUniqueNamer<DatabaseColumn>((DatabaseColumn c) => c.Name, list, _cSharpUtilities, null));
			}
			else
			{
				_columnNamers.Add(databaseTable, new CSharpUniqueNamer<DatabaseColumn>((DatabaseColumn c) => _candidateNamingService.GenerateCandidateIdentifier(c), list, _cSharpUtilities, null));
			}
		}
		return _columnNamers[databaseTable].GetName(column);
	}

	protected virtual ModelBuilder VisitDatabaseModel([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] DatabaseModel databaseModel)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseModel, "databaseModel");
		if (!string.IsNullOrEmpty(databaseModel.DefaultSchema))
		{
			modelBuilder.HasDefaultSchema(databaseModel.DefaultSchema);
		}
		if (!string.IsNullOrEmpty(databaseModel.DatabaseName))
		{
			modelBuilder.Model.SetDatabaseName(databaseModel.DatabaseName);
		}
		VisitSequences(modelBuilder, databaseModel.Sequences);
		VisitTables(modelBuilder, databaseModel.Tables);
		VisitForeignKeys(modelBuilder, databaseModel.Tables.SelectMany((DatabaseTable table) => table.ForeignKeys).ToList());
		modelBuilder.Model.AddAnnotations(databaseModel.GetAnnotations());
		return modelBuilder;
	}

	protected virtual ModelBuilder VisitSequences([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] ICollection<DatabaseSequence> sequences)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sequences, "sequences");
		foreach (DatabaseSequence sequence in sequences)
		{
			VisitSequence(modelBuilder, sequence);
		}
		return modelBuilder;
	}

	protected virtual SequenceBuilder VisitSequence([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] DatabaseSequence sequence)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sequence, "sequence");
		if (string.IsNullOrEmpty(sequence.Name))
		{
			_reporter.WriteWarning(DesignStrings.SequencesRequireName);
			return null;
		}
		Type type = null;
		if (sequence.StoreType != null)
		{
			type = _scaffoldingTypeMapper.FindMapping(sequence.StoreType, keyOrIndex: false, rowVersion: false)?.ClrType;
		}
		if (type != null && !Sequence.SupportedTypes.Contains(type))
		{
			_reporter.WriteWarning(DesignStrings.BadSequenceType(sequence.Name, sequence.StoreType));
			return null;
		}
		SequenceBuilder sequenceBuilder = ((type != null) ? modelBuilder.HasSequence(type, sequence.Name, sequence.Schema) : modelBuilder.HasSequence(sequence.Name, sequence.Schema));
		if (sequence.IncrementBy.HasValue)
		{
			sequenceBuilder.IncrementsBy(sequence.IncrementBy.Value);
		}
		if (sequence.MaxValue.HasValue)
		{
			sequenceBuilder.HasMax(sequence.MaxValue.Value);
		}
		if (sequence.MinValue.HasValue)
		{
			sequenceBuilder.HasMin(sequence.MinValue.Value);
		}
		if (sequence.StartValue.HasValue)
		{
			sequenceBuilder.StartsAt(sequence.StartValue.Value);
		}
		if (sequence.IsCyclic.HasValue)
		{
			sequenceBuilder.IsCyclic(sequence.IsCyclic.Value);
		}
		return sequenceBuilder;
	}

	protected virtual ModelBuilder VisitTables([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] ICollection<DatabaseTable> tables)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(tables, "tables");
		foreach (DatabaseTable table in tables)
		{
			VisitTable(modelBuilder, table);
		}
		return modelBuilder;
	}

	protected virtual EntityTypeBuilder VisitTable([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] DatabaseTable table)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(table, "table");
		string entityTypeName = GetEntityTypeName(table);
		EntityTypeBuilder entityTypeBuilder = modelBuilder.Entity(entityTypeName);
		string dbSetName = GetDbSetName(table);
		entityTypeBuilder.Metadata.SetDbSetName(dbSetName);
		if (table is DatabaseView)
		{
			entityTypeBuilder.ToView(table.Name, table.Schema);
		}
		else
		{
			entityTypeBuilder.ToTable(table.Name, table.Schema);
		}
		if (table.Comment != null)
		{
			entityTypeBuilder.HasComment(table.Comment);
		}
		VisitColumns(entityTypeBuilder, table.Columns);
		if (table.PrimaryKey != null && VisitPrimaryKey(entityTypeBuilder, table) == null)
		{
			string text = DesignStrings.UnableToGenerateEntityType(table.DisplayName());
			_reporter.WriteWarning(text);
			IMutableModel model = modelBuilder.Model;
			model.RemoveEntityType(entityTypeName);
			model.GetEntityTypeErrors().Add(entityTypeName, text);
			return null;
		}
		VisitUniqueConstraints(entityTypeBuilder, table.UniqueConstraints);
		VisitIndexes(entityTypeBuilder, table.Indexes);
		entityTypeBuilder.Metadata.AddAnnotations(table.GetAnnotations());
		return entityTypeBuilder;
	}

	protected virtual EntityTypeBuilder VisitColumns([JetBrains.Annotations.NotNull] EntityTypeBuilder builder, [JetBrains.Annotations.NotNull] ICollection<DatabaseColumn> columns)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(columns, "columns");
		foreach (DatabaseColumn column in columns)
		{
			VisitColumn(builder, column);
		}
		return builder;
	}

	protected virtual PropertyBuilder VisitColumn([JetBrains.Annotations.NotNull] EntityTypeBuilder builder, [JetBrains.Annotations.NotNull] DatabaseColumn column)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(column, "column");
		TypeScaffoldingInfo typeScaffoldingInfo = GetTypeScaffoldingInfo(column);
		if (typeScaffoldingInfo == null)
		{
			_unmappedColumns.Add(column);
			_reporter.WriteWarning(DesignStrings.CannotFindTypeMappingForColumn(column.DisplayName(), column.StoreType));
			return null;
		}
		Type type = typeScaffoldingInfo.ClrType;
		if (column.IsNullable)
		{
			type = type.MakeNullable();
		}
		if (type == typeof(bool) && column.DefaultValueSql != null)
		{
			_reporter.WriteWarning(DesignStrings.NonNullableBoooleanColumnHasDefaultConstraint(column.DisplayName()));
			type = type.MakeNullable();
		}
		PropertyBuilder propertyBuilder = builder.Property(type, GetPropertyName(column));
		propertyBuilder.HasColumnName(column.Name);
		if (!typeScaffoldingInfo.IsInferred && !string.IsNullOrWhiteSpace(column.StoreType))
		{
			propertyBuilder.HasColumnType(column.StoreType);
		}
		if (typeScaffoldingInfo.ScaffoldUnicode.HasValue)
		{
			propertyBuilder.IsUnicode(typeScaffoldingInfo.ScaffoldUnicode.Value);
		}
		if (typeScaffoldingInfo.ScaffoldFixedLength == true)
		{
			propertyBuilder.IsFixedLength();
		}
		if (typeScaffoldingInfo.ScaffoldMaxLength.HasValue)
		{
			propertyBuilder.HasMaxLength(typeScaffoldingInfo.ScaffoldMaxLength.Value);
		}
		if (column.ValueGenerated == ValueGenerated.OnAdd)
		{
			propertyBuilder.ValueGeneratedOnAdd();
		}
		if (column.ValueGenerated == ValueGenerated.OnUpdate)
		{
			propertyBuilder.ValueGeneratedOnUpdate();
		}
		if (column.ValueGenerated == ValueGenerated.OnAddOrUpdate)
		{
			propertyBuilder.ValueGeneratedOnAddOrUpdate();
		}
		if (column.DefaultValueSql != null)
		{
			propertyBuilder.HasDefaultValueSql(column.DefaultValueSql);
		}
		if (column.ComputedColumnSql != null)
		{
			propertyBuilder.HasComputedColumnSql(column.ComputedColumnSql);
		}
		if (column.Comment != null)
		{
			propertyBuilder.HasComment(column.Comment);
		}
		DatabasePrimaryKey primaryKey = column.Table.PrimaryKey;
		if (primaryKey == null || !primaryKey.Columns.Contains(column))
		{
			propertyBuilder.IsRequired(!column.IsNullable);
		}
		if ((bool?)column["ConcurrencyToken"] == true)
		{
			propertyBuilder.IsConcurrencyToken();
		}
		propertyBuilder.Metadata.SetColumnOrdinal(column.Table.Columns.IndexOf(column));
		propertyBuilder.Metadata.AddAnnotations(from a in column.GetAnnotations()
			where a.Name != "ConcurrencyToken"
			select a);
		return propertyBuilder;
	}

	protected virtual KeyBuilder VisitPrimaryKey([JetBrains.Annotations.NotNull] EntityTypeBuilder builder, [JetBrains.Annotations.NotNull] DatabaseTable table)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(table, "table");
		DatabasePrimaryKey primaryKey = table.PrimaryKey;
		List<string> list = (from c in primaryKey.Columns
			where _unmappedColumns.Contains(c)
			select c.Name).ToList();
		if (list.Count > 0)
		{
			_reporter.WriteWarning(DesignStrings.PrimaryKeyErrorPropertyNotFound(table.DisplayName(), string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, list)));
			return null;
		}
		KeyBuilder keyBuilder = builder.HasKey(primaryKey.Columns.Select(GetPropertyName).ToArray());
		if (primaryKey.Columns.Count == 1 && !primaryKey.Columns[0].ValueGenerated.HasValue && primaryKey.Columns[0].DefaultValueSql == null)
		{
			Property property = builder.Metadata.FindProperty(GetPropertyName(primaryKey.Columns[0]))?.AsProperty("VisitPrimaryKey");
			if (property != null && RelationalValueGenerationConvention.GetValueGenerated(property) == ValueGenerated.OnAdd)
			{
				property.ValueGenerated = ValueGenerated.Never;
			}
		}
		if (!string.IsNullOrEmpty(primaryKey.Name) && primaryKey.Name != keyBuilder.Metadata.GetDefaultName())
		{
			keyBuilder.HasName(primaryKey.Name);
		}
		keyBuilder.Metadata.AddAnnotations(primaryKey.GetAnnotations());
		return keyBuilder;
	}

	protected virtual EntityTypeBuilder VisitUniqueConstraints([JetBrains.Annotations.NotNull] EntityTypeBuilder builder, [JetBrains.Annotations.NotNull] ICollection<DatabaseUniqueConstraint> uniqueConstraints)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(uniqueConstraints, "uniqueConstraints");
		foreach (DatabaseUniqueConstraint uniqueConstraint in uniqueConstraints)
		{
			VisitUniqueConstraint(builder, uniqueConstraint);
		}
		return builder;
	}

	protected virtual IndexBuilder VisitUniqueConstraint([JetBrains.Annotations.NotNull] EntityTypeBuilder builder, [JetBrains.Annotations.NotNull] DatabaseUniqueConstraint uniqueConstraint)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(uniqueConstraint, "uniqueConstraint");
		List<string> list = (from c in uniqueConstraint.Columns
			where _unmappedColumns.Contains(c)
			select c.Name).ToList();
		if (list.Count > 0)
		{
			_reporter.WriteWarning(DesignStrings.UnableToScaffoldIndexMissingProperty(uniqueConstraint.Name, string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, list)));
			return null;
		}
		string[] propertyNames = uniqueConstraint.Columns.Select(GetPropertyName).ToArray();
		IndexBuilder indexBuilder = builder.HasIndex(propertyNames).IsUnique();
		if (!string.IsNullOrEmpty(uniqueConstraint.Name) && uniqueConstraint.Name != indexBuilder.Metadata.GetDefaultName())
		{
			indexBuilder.HasName(uniqueConstraint.Name);
		}
		indexBuilder.Metadata.AddAnnotations(uniqueConstraint.GetAnnotations());
		return indexBuilder;
	}

	protected virtual EntityTypeBuilder VisitIndexes([JetBrains.Annotations.NotNull] EntityTypeBuilder builder, [JetBrains.Annotations.NotNull] ICollection<DatabaseIndex> indexes)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(indexes, "indexes");
		foreach (DatabaseIndex index in indexes)
		{
			VisitIndex(builder, index);
		}
		return builder;
	}

	protected virtual IndexBuilder VisitIndex([JetBrains.Annotations.NotNull] EntityTypeBuilder builder, [JetBrains.Annotations.NotNull] DatabaseIndex index)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(builder, "builder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(index, "index");
		List<string> list = (from c in index.Columns
			where _unmappedColumns.Contains(c)
			select c.Name).ToList();
		if (list.Count > 0)
		{
			_reporter.WriteWarning(DesignStrings.UnableToScaffoldIndexMissingProperty(index.Name, string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, list)));
			return null;
		}
		string[] propertyNames = index.Columns.Select(GetPropertyName).ToArray();
		IndexBuilder indexBuilder = builder.HasIndex(propertyNames).IsUnique(index.IsUnique);
		if (index.Filter != null)
		{
			indexBuilder.HasFilter(index.Filter);
		}
		if (!string.IsNullOrEmpty(index.Name) && index.Name != indexBuilder.Metadata.GetDefaultName())
		{
			indexBuilder.HasName(index.Name);
		}
		indexBuilder.Metadata.AddAnnotations(index.GetAnnotations());
		return indexBuilder;
	}

	protected virtual ModelBuilder VisitForeignKeys([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] IList<DatabaseForeignKey> foreignKeys)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKeys, "foreignKeys");
		foreach (DatabaseForeignKey foreignKey in foreignKeys)
		{
			VisitForeignKey(modelBuilder, foreignKey);
		}
		foreach (IMutableForeignKey item in modelBuilder.Model.GetEntityTypes().SelectMany((IMutableEntityType et) => et.GetForeignKeys()))
		{
			AddNavigationProperties(item);
		}
		return modelBuilder;
	}

	protected virtual IMutableForeignKey VisitForeignKey([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] DatabaseForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelBuilder, "modelBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		if (foreignKey.PrincipalTable == null)
		{
			_reporter.WriteWarning(DesignStrings.ForeignKeyScaffoldErrorPrincipalTableNotFound(foreignKey.DisplayName()));
			return null;
		}
		if (foreignKey.Table == null)
		{
			return null;
		}
		IMutableEntityType dependentEntityType = modelBuilder.Model.FindEntityType(GetEntityTypeName(foreignKey.Table));
		if (dependentEntityType == null)
		{
			return null;
		}
		List<string> list = (from c in foreignKey.Columns
			where _unmappedColumns.Contains(c)
			select c.Name).ToList();
		if (list.Count > 0)
		{
			_reporter.WriteWarning(DesignStrings.ForeignKeyScaffoldErrorPropertyNotFound(foreignKey.DisplayName(), string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, list)));
			return null;
		}
		ReadOnlyCollection<IMutableProperty> properties = (from name in foreignKey.Columns.Select(GetPropertyName)
			select dependentEntityType.FindProperty(name)).ToList().AsReadOnly();
		IMutableEntityType principalEntityType = modelBuilder.Model.FindEntityType(GetEntityTypeName(foreignKey.PrincipalTable));
		if (principalEntityType == null)
		{
			_reporter.WriteWarning(DesignStrings.ForeignKeyScaffoldErrorPrincipalTableScaffoldingError(foreignKey.DisplayName(), foreignKey.PrincipalTable.DisplayName()));
			return null;
		}
		List<string> list2 = (from pc in foreignKey.PrincipalColumns
			where principalEntityType.FindProperty(GetPropertyName(pc)) == null
			select pc.Name).ToList();
		if (list2.Count > 0)
		{
			_reporter.WriteWarning(DesignStrings.ForeignKeyScaffoldErrorPropertyNotFound(foreignKey.DisplayName(), string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, list2)));
			return null;
		}
		List<(IMutableProperty, DatabaseColumn)> source = foreignKey.PrincipalColumns.Select((DatabaseColumn fc) => (property: principalEntityType.FindProperty(GetPropertyName(fc)), column: fc)).ToList();
		List<IMutableProperty> list3 = source.Select<(IMutableProperty, DatabaseColumn), IMutableProperty>(((IMutableProperty property, DatabaseColumn column) tuple) => tuple.property).ToList();
		IMutableKey mutableKey = principalEntityType.FindKey(list3);
		if (mutableKey == null)
		{
			IMutableIndex mutableIndex = principalEntityType.FindIndex(list3.AsReadOnly());
			if (mutableIndex == null || !mutableIndex.IsUnique)
			{
				List<string> values = foreignKey.PrincipalColumns.Select((DatabaseColumn c) => c.Name).ToList();
				_reporter.WriteWarning(DesignStrings.ForeignKeyScaffoldErrorPrincipalKeyNotFound(foreignKey.DisplayName(), string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, values), principalEntityType.DisplayName()));
				return null;
			}
			List<(IMutableProperty, DatabaseColumn)> list4 = source.Where<(IMutableProperty, DatabaseColumn)>(((IMutableProperty property, DatabaseColumn column) tuple) => tuple.property.IsNullable).ToList();
			if (list4.Count > 0)
			{
				_reporter.WriteWarning(DesignStrings.ForeignKeyPrincipalEndContainsNullableColumns(foreignKey.DisplayName(), mutableIndex.GetName(), list4.Select<(IMutableProperty, DatabaseColumn), string>(((IMutableProperty property, DatabaseColumn column) tuple) => tuple.column.DisplayName()).ToList().Aggregate((string a, string b) => a + "," + b)));
				list4.ToList().ForEach(delegate((IMutableProperty property, DatabaseColumn column) tuple)
				{
					tuple.property.IsNullable = false;
				});
			}
			mutableKey = principalEntityType.AddKey(list3);
		}
		IMutableForeignKey mutableForeignKey = dependentEntityType.AddForeignKey(properties, mutableKey, principalEntityType);
		IMutableKey mutableKey2 = dependentEntityType.FindKey(properties);
		IMutableIndex mutableIndex2 = dependentEntityType.FindIndex(properties);
		mutableForeignKey.IsUnique = mutableKey2 != null || (mutableIndex2?.IsUnique ?? false);
		if (!string.IsNullOrEmpty(foreignKey.Name) && foreignKey.Name != mutableForeignKey.GetDefaultName())
		{
			mutableForeignKey.SetConstraintName(foreignKey.Name);
		}
		AssignOnDeleteAction(foreignKey, mutableForeignKey);
		mutableForeignKey.AddAnnotations(foreignKey.GetAnnotations());
		return mutableForeignKey;
	}

	protected virtual void AddNavigationProperties([JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		List<string> existingIdentifiers = ExistingIdentifiers(foreignKey.DeclaringEntityType);
		string dependentEndCandidateNavigationPropertyName = _candidateNamingService.GetDependentEndCandidateNavigationPropertyName(foreignKey);
		string text = _cSharpUtilities.GenerateCSharpIdentifier(dependentEndCandidateNavigationPropertyName, existingIdentifiers, null, NavigationUniquifier);
		foreignKey.HasDependentToPrincipal(text);
		if ((AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue18633", out var isEnabled) && isEnabled) || foreignKey.DeclaringEntityType.FindPrimaryKey() != null)
		{
			List<string> existingIdentifiers2 = ExistingIdentifiers(foreignKey.PrincipalEntityType);
			string identifier = (foreignKey.IsSelfReferencing() ? string.Format(CultureInfo.CurrentCulture, "Inverse{0}", text) : _candidateNamingService.GetPrincipalEndCandidateNavigationPropertyName(foreignKey, text));
			if (!foreignKey.IsUnique && !foreignKey.IsSelfReferencing())
			{
				identifier = _pluralizer.Pluralize(identifier);
			}
			string name = _cSharpUtilities.GenerateCSharpIdentifier(identifier, existingIdentifiers2, null, NavigationUniquifier);
			foreignKey.HasPrincipalToDependent(name);
		}
	}

	protected virtual List<string> ExistingIdentifiers([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
		if (!_entityTypeAndPropertyIdentifiers.TryGetValue(entityType, out var value))
		{
			value = new List<string> { entityType.Name };
			value.AddRange(from p in entityType.GetProperties()
				select p.Name);
			_entityTypeAndPropertyIdentifiers[entityType] = value;
		}
		value.AddRange(from p in entityType.GetNavigations()
			select p.Name);
		return value;
	}

	protected virtual TypeScaffoldingInfo GetTypeScaffoldingInfo([JetBrains.Annotations.NotNull] DatabaseColumn column)
	{
		if (column.StoreType == null)
		{
			return null;
		}
		return _scaffoldingTypeMapper.FindMapping(column.StoreType, column.IsKeyOrIndex(), column.IsRowVersion());
	}

	private static void AssignOnDeleteAction([JetBrains.Annotations.NotNull] DatabaseForeignKey databaseForeignKey, [JetBrains.Annotations.NotNull] IMutableForeignKey foreignKey)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(databaseForeignKey, "databaseForeignKey");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(foreignKey, "foreignKey");
		switch (databaseForeignKey.OnDelete)
		{
		case ReferentialAction.Cascade:
			foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
			break;
		case ReferentialAction.SetNull:
			foreignKey.DeleteBehavior = DeleteBehavior.SetNull;
			break;
		default:
			foreignKey.DeleteBehavior = DeleteBehavior.ClientSetNull;
			break;
		}
	}

	private static string NavigationUniquifier([JetBrains.Annotations.NotNull] string proposedIdentifier, [JetBrains.Annotations.CanBeNull] ICollection<string> existingIdentifiers)
	{
		if (existingIdentifiers == null || !existingIdentifiers.Contains(proposedIdentifier))
		{
			return proposedIdentifier;
		}
		string text = string.Format(CultureInfo.CurrentCulture, "{0}Navigation", proposedIdentifier);
		int num = 1;
		while (existingIdentifiers.Contains(text))
		{
			text = proposedIdentifier + num;
			num++;
		}
		return text;
	}
}
