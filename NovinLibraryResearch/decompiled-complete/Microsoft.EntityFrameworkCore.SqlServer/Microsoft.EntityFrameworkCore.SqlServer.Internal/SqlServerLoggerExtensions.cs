using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.SqlServer.Internal;

public static class SqlServerLoggerExtensions
{
	public static void DecimalTypeDefaultWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
	{
		EventDefinition<string, string> eventDefinition = SqlServerResources.LogDefaultDecimalTypeColumn(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.Name, property.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyEventData(eventDefinition, DecimalTypeDefaultWarning, property));
		}
	}

	private static string DecimalTypeDefaultWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyEventData propertyEventData = (PropertyEventData)payload;
		return obj.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
	}

	public static void ByteIdentityColumnWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
	{
		EventDefinition<string, string> eventDefinition = SqlServerResources.LogByteIdentityColumn(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, property.Name, property.DeclaringEntityType.DisplayName());
		}
		if (diagnostics.DiagnosticSource.IsEnabled(eventDefinition.EventId.Name))
		{
			diagnostics.DiagnosticSource.Write(eventDefinition.EventId.Name, new PropertyEventData(eventDefinition, ByteIdentityColumnWarning, property));
		}
	}

	private static string ByteIdentityColumnWarning(EventDefinitionBase definition, EventData payload)
	{
		EventDefinition<string, string> obj = (EventDefinition<string, string>)definition;
		PropertyEventData propertyEventData = (PropertyEventData)payload;
		return obj.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
	}

	public static void ColumnFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string columnName, int ordinal, [JetBrains.Annotations.NotNull] string dataTypeName, int maxLength, int precision, int scale, bool nullable, bool identity, [JetBrains.Annotations.CanBeNull] string defaultValue, [JetBrains.Annotations.CanBeNull] string computedValue)
	{
		FallbackEventDefinition definition = SqlServerResources.LogFoundColumn(diagnostics);
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, delegate(ILogger l)
			{
				l.LogDebug(definition.EventId, null, definition.MessageFormat, tableName, columnName, ordinal, dataTypeName, maxLength, precision, scale, nullable, identity, defaultValue, computedValue);
			});
		}
	}

	public static void ForeignKeyFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string foreignKeyName, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string principalTableName, [JetBrains.Annotations.NotNull] string onDeleteAction)
	{
		EventDefinition<string, string, string, string> eventDefinition = SqlServerResources.LogFoundForeignKey(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, foreignKeyName, tableName, principalTableName, onDeleteAction);
		}
	}

	public static void DefaultSchemaFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string schemaName)
	{
		EventDefinition<string> eventDefinition = SqlServerResources.LogFoundDefaultSchema(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, schemaName);
		}
	}

	public static void TypeAliasFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string typeAliasName, [JetBrains.Annotations.NotNull] string systemTypeName)
	{
		EventDefinition<string, string> eventDefinition = SqlServerResources.LogFoundTypeAlias(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, typeAliasName, systemTypeName);
		}
	}

	public static void PrimaryKeyFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string primaryKeyName, [JetBrains.Annotations.NotNull] string tableName)
	{
		EventDefinition<string, string> eventDefinition = SqlServerResources.LogFoundPrimaryKey(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, primaryKeyName, tableName);
		}
	}

	public static void UniqueConstraintFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string uniqueConstraintName, [JetBrains.Annotations.NotNull] string tableName)
	{
		EventDefinition<string, string> eventDefinition = SqlServerResources.LogFoundUniqueConstraint(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, uniqueConstraintName, tableName);
		}
	}

	public static void IndexFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string indexName, [JetBrains.Annotations.NotNull] string tableName, bool unique)
	{
		EventDefinition<string, string, bool> eventDefinition = SqlServerResources.LogFoundIndex(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, indexName, tableName, unique);
		}
	}

	public static void ForeignKeyReferencesMissingPrincipalTableWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.CanBeNull] string foreignKeyName, [JetBrains.Annotations.CanBeNull] string tableName, [JetBrains.Annotations.CanBeNull] string principalTableName)
	{
		EventDefinition<string, string, string> eventDefinition = SqlServerResources.LogPrincipalTableNotInSelectionSet(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, foreignKeyName, tableName, principalTableName);
		}
	}

	public static void ForeignKeyPrincipalColumnMissingWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string foreignKeyName, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string principalColumnName, [JetBrains.Annotations.NotNull] string principalTableName)
	{
		EventDefinition<string, string, string, string> eventDefinition = SqlServerResources.LogPrincipalColumnNotFound(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, foreignKeyName, tableName, principalColumnName, principalTableName);
		}
	}

	public static void MissingSchemaWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.CanBeNull] string schemaName)
	{
		EventDefinition<string> eventDefinition = SqlServerResources.LogMissingSchema(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, schemaName);
		}
	}

	public static void MissingTableWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.CanBeNull] string tableName)
	{
		EventDefinition<string> eventDefinition = SqlServerResources.LogMissingTable(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, tableName);
		}
	}

	public static void SequenceFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string sequenceName, [JetBrains.Annotations.NotNull] string sequenceTypeName, bool cyclic, int increment, long start, long min, long max)
	{
		FallbackEventDefinition definition = SqlServerResources.LogFoundSequence(diagnostics);
		WarningBehavior logBehavior = definition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			definition.Log(diagnostics, logBehavior, delegate(ILogger l)
			{
				l.LogDebug(definition.EventId, null, definition.MessageFormat, sequenceName, sequenceTypeName, cyclic, increment, start, min, max);
			});
		}
	}

	public static void TableFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string tableName)
	{
		EventDefinition<string> eventDefinition = SqlServerResources.LogFoundTable(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, tableName);
		}
	}

	public static void ReflexiveConstraintIgnored([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string foreignKeyName, [JetBrains.Annotations.NotNull] string tableName)
	{
		EventDefinition<string, string> eventDefinition = SqlServerResources.LogReflexiveConstraintIgnored(diagnostics);
		WarningBehavior logBehavior = eventDefinition.GetLogBehavior(diagnostics);
		if (logBehavior != WarningBehavior.Ignore)
		{
			eventDefinition.Log(diagnostics, logBehavior, foreignKeyName, tableName);
		}
	}
}
