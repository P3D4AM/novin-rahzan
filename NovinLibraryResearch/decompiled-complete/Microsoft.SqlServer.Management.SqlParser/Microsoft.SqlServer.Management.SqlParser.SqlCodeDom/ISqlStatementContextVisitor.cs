namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface ISqlStatementContextVisitor<C>
{
	void Visit(SqlAlterFunctionStatement statement, C context);

	void Visit(SqlAlterLoginStatement statement, C context);

	void Visit(SqlAlterProcedureStatement statement, C context);

	void Visit(SqlAlterTriggerStatement statement, C context);

	void Visit(SqlAlterViewStatement statement, C context);

	void Visit(SqlBackupCertificateStatement statement, C context);

	void Visit(SqlBackupDatabaseStatement statement, C context);

	void Visit(SqlBackupLogStatement statement, C context);

	void Visit(SqlBackupMasterKeyStatement statement, C context);

	void Visit(SqlBackupServiceMasterKeyStatement statement, C context);

	void Visit(SqlBackupTableStatement statement, C context);

	void Visit(SqlBreakStatement statement, C context);

	void Visit(SqlCommentStatement statement, C context);

	void Visit(SqlCompoundStatement statement, C context);

	void Visit(SqlContinueStatement statement, C context);

	void Visit(SqlCreateFunctionStatement statement, C context);

	void Visit(SqlCreateIndexStatement statement, C context);

	void Visit(SqlCreateLoginFromAsymKeyStatement statement, C context);

	void Visit(SqlCreateLoginFromCertificateStatement statement, C context);

	void Visit(SqlCreateLoginFromWindowsStatement statement, C context);

	void Visit(SqlCreateLoginWithPasswordStatement statement, C context);

	void Visit(SqlCreateProcedureStatement statement, C context);

	void Visit(SqlCreateRoleStatement statement, C context);

	void Visit(SqlCreateSchemaStatement statement, C context);

	void Visit(SqlCreateSynonymStatement statement, C context);

	void Visit(SqlCreateTableStatement statement, C context);

	void Visit(SqlCreateTriggerStatement statement, C context);

	void Visit(SqlCreateUserDefinedDataTypeStatement statement, C context);

	void Visit(SqlCreateUserDefinedTableTypeStatement statement, C context);

	void Visit(SqlCreateUserDefinedTypeStatement statement, C context);

	void Visit(SqlCreateUserFromAsymKeyStatement statement, C context);

	void Visit(SqlCreateUserFromCertificateStatement statement, C context);

	void Visit(SqlCreateUserWithImplicitAuthenticationStatement statement, C context);

	void Visit(SqlCreateUserFromLoginStatement statement, C context);

	void Visit(SqlCreateUserFromExternalProviderStatement statement, C context);

	void Visit(SqlCreateUserStatement statement, C context);

	void Visit(SqlCreateUserWithoutLoginStatement statement, C context);

	void Visit(SqlCreateViewStatement statement, C context);

	void Visit(SqlCursorDeclareStatement statement, C context);

	void Visit(SqlDBCCStatement statement, C context);

	void Visit(SqlDeleteStatement statement, C context);

	void Visit(SqlDenyStatement statement, C context);

	void Visit(SqlDropAggregateStatement statement, C context);

	void Visit(SqlDropDatabaseStatement statement, C context);

	void Visit(SqlDropDefaultStatement statement, C context);

	void Visit(SqlDropFunctionStatement statement, C context);

	void Visit(SqlDropLoginStatement statement, C context);

	void Visit(SqlDropProcedureStatement statement, C context);

	void Visit(SqlDropRuleStatement statement, C context);

	void Visit(SqlDropSchemaStatement statement, C context);

	void Visit(SqlDropSecurityPolicyStatement statement, C context);

	void Visit(SqlDropSequenceStatement statement, C context);

	void Visit(SqlDropSynonymStatement statement, C context);

	void Visit(SqlDropTableStatement statement, C context);

	void Visit(SqlDropTriggerStatement statement, C context);

	void Visit(SqlDropTypeStatement statement, C context);

	void Visit(SqlDropUserStatement statement, C context);

	void Visit(SqlDropViewStatement statement, C context);

	void Visit(SqlExecuteModuleStatement statement, C context);

	void Visit(SqlExecuteStringStatement statement, C context);

	void Visit(SqlGrantStatement statement, C context);

	void Visit(SqlIfElseStatement statement, C context);

	void Visit(SqlInlineTableVariableDeclareStatement statement, C context);

	void Visit(SqlInsertStatement statement, C context);

	void Visit(SqlMergeStatement statement, C context);

	void Visit(SqlStatement statement, C context);

	void Visit(SqlRestoreDatabaseStatement statement, C context);

	void Visit(SqlRestoreInformationStatement statement, C context);

	void Visit(SqlRestoreLogStatement statement, C context);

	void Visit(SqlRestoreMasterKeyStatement statement, C context);

	void Visit(SqlRestoreServiceMasterKeyStatement statement, C context);

	void Visit(SqlRestoreTableStatement statement, C context);

	void Visit(SqlReturnStatement statement, C context);

	void Visit(SqlRevokeStatement statement, C context);

	void Visit(SqlSelectStatement statement, C context);

	void Visit(SqlSetAssignmentStatement statement, C context);

	void Visit(SqlSetStatement statement, C context);

	void Visit(SqlTryCatchStatement statement, C context);

	void Visit(SqlUpdateStatement statement, C context);

	void Visit(SqlUseStatement statement, C context);

	void Visit(SqlVariableDeclareStatement statement, C context);

	void Visit(SqlWhileStatement statement, C context);
}
public interface ISqlStatementContextVisitor<T, C>
{
	T Visit(SqlAlterFunctionStatement statement, C context);

	T Visit(SqlAlterLoginStatement statement, C context);

	T Visit(SqlAlterProcedureStatement statement, C context);

	T Visit(SqlAlterTriggerStatement statement, C context);

	T Visit(SqlAlterViewStatement statement, C context);

	T Visit(SqlBackupCertificateStatement statement, C context);

	T Visit(SqlBackupDatabaseStatement statement, C context);

	T Visit(SqlBackupLogStatement statement, C context);

	T Visit(SqlBackupMasterKeyStatement statement, C context);

	T Visit(SqlBackupServiceMasterKeyStatement statement, C context);

	T Visit(SqlBackupTableStatement statement, C context);

	T Visit(SqlBreakStatement statement, C context);

	T Visit(SqlCommentStatement statement, C context);

	T Visit(SqlCompoundStatement statement, C context);

	T Visit(SqlContinueStatement statement, C context);

	T Visit(SqlCreateFunctionStatement statement, C context);

	T Visit(SqlCreateIndexStatement statement, C context);

	T Visit(SqlCreateLoginFromAsymKeyStatement statement, C context);

	T Visit(SqlCreateLoginFromCertificateStatement statement, C context);

	T Visit(SqlCreateLoginFromWindowsStatement statement, C context);

	T Visit(SqlCreateLoginWithPasswordStatement statement, C context);

	T Visit(SqlCreateProcedureStatement statement, C context);

	T Visit(SqlCreateRoleStatement statement, C context);

	T Visit(SqlCreateSchemaStatement statement, C context);

	T Visit(SqlCreateSynonymStatement statement, C context);

	T Visit(SqlCreateTableStatement statement, C context);

	T Visit(SqlCreateTriggerStatement statement, C context);

	T Visit(SqlCreateUserDefinedDataTypeStatement statement, C context);

	T Visit(SqlCreateUserDefinedTableTypeStatement statement, C context);

	T Visit(SqlCreateUserDefinedTypeStatement statement, C context);

	T Visit(SqlCreateUserFromAsymKeyStatement statement, C context);

	T Visit(SqlCreateUserFromCertificateStatement statement, C context);

	T Visit(SqlCreateUserWithImplicitAuthenticationStatement statement, C context);

	T Visit(SqlCreateUserFromLoginStatement statement, C context);

	T Visit(SqlCreateUserFromExternalProviderStatement statement, C context);

	T Visit(SqlCreateUserStatement statement, C context);

	T Visit(SqlCreateUserWithoutLoginStatement statement, C context);

	T Visit(SqlCreateViewStatement statement, C context);

	T Visit(SqlCursorDeclareStatement statement, C context);

	T Visit(SqlDBCCStatement statement, C context);

	T Visit(SqlDeleteStatement statement, C context);

	T Visit(SqlDenyStatement statement, C context);

	T Visit(SqlDropAggregateStatement statement, C context);

	T Visit(SqlDropDatabaseStatement statement, C context);

	T Visit(SqlDropDefaultStatement statement, C context);

	T Visit(SqlDropFunctionStatement statement, C context);

	T Visit(SqlDropLoginStatement statement, C context);

	T Visit(SqlDropProcedureStatement statement, C context);

	T Visit(SqlDropRuleStatement statement, C context);

	T Visit(SqlDropSchemaStatement statement, C context);

	T Visit(SqlDropSecurityPolicyStatement statement, C context);

	T Visit(SqlDropSequenceStatement statement, C context);

	T Visit(SqlDropSynonymStatement statement, C context);

	T Visit(SqlDropTableStatement statement, C context);

	T Visit(SqlDropTriggerStatement statement, C context);

	T Visit(SqlDropTypeStatement statement, C context);

	T Visit(SqlDropUserStatement statement, C context);

	T Visit(SqlDropViewStatement statement, C context);

	T Visit(SqlExecuteModuleStatement statement, C context);

	T Visit(SqlExecuteStringStatement statement, C context);

	T Visit(SqlGrantStatement statement, C context);

	T Visit(SqlIfElseStatement statement, C context);

	T Visit(SqlInlineTableVariableDeclareStatement statement, C context);

	T Visit(SqlInsertStatement statement, C context);

	T Visit(SqlMergeStatement statement, C context);

	T Visit(SqlStatement statement, C context);

	T Visit(SqlRestoreDatabaseStatement statement, C context);

	T Visit(SqlRestoreInformationStatement statement, C context);

	T Visit(SqlRestoreLogStatement statement, C context);

	T Visit(SqlRestoreMasterKeyStatement statement, C context);

	T Visit(SqlRestoreServiceMasterKeyStatement statement, C context);

	T Visit(SqlRestoreTableStatement statement, C context);

	T Visit(SqlReturnStatement statement, C context);

	T Visit(SqlRevokeStatement statement, C context);

	T Visit(SqlSelectStatement statement, C context);

	T Visit(SqlSetAssignmentStatement statement, C context);

	T Visit(SqlSetStatement statement, C context);

	T Visit(SqlTryCatchStatement statement, C context);

	T Visit(SqlUpdateStatement statement, C context);

	T Visit(SqlUseStatement statement, C context);

	T Visit(SqlVariableDeclareStatement statement, C context);

	T Visit(SqlWhileStatement statement, C context);
}
