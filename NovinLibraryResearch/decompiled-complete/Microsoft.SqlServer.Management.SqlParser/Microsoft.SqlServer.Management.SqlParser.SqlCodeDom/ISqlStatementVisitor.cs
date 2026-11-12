namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public interface ISqlStatementVisitor
{
	void Visit(SqlAlterFunctionStatement statement);

	void Visit(SqlAlterLoginStatement statement);

	void Visit(SqlAlterProcedureStatement statement);

	void Visit(SqlAlterTriggerStatement statement);

	void Visit(SqlAlterViewStatement statement);

	void Visit(SqlBackupCertificateStatement statement);

	void Visit(SqlBackupDatabaseStatement statement);

	void Visit(SqlBackupLogStatement statement);

	void Visit(SqlBackupMasterKeyStatement statement);

	void Visit(SqlBackupServiceMasterKeyStatement statement);

	void Visit(SqlBackupTableStatement statement);

	void Visit(SqlBreakStatement statement);

	void Visit(SqlCommentStatement statement);

	void Visit(SqlCompoundStatement statement);

	void Visit(SqlContinueStatement statement);

	void Visit(SqlCreateFunctionStatement statement);

	void Visit(SqlCreateIndexStatement statement);

	void Visit(SqlCreateLoginFromAsymKeyStatement statement);

	void Visit(SqlCreateLoginFromCertificateStatement statement);

	void Visit(SqlCreateLoginFromWindowsStatement statement);

	void Visit(SqlCreateLoginWithPasswordStatement statement);

	void Visit(SqlCreateProcedureStatement statement);

	void Visit(SqlCreateRoleStatement statement);

	void Visit(SqlCreateSchemaStatement statement);

	void Visit(SqlCreateSynonymStatement statement);

	void Visit(SqlCreateTableStatement statement);

	void Visit(SqlCreateTriggerStatement statement);

	void Visit(SqlCreateUserDefinedDataTypeStatement statement);

	void Visit(SqlCreateUserDefinedTableTypeStatement statement);

	void Visit(SqlCreateUserDefinedTypeStatement statement);

	void Visit(SqlCreateUserFromAsymKeyStatement statement);

	void Visit(SqlCreateUserFromCertificateStatement statement);

	void Visit(SqlCreateUserWithImplicitAuthenticationStatement statement);

	void Visit(SqlCreateUserFromLoginStatement statement);

	void Visit(SqlCreateUserFromExternalProviderStatement statement);

	void Visit(SqlCreateUserStatement statement);

	void Visit(SqlCreateUserWithoutLoginStatement statement);

	void Visit(SqlCreateViewStatement statement);

	void Visit(SqlCursorDeclareStatement statement);

	void Visit(SqlDBCCStatement statement);

	void Visit(SqlDeleteStatement statement);

	void Visit(SqlDenyStatement statement);

	void Visit(SqlDropAggregateStatement statement);

	void Visit(SqlDropDatabaseStatement statement);

	void Visit(SqlDropDefaultStatement statement);

	void Visit(SqlDropFunctionStatement statement);

	void Visit(SqlDropLoginStatement statement);

	void Visit(SqlDropProcedureStatement statement);

	void Visit(SqlDropRuleStatement statement);

	void Visit(SqlDropSchemaStatement statement);

	void Visit(SqlDropSecurityPolicyStatement statement);

	void Visit(SqlDropSequenceStatement statement);

	void Visit(SqlDropSynonymStatement statement);

	void Visit(SqlDropTableStatement statement);

	void Visit(SqlDropTriggerStatement statement);

	void Visit(SqlDropTypeStatement statement);

	void Visit(SqlDropUserStatement statement);

	void Visit(SqlDropViewStatement statement);

	void Visit(SqlExecuteModuleStatement statement);

	void Visit(SqlExecuteStringStatement statement);

	void Visit(SqlGrantStatement statement);

	void Visit(SqlIfElseStatement statement);

	void Visit(SqlInlineTableVariableDeclareStatement statement);

	void Visit(SqlInsertStatement statement);

	void Visit(SqlMergeStatement statement);

	void Visit(SqlStatement statement);

	void Visit(SqlRestoreDatabaseStatement statement);

	void Visit(SqlRestoreInformationStatement statement);

	void Visit(SqlRestoreLogStatement statement);

	void Visit(SqlRestoreMasterKeyStatement statement);

	void Visit(SqlRestoreServiceMasterKeyStatement statement);

	void Visit(SqlRestoreTableStatement statement);

	void Visit(SqlReturnStatement statement);

	void Visit(SqlRevokeStatement statement);

	void Visit(SqlSelectStatement statement);

	void Visit(SqlSetAssignmentStatement statement);

	void Visit(SqlSetStatement statement);

	void Visit(SqlTryCatchStatement statement);

	void Visit(SqlUpdateStatement statement);

	void Visit(SqlUseStatement statement);

	void Visit(SqlVariableDeclareStatement statement);

	void Visit(SqlWhileStatement statement);
}
public interface ISqlStatementVisitor<T>
{
	T Visit(SqlAlterFunctionStatement statement);

	T Visit(SqlAlterLoginStatement statement);

	T Visit(SqlAlterProcedureStatement statement);

	T Visit(SqlAlterTriggerStatement statement);

	T Visit(SqlAlterViewStatement statement);

	T Visit(SqlBackupCertificateStatement statement);

	T Visit(SqlBackupDatabaseStatement statement);

	T Visit(SqlBackupLogStatement statement);

	T Visit(SqlBackupMasterKeyStatement statement);

	T Visit(SqlBackupServiceMasterKeyStatement statement);

	T Visit(SqlBackupTableStatement statement);

	T Visit(SqlBreakStatement statement);

	T Visit(SqlCommentStatement statement);

	T Visit(SqlCompoundStatement statement);

	T Visit(SqlContinueStatement statement);

	T Visit(SqlCreateFunctionStatement statement);

	T Visit(SqlCreateIndexStatement statement);

	T Visit(SqlCreateLoginFromAsymKeyStatement statement);

	T Visit(SqlCreateLoginFromCertificateStatement statement);

	T Visit(SqlCreateLoginFromWindowsStatement statement);

	T Visit(SqlCreateLoginWithPasswordStatement statement);

	T Visit(SqlCreateProcedureStatement statement);

	T Visit(SqlCreateRoleStatement statement);

	T Visit(SqlCreateSchemaStatement statement);

	T Visit(SqlCreateSynonymStatement statement);

	T Visit(SqlCreateTableStatement statement);

	T Visit(SqlCreateTriggerStatement statement);

	T Visit(SqlCreateUserDefinedDataTypeStatement statement);

	T Visit(SqlCreateUserDefinedTableTypeStatement statement);

	T Visit(SqlCreateUserDefinedTypeStatement statement);

	T Visit(SqlCreateUserFromAsymKeyStatement statement);

	T Visit(SqlCreateUserFromCertificateStatement statement);

	T Visit(SqlCreateUserWithImplicitAuthenticationStatement statement);

	T Visit(SqlCreateUserFromLoginStatement statement);

	T Visit(SqlCreateUserFromExternalProviderStatement statement);

	T Visit(SqlCreateUserStatement statement);

	T Visit(SqlCreateUserWithoutLoginStatement statement);

	T Visit(SqlCreateViewStatement statement);

	T Visit(SqlCursorDeclareStatement statement);

	T Visit(SqlDBCCStatement statement);

	T Visit(SqlDeleteStatement statement);

	T Visit(SqlDenyStatement statement);

	T Visit(SqlDropAggregateStatement statement);

	T Visit(SqlDropDatabaseStatement statement);

	T Visit(SqlDropDefaultStatement statement);

	T Visit(SqlDropFunctionStatement statement);

	T Visit(SqlDropLoginStatement statement);

	T Visit(SqlDropProcedureStatement statement);

	T Visit(SqlDropRuleStatement statement);

	T Visit(SqlDropSchemaStatement statement);

	T Visit(SqlDropSecurityPolicyStatement statement);

	T Visit(SqlDropSequenceStatement statement);

	T Visit(SqlDropSynonymStatement statement);

	T Visit(SqlDropTableStatement statement);

	T Visit(SqlDropTriggerStatement statement);

	T Visit(SqlDropTypeStatement statement);

	T Visit(SqlDropUserStatement statement);

	T Visit(SqlDropViewStatement statement);

	T Visit(SqlExecuteModuleStatement statement);

	T Visit(SqlExecuteStringStatement statement);

	T Visit(SqlGrantStatement statement);

	T Visit(SqlIfElseStatement statement);

	T Visit(SqlInlineTableVariableDeclareStatement statement);

	T Visit(SqlInsertStatement statement);

	T Visit(SqlMergeStatement statement);

	T Visit(SqlStatement statement);

	T Visit(SqlRestoreDatabaseStatement statement);

	T Visit(SqlRestoreInformationStatement statement);

	T Visit(SqlRestoreLogStatement statement);

	T Visit(SqlRestoreMasterKeyStatement statement);

	T Visit(SqlRestoreServiceMasterKeyStatement statement);

	T Visit(SqlRestoreTableStatement statement);

	T Visit(SqlReturnStatement statement);

	T Visit(SqlRevokeStatement statement);

	T Visit(SqlSelectStatement statement);

	T Visit(SqlSetAssignmentStatement statement);

	T Visit(SqlSetStatement statement);

	T Visit(SqlTryCatchStatement statement);

	T Visit(SqlUpdateStatement statement);

	T Visit(SqlUseStatement statement);

	T Visit(SqlVariableDeclareStatement statement);

	T Visit(SqlWhileStatement statement);
}
