using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal sealed class BindWorkList
{
	private static class BucketSize
	{
		public const int XSmall = 4;

		public const int Small = 8;

		public const int Medium = 16;

		public const int Large = 32;

		public const int XLarge = 64;
	}

	private class Bucket : List<SqlDdlStatement>
	{
		public Bucket(int initialCapacity)
			: base(initialCapacity)
		{
		}
	}

	private abstract class OwnedObjectWorkList
	{
		protected readonly Bucket[] m_buckets;

		protected OwnedObjectWorkList(params int[] bucketSizes)
		{
			m_buckets = new Bucket[bucketSizes.Length];
			for (int i = 0; i < m_buckets.Length; i++)
			{
				m_buckets[i] = new Bucket(bucketSizes[i]);
			}
		}

		public bool AddStatement(SqlDdlStatement statement)
		{
			int statementBucketIndex = GetStatementBucketIndex(statement);
			if (statementBucketIndex >= 0)
			{
				m_buckets[statementBucketIndex].Add(statement);
				return true;
			}
			return false;
		}

		public void BindAll(BatchBindingContext bindingContext)
		{
			for (int i = 0; i < m_buckets.Length; i++)
			{
				foreach (SqlDdlStatement item in m_buckets[i])
				{
					item.Bind(bindingContext, StatementBindMode.OutOfOrder);
				}
			}
		}

		public void FinalizeAll(BatchBindingContext bindingContext)
		{
			for (int i = 0; i < m_buckets.Length; i++)
			{
				foreach (SqlDdlStatement item in m_buckets[i])
				{
					item.FinalizeBind(bindingContext, StatementBindMode.OutOfOrder);
					item.PostBindFinalize();
				}
			}
		}

		public abstract void RunPostBindBuildModeChecks();

		protected abstract int GetStatementBucketIndex(SqlDdlStatement statement);
	}

	private sealed class ServerOwnedWorkList : OwnedObjectWorkList
	{
		private static readonly int[] BucketSizes = new int[1] { 8 };

		public ServerOwnedWorkList()
			: base(BucketSizes)
		{
		}

		protected override int GetStatementBucketIndex(SqlDdlStatement statement)
		{
			DdlTargetObjectType targetObjectType = statement.TargetObjectType;
			if (targetObjectType == DdlTargetObjectType.Database || targetObjectType == DdlTargetObjectType.Login)
			{
				return 0;
			}
			return -1;
		}

		public override void RunPostBindBuildModeChecks()
		{
			for (int i = 0; i < m_buckets.Length; i++)
			{
				foreach (SqlDdlStatement item in m_buckets[i])
				{
					RunPostBindBuildModeChecks(item);
				}
			}
		}

		private void RunPostBindBuildModeChecks(SqlDdlStatement ddlStatement)
		{
		}
	}

	private sealed class DatabaseOwnedWorkList : OwnedObjectWorkList
	{
		private static readonly int[] BucketSizes = new int[2] { 8, 8 };

		public DatabaseOwnedWorkList()
			: base(BucketSizes)
		{
		}

		protected override int GetStatementBucketIndex(SqlDdlStatement statement)
		{
			switch (statement.TargetObjectType)
			{
			case DdlTargetObjectType.User:
				return 0;
			case DdlTargetObjectType.Role:
			case DdlTargetObjectType.Schema:
				return 1;
			default:
				return -1;
			}
		}

		public override void RunPostBindBuildModeChecks()
		{
			for (int i = 0; i < m_buckets.Length; i++)
			{
				foreach (SqlDdlStatement item in m_buckets[i])
				{
					RunPostBindBuildModeChecks(item);
				}
			}
		}

		private void RunPostBindBuildModeChecks(SqlDdlStatement ddlStatement)
		{
			if (ddlStatement is SqlCreateUserFromLoginStatement { BoundUser: { Login: null } } sqlCreateUserFromLoginStatement)
			{
				sqlCreateUserFromLoginStatement.LoginName.ReportBindError(SR.CouldNotFindLogin(sqlCreateUserFromLoginStatement.LoginName.Value));
			}
			if (ddlStatement is SqlCreateUserStatement { BoundUser: { } boundUser2, DefaultSchema: not null } sqlCreateUserStatement && boundUser2.DefaultSchema == null)
			{
				sqlCreateUserStatement.DefaultSchema.ReportBindError(SR.SchemaNotExist(sqlCreateUserStatement.DefaultSchema.Value));
			}
		}
	}

	private sealed class SchemaOwnedWorkList : OwnedObjectWorkList
	{
		private static readonly int[] BucketSizes = new int[6] { 16, 32, 32, 32, 32, 32 };

		public SchemaOwnedWorkList()
			: base(BucketSizes)
		{
		}

		protected override int GetStatementBucketIndex(SqlDdlStatement statement)
		{
			switch (statement.TargetObjectType)
			{
			case DdlTargetObjectType.Type:
				return 0;
			case DdlTargetObjectType.Aggregate:
			case DdlTargetObjectType.Function:
				return 1;
			case DdlTargetObjectType.Table:
				return 2;
			case DdlTargetObjectType.View:
				return 3;
			case DdlTargetObjectType.Procedure:
			case DdlTargetObjectType.Trigger:
				return 4;
			case DdlTargetObjectType.Index:
				return 5;
			default:
				return -1;
			}
		}

		public override void RunPostBindBuildModeChecks()
		{
			for (int i = 0; i < m_buckets.Length; i++)
			{
				foreach (SqlDdlStatement item in m_buckets[i])
				{
					RunPostBindBuildModeChecks(item);
				}
			}
		}

		private void RunPostBindBuildModeChecks(SqlDdlStatement ddlStatement)
		{
		}
	}

	private class CodeObjectValidator : SqlCodeObjectVisitor<bool?>
	{
		private static readonly CodeObjectValidator Instance = new CodeObjectValidator();

		private CodeObjectValidator()
		{
		}

		public static bool Validate(SqlCodeObject codeObject)
		{
			return Instance.DispatchVisit(codeObject);
		}

		public override bool? Visit(Microsoft.SqlServer.Management.SqlParser.SqlCodeDom.SqlDataType codeObject)
		{
			DataTypeSpec typeSpec = codeObject.GetTypeSpec();
			if (typeSpec != null)
			{
				Microsoft.SqlServer.Management.SqlParser.Metadata.SqlDataType sqlDataType = typeSpec.SqlDataType;
				if ((uint)(sqlDataType - 11) <= 2u)
				{
					codeObject.ReportBindError(SR.InvalidBuildModeDataTypeUse(typeSpec.Name));
					return false;
				}
			}
			return true;
		}

		public override bool? Visit(SqlExecuteAsClause codeObject)
		{
			if (codeObject.Type == SqlModuleExecutionContextType.Self)
			{
				codeObject.ReportBindError(SR.InvalidBuildModeExecutionContextTypeSelf);
				return false;
			}
			return true;
		}

		private bool DispatchVisit(SqlCodeObject codeObject)
		{
			bool? flag = codeObject.Accept(this);
			if (!flag.HasValue)
			{
				flag = DefaultVisit(codeObject);
			}
			return flag.Value;
		}

		private bool DefaultVisit(SqlCodeObject codeObject)
		{
			return VisitList(codeObject.Children);
		}

		private bool VisitList<TElem>(IEnumerable<TElem> codeObjects) where TElem : SqlCodeObject
		{
			bool flag = true;
			foreach (TElem codeObject in codeObjects)
			{
				flag &= DispatchVisit(codeObject);
			}
			return flag;
		}
	}

	private readonly ServerOwnedWorkList m_serverOwnedList;

	private readonly DatabaseOwnedWorkList m_databaseOwnedList;

	private readonly SchemaOwnedWorkList m_schemaOwnedList;

	public BindWorkList()
	{
		m_serverOwnedList = new ServerOwnedWorkList();
		m_databaseOwnedList = new DatabaseOwnedWorkList();
		m_schemaOwnedList = new SchemaOwnedWorkList();
	}

	public void AddStatement(SqlStatement statement)
	{
		if (statement is SqlNullStatement)
		{
			statement.ReportBindError(SR.InvalidBuildModeSqlNullStatement(statement.StatementPhrase));
			return;
		}
		SqlDdlStatement asDdlStatement = statement.AsDdlStatement;
		if (asDdlStatement != null)
		{
			bool flag = CodeObjectValidator.Validate(statement);
			string preBindBuildModeErrorMessage = GetPreBindBuildModeErrorMessage(asDdlStatement);
			if (!string.IsNullOrEmpty(preBindBuildModeErrorMessage))
			{
				statement.ReportBindError(preBindBuildModeErrorMessage);
			}
			else if (flag)
			{
				if (m_schemaOwnedList.AddStatement(asDdlStatement) || m_databaseOwnedList.AddStatement(asDdlStatement))
				{
					_ = 1;
				}
				else
					m_serverOwnedList.AddStatement(asDdlStatement);
			}
		}
		else
		{
			statement.ReportBindError(SR.InvalidBuildModeSqlNullStatement(statement.StatementPhrase));
		}
	}

	public void BindAll(BatchBindingContext bindingContext)
	{
		m_serverOwnedList.BindAll(bindingContext);
		m_databaseOwnedList.BindAll(bindingContext);
		m_schemaOwnedList.BindAll(bindingContext);
		m_serverOwnedList.FinalizeAll(bindingContext);
		m_databaseOwnedList.FinalizeAll(bindingContext);
		m_schemaOwnedList.FinalizeAll(bindingContext);
	}

	public void RunPostBindBuildModeChecks()
	{
		m_serverOwnedList.RunPostBindBuildModeChecks();
		m_databaseOwnedList.RunPostBindBuildModeChecks();
		m_schemaOwnedList.RunPostBindBuildModeChecks();
	}

	private string GetPreBindBuildModeErrorMessage(SqlDdlStatement ddlStatement)
	{
		if (!ddlStatement.IsCreateStatement)
		{
			return SR.InvalidBuildModeSqlNullStatement(ddlStatement.StatementPhrase);
		}
		if (ddlStatement is SqlCreateSchemaStatement createSchemaStatement)
		{
			return GetPreBindBuildModeErrorMessage(createSchemaStatement);
		}
		if (ddlStatement is SqlCreateIndexStatement createIndexStatement)
		{
			return GetPreBindBuildModeErrorMessage(createIndexStatement);
		}
		if (ddlStatement is SqlCreateAlterProcedureStatementBase createProcStatement)
		{
			return GetPreBindBuildModeErrorMessage(createProcStatement);
		}
		if (ddlStatement is SqlCreateAlterFunctionStatementBase createFunctionStatement)
		{
			return GetPreBindBuildModeErrorMessage(createFunctionStatement);
		}
		if (ddlStatement is SqlCreateLoginStatement createLoginStatement)
		{
			return GetPreBindBuildModeErrorMessage(createLoginStatement);
		}
		if (ddlStatement is SqlCreateTriggerStatement createTriggerStatement)
		{
			return GetPreBindBuildModeErrorMessage(createTriggerStatement);
		}
		if (ddlStatement is SqlCreateAlterViewStatementBase createAlterViewStatementBase)
		{
			return GetPreBindBuildModeErrorMessage(createAlterViewStatementBase);
		}
		return null;
	}

	private string GetPreBindBuildModeErrorMessage(SqlCreateSchemaStatement createSchemaStatement)
	{
		if (createSchemaStatement.SchemaElements == null)
		{
			return null;
		}
		return SR.InvalidBuildModeStatementCreateSchema;
	}

	private string GetPreBindBuildModeErrorMessage(SqlCreateIndexStatement createIndexStatement)
	{
		if (!createIndexStatement.HasDropExistingOption)
		{
			return null;
		}
		return SR.InvalidBuildModeStatementCreateIndex;
	}

	private string GetPreBindBuildModeErrorMessage(SqlCreateAlterProcedureStatementBase createProcStatement)
	{
		string result = null;
		if (createProcStatement.Definition.Options != null)
		{
			foreach (SqlModuleOption option in createProcStatement.Definition.Options)
			{
				if (option.Type == SqlModuleOptionType.Encryption)
				{
					result = SR.InvalidBuildModeStatementCreateProcedureWithEncryption;
				}
			}
		}
		if (!IsParameterCollectionValidInBuildMode(createProcStatement.Definition.Parameters))
		{
			result = SR.InvalidBuildModeStatementCreateProcCursorParams;
		}
		return result;
	}

	private string GetPreBindBuildModeErrorMessage(SqlCreateAlterFunctionStatementBase createFunctionStatement)
	{
		string result = null;
		if (createFunctionStatement.Definition.Options != null)
		{
			foreach (SqlModuleOption option in createFunctionStatement.Definition.Options)
			{
				if (option.Type == SqlModuleOptionType.Encryption)
				{
					result = SR.InvalidBuildModeStatementCreateFunctionWithEncryption;
				}
			}
		}
		if (!IsParameterCollectionValidInBuildMode(createFunctionStatement.Definition.Parameters))
		{
			result = SR.InvalidBuildModeStatementCreateFunction;
		}
		if (createFunctionStatement.Definition.IsClrFunction)
		{
			result = SR.InvalidBuildModeSqlNullStatement(createFunctionStatement.StatementPhrase);
		}
		return result;
	}

	private string GetPreBindBuildModeErrorMessage(SqlCreateLoginStatement createLoginStatement)
	{
		string result = null;
		if (createLoginStatement is SqlCreateLoginWithPasswordStatement sqlCreateLoginWithPasswordStatement)
		{
			if (!sqlCreateLoginWithPasswordStatement.SqlPassword.MustChange)
			{
				result = SR.InvalidBuildModeStatementCreateLogin;
			}
			else if (sqlCreateLoginWithPasswordStatement.DefaultDatabase != null)
			{
				result = SR.InvalidBuildModeStatementCreateLoginWithDefaultDatabase;
			}
		}
		if (createLoginStatement is SqlCreateLoginFromWindowsStatement { DefaultDatabase: not null })
		{
			result = SR.InvalidBuildModeStatementCreateLoginWithDefaultDatabase;
		}
		return result;
	}

	private string GetPreBindBuildModeErrorMessage(SqlCreateTriggerStatement createTriggerStatement)
	{
		string result = null;
		if (createTriggerStatement.Definition.Options != null)
		{
			foreach (SqlModuleOption option in createTriggerStatement.Definition.Options)
			{
				if (option.Type == SqlModuleOptionType.Encryption)
				{
					result = SR.InvalidBuildModeStatementCreateTriggerWithEncryption;
				}
			}
		}
		if (createTriggerStatement.Definition.TriggerType == TriggerType.DdlTrigger)
		{
			result = SR.InvalidBuildModeStatementCreateTriggerDdl;
		}
		return result;
	}

	private string GetPreBindBuildModeErrorMessage(SqlCreateAlterViewStatementBase createAlterViewStatementBase)
	{
		string result = null;
		if (createAlterViewStatementBase.Definition.Options != null)
		{
			foreach (SqlModuleOption option in createAlterViewStatementBase.Definition.Options)
			{
				if (option.Type == SqlModuleOptionType.Encryption)
				{
					result = SR.InvalidBuildModeStatementCreateViewWithEncryption;
				}
			}
		}
		return result;
	}

	private bool IsParameterCollectionValidInBuildMode(SqlParameterDeclarationCollection parameters)
	{
		if (parameters == null || parameters.Count == 0)
		{
			return true;
		}
		foreach (SqlParameterDeclaration parameter in parameters)
		{
			if (parameter.Type.IsCursor)
			{
				return false;
			}
		}
		return true;
	}
}
