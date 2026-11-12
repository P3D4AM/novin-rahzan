using System.Reflection;
using System.Resources;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public static class RelationalStrings
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Properties.RelationalStrings", typeof(RelationalStrings).GetTypeInfo().Assembly);

	public static string MultipleProvidersConfigured => GetString("MultipleProvidersConfigured");

	public static string NoProviderConfigured => GetString("NoProviderConfigured");

	public static string ConnectionAndConnectionString => GetString("ConnectionAndConnectionString");

	public static string NoConnectionOrConnectionString => GetString("NoConnectionOrConnectionString");

	public static string RelationalNotInUse => GetString("RelationalNotInUse");

	public static string UpdateStoreException => GetString("UpdateStoreException");

	public static string TransactionAlreadyStarted => GetString("TransactionAlreadyStarted");

	public static string TransactionAssociatedWithDifferentConnection => GetString("TransactionAssociatedWithDifferentConnection");

	public static string BadSequenceType => GetString("BadSequenceType");

	public static string BadSequenceString => GetString("BadSequenceString");

	public static string BadCheckConstraintString => GetString("BadCheckConstraintString");

	public static string InvalidCommandTimeout => GetString("InvalidCommandTimeout");

	public static string InvalidMaxBatchSize => GetString("InvalidMaxBatchSize");

	public static string NoActiveTransaction => GetString("NoActiveTransaction");

	public static string ConflictingAmbientTransaction => GetString("ConflictingAmbientTransaction");

	public static string ConflictingEnlistedTransaction => GetString("ConflictingEnlistedTransaction");

	public static string InvalidMinBatchSize => GetString("InvalidMinBatchSize");

	public static string SqlFunctionArgumentsAndMappingsMismatch => GetString("SqlFunctionArgumentsAndMappingsMismatch");

	public static string SqlFunctionNullArgumentMapping => GetString("SqlFunctionNullArgumentMapping");

	public static string SqlFunctionUnexpectedInstanceMapping => GetString("SqlFunctionUnexpectedInstanceMapping");

	public static string PendingAmbientTransaction => GetString("PendingAmbientTransaction");

	public static string SetOperationNotWithinEntityTypeHierarchy => GetString("SetOperationNotWithinEntityTypeHierarchy");

	public static string FromSqlNonComposable => GetString("FromSqlNonComposable");

	public static string ModificationCommandInvalidEntityState([JetBrains.Annotations.CanBeNull] object entityState)
	{
		return string.Format(GetString("ModificationCommandInvalidEntityState", "entityState"), entityState);
	}

	public static string UpdateConcurrencyException([JetBrains.Annotations.CanBeNull] object expectedRows, [JetBrains.Annotations.CanBeNull] object actualRows)
	{
		return string.Format(GetString("UpdateConcurrencyException", "expectedRows", "actualRows"), expectedRows, actualRows);
	}

	public static string NamedConnectionStringNotFound([JetBrains.Annotations.CanBeNull] object name)
	{
		return string.Format(GetString("NamedConnectionStringNotFound", "name"), name);
	}

	public static string UnsupportedType([JetBrains.Annotations.CanBeNull] object clrType)
	{
		return string.Format(GetString("UnsupportedType", "clrType"), clrType);
	}

	public static string MigrationNotFound([JetBrains.Annotations.CanBeNull] object migrationName)
	{
		return string.Format(GetString("MigrationNotFound", "migrationName"), migrationName);
	}

	public static string UnknownOperation([JetBrains.Annotations.CanBeNull] object sqlGeneratorType, [JetBrains.Annotations.CanBeNull] object operationType)
	{
		return string.Format(GetString("UnknownOperation", "sqlGeneratorType", "operationType"), sqlGeneratorType, operationType);
	}

	public static string FromSqlMissingColumn([JetBrains.Annotations.CanBeNull] object column)
	{
		return string.Format(GetString("FromSqlMissingColumn", "column"), column);
	}

	public static string IncompatibleTableKeyNameMismatch([JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object otherEntityType, [JetBrains.Annotations.CanBeNull] object keyName, [JetBrains.Annotations.CanBeNull] object primaryKey, [JetBrains.Annotations.CanBeNull] object otherName, [JetBrains.Annotations.CanBeNull] object otherPrimaryKey)
	{
		return string.Format(GetString("IncompatibleTableKeyNameMismatch", "table", "entityType", "otherEntityType", "keyName", "primaryKey", "otherName", "otherPrimaryKey"), table, entityType, otherEntityType, keyName, primaryKey, otherName, otherPrimaryKey);
	}

	public static string IncompatibleTableCommentMismatch([JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object otherEntityType, [JetBrains.Annotations.CanBeNull] object comment, [JetBrains.Annotations.CanBeNull] object otherComment)
	{
		return string.Format(GetString("IncompatibleTableCommentMismatch", "table", "entityType", "otherEntityType", "comment", "otherComment"), table, entityType, otherEntityType, comment, otherComment);
	}

	public static string IncompatibleTableNoRelationship([JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object otherEntityType)
	{
		return string.Format(GetString("IncompatibleTableNoRelationship", "table", "entityType", "otherEntityType"), table, entityType, otherEntityType);
	}

	public static string MissingParameterValue([JetBrains.Annotations.CanBeNull] object parameter)
	{
		return string.Format(GetString("MissingParameterValue", "parameter"), parameter);
	}

	public static string ParameterNotObjectArray([JetBrains.Annotations.CanBeNull] object parameter)
	{
		return string.Format(GetString("ParameterNotObjectArray", "parameter"), parameter);
	}

	public static string DuplicateColumnNameDataTypeMismatch([JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object property1, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object property2, [JetBrains.Annotations.CanBeNull] object columnName, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object dataType1, [JetBrains.Annotations.CanBeNull] object dataType2)
	{
		return string.Format(GetString("DuplicateColumnNameDataTypeMismatch", "entityType1", "property1", "entityType2", "property2", "columnName", "table", "dataType1", "dataType2"), entityType1, property1, entityType2, property2, columnName, table, dataType1, dataType2);
	}

	public static string ConflictingRowUpdateTypes([JetBrains.Annotations.CanBeNull] object firstEntityType, [JetBrains.Annotations.CanBeNull] object firstState, [JetBrains.Annotations.CanBeNull] object secondEntityType, [JetBrains.Annotations.CanBeNull] object secondState)
	{
		return string.Format(GetString("ConflictingRowUpdateTypes", "firstEntityType", "firstState", "secondEntityType", "secondState"), firstEntityType, firstState, secondEntityType, secondState);
	}

	public static string ConflictingRowUpdateTypesSensitive([JetBrains.Annotations.CanBeNull] object firstEntityType, [JetBrains.Annotations.CanBeNull] object firstKeyValue, [JetBrains.Annotations.CanBeNull] object firstState, [JetBrains.Annotations.CanBeNull] object secondEntityType, [JetBrains.Annotations.CanBeNull] object secondKeyValue, [JetBrains.Annotations.CanBeNull] object secondState)
	{
		return string.Format(GetString("ConflictingRowUpdateTypesSensitive", "firstEntityType", "firstKeyValue", "firstState", "secondEntityType", "secondKeyValue", "secondState"), firstEntityType, firstKeyValue, firstState, secondEntityType, secondKeyValue, secondState);
	}

	public static string ConflictingRowValues([JetBrains.Annotations.CanBeNull] object firstEntityType, [JetBrains.Annotations.CanBeNull] object secondEntityType, [JetBrains.Annotations.CanBeNull] object firstProperties, [JetBrains.Annotations.CanBeNull] object secondProperties, [JetBrains.Annotations.CanBeNull] object columns)
	{
		return string.Format(GetString("ConflictingRowValues", "firstEntityType", "secondEntityType", "firstProperties", "secondProperties", "columns"), firstEntityType, secondEntityType, firstProperties, secondProperties, columns);
	}

	public static string ConflictingRowValuesSensitive([JetBrains.Annotations.CanBeNull] object firstEntityType, [JetBrains.Annotations.CanBeNull] object secondEntityType, [JetBrains.Annotations.CanBeNull] object keyValue, [JetBrains.Annotations.CanBeNull] object firstConflictingValues, [JetBrains.Annotations.CanBeNull] object secondConflictingValues, [JetBrains.Annotations.CanBeNull] object columns)
	{
		return string.Format(GetString("ConflictingRowValuesSensitive", "firstEntityType", "secondEntityType", "keyValue", "firstConflictingValues", "secondConflictingValues", "columns"), firstEntityType, secondEntityType, keyValue, firstConflictingValues, secondConflictingValues, columns);
	}

	public static string ConflictingOriginalRowValues([JetBrains.Annotations.CanBeNull] object firstEntityType, [JetBrains.Annotations.CanBeNull] object secondEntityType, [JetBrains.Annotations.CanBeNull] object firstProperties, [JetBrains.Annotations.CanBeNull] object secondProperties, [JetBrains.Annotations.CanBeNull] object columns)
	{
		return string.Format(GetString("ConflictingOriginalRowValues", "firstEntityType", "secondEntityType", "firstProperties", "secondProperties", "columns"), firstEntityType, secondEntityType, firstProperties, secondProperties, columns);
	}

	public static string ConflictingOriginalRowValuesSensitive([JetBrains.Annotations.CanBeNull] object firstEntityType, [JetBrains.Annotations.CanBeNull] object secondEntityType, [JetBrains.Annotations.CanBeNull] object keyValue, [JetBrains.Annotations.CanBeNull] object firstConflictingValues, [JetBrains.Annotations.CanBeNull] object secondConflictingValues, [JetBrains.Annotations.CanBeNull] object columns)
	{
		return string.Format(GetString("ConflictingOriginalRowValuesSensitive", "firstEntityType", "secondEntityType", "keyValue", "firstConflictingValues", "secondConflictingValues", "columns"), firstEntityType, secondEntityType, keyValue, firstConflictingValues, secondConflictingValues, columns);
	}

	public static string SharedRowEntryCountMismatch([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object tableName, [JetBrains.Annotations.CanBeNull] object missingEntityType, [JetBrains.Annotations.CanBeNull] object state)
	{
		return string.Format(GetString("SharedRowEntryCountMismatch", "entityType", "tableName", "missingEntityType", "state"), entityType, tableName, missingEntityType, state);
	}

	public static string SharedRowEntryCountMismatchSensitive([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object tableName, [JetBrains.Annotations.CanBeNull] object missingEntityType, [JetBrains.Annotations.CanBeNull] object keyValue, [JetBrains.Annotations.CanBeNull] object state)
	{
		return string.Format(GetString("SharedRowEntryCountMismatchSensitive", "entityType", "tableName", "missingEntityType", "keyValue", "state"), entityType, tableName, missingEntityType, keyValue, state);
	}

	public static string IncorrectDefaultValueType([JetBrains.Annotations.CanBeNull] object value, [JetBrains.Annotations.CanBeNull] object valueType, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object propertyType, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("IncorrectDefaultValueType", "value", "valueType", "property", "propertyType", "entityType"), value, valueType, property, propertyType, entityType);
	}

	public static string DuplicateColumnNameNullabilityMismatch([JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object property1, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object property2, [JetBrains.Annotations.CanBeNull] object columnName, [JetBrains.Annotations.CanBeNull] object table)
	{
		return string.Format(GetString("DuplicateColumnNameNullabilityMismatch", "entityType1", "property1", "entityType2", "property2", "columnName", "table"), entityType1, property1, entityType2, property2, columnName, table);
	}

	public static string DuplicateColumnNameComputedSqlMismatch([JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object property1, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object property2, [JetBrains.Annotations.CanBeNull] object columnName, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object value1, [JetBrains.Annotations.CanBeNull] object value2)
	{
		return string.Format(GetString("DuplicateColumnNameComputedSqlMismatch", "entityType1", "property1", "entityType2", "property2", "columnName", "table", "value1", "value2"), entityType1, property1, entityType2, property2, columnName, table, value1, value2);
	}

	public static string DuplicateColumnNameDefaultSqlMismatch([JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object property1, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object property2, [JetBrains.Annotations.CanBeNull] object columnName, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object value1, [JetBrains.Annotations.CanBeNull] object value2)
	{
		return string.Format(GetString("DuplicateColumnNameDefaultSqlMismatch", "entityType1", "property1", "entityType2", "property2", "columnName", "table", "value1", "value2"), entityType1, property1, entityType2, property2, columnName, table, value1, value2);
	}

	public static string DuplicateColumnNameCommentMismatch([JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object property1, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object property2, [JetBrains.Annotations.CanBeNull] object columnName, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object comment1, [JetBrains.Annotations.CanBeNull] object comment2)
	{
		return string.Format(GetString("DuplicateColumnNameCommentMismatch", "entityType1", "property1", "entityType2", "property2", "columnName", "table", "comment1", "comment2"), entityType1, property1, entityType2, property2, columnName, table, comment1, comment2);
	}

	public static string ConflictingColumnServerGeneration([JetBrains.Annotations.CanBeNull] object conflictingConfiguration, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object existingConfiguration)
	{
		return string.Format(GetString("ConflictingColumnServerGeneration", "conflictingConfiguration", "property", "existingConfiguration"), conflictingConfiguration, property, existingConfiguration);
	}

	public static string DuplicateCheckConstraint([JetBrains.Annotations.CanBeNull] object checkConstraint, [JetBrains.Annotations.CanBeNull] object entityType)
	{
		return string.Format(GetString("DuplicateCheckConstraint", "checkConstraint", "entityType"), checkConstraint, entityType);
	}

	public static string DuplicateForeignKeyColumnMismatch([JetBrains.Annotations.CanBeNull] object index1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object index2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object columnNames1, [JetBrains.Annotations.CanBeNull] object columnNames2)
	{
		return string.Format(GetString("DuplicateForeignKeyColumnMismatch", "index1", "entityType1", "index2", "entityType2", "table", "foreignKeyName", "columnNames1", "columnNames2"), index1, entityType1, index2, entityType2, table, foreignKeyName, columnNames1, columnNames2);
	}

	public static string DuplicateForeignKeyDeleteBehaviorMismatch([JetBrains.Annotations.CanBeNull] object index1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object index2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object deleteBehavior1, [JetBrains.Annotations.CanBeNull] object deleteBehavior2)
	{
		return string.Format(GetString("DuplicateForeignKeyDeleteBehaviorMismatch", "index1", "entityType1", "index2", "entityType2", "table", "foreignKeyName", "deleteBehavior1", "deleteBehavior2"), index1, entityType1, index2, entityType2, table, foreignKeyName, deleteBehavior1, deleteBehavior2);
	}

	public static string DuplicateForeignKeyPrincipalColumnMismatch([JetBrains.Annotations.CanBeNull] object index1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object index2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object principalColumnNames1, [JetBrains.Annotations.CanBeNull] object principalColumnNames2)
	{
		return string.Format(GetString("DuplicateForeignKeyPrincipalColumnMismatch", "index1", "entityType1", "index2", "entityType2", "table", "foreignKeyName", "principalColumnNames1", "principalColumnNames2"), index1, entityType1, index2, entityType2, table, foreignKeyName, principalColumnNames1, principalColumnNames2);
	}

	public static string DuplicateForeignKeyPrincipalTableMismatch([JetBrains.Annotations.CanBeNull] object index1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object index2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object foreignKeyName, [JetBrains.Annotations.CanBeNull] object principalTable1, [JetBrains.Annotations.CanBeNull] object principalTable2)
	{
		return string.Format(GetString("DuplicateForeignKeyPrincipalTableMismatch", "index1", "entityType1", "index2", "entityType2", "table", "foreignKeyName", "principalTable1", "principalTable2"), index1, entityType1, index2, entityType2, table, foreignKeyName, principalTable1, principalTable2);
	}

	public static string DuplicateForeignKeyUniquenessMismatch([JetBrains.Annotations.CanBeNull] object index1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object index2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object foreignKeyName)
	{
		return string.Format(GetString("DuplicateForeignKeyUniquenessMismatch", "index1", "entityType1", "index2", "entityType2", "table", "foreignKeyName"), index1, entityType1, index2, entityType2, table, foreignKeyName);
	}

	public static string DuplicateIndexColumnMismatch([JetBrains.Annotations.CanBeNull] object index1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object index2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object indexName, [JetBrains.Annotations.CanBeNull] object columnNames1, [JetBrains.Annotations.CanBeNull] object columnNames2)
	{
		return string.Format(GetString("DuplicateIndexColumnMismatch", "index1", "entityType1", "index2", "entityType2", "table", "indexName", "columnNames1", "columnNames2"), index1, entityType1, index2, entityType2, table, indexName, columnNames1, columnNames2);
	}

	public static string DuplicateIndexUniquenessMismatch([JetBrains.Annotations.CanBeNull] object index1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object index2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object indexName)
	{
		return string.Format(GetString("DuplicateIndexUniquenessMismatch", "index1", "entityType1", "index2", "entityType2", "table", "indexName"), index1, entityType1, index2, entityType2, table, indexName);
	}

	public static string DuplicateKeyColumnMismatch([JetBrains.Annotations.CanBeNull] object key1, [JetBrains.Annotations.CanBeNull] object entityType1, [JetBrains.Annotations.CanBeNull] object key2, [JetBrains.Annotations.CanBeNull] object entityType2, [JetBrains.Annotations.CanBeNull] object table, [JetBrains.Annotations.CanBeNull] object keyName, [JetBrains.Annotations.CanBeNull] object columnNames1, [JetBrains.Annotations.CanBeNull] object columnNames2)
	{
		return string.Format(GetString("DuplicateKeyColumnMismatch", "key1", "entityType1", "key2", "entityType2", "table", "keyName", "columnNames1", "columnNames2"), key1, entityType1, key2, entityType2, table, keyName, columnNames1, columnNames2);
	}

	public static string UnsupportedPropertyType([JetBrains.Annotations.CanBeNull] object entity, [JetBrains.Annotations.CanBeNull] object property, [JetBrains.Annotations.CanBeNull] object clrType)
	{
		return string.Format(GetString("UnsupportedPropertyType", "entity", "property", "clrType"), entity, property, clrType);
	}

	public static string TimeoutTooSmall([JetBrains.Annotations.CanBeNull] object seconds)
	{
		return string.Format(GetString("TimeoutTooSmall", "seconds"), seconds);
	}

	public static string TimeoutTooBig([JetBrains.Annotations.CanBeNull] object seconds)
	{
		return string.Format(GetString("TimeoutTooBig", "seconds"), seconds);
	}

	public static string DbFunctionInvalidReturnType([JetBrains.Annotations.CanBeNull] object function, [JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("DbFunctionInvalidReturnType", "function", "type"), function, type);
	}

	public static string DbFunctionNameEmpty([JetBrains.Annotations.CanBeNull] object function)
	{
		return string.Format(GetString("DbFunctionNameEmpty", "function"), function);
	}

	public static string DbFunctionInvalidParameterType([JetBrains.Annotations.CanBeNull] object parameter, [JetBrains.Annotations.CanBeNull] object function, [JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("DbFunctionInvalidParameterType", "parameter", "function", "type"), parameter, function, type);
	}

	public static string DbFunctionInvalidParameterName([JetBrains.Annotations.CanBeNull] object function, [JetBrains.Annotations.CanBeNull] object parameter)
	{
		return string.Format(GetString("DbFunctionInvalidParameterName", "function", "parameter"), function, parameter);
	}

	public static string DbFunctionGenericMethodNotSupported([JetBrains.Annotations.CanBeNull] object function)
	{
		return string.Format(GetString("DbFunctionGenericMethodNotSupported", "function"), function);
	}

	public static string DbFunctionExpressionIsNotMethodCall([JetBrains.Annotations.CanBeNull] object expression)
	{
		return string.Format(GetString("DbFunctionExpressionIsNotMethodCall", "expression"), expression);
	}

	public static string DbFunctionInvalidInstanceType([JetBrains.Annotations.CanBeNull] object function, [JetBrains.Annotations.CanBeNull] object type)
	{
		return string.Format(GetString("DbFunctionInvalidInstanceType", "function", "type"), function, type);
	}

	public static string ExpectedNonNullParameter([JetBrains.Annotations.CanBeNull] object parameter)
	{
		return string.Format(GetString("ExpectedNonNullParameter", "parameter"), parameter);
	}

	public static string DerivedTypeTable([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object baseType)
	{
		return string.Format(GetString("DerivedTypeTable", "entityType", "baseType"), entityType, baseType);
	}

	public static string RelationalCloneNotImplemented([JetBrains.Annotations.CanBeNull] object mapping)
	{
		return string.Format(GetString("RelationalCloneNotImplemented", "mapping"), mapping);
	}

	public static string CaseElseResultTypeUnexpected([JetBrains.Annotations.CanBeNull] object elseResultType, [JetBrains.Annotations.CanBeNull] object resultType)
	{
		return string.Format(GetString("CaseElseResultTypeUnexpected", "elseResultType", "resultType"), elseResultType, resultType);
	}

	public static string CaseWhenClauseResultTypeUnexpected([JetBrains.Annotations.CanBeNull] object whenResultType, [JetBrains.Annotations.CanBeNull] object resultType)
	{
		return string.Format(GetString("CaseWhenClauseResultTypeUnexpected", "whenResultType", "resultType"), whenResultType, resultType);
	}

	public static string CaseWhenClauseTestTypeUnexpected([JetBrains.Annotations.CanBeNull] object whenOperandType, [JetBrains.Annotations.CanBeNull] object expectedWhenOperandType)
	{
		return string.Format(GetString("CaseWhenClauseTestTypeUnexpected", "whenOperandType", "expectedWhenOperandType"), whenOperandType, expectedWhenOperandType);
	}

	public static string MissingConcurrencyColumn([JetBrains.Annotations.CanBeNull] object entityType, [JetBrains.Annotations.CanBeNull] object missingColumn, [JetBrains.Annotations.CanBeNull] object table)
	{
		return string.Format(GetString("MissingConcurrencyColumn", "entityType", "missingColumn", "table"), entityType, missingColumn, table);
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
