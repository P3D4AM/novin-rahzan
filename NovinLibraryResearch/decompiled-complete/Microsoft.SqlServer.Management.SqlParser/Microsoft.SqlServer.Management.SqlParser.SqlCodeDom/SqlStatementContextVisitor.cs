namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlStatementContextVisitor<C> : ISqlStatementContextVisitor<C>
{
	public virtual void Visit(SqlAlterFunctionStatement statement, C context)
	{
	}

	public virtual void Visit(SqlAlterLoginStatement statement, C context)
	{
	}

	public virtual void Visit(SqlAlterProcedureStatement statement, C context)
	{
	}

	public virtual void Visit(SqlAlterTriggerStatement statement, C context)
	{
	}

	public virtual void Visit(SqlAlterViewStatement statement, C context)
	{
	}

	public virtual void Visit(SqlBackupCertificateStatement statement, C context)
	{
	}

	public virtual void Visit(SqlBackupDatabaseStatement statement, C context)
	{
	}

	public virtual void Visit(SqlBackupLogStatement statement, C context)
	{
	}

	public virtual void Visit(SqlBackupMasterKeyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlBackupServiceMasterKeyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlBackupTableStatement statement, C context)
	{
	}

	public virtual void Visit(SqlBreakStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCommentStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCompoundStatement statement, C context)
	{
	}

	public virtual void Visit(SqlContinueStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateFunctionStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateIndexStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateLoginFromAsymKeyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateLoginFromCertificateStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateLoginFromWindowsStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateLoginWithPasswordStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateProcedureStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateRoleStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateSchemaStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateSynonymStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateTableStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateTriggerStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserDefinedDataTypeStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserDefinedTableTypeStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserDefinedTypeStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserFromAsymKeyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserFromCertificateStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserWithImplicitAuthenticationStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserFromLoginStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserFromExternalProviderStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateUserWithoutLoginStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCreateViewStatement statement, C context)
	{
	}

	public virtual void Visit(SqlCursorDeclareStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDBCCStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDeleteStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDenyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropAggregateStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropDatabaseStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropDefaultStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropFunctionStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropLoginStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropProcedureStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropRuleStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropSchemaStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropSecurityPolicyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropSequenceStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropSynonymStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropTableStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropTriggerStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropTypeStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropUserStatement statement, C context)
	{
	}

	public virtual void Visit(SqlDropViewStatement statement, C context)
	{
	}

	public virtual void Visit(SqlExecuteModuleStatement statement, C context)
	{
	}

	public virtual void Visit(SqlExecuteStringStatement statement, C context)
	{
	}

	public virtual void Visit(SqlGrantStatement statement, C context)
	{
	}

	public virtual void Visit(SqlIfElseStatement statement, C context)
	{
	}

	public virtual void Visit(SqlInlineTableVariableDeclareStatement statement, C context)
	{
	}

	public virtual void Visit(SqlInsertStatement statement, C context)
	{
	}

	public virtual void Visit(SqlMergeStatement statement, C context)
	{
	}

	public virtual void Visit(SqlStatement statement, C context)
	{
	}

	public virtual void Visit(SqlRestoreDatabaseStatement statement, C context)
	{
	}

	public virtual void Visit(SqlRestoreInformationStatement statement, C context)
	{
	}

	public virtual void Visit(SqlRestoreLogStatement statement, C context)
	{
	}

	public virtual void Visit(SqlRestoreMasterKeyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlRestoreServiceMasterKeyStatement statement, C context)
	{
	}

	public virtual void Visit(SqlRestoreTableStatement statement, C context)
	{
	}

	public virtual void Visit(SqlReturnStatement statement, C context)
	{
	}

	public virtual void Visit(SqlRevokeStatement statement, C context)
	{
	}

	public virtual void Visit(SqlSelectStatement statement, C context)
	{
	}

	public virtual void Visit(SqlSetAssignmentStatement statement, C context)
	{
	}

	public virtual void Visit(SqlSetStatement statement, C context)
	{
	}

	public virtual void Visit(SqlTryCatchStatement statement, C context)
	{
	}

	public virtual void Visit(SqlUpdateStatement statement, C context)
	{
	}

	public virtual void Visit(SqlUseStatement statement, C context)
	{
	}

	public virtual void Visit(SqlVariableDeclareStatement statement, C context)
	{
	}

	public virtual void Visit(SqlWhileStatement statement, C context)
	{
	}
}
public class SqlStatementContextVisitor<T, C> : ISqlStatementContextVisitor<T, C>
{
	public virtual T Visit(SqlAlterFunctionStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlAlterLoginStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlAlterProcedureStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlAlterTriggerStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlAlterViewStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlBackupCertificateStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlBackupDatabaseStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlBackupLogStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlBackupMasterKeyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlBackupServiceMasterKeyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlBackupTableStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlBreakStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCommentStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCompoundStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlContinueStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateFunctionStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateIndexStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromAsymKeyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromCertificateStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginFromWindowsStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateLoginWithPasswordStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateProcedureStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateRoleStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateSchemaStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateSynonymStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateTableStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateTriggerStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedDataTypeStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedTableTypeStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserDefinedTypeStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromAsymKeyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromCertificateStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserWithImplicitAuthenticationStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromLoginStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserFromExternalProviderStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateUserWithoutLoginStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCreateViewStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlCursorDeclareStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDBCCStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDeleteStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDenyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropAggregateStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropDatabaseStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropDefaultStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropFunctionStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropLoginStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropProcedureStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropRuleStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropSchemaStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropSecurityPolicyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropSequenceStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropSynonymStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropTableStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropTriggerStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropTypeStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropUserStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlDropViewStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlExecuteModuleStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlExecuteStringStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlGrantStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlIfElseStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlInlineTableVariableDeclareStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlInsertStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlMergeStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlRestoreDatabaseStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlRestoreInformationStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlRestoreLogStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlRestoreMasterKeyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlRestoreServiceMasterKeyStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlRestoreTableStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlReturnStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlRevokeStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlSelectStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlSetAssignmentStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlSetStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlTryCatchStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlUpdateStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlUseStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlVariableDeclareStatement statement, C context)
	{
		return default(T);
	}

	public virtual T Visit(SqlWhileStatement statement, C context)
	{
		return default(T);
	}
}
