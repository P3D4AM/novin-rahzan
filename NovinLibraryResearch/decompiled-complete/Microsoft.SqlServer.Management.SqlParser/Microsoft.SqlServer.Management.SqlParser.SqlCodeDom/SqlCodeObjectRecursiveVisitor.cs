using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCodeObjectRecursiveVisitor : ISqlCodeObjectVisitor, ISqlStatementVisitor
{
	private void VisitList<TElem>(IEnumerable<TElem> codeObjects) where TElem : IVisitableSqlCodeObject
	{
		foreach (TElem codeObject in codeObjects)
		{
			codeObject.Accept(this);
		}
	}

	public virtual void Visit(SqlAggregateFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAllAnyComparisonBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAllowPageLocksIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAllowRowLocksIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAssignment codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAtTimeZoneExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBatch codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBetweenBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBinaryBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBinaryFilterExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBinaryQueryExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBinaryScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBooleanFilterExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBuiltinScalarFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCastExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlChangeTrackingContext codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCheckConstraint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlClrAssemblySpecifier codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlClrClassSpecifier codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlClrFunctionBodyDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlClrMethodSpecifier codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCollateScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCollation codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlColumnAssignment codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDefaultConstraint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlColumnDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlColumnIdentity codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlColumnRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCommonTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlComparisonBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCompressionPartitionRange codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlComputedColumnDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlConditionClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlConstraint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlConvertExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCubeGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCursorOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCursorVariableAssignment codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCursorVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDataCompressionIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDataType codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDataTypeSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDdlTriggerDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDefaultValuesInsertMergeActionSource codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDefaultValuesInsertSource codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDeleteMergeAction codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDeleteSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDerivedTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDmlSpecificationTableSource codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDmlTriggerDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropExistingIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlExecuteArgument codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlExecuteAsClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlExistsBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlFillFactorIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlFilterClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForBrowseClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForeignKeyConstraint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForXmlAutoClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForXmlClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForXmlDirective codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForXmlExplicitClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForXmlPathClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlForXmlRawClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlFromClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlFullTextBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlFullTextColumn codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlGlobalScalarVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlGrandTotalGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlGrandTotalGroupingSet codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlGroupByClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlGroupBySets codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlGroupingSetItemsCollection codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlHavingClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIdentifier codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIdentityFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIgnoreDupKeyIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInBooleanExpressionCollectionValue codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInBooleanExpressionQueryValue codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIndexedColumn codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIndexHint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInlineIndexConstraint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInlineFunctionBodyDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInlineTableRelationalFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInlineTableVariableDeclaration codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInsertMergeAction codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInsertSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIntoClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIsNullBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlLargeDataStorageInformation codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlLikeBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlLiteralExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlLoginPassword codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlMaxDegreeOfParallelismIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlMergeActionClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlMergeSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInsertSource codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleCalledOnNullInputOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleEncryptionOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleExecuteAsOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleNativeCompilationOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleRecompileOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleReturnsNullOnNullInputOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleSchemaBindingOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlModuleViewMetadataOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlMultistatementFunctionBodyDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlNotBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlQueryExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlObjectIdentifier codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlObjectReference codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlOnlineIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlOptimizeForSequentialKeyIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlResumableIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBucketCountIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCompressionDelayIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlMaxDurationIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlOffsetFetchClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlOrderByClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlOrderByItem codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlOutputClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlOutputIntoClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlPadIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlParameterDeclaration codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlPivotClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlPivotTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlPrimaryKeyConstraint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlStorageSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlProcedureDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlQualifiedJoinTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlQuerySpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlQueryWithClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRollupGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRowConstructorExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarClrFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarFunctionReturnType codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarRelationalFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarSubQueryExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarVariableAssignment codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScalarVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlScript codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSearchedCaseExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSearchedWhenClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectIntoClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectSpecificationInsertSource codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectStarExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectVariableAssignmentExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSetClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSimpleCaseExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSimpleGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSimpleOrderByClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSimpleOrderByItem codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSimpleWhenClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSortedDataIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSortedDataReorgIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSortInTempDbIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlStatisticsIncrementalIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlStatisticsNoRecomputeIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlStatisticsOnlyIndexOption codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableClrFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableConstructorExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableConstructorInsertSource codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableFunctionReturnType codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableHint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableValuedFunctionRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTableUdtInstanceMethodExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTargetTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTemporalPeriodDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTopSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTriggerAction codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTriggerDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTriggerEvent codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUdtInstanceDataMemberExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUdtInstanceMethodExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUdtStaticDataMemberExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUdtStaticMethodExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUnaryScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUniqueConstraint codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUnpivotClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUnpivotTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUnqualifiedJoinTableExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUpdateBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUpdateMergeAction codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUpdateSpecification codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUserDefinedScalarFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlValuesInsertMergeActionSource codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlVariableColumnAssignment codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlVariableDeclaration codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlViewDefinition codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlWhereClause codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlXmlNamespacesDeclaration codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAlterFunctionStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAlterLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAlterProcedureStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAlterTriggerStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlAlterViewStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBackupCertificateStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBackupDatabaseStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBackupLogStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBackupMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBackupServiceMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBackupTableStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlBreakStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCommentStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCompoundStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlContinueStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateFunctionStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateIndexStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateLoginFromAsymKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateLoginFromCertificateStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateLoginFromWindowsStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateLoginWithPasswordStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateProcedureStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateRoleStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateSchemaStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateSynonymStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateTableStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateTriggerStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserDefinedDataTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserDefinedTableTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserDefinedTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserFromAsymKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserFromCertificateStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserWithImplicitAuthenticationStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserFromLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserFromExternalProviderStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateUserWithoutLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCreateViewStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlCursorDeclareStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDBCCStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDeleteStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDenyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropAggregateStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropDatabaseStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropDefaultStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropFunctionStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropProcedureStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropRuleStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropSchemaStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropSecurityPolicyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropSequenceStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropSynonymStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropTableStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropTriggerStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropUserStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlDropViewStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlExecuteModuleStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlExecuteStringStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlGrantStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlIfElseStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInlineTableVariableDeclareStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlInsertStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlMergeStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRestoreDatabaseStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRestoreInformationStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRestoreLogStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRestoreMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRestoreServiceMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRestoreTableStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlReturnStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlRevokeStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSelectStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSetAssignmentStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlSetStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlTryCatchStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUpdateStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlUseStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlVariableDeclareStatement codeObject)
	{
		VisitList(codeObject.Children);
	}

	public virtual void Visit(SqlWhileStatement codeObject)
	{
		VisitList(codeObject.Children);
	}
}
public class SqlCodeObjectRecursiveVisitor<T> : ISqlCodeObjectVisitor<T>, ISqlStatementVisitor<T>
{
	private void VisitList<TElem>(IEnumerable<TElem> codeObjects) where TElem : IVisitableSqlCodeObject
	{
		foreach (TElem codeObject in codeObjects)
		{
			codeObject.Accept(this);
		}
	}

	public virtual T Visit(SqlAggregateFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAllAnyComparisonBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAllowPageLocksIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAllowRowLocksIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAssignment codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAtTimeZoneExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBatch codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBetweenBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBinaryBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBinaryFilterExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBinaryQueryExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBinaryScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBooleanFilterExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBuiltinScalarFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCastExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlChangeTrackingContext codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCheckConstraint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlClrAssemblySpecifier codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlClrClassSpecifier codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlClrFunctionBodyDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlClrMethodSpecifier codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCollateScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCollation codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlColumnAssignment codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDefaultConstraint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlColumnDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlColumnIdentity codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlColumnRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCommonTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlComparisonBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCompressionPartitionRange codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlComputedColumnDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlConditionClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlConstraint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlConvertExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCubeGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCursorOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCursorVariableAssignment codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCursorVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDataCompressionIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDataType codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDataTypeSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDdlTriggerDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDefaultValuesInsertMergeActionSource codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDefaultValuesInsertSource codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDeleteMergeAction codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDeleteSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDerivedTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDmlSpecificationTableSource codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDmlTriggerDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropExistingIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlExecuteArgument codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlExecuteAsClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlExistsBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlFillFactorIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlFilterClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForBrowseClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForeignKeyConstraint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForXmlAutoClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForXmlClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForXmlDirective codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForXmlExplicitClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForXmlPathClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlForXmlRawClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlFromClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlFullTextBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlFullTextColumn codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlGlobalScalarVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlGrandTotalGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlGrandTotalGroupingSet codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlGroupByClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlGroupBySets codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlGroupingSetItemsCollection codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlHavingClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIdentifier codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIdentityFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIgnoreDupKeyIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInBooleanExpressionCollectionValue codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInBooleanExpressionQueryValue codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIndexedColumn codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIndexHint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInlineIndexConstraint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInlineFunctionBodyDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInlineTableRelationalFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInlineTableVariableDeclaration codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInsertMergeAction codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInsertSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIntoClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIsNullBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlLargeDataStorageInformation codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlLikeBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlLiteralExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlLoginPassword codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlMaxDegreeOfParallelismIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlMergeActionClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlMergeSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInsertSource codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleCalledOnNullInputOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleEncryptionOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleExecuteAsOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleNativeCompilationOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleRecompileOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleReturnsNullOnNullInputOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleSchemaBindingOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlModuleViewMetadataOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlMultistatementFunctionBodyDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlNotBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlQueryExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlObjectIdentifier codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlObjectReference codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlOnlineIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlOptimizeForSequentialKeyIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlResumableIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBucketCountIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCompressionDelayIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlMaxDurationIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlOffsetFetchClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlOrderByClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlOrderByItem codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlOutputClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlOutputIntoClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlPadIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlParameterDeclaration codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlPivotClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlPivotTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlPrimaryKeyConstraint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlStorageSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlProcedureDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlQualifiedJoinTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlQuerySpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlQueryWithClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRollupGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRowConstructorExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarClrFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarFunctionReturnType codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarRelationalFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarSubQueryExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarVariableAssignment codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScalarVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlScript codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSearchedCaseExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSearchedWhenClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectIntoClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectSpecificationInsertSource codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectStarExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectVariableAssignmentExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSetClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSimpleCaseExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSimpleGroupByItem codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSimpleOrderByClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSimpleOrderByItem codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSimpleWhenClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSortedDataIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSortedDataReorgIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSortInTempDbIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlStatisticsIncrementalIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlStatisticsNoRecomputeIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlStatisticsOnlyIndexOption codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableClrFunctionDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableConstructorExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableConstructorInsertSource codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableFunctionReturnType codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableHint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableValuedFunctionRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableVariableRefExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTableUdtInstanceMethodExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTargetTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTemporalPeriodDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTopSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTriggerAction codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTriggerDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTriggerEvent codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUdtInstanceDataMemberExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUdtInstanceMethodExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUdtStaticDataMemberExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUdtStaticMethodExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUnaryScalarExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUniqueConstraint codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUnpivotClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUnpivotTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUnqualifiedJoinTableExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUpdateBooleanExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUpdateMergeAction codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUpdateSpecification codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUserDefinedScalarFunctionCallExpression codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlValuesInsertMergeActionSource codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlVariableColumnAssignment codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlVariableDeclaration codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlViewDefinition codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlWhereClause codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlXmlNamespacesDeclaration codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAlterFunctionStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAlterLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAlterProcedureStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAlterTriggerStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlAlterViewStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBackupCertificateStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBackupDatabaseStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBackupLogStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBackupMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBackupServiceMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBackupTableStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlBreakStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCommentStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCompoundStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlContinueStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateFunctionStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateIndexStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromAsymKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromCertificateStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromWindowsStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginWithPasswordStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateProcedureStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateRoleStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateSchemaStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateSynonymStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateTableStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateTriggerStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedDataTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedTableTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromAsymKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromCertificateStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserWithImplicitAuthenticationStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromExternalProviderStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserWithoutLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCreateViewStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlCursorDeclareStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDBCCStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDeleteStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDenyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropAggregateStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropDatabaseStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropDefaultStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropFunctionStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropLoginStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropProcedureStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropRuleStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropSchemaStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropSecurityPolicyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropSequenceStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropSynonymStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropTableStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropTriggerStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropTypeStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropUserStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlDropViewStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlExecuteModuleStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlExecuteStringStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlGrantStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlIfElseStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInlineTableVariableDeclareStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlInsertStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlMergeStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRestoreDatabaseStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRestoreInformationStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRestoreLogStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRestoreMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRestoreServiceMasterKeyStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRestoreTableStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlReturnStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlRevokeStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSelectStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSetAssignmentStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlSetStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlTryCatchStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUpdateStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlUseStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlVariableDeclareStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}

	public virtual T Visit(SqlWhileStatement codeObject)
	{
		VisitList(codeObject.Children);
		return default(T);
	}
}
