namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public sealed class SqlStatementRecursiveVisitor : ISqlStatementVisitor
{
	private readonly ISqlStatementVisitor visitor;

	public SqlStatementRecursiveVisitor(ISqlStatementVisitor visitor)
	{
		this.visitor = visitor;
	}

	public void Visit(SqlAlterFunctionStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlAlterLoginStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlAlterProcedureStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlAlterTriggerStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlAlterViewStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlBackupCertificateStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlBackupDatabaseStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlBackupLogStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlBackupMasterKeyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlBackupServiceMasterKeyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlBackupTableStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlBreakStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCommentStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCompoundStatement statement)
	{
		visitor.Visit(statement);
		foreach (SqlStatement statement2 in statement.Statements)
		{
			statement2.Accept(this);
		}
	}

	public void Visit(SqlContinueStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateFunctionStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateIndexStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateLoginFromAsymKeyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateLoginFromCertificateStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateLoginFromWindowsStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateLoginWithPasswordStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateProcedureStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateRoleStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateSchemaStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateSynonymStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateTableStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateTriggerStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserDefinedDataTypeStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserDefinedTableTypeStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserDefinedTypeStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserFromAsymKeyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserFromCertificateStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserWithImplicitAuthenticationStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserFromLoginStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserFromExternalProviderStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateUserWithoutLoginStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCreateViewStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlCursorDeclareStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDBCCStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDeleteStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDenyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropAggregateStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropDatabaseStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropDefaultStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropFunctionStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropLoginStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropProcedureStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropRuleStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropSchemaStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropSecurityPolicyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropSequenceStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropSynonymStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropTableStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropTriggerStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropTypeStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropUserStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlDropViewStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlExecuteModuleStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlExecuteStringStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlGrantStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlIfElseStatement statement)
	{
		visitor.Visit(statement);
		statement.TrueStatement.Accept(this);
		if (statement.FalseStatement != null)
		{
			statement.FalseStatement.Accept(this);
		}
	}

	public void Visit(SqlInlineTableVariableDeclareStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlInsertStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlMergeStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlRestoreDatabaseStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlRestoreInformationStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlRestoreLogStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlRestoreMasterKeyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlRestoreServiceMasterKeyStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlRestoreTableStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlReturnStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlRevokeStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlSelectStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlSetAssignmentStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlSetStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlTryCatchStatement statement)
	{
		visitor.Visit(statement);
		foreach (SqlStatement item in statement.TryBlock)
		{
			item.Accept(this);
		}
		foreach (SqlStatement item2 in statement.CatchBlock)
		{
			item2.Accept(this);
		}
	}

	public void Visit(SqlUpdateStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlUseStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlVariableDeclareStatement statement)
	{
		visitor.Visit(statement);
	}

	public void Visit(SqlWhileStatement statement)
	{
		visitor.Visit(statement);
		statement.TrueStatement.Accept(this);
	}
}
