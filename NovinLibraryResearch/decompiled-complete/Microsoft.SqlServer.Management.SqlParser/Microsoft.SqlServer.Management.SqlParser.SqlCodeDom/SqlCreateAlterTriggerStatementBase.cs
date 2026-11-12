using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlCreateAlterTriggerStatementBase : SqlDdlStatement
{
	private TriggerUnion<IMutableDmlTrigger, IMutableDatabaseDdlTrigger, IMutableServerDdlTrigger, IMutableTrigger> boundTrigger;

	private ITableViewBase boundTable;

	private bool executionSuccedded;

	private readonly SqlTriggerDefinition definition;

	private readonly SqlStatementCollection statements;

	private readonly bool isOrAlterStatement;

	internal sealed override bool IsTrueLastStatement => true;

	internal sealed override bool IsModuleStatement => true;

	internal sealed override bool IsDropStatement => false;

	internal sealed override DdlTargetObjectType TargetObjectType => DdlTargetObjectType.Trigger;

	public ITrigger BoundTrigger => boundTrigger.AsTrigger;

	public override IMetadataObject BoundObject => BoundTrigger;

	public SqlTriggerDefinition Definition => definition;

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

	private static IMutableTableViewBase GetAlterable(SchemaEx schema, ITableViewBase tableOrView, LexicalScope lexicalScope, StatementBindMode bindMode)
	{
		ITable immutableObject;
		if ((immutableObject = BindUtils.Table.AsTable(tableOrView)) != null)
		{
			return schema.Tables.GetAlterableObject(immutableObject, lexicalScope, bindMode);
		}
		IView immutableObject2;
		if ((immutableObject2 = BindUtils.Table.AsView(tableOrView)) != null)
		{
			return schema.Views.GetAlterableObject(immutableObject2, lexicalScope, bindMode);
		}
		return null;
	}

	private void InitiateBindDmlTrigger(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		LexicalScope execScope = GetLexicalScope();
		ITriggerFactory trigger = bindingContext.MetadataFactory.Trigger;
		IDmlTriggerDefinitionInfo dmlDefinitionInfo = definition.DmlDefinitionInfo;
		string value = definition.Name.SchemaName.Value;
		string value2 = definition.Name.ObjectName.Value;
		string value3 = dmlDefinitionInfo.TargetName.SchemaName.Value;
		string value4 = dmlDefinitionInfo.TargetName.ObjectName.Value;
		ITableViewBase targetTable;
		IDmlTrigger dmlTrigger = bindingContext.FindDmlTrigger(value, value4, value2, execScope, out targetTable);
		ITableViewBase tableViewBase = bindingContext.FindTableOrView(null, value3 ?? value, value4, execScope);
		ITableViewBase tableViewBase2 = tableViewBase;
		if (tableViewBase2 == null)
		{
			tableViewBase2 = bindingContext.GetCDASchema(value3 ?? value, execScope).EmptyTable;
		}
		IMutableDmlTrigger mutableDmlTrigger = trigger.CreateDmlTrigger(tableViewBase2, value2);
		boundTrigger = new TriggerUnion<IMutableDmlTrigger, IMutableDatabaseDdlTrigger, IMutableServerDdlTrigger, IMutableTrigger>(mutableDmlTrigger);
		boundTable = tableViewBase;
		base.BindingState |= BindingState.BindSucceeded;
		SchemaEx schemaEx;
		if (IsCreateStatement)
		{
			if (!isOrAlterStatement || (isOrAlterStatement && targetTable == null))
			{
				SqlIdentifier sqlIdentifier = ((!definition.Name.SchemaName.IsEmpty) ? definition.Name.SchemaName : dmlDefinitionInfo.TargetName.SchemaName);
				schemaEx = bindingContext.GetCDASchema(sqlIdentifier.Value, execScope);
				if (tableViewBase == null)
				{
					dmlDefinitionInfo.TargetName.ReportBindError(SR.ObjectNotExistOrIsInvalid(dmlDefinitionInfo.TargetName.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
					base.BindingState |= BindingState.BindFailed;
				}
				else if (dmlTrigger != null)
				{
					definition.Name.ReportBindError(SR.DatabaseObjectExist(definition.Name.ToString()));
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					SchemaEx schemaEx2 = bindingContext.SchemaToSchemaEx(tableViewBase.Schema);
					if (schemaEx.IsEmptySchema)
					{
						sqlIdentifier.ReportBindError(SR.SchemaNotExist(sqlIdentifier.Value), ErrorFlags.UnresolvedDatabaseObjectReference);
						base.BindingState |= BindingState.BindFailed;
					}
					else if (schemaEx2 != schemaEx)
					{
						sqlIdentifier.ReportBindError(SR.InvalidTriggerSchema(sqlIdentifier.Value));
						base.BindingState |= BindingState.BindFailed;
					}
					else if (tableViewBase.Triggers.Contains(value2))
					{
						definition.Name.ReportBindError(SR.DatabaseObjectExist(definition.Name.ToString()));
						base.BindingState |= BindingState.BindFailed;
					}
					else if (definition.Name.ObjectName.IsTempName)
					{
						definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()));
						base.BindingState |= BindingState.BindFailed;
					}
					else
					{
						IMutableTableViewBase alterable = GetAlterable(schemaEx, tableViewBase, execScope, bindMode);
						alterable.Triggers.Add(mutableDmlTrigger);
						boundTable = alterable;
						executionSuccedded = true;
					}
				}
			}
			else
			{
				schemaEx = GetSchema(tableViewBase, dmlTrigger, targetTable, this, bindingContext, execScope);
				DoExecuteAlterDmlTrigger(tableViewBase, targetTable, mutableDmlTrigger, this, schemaEx, bindingContext, execScope, bindMode);
			}
		}
		else
		{
			schemaEx = GetSchema(tableViewBase, dmlTrigger, targetTable, this, bindingContext, execScope);
			if (dmlTrigger == null)
			{
				definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
				base.BindingState |= BindingState.BindFailed;
			}
			else
			{
				DoExecuteAlterDmlTrigger(tableViewBase, targetTable, mutableDmlTrigger, this, schemaEx, bindingContext, execScope, bindMode);
			}
		}
		if (boundTable == null)
		{
			boundTable = bindingContext.MetadataFactory.Tabular.CreateTable(schemaEx, value4);
		}
		bindingContext.SetBoundObject(dmlDefinitionInfo.TargetName.ObjectName, boundTable);
		mutableDmlTrigger.InsteadOf = dmlDefinitionInfo.ActivationType == SqlTriggerActivationType.InsteadOf;
		if (dmlDefinitionInfo.Actions != null)
		{
			using (IEnumerator<SqlTriggerAction> enumerator = dmlDefinitionInfo.Actions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current.Type)
					{
					case SqlTriggerActionType.Delete:
						mutableDmlTrigger.Delete = true;
						break;
					case SqlTriggerActionType.Insert:
						mutableDmlTrigger.Insert = true;
						break;
					case SqlTriggerActionType.Update:
						mutableDmlTrigger.Update = true;
						break;
					default:
						base.BindingState |= BindingState.BindFailed;
						break;
					}
				}
				return;
			}
		}
		base.BindingState |= BindingState.BindFailed;
	}

	private void InitiateBindDatabaseDdlTrigger(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		LexicalScope execScope = GetLexicalScope();
		ITriggerFactory trigger = bindingContext.MetadataFactory.Trigger;
		_ = definition.DdlDefinitionInfo;
		DatabaseEx connectedDatabase = bindingContext.ConnectedDatabase;
		string value = definition.Name.ObjectName.Value;
		IMutableDatabaseDdlTrigger mutableDatabaseDdlTrigger = trigger.CreateDatabaseDdlTrigger(connectedDatabase.ProxyObject, value);
		boundTrigger = new TriggerUnion<IMutableDmlTrigger, IMutableDatabaseDdlTrigger, IMutableServerDdlTrigger, IMutableTrigger>(mutableDatabaseDdlTrigger);
		base.BindingState |= BindingState.BindSucceeded;
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		IDatabaseDdlTrigger databaseDdlTrigger = bindingContext.FindDatabaseTrigger(value, execScope);
		if (IsCreateStatement)
		{
			if (!isOrAlterStatement || (isOrAlterStatement && databaseDdlTrigger == null))
			{
				if (databaseDdlTrigger != null)
				{
					definition.Name.ReportBindError(SR.DatabaseObjectExist(definition.Name.ToString()));
					base.BindingState |= BindingState.BindFailed;
				}
				else if (definition.Name.ObjectName.IsTempName)
				{
					definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()));
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					connectedDatabase.Triggers.Create(mutableDatabaseDdlTrigger, this, bindMode);
				}
			}
			else
			{
				DoExecuteAlterDatabaseTrigger(connectedDatabase, mutableDatabaseDdlTrigger, this, execScope);
			}
		}
		else if (databaseDdlTrigger == null)
		{
			definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			DoExecuteAlterDatabaseTrigger(connectedDatabase, mutableDatabaseDdlTrigger, this, execScope);
		}
	}

	private void InitiateBindServerDdlTrigger(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		LexicalScope execScope = GetLexicalScope();
		ITriggerFactory trigger = bindingContext.MetadataFactory.Trigger;
		_ = definition.DdlDefinitionInfo;
		ServerEx server = bindingContext.GetServer();
		string value = definition.Name.ObjectName.Value;
		IMutableServerDdlTrigger mutableServerDdlTrigger = trigger.CreateServerDdlTrigger(server.ProxyObject, value);
		boundTrigger = new TriggerUnion<IMutableDmlTrigger, IMutableDatabaseDdlTrigger, IMutableServerDdlTrigger, IMutableTrigger>(mutableServerDdlTrigger);
		base.BindingState |= BindingState.BindSucceeded;
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		IServerDdlTrigger serverDdlTrigger = bindingContext.FindServerTrigger(value, execScope);
		if (IsCreateStatement)
		{
			if (!isOrAlterStatement || (isOrAlterStatement && serverDdlTrigger == null))
			{
				if (serverDdlTrigger != null)
				{
					definition.Name.ReportBindError(SR.DatabaseObjectExist(definition.Name.ToString()));
					base.BindingState |= BindingState.BindFailed;
				}
				else if (definition.Name.ObjectName.IsTempName)
				{
					definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()));
					base.BindingState |= BindingState.BindFailed;
				}
				else
				{
					server.Triggers.Create(mutableServerDdlTrigger, this, bindMode);
				}
			}
			else
			{
				DoExecuteAlterServerTrigger(server, mutableServerDdlTrigger, this, execScope);
			}
		}
		else if (serverDdlTrigger == null)
		{
			definition.Name.ReportBindError(SR.InvalidObjectName(definition.Name.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			base.BindingState |= BindingState.BindFailed;
		}
		else
		{
			DoExecuteAlterServerTrigger(server, mutableServerDdlTrigger, this, execScope);
		}
	}

	internal override void InitiateBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		switch (definition.TriggerType)
		{
		case TriggerType.DmlTrigger:
			InitiateBindDmlTrigger(bindingContext, bindMode);
			break;
		case TriggerType.DdlTrigger:
			switch (definition.DdlDefinitionInfo.TargetType)
			{
			case SqlDdlTriggerTargetType.Database:
				InitiateBindDatabaseDdlTrigger(bindingContext, bindMode);
				break;
			case SqlDdlTriggerTargetType.AllServer:
				InitiateBindServerDdlTrigger(bindingContext, bindMode);
				break;
			default:
				base.BindingState |= BindingState.BindFailed;
				break;
			}
			break;
		default:
			base.BindingState |= BindingState.BindFailed;
			break;
		}
	}

	internal override void FinalizeBindStatement(BatchBindingContext bindingContext, StatementBindMode bindMode)
	{
		SchemaEx schema;
		IMetadataCollection<ITabular> dmlTables;
		if (boundTable != null)
		{
			schema = bindingContext.SchemaToSchemaEx(boundTable.Schema);
			CollationInfo ordinalIgnoreCase = CollationInfo.OrdinalIgnoreCase;
			ITabularFactory tabular = bindingContext.MetadataFactory.Tabular;
			dmlTables = Collection<ITabular>.CreateOrderedCollection(ordinalIgnoreCase, tabular.CreateDmlInsertedTable(boundTable), tabular.CreateDmlDeletedTable(boundTable));
		}
		else
		{
			schema = bindingContext.ConnectedDatabase.DboSchema;
			dmlTables = Collection<ITabular>.Empty;
		}
		BatchBindingContext orCreateModuleBindingContext = bindingContext.GetOrCreateModuleBindingContext(schema, dmlTables);
		definition.Bind(orCreateModuleBindingContext, boundTrigger, boundTable, executionSuccedded);
		base.BindingState |= definition.BindingState;
		foreach (SqlStatement statement in statements)
		{
			statement.Bind(orCreateModuleBindingContext);
			base.BindingState |= statement.BindingState;
		}
		if (bindMode == StatementBindMode.OutOfOrder)
		{
			boundTrigger.AsTrigger.BodyText = SqlCodeObject.GetSqlSource(statements);
		}
	}

	internal override void Unbind()
	{
		boundTrigger = default(TriggerUnion<IMutableDmlTrigger, IMutableDatabaseDdlTrigger, IMutableServerDdlTrigger, IMutableTrigger>);
		boundTable = null;
		base.Unbind();
	}

	private static SchemaEx GetSchema(ITableViewBase targetTable, IDmlTrigger existingTrigger, ITableViewBase existingTriggerTarget, SqlCreateAlterTriggerStatementBase sqlCreateAlterTriggerStatementBase, BatchBindingContext bindingContext, LexicalScope execScope)
	{
		if (targetTable != null)
		{
			return bindingContext.SchemaToSchemaEx(targetTable.Schema);
		}
		if (existingTrigger != null)
		{
			return bindingContext.SchemaToSchemaEx(existingTriggerTarget.Schema);
		}
		IDmlTriggerDefinitionInfo dmlDefinitionInfo = sqlCreateAlterTriggerStatementBase.definition.DmlDefinitionInfo;
		SqlIdentifier sqlIdentifier = ((!dmlDefinitionInfo.TargetName.SchemaName.IsEmpty) ? dmlDefinitionInfo.TargetName.SchemaName : sqlCreateAlterTriggerStatementBase.definition.Name.SchemaName);
		return bindingContext.GetCDASchema(sqlIdentifier.Value, execScope);
	}

	private static void DoExecuteAlterDmlTrigger(ITableViewBase targetTable, ITableViewBase existingTriggerTarget, IMutableDmlTrigger trigger, SqlCreateAlterTriggerStatementBase sqlCreateAlterTriggerStatementBase, SchemaEx schema, BatchBindingContext bindingContext, LexicalScope execScope, StatementBindMode bindMode)
	{
		IDmlTriggerDefinitionInfo dmlDefinitionInfo = sqlCreateAlterTriggerStatementBase.definition.DmlDefinitionInfo;
		if (targetTable == null)
		{
			dmlDefinitionInfo.TargetName.ReportBindError(SR.ObjectNotExistOrIsInvalid(dmlDefinitionInfo.TargetName.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
			sqlCreateAlterTriggerStatementBase.BindingState |= BindingState.BindFailed;
			return;
		}
		if (existingTriggerTarget != targetTable)
		{
			sqlCreateAlterTriggerStatementBase.definition.Name.ReportBindError(SR.TriggerDoesNotBelongToTarget(sqlCreateAlterTriggerStatementBase.definition.Name.ToString(), dmlDefinitionInfo.TargetName.ToString()));
			sqlCreateAlterTriggerStatementBase.BindingState |= BindingState.BindFailed;
			return;
		}
		if (sqlCreateAlterTriggerStatementBase.definition.Name.ObjectName.IsTempName)
		{
			sqlCreateAlterTriggerStatementBase.definition.Name.ReportBindError(SR.InvalidObjectName(sqlCreateAlterTriggerStatementBase.definition.Name.ToString()));
			sqlCreateAlterTriggerStatementBase.BindingState |= BindingState.BindFailed;
			return;
		}
		IMutableTableViewBase alterable = GetAlterable(schema, targetTable, execScope, bindMode);
		alterable.Triggers[trigger.Name] = trigger;
		sqlCreateAlterTriggerStatementBase.boundTable = alterable;
		sqlCreateAlterTriggerStatementBase.executionSuccedded = true;
	}

	private static void DoExecuteAlterDatabaseTrigger(DatabaseEx database, IMutableDatabaseDdlTrigger trigger, SqlCreateAlterTriggerStatementBase sqlCreateAlterTriggerStatementBase, LexicalScope execScope)
	{
		if (sqlCreateAlterTriggerStatementBase.definition.Name.ObjectName.IsTempName)
		{
			sqlCreateAlterTriggerStatementBase.definition.Name.ReportBindError(SR.InvalidObjectName(sqlCreateAlterTriggerStatementBase.definition.Name.ToString()));
			sqlCreateAlterTriggerStatementBase.BindingState |= BindingState.BindFailed;
		}
		else
		{
			database.Triggers.Alter(trigger, execScope);
		}
	}

	private static void DoExecuteAlterServerTrigger(ServerEx server, IMutableServerDdlTrigger trigger, SqlCreateAlterTriggerStatementBase sqlCreateAlterTriggerStatementBase, LexicalScope execScope)
	{
		if (sqlCreateAlterTriggerStatementBase.definition.Name.ObjectName.IsTempName)
		{
			sqlCreateAlterTriggerStatementBase.definition.Name.ReportBindError(SR.InvalidObjectName(sqlCreateAlterTriggerStatementBase.definition.Name.ToString()));
			sqlCreateAlterTriggerStatementBase.BindingState |= BindingState.BindFailed;
		}
		else
		{
			server.Triggers.Alter(trigger, execScope);
		}
	}

	internal SqlCreateAlterTriggerStatementBase(SqlTriggerDefinition definition, List<SqlStatement> statements)
	{
		this.definition = definition;
		this.statements = ((statements != null) ? new SqlStatementCollection(statements) : new SqlStatementCollection());
		isOrAlterStatement = definition.IsOrAlterStatement;
		foreach (SqlStatement statement in this.statements)
		{
			statement.Accept(ValidateModuleBodyVisitor.TriggerParserVisitor);
		}
	}
}
