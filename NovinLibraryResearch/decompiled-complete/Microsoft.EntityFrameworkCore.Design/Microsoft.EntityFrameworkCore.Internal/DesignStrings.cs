using System.Reflection;
using System.Resources;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public static class DesignStrings
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.DesignStrings", typeof(DesignStrings).GetTypeInfo().Assembly);

	public static string MultipleContexts => GetString("MultipleContexts");

	public static string ManuallyDeleted => GetString("ManuallyDeleted");

	public static string NoSnapshot => GetString("NoSnapshot");

	public static string RemovingSnapshot => GetString("RemovingSnapshot");

	public static string RevertingSnapshot => GetString("RevertingSnapshot");

	public static string DestructiveOperation => GetString("DestructiveOperation");

	public static string Done => GetString("Done");

	public static string FindingContexts => GetString("FindingContexts");

	public static string SensitiveInformationWarning => GetString("SensitiveInformationWarning");

	public static string FindingServiceProvider => GetString("FindingServiceProvider");

	public static string NoServiceProvider => GetString("NoServiceProvider");

	public static string NoDesignTimeServices => GetString("NoDesignTimeServices");

	public static string FindingContextFactories => GetString("FindingContextFactories");

	public static string FindingReferencedContexts => GetString("FindingReferencedContexts");

	public static string SequencesRequireName => GetString("SequencesRequireName");

	public static string NoReferencedServices => GetString("NoReferencedServices");

	public static string FindingHostingServices => GetString("FindingHostingServices");

	public static string MalformedCreateHostBuilder => GetString("MalformedCreateHostBuilder");

	public static string NoCreateHostBuilder => GetString("NoCreateHostBuilder");

	public static string UsingHostingServices => GetString("UsingHostingServices");

	public static string DuplicateMigrationName([JetBrains.Annotations.CanBeNull] object migrationName)
	{
		return string.Format(GetString("DuplicateMigrationName", "migrationName"), migrationName);
	}

	public static string MultipleContextsWithName([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("MultipleContextsWithName", "name"), name);
	}

	public static string MultipleContextsWithQualifiedName([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("MultipleContextsWithQualifiedName", "name"), name);
	}

	public static string NoContext([JetBrains.Annotations.CanBeNull] object assembly)
	{
		return string.Format(GetString("NoContext", "assembly"), assembly);
	}

	public static string NoContextWithName([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("NoContextWithName", "name"), name);
	}

	public static string UseContext([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("UseContext", "name"), name);
	}

	public static string DroppingDatabase([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("DroppingDatabase", "name"), name);
	}

	public static string DatabaseDropped([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("DatabaseDropped", "name"), name);
	}

	public static string NoMigrationFile([JetBrains.Annotations.CanBeNull] object file, [JetBrains.Annotations.CanBeNull] object migrationClass)
	{
		return string.Format(GetString("NoMigrationFile", "file", "migrationClass"), file, migrationClass);
	}

	public static string NoMigrationMetadataFile([JetBrains.Annotations.CanBeNull] object file)
	{
		return string.Format(GetString("NoMigrationMetadataFile", "file"), file);
	}

	public static string NoSnapshotFile([JetBrains.Annotations.CanBeNull] object file, [JetBrains.Annotations.CanBeNull] object snapshotClass)
	{
		return string.Format(GetString("NoSnapshotFile", "file", "snapshotClass"), file, snapshotClass);
	}

	public static string RemovingMigration([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("RemovingMigration", "name"), name);
	}

	public static string RevertMigration([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("RevertMigration", "name"), name);
	}

	public static string UnknownOperation([JetBrains.Annotations.CanBeNull] object operationType)
	{
		return string.Format(GetString("UnknownOperation", "operationType"), operationType);
	}

	public static string UnknownLiteral([JetBrains.Annotations.CanBeNull] object literalType)
	{
		return string.Format(GetString("UnknownLiteral", "literalType"), literalType);
	}

	public static string LiteralExpressionNotSupported([JetBrains.Annotations.CanBeNull] object expression, [JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("LiteralExpressionNotSupported", "expression", "type"), expression, type);
	}

	public static string CannotFindRuntimeProviderAssembly([JetBrains.Annotations.CanBeNull] object assemblyName)
	{
		return string.Format(GetString("CannotFindRuntimeProviderAssembly", "assemblyName"), assemblyName);
	}

	public static string WritingMigration([JetBrains.Annotations.CanBeNull] object file)
	{
		return string.Format(GetString("WritingMigration", "file"), file);
	}

	public static string WritingSnapshot([JetBrains.Annotations.CanBeNull] object file)
	{
		return string.Format(GetString("WritingSnapshot", "file"), file);
	}

	public static string ReusingNamespace([JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("ReusingNamespace", "type"), type);
	}

	public static string ReusingSnapshotName([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("ReusingSnapshotName", "name"), name);
	}

	public static string CannotFindDesignTimeProviderAssemblyAttribute([JetBrains.Annotations.CanBeNull] object attributeName, [JetBrains.Annotations.CanBeNull] object runtimeProviderAssemblyName)
	{
		return string.Format(GetString("CannotFindDesignTimeProviderAssemblyAttribute", "attributeName", "runtimeProviderAssemblyName"), attributeName, runtimeProviderAssemblyName);
	}

	public static string NonRelationalProvider([JetBrains.Annotations.CanBeNull] object provider)
	{
		return string.Format(GetString("NonRelationalProvider", "provider"), provider);
	}

	public static string UnreferencedAssembly([JetBrains.Annotations.CanBeNull] object assembly, [JetBrains.Annotations.CanBeNull] object startupProject)
	{
		return string.Format(GetString("UnreferencedAssembly", "assembly", "startupProject"), assembly, startupProject);
	}

	public static string ForeignMigrations([JetBrains.Annotations.CanBeNull] object migrationsNamespace)
	{
		return string.Format(GetString("ForeignMigrations", "migrationsNamespace"), migrationsNamespace);
	}

	public static string ContextClassNotValidCSharpIdentifier([JetBrains.Annotations.CanBeNull] object contextClassName)
	{
		return string.Format(GetString("ContextClassNotValidCSharpIdentifier", "contextClassName"), contextClassName);
	}

	public static string MigrationsAssemblyMismatch([JetBrains.Annotations.CanBeNull] object assembly, [JetBrains.Annotations.CanBeNull] object migrationsAssembly)
	{
		return string.Format(GetString("MigrationsAssemblyMismatch", "assembly", "migrationsAssembly"), assembly, migrationsAssembly);
	}

	public static string ForceRemoveMigration([JetBrains.Annotations.CanBeNull] object name, [JetBrains.Annotations.CanBeNull] object error)
	{
		return string.Format(GetString("ForceRemoveMigration", "name", "error"), name, error);
	}

	public static string NoParameterlessConstructor([JetBrains.Annotations.CanBeNull] object contextType)
	{
		return string.Format(GetString("NoParameterlessConstructor", "contextType"), contextType);
	}

	public static string NotExistDatabase([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("NotExistDatabase", "name"), name);
	}

	public static string UsingEnvironment([JetBrains.Annotations.CanBeNull] object environment)
	{
		return string.Format(GetString("UsingEnvironment", "environment"), environment);
	}

	public static string FoundDbContext([JetBrains.Annotations.CanBeNull] object contextType)
	{
		return string.Format(GetString("FoundDbContext", "contextType"), contextType);
	}

	public static string UsingDbContextFactory([JetBrains.Annotations.CanBeNull] object factory)
	{
		return string.Format(GetString("UsingDbContextFactory", "factory"), factory);
	}

	public static string FindingDesignTimeServices([JetBrains.Annotations.CanBeNull] object startupAssembly)
	{
		return string.Format(GetString("FindingDesignTimeServices", "startupAssembly"), startupAssembly);
	}

	public static string FindingProviderServices([JetBrains.Annotations.CanBeNull] object provider)
	{
		return string.Format(GetString("FindingProviderServices", "provider"), provider);
	}

	public static string UsingDesignTimeServices([JetBrains.Annotations.CanBeNull] object designTimeServices)
	{
		return string.Format(GetString("UsingDesignTimeServices", "designTimeServices"), designTimeServices);
	}

	public static string UsingProviderServices([JetBrains.Annotations.CanBeNull] object provider)
	{
		return string.Format(GetString("UsingProviderServices", "provider"), provider);
	}

	public static string FoundContextFactory([JetBrains.Annotations.CanBeNull] object factory)
	{
		return string.Format(GetString("FoundContextFactory", "factory"), factory);
	}

	public static string ProviderReturnedNullModel([JetBrains.Annotations.CanBeNull] object providerTypeName)
	{
		return string.Format(GetString("ProviderReturnedNullModel", "providerTypeName"), providerTypeName);
	}

	public static string ReadOnlyFiles([JetBrains.Annotations.CanBeNull] object outputDirectoryName, [JetBrains.Annotations.CanBeNull] object readOnlyFiles)
	{
		return string.Format(GetString("ReadOnlyFiles", "outputDirectoryName", "readOnlyFiles"), outputDirectoryName, readOnlyFiles);
	}

	public static string ExistingFiles([JetBrains.Annotations.CanBeNull] object outputDirectoryName, [JetBrains.Annotations.CanBeNull] object existingFiles)
	{
		return string.Format(GetString("ExistingFiles", "outputDirectoryName", "existingFiles"), outputDirectoryName, existingFiles);
	}

	public static string CannotFindTypeMappingForColumn([JetBrains.Annotations.CanBeNull] object columnName, [JetBrains.Annotations.CanBeNull] object dateType)
	{
		return string.Format(GetString("CannotFindTypeMappingForColumn", "columnName", "dateType"), columnName, dateType);
	}

	public static string ForeignKeyScaffoldErrorPrincipalKeyNotFound([JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object columnsList, [JetBrains.Annotations.CanBeNull] object principalEntityType)
	{
		return string.Format(GetString("ForeignKeyScaffoldErrorPrincipalKeyNotFound", "foreignKeyName", "columnsList", "principalEntityType"), foreignKeyName, columnsList, principalEntityType);
	}

	public static string ForeignKeyScaffoldErrorPrincipalTableScaffoldingError([JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object principaltableName)
	{
		return string.Format(GetString("ForeignKeyScaffoldErrorPrincipalTableScaffoldingError", "foreignKeyName", "principaltableName"), foreignKeyName, principaltableName);
	}

	public static string PrimaryKeyErrorPropertyNotFound([JetBrains.Annotations.CanBeNull] object tableName, [JetBrains.Annotations.CanBeNull] object columnNames)
	{
		return string.Format(GetString("PrimaryKeyErrorPropertyNotFound", "tableName", "columnNames"), tableName, columnNames);
	}

	public static string UnableToGenerateEntityType([JetBrains.Annotations.CanBeNull] object tableName)
	{
		return string.Format(GetString("UnableToGenerateEntityType", "tableName"), tableName);
	}

	public static string BadSequenceType([JetBrains.Annotations.CanBeNull] object sequenceName, [JetBrains.Annotations.CanBeNull] object typeName)
	{
		return string.Format(GetString("BadSequenceType", "sequenceName", "typeName"), sequenceName, typeName);
	}

	public static string ForeignKeyPrincipalEndContainsNullableColumns([JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object indexName, [JetBrains.Annotations.CanBeNull] object columnNames)
	{
		return string.Format(GetString("ForeignKeyPrincipalEndContainsNullableColumns", "foreignKeyName", "indexName", "columnNames"), foreignKeyName, indexName, columnNames);
	}

	public static string NonNullableBoooleanColumnHasDefaultConstraint([JetBrains.Annotations.CanBeNull] object columnName)
	{
		return string.Format(GetString("NonNullableBoooleanColumnHasDefaultConstraint", "columnName"), columnName);
	}

	public static string ForeignKeyScaffoldErrorPrincipalTableNotFound([JetBrains.Annotations.CanBeNull] object foreignKeyName)
	{
		return string.Format(GetString("ForeignKeyScaffoldErrorPrincipalTableNotFound", "foreignKeyName"), foreignKeyName);
	}

	public static string ForeignKeyScaffoldErrorPropertyNotFound([JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object columnNames)
	{
		return string.Format(GetString("ForeignKeyScaffoldErrorPropertyNotFound", "foreignKeyName", "columnNames"), foreignKeyName, columnNames);
	}

	public static string UnableToScaffoldIndexMissingProperty([JetBrains.Annotations.CanBeNull] object indexName, [JetBrains.Annotations.CanBeNull] object columnNames)
	{
		return string.Format(GetString("UnableToScaffoldIndexMissingProperty", "indexName", "columnNames"), indexName, columnNames);
	}

	public static string MultipleAnnotationConflict([JetBrains.Annotations.CanBeNull] object annotationName)
	{
		return string.Format(GetString("MultipleAnnotationConflict", "annotationName"), annotationName);
	}

	public static string NoLanguageService([JetBrains.Annotations.CanBeNull] object language, [JetBrains.Annotations.CanBeNull] object service)
	{
		return string.Format(GetString("NoLanguageService", "language", "service"), language, service);
	}

	public static string RevertingMigration([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("RevertingMigration", "name"), name);
	}

	public static string FindingReferencedServices([JetBrains.Annotations.CanBeNull] object startupAssembly)
	{
		return string.Format(GetString("FindingReferencedServices", "startupAssembly"), startupAssembly);
	}

	public static string UsingReferencedServices([JetBrains.Annotations.CanBeNull] object referencedAssembly)
	{
		return string.Format(GetString("UsingReferencedServices", "referencedAssembly"), referencedAssembly);
	}

	public static string VersionMismatch([JetBrains.Annotations.CanBeNull] object toolsVersion, [JetBrains.Annotations.CanBeNull] object runtimeVersion)
	{
		return string.Format(GetString("VersionMismatch", "toolsVersion", "runtimeVersion"), toolsVersion, runtimeVersion);
	}

	public static string InvokeCreateHostBuilderFailed([JetBrains.Annotations.CanBeNull] object error)
	{
		return string.Format(GetString("InvokeCreateHostBuilderFailed", "error"), error);
	}

	private static string GetString(string name, params string[] formatterNames)
	{
		string text = _resourceManager.GetString(name);
		for (int i = 0; i < formatterNames.Length; i++)
		{
			text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
		}
		return text;
	}
}
