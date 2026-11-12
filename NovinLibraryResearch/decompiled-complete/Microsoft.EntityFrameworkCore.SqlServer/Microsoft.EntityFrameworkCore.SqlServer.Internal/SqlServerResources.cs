using System.Reflection;
using System.Resources;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.SqlServer.Diagnostics.Internal;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.SqlServer.Internal;

public static class SqlServerResources
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.SqlServer.Properties.SqlServerStrings", typeof(SqlServerResources).GetTypeInfo().Assembly);

	public static EventDefinition<string, string> LogDefaultDecimalTypeColumn([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogDefaultDecimalTypeColumn;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogDefaultDecimalTypeColumn, () => new EventDefinition<string, string>(logger.Options, SqlServerEventId.DecimalTypeDefaultWarning, LogLevel.Warning, "SqlServerEventId.DecimalTypeDefaultWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, SqlServerEventId.DecimalTypeDefaultWarning, _resourceManager.GetString("LogDefaultDecimalTypeColumn"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogByteIdentityColumn([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogByteIdentityColumn;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogByteIdentityColumn, () => new EventDefinition<string, string>(logger.Options, SqlServerEventId.ByteIdentityColumnWarning, LogLevel.Warning, "SqlServerEventId.ByteIdentityColumnWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, SqlServerEventId.ByteIdentityColumnWarning, _resourceManager.GetString("LogByteIdentityColumn"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogFoundDefaultSchema([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundDefaultSchema;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundDefaultSchema, () => new EventDefinition<string>(logger.Options, SqlServerEventId.DefaultSchemaFound, LogLevel.Debug, "SqlServerEventId.DefaultSchemaFound", (LogLevel level) => LoggerMessage.Define<string>(level, SqlServerEventId.DefaultSchemaFound, _resourceManager.GetString("LogFoundDefaultSchema"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogFoundTypeAlias([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundTypeAlias;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundTypeAlias, () => new EventDefinition<string, string>(logger.Options, SqlServerEventId.TypeAliasFound, LogLevel.Debug, "SqlServerEventId.TypeAliasFound", (LogLevel level) => LoggerMessage.Define<string, string>(level, SqlServerEventId.TypeAliasFound, _resourceManager.GetString("LogFoundTypeAlias"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static FallbackEventDefinition LogFoundColumn([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundColumn;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundColumn, () => new FallbackEventDefinition(logger.Options, SqlServerEventId.ColumnFound, LogLevel.Debug, "SqlServerEventId.ColumnFound", _resourceManager.GetString("LogFoundColumn")));
		}
		return (FallbackEventDefinition)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogFoundForeignKey([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundForeignKey;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundForeignKey, () => new EventDefinition<string, string, string, string>(logger.Options, SqlServerEventId.ForeignKeyFound, LogLevel.Debug, "SqlServerEventId.ForeignKeyFound", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, SqlServerEventId.ForeignKeyFound, _resourceManager.GetString("LogFoundForeignKey"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string> LogPrincipalTableNotInSelectionSet([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogPrincipalTableNotInSelectionSet;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogPrincipalTableNotInSelectionSet, () => new EventDefinition<string, string, string>(logger.Options, SqlServerEventId.ForeignKeyReferencesMissingPrincipalTableWarning, LogLevel.Warning, "SqlServerEventId.ForeignKeyReferencesMissingPrincipalTableWarning", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, SqlServerEventId.ForeignKeyReferencesMissingPrincipalTableWarning, _resourceManager.GetString("LogPrincipalTableNotInSelectionSet"))));
		}
		return (EventDefinition<string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogMissingSchema([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogMissingSchema;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogMissingSchema, () => new EventDefinition<string>(logger.Options, SqlServerEventId.MissingSchemaWarning, LogLevel.Warning, "SqlServerEventId.MissingSchemaWarning", (LogLevel level) => LoggerMessage.Define<string>(level, SqlServerEventId.MissingSchemaWarning, _resourceManager.GetString("LogMissingSchema"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string> LogMissingTable([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogMissingTable;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogMissingTable, () => new EventDefinition<string>(logger.Options, SqlServerEventId.MissingTableWarning, LogLevel.Warning, "SqlServerEventId.MissingTableWarning", (LogLevel level) => LoggerMessage.Define<string>(level, SqlServerEventId.MissingTableWarning, _resourceManager.GetString("LogMissingTable"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static FallbackEventDefinition LogFoundSequence([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundSequence;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundSequence, () => new FallbackEventDefinition(logger.Options, SqlServerEventId.SequenceFound, LogLevel.Debug, "SqlServerEventId.SequenceFound", _resourceManager.GetString("LogFoundSequence")));
		}
		return (FallbackEventDefinition)eventDefinitionBase;
	}

	public static EventDefinition<string> LogFoundTable([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundTable;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundTable, () => new EventDefinition<string>(logger.Options, SqlServerEventId.TableFound, LogLevel.Debug, "SqlServerEventId.TableFound", (LogLevel level) => LoggerMessage.Define<string>(level, SqlServerEventId.TableFound, _resourceManager.GetString("LogFoundTable"))));
		}
		return (EventDefinition<string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, bool> LogFoundIndex([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundIndex;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundIndex, () => new EventDefinition<string, string, bool>(logger.Options, SqlServerEventId.IndexFound, LogLevel.Debug, "SqlServerEventId.IndexFound", (LogLevel level) => LoggerMessage.Define<string, string, bool>(level, SqlServerEventId.IndexFound, _resourceManager.GetString("LogFoundIndex"))));
		}
		return (EventDefinition<string, string, bool>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogFoundPrimaryKey([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundPrimaryKey;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundPrimaryKey, () => new EventDefinition<string, string>(logger.Options, SqlServerEventId.PrimaryKeyFound, LogLevel.Debug, "SqlServerEventId.PrimaryKeyFound", (LogLevel level) => LoggerMessage.Define<string, string>(level, SqlServerEventId.PrimaryKeyFound, _resourceManager.GetString("LogFoundPrimaryKey"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogFoundUniqueConstraint([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundUniqueConstraint;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogFoundUniqueConstraint, () => new EventDefinition<string, string>(logger.Options, SqlServerEventId.UniqueConstraintFound, LogLevel.Debug, "SqlServerEventId.UniqueConstraintFound", (LogLevel level) => LoggerMessage.Define<string, string>(level, SqlServerEventId.UniqueConstraintFound, _resourceManager.GetString("LogFoundUniqueConstraint"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string, string, string> LogPrincipalColumnNotFound([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogPrincipalColumnNotFound;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogPrincipalColumnNotFound, () => new EventDefinition<string, string, string, string>(logger.Options, SqlServerEventId.ForeignKeyPrincipalColumnMissingWarning, LogLevel.Warning, "SqlServerEventId.ForeignKeyPrincipalColumnMissingWarning", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, SqlServerEventId.ForeignKeyPrincipalColumnMissingWarning, _resourceManager.GetString("LogPrincipalColumnNotFound"))));
		}
		return (EventDefinition<string, string, string, string>)eventDefinitionBase;
	}

	public static EventDefinition<string, string> LogReflexiveConstraintIgnored([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
	{
		EventDefinitionBase eventDefinitionBase = ((SqlServerLoggingDefinitions)logger.Definitions).LogReflexiveConstraintIgnored;
		if (eventDefinitionBase == null)
		{
			eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((SqlServerLoggingDefinitions)logger.Definitions).LogReflexiveConstraintIgnored, () => new EventDefinition<string, string>(logger.Options, SqlServerEventId.ReflexiveConstraintIgnored, LogLevel.Debug, "SqlServerEventId.ReflexiveConstraintIgnored", (LogLevel level) => LoggerMessage.Define<string, string>(level, SqlServerEventId.ReflexiveConstraintIgnored, _resourceManager.GetString("LogReflexiveConstraintIgnored"))));
		}
		return (EventDefinition<string, string>)eventDefinitionBase;
	}
}
