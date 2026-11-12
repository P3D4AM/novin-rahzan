using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCreateAlterProcedureStatementBase : SqlDdlStatement
{
	private IMutableStoredProcedure boundStoredProc;

	private readonly SqlProcedureDefinition definition;

	private readonly SqlStatementCollection statements;

	private readonly bool isOrAlterStatement;

	internal sealed override bool IsTrueLastStatement => true;

	internal sealed override bool IsModuleStatement => true;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Procedure;

	public IStoredProcedure BoundStoredProc => boundStoredProc;

	public override IMetadataObject BoundObject => boundStoredProc;

	public SqlProcedureDefinition Definition => definition;

	public SqlStatementCollection Statements => statements;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return definition;
			foreach (SqlStatement statement in statements)
			{
				yield return statement;
			}
		}
	}

	internal override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		string value = definition.Name.SchemaName.Value;
		string value2 = definition.Name.ObjectName.Value;
		LexicalScope execScope = GetLexicalScope();
		SchemaEx cDASchema = bindingContext.GetCDASchema(value, execScope);
		boundStoredProc = bindingContext.MetadataFactory.StoredProcedure.Create(cDASchema.ProxyObject, value2);
		IStoredProcedure storedProcedure = bindingContext.FindStoredProcedure(null, value, value2, execScope);
		if (IsCreateStatement)
		{
			if (!isOrAlterStatement || (isOrAlterStatement && storedProcedure == null))
			{
				if (cDASchema.IsEmptySchema)
				{
					if (value != null)
					{
						definition.Name.SchemaName.ReportBindError(SR.SchemaNotExist(value), ErrorFlags.UnresolvedDatabaseObjectReference);
					}
					base.BindingState |= BindingState.BindFailed;
				}
				else if (storedProcedure != null)
				{
					definition.Name.ReportBindError(SR.DatabaseObjectExist(value2));
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					cDASchema.StoredProcedures.Create(boundStoredProc, this, bindMode);
					base.BindingState |= BindingState.BindSucceeded;
				}
			}
			else
			{
				DoExecuteAlter(storedProcedure, this, bindingContext, execScope);
			}
		}
		else if (storedProcedure != null)
		{
			DoExecuteAlter(storedProcedure, this, bindingContext, execScope);
		}
		else
		{
			if (BatchBindingContext.ObjectExistsInSchema(cDASchema, value2, execScope))
			{
				definition.Name.ReportBindError(SR.CannotPerformAlterOnObject(definition.Name.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			else
			{
				definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			}
			base.BindingState |= BindingState.BindFailed;
		}
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		SchemaEx schema = bindingContext.SchemaToSchemaEx(boundStoredProc.Schema);
		BatchBindingContext orCreateModuleBindingContext = bindingContext.GetOrCreateModuleBindingContext(schema);
		definition.Bind(orCreateModuleBindingContext, boundStoredProc);
		base.BindingState |= definition.BindingState;
		foreach (SqlStatement statement in statements)
		{
			statement.Bind(orCreateModuleBindingContext);
			base.BindingState |= statement.BindingState;
		}
		if (bindMode == StatementBindMode.OutOfOrder)
		{
			boundStoredProc.BodyText = SqlCodeObject.GetSqlSource(statements);
		}
	}

	internal override void Unbind()
	{
		boundStoredProc = null;
		base.Unbind();
	}

	private static void DoExecuteAlter(IStoredProcedure alterProc, SqlCreateAlterProcedureStatementBase sqlCreateAlterProcedureStatementBase, BatchBindingContext bindingContext, LexicalScope execScope)
	{
		bindingContext.SchemaToSchemaEx(alterProc.Schema).StoredProcedures.Alter(sqlCreateAlterProcedureStatementBase.boundStoredProc, execScope);
		sqlCreateAlterProcedureStatementBase.BindingState |= BindingState.BindSucceeded;
	}

	internal SqlCreateAlterProcedureStatementBase(SqlProcedureDefinition definition, List<SqlStatement> statements)
	{
		this.definition = definition;
		this.statements = ((statements != null) ? new SqlStatementCollection(statements) : new SqlStatementCollection());
		foreach (SqlStatement statement in this.statements)
		{
			statement.Accept(ValidateModuleBodyVisitor.ProcedureParserVisitor);
		}
	}

	internal SqlCreateAlterProcedureStatementBase(SqlProcedureDefinitionForCreate definition, List<SqlStatement> statements)
	{
		this.definition = definition;
		this.statements = ((statements != null) ? new SqlStatementCollection(statements) : new SqlStatementCollection());
		isOrAlterStatement = definition.IsOrAlterStatement;
		foreach (SqlStatement statement in this.statements)
		{
			statement.Accept(ValidateModuleBodyVisitor.ProcedureParserVisitor);
		}
	}
}
