using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public static class ValidateModuleBodyVisitor
{
	private static class BinderSingletonHelper
	{
		public static readonly SqlStatementRecursiveVisitor FunctionBinderVisitor;

		static BinderSingletonHelper()
		{
			FunctionBinderVisitor = new SqlStatementRecursiveVisitor(new FunctionStatementBinderVisitor());
		}
	}

	private sealed class FunctionStatementBinderVisitor : ISqlStatementVisitor
	{
		private void ReportInvalidUseOfSideEffectingOperator(SqlStatement stmt)
		{
			stmt.ReportBindError(SR.InvalidUseOfSideEffectingOperatorWithinFunction(stmt.StatementPhrase));
		}

		private bool IsAllowedTargetTableExpression(SqlTableExpression tableExpr)
		{
			if (tableExpr.BoundTable == null)
			{
				return true;
			}
			switch (tableExpr.BoundTable.TabularType)
			{
			case TabularType.Table:
			case TabularType.TableAlias:
			case TabularType.TemporaryTable:
			case TabularType.View:
				return false;
			case TabularType.TableVariable:
				return true;
			default:
				return true;
			}
		}

		public void Visit(SqlAlterFunctionStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlAlterLoginStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlAlterProcedureStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlAlterTriggerStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlAlterViewStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlBackupCertificateStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlBackupDatabaseStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlBackupLogStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlBackupMasterKeyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlBackupServiceMasterKeyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlBackupTableStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlBatch statement)
		{
		}

		public void Visit(SqlBreakStatement statement)
		{
		}

		public void Visit(SqlCommentStatement statement)
		{
		}

		public void Visit(SqlCompoundStatement statement)
		{
		}

		public void Visit(SqlContinueStatement statement)
		{
		}

		public void Visit(SqlCreateFunctionStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateIndexStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateLoginFromAsymKeyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateLoginFromCertificateStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateLoginFromWindowsStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateLoginWithPasswordStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateProcedureStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateRoleStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateSchemaStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateSynonymStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateTableStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateTriggerStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserDefinedDataTypeStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserDefinedTableTypeStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserDefinedTypeStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserFromAsymKeyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserFromCertificateStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserWithImplicitAuthenticationStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserFromLoginStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserFromExternalProviderStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateUserWithoutLoginStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCreateViewStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlCursorDeclareStatement statement)
		{
		}

		public void Visit(SqlDBCCStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDeleteStatement statement)
		{
			SqlDeleteSpecification deleteSpecification = statement.DeleteSpecification;
			bool flag = false;
			if (!IsAllowedTargetTableExpression(deleteSpecification.Target))
			{
				flag = true;
			}
			else if (deleteSpecification.OutputClause != null)
			{
				flag = true;
			}
			else if (deleteSpecification.OutputIntoClause != null && !IsAllowedTargetTableExpression(deleteSpecification.OutputIntoClause.IntoClause.Target))
			{
				flag = true;
			}
			if (flag)
			{
				ReportInvalidUseOfSideEffectingOperator(statement);
			}
		}

		public void Visit(SqlDenyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropAggregateStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropDatabaseStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropDefaultStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropFunctionStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropLoginStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropUserStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropRuleStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropSchemaStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropSecurityPolicyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropSequenceStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropSynonymStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropTableStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropTriggerStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropTypeStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropProcedureStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlDropViewStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlExecuteModuleStatement statement)
		{
		}

		public void Visit(SqlExecuteStringStatement statement)
		{
		}

		public void Visit(SqlGrantStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlIfElseStatement statement)
		{
		}

		public void Visit(SqlInlineTableVariableDeclareStatement statement)
		{
		}

		public void Visit(SqlInsertStatement statement)
		{
			SqlInsertSpecification insertSpecification = statement.InsertSpecification;
			bool flag = false;
			if (!IsAllowedTargetTableExpression(insertSpecification.Target))
			{
				flag = true;
			}
			else if (insertSpecification.OutputClause != null)
			{
				flag = true;
			}
			else if (insertSpecification.OutputIntoClause != null && !IsAllowedTargetTableExpression(insertSpecification.OutputIntoClause.IntoClause.Target))
			{
				flag = true;
			}
			if (flag)
			{
				ReportInvalidUseOfSideEffectingOperator(statement);
			}
		}

		public void Visit(SqlMergeStatement statement)
		{
			SqlMergeSpecification mergeSpecification = statement.MergeSpecification;
			bool flag = false;
			if (!IsAllowedTargetTableExpression(mergeSpecification.Target))
			{
				flag = true;
			}
			else if (mergeSpecification.OutputClause != null)
			{
				flag = true;
			}
			else if (mergeSpecification.OutputIntoClause != null && !IsAllowedTargetTableExpression(mergeSpecification.OutputIntoClause.IntoClause.Target))
			{
				flag = true;
			}
			if (flag)
			{
				ReportInvalidUseOfSideEffectingOperator(statement);
			}
		}

		public void Visit(SqlStatement statement)
		{
		}

		public void Visit(SqlRestoreDatabaseStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlRestoreInformationStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlRestoreLogStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlRestoreMasterKeyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlRestoreServiceMasterKeyStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlRestoreTableStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlReturnStatement statement)
		{
		}

		public void Visit(SqlRevokeStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlSelectStatement statement)
		{
			if (statement.GetIntoClause() != null)
			{
				ReportInvalidUseOfSideEffectingOperator(statement);
				return;
			}
			bool flag;
			if (statement.SelectSpecification.QueryExpression is SqlQuerySpecification sqlQuerySpecification)
			{
				SelectExpressionType selectExpressionType = SelectExpressionType.Unknown;
				foreach (SqlSelectExpression selectExpression in sqlQuerySpecification.SelectClause.SelectExpressions)
				{
					selectExpressionType |= selectExpression.ExpressionType;
				}
				flag = (selectExpressionType | (SelectExpressionType.VariableAssignment | SelectExpressionType.Error)) != (SelectExpressionType.VariableAssignment | SelectExpressionType.Error);
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				statement.ReportBindError(SR.SelectStatementWithinFunctionCannotReturnData);
			}
		}

		public void Visit(SqlSetAssignmentStatement statement)
		{
		}

		public void Visit(SqlSetStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlTryCatchStatement statement)
		{
			ReportInvalidUseOfSideEffectingOperator(statement);
		}

		public void Visit(SqlUpdateStatement statement)
		{
		}

		public void Visit(SqlUseStatement statement)
		{
		}

		public void Visit(SqlVariableDeclareStatement statement)
		{
		}

		public void Visit(SqlWhileStatement statement)
		{
		}
	}

	private static class ParserSingletonHelper
	{
		public static readonly SqlStatementRecursiveVisitor ScalarFunctionParserVisitor;

		public static readonly SqlStatementRecursiveVisitor TableFunctionParserVisitor;

		public static readonly SqlStatementRecursiveVisitor ProcedureParserVisitor;

		public static readonly SqlStatementRecursiveVisitor TriggerParserVisitor;

		static ParserSingletonHelper()
		{
			ScalarFunctionParserVisitor = new SqlStatementRecursiveVisitor(new ScalarFunctionStatementParserVisitor());
			TableFunctionParserVisitor = new SqlStatementRecursiveVisitor(new TableFunctionStatementParserVisitor());
			ProcedureParserVisitor = new SqlStatementRecursiveVisitor(new ProcedureStatementParserVisitor());
			TriggerParserVisitor = new SqlStatementRecursiveVisitor(new TriggerStatementParserVisitor());
		}
	}

	private abstract class ModuleStatementParserVisitorBase : ISqlStatementVisitor
	{
		public virtual void Visit(SqlAlterFunctionStatement statement)
		{
		}

		public void Visit(SqlAlterLoginStatement statement)
		{
		}

		public virtual void Visit(SqlAlterProcedureStatement statement)
		{
		}

		public virtual void Visit(SqlAlterTriggerStatement statement)
		{
		}

		public virtual void Visit(SqlAlterViewStatement statement)
		{
		}

		public virtual void Visit(SqlBackupCertificateStatement statement)
		{
		}

		public virtual void Visit(SqlBackupDatabaseStatement statement)
		{
		}

		public virtual void Visit(SqlBackupLogStatement statement)
		{
		}

		public virtual void Visit(SqlBackupMasterKeyStatement statement)
		{
		}

		public virtual void Visit(SqlBackupServiceMasterKeyStatement statement)
		{
		}

		public virtual void Visit(SqlBackupTableStatement statement)
		{
		}

		public virtual void Visit(SqlBatch statement)
		{
		}

		public virtual void Visit(SqlBreakStatement statement)
		{
		}

		public virtual void Visit(SqlCommentStatement statement)
		{
		}

		public virtual void Visit(SqlCompoundStatement statement)
		{
		}

		public virtual void Visit(SqlContinueStatement statement)
		{
		}

		public virtual void Visit(SqlCreateFunctionStatement statement)
		{
		}

		public virtual void Visit(SqlCreateIndexStatement statement)
		{
		}

		public virtual void Visit(SqlCreateLoginFromAsymKeyStatement statement)
		{
		}

		public virtual void Visit(SqlCreateLoginFromCertificateStatement statement)
		{
		}

		public virtual void Visit(SqlCreateLoginFromWindowsStatement statement)
		{
		}

		public virtual void Visit(SqlCreateLoginWithPasswordStatement statement)
		{
		}

		public virtual void Visit(SqlCreateProcedureStatement statement)
		{
		}

		public virtual void Visit(SqlCreateRoleStatement statement)
		{
		}

		public virtual void Visit(SqlCreateSchemaStatement statement)
		{
		}

		public virtual void Visit(SqlCreateSynonymStatement statement)
		{
		}

		public virtual void Visit(SqlCreateTableStatement statement)
		{
		}

		public virtual void Visit(SqlCreateTriggerStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserDefinedDataTypeStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserDefinedTableTypeStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserDefinedTypeStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserFromAsymKeyStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserFromCertificateStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserWithImplicitAuthenticationStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserFromLoginStatement statement)
		{
		}

		public void Visit(SqlCreateUserFromExternalProviderStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserStatement statement)
		{
		}

		public virtual void Visit(SqlCreateUserWithoutLoginStatement statement)
		{
		}

		public virtual void Visit(SqlCreateViewStatement statement)
		{
		}

		public virtual void Visit(SqlCursorDeclareStatement statement)
		{
		}

		public virtual void Visit(SqlDBCCStatement statement)
		{
		}

		public virtual void Visit(SqlDeleteStatement statement)
		{
		}

		public virtual void Visit(SqlDenyStatement statement)
		{
		}

		public virtual void Visit(SqlDropAggregateStatement statement)
		{
		}

		public virtual void Visit(SqlDropDatabaseStatement statement)
		{
		}

		public virtual void Visit(SqlDropDefaultStatement statement)
		{
		}

		public virtual void Visit(SqlDropFunctionStatement statement)
		{
		}

		public virtual void Visit(SqlDropLoginStatement statement)
		{
		}

		public virtual void Visit(SqlDropProcedureStatement statement)
		{
		}

		public virtual void Visit(SqlDropRuleStatement statement)
		{
		}

		public virtual void Visit(SqlDropSecurityPolicyStatement statement)
		{
		}

		public virtual void Visit(SqlDropSequenceStatement statement)
		{
		}

		public virtual void Visit(SqlDropSchemaStatement statement)
		{
		}

		public virtual void Visit(SqlDropSynonymStatement statement)
		{
		}

		public virtual void Visit(SqlDropTableStatement statement)
		{
		}

		public virtual void Visit(SqlDropTriggerStatement statement)
		{
		}

		public virtual void Visit(SqlDropTypeStatement statement)
		{
		}

		public virtual void Visit(SqlDropUserStatement statement)
		{
		}

		public virtual void Visit(SqlDropViewStatement statement)
		{
		}

		public virtual void Visit(SqlExecuteModuleStatement statement)
		{
		}

		public virtual void Visit(SqlExecuteStringStatement statement)
		{
		}

		public virtual void Visit(SqlGrantStatement statement)
		{
		}

		public virtual void Visit(SqlIfElseStatement statement)
		{
		}

		public virtual void Visit(SqlInlineTableVariableDeclareStatement statement)
		{
		}

		public virtual void Visit(SqlInsertStatement statement)
		{
		}

		public virtual void Visit(SqlMergeStatement statement)
		{
		}

		public virtual void Visit(SqlStatement statement)
		{
		}

		public virtual void Visit(SqlRestoreDatabaseStatement statement)
		{
		}

		public virtual void Visit(SqlRestoreInformationStatement statement)
		{
		}

		public virtual void Visit(SqlRestoreLogStatement statement)
		{
		}

		public virtual void Visit(SqlRestoreMasterKeyStatement statement)
		{
		}

		public virtual void Visit(SqlRestoreServiceMasterKeyStatement statement)
		{
		}

		public virtual void Visit(SqlRestoreTableStatement statement)
		{
		}

		public virtual void Visit(SqlReturnStatement statement)
		{
		}

		public virtual void Visit(SqlRevokeStatement statement)
		{
		}

		public virtual void Visit(SqlSelectStatement statement)
		{
		}

		public virtual void Visit(SqlSetAssignmentStatement statement)
		{
		}

		public virtual void Visit(SqlSetStatement statement)
		{
		}

		public virtual void Visit(SqlTryCatchStatement statement)
		{
		}

		public virtual void Visit(SqlUpdateStatement statement)
		{
		}

		public virtual void Visit(SqlUseStatement statement)
		{
			statement.ReportParseError(SR.UseDatabaseStatementNotAllowed);
		}

		public virtual void Visit(SqlVariableDeclareStatement statement)
		{
		}

		public virtual void Visit(SqlWhileStatement statement)
		{
		}
	}

	private sealed class ScalarFunctionStatementParserVisitor : ModuleStatementParserVisitorBase
	{
		public override void Visit(SqlReturnStatement statement)
		{
			if (statement.Expression == null)
			{
				statement.ReportParseError(SR.ReturnStatementInScalarValuedFunctionMustIncludeArg);
			}
		}
	}

	private sealed class TableFunctionStatementParserVisitor : ModuleStatementParserVisitorBase
	{
		public override void Visit(SqlReturnStatement statement)
		{
			if (statement.Expression != null)
			{
				statement.ReportParseError(SR.UseReturnStatementWithValueCannotBeUsed);
			}
		}
	}

	private sealed class ProcedureStatementParserVisitor : ModuleStatementParserVisitorBase
	{
	}

	private sealed class TriggerStatementParserVisitor : ModuleStatementParserVisitorBase
	{
	}

	public static SqlStatementRecursiveVisitor FunctionBinderVisitor => BinderSingletonHelper.FunctionBinderVisitor;

	public static SqlStatementRecursiveVisitor ScalarFunctionParserVisitor => ParserSingletonHelper.ScalarFunctionParserVisitor;

	public static SqlStatementRecursiveVisitor TableFunctionParserVisitor => ParserSingletonHelper.TableFunctionParserVisitor;

	public static SqlStatementRecursiveVisitor ProcedureParserVisitor => ParserSingletonHelper.ProcedureParserVisitor;

	public static SqlStatementRecursiveVisitor TriggerParserVisitor => ParserSingletonHelper.TriggerParserVisitor;
}
