using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlCodeObjectRecursiveContextVisitor<C> : ISqlCodeObjectContextVisitor<C>, ISqlStatementContextVisitor<C>
{
	private void VisitList<TElem>(IEnumerable<TElem> codeObjects, C context) where TElem : IVisitableSqlCodeObject
	{
		foreach (TElem codeObject in codeObjects)
		{
			codeObject.Accept(this, context);
		}
	}

	public virtual void Visit(SqlAggregateFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAllAnyComparisonBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAllowPageLocksIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAllowRowLocksIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAtTimeZoneExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBatch codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBetweenBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBinaryBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBinaryFilterExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBinaryQueryExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBinaryScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBooleanFilterExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBuiltinScalarFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCastExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlChangeTrackingContext codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCheckConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlClrAssemblySpecifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlClrClassSpecifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlClrFunctionBodyDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlClrMethodSpecifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCollateScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCollation codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlColumnAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDefaultConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlColumnDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlColumnIdentity codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlColumnRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCommonTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlComparisonBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCompressionPartitionRange codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlComputedColumnDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlConditionClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlConvertExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCubeGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCursorOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCursorVariableAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCursorVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDataCompressionIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDataType codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDataTypeSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDdlTriggerDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDefaultValuesInsertMergeActionSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDefaultValuesInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDeleteMergeAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDeleteSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDerivedTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDmlSpecificationTableSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDmlTriggerDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropExistingIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlExecuteArgument codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlExecuteAsClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlExistsBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlFillFactorIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlFilterClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForBrowseClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForeignKeyConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForXmlAutoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForXmlClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForXmlDirective codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForXmlExplicitClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForXmlPathClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlForXmlRawClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlFromClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlFullTextBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlFullTextColumn codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlGlobalScalarVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlGrandTotalGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlGrandTotalGroupingSet codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlGroupByClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlGroupBySets codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlGroupingSetItemsCollection codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlHavingClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIdentifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIdentityFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIgnoreDupKeyIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInBooleanExpressionCollectionValue codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInBooleanExpressionQueryValue codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIndexedColumn codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIndexHint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInlineIndexConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInlineFunctionBodyDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInlineTableRelationalFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInlineTableVariableDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInsertMergeAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInsertSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIntoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIsNullBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlLargeDataStorageInformation codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlLikeBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlLiteralExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlLoginPassword codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlMaxDegreeOfParallelismIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlMergeActionClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlMergeSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleCalledOnNullInputOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleEncryptionOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleExecuteAsOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleNativeCompilationOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleRecompileOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleReturnsNullOnNullInputOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleSchemaBindingOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlModuleViewMetadataOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlMultistatementFunctionBodyDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlNotBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlQueryExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlObjectIdentifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlObjectReference codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlOnlineIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlOptimizeForSequentialKeyIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlResumableIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBucketCountIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCompressionDelayIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlMaxDurationIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlOffsetFetchClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlOrderByClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlOrderByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlOutputClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlOutputIntoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlPadIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlParameterDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlPivotClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlPivotTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlPrimaryKeyConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlStorageSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlProcedureDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlQualifiedJoinTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlQuerySpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlQueryWithClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRollupGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRowConstructorExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarClrFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarFunctionReturnType codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarRelationalFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarSubQueryExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarVariableAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScalarVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlScript codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSearchedCaseExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSearchedWhenClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectIntoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectSpecificationInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectStarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectVariableAssignmentExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSetClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSimpleCaseExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSimpleGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSimpleOrderByClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSimpleOrderByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSimpleWhenClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSortedDataIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSortedDataReorgIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSortInTempDbIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlStatisticsIncrementalIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlStatisticsNoRecomputeIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlStatisticsOnlyIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableClrFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableConstructorExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableConstructorInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableFunctionReturnType codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableHint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableValuedFunctionRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTableUdtInstanceMethodExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTargetTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTemporalPeriodDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTopSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTriggerAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTriggerDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTriggerEvent codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUdtInstanceDataMemberExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUdtInstanceMethodExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUdtStaticDataMemberExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUdtStaticMethodExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUnaryScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUniqueConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUnpivotClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUnpivotTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUnqualifiedJoinTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUpdateBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUpdateMergeAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUpdateSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUserDefinedScalarFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlValuesInsertMergeActionSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlVariableColumnAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlVariableDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlViewDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlWhereClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlXmlNamespacesDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAlterFunctionStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAlterLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAlterProcedureStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAlterTriggerStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlAlterViewStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBackupCertificateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBackupDatabaseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBackupLogStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBackupMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBackupServiceMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBackupTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlBreakStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCommentStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCompoundStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlContinueStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateFunctionStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateIndexStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateLoginFromAsymKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateLoginFromCertificateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateLoginFromWindowsStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateLoginWithPasswordStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateProcedureStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateRoleStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateSchemaStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateSynonymStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateTriggerStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserDefinedDataTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserDefinedTableTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserDefinedTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserFromAsymKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserFromCertificateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserWithImplicitAuthenticationStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserFromLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserFromExternalProviderStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateUserWithoutLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCreateViewStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlCursorDeclareStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDBCCStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDeleteStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDenyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropAggregateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropDatabaseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropDefaultStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropFunctionStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropProcedureStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropRuleStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropSchemaStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropSecurityPolicyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropSequenceStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropSynonymStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropTriggerStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropUserStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlDropViewStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlExecuteModuleStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlExecuteStringStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlGrantStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlIfElseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInlineTableVariableDeclareStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlInsertStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlMergeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRestoreDatabaseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRestoreInformationStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRestoreLogStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRestoreMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRestoreServiceMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRestoreTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlReturnStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlRevokeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSelectStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSetAssignmentStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlSetStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlTryCatchStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUpdateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlUseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlVariableDeclareStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}

	public virtual void Visit(SqlWhileStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
	}
}
public class SqlCodeObjectRecursiveContextVisitor<T, C> : ISqlCodeObjectContextVisitor<T, C>, ISqlStatementContextVisitor<T, C>
{
	private void VisitList<TElem>(IEnumerable<TElem> codeObjects, C context) where TElem : IVisitableSqlCodeObject
	{
		foreach (TElem codeObject in codeObjects)
		{
			codeObject.Accept(this, context);
		}
	}

	public virtual T Visit(SqlAggregateFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAllAnyComparisonBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAllowPageLocksIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAllowRowLocksIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAtTimeZoneExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBatch codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBetweenBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBinaryBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBinaryFilterExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBinaryQueryExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBinaryScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBooleanFilterExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBuiltinScalarFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCastExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlChangeTrackingContext codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCheckConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlClrAssemblySpecifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlClrClassSpecifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlClrFunctionBodyDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlClrMethodSpecifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCollateScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCollation codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlColumnAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDefaultConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlColumnDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlColumnIdentity codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlColumnRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCommonTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlComparisonBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCompressionPartitionRange codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlComputedColumnDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlConditionClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlConvertExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCubeGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCursorOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCursorVariableAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCursorVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDataCompressionIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDataType codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDataTypeSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDdlTriggerDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDefaultValuesInsertMergeActionSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDefaultValuesInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDeleteMergeAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDeleteSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDerivedTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDmlSpecificationTableSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDmlTriggerDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropExistingIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlExecuteArgument codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlExecuteAsClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlExistsBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlFillFactorIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlFilterClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForBrowseClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForeignKeyConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForXmlAutoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForXmlClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForXmlDirective codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForXmlExplicitClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForXmlPathClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlForXmlRawClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlFromClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlFullTextBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlFullTextColumn codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlGlobalScalarVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlGrandTotalGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlGrandTotalGroupingSet codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlGroupByClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlGroupBySets codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlGroupingSetItemsCollection codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlHavingClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIdentifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIdentityFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIgnoreDupKeyIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInBooleanExpressionCollectionValue codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInBooleanExpressionQueryValue codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIndexedColumn codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIndexHint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInlineIndexConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInlineFunctionBodyDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInlineTableRelationalFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInlineTableVariableDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInsertMergeAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInsertSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIntoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIsNullBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlLargeDataStorageInformation codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlLikeBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlLiteralExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlLoginPassword codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlMaxDegreeOfParallelismIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlMergeActionClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlMergeSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleCalledOnNullInputOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleEncryptionOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleExecuteAsOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleNativeCompilationOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleRecompileOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleReturnsNullOnNullInputOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleSchemaBindingOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlModuleViewMetadataOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlMultistatementFunctionBodyDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlNotBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlQueryExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlObjectIdentifier codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlObjectReference codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlOnlineIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlOptimizeForSequentialKeyIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlResumableIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBucketCountIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCompressionDelayIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlMaxDurationIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlOffsetFetchClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlOrderByClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlOrderByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlOutputClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlOutputIntoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlPadIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlParameterDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlPivotClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlPivotTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlPrimaryKeyConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlStorageSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlProcedureDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlQualifiedJoinTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlQuerySpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlQueryWithClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRollupGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRowConstructorExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarClrFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarFunctionReturnType codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarRelationalFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarSubQueryExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarVariableAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScalarVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlScript codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSearchedCaseExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSearchedWhenClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectIntoClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectSpecificationInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectStarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectVariableAssignmentExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSetClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSimpleCaseExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSimpleGroupByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSimpleOrderByClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSimpleOrderByItem codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSimpleWhenClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSortedDataIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSortedDataReorgIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSortInTempDbIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlStatisticsIncrementalIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlStatisticsNoRecomputeIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlStatisticsOnlyIndexOption codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableClrFunctionDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableConstructorExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableConstructorInsertSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableFunctionReturnType codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableHint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableValuedFunctionRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableVariableRefExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTableUdtInstanceMethodExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTargetTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTemporalPeriodDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTopSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTriggerAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTriggerDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTriggerEvent codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUdtInstanceDataMemberExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUdtInstanceMethodExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUdtStaticDataMemberExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUdtStaticMethodExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUnaryScalarExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUniqueConstraint codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUnpivotClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUnpivotTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUnqualifiedJoinTableExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUpdateBooleanExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUpdateMergeAction codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUpdateSpecification codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUserDefinedScalarFunctionCallExpression codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlValuesInsertMergeActionSource codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlVariableColumnAssignment codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlVariableDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlViewDefinition codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlWhereClause codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlXmlNamespacesDeclaration codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAlterFunctionStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAlterLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAlterProcedureStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAlterTriggerStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlAlterViewStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBackupCertificateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBackupDatabaseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBackupLogStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBackupMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBackupServiceMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBackupTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlBreakStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCommentStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCompoundStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlContinueStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateFunctionStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateIndexStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromAsymKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromCertificateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromWindowsStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginWithPasswordStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateProcedureStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateRoleStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateSchemaStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateSynonymStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateTriggerStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedDataTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedTableTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromAsymKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromCertificateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserWithImplicitAuthenticationStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromExternalProviderStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateUserWithoutLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCreateViewStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlCursorDeclareStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDBCCStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDeleteStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDenyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropAggregateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropDatabaseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropDefaultStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropFunctionStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropLoginStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropProcedureStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropRuleStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropSchemaStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropSecurityPolicyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropSequenceStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropSynonymStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropTriggerStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropTypeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropUserStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlDropViewStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlExecuteModuleStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlExecuteStringStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlGrantStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlIfElseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInlineTableVariableDeclareStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlInsertStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlMergeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRestoreDatabaseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRestoreInformationStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRestoreLogStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRestoreMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRestoreServiceMasterKeyStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRestoreTableStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlReturnStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlRevokeStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSelectStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSetAssignmentStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlSetStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlTryCatchStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUpdateStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlUseStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlVariableDeclareStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}

	public virtual T Visit(SqlWhileStatement codeObject, C context)
	{
		VisitList(codeObject.Children, context);
		return default(T);
	}
}
