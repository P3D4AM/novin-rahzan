namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface ISqlCodeObjectContextVisitor<C> : ISqlStatementContextVisitor<C>
{
	void Visit(SqlAggregateFunctionCallExpression codeObject, C context);

	void Visit(SqlAllAnyComparisonBooleanExpression codeObject, C context);

	void Visit(SqlAllowPageLocksIndexOption codeObject, C context);

	void Visit(SqlAllowRowLocksIndexOption codeObject, C context);

	void Visit(SqlAssignment codeObject, C context);

	void Visit(SqlAtTimeZoneExpression codeObject, C context);

	void Visit(SqlBatch codeObject, C context);

	void Visit(SqlBetweenBooleanExpression codeObject, C context);

	void Visit(SqlBinaryBooleanExpression codeObject, C context);

	void Visit(SqlBinaryFilterExpression codeObject, C context);

	void Visit(SqlBinaryQueryExpression codeObject, C context);

	void Visit(SqlBinaryScalarExpression codeObject, C context);

	void Visit(SqlBooleanExpression codeObject, C context);

	void Visit(SqlBooleanFilterExpression codeObject, C context);

	void Visit(SqlBuiltinScalarFunctionCallExpression codeObject, C context);

	void Visit(SqlCastExpression codeObject, C context);

	void Visit(SqlChangeTrackingContext codeObject, C context);

	void Visit(SqlCheckConstraint codeObject, C context);

	void Visit(SqlClrAssemblySpecifier codeObject, C context);

	void Visit(SqlClrClassSpecifier codeObject, C context);

	void Visit(SqlClrFunctionBodyDefinition codeObject, C context);

	void Visit(SqlClrMethodSpecifier codeObject, C context);

	void Visit(SqlCollateScalarExpression codeObject, C context);

	void Visit(SqlCollation codeObject, C context);

	void Visit(SqlColumnAssignment codeObject, C context);

	void Visit(SqlDefaultConstraint codeObject, C context);

	void Visit(SqlColumnDefinition codeObject, C context);

	void Visit(SqlColumnIdentity codeObject, C context);

	void Visit(SqlColumnRefExpression codeObject, C context);

	void Visit(SqlCommonTableExpression codeObject, C context);

	void Visit(SqlComparisonBooleanExpression codeObject, C context);

	void Visit(SqlCompressionPartitionRange codeObject, C context);

	void Visit(SqlComputedColumnDefinition codeObject, C context);

	void Visit(SqlConditionClause codeObject, C context);

	void Visit(SqlConstraint codeObject, C context);

	void Visit(SqlConvertExpression codeObject, C context);

	void Visit(SqlCreateUserOption codeObject, C context);

	void Visit(SqlCubeGroupByItem codeObject, C context);

	void Visit(SqlCursorOption codeObject, C context);

	void Visit(SqlCursorVariableAssignment codeObject, C context);

	void Visit(SqlCursorVariableRefExpression codeObject, C context);

	void Visit(SqlDataCompressionIndexOption codeObject, C context);

	void Visit(SqlDataType codeObject, C context);

	void Visit(SqlDataTypeSpecification codeObject, C context);

	void Visit(SqlDdlTriggerDefinition codeObject, C context);

	void Visit(SqlDefaultValuesInsertMergeActionSource codeObject, C context);

	void Visit(SqlDefaultValuesInsertSource codeObject, C context);

	void Visit(SqlDeleteMergeAction codeObject, C context);

	void Visit(SqlDeleteSpecification codeObject, C context);

	void Visit(SqlDerivedTableExpression codeObject, C context);

	void Visit(SqlDmlSpecificationTableSource codeObject, C context);

	void Visit(SqlDmlTriggerDefinition codeObject, C context);

	void Visit(SqlDropExistingIndexOption codeObject, C context);

	void Visit(SqlExecuteArgument codeObject, C context);

	void Visit(SqlExecuteAsClause codeObject, C context);

	void Visit(SqlExistsBooleanExpression codeObject, C context);

	void Visit(SqlFillFactorIndexOption codeObject, C context);

	void Visit(SqlFilterClause codeObject, C context);

	void Visit(SqlForBrowseClause codeObject, C context);

	void Visit(SqlForeignKeyConstraint codeObject, C context);

	void Visit(SqlForXmlAutoClause codeObject, C context);

	void Visit(SqlForXmlClause codeObject, C context);

	void Visit(SqlForXmlDirective codeObject, C context);

	void Visit(SqlForXmlExplicitClause codeObject, C context);

	void Visit(SqlForXmlPathClause codeObject, C context);

	void Visit(SqlForXmlRawClause codeObject, C context);

	void Visit(SqlFromClause codeObject, C context);

	void Visit(SqlFullTextBooleanExpression codeObject, C context);

	void Visit(SqlFullTextColumn codeObject, C context);

	void Visit(SqlFunctionDefinition codeObject, C context);

	void Visit(SqlGlobalScalarVariableRefExpression codeObject, C context);

	void Visit(SqlGrandTotalGroupByItem codeObject, C context);

	void Visit(SqlGrandTotalGroupingSet codeObject, C context);

	void Visit(SqlGroupByClause codeObject, C context);

	void Visit(SqlGroupBySets codeObject, C context);

	void Visit(SqlGroupingSetItemsCollection codeObject, C context);

	void Visit(SqlHavingClause codeObject, C context);

	void Visit(SqlIdentifier codeObject, C context);

	void Visit(SqlIdentityFunctionCallExpression codeObject, C context);

	void Visit(SqlIgnoreDupKeyIndexOption codeObject, C context);

	void Visit(SqlInBooleanExpression codeObject, C context);

	void Visit(SqlInBooleanExpressionCollectionValue codeObject, C context);

	void Visit(SqlInBooleanExpressionQueryValue codeObject, C context);

	void Visit(SqlIndexedColumn codeObject, C context);

	void Visit(SqlIndexHint codeObject, C context);

	void Visit(SqlIndexOption codeObject, C context);

	void Visit(SqlInlineIndexConstraint codeObject, C context);

	void Visit(SqlInlineFunctionBodyDefinition codeObject, C context);

	void Visit(SqlInlineTableRelationalFunctionDefinition codeObject, C context);

	void Visit(SqlInlineTableVariableDeclaration codeObject, C context);

	void Visit(SqlInsertMergeAction codeObject, C context);

	void Visit(SqlInsertSpecification codeObject, C context);

	void Visit(SqlIntoClause codeObject, C context);

	void Visit(SqlIsNullBooleanExpression codeObject, C context);

	void Visit(SqlLargeDataStorageInformation codeObject, C context);

	void Visit(SqlLikeBooleanExpression codeObject, C context);

	void Visit(SqlLiteralExpression codeObject, C context);

	void Visit(SqlLoginPassword codeObject, C context);

	void Visit(SqlMaxDegreeOfParallelismIndexOption codeObject, C context);

	void Visit(SqlMergeActionClause codeObject, C context);

	void Visit(SqlMergeSpecification codeObject, C context);

	void Visit(SqlInsertSource codeObject, C context);

	void Visit(SqlModuleCalledOnNullInputOption codeObject, C context);

	void Visit(SqlModuleEncryptionOption codeObject, C context);

	void Visit(SqlModuleExecuteAsOption codeObject, C context);

	void Visit(SqlModuleNativeCompilationOption codeObject, C context);

	void Visit(SqlModuleOption codeObject, C context);

	void Visit(SqlModuleRecompileOption codeObject, C context);

	void Visit(SqlModuleReturnsNullOnNullInputOption codeObject, C context);

	void Visit(SqlModuleSchemaBindingOption codeObject, C context);

	void Visit(SqlModuleViewMetadataOption codeObject, C context);

	void Visit(SqlMultistatementFunctionBodyDefinition codeObject, C context);

	void Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject, C context);

	void Visit(SqlNotBooleanExpression codeObject, C context);

	void Visit(SqlQueryExpression codeObject, C context);

	void Visit(SqlScalarExpression codeObject, C context);

	void Visit(SqlTableExpression codeObject, C context);

	void Visit(SqlObjectIdentifier codeObject, C context);

	void Visit(SqlObjectReference codeObject, C context);

	void Visit(SqlOnlineIndexOption codeObject, C context);

	void Visit(SqlOptimizeForSequentialKeyIndexOption codeObject, C context);

	void Visit(SqlResumableIndexOption codeObject, C context);

	void Visit(SqlBucketCountIndexOption codeObject, C context);

	void Visit(SqlCompressionDelayIndexOption codeObject, C context);

	void Visit(SqlMaxDurationIndexOption codeObject, C context);

	void Visit(SqlOffsetFetchClause codeObject, C context);

	void Visit(SqlOrderByClause codeObject, C context);

	void Visit(SqlOrderByItem codeObject, C context);

	void Visit(SqlOutputClause codeObject, C context);

	void Visit(SqlOutputIntoClause codeObject, C context);

	void Visit(SqlPadIndexOption codeObject, C context);

	void Visit(SqlParameterDeclaration codeObject, C context);

	void Visit(SqlPivotClause codeObject, C context);

	void Visit(SqlPivotTableExpression codeObject, C context);

	void Visit(SqlPrimaryKeyConstraint codeObject, C context);

	void Visit(SqlStorageSpecification codeObject, C context);

	void Visit(SqlProcedureDefinition codeObject, C context);

	void Visit(SqlQualifiedJoinTableExpression codeObject, C context);

	void Visit(SqlQuerySpecification codeObject, C context);

	void Visit(SqlQueryWithClause codeObject, C context);

	void Visit(SqlRollupGroupByItem codeObject, C context);

	void Visit(SqlRowConstructorExpression codeObject, C context);

	void Visit(SqlScalarClrFunctionDefinition codeObject, C context);

	void Visit(SqlScalarFunctionReturnType codeObject, C context);

	void Visit(SqlScalarRefExpression codeObject, C context);

	void Visit(SqlScalarRelationalFunctionDefinition codeObject, C context);

	void Visit(SqlScalarSubQueryExpression codeObject, C context);

	void Visit(SqlScalarVariableAssignment codeObject, C context);

	void Visit(SqlScalarVariableRefExpression codeObject, C context);

	void Visit(SqlScript codeObject, C context);

	void Visit(SqlSearchedCaseExpression codeObject, C context);

	void Visit(SqlSearchedWhenClause codeObject, C context);

	void Visit(SqlSelectClause codeObject, C context);

	void Visit(SqlSelectIntoClause codeObject, C context);

	void Visit(SqlSelectScalarExpression codeObject, C context);

	void Visit(SqlSelectSpecification codeObject, C context);

	void Visit(SqlSelectSpecificationInsertSource codeObject, C context);

	void Visit(SqlSelectStarExpression codeObject, C context);

	void Visit(SqlSelectVariableAssignmentExpression codeObject, C context);

	void Visit(SqlSetClause codeObject, C context);

	void Visit(SqlSimpleCaseExpression codeObject, C context);

	void Visit(SqlSimpleGroupByItem codeObject, C context);

	void Visit(SqlSimpleOrderByClause codeObject, C context);

	void Visit(SqlSimpleOrderByItem codeObject, C context);

	void Visit(SqlSimpleWhenClause codeObject, C context);

	void Visit(SqlSortedDataIndexOption codeObject, C context);

	void Visit(SqlSortedDataReorgIndexOption codeObject, C context);

	void Visit(SqlSortInTempDbIndexOption codeObject, C context);

	void Visit(SqlStatisticsIncrementalIndexOption codeObject, C context);

	void Visit(SqlStatisticsNoRecomputeIndexOption codeObject, C context);

	void Visit(SqlStatisticsOnlyIndexOption codeObject, C context);

	void Visit(SqlTableClrFunctionDefinition codeObject, C context);

	void Visit(SqlTableConstructorExpression codeObject, C context);

	void Visit(SqlTableConstructorInsertSource codeObject, C context);

	void Visit(SqlTableDefinition codeObject, C context);

	void Visit(SqlTableFunctionReturnType codeObject, C context);

	void Visit(SqlTableHint codeObject, C context);

	void Visit(SqlTableRefExpression codeObject, C context);

	void Visit(SqlTableValuedFunctionRefExpression codeObject, C context);

	void Visit(SqlTableVariableRefExpression codeObject, C context);

	void Visit(SqlTableUdtInstanceMethodExpression codeObject, C context);

	void Visit(SqlTargetTableExpression codeObject, C context);

	void Visit(SqlTemporalPeriodDefinition codeObject, C context);

	void Visit(SqlTopSpecification codeObject, C context);

	void Visit(SqlTriggerAction codeObject, C context);

	void Visit(SqlTriggerDefinition codeObject, C context);

	void Visit(SqlTriggerEvent codeObject, C context);

	void Visit(SqlUdtInstanceDataMemberExpression codeObject, C context);

	void Visit(SqlUdtInstanceMethodExpression codeObject, C context);

	void Visit(SqlUdtStaticDataMemberExpression codeObject, C context);

	void Visit(SqlUdtStaticMethodExpression codeObject, C context);

	void Visit(SqlUnaryScalarExpression codeObject, C context);

	void Visit(SqlUniqueConstraint codeObject, C context);

	void Visit(SqlUnpivotClause codeObject, C context);

	void Visit(SqlUnpivotTableExpression codeObject, C context);

	void Visit(SqlUnqualifiedJoinTableExpression codeObject, C context);

	void Visit(SqlUpdateBooleanExpression codeObject, C context);

	void Visit(SqlUpdateMergeAction codeObject, C context);

	void Visit(SqlUpdateSpecification codeObject, C context);

	void Visit(SqlUserDefinedScalarFunctionCallExpression codeObject, C context);

	void Visit(SqlValuesInsertMergeActionSource codeObject, C context);

	void Visit(SqlVariableColumnAssignment codeObject, C context);

	void Visit(SqlVariableDeclaration codeObject, C context);

	void Visit(SqlViewDefinition codeObject, C context);

	void Visit(SqlWhereClause codeObject, C context);

	void Visit(SqlXmlNamespacesDeclaration codeObject, C context);
}
public interface ISqlCodeObjectContextVisitor<T, C> : ISqlStatementContextVisitor<T, C>
{
	T Visit(SqlAggregateFunctionCallExpression codeObject, C context);

	T Visit(SqlAllAnyComparisonBooleanExpression codeObject, C context);

	T Visit(SqlAllowPageLocksIndexOption codeObject, C context);

	T Visit(SqlAllowRowLocksIndexOption codeObject, C context);

	T Visit(SqlAssignment codeObject, C context);

	T Visit(SqlAtTimeZoneExpression codeObject, C context);

	T Visit(SqlBatch codeObject, C context);

	T Visit(SqlBetweenBooleanExpression codeObject, C context);

	T Visit(SqlBinaryBooleanExpression codeObject, C context);

	T Visit(SqlBinaryFilterExpression codeObject, C context);

	T Visit(SqlBinaryQueryExpression codeObject, C context);

	T Visit(SqlBinaryScalarExpression codeObject, C context);

	T Visit(SqlBooleanExpression codeObject, C context);

	T Visit(SqlBooleanFilterExpression codeObject, C context);

	T Visit(SqlBuiltinScalarFunctionCallExpression codeObject, C context);

	T Visit(SqlCastExpression codeObject, C context);

	T Visit(SqlChangeTrackingContext codeObject, C context);

	T Visit(SqlCheckConstraint codeObject, C context);

	T Visit(SqlClrAssemblySpecifier codeObject, C context);

	T Visit(SqlClrClassSpecifier codeObject, C context);

	T Visit(SqlClrFunctionBodyDefinition codeObject, C context);

	T Visit(SqlClrMethodSpecifier codeObject, C context);

	T Visit(SqlCollateScalarExpression codeObject, C context);

	T Visit(SqlCollation codeObject, C context);

	T Visit(SqlColumnAssignment codeObject, C context);

	T Visit(SqlDefaultConstraint codeObject, C context);

	T Visit(SqlColumnDefinition codeObject, C context);

	T Visit(SqlColumnIdentity codeObject, C context);

	T Visit(SqlColumnRefExpression codeObject, C context);

	T Visit(SqlCommonTableExpression codeObject, C context);

	T Visit(SqlComparisonBooleanExpression codeObject, C context);

	T Visit(SqlCompressionPartitionRange codeObject, C context);

	T Visit(SqlComputedColumnDefinition codeObject, C context);

	T Visit(SqlConditionClause codeObject, C context);

	T Visit(SqlConstraint codeObject, C context);

	T Visit(SqlConvertExpression codeObject, C context);

	T Visit(SqlCreateUserOption codeObject, C context);

	T Visit(SqlCubeGroupByItem codeObject, C context);

	T Visit(SqlCursorOption codeObject, C context);

	T Visit(SqlCursorVariableAssignment codeObject, C context);

	T Visit(SqlCursorVariableRefExpression codeObject, C context);

	T Visit(SqlDataCompressionIndexOption codeObject, C context);

	T Visit(SqlDataType codeObject, C context);

	T Visit(SqlDataTypeSpecification codeObject, C context);

	T Visit(SqlDdlTriggerDefinition codeObject, C context);

	T Visit(SqlDefaultValuesInsertMergeActionSource codeObject, C context);

	T Visit(SqlDefaultValuesInsertSource codeObject, C context);

	T Visit(SqlDeleteMergeAction codeObject, C context);

	T Visit(SqlDeleteSpecification codeObject, C context);

	T Visit(SqlDerivedTableExpression codeObject, C context);

	T Visit(SqlDmlSpecificationTableSource codeObject, C context);

	T Visit(SqlDmlTriggerDefinition codeObject, C context);

	T Visit(SqlDropExistingIndexOption codeObject, C context);

	T Visit(SqlExecuteArgument codeObject, C context);

	T Visit(SqlExecuteAsClause codeObject, C context);

	T Visit(SqlExistsBooleanExpression codeObject, C context);

	T Visit(SqlFillFactorIndexOption codeObject, C context);

	T Visit(SqlFilterClause codeObject, C context);

	T Visit(SqlForBrowseClause codeObject, C context);

	T Visit(SqlForeignKeyConstraint codeObject, C context);

	T Visit(SqlForXmlAutoClause codeObject, C context);

	T Visit(SqlForXmlClause codeObject, C context);

	T Visit(SqlForXmlDirective codeObject, C context);

	T Visit(SqlForXmlExplicitClause codeObject, C context);

	T Visit(SqlForXmlPathClause codeObject, C context);

	T Visit(SqlForXmlRawClause codeObject, C context);

	T Visit(SqlFromClause codeObject, C context);

	T Visit(SqlFullTextBooleanExpression codeObject, C context);

	T Visit(SqlFullTextColumn codeObject, C context);

	T Visit(SqlFunctionDefinition codeObject, C context);

	T Visit(SqlGlobalScalarVariableRefExpression codeObject, C context);

	T Visit(SqlGrandTotalGroupByItem codeObject, C context);

	T Visit(SqlGrandTotalGroupingSet codeObject, C context);

	T Visit(SqlGroupByClause codeObject, C context);

	T Visit(SqlGroupBySets codeObject, C context);

	T Visit(SqlGroupingSetItemsCollection codeObject, C context);

	T Visit(SqlHavingClause codeObject, C context);

	T Visit(SqlIdentifier codeObject, C context);

	T Visit(SqlIdentityFunctionCallExpression codeObject, C context);

	T Visit(SqlIgnoreDupKeyIndexOption codeObject, C context);

	T Visit(SqlInBooleanExpression codeObject, C context);

	T Visit(SqlInBooleanExpressionCollectionValue codeObject, C context);

	T Visit(SqlInBooleanExpressionQueryValue codeObject, C context);

	T Visit(SqlIndexedColumn codeObject, C context);

	T Visit(SqlIndexHint codeObject, C context);

	T Visit(SqlIndexOption codeObject, C context);

	T Visit(SqlInlineIndexConstraint codeObject, C context);

	T Visit(SqlInlineFunctionBodyDefinition codeObject, C context);

	T Visit(SqlInlineTableRelationalFunctionDefinition codeObject, C context);

	T Visit(SqlInlineTableVariableDeclaration codeObject, C context);

	T Visit(SqlInsertMergeAction codeObject, C context);

	T Visit(SqlInsertSpecification codeObject, C context);

	T Visit(SqlIntoClause codeObject, C context);

	T Visit(SqlIsNullBooleanExpression codeObject, C context);

	T Visit(SqlLargeDataStorageInformation codeObject, C context);

	T Visit(SqlLikeBooleanExpression codeObject, C context);

	T Visit(SqlLiteralExpression codeObject, C context);

	T Visit(SqlLoginPassword codeObject, C context);

	T Visit(SqlMaxDegreeOfParallelismIndexOption codeObject, C context);

	T Visit(SqlMergeActionClause codeObject, C context);

	T Visit(SqlMergeSpecification codeObject, C context);

	T Visit(SqlInsertSource codeObject, C context);

	T Visit(SqlModuleCalledOnNullInputOption codeObject, C context);

	T Visit(SqlModuleEncryptionOption codeObject, C context);

	T Visit(SqlModuleExecuteAsOption codeObject, C context);

	T Visit(SqlModuleNativeCompilationOption codeObject, C context);

	T Visit(SqlModuleOption codeObject, C context);

	T Visit(SqlModuleRecompileOption codeObject, C context);

	T Visit(SqlModuleReturnsNullOnNullInputOption codeObject, C context);

	T Visit(SqlModuleSchemaBindingOption codeObject, C context);

	T Visit(SqlModuleViewMetadataOption codeObject, C context);

	T Visit(SqlMultistatementFunctionBodyDefinition codeObject, C context);

	T Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject, C context);

	T Visit(SqlNotBooleanExpression codeObject, C context);

	T Visit(SqlQueryExpression codeObject, C context);

	T Visit(SqlScalarExpression codeObject, C context);

	T Visit(SqlTableExpression codeObject, C context);

	T Visit(SqlObjectIdentifier codeObject, C context);

	T Visit(SqlObjectReference codeObject, C context);

	T Visit(SqlOnlineIndexOption codeObject, C context);

	T Visit(SqlOptimizeForSequentialKeyIndexOption codeObject, C context);

	T Visit(SqlResumableIndexOption codeObject, C context);

	T Visit(SqlBucketCountIndexOption codeObject, C context);

	T Visit(SqlCompressionDelayIndexOption codeObject, C context);

	T Visit(SqlMaxDurationIndexOption codeObject, C context);

	T Visit(SqlOffsetFetchClause codeObject, C context);

	T Visit(SqlOrderByClause codeObject, C context);

	T Visit(SqlOrderByItem codeObject, C context);

	T Visit(SqlOutputClause codeObject, C context);

	T Visit(SqlOutputIntoClause codeObject, C context);

	T Visit(SqlPadIndexOption codeObject, C context);

	T Visit(SqlParameterDeclaration codeObject, C context);

	T Visit(SqlPivotClause codeObject, C context);

	T Visit(SqlPivotTableExpression codeObject, C context);

	T Visit(SqlPrimaryKeyConstraint codeObject, C context);

	T Visit(SqlStorageSpecification codeObject, C context);

	T Visit(SqlProcedureDefinition codeObject, C context);

	T Visit(SqlQualifiedJoinTableExpression codeObject, C context);

	T Visit(SqlQuerySpecification codeObject, C context);

	T Visit(SqlQueryWithClause codeObject, C context);

	T Visit(SqlRollupGroupByItem codeObject, C context);

	T Visit(SqlRowConstructorExpression codeObject, C context);

	T Visit(SqlScalarClrFunctionDefinition codeObject, C context);

	T Visit(SqlScalarFunctionReturnType codeObject, C context);

	T Visit(SqlScalarRefExpression codeObject, C context);

	T Visit(SqlScalarRelationalFunctionDefinition codeObject, C context);

	T Visit(SqlScalarSubQueryExpression codeObject, C context);

	T Visit(SqlScalarVariableAssignment codeObject, C context);

	T Visit(SqlScalarVariableRefExpression codeObject, C context);

	T Visit(SqlScript codeObject, C context);

	T Visit(SqlSearchedCaseExpression codeObject, C context);

	T Visit(SqlSearchedWhenClause codeObject, C context);

	T Visit(SqlSelectClause codeObject, C context);

	T Visit(SqlSelectIntoClause codeObject, C context);

	T Visit(SqlSelectScalarExpression codeObject, C context);

	T Visit(SqlSelectSpecification codeObject, C context);

	T Visit(SqlSelectSpecificationInsertSource codeObject, C context);

	T Visit(SqlSelectStarExpression codeObject, C context);

	T Visit(SqlSelectVariableAssignmentExpression codeObject, C context);

	T Visit(SqlSetClause codeObject, C context);

	T Visit(SqlSimpleCaseExpression codeObject, C context);

	T Visit(SqlSimpleGroupByItem codeObject, C context);

	T Visit(SqlSimpleOrderByClause codeObject, C context);

	T Visit(SqlSimpleOrderByItem codeObject, C context);

	T Visit(SqlSimpleWhenClause codeObject, C context);

	T Visit(SqlSortedDataIndexOption codeObject, C context);

	T Visit(SqlSortedDataReorgIndexOption codeObject, C context);

	T Visit(SqlSortInTempDbIndexOption codeObject, C context);

	T Visit(SqlStatisticsIncrementalIndexOption codeObject, C context);

	T Visit(SqlStatisticsNoRecomputeIndexOption codeObject, C context);

	T Visit(SqlStatisticsOnlyIndexOption codeObject, C context);

	T Visit(SqlTableClrFunctionDefinition codeObject, C context);

	T Visit(SqlTableConstructorExpression codeObject, C context);

	T Visit(SqlTableConstructorInsertSource codeObject, C context);

	T Visit(SqlTableDefinition codeObject, C context);

	T Visit(SqlTableFunctionReturnType codeObject, C context);

	T Visit(SqlTableHint codeObject, C context);

	T Visit(SqlTableRefExpression codeObject, C context);

	T Visit(SqlTableValuedFunctionRefExpression codeObject, C context);

	T Visit(SqlTableVariableRefExpression codeObject, C context);

	T Visit(SqlTableUdtInstanceMethodExpression codeObject, C context);

	T Visit(SqlTargetTableExpression codeObject, C context);

	T Visit(SqlTemporalPeriodDefinition codeObject, C context);

	T Visit(SqlTopSpecification codeObject, C context);

	T Visit(SqlTriggerAction codeObject, C context);

	T Visit(SqlTriggerDefinition codeObject, C context);

	T Visit(SqlTriggerEvent codeObject, C context);

	T Visit(SqlUdtInstanceDataMemberExpression codeObject, C context);

	T Visit(SqlUdtInstanceMethodExpression codeObject, C context);

	T Visit(SqlUdtStaticDataMemberExpression codeObject, C context);

	T Visit(SqlUdtStaticMethodExpression codeObject, C context);

	T Visit(SqlUnaryScalarExpression codeObject, C context);

	T Visit(SqlUniqueConstraint codeObject, C context);

	T Visit(SqlUnpivotClause codeObject, C context);

	T Visit(SqlUnpivotTableExpression codeObject, C context);

	T Visit(SqlUnqualifiedJoinTableExpression codeObject, C context);

	T Visit(SqlUpdateBooleanExpression codeObject, C context);

	T Visit(SqlUpdateMergeAction codeObject, C context);

	T Visit(SqlUpdateSpecification codeObject, C context);

	T Visit(SqlUserDefinedScalarFunctionCallExpression codeObject, C context);

	T Visit(SqlValuesInsertMergeActionSource codeObject, C context);

	T Visit(SqlVariableColumnAssignment codeObject, C context);

	T Visit(SqlVariableDeclaration codeObject, C context);

	T Visit(SqlViewDefinition codeObject, C context);

	T Visit(SqlWhereClause codeObject, C context);

	T Visit(SqlXmlNamespacesDeclaration codeObject, C context);
}
