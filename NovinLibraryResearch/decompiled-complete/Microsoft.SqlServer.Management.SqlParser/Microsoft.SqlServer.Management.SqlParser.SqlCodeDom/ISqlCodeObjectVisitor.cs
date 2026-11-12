namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface ISqlCodeObjectVisitor : ISqlStatementVisitor
{
	void Visit(SqlAggregateFunctionCallExpression codeObject);

	void Visit(SqlAllAnyComparisonBooleanExpression codeObject);

	void Visit(SqlAllowPageLocksIndexOption codeObject);

	void Visit(SqlAllowRowLocksIndexOption codeObject);

	void Visit(SqlAssignment codeObject);

	void Visit(SqlAtTimeZoneExpression codeObject);

	void Visit(SqlBatch codeObject);

	void Visit(SqlBetweenBooleanExpression codeObject);

	void Visit(SqlBinaryBooleanExpression codeObject);

	void Visit(SqlBinaryFilterExpression codeObject);

	void Visit(SqlBinaryQueryExpression codeObject);

	void Visit(SqlBinaryScalarExpression codeObject);

	void Visit(SqlBooleanExpression codeObject);

	void Visit(SqlBooleanFilterExpression codeObject);

	void Visit(SqlBuiltinScalarFunctionCallExpression codeObject);

	void Visit(SqlCastExpression codeObject);

	void Visit(SqlChangeTrackingContext codeObject);

	void Visit(SqlCheckConstraint codeObject);

	void Visit(SqlClrAssemblySpecifier codeObject);

	void Visit(SqlClrClassSpecifier codeObject);

	void Visit(SqlClrFunctionBodyDefinition codeObject);

	void Visit(SqlClrMethodSpecifier codeObject);

	void Visit(SqlCollateScalarExpression codeObject);

	void Visit(SqlCollation codeObject);

	void Visit(SqlColumnAssignment codeObject);

	void Visit(SqlDefaultConstraint codeObject);

	void Visit(SqlColumnDefinition codeObject);

	void Visit(SqlColumnIdentity codeObject);

	void Visit(SqlColumnRefExpression codeObject);

	void Visit(SqlCommonTableExpression codeObject);

	void Visit(SqlComparisonBooleanExpression codeObject);

	void Visit(SqlCompressionPartitionRange codeObject);

	void Visit(SqlComputedColumnDefinition codeObject);

	void Visit(SqlConditionClause codeObject);

	void Visit(SqlConstraint codeObject);

	void Visit(SqlConvertExpression codeObject);

	void Visit(SqlCreateUserOption codeObject);

	void Visit(SqlCubeGroupByItem codeObject);

	void Visit(SqlCursorOption codeObject);

	void Visit(SqlCursorVariableAssignment codeObject);

	void Visit(SqlCursorVariableRefExpression codeObject);

	void Visit(SqlDataCompressionIndexOption codeObject);

	void Visit(SqlDataType codeObject);

	void Visit(SqlDataTypeSpecification codeObject);

	void Visit(SqlDdlTriggerDefinition codeObject);

	void Visit(SqlDefaultValuesInsertMergeActionSource codeObject);

	void Visit(SqlDefaultValuesInsertSource codeObject);

	void Visit(SqlDeleteMergeAction codeObject);

	void Visit(SqlDeleteSpecification codeObject);

	void Visit(SqlDerivedTableExpression codeObject);

	void Visit(SqlDmlSpecificationTableSource codeObject);

	void Visit(SqlDmlTriggerDefinition codeObject);

	void Visit(SqlDropExistingIndexOption codeObject);

	void Visit(SqlExecuteArgument codeObject);

	void Visit(SqlExecuteAsClause codeObject);

	void Visit(SqlExistsBooleanExpression codeObject);

	void Visit(SqlFillFactorIndexOption codeObject);

	void Visit(SqlFilterClause codeObject);

	void Visit(SqlForBrowseClause codeObject);

	void Visit(SqlForeignKeyConstraint codeObject);

	void Visit(SqlForXmlAutoClause codeObject);

	void Visit(SqlForXmlClause codeObject);

	void Visit(SqlForXmlDirective codeObject);

	void Visit(SqlForXmlExplicitClause codeObject);

	void Visit(SqlForXmlPathClause codeObject);

	void Visit(SqlForXmlRawClause codeObject);

	void Visit(SqlFromClause codeObject);

	void Visit(SqlFullTextBooleanExpression codeObject);

	void Visit(SqlFullTextColumn codeObject);

	void Visit(SqlFunctionDefinition codeObject);

	void Visit(SqlGlobalScalarVariableRefExpression codeObject);

	void Visit(SqlGrandTotalGroupByItem codeObject);

	void Visit(SqlGrandTotalGroupingSet codeObject);

	void Visit(SqlGroupByClause codeObject);

	void Visit(SqlGroupBySets codeObject);

	void Visit(SqlGroupingSetItemsCollection codeObject);

	void Visit(SqlHavingClause codeObject);

	void Visit(SqlIdentifier codeObject);

	void Visit(SqlIdentityFunctionCallExpression codeObject);

	void Visit(SqlIgnoreDupKeyIndexOption codeObject);

	void Visit(SqlInBooleanExpression codeObject);

	void Visit(SqlInBooleanExpressionCollectionValue codeObject);

	void Visit(SqlInBooleanExpressionQueryValue codeObject);

	void Visit(SqlIndexedColumn codeObject);

	void Visit(SqlIndexHint codeObject);

	void Visit(SqlIndexOption codeObject);

	void Visit(SqlInlineIndexConstraint codeObject);

	void Visit(SqlInlineFunctionBodyDefinition codeObject);

	void Visit(SqlInlineTableRelationalFunctionDefinition codeObject);

	void Visit(SqlInlineTableVariableDeclaration codeObject);

	void Visit(SqlInsertMergeAction codeObject);

	void Visit(SqlInsertSpecification codeObject);

	void Visit(SqlIntoClause codeObject);

	void Visit(SqlIsNullBooleanExpression codeObject);

	void Visit(SqlLargeDataStorageInformation codeObject);

	void Visit(SqlLikeBooleanExpression codeObject);

	void Visit(SqlLiteralExpression codeObject);

	void Visit(SqlLoginPassword codeObject);

	void Visit(SqlMaxDegreeOfParallelismIndexOption codeObject);

	void Visit(SqlMergeActionClause codeObject);

	void Visit(SqlMergeSpecification codeObject);

	void Visit(SqlInsertSource codeObject);

	void Visit(SqlModuleCalledOnNullInputOption codeObject);

	void Visit(SqlModuleEncryptionOption codeObject);

	void Visit(SqlModuleExecuteAsOption codeObject);

	void Visit(SqlModuleNativeCompilationOption codeObject);

	void Visit(SqlModuleOption codeObject);

	void Visit(SqlModuleRecompileOption codeObject);

	void Visit(SqlModuleReturnsNullOnNullInputOption codeObject);

	void Visit(SqlModuleSchemaBindingOption codeObject);

	void Visit(SqlModuleViewMetadataOption codeObject);

	void Visit(SqlMultistatementFunctionBodyDefinition codeObject);

	void Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject);

	void Visit(SqlNotBooleanExpression codeObject);

	void Visit(SqlQueryExpression codeObject);

	void Visit(SqlScalarExpression codeObject);

	void Visit(SqlTableExpression codeObject);

	void Visit(SqlObjectIdentifier codeObject);

	void Visit(SqlObjectReference codeObject);

	void Visit(SqlOnlineIndexOption codeObject);

	void Visit(SqlOptimizeForSequentialKeyIndexOption codeObject);

	void Visit(SqlResumableIndexOption codeObject);

	void Visit(SqlBucketCountIndexOption codeObject);

	void Visit(SqlCompressionDelayIndexOption codeObject);

	void Visit(SqlMaxDurationIndexOption codeObject);

	void Visit(SqlOffsetFetchClause codeObject);

	void Visit(SqlOrderByClause codeObject);

	void Visit(SqlOrderByItem codeObject);

	void Visit(SqlOutputClause codeObject);

	void Visit(SqlOutputIntoClause codeObject);

	void Visit(SqlPadIndexOption codeObject);

	void Visit(SqlParameterDeclaration codeObject);

	void Visit(SqlPivotClause codeObject);

	void Visit(SqlPivotTableExpression codeObject);

	void Visit(SqlPrimaryKeyConstraint codeObject);

	void Visit(SqlStorageSpecification codeObject);

	void Visit(SqlProcedureDefinition codeObject);

	void Visit(SqlQualifiedJoinTableExpression codeObject);

	void Visit(SqlQuerySpecification codeObject);

	void Visit(SqlQueryWithClause codeObject);

	void Visit(SqlRollupGroupByItem codeObject);

	void Visit(SqlRowConstructorExpression codeObject);

	void Visit(SqlScalarClrFunctionDefinition codeObject);

	void Visit(SqlScalarFunctionReturnType codeObject);

	void Visit(SqlScalarRefExpression codeObject);

	void Visit(SqlScalarRelationalFunctionDefinition codeObject);

	void Visit(SqlScalarSubQueryExpression codeObject);

	void Visit(SqlScalarVariableAssignment codeObject);

	void Visit(SqlScalarVariableRefExpression codeObject);

	void Visit(SqlScript codeObject);

	void Visit(SqlSearchedCaseExpression codeObject);

	void Visit(SqlSearchedWhenClause codeObject);

	void Visit(SqlSelectClause codeObject);

	void Visit(SqlSelectIntoClause codeObject);

	void Visit(SqlSelectScalarExpression codeObject);

	void Visit(SqlSelectSpecification codeObject);

	void Visit(SqlSelectSpecificationInsertSource codeObject);

	void Visit(SqlSelectStarExpression codeObject);

	void Visit(SqlSelectVariableAssignmentExpression codeObject);

	void Visit(SqlSetClause codeObject);

	void Visit(SqlSimpleCaseExpression codeObject);

	void Visit(SqlSimpleGroupByItem codeObject);

	void Visit(SqlSimpleOrderByClause codeObject);

	void Visit(SqlSimpleOrderByItem codeObject);

	void Visit(SqlSimpleWhenClause codeObject);

	void Visit(SqlSortedDataIndexOption codeObject);

	void Visit(SqlSortedDataReorgIndexOption codeObject);

	void Visit(SqlSortInTempDbIndexOption codeObject);

	void Visit(SqlStatisticsIncrementalIndexOption codeObject);

	void Visit(SqlStatisticsNoRecomputeIndexOption codeObject);

	void Visit(SqlStatisticsOnlyIndexOption codeObject);

	void Visit(SqlTableClrFunctionDefinition codeObject);

	void Visit(SqlTableConstructorExpression codeObject);

	void Visit(SqlTableConstructorInsertSource codeObject);

	void Visit(SqlTableDefinition codeObject);

	void Visit(SqlTableFunctionReturnType codeObject);

	void Visit(SqlTableHint codeObject);

	void Visit(SqlTableRefExpression codeObject);

	void Visit(SqlTableValuedFunctionRefExpression codeObject);

	void Visit(SqlTableVariableRefExpression codeObject);

	void Visit(SqlTableUdtInstanceMethodExpression codeObject);

	void Visit(SqlTargetTableExpression codeObject);

	void Visit(SqlTemporalPeriodDefinition codeObject);

	void Visit(SqlTopSpecification codeObject);

	void Visit(SqlTriggerAction codeObject);

	void Visit(SqlTriggerDefinition codeObject);

	void Visit(SqlTriggerEvent codeObject);

	void Visit(SqlUdtInstanceDataMemberExpression codeObject);

	void Visit(SqlUdtInstanceMethodExpression codeObject);

	void Visit(SqlUdtStaticDataMemberExpression codeObject);

	void Visit(SqlUdtStaticMethodExpression codeObject);

	void Visit(SqlUnaryScalarExpression codeObject);

	void Visit(SqlUniqueConstraint codeObject);

	void Visit(SqlUnpivotClause codeObject);

	void Visit(SqlUnpivotTableExpression codeObject);

	void Visit(SqlUnqualifiedJoinTableExpression codeObject);

	void Visit(SqlUpdateBooleanExpression codeObject);

	void Visit(SqlUpdateMergeAction codeObject);

	void Visit(SqlUpdateSpecification codeObject);

	void Visit(SqlUserDefinedScalarFunctionCallExpression codeObject);

	void Visit(SqlValuesInsertMergeActionSource codeObject);

	void Visit(SqlVariableColumnAssignment codeObject);

	void Visit(SqlVariableDeclaration codeObject);

	void Visit(SqlViewDefinition codeObject);

	void Visit(SqlWhereClause codeObject);

	void Visit(SqlXmlNamespacesDeclaration codeObject);
}
public interface ISqlCodeObjectVisitor<T> : ISqlStatementVisitor<T>
{
	T Visit(SqlAggregateFunctionCallExpression codeObject);

	T Visit(SqlAllAnyComparisonBooleanExpression codeObject);

	T Visit(SqlAllowPageLocksIndexOption codeObject);

	T Visit(SqlAllowRowLocksIndexOption codeObject);

	T Visit(SqlAssignment codeObject);

	T Visit(SqlAtTimeZoneExpression codeObject);

	T Visit(SqlBatch codeObject);

	T Visit(SqlBetweenBooleanExpression codeObject);

	T Visit(SqlBinaryBooleanExpression codeObject);

	T Visit(SqlBinaryFilterExpression codeObject);

	T Visit(SqlBinaryQueryExpression codeObject);

	T Visit(SqlBinaryScalarExpression codeObject);

	T Visit(SqlBooleanExpression codeObject);

	T Visit(SqlBooleanFilterExpression codeObject);

	T Visit(SqlBuiltinScalarFunctionCallExpression codeObject);

	T Visit(SqlCastExpression codeObject);

	T Visit(SqlChangeTrackingContext codeObject);

	T Visit(SqlCheckConstraint codeObject);

	T Visit(SqlClrAssemblySpecifier codeObject);

	T Visit(SqlClrClassSpecifier codeObject);

	T Visit(SqlClrFunctionBodyDefinition codeObject);

	T Visit(SqlClrMethodSpecifier codeObject);

	T Visit(SqlCollateScalarExpression codeObject);

	T Visit(SqlCollation codeObject);

	T Visit(SqlColumnAssignment codeObject);

	T Visit(SqlDefaultConstraint codeObject);

	T Visit(SqlColumnDefinition codeObject);

	T Visit(SqlColumnIdentity codeObject);

	T Visit(SqlColumnRefExpression codeObject);

	T Visit(SqlCommonTableExpression codeObject);

	T Visit(SqlComparisonBooleanExpression codeObject);

	T Visit(SqlCompressionPartitionRange codeObject);

	T Visit(SqlComputedColumnDefinition codeObject);

	T Visit(SqlConditionClause codeObject);

	T Visit(SqlConstraint codeObject);

	T Visit(SqlConvertExpression codeObject);

	T Visit(SqlCreateUserOption codeObject);

	T Visit(SqlCubeGroupByItem codeObject);

	T Visit(SqlCursorOption codeObject);

	T Visit(SqlCursorVariableAssignment codeObject);

	T Visit(SqlCursorVariableRefExpression codeObject);

	T Visit(SqlDataCompressionIndexOption codeObject);

	T Visit(SqlDataType codeObject);

	T Visit(SqlDataTypeSpecification codeObject);

	T Visit(SqlDdlTriggerDefinition codeObject);

	T Visit(SqlDefaultValuesInsertMergeActionSource codeObject);

	T Visit(SqlDefaultValuesInsertSource codeObject);

	T Visit(SqlDeleteMergeAction codeObject);

	T Visit(SqlDeleteSpecification codeObject);

	T Visit(SqlDerivedTableExpression codeObject);

	T Visit(SqlDmlSpecificationTableSource codeObject);

	T Visit(SqlDmlTriggerDefinition codeObject);

	T Visit(SqlDropExistingIndexOption codeObject);

	T Visit(SqlExecuteArgument codeObject);

	T Visit(SqlExecuteAsClause codeObject);

	T Visit(SqlExistsBooleanExpression codeObject);

	T Visit(SqlFillFactorIndexOption codeObject);

	T Visit(SqlFilterClause codeObject);

	T Visit(SqlForBrowseClause codeObject);

	T Visit(SqlForeignKeyConstraint codeObject);

	T Visit(SqlForXmlAutoClause codeObject);

	T Visit(SqlForXmlClause codeObject);

	T Visit(SqlForXmlDirective codeObject);

	T Visit(SqlForXmlExplicitClause codeObject);

	T Visit(SqlForXmlPathClause codeObject);

	T Visit(SqlForXmlRawClause codeObject);

	T Visit(SqlFromClause codeObject);

	T Visit(SqlFullTextBooleanExpression codeObject);

	T Visit(SqlFullTextColumn codeObject);

	T Visit(SqlFunctionDefinition codeObject);

	T Visit(SqlGlobalScalarVariableRefExpression codeObject);

	T Visit(SqlGrandTotalGroupByItem codeObject);

	T Visit(SqlGrandTotalGroupingSet codeObject);

	T Visit(SqlGroupByClause codeObject);

	T Visit(SqlGroupBySets codeObject);

	T Visit(SqlGroupingSetItemsCollection codeObject);

	T Visit(SqlHavingClause codeObject);

	T Visit(SqlIdentifier codeObject);

	T Visit(SqlIdentityFunctionCallExpression codeObject);

	T Visit(SqlIgnoreDupKeyIndexOption codeObject);

	T Visit(SqlInBooleanExpression codeObject);

	T Visit(SqlInBooleanExpressionCollectionValue codeObject);

	T Visit(SqlInBooleanExpressionQueryValue codeObject);

	T Visit(SqlIndexedColumn codeObject);

	T Visit(SqlIndexHint codeObject);

	T Visit(SqlIndexOption codeObject);

	T Visit(SqlInlineIndexConstraint codeObject);

	T Visit(SqlInlineFunctionBodyDefinition codeObject);

	T Visit(SqlInlineTableRelationalFunctionDefinition codeObject);

	T Visit(SqlInlineTableVariableDeclaration codeObject);

	T Visit(SqlInsertMergeAction codeObject);

	T Visit(SqlInsertSpecification codeObject);

	T Visit(SqlIntoClause codeObject);

	T Visit(SqlIsNullBooleanExpression codeObject);

	T Visit(SqlLargeDataStorageInformation codeObject);

	T Visit(SqlLikeBooleanExpression codeObject);

	T Visit(SqlLiteralExpression codeObject);

	T Visit(SqlLoginPassword codeObject);

	T Visit(SqlMaxDegreeOfParallelismIndexOption codeObject);

	T Visit(SqlMergeActionClause codeObject);

	T Visit(SqlMergeSpecification codeObject);

	T Visit(SqlInsertSource codeObject);

	T Visit(SqlModuleCalledOnNullInputOption codeObject);

	T Visit(SqlModuleEncryptionOption codeObject);

	T Visit(SqlModuleExecuteAsOption codeObject);

	T Visit(SqlModuleNativeCompilationOption codeObject);

	T Visit(SqlModuleOption codeObject);

	T Visit(SqlModuleRecompileOption codeObject);

	T Visit(SqlModuleReturnsNullOnNullInputOption codeObject);

	T Visit(SqlModuleSchemaBindingOption codeObject);

	T Visit(SqlModuleViewMetadataOption codeObject);

	T Visit(SqlMultistatementFunctionBodyDefinition codeObject);

	T Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject);

	T Visit(SqlNotBooleanExpression codeObject);

	T Visit(SqlQueryExpression codeObject);

	T Visit(SqlScalarExpression codeObject);

	T Visit(SqlTableExpression codeObject);

	T Visit(SqlObjectIdentifier codeObject);

	T Visit(SqlObjectReference codeObject);

	T Visit(SqlOnlineIndexOption codeObject);

	T Visit(SqlOptimizeForSequentialKeyIndexOption codeObject);

	T Visit(SqlResumableIndexOption codeObject);

	T Visit(SqlBucketCountIndexOption codeObject);

	T Visit(SqlCompressionDelayIndexOption codeObject);

	T Visit(SqlMaxDurationIndexOption codeObject);

	T Visit(SqlOffsetFetchClause codeObject);

	T Visit(SqlOrderByClause codeObject);

	T Visit(SqlOrderByItem codeObject);

	T Visit(SqlOutputClause codeObject);

	T Visit(SqlOutputIntoClause codeObject);

	T Visit(SqlPadIndexOption codeObject);

	T Visit(SqlParameterDeclaration codeObject);

	T Visit(SqlPivotClause codeObject);

	T Visit(SqlPivotTableExpression codeObject);

	T Visit(SqlPrimaryKeyConstraint codeObject);

	T Visit(SqlStorageSpecification codeObject);

	T Visit(SqlProcedureDefinition codeObject);

	T Visit(SqlQualifiedJoinTableExpression codeObject);

	T Visit(SqlQuerySpecification codeObject);

	T Visit(SqlQueryWithClause codeObject);

	T Visit(SqlRollupGroupByItem codeObject);

	T Visit(SqlRowConstructorExpression codeObject);

	T Visit(SqlScalarClrFunctionDefinition codeObject);

	T Visit(SqlScalarFunctionReturnType codeObject);

	T Visit(SqlScalarRefExpression codeObject);

	T Visit(SqlScalarRelationalFunctionDefinition codeObject);

	T Visit(SqlScalarSubQueryExpression codeObject);

	T Visit(SqlScalarVariableAssignment codeObject);

	T Visit(SqlScalarVariableRefExpression codeObject);

	T Visit(SqlScript codeObject);

	T Visit(SqlSearchedCaseExpression codeObject);

	T Visit(SqlSearchedWhenClause codeObject);

	T Visit(SqlSelectClause codeObject);

	T Visit(SqlSelectIntoClause codeObject);

	T Visit(SqlSelectScalarExpression codeObject);

	T Visit(SqlSelectSpecification codeObject);

	T Visit(SqlSelectSpecificationInsertSource codeObject);

	T Visit(SqlSelectStarExpression codeObject);

	T Visit(SqlSelectVariableAssignmentExpression codeObject);

	T Visit(SqlSetClause codeObject);

	T Visit(SqlSimpleCaseExpression codeObject);

	T Visit(SqlSimpleGroupByItem codeObject);

	T Visit(SqlSimpleOrderByClause codeObject);

	T Visit(SqlSimpleOrderByItem codeObject);

	T Visit(SqlSimpleWhenClause codeObject);

	T Visit(SqlSortedDataIndexOption codeObject);

	T Visit(SqlSortedDataReorgIndexOption codeObject);

	T Visit(SqlSortInTempDbIndexOption codeObject);

	T Visit(SqlStatisticsIncrementalIndexOption codeObject);

	T Visit(SqlStatisticsNoRecomputeIndexOption codeObject);

	T Visit(SqlStatisticsOnlyIndexOption codeObject);

	T Visit(SqlTableClrFunctionDefinition codeObject);

	T Visit(SqlTableConstructorExpression codeObject);

	T Visit(SqlTableConstructorInsertSource codeObject);

	T Visit(SqlTableDefinition codeObject);

	T Visit(SqlTableFunctionReturnType codeObject);

	T Visit(SqlTableHint codeObject);

	T Visit(SqlTableRefExpression codeObject);

	T Visit(SqlTableValuedFunctionRefExpression codeObject);

	T Visit(SqlTableVariableRefExpression codeObject);

	T Visit(SqlTableUdtInstanceMethodExpression codeObject);

	T Visit(SqlTargetTableExpression codeObject);

	T Visit(SqlTemporalPeriodDefinition codeObject);

	T Visit(SqlTopSpecification codeObject);

	T Visit(SqlTriggerAction codeObject);

	T Visit(SqlTriggerDefinition codeObject);

	T Visit(SqlTriggerEvent codeObject);

	T Visit(SqlUdtInstanceDataMemberExpression codeObject);

	T Visit(SqlUdtInstanceMethodExpression codeObject);

	T Visit(SqlUdtStaticDataMemberExpression codeObject);

	T Visit(SqlUdtStaticMethodExpression codeObject);

	T Visit(SqlUnaryScalarExpression codeObject);

	T Visit(SqlUniqueConstraint codeObject);

	T Visit(SqlUnpivotClause codeObject);

	T Visit(SqlUnpivotTableExpression codeObject);

	T Visit(SqlUnqualifiedJoinTableExpression codeObject);

	T Visit(SqlUpdateBooleanExpression codeObject);

	T Visit(SqlUpdateMergeAction codeObject);

	T Visit(SqlUpdateSpecification codeObject);

	T Visit(SqlUserDefinedScalarFunctionCallExpression codeObject);

	T Visit(SqlValuesInsertMergeActionSource codeObject);

	T Visit(SqlVariableColumnAssignment codeObject);

	T Visit(SqlVariableDeclaration codeObject);

	T Visit(SqlViewDefinition codeObject);

	T Visit(SqlWhereClause codeObject);

	T Visit(SqlXmlNamespacesDeclaration codeObject);
}
